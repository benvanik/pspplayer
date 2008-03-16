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
using Noxa.Emulation.Psp.Video;

namespace Noxa.Emulation.Psp.Bios.ManagedHLE.Modules
{
	class sceDisplay : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceDisplay";
			}
		}

		#endregion

		#region State Management

		public sceDisplay( Kernel kernel )
			: base( kernel )
		{
		}

		public override void Start()
		{
			_driver = _kernel.Emulator.Video;
			if( _driver == null )
				return;

			NativeMethods.QueryPerformanceFrequency( out _frequency );

			_driver.SetMode( 0, 480, 272 );
			_driver.SetFrameBuffer( MemorySystem.VideoMemoryBase, 512, PixelFormat.Rgba8888, BufferSyncMode.NextFrame );
		}

		public override void Stop()
		{
			_driver = null;
		}

		#endregion

		public IVideoDriver _driver;

		[SuggestNative]
		[Stateless]
		[BiosFunction( 0xDBA6C4C4, "sceDisplayGetFramePerSec" )]
		// manual add - is this int or float return?
		public int sceDisplayGetFramePerSec()
		{
			// ?
			return 60;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7ED59BC4, "sceDisplaySetHoldMode" )]
		public int sceDisplaySetHoldMode(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA544C486, "sceDisplaySetResumeMode" )]
		public int sceDisplaySetResumeMode(){ return Module.NotImplementedReturn; }

		[DontTrace]
		[Stateless]
		[BiosFunction( 0x0E20F177, "sceDisplaySetMode" )]
		// SDK location: /display/pspdisplay.h:53
		// SDK declaration: int sceDisplaySetMode(int mode, int width, int height);
		public int sceDisplaySetMode( int mode, int width, int height )
		{
			if( _driver.SetMode( mode, width, height ) == true )
				return 0;
			else
				return -1;
		}

		[Stateless]
		[BiosFunction( 0xDEA197D4, "sceDisplayGetMode" )]
		// SDK location: /display/pspdisplay.h:64
		// SDK declaration: int sceDisplayGetMode(int *pmode, int *pwidth, int *pheight);
		public int sceDisplayGetMode( int pmode, int pwidth, int pheight )
		{
			DisplayProperties props = _driver.QueryDisplayProperties();
			if( pmode != 0 )
				_memory.WriteWord( pmode, 4, props.Mode );
			if( pwidth != 0 )
				_memory.WriteWord( pwidth, 4, props.Width );
			if( pheight != 0 )
				_memory.WriteWord( pheight, 4, props.Height );
			return 0;
		}

		[DontTrace]
		[SuggestNative]
		[Stateless]
		[BiosFunction( 0x289D82FE, "sceDisplaySetFrameBuf" )]
		// SDK location: /display/pspdisplay.h:74
		// SDK declaration: void sceDisplaySetFrameBuf(void *topaddr, int bufferwidth, int pixelformat, int sync);
		public int sceDisplaySetFrameBuf( int topaddr, int bufferwidth, int pixelformat, int sync )
		{
			if( _driver.SetFrameBuffer( ( uint )( topaddr & 0x0FFFFFFF ), ( uint )bufferwidth, ( PixelFormat )pixelformat, ( BufferSyncMode )sync ) == true )
				return 0;
			else
				return 1;
		}

		[Stateless]
		[BiosFunction( 0xEEDA2E54, "sceDisplayGetFrameBuf" )]
		// SDK location: /display/pspdisplay.h:84
		// SDK declaration: int sceDisplayGetFrameBuf(void **topaddr, int *bufferwidth, int *pixelformat, int unk1);
		public int sceDisplayGetFrameBuf( int topaddr, int bufferwidth, int pixelformat, int sync )
		{
			DisplayProperties props = _driver.QueryDisplayProperties();
			if( topaddr != 0 )
				_memory.WriteWord( topaddr, 4, ( int )props.BufferAddress );
			if( bufferwidth != 0 )
				_memory.WriteWord( bufferwidth, 4, ( int )props.BufferSize );
			if( pixelformat != 0 )
				_memory.WriteWord( pixelformat, 4, ( int )props.PixelFormat );
			return 0;
		}

		[SuggestNative]
		[Stateless]
		[BiosFunction( 0xB4F378FA, "sceDisplayIsForeground" )]
		public int sceDisplayIsForeground()
		{
			return 1;
		}

		[DontTrace]
		[SuggestNative]
		[Stateless]
		[BiosFunction( 0x9C6EAAD7, "sceDisplayGetVcount" )]
		// SDK location: /display/pspdisplay.h:89
		// SDK declaration: unsigned int sceDisplayGetVcount();
		public int sceDisplayGetVcount()
		{
			return ( int )_driver.Vcount;
		}

		[SuggestNative]
		[Stateless]
		[BiosFunction( 0x773DD3A3, "sceDisplayGetCurrentHcount" )]
		// manual add
		public int sceDisplayGetCurrentHcount()
		{
			return 0;
		}

		[SuggestNative]
		[Stateless]
		[BiosFunction( 0x210EAB3A, "sceDisplayGetAccumulatedHcount" )]
		// manual add
		public int sceDisplayGetAccumulatedHcount()
		{
			ulong count = _driver.Vcount * 480 * 272;
			return ( int )( uint )( count % 0x7FFFFFFF );
		}

		// 16.7ms is right, but that assumes we just missed the last one
		//private const int VblankTime = 16777;
		private const int VblankTime = 1000;

		private long _frequency;
		private long _lastVblankWait;

		private void WaitVblank( bool allowCallbacks )
		{
			// This is essentially the same logic in the video driver, but
			// replicated here because the video driver can't wait for callbacks
			long time;
			NativeMethods.QueryPerformanceCounter( out time );
			long elapsed = time - _lastVblankWait;
			if( _lastVblankWait == 0 )
			{
				elapsed = 1000;
			}
			else
			{
				// ticks are 100ns, we need us
				elapsed /= ( _frequency / 10000 );
				if( elapsed == 0 )
					elapsed = 1000;
			}
			_lastVblankWait = time;

			// elapsed now has the number of milliseconds that have elapsed since the last time
			uint fixedElapsed =	16777 - Math.Min( 16777, ( uint )elapsed );
			if( fixedElapsed < 1000 )
				return;

			// This could be just a return
			if( _kernel.SpeedLocked == false )
				fixedElapsed = 1000;

			KThread thread = _kernel.ActiveThread;
			Debug.Assert( thread != null );
			thread.Delay( fixedElapsed, allowCallbacks );
		}

		[DontTrace]
		[SuggestNative]
		[Stateless]
		[BiosFunction( 0x36CDFADE, "sceDisplayWaitVblank" )]
		// SDK location: /display/pspdisplay.h:104
		// SDK declaration: int sceDisplayWaitVblank();
		public int sceDisplayWaitVblank()
		{
			// RETURN FB ADDRESS
			this.WaitVblank( false );
			return 0x0000DEAD;
		}

		[DontTrace]
		[Stateless]
		[BiosFunction( 0x8EB9EC49, "sceDisplayWaitVblankCB" )]
		// SDK location: /display/pspdisplay.h:109
		// SDK declaration: int sceDisplayWaitVblankCB();
		public int sceDisplayWaitVblankCB()
		{
			// RETURN FB ADDRESS
			this.WaitVblank( true );
			return 0x0000DEAD;
		}

		[DontTrace]
		[SuggestNative]
		[Stateless]
		[BiosFunction( 0x984C27E7, "sceDisplayWaitVblankStart" )]
		// SDK location: /display/pspdisplay.h:94
		// SDK declaration: int sceDisplayWaitVblankStart();
		public int sceDisplayWaitVblankStart()
		{
			// RETURN FB ADDRESS
			this.WaitVblank( false );
			return 0x0000DEAD;
		}

		[DontTrace]
		[Stateless]
		[BiosFunction( 0x46F186C3, "sceDisplayWaitVblankStartCB" )]
		// SDK location: /display/pspdisplay.h:99
		// SDK declaration: int sceDisplayWaitVblankStartCB();
		public int sceDisplayWaitVblankStartCB()
		{
			// RETURN FB ADDRESS
			this.WaitVblank( true );
			return 0x0000DEAD;
		}
	}
}

/* GenerateStubsV2: auto-generated - 94D9F6B7 */
