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
	class sceUmd : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceUmd";
			}
		}

		#endregion

		#region State Management

		public sceUmd( Kernel kernel )
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
		[BiosFunction( 0x3342000C, "sceUmdIsUMD" )]
		int sceUmdIsUMD(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB8479844, "sceUmdCallBackInit" )]
		int sceUmdCallBackInit(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBE17B77C, "sceUmd_BE17B77C" )]
		int sceUmd_BE17B77C(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x28355079, "sceUmd_28355079" )]
		int sceUmd_28355079(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7850F057, "sceUmd_7850F057" )]
		int sceUmd_7850F057(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF6AC1DBA, "sceUmd_F6AC1DBA" )]
		int sceUmd_F6AC1DBA(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xAEE7404D, "sceUmdRegisterUMDCallBack" )]
		int sceUmdRegisterUMDCallBack(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1FB77367, "sceUmd_1FB77367" )]
		int sceUmd_1FB77367(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x84231FCF, "sceUmd_84231FCF" )]
		int sceUmd_84231FCF(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x18624052, "sceUmd_18624052" )]
		int sceUmd_18624052(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8EF268AC, "sceUmd_8EF268AC" )]
		int sceUmd_8EF268AC(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE71270FA, "sceUmd_E71270FA" )]
		int sceUmd_E71270FA(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x04D1AAD9, "sceUmd_04D1AAD9" )]
		int sceUmd_04D1AAD9(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBD2BDE07, "sceUmdUnRegisterUMDCallBack" )]
		int sceUmdUnRegisterUMDCallBack(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x27A764A1, "sceUmd_27A764A1" )]
		int sceUmd_27A764A1(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7AA26C9A, "sceUmd_7AA26C9A" )]
		int sceUmd_7AA26C9A(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF8FD25E7, "sceUmd_F8FD25E7" )]
		int sceUmd_F8FD25E7(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA140DEC2, "sceUmd_A140DEC2" )]
		int sceUmd_A140DEC2(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x725DFB14, "sceUmd_725DFB14" )]
		int sceUmd_725DFB14(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7F40CC41, "sceUmd_7F40CC41" )]
		int sceUmd_7F40CC41(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1A2485D2, "sceUmd_1A2485D2" )]
		int sceUmd_1A2485D2(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3BA4EC53, "sceUmd_3BA4EC53" )]
		int sceUmd_3BA4EC53(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x08709F2D, "sceUmd_08709F2D" )]
		int sceUmd_08709F2D(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xAD18C797, "sceUmd_AD18C797" )]
		int sceUmd_AD18C797(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9CADBF19, "sceUmdSetActivateData" )]
		int sceUmdSetActivateData(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7F38693E, "sceUmdSetDeactivateData" )]
		int sceUmdSetDeactivateData(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x075F1E0B, "sceUmd_075F1E0B" )]
		int sceUmd_075F1E0B(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xEB56097E, "sceUmd_EB56097E" )]
		int sceUmd_EB56097E(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x46EBB729, "sceUmdCheckMedium" )]
		int sceUmdCheckMedium(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x230666E3, "sceUmdSetDriveStatus" )]
		int sceUmdSetDriveStatus(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xAE53DC2D, "sceUmdClearDriveStatus" )]
		int sceUmdClearDriveStatus(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD45D1FE6, "sceUmdGetDriveStatus" )]
		int sceUmdGetDriveStatus(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3D0DECD5, "sceUmd_3D0DECD5" )]
		int sceUmd_3D0DECD5(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD01B2DC6, "sceUmd_D01B2DC6" )]
		int sceUmd_D01B2DC6(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3925CBD8, "sceUmd_3925CBD8" )]
		int sceUmd_3925CBD8(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x003E3396, "sceUmdGetUMDPower" )]
		int sceUmdGetUMDPower(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0F7E578A, "sceUmd_0F7E578A" )]
		int sceUmd_0F7E578A(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9DCB2DA6, "sceUmdSetResumeStatus" )]
		int sceUmdSetResumeStatus(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFD3878D6, "sceUmdSetSuspendStatus" )]
		int sceUmdSetSuspendStatus(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3CE40626, "sceUmd_3CE40626" )]
		int sceUmd_3CE40626(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1E62CCA3, "sceUmd_1E62CCA3" )]
		int sceUmd_1E62CCA3(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6A41ED25, "sceUmdGetSuspendResumeMode" )]
		int sceUmdGetSuspendResumeMode(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4C952ACF, "sceUmdSetSuspendResumeMode" )]
		int sceUmdSetSuspendResumeMode(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - 83666B35 */
