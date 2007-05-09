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
	class sceAta_driver : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceAta_driver";
			}
		}

		#endregion

		#region State Management

		public sceAta_driver( Kernel kernel )
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
		[BiosFunction( 0xB176BB2E, "sceAtaInit" )]
		int sceAtaInit(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x071D3D4D, "sceAtaTerm" )]
		int sceAtaTerm(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD1E6E175, "sceAta_driver_D1E6E175" )]
		int sceAta_driver_D1E6E175(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xAC800B1D, "sceAta_driver_AC800B1D" )]
		int sceAta_driver_AC800B1D(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC8BC8B83, "sceAtaStart" )]
		int sceAtaStart(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCDC50BF0, "sceAtaStop" )]
		int sceAtaStop(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x52D9A4CA, "sceAtaGetAtaDrive" )]
		int sceAtaGetAtaDrive(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xEBB91566, "sceAtaSetupAtaBlock" )]
		int sceAtaSetupAtaBlock(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6C58F096, "sceAta_driver_6C58F096" )]
		int sceAta_driver_6C58F096(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC21318E6, "sceAta_driver_C21318E6" )]
		int sceAta_driver_C21318E6(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA77C230B, "sceAta_driver_A77C230B" )]
		int sceAta_driver_A77C230B(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3AA3FA39, "sceAtaSelectDevice" )]
		int sceAtaSelectDevice(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xACCEE63F, "sceAta_driver_ACCEE63F" )]
		int sceAta_driver_ACCEE63F(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6E639701, "sceAtaScanDevice" )]
		int sceAtaScanDevice(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB863FD83, "sceAtaCheckDeviceReady" )]
		int sceAtaCheckDeviceReady(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB6BED47E, "sceAtaAccessDataPort" )]
		int sceAtaAccessDataPort(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6DE1C65F, "sceAtaAccessDataPortIE" )]
		int sceAtaAccessDataPortIE(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0B53CAD8, "sceAta_driver_0B53CAD8" )]
		int sceAta_driver_0B53CAD8(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1DF6C4D2, "sceAtaExecCmd" )]
		int sceAtaExecCmd(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xAB66789B, "sceAtaExecCmdIE" )]
		int sceAtaExecCmdIE(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC74F04B7, "sceAtaExecPacketCmd" )]
		int sceAtaExecPacketCmd(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBDD30DEE, "sceAtaExecPacketCmdIE" )]
		int sceAtaExecPacketCmdIE(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x010F750B, "sceAta_driver_010F750B" )]
		int sceAta_driver_010F750B(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9CA52F94, "sceAta_driver_9CA52F94" )]
		int sceAta_driver_9CA52F94(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBDEA51A5, "sceAta_driver_BDEA51A5" )]
		int sceAta_driver_BDEA51A5(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB9C9F2E4, "sceAta_driver_B9C9F2E4" )]
		int sceAta_driver_B9C9F2E4(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFDDF4C74, "sceAta_driver_FDDF4C74" )]
		int sceAta_driver_FDDF4C74(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC4060B8A, "sceAta_driver_C4060B8A" )]
		int sceAta_driver_C4060B8A(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x84E14690, "sceAta_driver_84E14690" )]
		int sceAta_driver_84E14690(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC7B02795, "sceAta_driver_C7B02795" )]
		int sceAta_driver_C7B02795(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1F05F48C, "sceAta_driver_1F05F48C" )]
		int sceAta_driver_1F05F48C(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBA09142A, "sceAta_driver_BA09142A" )]
		int sceAta_driver_BA09142A(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1C8DA2FD, "sceAta_driver_1C8DA2FD" )]
		int sceAta_driver_1C8DA2FD(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2AE26E08, "sceAta_driver_2AE26E08" )]
		int sceAta_driver_2AE26E08(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9D98086E, "sceAta_driver_9D98086E" )]
		int sceAta_driver_9D98086E(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x689FCB7D, "sceAta_driver_689FCB7D" )]
		int sceAta_driver_689FCB7D(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC144826E, "sceAta_driver_C144826E" )]
		int sceAta_driver_C144826E(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFCF939D9, "sceAtaGetAtaCallBack" )]
		int sceAtaGetAtaCallBack(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x298DDC3D, "sceAta_driver_298DDC3D" )]
		int sceAta_driver_298DDC3D(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF6AC2E5D, "sceAta_driver_F6AC2E5D" )]
		int sceAta_driver_F6AC2E5D(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF76A5B9C, "sceAta_driver_F76A5B9C" )]
		int sceAta_driver_F76A5B9C(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xAE100256, "sceAta_driver_AE100256" )]
		int sceAta_driver_AE100256(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4222D6F3, "sceAta_driver_4222D6F3" )]
		int sceAta_driver_4222D6F3(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4D225674, "sceAta_driver_4D225674" )]
		int sceAta_driver_4D225674(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x01248DA2, "sceAta_driver_01248DA2" )]
		int sceAta_driver_01248DA2(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7DE9E14A, "sceAta_driver_7DE9E14A" )]
		int sceAta_driver_7DE9E14A(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2E32045A, "sceAta_driver_2E32045A" )]
		int sceAta_driver_2E32045A(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1E6202F8, "sceAta_driver_1E6202F8" )]
		int sceAta_driver_1E6202F8(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE3F01009, "sceAta_driver_E3F01009" )]
		int sceAta_driver_E3F01009(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF512A273, "sceAta_driver_F512A273" )]
		int sceAta_driver_F512A273(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4844EB0C, "sceAta_driver_4844EB0C" )]
		int sceAta_driver_4844EB0C(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD6BF0B85, "sceAta_driver_D6BF0B85" )]
		int sceAta_driver_D6BF0B85(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xAC84FF0A, "sceAta_driver_AC84FF0A" )]
		int sceAta_driver_AC84FF0A(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x943FC7AC, "sceAta_driver_943FC7AC" )]
		int sceAta_driver_943FC7AC(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x322460B6, "sceAta_driver_322460B6" )]
		int sceAta_driver_322460B6(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF61EAFC0, "sceAta_driver_F61EAFC0" )]
		int sceAta_driver_F61EAFC0(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x76C0923D, "sceAta_driver_76C0923D" )]
		int sceAta_driver_76C0923D(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE3E1EED7, "sceAta_driver_E3E1EED7" )]
		int sceAta_driver_E3E1EED7(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA62258FD, "sceAtaBufferSelectSemaphore" )]
		int sceAtaBufferSelectSemaphore(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC10F87AB, "sceAta_driver_C10F87AB" )]
		int sceAta_driver_C10F87AB(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB985F2B0, "sceAta_driver_B985F2B0" )]
		int sceAta_driver_B985F2B0(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8CADA96B, "sceAta_driver_8CADA96B" )]
		int sceAta_driver_8CADA96B(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7C6B31D8, "sceAta_driver_7C6B31D8" )]
		int sceAta_driver_7C6B31D8(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6FD8E2AB, "sceAta_driver_6FD8E2AB" )]
		int sceAta_driver_6FD8E2AB(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9AE67E14, "sceAtaGetDriveStat" )]
		int sceAtaGetDriveStat(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x110D3739, "sceAtaSetDriveStat" )]
		int sceAtaSetDriveStat(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7BBA095C, "sceAtaClearDriveStat" )]
		int sceAtaClearDriveStat(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3A61BF97, "sceAta_driver_3A61BF97" )]
		int sceAta_driver_3A61BF97(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xACFF7CB5, "sceAta_driver_ACFF7CB5" )]
		int sceAta_driver_ACFF7CB5(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBE07B3A7, "sceAta_driver_BE07B3A7" )]
		int sceAta_driver_BE07B3A7(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD8E525CB, "sceAtaGetIntrFlag" )]
		int sceAtaGetIntrFlag(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB7F5B2CA, "sceAtaSetIntrFlag" )]
		int sceAtaSetIntrFlag(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x638EEA14, "sceAta_driver_638EEA14" )]
		int sceAta_driver_638EEA14(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB5982381, "sceAta_driver_B5982381" )]
		int sceAta_driver_B5982381(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - 2CF6EB56 */
