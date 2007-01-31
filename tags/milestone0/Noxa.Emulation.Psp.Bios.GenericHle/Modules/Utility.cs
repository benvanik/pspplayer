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

namespace Noxa.Emulation.Psp.Bios.GenericHle.Modules
{
	class Utility : IModule
	{
		#region IModule Members
		
		protected HleInstance _hle;
		protected Kernel _kernel;

		public Utility( HleInstance hle )
		{
			Debug.Assert( hle != null );

			_hle = hle;
			_kernel = hle.Kernel as Kernel;
		}

		public string Name
		{
			get
			{
				return "sceUtility";
			}
		}

		#endregion

		[BiosStub( 0xc492f751, "sceUtilityGameSharingInitStart", false, 0 )]
		[BiosStubIncomplete]
		public int sceUtilityGameSharingInitStart( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xefc6f80f, "sceUtilityGameSharingShutdownStart", false, 0 )]
		[BiosStubIncomplete]
		public int sceUtilityGameSharingShutdownStart( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x7853182d, "sceUtilityGameSharingUpdate", false, 0 )]
		[BiosStubIncomplete]
		public int sceUtilityGameSharingUpdate( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x946963f3, "sceUtilityGameSharingGetStatus", false, 0 )]
		[BiosStubIncomplete]
		public int sceUtilityGameSharingGetStatus( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x3ad50ae7, "sceNetplayDialogInitStart", false, 0 )]
		[BiosStubIncomplete]
		public int sceNetplayDialogInitStart( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xbc6b6296, "sceNetplayDialogShutdownStart", false, 0 )]
		[BiosStubIncomplete]
		public int sceNetplayDialogShutdownStart( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x417bed54, "sceNetplayDialogUpdate", false, 0 )]
		[BiosStubIncomplete]
		public int sceNetplayDialogUpdate( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xb6cee597, "sceNetplayDialogGetStatus", false, 0 )]
		[BiosStubIncomplete]
		public int sceNetplayDialogGetStatus( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x4db1e739, "sceUtilityNetconfInitStart", true, 1 )]
		[BiosStubIncomplete]
		public int sceUtilityNetconfInitStart( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = pspUtilityNetconfData *data
			
			// int
			return 0;
		}

		[BiosStub( 0xf88155f6, "sceUtilityNetconfShutdownStart", true, 0 )]
		[BiosStubIncomplete]
		public int sceUtilityNetconfShutdownStart( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// int
			return 0;
		}

		[BiosStub( 0x91e70e35, "sceUtilityNetconfUpdate", true, 1 )]
		[BiosStubIncomplete]
		public int sceUtilityNetconfUpdate( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = int unknown
			
			// int
			return 0;
		}

		[BiosStub( 0x6332aa39, "sceUtilityNetconfGetStatus", true, 0 )]
		[BiosStubIncomplete]
		public int sceUtilityNetconfGetStatus( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// int
			return 0;
		}

		[BiosStub( 0x50c4cd57, "sceUtilitySavedataInitStart", true, 1 )]
		[BiosStubIncomplete]
		public int sceUtilitySavedataInitStart( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceUtilitySavedataParam *params
			
			// int
			return 0;
		}

		[BiosStub( 0x9790b33c, "sceUtilitySavedataShutdownStart", true, 0 )]
		[BiosStubIncomplete]
		public int sceUtilitySavedataShutdownStart( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// int
			return 0;
		}

		[BiosStub( 0xd4b95ffb, "sceUtilitySavedataUpdate", false, 1 )]
		[BiosStubIncomplete]
		public int sceUtilitySavedataUpdate( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = int unknown
			
			return 0;
		}

		[BiosStub( 0x8874dbe0, "sceUtilitySavedataGetStatus", true, 0 )]
		[BiosStubIncomplete]
		public int sceUtilitySavedataGetStatus( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// int
			return 0;
		}

		[BiosStub( 0x2995d020, "sceUtility_0x2995D020", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown1( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xb62a4061, "sceUtility_0xB62A4061", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown2( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xed0fad38, "sceUtility_0xED0FAD38", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown3( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x88bc7406, "sceUtility_0x88BC7406", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown4( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x2ad8e239, "sceUtilityMsgDialogInitStart", true, 1 )]
		[BiosStubIncomplete]
		public int sceUtilityMsgDialogInitStart( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceUtilityMsgDialogParams *params
			
			// int
			return 0;
		}

		[BiosStub( 0x67af3428, "sceUtilityMsgDialogShutdownStart", false, 0 )]
		[BiosStubIncomplete]
		public int sceUtilityMsgDialogShutdownStart( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x95fc253b, "sceUtilityMsgDialogUpdate", false, 1 )]
		[BiosStubIncomplete]
		public int sceUtilityMsgDialogUpdate( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = int n
			
			return 0;
		}

		[BiosStub( 0x9a1c91d7, "sceUtilityMsgDialogGetStatus", true, 0 )]
		[BiosStubIncomplete]
		public int sceUtilityMsgDialogGetStatus( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// int
			return 0;
		}

		[BiosStub( 0xf6269b82, "sceUtilityOskInitStart", true, 1 )]
		[BiosStubIncomplete]
		public int sceUtilityOskInitStart( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceUtilityOskParams *params
			
			// int
			return 0;
		}

		[BiosStub( 0x3dfaeba9, "sceUtilityOskShutdownStart", true, 0 )]
		[BiosStubIncomplete]
		public int sceUtilityOskShutdownStart( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// int
			return 0;
		}

		[BiosStub( 0x4b85c861, "sceUtilityOskUpdate", true, 1 )]
		[BiosStubIncomplete]
		public int sceUtilityOskUpdate( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = int n
			
			// int
			return 0;
		}

		[BiosStub( 0xf3f76017, "sceUtilityOskGetStatus", true, 0 )]
		[BiosStubIncomplete]
		public int sceUtilityOskGetStatus( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// int
			return 0;
		}

		[BiosStub( 0x45c18506, "sceUtilitySetSystemParamInt", true, 2 )]
		[BiosStubIncomplete]
		public int sceUtilitySetSystemParamInt( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = int id
			// a1 = int value
			
			// int
			return 0;
		}

		[BiosStub( 0xdde5389d, "sceUtility_0xDDE5389D", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown5( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x41e30674, "sceUtilitySetSystemParamString", true, 2 )]
		[BiosStubIncomplete]
		public int sceUtilitySetSystemParamString( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = int id
			// a1 = const char *str
			
			// int
			return 0;
		}

		[BiosStub( 0x149a7895, "sceUtility_0x149A7895", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown6( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xa5da2406, "sceUtilityGetSystemParamInt", true, 2 )]
		[BiosStubIncomplete]
		public int sceUtilityGetSystemParamInt( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = int id
			// a1 = int *value
			
			// int
			return 0;
		}

		[BiosStub( 0x4a833ba4, "sceUtility_0x4A833BA4", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown7( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x34b78343, "sceUtilityGetSystemParamString", true, 3 )]
		[BiosStubIncomplete]
		public int sceUtilityGetSystemParamString( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = int id
			// a1 = char *str
			// a2 = int len
			
			// int
			return 0;
		}

		[BiosStub( 0xa50e5b30, "sceUtility_0xA50E5B30", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown8( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x5eee6548, "sceUtilityCheckNetParam", true, 1 )]
		[BiosStubIncomplete]
		public int sceUtilityCheckNetParam( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = int id
			
			// int
			return 0;
		}

		[BiosStub( 0x943cba46, "sceUtility_0x943CBA46", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown9( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x434d4b3a, "sceUtilityGetNetParam", true, 3 )]
		[BiosStubIncomplete]
		public int sceUtilityGetNetParam( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = int conf
			// a1 = int param
			// a2 = netData *data
			
			// int
			return 0;
		}

		[BiosStub( 0x0f3eeaac, "sceUtility_0x0F3EEAACC", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown10( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x147f7c85, "sceUtility_0x147F7C85", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown11( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x16a1a8d8, "sceUtility_0x16A1A8D8", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown12( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xcdc3aa41, "sceUtility_0xCDC3AA41", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown13( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xf5ce1134, "sceUtility_0xF5CE1134", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown14( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x05afb9e4, "sceUtility_0x05AFB9E4", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown15( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xbda7d894, "sceUtility_0xBDA7D894", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown16( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x4fed24d8, "sceUtility_0x4FED24D8", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown17( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x1579a159, "sceUtility_0x1579A159", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown18( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x64d50c56, "sceUtility_0x64D50C56", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown19( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}
	}
}
