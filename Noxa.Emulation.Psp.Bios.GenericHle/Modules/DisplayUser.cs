using System;
using System.Collections.Generic;
using System.Text;
using Noxa.Emulation.Psp.Cpu;
using System.Diagnostics;
using Noxa.Emulation.Psp.Video;

namespace Noxa.Emulation.Psp.Bios.GenericHle.Modules
{
	class DisplayUser : IModule
	{
		#region IModule Members

		protected HleInstance _hle;
		protected Kernel _kernel;

		public DisplayUser( HleInstance hle )
		{
			Debug.Assert( hle != null );

			_hle = hle;
			_kernel = hle.Kernel as Kernel;
		}

		public string Name
		{
			get
			{
				return "sceDisplay";
			}
		}

		#endregion

		[BiosStub( 0x0e20f177, "sceDisplaySetMode", true, 3 )]
		public int sceDisplaySetMode( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = int mode
			// a1 = int width
			// a2 = int height

			_hle.Emulator.Video.Suspend();

			DisplayProperties props = _hle.Emulator.Video.Properties;
			props.Mode = a0;
			props.Width = a1;
			props.Height = a2;

			if( _hle.Emulator.Video.Resume() != true )
				return -1;
			
			// int
			return 0;
		}

		[BiosStub( 0xdea197d4, "sceDisplayGetMode", true, 3 )]
		public int sceDisplayGetMode( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = int *pmode
			// a1 = int *pwidth
			// a2 = int *pheight

			DisplayProperties props = _hle.Emulator.Video.Properties;

			if( a0 != 0 )
				memory.WriteWord( a0, 4, props.Mode );
			if( a1 != 0 )
				memory.WriteWord( a1, 4, props.Width );
			if( a2 != 0 )
				memory.WriteWord( a2, 4, props.Height );
			
			// int
			return 0;
		}

		[BiosStub( 0xdba6c4c4, "sceDisplayGetFramePerSec", false, 0 )]
		[BiosStubIncomplete]
		public int sceDisplayGetFramePerSec( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x7ed59bc4, "sceDisplaySetHoldMode", false, 0 )]
		[BiosStubIncomplete]
		public int sceDisplaySetHoldMode( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xa544c486, "sceDisplaySetResumeMode", false, 0 )]
		[BiosStubIncomplete]
		public int sceDisplaySetResumeMode( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x289d82fe, "sceDisplaySetFrameBuf", false, 4 )]
		public int sceDisplaySetFrameBuf( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = void *topaddr
			// a1 = int bufferwidth
			// a2 = int pixelformat
			// a3 = int sync

			_hle.Emulator.Video.Suspend();

			DisplayProperties props = _hle.Emulator.Video.Properties;
			props.BufferAddress = ( uint )( a0 & 0x0FFFFFFF );
			props.BufferSize = ( uint )a1;
			props.PixelFormat = ( PixelFormat )a2;
			props.SyncMode = ( BufferSyncMode )a3;

			//Debug.WriteLine( string.Format( "fb addr set to {0:X8}", a0 ) );

			_hle.Emulator.Video.Resume();
			
			return 0;
		}

		[BiosStub( 0xbf79f646, "", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown1( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xeeda2e54, "sceDisplayGetFrameBuf", true, 4 )]
		public int sceDisplayGetFrameBuf( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = void * *topaddr
			// a1 = int *bufferwidth
			// a2 = int *pixelformat
			// a3 = int *unk1

			DisplayProperties props = _hle.Emulator.Video.Properties;

			if( a0 != 0 )
				memory.WriteWord( a0, 4, ( int )props.BufferAddress );
			if( a1 != 0 )
				memory.WriteWord( a1, 4, ( int )props.BufferSize );
			if( a2 != 0 )
				memory.WriteWord( a2, 4, ( int )props.PixelFormat );
			if( a3 != 0 )
				memory.WriteWord( a3, 4, ( int )props.SyncMode );
			
			// int
			return 0;
		}

		[BiosStub( 0xb4f378fa, "sceDisplayIsForeground", false, 0 )]
		[BiosStubIncomplete]
		public int sceDisplayIsForeground( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x31c4baa8, "sceDisplayGetBrightness", false, 0 )]
		[BiosStubIncomplete]
		public int sceDisplayGetBrightness( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x9c6eaad7, "sceDisplayGetVcount", true, 0 )]
		public int sceDisplayGetVcount( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// unsigned int
			return ( int )_hle.Emulator.Video.Vcount;
		}

		[BiosStub( 0x4d4e10ec, "sceDisplayIsVblank", false, 0 )]
		[BiosStubIncomplete]
		public int sceDisplayIsVblank( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x36cdfade, "sceDisplayWaitVblank", false, 0 )]
		[BiosStubIncomplete]
		public int sceDisplayWaitVblank( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x8eb9ec49, "sceDisplayWaitVblankCB", false, 0 )]
		[BiosStubIncomplete]
		public int sceDisplayWaitVblankCB( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x984c27e7, "sceDisplayWaitVblankStart", true, 0 )]
		public int sceDisplayWaitVblankStart( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			IVideoDriver video = _hle.Emulator.Video;
			//if( video.Vblank != null )
			//	video.Vblank.WaitOne( 1000, false );

			// int
			return 0;
		}

		[BiosStub( 0x21038913, "", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown2( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x46f186c3, "sceDisplayWaitVblankStartCB", false, 0 )]
		[BiosStubIncomplete]
		public int sceDisplayWaitVblankStartCB( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x773dd3a3, "sceDisplayGetCurrentHcount", false, 0 )]
		[BiosStubIncomplete]
		public int sceDisplayGetCurrentHcount( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x210eab3a, "sceDisplayGetAccumulatedHcount", false, 0 )]
		[BiosStubIncomplete]
		public int sceDisplayGetAccumulatedHcount( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xa83ef139, "", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown3( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x69b53541, "sceDisplayGetVblankRest", false, 0 )]
		[BiosStubIncomplete]
		public int sceDisplayGetVblankRest( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}
	}
}
