// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Noxa.Emulation.Psp.Cpu;
using Noxa.Emulation.Psp.Media;

namespace Noxa.Emulation.Psp.Bios.GenericHle.Modules
{
	class UmdUser : IModule
	{
		#region IModule Members
		
		protected HleInstance _hle;
		protected Kernel _kernel;

		public UmdUser( HleInstance hle )
		{
			Debug.Assert( hle != null );

			_hle = hle;
			_kernel = hle.Kernel as Kernel;
		}

		public string Name
		{
			get
			{
				return "sceUmdUser";
			}
		}

		#endregion

		[Flags]
		public enum UmdStatus
		{
			Init = 0x00,
			MediaOut = 0x01,
			MediaIn = 0x02,
			MediaChange = 0x04,
			NotReady = 0x08,
			Ready = 0x10,
			Readable = 0x20,
		}

		public enum UmdMode
		{
			PowerOn = 0x01,
			PowerCurrent = 0x02,
		}

		[BiosStub( 0xc6183d47, "sceUmdActivate", true, 2 )]
		public int sceUmdActivate( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = int mode
			// a1 = const char *drive

			Debug.WriteLine( string.Format( "sceUmdActivate: activating mode {0} as {1}", ( UmdMode )a0, Kernel.ReadString( memory, a1 ) ) );
			
			// int
			return 0;
		}

		[BiosStub( 0xe83742ba, "sceUmdDeactivate", true, 2 )]
		public int sceUmdDeactivate( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = int mode
			// a1 = const char *drive

			Debug.WriteLine( string.Format( "sceUmdDeactivate: deactivating mode {0} as {1}", ( UmdMode )a0, Kernel.ReadString( memory, a1 ) ) );

			// int
			return 0;
		}

		[BiosStub( 0x8ef08fce, "sceUmdWaitDriveStat", true, 1 )]
		public int sceUmdWaitDriveStat( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = int stat

			UmdStatus status = ( UmdStatus )a0;
			Debug.WriteLine( string.Format( "sceUmdWaitDriveStat: waiting for status {0}", status ) );
			
			// int
			return 0;
		}

		[BiosStub( 0x56202973, "sceUmdWaitDriveStatWithTimer", false, 0 )]
		[BiosStubIncomplete]
		public int sceUmdWaitDriveStatWithTimer( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x4a9e5e29, "sceUmdWaitDriveStatCB", false, 0 )]
		[BiosStubIncomplete]
		public int sceUmdWaitDriveStatCB( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xaee7404d, "sceUmdRegisterUMDCallBack", true, 1 )]
		[BiosStubIncomplete]
		public int sceUmdRegisterUMDCallBack( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = int cbid
			
			// int
			return 0;
		}

		[BiosStub( 0x6b4a146c, "sceUmdGetDriveStat", true, 0 )]
		public int sceUmdGetDriveStat( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// int
			return ( int )( UmdStatus.Init | UmdStatus.MediaIn | UmdStatus.Ready | UmdStatus.Readable );
		}

		[BiosStub( 0x6af9b50a, "sceUmd_0x6AF9B50A", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown1( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x20628e6f, "sceUmdGetErrorStat", false, 0 )]
		[BiosStubIncomplete]
		public int sceUmdGetErrorStat( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x340b7686, "sceUmdGetDiscInfo", true, 1 )]
		public int sceUmdGetDiscInfo( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceUmdDiscInfo *ptr

			IUmdDevice umd = _hle.Emulator.Umd;
			if( umd == null )
				return -1;

			// uint size
			// uint mediaType
			memory.WriteWord( a0, 4, ( int )umd.Capacity );
			memory.WriteWord( a0 + 4, 4, ( int )umd.DiscType );

			// int
			return 0;
		}

		[BiosStub( 0xbd2bde07, "sceUmdUnRegisterUMDCallBack", false, 0 )]
		[BiosStubIncomplete]
		public int sceUmdUnRegisterUMDCallBack( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x46ebb729, "sceUmdCheckMedium", true, 0 )]
		public int sceUmdCheckMedium( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			IUmdDevice umd = _hle.Emulator.Umd;
			if( umd == null )
				return 0;

			// int
			switch( umd.State )
			{
				default:
				case MediaState.Ejected:
					return 0;
				case MediaState.Present:
					return 1;
			}
		}

		[BiosStub( 0x87533940, "sceUmd_0x87533940", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown2( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xcbe9f02a, "sceUmd_0xCBE9F02A", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown3( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}


	}
}
