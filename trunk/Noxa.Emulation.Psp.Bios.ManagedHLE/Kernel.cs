// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading;
using ComTypes = System.Runtime.InteropServices.ComTypes;

using Noxa.Emulation.Psp;
using Noxa.Emulation.Psp.Bios;
using Noxa.Emulation.Psp.Cpu;
using Noxa.Emulation.Psp.Games;
using Noxa.Emulation.Psp.Media;

namespace Noxa.Emulation.Psp.Bios.ManagedHLE
{
	partial class Kernel : IDisposable
	{
		public Bios Bios;
		public IEmulationInstance Emulator;
		public ICpu Cpu;
		public ICpuCore CpuCore;
		public IMemory Memory;
		public MemorySystem MemorySystem;

		public List<KModule> UserModules;
		public KModule MainModule;

		public Dictionary<uint, KHandle> Handles;
		public KPartition[] Partitions;
		public List<KThread> Threads;
		public FastLinkedList<KThread> SchedulableThreads;
		public KThread ActiveThread;

		public KDevice[] Devices;
		public Dictionary<string, KDevice> DeviceLookup;
		public IMediaFolder CurrentPath;
		public KStdFile StdIn;
		public KStdFile StdOut;
		public KStdFile StdErr;

		public KIntHandler[][] Interrupts;
		public List<FastLinkedList<KCallback>> Callbacks;
		public static class CallbackTypes
		{
			public const int Exit = 0;		// Issued on 'home' press
			public const int Umd = 1;		// Issed on UMD state change
			
			public const int CallbackCount = 2;
		}

		public long StartTick;
		public long TickFrequency;

		private int _lastUid;

		public Kernel( Bios bios )
		{
			Debug.Assert( bios != null );
			Bios = bios;
			Emulator = Bios.Emulator;

			Debug.Assert( Emulator.Cpu != null );
			Cpu = Emulator.Cpu;
			CpuCore = Cpu.Cores[ 0 ];
			Memory = Cpu.Memory;
			MemorySystem = Memory.MemorySystem;

			UserModules = new List<KModule>( 10 );

			Threads = new List<KThread>( 128 );
			SchedulableThreads = new FastLinkedList<KThread>();

			Handles = new Dictionary<uint, KHandle>();
			_lastUid = 100;
		}

		~Kernel()
		{
			this.Dispose();
		}

		public void Dispose()
		{
			GC.SuppressFinalize( this );

			if( ( Bios != null ) &&
				( Bios.Game != null ) )
				this.StopGame( 0 );
		}

		public void StartGame()
		{
			Debug.Assert( Bios.Game != null );
			if( Bios.Game == null )
				return;

			Partitions = new KPartition[]{
				new KPartition( this, 0x00000000, 0x00000000 ), // dummy
				new KPartition( this, 0x08000000, 0x00300000 ), // kernel 1 (0x8...)
				new KPartition( this, 0x08000000, 0x01800000 ), // user
				new KPartition( this, 0x08000000, 0x00300000 ), // kernel 1
				new KPartition( this, 0x08300000, 0x00100000 ), // kernel 2 (0x8...)
				new KPartition( this, 0x08400000, 0x00400000 ), // kernel 3 (0x8...)
				new KPartition( this, 0x08800000, 0x01800000 ), // user
			};

			Devices = new KDevice[]{
				new KDevice( this, "ms0", new string[]{ "ms0", "fatms0", "ms", "fatms" }, Emulator.MemoryStick ),
				new KDevice( this, "umd0", new string[]{ "umd0", "disc0", "umd", "isofs", "isofs0" }, Emulator.Umd ),
			};
			DeviceLookup = new Dictionary<string, KDevice>();
			foreach( KDevice device in Devices )
			{
				foreach( string alias in device.Aliases )
					DeviceLookup.Add( alias, device );
			}

			Interrupts = new KIntHandler[ 67 ][];
			for( int n = 0; n < Interrupts.Length; n++ )
				Interrupts[ n ] = new KIntHandler[ 16 ];

			Callbacks = new List<FastLinkedList<KCallback>>( CallbackTypes.CallbackCount );
			for( int n = 0; n < CallbackTypes.CallbackCount; n++ )
				Callbacks.Add( new FastLinkedList<KCallback>() );

			// Timing... not sure if this is right
			NativeMethods.QueryPerformanceCounter( out StartTick );
			NativeMethods.QueryPerformanceFrequency( out TickFrequency );

			this.CreateStdio();
			this.CreateTimerQueue();
		}

		public void StopGame( int exitCode )
		{
			Cpu.Stop();
			foreach( KThread thread in this.Threads )
				thread.Exit( 0 );

			this.DestroyTimerQueue();

			for( int n = 0; n < Interrupts.Length; n++ )
			{
				for( int m = 0; m < Interrupts[ n ].Length; m++ )
				{
					KIntHandler handler = Interrupts[ n ][ m ];
					if( handler != null )
					{
						handler.Dispose();
						Interrupts[ n ][ m ] = null;
					}
				}
			}

			foreach( Module module in Bios._modules )
				module.Stop();

			Bios.Game = null;
			Bios.BootStream = null;
		}

		public LoadResults LoadGame()
		{
			// Clear everything (needed?)
			Emulator.LightReset();

			// Get bootstream
			Debug.Assert( Bios.BootStream == null );
			Bios.BootStream = GameLoader.FindBootStream( Bios.Game );
			Debug.Assert( Bios.BootStream != null );

			LoadParameters loadParams = new LoadParameters();
			loadParams.Path = Bios.Game.Folder;
			LoadResults results = Bios.Loader.LoadModule( ModuleType.Boot, Bios.BootStream, loadParams );

			Debug.Assert( results.Successful == true );
			if( results.Successful == false )
			{
				Log.WriteLine( Verbosity.Critical, Feature.Bios, "load of game failed" );
				Bios.Game = null;
				return results;
			}

			this.CurrentPath = Bios.Game.Folder;
			this.Cpu.SetupGame( Bios.Game, Bios.BootStream );

			// Start modules
			foreach( Module module in Bios._modules )
				module.Start();

			Log.WriteLine( Verbosity.Normal, Feature.Bios, "game loaded" );

			return results;
		}

		public KHandle AddHandle( KHandle handle )
		{
			Debug.Assert( handle != null );
			handle.UID = ( uint )Interlocked.Increment( ref _lastUid ) - 1;
			Handles.Add( handle.UID, handle );
			return handle;
		}

		public KHandle GetHandle( uint uid )
		{
			KHandle handle;
			if( Handles.TryGetValue( uid, out handle ) == true )
				return handle;
			else
				return null;
		}

		public T GetHandle<T>( uint uid ) where T : KHandle
		{
			KHandle handle;
			if( Handles.TryGetValue( uid, out handle ) == true )
				return ( T )handle;
			else
				return null;
		}

		public T GetHandle<T>( int uid ) where T : KHandle
		{
			KHandle handle;
			if( Handles.TryGetValue( ( uint )uid, out handle ) == true )
				return ( T )handle;
			else
				return null;
		}

		public void RemoveHandle( uint uid )
		{
			Handles.Remove( uid );
		}

		public void RemoveHandle( int uid )
		{
			Handles.Remove( ( uint )uid );
		}

		/// <summary>
		/// Unix time since 1970-01-01 UTC (not accurate) in microseconds.
		/// </summary>
		public long GetClockTime()
		{
			ComTypes.FILETIME ft;
			NativeMethods.GetSystemTimeAsFileTime( out ft );
			ulong ftl = ( uint )ft.dwLowDateTime | ( ( ulong )( uint )ft.dwHighDateTime << 32 );
			return 11644473600 + ( long )ftl;
		}

		/// <summary>
		/// Time in microseconds since the game started.
		/// </summary>
		public long GetRunTime()
		{
			long tick;
			NativeMethods.QueryPerformanceCounter( out tick );
			return ( tick - StartTick ) / TickFrequency;
		}

		private void CreateStdio()
		{
			StdIn = new KStdFile( this, KSpecialFileHandle.StdIn );
			StdOut = new KStdFile( this, KSpecialFileHandle.StdOut );
			StdErr = new KStdFile( this, KSpecialFileHandle.StdErr );

			this.Handles.Add( StdIn.UID, StdIn );
			this.Handles.Add( StdOut.UID, StdOut );
			this.Handles.Add( StdErr.UID, StdErr );
		}
	}
}
