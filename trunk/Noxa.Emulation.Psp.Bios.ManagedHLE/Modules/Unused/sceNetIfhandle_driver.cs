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
	class sceNetIfhandle_driver : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceNetIfhandle_driver";
			}
		}

		#endregion

		#region State Management

		public sceNetIfhandle_driver( Kernel kernel )
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
		[BiosFunction( 0x30F69334, "sceNetIfhandleInit" )]
		int sceNetIfhandleInit(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB9096E48, "sceNetIfhandleTerm" )]
		int sceNetIfhandleTerm(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB8188F96, "sceNetIfhandle_driver_B8188F96" )]
		int sceNetIfhandle_driver_B8188F96(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4FB43BCE, "sceNetIfhandle_driver_4FB43BCE" )]
		int sceNetIfhandle_driver_4FB43BCE(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB1F5BB87, "sceNetIfhandleIfStart" )]
		int sceNetIfhandleIfStart(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8FCB05A1, "sceNetIfhandleIfUp" )]
		int sceNetIfhandleIfUp(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xEAD3A759, "sceNetIfhandleIfDown" )]
		int sceNetIfhandleIfDown(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0296C7D6, "sceNetIfhandleIfIoctl" )]
		int sceNetIfhandleIfIoctl(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE440A7D8, "sceNetIfhandleIfDequeue" )]
		int sceNetIfhandleIfDequeue(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x30602CE9, "sceNetIfhandleSignalSema" )]
		int sceNetIfhandleSignalSema(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD5DA7B3C, "sceNetIfhandleWaitSema" )]
		int sceNetIfhandleWaitSema(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2162EE67, "sceNetIfhandlePollSema" )]
		int sceNetIfhandlePollSema(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x16042084, "sceNetIfhandle_driver_16042084" )]
		int sceNetIfhandle_driver_16042084(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC9344A59, "sceNetIfhandle_driver_C9344A59" )]
		int sceNetIfhandle_driver_C9344A59(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xAE81C0CB, "sceNetIfhandle_driver_AE81C0CB" )]
		int sceNetIfhandle_driver_AE81C0CB(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x54D1AEA1, "sceNetIfhandle_driver_54D1AEA1" )]
		int sceNetIfhandle_driver_54D1AEA1(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF94BAF52, "sceNetIfhandle_driver_F94BAF52" )]
		int sceNetIfhandle_driver_F94BAF52(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x263767F6, "sceNetIfhandle_driver_263767F6" )]
		int sceNetIfhandle_driver_263767F6(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE2F4F1C9, "sceNetIfDequeue" )]
		int sceNetIfDequeue(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC28F6FF2, "sceNetIfEnqueue" )]
		int sceNetIfEnqueue(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x16246B99, "sceNetIfPrepend" )]
		int sceNetIfPrepend(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC6D14282, "sceNetIfhandle_driver_C6D14282" )]
		int sceNetIfhandle_driver_C6D14282(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD5AD6DEA, "sceNetIfhandle_driver_D5AD6DEA" )]
		int sceNetIfhandle_driver_D5AD6DEA(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0C391E9F, "sceNetIfhandle_driver_0C391E9F" )]
		int sceNetIfhandle_driver_0C391E9F(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x29ED84C5, "sceNetIfhandle_driver_29ED84C5" )]
		int sceNetIfhandle_driver_29ED84C5(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x15CFE3C0, "sceNetMallocInternal" )]
		int sceNetMallocInternal(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x76BAD213, "sceNetFreeInternal" )]
		int sceNetFreeInternal(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4C2886CB, "sceNetGetMallocStatInternal" )]
		int sceNetGetMallocStatInternal(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0FB8AE0D, "sceNetIfhandle_driver_0FB8AE0D" )]
		int sceNetIfhandle_driver_0FB8AE0D(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5FB31C72, "sceNetIfhandle_driver_5FB31C72" )]
		int sceNetIfhandle_driver_5FB31C72(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x62B20015, "sceNetIfhandle_driver_62B20015" )]
		int sceNetIfhandle_driver_62B20015(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBFF3CEA5, "sceNetMAdj" )]
		int sceNetMAdj(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3E8DD3F8, "sceNetMCat" )]
		int sceNetMCat(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1560F143, "sceNetMCopyback" )]
		int sceNetMCopyback(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9A6261EC, "sceNetIfhandle_driver_9A6261EC" )]
		int sceNetIfhandle_driver_9A6261EC(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x456E3146, "sceNetIfhandle_driver_456E3146" )]
		int sceNetIfhandle_driver_456E3146(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x955F2924, "sceNetIfhandle_driver_955F2924" )]
		int sceNetIfhandle_driver_955F2924(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6AB53C27, "sceNetMDup" )]
		int sceNetMDup(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF8825DC4, "sceNetMFree" )]
		int sceNetMFree(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF56FAC82, "sceNetMFreem" )]
		int sceNetMFreem(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA493AA5F, "sceNetMGet" )]
		int sceNetMGet(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x59F0D619, "sceNetIfhandle_driver_59F0D619" )]
		int sceNetIfhandle_driver_59F0D619(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4CF15C43, "sceNetIfhandle_driver_4CF15C43" )]
		int sceNetIfhandle_driver_4CF15C43(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC3325FDC, "sceNetMPrepend" )]
		int sceNetMPrepend(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE80F00A4, "sceNetIfhandle_driver_E80F00A4" )]
		int sceNetIfhandle_driver_E80F00A4(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x49EDBB18, "sceNetIfhandle_driver_49EDBB18" )]
		int sceNetIfhandle_driver_49EDBB18(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0542835F, "sceNetIfhandle_driver_0542835F" )]
		int sceNetIfhandle_driver_0542835F(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x773FC77C, "sceNetIfhandle_driver_773FC77C" )]
		int sceNetIfhandle_driver_773FC77C(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9CBA24D4, "sceNetIfhandle_driver_9CBA24D4" )]
		int sceNetIfhandle_driver_9CBA24D4(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC5623112, "sceNetIfhandle_driver_C5623112" )]
		int sceNetIfhandle_driver_C5623112(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC80181A2, "sceNetGetDropRate" )]
		int sceNetGetDropRate(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFD8585E1, "sceNetSetDropRate" )]
		int sceNetSetDropRate(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - B9077302 */
