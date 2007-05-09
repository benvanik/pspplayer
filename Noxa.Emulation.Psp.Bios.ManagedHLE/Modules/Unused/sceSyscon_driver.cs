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
	class sceSyscon_driver : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceSyscon_driver";
			}
		}

		#endregion

		#region State Management

		public sceSyscon_driver( Kernel kernel )
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
		[BiosFunction( 0x0A771482, "sceSysconInit" )]
		int sceSysconInit(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x92D16FC7, "sceSysconEnd" )]
		int sceSysconEnd(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x081826B4, "sceSysconSuspend" )]
		int sceSysconSuspend(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x56931095, "sceSysconResume" )]
		int sceSysconResume(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5EE92F3C, "sceSysconSetDebugHandlers" )]
		int sceSysconSetDebugHandlers(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5FF1D610, "sceSyscon_driver_5FF1D610" )]
		int sceSyscon_driver_5FF1D610(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xAD555CE5, "sceSysconSetLowBatteryCallback" )]
		int sceSysconSetLowBatteryCallback(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF281805D, "sceSysconSetPowerSwitchCallback" )]
		int sceSysconSetPowerSwitchCallback(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA068B3D0, "sceSysconSetAlarmCallback" )]
		int sceSysconSetAlarmCallback(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE540E532, "sceSyscon_driver_E540E532" )]
		int sceSyscon_driver_E540E532(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBBFB70C0, "sceSysconSetHPConnectCallback" )]
		int sceSysconSetHPConnectCallback(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x805180D1, "sceSysconSetHRPowerCallback" )]
		int sceSysconSetHRPowerCallback(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x53072985, "sceSysconSetWlanSwitchCallback" )]
		int sceSysconSetWlanSwitchCallback(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF9193EC3, "sceSysconSetWlanPowerCallback" )]
		int sceSysconSetWlanPowerCallback(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7479DB05, "sceSysconSetHoldSwitchCallback" )]
		int sceSysconSetHoldSwitchCallback(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6848D817, "sceSysconSetUmdSwitchCallback" )]
		int sceSysconSetUmdSwitchCallback(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5B9ACC97, "sceSysconCmdExec" )]
		int sceSysconCmdExec(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3AC3D2A4, "sceSysconCmdExecAsync" )]
		int sceSysconCmdExecAsync(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1602ED0D, "sceSysconCmdCancel" )]
		int sceSysconCmdCancel(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF350F666, "sceSysconCmdSync" )]
		int sceSysconCmdSync(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x86D4CAD8, "sceSyscon_driver_86D4CAD8" )]
		int sceSyscon_driver_86D4CAD8(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x32CFD20F, "sceSysconIsLowBattery" )]
		int sceSysconIsLowBattery(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xEC0DE439, "sceSysconGetPowerSwitch" )]
		int sceSysconGetPowerSwitch(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xEA5B9823, "sceSysconIsAlarmed" )]
		int sceSysconIsAlarmed(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE20D08FE, "sceSyscon_driver_E20D08FE" )]
		int sceSyscon_driver_E20D08FE(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE0DDFE18, "sceSysconGetHPConnect" )]
		int sceSysconGetHPConnect(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBDA16E46, "sceSysconGetWlanSwitch" )]
		int sceSysconGetWlanSwitch(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF6BB4649, "sceSysconGetHoldSwitch" )]
		int sceSysconGetHoldSwitch(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x138747DE, "sceSysconGetUmdSwitch" )]
		int sceSysconGetUmdSwitch(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x71AE1BCE, "sceSysconGetHRPowerStatus" )]
		int sceSysconGetHRPowerStatus(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7016161C, "sceSysconGetWlanPowerStatus" )]
		int sceSysconGetWlanPowerStatus(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x48AB0E44, "sceSysconGetLeptonPowerCtrl" )]
		int sceSysconGetLeptonPowerCtrl(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x628F2351, "sceSysconGetMsPowerCtrl" )]
		int sceSysconGetMsPowerCtrl(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3C739F57, "sceSysconGetHRPowerCtrl" )]
		int sceSysconGetHRPowerCtrl(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xEC37C549, "sceSysconGetWlanPowerCtrl" )]
		int sceSysconGetWlanPowerCtrl(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8DDA4CA6, "sceSyscon_driver_8DDA4CA6" )]
		int sceSyscon_driver_8DDA4CA6(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x52B74B6C, "sceSyscon_driver_52B74B6C" )]
		int sceSyscon_driver_52B74B6C(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1B17D3E3, "sceSyscon_driver_1B17D3E3" )]
		int sceSyscon_driver_1B17D3E3(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5F19C00F, "sceSysconGetLcdPowerCtrl" )]
		int sceSysconGetLcdPowerCtrl(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCC04A978, "sceSysconGetWlanLedCtrl" )]
		int sceSysconGetWlanLedCtrl(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE6B74CB9, "sceSysconNop" )]
		int sceSysconNop(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7EC5A957, "sceSyscon_driver_7EC5A957" )]
		int sceSyscon_driver_7EC5A957(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7BCC5EAE, "sceSyscon_driver_7BCC5EAE" )]
		int sceSyscon_driver_7BCC5EAE(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3B657A27, "sceSyscon_driver_3B657A27" )]
		int sceSyscon_driver_3B657A27(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFC32141A, "sceSysconGetPowerSupplyStatus" )]
		int sceSysconGetPowerSupplyStatus(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF775BC34, "sceSyscon_driver_F775BC34" )]
		int sceSyscon_driver_F775BC34(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA9AEF39F, "sceSyscon_driver_A9AEF39F" )]
		int sceSyscon_driver_A9AEF39F(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC4D66C1D, "sceSysconReadClock" )]
		int sceSysconReadClock(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC7634A7A, "sceSysconWriteClock" )]
		int sceSysconWriteClock(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7A805EE4, "sceSysconReadAlarm" )]
		int sceSysconReadAlarm(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6C911742, "sceSysconWriteAlarm" )]
		int sceSysconWriteAlarm(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x65EB6096, "sceSyscon_driver_65EB6096" )]
		int sceSyscon_driver_65EB6096(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xEB277C88, "sceSyscon_driver_EB277C88" )]
		int sceSyscon_driver_EB277C88(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x992C22C2, "sceSysconSendSetParam" )]
		int sceSysconSendSetParam(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x08234E6D, "sceSysconReceiveSetParam" )]
		int sceSysconReceiveSetParam(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x882F0AAB, "sceSyscon_driver_882F0AAB" )]
		int sceSyscon_driver_882F0AAB(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2EE82492, "sceSyscon_driver_2EE82492" )]
		int sceSyscon_driver_2EE82492(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8CBC7987, "sceSysconResetDevice" )]
		int sceSysconResetDevice(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x00E7B6C2, "sceSyscon_driver_00E7B6C2" )]
		int sceSyscon_driver_00E7B6C2(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x44439604, "sceSysconCtrlHRPower" )]
		int sceSysconCtrlHRPower(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC8439C57, "sceSysconPowerStandby" )]
		int sceSysconPowerStandby(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x91E183CB, "sceSysconPowerSuspend" )]
		int sceSysconPowerSuspend(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE7E87741, "sceSyscon_driver_E7E87741" )]
		int sceSyscon_driver_E7E87741(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFB148FB6, "sceSyscon_driver_FB148FB6" )]
		int sceSyscon_driver_FB148FB6(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x01677F91, "sceSysconCtrlVoltage" )]
		int sceSysconCtrlVoltage(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBE27FE66, "sceSysconCtrlPower" )]
		int sceSysconCtrlPower(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x09721F7F, "sceSysconGetPowerStatus" )]
		int sceSysconGetPowerStatus(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x18BFBE65, "sceSysconCtrlLED" )]
		int sceSysconCtrlLED(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD1B501E8, "sceSyscon_driver_D1B501E8" )]
		int sceSyscon_driver_D1B501E8(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3DE38336, "sceSyscon_driver_3DE38336" )]
		int sceSyscon_driver_3DE38336(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2B9E6A06, "sceSysconGetPowerError" )]
		int sceSysconGetPowerError(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8A4519F5, "sceSysconCtrlLeptonPower" )]
		int sceSysconCtrlLeptonPower(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x99BBB24C, "sceSysconCtrlMsPower" )]
		int sceSysconCtrlMsPower(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF0ED3255, "sceSysconCtrlWlanPower" )]
		int sceSysconCtrlWlanPower(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3C6DB1C5, "sceSyscon_driver_3C6DB1C5" )]
		int sceSyscon_driver_3C6DB1C5(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB2558E37, "sceSyscon_driver_B2558E37" )]
		int sceSyscon_driver_B2558E37(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE5E35721, "sceSyscon_driver_E5E35721" )]
		int sceSyscon_driver_E5E35721(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9478F399, "sceSysconCtrlLcdPower" )]
		int sceSysconCtrlLcdPower(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x806D4D6C, "sceSyscon_driver_806D4D6C" )]
		int sceSyscon_driver_806D4D6C(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD8471760, "sceSyscon_driver_D8471760" )]
		int sceSyscon_driver_D8471760(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xEAB13FBE, "sceSyscon_driver_EAB13FBE" )]
		int sceSyscon_driver_EAB13FBE(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC5075828, "sceSyscon_driver_C5075828" )]
		int sceSyscon_driver_C5075828(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE448FD3F, "sceSysconBatteryNop" )]
		int sceSysconBatteryNop(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6A53F3F8, "sceSysconBatteryGetStatusCap" )]
		int sceSysconBatteryGetStatusCap(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x70C10E61, "sceSysconBatteryGetTemp" )]
		int sceSysconBatteryGetTemp(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8BDEBB1E, "sceSysconBatteryGetVolt" )]
		int sceSysconBatteryGetVolt(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x373EC933, "sceSysconBatteryGetElec" )]
		int sceSysconBatteryGetElec(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x82861DE2, "sceSysconBatteryGetRCap" )]
		int sceSysconBatteryGetRCap(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x876CA580, "sceSysconBatteryGetCap" )]
		int sceSysconBatteryGetCap(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x71135D7D, "sceSysconBatteryGetFullCap" )]
		int sceSysconBatteryGetFullCap(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7CBD4522, "sceSysconBatteryGetIFC" )]
		int sceSysconBatteryGetIFC(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x284FE366, "sceSysconBatteryGetLimitTime" )]
		int sceSysconBatteryGetLimitTime(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x75025299, "sceSysconBatteryGetStatus" )]
		int sceSysconBatteryGetStatus(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB5105D51, "sceSysconBatteryGetCycle" )]
		int sceSysconBatteryGetCycle(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD5340103, "sceSysconBatteryGetSerial" )]
		int sceSysconBatteryGetSerial(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFA4C4518, "sceSysconBatteryGetInfo" )]
		int sceSysconBatteryGetInfo(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB71B98A8, "sceSysconBatteryGetTempAD" )]
		int sceSysconBatteryGetTempAD(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x87671B18, "sceSysconBatteryGetVoltAD" )]
		int sceSysconBatteryGetVoltAD(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x75D22BF8, "sceSysconBatteryGetElecAD" )]
		int sceSysconBatteryGetElecAD(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4C539345, "sceSysconBatteryGetTotalElec" )]
		int sceSysconBatteryGetTotalElec(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4C0EE2FA, "sceSyscon_driver_4C0EE2FA" )]
		int sceSyscon_driver_4C0EE2FA(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1165C864, "sceSyscon_driver_1165C864" )]
		int sceSyscon_driver_1165C864(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x68EF0BEF, "sceSyscon_driver_68EF0BEF" )]
		int sceSyscon_driver_68EF0BEF(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x36E28C5F, "sceSysconBatteryAuth" )]
		int sceSysconBatteryAuth(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x08DA3752, "sceSyscon_driver_08DA3752" )]
		int sceSyscon_driver_08DA3752(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9C4E3CA9, "sceSyscon_driver_9C4E3CA9" )]
		int sceSyscon_driver_9C4E3CA9(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x34C36FF9, "sceSyscon_driver_34C36FF9" )]
		int sceSyscon_driver_34C36FF9(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB8919D79, "sceSysconMsOn" )]
		int sceSysconMsOn(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7BE86143, "sceSysconMsOff" )]
		int sceSysconMsOff(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0E8560F9, "sceSysconWlanOn" )]
		int sceSysconWlanOn(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1B60C8AD, "sceSysconWlanOff" )]
		int sceSysconWlanOff(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE00BFC9E, "sceSyscon_driver_E00BFC9E" )]
		int sceSyscon_driver_E00BFC9E(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC8D97773, "sceSyscon_driver_C8D97773" )]
		int sceSyscon_driver_C8D97773(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - 77CD7C05 */
