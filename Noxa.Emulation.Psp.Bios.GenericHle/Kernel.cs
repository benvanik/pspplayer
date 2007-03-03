// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Noxa.Emulation.Psp.Games;
using Noxa.Emulation.Psp.Cpu;
using Noxa.Emulation.Psp.Media;
using Noxa.Emulation.Psp.Utilities;

namespace Noxa.Emulation.Psp.Bios.GenericHle
{
	class Kernel : IKernel
	{
		protected HleInstance _hle;
		protected GameInformation _game;
		protected AutoResetEvent _gameEvent = new AutoResetEvent( false );

		protected PerformanceTimer _timer = new PerformanceTimer();
		protected double _startTime;
		protected uint _startTick;
		protected DateTime _unixBaseTime = new DateTime( 1970, 1, 1, 0, 0, 0, DateTimeKind.Utc );

		protected Dictionary<int, KernelThread> _threads = new Dictionary<int, KernelThread>();
		protected KernelThread _activeThread;
		protected KernelStatus _status;

		protected KernelFileHandle _stdIn;
		protected KernelFileHandle _stdOut;
		protected KernelFileHandle _stdErr;

		protected KernelPartition[] _partitions;
		protected List<KernelDevice> _devices = new List<KernelDevice>();
		protected Dictionary<string, KernelDevice> _deviceMap = new Dictionary<string, KernelDevice>();
		protected IMediaFolder _currentPath;

		protected Dictionary<int, KernelHandle> _handles = new Dictionary<int, KernelHandle>();
		protected Dictionary<KernelCallbackName, KernelCallback> _callbacks = new Dictionary<KernelCallbackName, KernelCallback>();
		protected List<KernelThread> _threadsWaitingOnEvents = new List<KernelThread>();

		protected int _lastUid = 0;

		public Kernel( HleInstance hle )
		{
			Debug.Assert( hle != null );

			_hle = hle;

			// Took out MSB because addresses come translated
			_partitions = new KernelPartition[]{
				new KernelPartition( this, 1, 0x08000000, 0x00300000 ), /* Kernel 1 0x8*/
				new KernelPartition( this, 2, 0x08000000, 0x01800000 ), /* User */
				new KernelPartition( this, 3, 0x08000000, 0x00300000 ), /* Kernel 1 */
				new KernelPartition( this, 4, 0x08300000, 0x00100000 ), /* Kernel 2 0x8 */
				new KernelPartition( this, 5, 0x08400000, 0x00400000 ), /* Kernel 3 0x8 */
				new KernelPartition( this, 6, 0x08800000, 0x01800000 ), /* User */
			};

			_status = new KernelStatus();
		}

		public GameInformation Game
		{
			get
			{
				return _game;
			}
			set
			{
				if( _game != null )
				{
					// Shouldn't happen?
				}
				_game = value;
				this.StartGame();
			}
		}

		public KernelThread[] Threads
		{
			get
			{
				KernelThread[] threads = new KernelThread[ _threads.Count ];
				int n = 0;
				foreach( KernelThread thread in _threads.Values )
				{
					threads[ n ] = thread;
					n++;
				}
				return threads;
			}
		}

		public KernelThread ActiveThread
		{
			get
			{
				return _activeThread;
			}
		}

		public KernelPartition[] Partitions
		{
			get
			{
				return _partitions;
			}
		}

		public KernelStatus Status
		{
			get
			{
				return _status;
			}
		}

		public KernelHandle FindHandle( int uid )
		{
			if( _handles.ContainsKey( uid ) == true )
				return _handles[ uid ];
			return null;
		}

		#region Threading

		public KernelThread FindThread( int uid )
		{
			if( _threads.ContainsKey( uid ) == true )
				return _threads[ uid ];
			return null;
		}

		public void CreateThread( KernelThread thread )
		{
			_threads.Add( thread.Uid, thread );
		}

		public void DeleteThread( KernelThread thread )
		{
			_threads.Remove( thread.Uid );

			if( _activeThread == thread )
			{
				Debug.WriteLine( "Kernel::DeleteThread called on the active thread" );
				_activeThread = null;
			}
		}

		public void WaitThreadOnEvent( KernelThread thread, KernelEvent ev, int bitMask, int outAddress )
		{
			thread.Wait( ev, bitMask, outAddress );
			thread.CanHandleCallbacks = true;
			_threadsWaitingOnEvents.Add( thread );

			this.ContextSwitch();
		}

		public void SignalEvent( KernelEvent ev )
		{
			bool needsSwitch = false;
			for( int n = 0; n < _threadsWaitingOnEvents.Count; n++ )
			{
				KernelThread thread = _threadsWaitingOnEvents[ n ];
				if( thread.WaitEvent == ev )
				{
					// Check for a match somehow
					if( ( thread.WaitId & ev.BitMask ) != 0 )
					{
						thread.State = KernelThreadState.Ready;

						// Need to obey output param
						if( thread.OutAddress != 0x0 )
							_hle.Emulator.Cpu.Memory.WriteWord( thread.OutAddress, 4, ev.BitMask );
						
						needsSwitch = true;
					}
				}
			}
			if( needsSwitch == true )
				this.ContextSwitch();
		}

		private int ThreadPriorityComparer( KernelThread a, KernelThread b )
		{
			if( a.Priority < b.Priority )
				return -1;
			else if( a.Priority > b.Priority )
				return 1;
			else
				return 0;
		}

		public void ContextSwitch()
		{
			if( _threads.Count == 0 )
			{
				_activeThread = null;
				return;
			}

			List<KernelThread> threads = new List<KernelThread>( _threads.Count );
			threads.AddRange( _threads.Values );
			threads.Sort( new Comparison<KernelThread>( ThreadPriorityComparer ) );

			// Find the first thread (lowest priority) that is runnable
			// While doing this, also check to see if we can wake threads up
			KernelThread newThread = null;
			for( int n = 0; n < threads.Count; n++ )
			{
				if( ( threads[ n ].State == KernelThreadState.Waiting ) ||
					( threads[ n ].State == KernelThreadState.Suspended ) ||
					( threads[ n ].State == KernelThreadState.Stopped ) ||
					( threads[ n ].State == KernelThreadState.Killed ) )
				{
					if( threads[ n ].State == KernelThreadState.Waiting )
					{
						switch( threads[ n ].WaitType )
						{
							case KernelThreadWait.Sleep:
								// Cannot un-wait automatically
								break;
							case KernelThreadWait.Delay:
								// Time
								break;
							case KernelThreadWait.ThreadEnd:
								// Check thread
								if( _threads.ContainsKey( threads[ n ].WaitId ) == true )
								{
									KernelThread waitThread = _threads[ threads[ n ].WaitId ];
									if( ( waitThread.State == KernelThreadState.Killed ) ||
										( waitThread.State == KernelThreadState.Stopped ) )
									{
										// Thread ended, wake
										threads[ n ].State = KernelThreadState.Ready;
									}
								}
								else
								{
									// Thread died, wake
									threads[ n ].State = KernelThreadState.Ready;
								}
								break;
							case KernelThreadWait.Event:
								// Waiting on WaitEvent for bitmask in WaitId
								break;
						}
						// If we didn't wake, skip
						if( threads[ n ].State == KernelThreadState.Waiting )
							continue;
					}
					else
						continue;
				}

				// So that we only get the first (lowest pri) one
				if( newThread == null )
					newThread = threads[ n ];
				//break;
			}

			if( newThread == null )
			{
				// No threads to run
				_activeThread = null;
				Debug.WriteLine( "Kernel: ran out of threads to run, ending sim" );
				_hle.Emulator.Stop();
				return;
			}

			// Switch
			if( _activeThread != newThread )
			{
				ICpuCore core = _hle.Emulator.Cpu.Cores[ 0 ];

				if( _activeThread != null )
				{
					// Save context
					if( _activeThread.Context == null )
						_activeThread.Context = new KernelThreadContext();
					_activeThread.Context.CoreState = core.Context;

					// Thread may have stopped/gone to sleep
					if( _activeThread.State == KernelThreadState.Running )
						_activeThread.State = KernelThreadState.Ready;
				}

				_activeThread = newThread;

				Debug.WriteLine( string.Format( "Kernel: context switch, new pc={0:X8}", _activeThread.EntryAddress ) );

				if( _activeThread.Context != null )
				{
					// Restore context
					core.Context = _activeThread.Context.CoreState;
				}
				else
				{
					// Start of thread
					// Set entry address and argc / argv
					core.ProgramCounter = _activeThread.EntryAddress; // PC is weird - interpreted needs -4!
					core.SetGeneralRegister( 29, ( int )_activeThread.StackBlock.Address );
					core.SetGeneralRegister( 4, _activeThread.ArgumentsLength );
					core.SetGeneralRegister( 5, _activeThread.ArgumentsPointer );
				}

				_activeThread.State = KernelThreadState.Running;

				_status.ThreadSwitchCount++;
			}
		}

		#endregion

		#region Control

		protected void StartGame()
		{
			_timer.Reset();
			_startTime = 0.0;
			_startTick = ( uint )Environment.TickCount;

			_deviceMap.Clear();
			_devices.Clear();

			// We do this here because we want to make sure the file systems are setup
			IMediaDevice msDevice = _hle.Emulator.MemoryStick;
			IMediaDevice umdDevice = _hle.Emulator.Umd;
			_devices.Add( new KernelFileDevice( "MemoryStick", new string[] { "fatms0", "ms0", "fatms" }, true, ( msDevice != null ) ? msDevice.IsReadOnly : true, msDevice, ( msDevice != null ) ? msDevice.Root : null ) );
			_devices.Add( new KernelFileDevice( "UMD", new string[] { "umd0", "isofs", "isofs0", "disc0" }, true, true, umdDevice, ( umdDevice != null ) ? umdDevice.Root : null ) );
			//_devices.Add( new KernelFileDevice( "flash0", new string[] { "flash0", "flashfat", "flashfat0" }, true, false, null, null ) );
			//_devices.Add( new KernelFileDevice( "flash1", new string[] { "flash1", "flashfat1" }, true, false, null, null ) );

			foreach( KernelDevice device in _devices )
			{
				for( int n = 0; n < device.Paths.Length; n++ )
					_deviceMap.Add( device.Paths[ n ], device );
			}

			_currentPath = _game.Folder;

			_hle.ClearModules();
			_hle.StartModules();

			_gameEvent.Set();
		}

		public void ExitGame()
		{
			_hle.StopModules();

			_deviceMap.Clear();
			_devices.Clear();

			_game = null;
			_gameEvent.Set();
		}

		#endregion

		#region Handles/stdio/etc

		public void AddHandle( KernelHandle handle )
		{
			_handles.Add( handle.Uid, handle );
		}

		public KernelHandle GetHandle( int uid )
		{
			if( _handles.ContainsKey( uid ) == true )
				return _handles[ uid ];
			else
				return null;
		}

		public void RemoveHandle( KernelHandle handle )
		{
			Debug.Assert( handle != null );
			if( _handles.ContainsKey( handle.Uid ) == true )
				_handles.Remove( handle.Uid );
		}

		private void CreateStdio()
		{
			_stdIn = new KernelFileHandle();
			_stdIn.HandleType = KernelHandleType.Stdio;
			_stdIn.CanWrite = false;
			_stdIn.CanSeek = false;
			_stdIn.IsOpen = true;
			_stdIn.Stream = new MemoryStream();
			_stdIn.Uid = this.AllocateUid();
			this.AddHandle( _stdIn );

			_stdOut = new KernelFileHandle();
			_stdOut.HandleType = KernelHandleType.Stdio;
			_stdOut.CanWrite = true;
			_stdOut.CanSeek = false;
			_stdOut.IsOpen = true;
			_stdOut.Stream = new MemoryStream();
			_stdOut.Uid = this.AllocateUid();
			this.AddHandle( _stdOut );

			_stdErr = new KernelFileHandle();
			_stdErr.HandleType = KernelHandleType.Stdio;
			_stdErr.CanWrite = true;
			_stdErr.CanSeek = false;
			_stdErr.IsOpen = true;
			_stdErr.Stream = new MemoryStream();
			_stdErr.Uid = this.AllocateUid();
			this.AddHandle( _stdErr );
		}

		public KernelFileHandle StdIn
		{
			get
			{
				return _stdIn;
			}
		}

		public KernelFileHandle StdOut
		{
			get
			{
				return _stdOut;
			}
		}

		public KernelFileHandle StdErr
		{
			get
			{
				return _stdErr;
			}
		}

		public KernelDevice FindDevice( string path )
		{
			int colon = path.IndexOf( ':' );
			if( colon < 0 )
			{
				IMediaDevice device = _currentPath.Device;
				for( int n = 0; n < _devices.Count; n++ )
				{
					KernelFileDevice fileDevice = _devices[ n ] as KernelFileDevice;
					if( fileDevice != null )
					{
						if( fileDevice.MediaDevice == device )
							return fileDevice;
					}
					else
					{
						KernelBlockDevice blockDevice = _devices[ n ] as KernelBlockDevice;
						// TODO: block device lookup?
					}
				}
			}
			else
			{
				string dev = path.Substring( 0, colon ).ToLowerInvariant();
				if( _deviceMap.ContainsKey( dev ) == true )
					return _deviceMap[ dev ];
			}
			return null;
		}

		public IMediaFolder CurrentPath
		{
			get
			{
				return _currentPath;
			}
			set
			{
				_currentPath = value;
			}
		}

		#endregion

		#region Callbacks/events

		public Dictionary<KernelCallbackName, KernelCallback> Callbacks
		{
			get
			{
				return _callbacks;
			}
		}

		#endregion

		#region Timing

		/// <summary>
		/// Unix time since 1970-01-01 UTC (not accurate) in microseconds.
		/// </summary>
		public uint ClockTime
		{
			get
			{
				// 1000000 us per second
				// 10000000 ticks per second
				TimeSpan elapsed = DateTime.UtcNow - _unixBaseTime;
				return ( uint )( elapsed.Ticks / 10 );
			}
		}

		/// <summary>
		/// The tick at which the game was started.
		/// </summary>
		public uint StartTick
		{
			get
			{
				return _startTick;
			}
		}

		/// <summary>
		/// Accurate time since game start in seconds.
		/// </summary>
		public double RunTime
		{
			get
			{
				return _timer.Elapsed - _startTime;
			}
		}

		#endregion

		public int AllocateUid()
		{
			int uid = _lastUid;
			_lastUid++;
			return uid;
		}

		public void Execute()
		{
			ICpu cpu = _hle.Emulator.Cpu;
			ICpuCore core0 = cpu.Cores[ 0 ];

			if( _activeThread != null )
			{
				// Execute active thread
				int instructionCount = cpu.ExecuteBlock();
			}
			else
			{
				// Load game and run until it creates it's first thread

				if( _game == null )
				{
					_gameEvent.WaitOne();
				}

				// Clear everything
				_hle.Emulator.LightReset();

				GameLoader g = new GameLoader();
				uint lowerBounds;
				uint upperBounds;
				uint entryAddress;
				if( g.LoadBoot( _game, _hle.Emulator, out lowerBounds, out upperBounds, out entryAddress ) == false )
				{
					Debug.WriteLine( "Kernel: game load failed, aborting" );
					_game = null;
					return;
				}

				// Have to allocate the stuff taken by the elf
				_partitions[ 1 ].Allocate( KernelAllocationType.SpecificAddress, lowerBounds, upperBounds - lowerBounds );

				this.CreateStdio();

				int preThreadCount = 0;
				while( _activeThread == null )
				{
					preThreadCount += cpu.ExecuteBlock();
				}

				Debug.WriteLine( string.Format( "Kernel: first thread created, executed {0} instructions", preThreadCount ) );
			}
		}

		#region Helpers

		public static string ReadString( IMemory memory, int address )
		{
			StringBuilder sb = new StringBuilder();
			while( true )
			{
				byte c = ( byte )( memory.ReadWord( address ) & 0xFF );
				if( c == 0 )
					break;
				sb.Append( ( char )c );
				address++;
			}
			return sb.ToString();
		}

		public static int WriteString( IMemory memory, int address, string value )
		{
			byte[] bytes = Encoding.ASCII.GetBytes( value );
			memory.WriteBytes( address, bytes );
			memory.WriteWord( address + bytes.Length, 1, 0 );
			return bytes.Length + 1;
		}

		//unsigned short	year 
		//unsigned short	month 
		//unsigned short	day 
		//unsigned short	hour 
		//unsigned short	minute 
		//unsigned short	second 
		//unsigned int		microsecond

		public static DateTime ReadTime( IMemory memory, int address )
		{
			ushort year = ( ushort )( memory.ReadWord( address ) & 0xFFFF );
			ushort month = ( ushort )( memory.ReadWord( address + 2 ) & 0xFFFF );
			ushort day = ( ushort )( memory.ReadWord( address + 4 ) & 0xFFFF );
			ushort hour = ( ushort )( memory.ReadWord( address + 6 ) & 0xFFFF );
			ushort minute = ( ushort )( memory.ReadWord( address + 8 ) & 0xFFFF );
			ushort second = ( ushort )( memory.ReadWord( address + 10 ) & 0xFFFF );
			uint microsecond = ( uint )( memory.ReadWord( address + 12 ) );

			// 1000 microseconds per millisecond?
			return new DateTime( year, month, day, hour, minute, second, ( int )( microsecond / 1000 ) );
		}

		public static int WriteTime( IMemory memory, int address, DateTime time )
		{
			memory.WriteWord( address, 2, time.Year );
			memory.WriteWord( address + 2, 2, time.Month );
			memory.WriteWord( address + 4, 2, time.Day );
			memory.WriteWord( address + 6, 2, time.Hour );
			memory.WriteWord( address + 8, 2, time.Minute );
			memory.WriteWord( address + 10, 2, time.Second );
			memory.WriteWord( address + 12, 4, time.Millisecond * 1000 );

			return 16;
		}

		#endregion
	}
}
