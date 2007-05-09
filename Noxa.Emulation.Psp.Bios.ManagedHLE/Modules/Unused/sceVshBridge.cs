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
	class sceVshBridge : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceVshBridge";
			}
		}

		#endregion

		#region State Management

		public sceVshBridge( Kernel kernel )
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
		[BiosFunction( 0xA5628F0D, "vshKernelLoadModuleVSH" )]
		int vshKernelLoadModuleVSH(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x41C54ADF, "vshKernelLoadModuleVSHByID" )]
		int vshKernelLoadModuleVSHByID(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC9626587, "vshKernelLoadModuleBufferVSH" )]
		int vshKernelLoadModuleBufferVSH(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5C2983C2, "sceVshBridge_5C2983C2" )]
		int sceVshBridge_5C2983C2(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC949966C, "sceVshBridge_C949966C" )]
		int sceVshBridge_C949966C(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC6395C03, "vshCtrlReadBufferPositive" )]
		int vshCtrlReadBufferPositive(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0163A8E7, "vshUmdManTerm" )]
		int vshUmdManTerm(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7C1658F2, "sceVshBridge_7C1658F2" )]
		int sceVshBridge_7C1658F2(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0E10922A, "vshDisplaySetHoldMode" )]
		int vshDisplaySetHoldMode(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCA719C34, "vshImposeGetStatus" )]
		int vshImposeGetStatus(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4E4E4DA3, "vshImposeSetStatus" )]
		int vshImposeSetStatus(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x639C3CB3, "vshImposeGetParam" )]
		int vshImposeGetParam(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4A596D2D, "vshImposeSetParam" )]
		int vshImposeSetParam(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5894C339, "vshImposeChanges" )]
		int vshImposeChanges(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0FA48729, "vshRtcSetConf" )]
		int vshRtcSetConf(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x16415246, "vshRtcSetCurrentTick" )]
		int vshRtcSetCurrentTick(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5350C073, "vshMSAudioFormatICV" )]
		int vshMSAudioFormatICV(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2380DC08, "vshIoDevctl" )]
		int vshIoDevctl(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4DB43867, "vshIdStorageLookup" )]
		int vshIdStorageLookup(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7B67394E, "vshAudioSetFrequency" )]
		int vshAudioSetFrequency(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCE32CBEF, "vshMSAudioInit" )]
		int vshMSAudioInit(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE5DA5E95, "vshMSAudioEnd" )]
		int vshMSAudioEnd(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6CAEB765, "vshMSAudioAuth" )]
		int vshMSAudioAuth(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x53BFD101, "vshMSAudioCheckICV" )]
		int vshMSAudioCheckICV(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE174218C, "sceVshBridge_E174218C" )]
		int sceVshBridge_E174218C(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7EA32357, "vshMSAudioDeauth" )]
		int vshMSAudioDeauth(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x14877197, "sceVshBridge_14877197" )]
		int sceVshBridge_14877197(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5BBB35E4, "sceVshBridge_5BBB35E4" )]
		int sceVshBridge_5BBB35E4(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB27C593F, "sceVshBridge_B27C593F" )]
		int sceVshBridge_B27C593F(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0D2CEAD2, "sceVshBridge_0D2CEAD2" )]
		int sceVshBridge_0D2CEAD2(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD120667D, "sceVshBridge_D120667D" )]
		int sceVshBridge_D120667D(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD907B6AA, "sceVshBridge_D907B6AA" )]
		int sceVshBridge_D907B6AA(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD46D4528, "vshMSAudioInvalidateICV" )]
		int vshMSAudioInvalidateICV(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7A63BE73, "sceVshBridge_7A63BE73" )]
		int sceVshBridge_7A63BE73(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x222A18C4, "sceVshBridge_222A18C4" )]
		int sceVshBridge_222A18C4(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x04310D7C, "sceVshBridge_04310D7C" )]
		int sceVshBridge_04310D7C(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4E61C72E, "sceVshBridge_4E61C72E" )]
		int sceVshBridge_4E61C72E(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE2DD0A81, "vshMSAudioGetInitialEKB" )]
		int vshMSAudioGetInitialEKB(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6396ACBD, "vshMSAudioGetICVInfo" )]
		int vshMSAudioGetICVInfo(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x274BB6AE, "vshVaudioOutputBlocking" )]
		int vshVaudioOutputBlocking(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8C440581, "vshVaudioChReserve" )]
		int vshVaudioChReserve(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x07EC5661, "vshVaudioChRelease" )]
		int vshVaudioChRelease(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3B3D9F5D, "sceVshBridge_3B3D9F5D" )]
		int sceVshBridge_3B3D9F5D(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB7F233A2, "sceVshBridge_B7F233A2" )]
		int sceVshBridge_B7F233A2(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC58D0939, "sceVshBridge_C58D0939" )]
		int sceVshBridge_C58D0939(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCBDA2613, "vshMeRpcLock" )]
		int vshMeRpcLock(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA7F0E8E0, "vshMeRpcUnlock" )]
		int vshMeRpcUnlock(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x98B4117E, "vshKernelLoadExecBufferPlain0" )]
		int vshKernelLoadExecBufferPlain0(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8399A8AA, "vshKernelLoadExecBufferPlain" )]
		int vshKernelLoadExecBufferPlain(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE614F45F, "vshKernelLoadExecFromHost" )]
		int vshKernelLoadExecFromHost(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xEEFB02BB, "vshKernelLoadExec" )]
		int vshKernelLoadExec(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9929DDA5, "vshKernelExitVSH" )]
		int vshKernelExitVSH(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB7C46DCA, "vshKernelLoadExecVSHDiscUpdater" )]
		int vshKernelLoadExecVSHDiscUpdater(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x04AEC74C, "vshKernelLoadExecVSHDiscDebug" )]
		int vshKernelLoadExecVSHDiscDebug(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF4873F4D, "sceVshBridge_F4873F4D" )]
		int sceVshBridge_F4873F4D(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x83528906, "vshKernelLoadExecBufferVSHUsbWlan" )]
		int vshKernelLoadExecBufferVSHUsbWlan(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x68BE3316, "vshKernelLoadExecBufferVSHUsbWlanDebug" )]
		int vshKernelLoadExecBufferVSHUsbWlanDebug(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF35BFB7D, "vshKernelLoadExecVSHMs1" )]
		int vshKernelLoadExecVSHMs1(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x97FB006F, "vshKernelLoadExecVSHMs2" )]
		int vshKernelLoadExecVSHMs2(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x029EF6C9, "vshKernelLoadExecVSHMs3" )]
		int vshKernelLoadExecVSHMs3(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x40716012, "vshKernelExitVSHVSH" )]
		int vshKernelExitVSHVSH(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x88DA81A5, "vshKernelLoadExecBufferVSHPlain" )]
		int vshKernelLoadExecBufferVSHPlain(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x88BD8364, "vshKernelLoadExecBufferVSHFromHost" )]
		int vshKernelLoadExecBufferVSHFromHost(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x74DA9D25, "vshLflashFatfmtStartFatfmt" )]
		int vshLflashFatfmtStartFatfmt(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x61001D64, "vshChkregGetPsCode" )]
		int vshChkregGetPsCode(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - E9FC3B93 */
