// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using Noxa.Emulation.Psp;
using Noxa.Emulation.Psp.Bios;
using Noxa.Emulation.Psp.Cpu;
using Noxa.Emulation.Psp.Media;
using Noxa.Utilities;

namespace Noxa.Emulation.Psp.Bios.ManagedHLE.Modules
{
	class ModuleMgrForUser : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "ModuleMgrForUser";
			}
		}

		#endregion

		#region State Management

		public ModuleMgrForUser( Kernel kernel )
			: base( kernel )
		{
		}

		public override void Start()
		{
		}

		public override void Stop()
		{
		}

		#endregion

		public const int FakeModuleUID = 3;
		public const int ModuleAlreadyLoaded = unchecked( ( int )0x80020139 );

		#region Module Loading

		/// <summary>
		/// List of module prxs to blacklist.
		/// </summary>
		private static readonly string[] _moduleBlacklist = new string[]
		{
			"libatrac3plus.prx",
			"sc_sascore.prx",
			"audiocodec.prx",
			"videocodec.prx",
			"mpegbase.prx",
			"mpeg.prx",
		};

		private int LoadModule( IMediaFile file, Stream stream, int flags, int option )
		{
			// Load!
			LoadParameters loadParams = new LoadParameters();
			if( file != null )
			{
				loadParams.Path = file.Parent;
				string fileName = file.Name.ToLowerInvariant();
				foreach( string blacklisted in _moduleBlacklist )
				{
					if( fileName == blacklisted )
					{
						// Module is blacklisted - ignore
						Log.WriteLine( Verbosity.Normal, Feature.Bios, "LoadModule: module is blacklisted, ignoring" );
						return FakeModuleUID;
					}
				}
			}
#if DEBUG
			loadParams.AppendDatabase = true;
#endif

			// Check to see if it's one we have a decoded version of
			string kernelLocation = Path.GetDirectoryName( Assembly.GetExecutingAssembly().Location );
			string prxLocation = Path.Combine( kernelLocation, "PRX" );
			string lookasidePrx = Path.Combine( prxLocation, file.Name );
			if( File.Exists( lookasidePrx ) == true )
			{
				// Load ours instead
				Log.WriteLine( Verbosity.Normal, Feature.Bios, "LoadModule: lookaside prx found at {0}", lookasidePrx );
				stream = File.OpenRead( lookasidePrx );
			}

			Debug.Assert( stream != null );
			if( stream == null )
			{
				Log.WriteLine( Verbosity.Critical, Feature.Bios, "LoadModule: unable to load module {0}", file.Name );
				return -1;
			}

			// Quick check to make sure it isn't encrypted before sending off to loader
			byte[] magicBytes = new byte[ 4 ];
			stream.Read( magicBytes, 0, 4 );
			stream.Seek( -4, SeekOrigin.Current );
			// 0x7E, 0x50, 0x53, 0x50 = ~PSP
			bool encrypted = (
				( magicBytes[ 0 ] == 0x7E ) &&
				( magicBytes[ 1 ] == 0x50 ) &&
				( magicBytes[ 2 ] == 0x53 ) &&
				( magicBytes[ 3 ] == 0x50 ) );
			//Debug::Assert( encrypted == false );
			if( encrypted == true )
			{
				Log.WriteLine( Verbosity.Critical, Feature.Bios, "LoadModule: module {0} is encrypted - unable to load", file.Name );

				// We spoof the caller in to thinking we worked right... by just returning 0 ^_^
				KModule fakemod = new KModule( _kernel, null );
				_kernel.AddHandle( fakemod );
				return ( int )fakemod.UID;
			}

			LoadResults results = _kernel.Bios._loader.LoadModule( ModuleType.Prx, stream, loadParams );
			//Debug.Assert( results.Successful == true );
			if( results.Successful == false )
			{
				Log.WriteLine( Verbosity.Critical, Feature.Bios, "LoadModule: loader failed" );
				return FakeModuleUID;
			}
			if( results.Ignored == true )
			{
				// Faked
				Log.WriteLine( Verbosity.Normal, Feature.Bios, "LoadModule: ignoring module" );
				return FakeModuleUID;
			}

			// Create a local representation of the module
			BiosModule module = new BiosModule( results.Name, results.Exports.ToArray() );
			_kernel.Bios._metaModules.Add( module );
			_kernel.Bios._metaModuleLookup.Add( module.Name, module );

			KModule kmod = new KModule( _kernel, module );
			kmod.LoadResults = results;
			_kernel.AddHandle( kmod );

			return ( int )kmod.UID;
		}

		#endregion

		#region Module Start/Stop/Etc

		private void CallStartModule( KModule module, int args, int argp )
		{
			if( module.ModuleStart == 0 )
			{
				// Probably a fake load
				Log.WriteLine( Verbosity.Normal, Feature.Bios, "ModuleMgrForUser: not starting module {0} - no ModuleStart defined - probably faked", module.Name );
				return;
			}

			// Create a thread
			KThread thread = new KThread( _kernel,
				module,
				_kernel.Partitions[ 6 ],
				"module_start_thread",
				module.ModuleStart,
				0,
				KThreadAttributes.User,
				0x4000 );
			_kernel.AddHandle( thread );
			thread.Start( ( uint )args, ( uint )argp );

			// Setup handler so that we get the callback when the thread ends and we can kill it
			_kernel.Cpu.SetContextSafetyCallback( thread.ContextID, new ContextSafetyDelegate( this.KmoduleStartThreadEnd ), ( int )thread.UID );

			Log.WriteLine( Verbosity.Verbose, Feature.Bios, "ModuleMgrForUser: starting module_start thread with UID {0} for module {1}", thread.UID, module.Name );

			// Schedule so that our thread runs
			_kernel.Schedule();
		}

		private void KmoduleStartThreadEnd( int tcsId, int state )
		{
			// Our thread ended - kill it!
			KThread thread = _kernel.GetHandle<KThread>( ( uint )state );
			Debug.Assert( thread != null );
			if( thread != null )
			{
				Log.WriteLine( Verbosity.Verbose, Feature.Bios, "ModuleMgrForUser: killing module_start thread with UID {0}", thread.UID );

				thread.Exit( 0 );
				_kernel.RemoveHandle( thread.UID );

				_kernel.Schedule();

				thread.Delete();
			}
		}

		private void CallStopModule( KModule module, int args, int argp )
		{
			if( module.ModuleStop == 0 )
				return;

			// Create a thread
			KThread thread = new KThread( _kernel,
				module,
				_kernel.Partitions[ 6 ],
				"module_stop_thread",
				module.ModuleStop,
				0,
				KThreadAttributes.User,
				0x4000 );
			_kernel.AddHandle( thread );
			thread.Start( ( uint )args, ( uint )argp );

			// Setup handler so that we get the callback when the thread ends and we can kill it
			_kernel.Cpu.SetContextSafetyCallback( thread.ContextID, new ContextSafetyDelegate( this.KmoduleStopThreadEnd ), ( int )thread.UID );

			Log.WriteLine( Verbosity.Verbose, Feature.Bios, "ModuleMgrForUser: starting module_stop thread with UID {0} for module {1}", thread.UID, module.Name );

			// Schedule so that our thread runs
			_kernel.Schedule();
		}

		private void KmoduleStopThreadEnd( int tcsId, int state )
		{
			// Our thread ended - kill it!
			KThread thread = _kernel.GetHandle<KThread>( ( uint )state );
			Debug.Assert( thread != null );
			if( thread != null )
			{
				Log.WriteLine( Verbosity.Verbose, Feature.Bios, "ModuleMgrForUser: killing module_stop thread with UID {0}", thread.UID );

				thread.Exit( 0 );
				_kernel.RemoveHandle( thread.UID );

				_kernel.Schedule();

				thread.Delete();
			}
		}

		#endregion

		[Stateless]
		[BiosFunction( 0xB7F46618, "sceKernelLoadModuleByID" )]
		// SDK location: /user/pspmodulemgr.h:91
		// SDK declaration: SceUID sceKernelLoadModuleByID(SceUID fid, int flags, SceKernelLMOption *option);
		public int sceKernelLoadModuleByID( int fid, int flags, int option )
		{
			KFile handle = _kernel.GetHandle<KFile>( fid );
			if( handle == null )
			{
				Log.WriteLine( Verbosity.Normal, Feature.Bios, "sceKernelLoadModuleByID: file handle {0} not found", fid );
				return -1;
			}

			Debug.Assert( handle.IsOpen == true );

			if( handle.Item != null )
				Log.WriteLine( Verbosity.Normal, Feature.Bios, "sceKernelLoadModuleByID: loading module with file handle {0} (source: {1})", fid, handle.Item.AbsolutePath );
			else
				Log.WriteLine( Verbosity.Normal, Feature.Bios, "sceKernelLoadModuleByID: loading module with file handle {0}", fid );

			return this.LoadModule( ( IMediaFile )handle.Item, handle.Stream, flags, option );
		}

		[Stateless]
		[BiosFunction( 0x977DE386, "sceKernelLoadModule" )]
		// SDK location: /user/pspmodulemgr.h:68
		// SDK declaration: SceUID sceKernelLoadModule(const char *path, int flags, SceKernelLMOption *option);
		public int sceKernelLoadModule( int path, int flags, int option )
		{
			string modulePath = _kernel.ReadString( ( uint )path );
			IMediaFile file = ( IMediaFile )_kernel.FindPath( modulePath );
			if( file == null )
			{
				Log.WriteLine( Verbosity.Normal, Feature.Bios, "sceKernelLoadModule: module not found: {0}", modulePath );
				return -1;
			}

			Log.WriteLine( Verbosity.Normal, Feature.Bios, "sceKernelLoadModule: loading module {0}", modulePath );

			Stream stream = file.OpenRead();
			return this.LoadModule( file, stream, flags, option );
		}

		[Stateless]
		[BiosFunction( 0x710F61B5, "sceKernelLoadModuleMs" )]
		// SDK location: /user/pspmodulemgr.h:80
		// SDK declaration: SceUID sceKernelLoadModuleMs(const char *path, int flags, SceKernelLMOption *option);
		public int sceKernelLoadModuleMs( int path, int flags, int option )
		{
			return this.sceKernelLoadModule( path, flags, option );
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF9275D98, "sceKernelLoadModuleBufferUsbWlan" )]
		// SDK location: /user/pspmodulemgr.h:106
		// SDK declaration: SceUID sceKernelLoadModuleBufferUsbWlan(SceSize bufsize, void *buf, int flags, SceKernelLMOption *option);
		public int sceKernelLoadModuleBufferUsbWlan( int bufsize, int buf, int flags, int option ) { return Module.NotImplementedReturn; }

		[Stateless]
		[BiosFunction( 0x50F0C1EC, "sceKernelStartModule" )]
		// SDK location: /user/pspmodulemgr.h:119
		// SDK declaration: int sceKernelStartModule(SceUID modid, SceSize argsize, void *argp, int *status, SceKernelSMOption *option);
		public int sceKernelStartModule( int modid, int argsize, int argp, int status, int option )
		{
			// argsize & argp are passed to module_stop
			// status is the return from module_stop
			// option is unused right now

			if( modid == FakeModuleUID )
			{
				if( status != 0 )
				{
					unsafe
					{
						uint* pstatus = ( uint* )_memorySystem.Translate( ( uint )status );
						*pstatus = 0;
					}
				}
				return 0;
			}

			KModule module = _kernel.GetHandle<KModule>( modid );
			Debug.Assert( module != null );
			if( module == null )
				return -1;

			// Don't current support getting result - just set to 0?
			//Debug.Assert( status == 0 );
			if( status != 0 )
			{
				unsafe
				{
					uint* pstatus = ( uint* )_memorySystem.Translate( ( uint )status );
					*pstatus = 0;
				}
			}

			this.CallStartModule( module, argsize, argp );

			return 0;
		}

		private enum LoadAvModule
		{
			AvCodec = 0,
			SasCore = 1,
			Atrac3Plus = 2,
			MpegBase = 3,
		}

		[Stateless]
		[BiosFunction( 0xC629AF26, "sceUtilityLoadAvModule" )]
		// manual add - loads avcodec.prx (or audiocodec.prx)
		public int sceUtilityLoadAvModule( int module )
		{
			LoadAvModule moduleType = ( LoadAvModule )module;
			// TODO: return the ID of the module
			Log.WriteLine( Verbosity.Critical, Feature.Bios, "sceUtilityLoadAvModule: attempted to load {0} ({1}) - return code should be module ID", moduleType, module );
			return 0;
		}

		[Stateless]
		[NotImplemented]
		[BiosFunction( 0xF7D8D092, "sceUtilityUnloadAvModule" )]
		// manual add
		public int sceUtilityUnloadAvModule( int module )
		{
			return Module.NotImplementedReturn;
		}

		private enum LoadNetModule
		{
			Common = 1,
			Adhoc = 2,
			Inet = 3,
			ParseUri = 4,
			ParseHttp = 5,
			Http = 6,
			Ssl = 7,
		}

		[Stateless]
		[BiosFunction( 0x1579A159, "sceUtilityLoadNetModule" )]
		// manual add - loads one of the PSP_NET_MODULE_ prxs
		public int sceUtilityLoadNetModule( int module )
		{
			LoadNetModule moduleType = ( LoadNetModule )module;
			// TODO: return the ID of the module
			Log.WriteLine( Verbosity.Critical, Feature.Bios, "sceUtilityLoadNetModule: attempted to load {0} ({1}) - return code should be module ID", moduleType, module );
			return 0;
		}

		[Stateless]
		[NotImplemented]
		[BiosFunction( 0x64D50C56, "sceUtilityUnloadNetModule" )]
		// manual add
		public int sceUtilityUnloadNetModule( int module )
		{
			return Module.NotImplementedReturn;
		}

		[Stateless]
		[BiosFunction( 0xD1FF982A, "sceKernelStopModule" )]
		// SDK location: /user/pspmodulemgr.h:132
		// SDK declaration: int sceKernelStopModule(SceUID modid, SceSize argsize, void *argp, int *status, SceKernelSMOption *option);
		public int sceKernelStopModule( int modid, int argsize, int argp, int status, int option )
		{
			// argsize & argp are passed to module_stop
			// status is the return from module_stop
			// option is unused right now

			if( modid == FakeModuleUID )
			{
				if( status != 0 )
				{
					unsafe
					{
						uint* pstatus = ( uint* )_memorySystem.Translate( ( uint )status );
						*pstatus = 0;
					}
				}
				return 0;
			}

			KModule module = _kernel.GetHandle<KModule>( modid );
			Debug.Assert( module != null );
			if( module == null )
				return -1;

			// Don't current support getting result - just set to 0?
			Debug.Assert( status == 0 );
			if( status != 0 )
			{
				unsafe
				{
					uint* pstatus = ( uint* )_memorySystem.Translate( ( uint )status );
					*pstatus = 0;
				}
			}

			this.CallStopModule( _kernel.MainModule, argsize, argp );

			return 0;
		}

		[Stateless]
		[BiosFunction( 0x2E0911AA, "sceKernelUnloadModule" )]
		// SDK location: /user/pspmodulemgr.h:141
		// SDK declaration: int sceKernelUnloadModule(SceUID modid);
		public int sceKernelUnloadModule( int modid )
		{
			if( modid == FakeModuleUID )
				return 0;

			KModule module = _kernel.GetHandle<KModule>( modid );
			Debug.Assert( module != null );
			if( module == null )
				return -1;

			// How do we unload? ruh roh

			return 0;
		}

		[BiosFunction( 0xD675EBB8, "sceKernelSelfStopUnloadModule" )]
		// SDK location: /user/pspmodulemgr.h:152
		// SDK declaration: int sceKernelSelfStopUnloadModule(int unknown, SceSize argsize, void *argp);
		public int sceKernelSelfStopUnloadModule( int unknown, int argsize, int argp )
		{
			return this.sceKernelStopUnloadSelfModule( argsize, argp, 0, 0 );
		}

		[BiosFunction( 0xCC1D3699, "sceKernelStopUnloadSelfModule" )]
		// SDK location: /user/pspmodulemgr.h:164
		// SDK declaration: int sceKernelStopUnloadSelfModule(SceSize argsize, void *argp, int *status, SceKernelSMOption *option);
		public int sceKernelStopUnloadSelfModule( int argsize, int argp, int status, int option )
		{
			int modId = this.sceKernelGetModuleId();
			if( modId == ( int )_kernel.MainModule.UID )
			{
				// Main user module - exit game!
				_kernel.StopGame( 0 );
			}
			else
			{
				// Normal module
				if( this.sceKernelStopModule( modId, argsize, argp, status, option ) != 0 )
					return -1;
				if( this.sceKernelUnloadModule( modId ) != 0 )
					return -1;
			}
			return 0;
		}

		[Stateless]
		[BiosFunction( 0xF0A26395, "sceKernelGetModuleId" )]
		// manual add
		public int sceKernelGetModuleId()
		{
			return ( int )_kernel.ActiveThread.Module.UID;
		}

		[Stateless]
		[BiosFunction( 0xD8B73127, "sceKernelGetModuleIdByAddress" )]
		// manual add
		public int sceKernelGetModuleIdByAddress( int address )
		{
			// This is nasty - need to have a list
			foreach( KHandle handle in _kernel.Handles.Values )
			{
				if( handle is KModule )
				{
					KModule module = ( KModule )handle;
					if( module.LoadResults != null )
					{
						if( ( address >= module.LoadResults.LowerBounds ) &&
							( address < module.LoadResults.UpperBounds ) )
							return ( int )module.UID;
					}
				}
			}
			return -1;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x748CBED9, "sceKernelQueryModuleInfo" )]
		// SDK location: /user/pspmodulemgr.h:198
		// SDK declaration: int sceKernelQueryModuleInfo(SceUID modid, SceKernelModuleInfo *info);
		public int sceKernelQueryModuleInfo( int modid, int info ) { return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - 4E257708 */
