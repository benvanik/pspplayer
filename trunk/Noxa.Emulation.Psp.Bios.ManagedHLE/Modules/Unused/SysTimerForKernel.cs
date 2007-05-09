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
	class SysTimerForKernel : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "SysTimerForKernel";
			}
		}

		#endregion

		#region State Management

		public SysTimerForKernel( Kernel kernel )
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
		[BiosFunction( 0xC99073E3, "sceSTimerAlloc" )]
		int sceSTimerAlloc(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC105CF38, "sceSTimerFree" )]
		int sceSTimerFree(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB53534B4, "SysTimerForKernel_B53534B4" )]
		int SysTimerForKernel_B53534B4(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x975D8E84, "sceSTimerSetHandler" )]
		int sceSTimerSetHandler(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA95143E2, "sceSTimerStartCount" )]
		int sceSTimerStartCount(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4A01F9D3, "sceSTimerStopCount" )]
		int sceSTimerStopCount(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x54BB5DB4, "sceSTimerResetCount" )]
		int sceSTimerResetCount(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x228EDAE4, "sceSTimerGetCount" )]
		int sceSTimerGetCount(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x53231A15, "SysTimerForKernel_53231A15" )]
		int SysTimerForKernel_53231A15(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - D14FCF59 */
