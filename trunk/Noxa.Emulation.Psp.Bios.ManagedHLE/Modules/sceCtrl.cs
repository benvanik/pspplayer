// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

using Noxa.Utilities;
using Noxa.Emulation.Psp;
using Noxa.Emulation.Psp.Bios;
using Noxa.Emulation.Psp.Cpu;
using Noxa.Emulation.Psp.Input;
using System.Threading;

namespace Noxa.Emulation.Psp.Bios.ManagedHLE.Modules
{
	unsafe class sceCtrl : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceCtrl";
			}
		}

		#endregion

		private class Sample
		{
			public uint Timestamp;
			public int Buttons;
			public int AnalogX;
			public int AnalogY;
		}

		private Timer _timer;
		private FastLinkedList<Sample> _buffer;
		private AutoResetEvent _bufferEvent;
		private object _bufferSyncRoot;

		#region State Management

		public sceCtrl( Kernel kernel )
			: base( kernel )
		{
		}

		public override void Start()
		{
			_device = _kernel.Emulator.Input;
			if( _device == null )
				return;

			_buffer = new FastLinkedList<Sample>();
			_bufferEvent = new AutoResetEvent( false );
			_bufferSyncRoot = new object();

			_timer = _kernel.TimerQueue.CreatePeriodicTimer(
				new TimerCallback( this.PollCallback ), 16,
				TimerExecutionContext.WorkerThread, false );
		}

		public override void Stop()
		{
			if( ( _timer != null ) &&
				( _kernel.TimerQueue != null ) )
				_kernel.TimerQueue.StopTimer( _timer );
			_timer = null;

			if( _buffer != null )
				_buffer.Clear();
		}

		private void PollCallback( Timer timer )
		{
			_device.Poll();

			Sample sample = new Sample();
			sample.Timestamp = ( uint )Environment.TickCount;
			sample.Buttons = ( int )_device.Buttons;
			sample.AnalogX = _device.AnalogX;
			sample.AnalogY = _device.AnalogY;
			lock( _bufferSyncRoot )
			{
				_buffer.Dequeue();
				_buffer.Enqueue( sample );
			}
			_bufferEvent.Set();

			uint oldPressed = _pressedButtons;
			_pressedButtons = ( uint )_device.Buttons;
			uint stillPressed = _pressedButtons & oldPressed;
			_releasedButtons = ~_pressedButtons;

			_makedButtons = _pressedButtons & ~stillPressed;
			_breakedButtons = oldPressed & ~stillPressed;
		}

		#endregion

		private enum ControlSamplingMode
		{
			DigitalOnly = 0,
			AnalogAndDigital = 1,
		}

		private IInputDevice _device;
		private int _sampleCycle = 0;
		private ControlSamplingMode _sampleMode = ControlSamplingMode.AnalogAndDigital;

		private uint _makedButtons;
		private uint _breakedButtons;
		private uint _pressedButtons;
		private uint _releasedButtons;

		[Stateless]
		[BiosFunction( 0x6A2774F3, "sceCtrlSetSamplingCycle" )]
		// SDK location: /ctrl/pspctrl.h:119
		// SDK declaration: int sceCtrlSetSamplingCycle(int cycle);
		public int sceCtrlSetSamplingCycle( int cycle )
		{
			int old = _sampleCycle;
			_sampleCycle = cycle;
			return old;
		}

		[Stateless]
		[BiosFunction( 0x02BAAD91, "sceCtrlGetSamplingCycle" )]
		// SDK location: /ctrl/pspctrl.h:128
		// SDK declaration: int sceCtrlGetSamplingCycle(int *pcycle);
		public int sceCtrlGetSamplingCycle( int pcycle )
		{
			if( pcycle != 0 )
				_memory.WriteWord( pcycle, 4, _sampleCycle );
			return 0;
		}

		[Stateless]
		[BiosFunction( 0x1F4011E6, "sceCtrlSetSamplingMode" )]
		// SDK location: /ctrl/pspctrl.h:137
		// SDK declaration: int sceCtrlSetSamplingMode(int mode);
		public int sceCtrlSetSamplingMode( int mode )
		{
			int old = ( int )_sampleMode;
			_sampleMode = ( ControlSamplingMode )mode;
			return old;
		}

		[Stateless]
		[BiosFunction( 0xDA6B76A1, "sceCtrlGetSamplingMode" )]
		// SDK location: /ctrl/pspctrl.h:146
		// SDK declaration: int sceCtrlGetSamplingMode(int *pmode);
		public int sceCtrlGetSamplingMode( int pmode )
		{
			if( pmode != 0 )
				_memory.WriteWord( pmode, 4, ( int )_sampleMode );
			return 0;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA7144800, "sceCtrlSetIdleCancelThreshold" )]
		// manual add
		public int sceCtrlSetIdleCancelThreshold()
		{
			return Module.NotImplementedReturn;
		}

		private byte* WritePadData( byte* p, Sample sample, bool positive )
		{
			byte analogX;
			byte analogY;
			if( _sampleMode == ControlSamplingMode.AnalogAndDigital )
			{
				analogX = ( byte )( ( ( ( float )sample.AnalogX / ( float )( ushort.MaxValue / 2 ) ) + 0.5f ) * ( float )( byte.MaxValue - 1 ) );
				analogY = ( byte )( ( ( ( float )sample.AnalogY / ( float )( ushort.MaxValue / 2 ) ) + 0.5f ) * ( float )( byte.MaxValue - 1 ) );
			}
			else
			{
				analogX = 0;
				analogY = 0;
			}

			*( ( int* )p ) = ( int )sample.Timestamp;
			if( positive == true )
				*( ( int* )( p + 4 ) ) = ( int )sample.Buttons;
			else
				*( ( int* )( p + 4 ) ) = ~( int )sample.Buttons;
			*( p + 8 ) = analogX;
			*( p + 9 ) = analogY;
			// 6 bytes of junk

			return p + 16;
		}

		[DontTrace]
		[Stateless]
		[BiosFunction( 0x3A622550, "sceCtrlPeekBufferPositive" )]
		// SDK location: /ctrl/pspctrl.h:148
		// SDK declaration: int sceCtrlPeekBufferPositive(SceCtrlData *pad_data, int count);
		public int sceCtrlPeekBufferPositive( int pad_data, int count )
		{
			if( pad_data != 0 )
			{
				byte* p = _memorySystem.Translate( ( uint )pad_data );
				for( int n = 0; n < count; n++ )
				{
					Sample sample;
					lock( _bufferSyncRoot )
						sample = _buffer.Dequeue();
					if( sample == null )
						return n;

					p = WritePadData( p, sample, true );
				}
				return count;
			}
			return 0;
		}

		[DontTrace]
		[Stateless]
		[BiosFunction( 0xC152080A, "sceCtrlPeekBufferNegative" )]
		// SDK location: /ctrl/pspctrl.h:150
		// SDK declaration: int sceCtrlPeekBufferNegative(SceCtrlData *pad_data, int count);
		public int sceCtrlPeekBufferNegative( int pad_data, int count )
		{
			if( pad_data != 0 )
			{
				byte* p = _memorySystem.Translate( ( uint )pad_data );
				for( int n = 0; n < count; n++ )
				{
					Sample sample;
					lock( _bufferSyncRoot )
						sample = _buffer.Dequeue();
					if( sample == null )
						return n;

					p = WritePadData( p, sample, false );
				}
				return count;
			}
			return 0;
		}

		[DontTrace]
		[Stateless]
		[BiosFunction( 0x1F803938, "sceCtrlReadBufferPositive" )]
		// SDK location: /ctrl/pspctrl.h:168
		// SDK declaration: int sceCtrlReadBufferPositive(SceCtrlData *pad_data, int count);
		public int sceCtrlReadBufferPositive( int pad_data, int count )
		{
			if( pad_data != 0 )
			{
				byte* p = _memorySystem.Translate( ( uint )pad_data );
				for( int n = 0; n < count; n++ )
				{
					Sample sample;
					do
					{
						_bufferEvent.WaitOne();
						lock( _bufferSyncRoot )
							sample = _buffer.Dequeue();
					} while( sample == null );

					p = WritePadData( p, sample, true );
				}
				return count;
			}
			return 0;
		}

		[DontTrace]
		[Stateless]
		[BiosFunction( 0x60B81F86, "sceCtrlReadBufferNegative" )]
		// SDK location: /ctrl/pspctrl.h:170
		// SDK declaration: int sceCtrlReadBufferNegative(SceCtrlData *pad_data, int count);
		public int sceCtrlReadBufferNegative( int pad_data, int count )
		{
			if( pad_data != 0 )
			{
				byte* p = _memorySystem.Translate( ( uint )pad_data );
				for( int n = 0; n < count; n++ )
				{
					Sample sample;
					do
					{
						_bufferEvent.WaitOne();
						lock( _bufferSyncRoot )
							sample = _buffer.Dequeue();
					} while( sample == null );

					p = WritePadData( p, sample, false );
				}
				return count;
			}
			return 0;
		}

		[DontTrace]
		[Stateless]
		[BiosFunction( 0xB1D0E5CD, "sceCtrlPeekLatch" )]
		// SDK location: /ctrl/pspctrl.h:172
		// SDK declaration: int sceCtrlPeekLatch(SceCtrlLatch *latch_data);
		public int sceCtrlPeekLatch( int latch_data )
		{
			uint* ptr = ( uint* )_memorySystem.Translate( ( uint )latch_data );
			*ptr = _makedButtons;
			*( ptr + 1 ) = _breakedButtons;
			*( ptr + 2 ) = _pressedButtons;
			*( ptr + 3 ) = _releasedButtons;
			return 0;
		}

		[DontTrace]
		[Stateless]
		[BiosFunction( 0x0B588501, "sceCtrlReadLatch" )]
		// SDK location: /ctrl/pspctrl.h:174
		// SDK declaration: int sceCtrlReadLatch(SceCtrlLatch *latch_data);
		public int sceCtrlReadLatch( int latch_data )
		{
			uint* ptr = ( uint* )_memorySystem.Translate( ( uint )latch_data );
			*ptr = _makedButtons;
			*( ptr + 1 ) = _breakedButtons;
			*( ptr + 2 ) = _pressedButtons;
			*( ptr + 3 ) = _releasedButtons;
			_makedButtons = 0;
			_breakedButtons = 0;
			return 0;
		}

	}
}

/* GenerateStubsV2: auto-generated - D9D618DF */
