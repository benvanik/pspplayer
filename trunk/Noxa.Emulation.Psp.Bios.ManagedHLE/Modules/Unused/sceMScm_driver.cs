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
	class sceMScm_driver : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceMScm_driver";
			}
		}

		#endregion

		#region State Management

		public sceMScm_driver( Kernel kernel )
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
		[BiosFunction( 0x4E923738, "sceMScmStartModule" )]
		int sceMScmStartModule(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x68C14F25, "sceMScmStopModule" )]
		int sceMScmStopModule(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5BAAB238, "sceMScmRegisterCLDM" )]
		int sceMScmRegisterCLDM(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9A5CB5CC, "sceMScmUnRegisterCLDM" )]
		int sceMScmUnRegisterCLDM(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x54B3A3F1, "sceMScmGetCLDM" )]
		int sceMScmGetCLDM(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5AB92658, "sceMScm_driver_5AB92658" )]
		int sceMScm_driver_5AB92658(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x12BD8FEC, "sceMScmRegisterCLD" )]
		int sceMScmRegisterCLD(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x08A1EC6B, "sceMScmUnRegisterCLD" )]
		int sceMScmUnRegisterCLD(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4451D813, "sceMScmDetachCLD" )]
		int sceMScmDetachCLD(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0D8D6A54, "sceMScm_driver_0D8D6A54" )]
		int sceMScm_driver_0D8D6A54(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF0CC24D1, "sceMScm_driver_F0CC24D1" )]
		int sceMScm_driver_F0CC24D1(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7B5396BA, "sceMScm_driver_7B5396BA" )]
		int sceMScm_driver_7B5396BA(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x42A40895, "sceMScmReadMSReg" )]
		int sceMScmReadMSReg(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0128147B, "sceMScmWriteMSReg" )]
		int sceMScmWriteMSReg(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCBB2BF6F, "sceMScm_driver_CBB2BF6F" )]
		int sceMScm_driver_CBB2BF6F(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6C8AEF0B, "sceMScm_driver_6C8AEF0B" )]
		int sceMScm_driver_6C8AEF0B(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF1133DAF, "sceMScm_driver_F1133DAF" )]
		int sceMScm_driver_F1133DAF(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x907D7766, "sceMScm_driver_907D7766" )]
		int sceMScm_driver_907D7766(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x46AAC4E5, "sceMScm_driver_46AAC4E5" )]
		int sceMScm_driver_46AAC4E5(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2D7C40FA, "sceMScmWaitHCIntr" )]
		int sceMScmWaitHCIntr(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0A054CDA, "sceMScm_driver_0A054CDA" )]
		int sceMScm_driver_0A054CDA(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x21183216, "sceMScm_driver_21183216" )]
		int sceMScm_driver_21183216(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBE455B5D, "sceMScm_driver_BE455B5D" )]
		int sceMScm_driver_BE455B5D(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDE188BB6, "sceMScmStartReadDataDMA" )]
		int sceMScmStartReadDataDMA(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x97290A44, "sceMScm_driver_97290A44" )]
		int sceMScm_driver_97290A44(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB1D1C718, "sceMScm_driver_B1D1C718" )]
		int sceMScm_driver_B1D1C718(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD6DB3199, "sceMScm_driver_D6DB3199" )]
		int sceMScm_driver_D6DB3199(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF1924A06, "sceMScmSetSlotPower" )]
		int sceMScmSetSlotPower(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFCD92C74, "sceMScmGetSlotPower" )]
		int sceMScmGetSlotPower(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2AD0A649, "sceMScmGetSlotState" )]
		int sceMScmGetSlotState(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xAA67D87A, "sceMScmResetHC" )]
		int sceMScmResetHC(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x36921225, "sceMScm_driver_36921225" )]
		int sceMScm_driver_36921225(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xEF42A4A3, "sceMScm_driver_EF42A4A3" )]
		int sceMScm_driver_EF42A4A3(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFF6C50D8, "sceMScm_driver_FF6C50D8" )]
		int sceMScm_driver_FF6C50D8(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x34124B97, "sceMScm_driver_34124B97" )]
		int sceMScm_driver_34124B97(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF82AF926, "sceMScm_driver_F82AF926" )]
		int sceMScm_driver_F82AF926(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3FFE76E5, "sceMScm_driver_3FFE76E5" )]
		int sceMScm_driver_3FFE76E5(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4FA42259, "sceMScm_driver_4FA42259" )]
		int sceMScm_driver_4FA42259(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x494FB570, "sceMScm_driver_494FB570" )]
		int sceMScm_driver_494FB570(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC003705D, "sceMScm_driver_C003705D" )]
		int sceMScm_driver_C003705D(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4E4C3099, "sceMScm_driver_4E4C3099" )]
		int sceMScm_driver_4E4C3099(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6F4F7E4C, "sceMScm_driver_6F4F7E4C" )]
		int sceMScm_driver_6F4F7E4C(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCFD8F662, "sceMScm_driver_CFD8F662" )]
		int sceMScm_driver_CFD8F662(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE24B5D0C, "sceMScm_driver_E24B5D0C" )]
		int sceMScm_driver_E24B5D0C(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB01C378C, "sceMScm_driver_B01C378C" )]
		int sceMScm_driver_B01C378C(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x18DD6E90, "sceMScm_driver_18DD6E90" )]
		int sceMScm_driver_18DD6E90(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - BEC1C380 */
