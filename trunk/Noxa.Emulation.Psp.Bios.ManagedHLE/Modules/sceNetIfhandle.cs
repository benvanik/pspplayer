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

namespace Noxa.Emulation.Psp.Bios.ManagedHLE.Modules
{
	class sceNetIfhandle : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceNetIfhandle_lib";
			}
		}

		#endregion

		#region State Management

		public sceNetIfhandle( Kernel kernel )
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

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC80181A2, "sceNetGetDropRate" )]
		public int sceNetGetDropRate()
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFD8585E1, "sceNetSetDropRate" )]
		public int sceNetSetDropRate()
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x30F69334, "sceNetIfhandleInit" )]
		public int sceNetIfhandleInit()
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB9096E48, "sceNetIfhandleTerm" )]
		public int sceNetIfhandleTerm()
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB8188F96, "sceNetIfhandle_lib_B8188F96" )]
		public int sceNetIfhandle_lib_B8188F96()
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4FB43BCE, "sceNetIfhandle_lib_4FB43BCE" )]
		public int sceNetIfhandle_lib_4FB43BCE()
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB1F5BB87, "sceNetIfhandleIfStart" )]
		public int sceNetIfhandleIfStart()
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8FCB05A1, "sceNetIfhandleIfUp" )]
		public int sceNetIfhandleIfUp()
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xEAD3A759, "sceNetIfhandleIfDown" )]
		public int sceNetIfhandleIfDown()
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0296C7D6, "sceNetIfhandleIfIoctl" )]
		public int sceNetIfhandleIfIoctl()
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE440A7D8, "sceNetIfhandleIfDequeue" )]
		public int sceNetIfhandleIfDequeue()
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x30602CE9, "sceNetIfhandleSignalSema" )]
		public int sceNetIfhandleSignalSema()
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD5DA7B3C, "sceNetIfhandleWaitSema" )]
		public int sceNetIfhandleWaitSema()
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2162EE67, "sceNetIfhandlePollSema" )]
		public int sceNetIfhandlePollSema()
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0C391E9F, "sceNetIfhandle_lib_0C391E9F" )]
		public int sceNetIfhandle_lib_0C391E9F()
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x29ED84C5, "sceNetIfhandle_lib_29ED84C5" )]
		public int sceNetIfhandle_lib_29ED84C5()
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x15CFE3C0, "sceNetMallocInternal" )]
		public int sceNetMallocInternal()
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x76BAD213, "sceNetFreeInternal" )]
		public int sceNetFreeInternal()
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4C2886CB, "sceNetGetMallocStatInternal" )]
		public int sceNetGetMallocStatInternal()
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0FB8AE0D, "sceNetIfhandle_lib_0FB8AE0D" )]
		public int sceNetIfhandle_lib_0FB8AE0D()
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5FB31C72, "sceNetIfhandle_lib_5FB31C72" )]
		public int sceNetIfhandle_lib_5FB31C72()
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x62B20015, "sceNetIfhandle_lib_62B20015" )]
		public int sceNetIfhandle_lib_62B20015()
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBFF3CEA5, "sceNetMAdj" )]
		public int sceNetMAdj()
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3E8DD3F8, "sceNetMCat" )]
		public int sceNetMCat()
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1560F143, "sceNetMCopyback" )]
		public int sceNetMCopyback()
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9A6261EC, "sceNetIfhandle_lib_9A6261EC" )]
		public int sceNetIfhandle_lib_9A6261EC()
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x456E3146, "sceNetIfhandle_lib_456E3146" )]
		public int sceNetIfhandle_lib_456E3146()
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x955F2924, "sceNetIfhandle_lib_955F2924" )]
		public int sceNetIfhandle_lib_955F2924()
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6AB53C27, "sceNetMDup" )]
		public int sceNetMDup()
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF8825DC4, "sceNetMFree" )]
		public int sceNetMFree()
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF56FAC82, "sceNetMFreem" )]
		public int sceNetMFreem()
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA493AA5F, "sceNetMGet" )]
		public int sceNetMGet()
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x59F0D619, "sceNetIfhandle_lib_59F0D619" )]
		public int sceNetIfhandle_lib_59F0D619()
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4CF15C43, "sceNetIfhandle_lib_4CF15C43" )]
		public int sceNetIfhandle_lib_4CF15C43()
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC3325FDC, "sceNetMPrepend" )]
		public int sceNetMPrepend()
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE80F00A4, "sceNetIfhandle_lib_E80F00A4" )]
		public int sceNetIfhandle_lib_E80F00A4()
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x49EDBB18, "sceNetIfhandle_lib_49EDBB18" )]
		public int sceNetIfhandle_lib_49EDBB18()
		{
			return Module.NotImplementedReturn;
		}
	}
}

/* GenerateStubsV2: auto-generated - 3E99645D */
