// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Noxa.Emulation.Psp.Cpu;
using System.Diagnostics;
using Noxa.Emulation.Psp.Input;
using System.Threading;

namespace Noxa.Emulation.Psp.Bios.GenericHle.Modules
{
	class Ctrl : IStatefulModule
	{
		#region IModule Members

		protected HleInstance _hle;
		protected Kernel _kernel;

		public Ctrl( HleInstance hle )
		{
			Debug.Assert( hle != null );

			_hle = hle;
			_kernel = hle.Kernel as Kernel;

			this.Clear();
		}

		public string Name
		{
			get
			{
				return "sceCtrl";
			}
		}

		#endregion

		protected enum ControlSamplingMode
		{
			DigitalOnly = 0,
			AnalogAndDigital = 1
		}

		protected class ControlSample
		{
			public uint Timestamp;
			public PadButtons Buttons;
			public int AnalogX;
			public int AnalogY;
		}

		protected int _sampleCycle;
		protected ControlSamplingMode _sampleMode;
		protected CircularList<ControlSample> _buffer;
		protected AutoResetEvent _dataPresent;
		
		protected static byte[] ReservedBytes = new byte[] { 0, 0, 0, 0, 0, 0 };
		protected const int InputPollInterval = 75;

		protected bool _threadRunning;
		protected Thread _thread;

		#region IStatefulModule Members

		public void Start()
		{
			if( _threadRunning == true )
				this.Stop();

			_threadRunning = true;
			_thread = new Thread( new ThreadStart( this.InputThread ) );
			_thread.IsBackground = true;
			_thread.Name = "Kernel Input Thread";
			_thread.Priority = ThreadPriority.Normal;
			_thread.Start();
		}

		public void Stop()
		{
			_threadRunning = false;
			if( _thread != null )
				_thread.Join();
			_thread = null;
		}

		public void Clear()
		{
			if( _threadRunning == true )
				this.Stop();

			_sampleCycle = 0;
			_sampleMode = ControlSamplingMode.AnalogAndDigital;
			_buffer = new CircularList<ControlSample>( 100 );
			_dataPresent = new AutoResetEvent( false );
		}

		protected void InputThread()
		{
			try
			{
				while( _threadRunning == true )
				{
					ControlSample sample = new ControlSample();
					sample.Timestamp = ( uint )( _kernel.RunTime * 1000 );

					IInputDevice device = _hle.Emulator.Input;
					if( device != null )
					{
						device.Poll();
						sample.Buttons |= device.Buttons;
						float max = ushort.MaxValue;
						if( sample.AnalogX == 0 )
							sample.AnalogX = ( int )( ( ( ( float )device.AnalogX / max ) + 0.5f ) * 256 );
						if( sample.AnalogY == 0 )
							sample.AnalogY = ( int )( ( ( -( float )device.AnalogY / max ) + 0.5f ) * 256 );

						lock( this )
						{
							_buffer.Add( sample );
							if( _buffer.Count == 1 )
								_dataPresent.Set();
						}
					}

					Thread.Sleep( InputPollInterval );
				}
			}
			catch( ThreadAbortException )
			{
			}
			catch( ThreadInterruptedException )
			{
			}
		}

		#endregion

		[BiosStub( 0x6a2774f3, "sceCtrlSetSamplingCycle", true, 1 )]
		public int sceCtrlSetSamplingCycle( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = int cycle

			int old = _sampleCycle;
			_sampleCycle = a0;

			// int
			return old;
		}

		[BiosStub( 0x02baad91, "sceCtrlGetSamplingCycle", true, 1 )]
		public int sceCtrlGetSamplingCycle( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = int *pcycle

			if( a0 != 0 )
				memory.WriteWord( a0, 4, _sampleCycle );

			// int
			return 0;
		}

		[BiosStub( 0x1f4011e6, "sceCtrlSetSamplingMode", true, 1 )]
		public int sceCtrlSetSamplingMode( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = int mode

			int old = ( int )_sampleMode;
			_sampleMode = ( ControlSamplingMode )a0;

			// int
			return old;
		}

		[BiosStub( 0xda6b76a1, "sceCtrlGetSamplingMode", true, 1 )]
		public int sceCtrlGetSamplingMode( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = int *pmode

			if( a0 != 0 )
				memory.WriteWord( a0, 4, ( int )_sampleMode );

			// int
			return 0;
		}

		[BiosStub( 0x3a622550, "sceCtrlPeekBufferPositive", true, 2 )]
		public int sceCtrlPeekBufferPositive( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceCtrlData *pad_data
			// a1 = int count

			List<ControlSample> samples = new List<ControlSample>( a1 );

			int toRead;
			lock( this )
			{
				toRead = Math.Min( a1, _buffer.Count );
				// TODO: fix up to do more than just 1
				for( int n = 0; n < toRead; n++ )
					samples.Add( _buffer.Peek() );
			}

			if( a0 != 0 )
			{
				int addr = a0;
				for( int n = 0; n < samples.Count; n++ )
				{
					ControlSample sample = samples[ n ];
					memory.WriteWord( addr + 0, 4, ( int )sample.Timestamp );
					memory.WriteWord( addr + 4, 4, ( int )sample.Buttons );
					memory.WriteWord( addr + 8, 4, sample.AnalogX );
					memory.WriteWord( addr + 12, 4, sample.AnalogY );
					//memory.WriteBytes( addr + 16, ReservedBytes );
					addr += 22;
				}
			}

			// int
			return toRead;
		}

		[BiosStub( 0xc152080a, "sceCtrlPeekBufferNegative", true, 2 )]
		public int sceCtrlPeekBufferNegative( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceCtrlData *pad_data
			// a1 = int count

			List<ControlSample> samples = new List<ControlSample>( a1 );

			int toRead;
			lock( this )
			{
				toRead = Math.Min( a1, _buffer.Count );
				// TODO: fix up to do more than just 1
				for( int n = 0; n < toRead; n++ )
					samples.Add( _buffer.Peek() );
			}

			if( a0 != 0 )
			{
				int addr = a0;
				for( int n = 0; n < samples.Count; n++ )
				{
					ControlSample sample = samples[ n ];
					memory.WriteWord( addr + 0, 4, ( int )sample.Timestamp );
					memory.WriteWord( addr + 4, 4, ~( int )sample.Buttons );
					memory.WriteWord( addr + 8, 4, sample.AnalogX );
					memory.WriteWord( addr + 12, 4, sample.AnalogY );
					//memory.WriteBytes( addr + 16, ReservedBytes );
					addr += 22;
				}
			}

			// int
			return toRead;
		}

		[BiosStub( 0x1f803938, "sceCtrlReadBufferPositive", true, 2 )]
		public int sceCtrlReadBufferPositive( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceCtrlData *pad_data
			// a1 = int count

			List<ControlSample> samples = new List<ControlSample>( a1 );

			while( samples.Count < a1 )
			{
				while( _buffer.Count == 0 )
				{
					_dataPresent.WaitOne( InputPollInterval * 2, true );
				}
				lock( this )
					samples.Add( _buffer.Dequeue() );
			}

			if( a0 != 0 )
			{
				int addr = a0;
				for( int n = 0; n < samples.Count; n++ )
				{
					ControlSample sample = samples[ n ];
					memory.WriteWord( addr + 0, 4, ( int )sample.Timestamp );
					memory.WriteWord( addr + 4, 4, ( int )sample.Buttons );
					memory.WriteWord( addr + 8, 1, ( byte )sample.AnalogX );
					memory.WriteWord( addr + 9, 1, ( byte )sample.AnalogY );
					//memory.WriteBytes( addr + 10, ReservedBytes );
					addr += 16;
				}
			}

			// int
			return samples.Count;
		}

		[BiosStub( 0x60b81f86, "sceCtrlReadBufferNegative", true, 2 )]
		public int sceCtrlReadBufferNegative( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceCtrlData *pad_data
			// a1 = int count

			List<ControlSample> samples = new List<ControlSample>( a1 );

			lock( this )
			{
				while( samples.Count < a1 )
				{
					while( _buffer.Count == 0 )
						_dataPresent.WaitOne();

					samples.Add( _buffer.Dequeue() );
				}
			}

			if( a0 != 0 )
			{
				int addr = a0;
				for( int n = 0; n < samples.Count; n++ )
				{
					ControlSample sample = samples[ n ];
					memory.WriteWord( addr + 0, 4, ( int )sample.Timestamp );
					memory.WriteWord( addr + 4, 4, ~( int )sample.Buttons );
					memory.WriteWord( addr + 8, 4, sample.AnalogX );
					memory.WriteWord( addr + 12, 4, sample.AnalogY );
					//memory.WriteBytes( addr + 16, ReservedBytes );
					addr += 22;
				}
			}

			// int
			return samples.Count;
		}

		[BiosStub( 0xb1d0e5cd, "sceCtrlPeekLatch", false, 0 )]
		[BiosStubIncomplete]
		public int sceCtrlPeekLatch( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x0b588501, "sceCtrlReadLatch", false, 0 )]
		[BiosStubIncomplete]
		public int sceCtrlReadLatch( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}
	}
}
