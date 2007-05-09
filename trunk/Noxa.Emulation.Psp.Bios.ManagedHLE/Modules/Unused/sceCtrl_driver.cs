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
	class sceCtrl_driver : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceCtrl_driver";
			}
		}

		#endregion

		#region State Management

		public sceCtrl_driver( Kernel kernel )
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
		[BiosFunction( 0x3E65A0EA, "sceCtrlInit" )]
		int sceCtrlInit(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE03956E9, "sceCtrlEnd" )]
		int sceCtrlEnd(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC3F607F3, "sceCtrlSuspend" )]
		int sceCtrlSuspend(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC245B57B, "sceCtrlResume" )]
		int sceCtrlResume(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6A2774F3, "sceCtrlSetSamplingCycle" )]
		int sceCtrlSetSamplingCycle(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x02BAAD91, "sceCtrlGetSamplingCycle" )]
		int sceCtrlGetSamplingCycle(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1F4011E6, "sceCtrlSetSamplingMode" )]
		int sceCtrlSetSamplingMode(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDA6B76A1, "sceCtrlGetSamplingMode" )]
		int sceCtrlGetSamplingMode(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3A622550, "sceCtrlPeekBufferPositive" )]
		int sceCtrlPeekBufferPositive(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC152080A, "sceCtrlPeekBufferNegative" )]
		int sceCtrlPeekBufferNegative(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1F803938, "sceCtrlReadBufferPositive" )]
		int sceCtrlReadBufferPositive(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x60B81F86, "sceCtrlReadBufferNegative" )]
		int sceCtrlReadBufferNegative(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB1D0E5CD, "sceCtrlPeekLatch" )]
		int sceCtrlPeekLatch(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0B588501, "sceCtrlReadLatch" )]
		int sceCtrlReadLatch(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA88E8D22, "sceCtrl_driver_A88E8D22" )]
		int sceCtrl_driver_A88E8D22(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB7CEAED4, "sceCtrl_driver_B7CEAED4" )]
		int sceCtrl_driver_B7CEAED4(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x348D99D4, "sceCtrl_driver_348D99D4" )]
		int sceCtrl_driver_348D99D4(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xAF5960F3, "sceCtrl_driver_AF5960F3" )]
		int sceCtrl_driver_AF5960F3(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA68FD260, "sceCtrl_driver_A68FD260" )]
		int sceCtrl_driver_A68FD260(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6841BE1A, "sceCtrl_driver_6841BE1A" )]
		int sceCtrl_driver_6841BE1A(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7CA723DC, "sceCtrl_driver_7CA723DC" )]
		int sceCtrl_driver_7CA723DC(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5E77BC8A, "sceCtrl_driver_5E77BC8A" )]
		int sceCtrl_driver_5E77BC8A(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5C56C779, "sceCtrl_driver_5C56C779" )]
		int sceCtrl_driver_5C56C779(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - 7748E421 */
