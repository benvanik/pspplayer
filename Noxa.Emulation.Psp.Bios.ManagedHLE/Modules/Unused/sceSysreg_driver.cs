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
	class sceSysreg_driver : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceSysreg_driver";
			}
		}

		#endregion

		#region State Management

		public sceSysreg_driver( Kernel kernel )
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
		[BiosFunction( 0x9C863542, "sceSysregInit" )]
		int sceSysregInit(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF2DEAA14, "sceSysregEnd" )]
		int sceSysregEnd(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE88B77ED, "sceSysreg_driver_E88B77ED" )]
		int sceSysreg_driver_E88B77ED(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCD0F6715, "sceSysreg_driver_CD0F6715" )]
		int sceSysreg_driver_CD0F6715(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x844AF6BD, "sceSysreg_driver_844AF6BD" )]
		int sceSysreg_driver_844AF6BD(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE2A5D1EE, "sceSysreg_driver_E2A5D1EE" )]
		int sceSysreg_driver_E2A5D1EE(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4F46EEDE, "sceSysreg_driver_4F46EEDE" )]
		int sceSysreg_driver_4F46EEDE(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8F4F4E96, "sceSysreg_driver_8F4F4E96" )]
		int sceSysreg_driver_8F4F4E96(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC29D614E, "sceSysregTopResetEnable" )]
		int sceSysregTopResetEnable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDC6139A4, "sceSysregScResetEnable" )]
		int sceSysregScResetEnable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDE59DACB, "sceSysregMeResetEnable" )]
		int sceSysregMeResetEnable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2DB0EB28, "sceSysregMeResetDisable" )]
		int sceSysregMeResetDisable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x26283A6F, "sceSysregAwResetEnable" )]
		int sceSysregAwResetEnable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA374195E, "sceSysregAwResetDisable" )]
		int sceSysregAwResetDisable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD20581EA, "sceSysregVmeResetEnable" )]
		int sceSysregVmeResetEnable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7558064A, "sceSysregVmeResetDisable" )]
		int sceSysregVmeResetDisable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9BB70D34, "sceSysregAvcResetEnable" )]
		int sceSysregAvcResetEnable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFD6C562B, "sceSysregAvcResetDisable" )]
		int sceSysregAvcResetDisable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCD4FB614, "sceSysregUsbResetEnable" )]
		int sceSysregUsbResetEnable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x69EECBE5, "sceSysregUsbResetDisable" )]
		int sceSysregUsbResetDisable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF5B80837, "sceSysregAtaResetEnable" )]
		int sceSysregAtaResetEnable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8075303F, "sceSysregAtaResetDisable" )]
		int sceSysregAtaResetDisable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x00C2628E, "sceSysregMsifResetEnable" )]
		int sceSysregMsifResetEnable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xEC4BF81F, "sceSysregMsifResetDisable" )]
		int sceSysregMsifResetDisable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8A7F9EB4, "sceSysregKirkResetEnable" )]
		int sceSysregKirkResetEnable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC32F2491, "sceSysregKirkResetDisable" )]
		int sceSysregKirkResetDisable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB21B6CBF, "sceSysreg_driver_B21B6CBF" )]
		int sceSysreg_driver_B21B6CBF(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBB3623DF, "sceSysreg_driver_BB3623DF" )]
		int sceSysreg_driver_BB3623DF(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x53A6838B, "sceSysreg_driver_53A6838B" )]
		int sceSysreg_driver_53A6838B(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB4560C45, "sceSysreg_driver_B4560C45" )]
		int sceSysreg_driver_B4560C45(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDCA57573, "sceSysreg_driver_DCA57573" )]
		int sceSysreg_driver_DCA57573(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x44F6CDA7, "sceSysregMeBusClockEnable" )]
		int sceSysregMeBusClockEnable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x158AD4FC, "sceSysregMeBusClockDisable" )]
		int sceSysregMeBusClockDisable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4D4CE2B8, "sceSysreg_driver_4D4CE2B8" )]
		int sceSysreg_driver_4D4CE2B8(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x789BD609, "sceSysreg_driver_789BD609" )]
		int sceSysreg_driver_789BD609(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x391CE1C0, "sceSysreg_driver_391CE1C0" )]
		int sceSysreg_driver_391CE1C0(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x82D35024, "sceSysreg_driver_82D35024" )]
		int sceSysreg_driver_82D35024(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xAF904657, "sceSysregAwEdramBusClockEnable" )]
		int sceSysregAwEdramBusClockEnable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x438AECE9, "sceSysregAwEdramBusClockDisable" )]
		int sceSysregAwEdramBusClockDisable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x678AD3ED, "sceSysregDmacplusBusClockEnable" )]
		int sceSysregDmacplusBusClockEnable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x168C09B8, "sceSysregDmacplusBusClockDisable" )]
		int sceSysregDmacplusBusClockDisable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7E7EBC20, "sceSysregDmacBusClockEnable" )]
		int sceSysregDmacBusClockEnable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA265C719, "sceSysregDmacBusClockDisable" )]
		int sceSysregDmacBusClockDisable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4F5AFBBE, "sceSysregKirkBusClockEnable" )]
		int sceSysregKirkBusClockEnable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x845DD1A6, "sceSysregKirkBusClockDisable" )]
		int sceSysregKirkBusClockDisable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x16909002, "sceSysregAtaBusClockEnable" )]
		int sceSysregAtaBusClockEnable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB6C10DF0, "sceSysregAtaBusClockDisable" )]
		int sceSysregAtaBusClockDisable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3E216017, "sceSysregUsbBusClockEnable" )]
		int sceSysregUsbBusClockEnable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBFBABB63, "sceSysregUsbBusClockDisable" )]
		int sceSysregUsbBusClockDisable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4716E71E, "sceSysregMsifBusClockEnable" )]
		int sceSysregMsifBusClockEnable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x826430C0, "sceSysregMsifBusClockDisable" )]
		int sceSysregMsifBusClockDisable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7CC6CBFD, "sceSysregEmcddrBusClockEnable" )]
		int sceSysregEmcddrBusClockEnable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xEE6B9411, "sceSysregEmcddrBusClockDisable" )]
		int sceSysregEmcddrBusClockDisable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF97D9D73, "sceSysregEmcsmBusClockEnable" )]
		int sceSysregEmcsmBusClockEnable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2D0F7755, "sceSysregEmcsmBusClockDisable" )]
		int sceSysregEmcsmBusClockDisable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x63B9EB65, "sceSysregApbBusClockEnable" )]
		int sceSysregApbBusClockEnable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE1AA9788, "sceSysregApbBusClockDisable" )]
		int sceSysregApbBusClockDisable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xAA63C8BD, "sceSysregAudioBusClockEnable" )]
		int sceSysregAudioBusClockEnable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x054AC8C6, "sceSysregAudioBusClockDisable" )]
		int sceSysregAudioBusClockDisable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6B01D71B, "sceSysregAtaClkEnable" )]
		int sceSysregAtaClkEnable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFC186A83, "sceSysregAtaClkDisable" )]
		int sceSysregAtaClkDisable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7234EA80, "sceSysregUsbClkEnable" )]
		int sceSysregUsbClkEnable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x38EC3281, "sceSysregUsbClkDisable" )]
		int sceSysregUsbClkDisable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x31154490, "sceSysregMsifClkEnable" )]
		int sceSysregMsifClkEnable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8E2D835D, "sceSysregMsifClkDisable" )]
		int sceSysregMsifClkDisable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8835D1E1, "sceSysregSpiClkEnable" )]
		int sceSysregSpiClkEnable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8B90B8B5, "sceSysregSpiClkDisable" )]
		int sceSysregSpiClkDisable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7725CA08, "sceSysregUartClkEnable" )]
		int sceSysregUartClkEnable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA3E4154C, "sceSysregUartClkDisable" )]
		int sceSysregUartClkDisable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE8533DCA, "sceSysreg_driver_E8533DCA" )]
		int sceSysreg_driver_E8533DCA(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF6D83AD0, "sceSysreg_driver_F6D83AD0" )]
		int sceSysreg_driver_F6D83AD0(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA9CD1C1F, "sceSysregAudioClkEnable" )]
		int sceSysregAudioClkEnable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2F216F38, "sceSysregAudioClkDisable" )]
		int sceSysregAudioClkDisable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA24C242A, "sceSysregLcdcClkEnable" )]
		int sceSysregLcdcClkEnable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE89243BE, "sceSysregLcdcClkDisable" )]
		int sceSysregLcdcClkDisable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7DCA8302, "sceSysregPwmClkEnable" )]
		int sceSysregPwmClkEnable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x45225F8F, "sceSysregPwmClkDisable" )]
		int sceSysregPwmClkDisable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD74D3AB6, "sceSysregKeyClkEnable" )]
		int sceSysregKeyClkEnable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xAD7C4ACB, "sceSysregKeyClkDisable" )]
		int sceSysregKeyClkDisable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDC68A93F, "sceSysregIicClkEnable" )]
		int sceSysregIicClkEnable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x94B0323C, "sceSysregIicClkDisable" )]
		int sceSysregIicClkDisable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6417CDD6, "sceSysregSircsClkEnable" )]
		int sceSysregSircsClkEnable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x20388C9E, "sceSysregSircsClkDisable" )]
		int sceSysregSircsClkDisable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE3AECFFA, "sceSysregGpioClkEnable" )]
		int sceSysregGpioClkEnable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3BBD0C0C, "sceSysregGpioClkDisable" )]
		int sceSysregGpioClkDisable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC1DA05D2, "sceSysreg_driver_C1DA05D2" )]
		int sceSysreg_driver_C1DA05D2(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDE170397, "sceSysreg_driver_DE170397" )]
		int sceSysreg_driver_DE170397(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1969E840, "sceSysregMsifClkSelect" )]
		int sceSysregMsifClkSelect(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1D382514, "sceSysregMsifDelaySelect" )]
		int sceSysregMsifDelaySelect(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x833E6FB1, "sceSysregAtaClkSelect" )]
		int sceSysregAtaClkSelect(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x03340297, "sceSysreg_driver_03340297" )]
		int sceSysreg_driver_03340297(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9100B4E5, "sceSysregAudioClkSelect" )]
		int sceSysregAudioClkSelect(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0A83FC7B, "sceSysreg_driver_0A83FC7B" )]
		int sceSysreg_driver_0A83FC7B(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD6628A48, "sceSysregSpiClkSelect" )]
		int sceSysregSpiClkSelect(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1E18EA43, "sceSysregLcdcClkSelect" )]
		int sceSysregLcdcClkSelect(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9DD1F821, "sceSysregEmcsmIoEnable" )]
		int sceSysregEmcsmIoEnable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1C4C4C7A, "sceSysregEmcsmIoDisable" )]
		int sceSysregEmcsmIoDisable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBC68D9B6, "sceSysregUsbIoEnable" )]
		int sceSysregUsbIoEnable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA3C8E075, "sceSysregUsbIoDisable" )]
		int sceSysregUsbIoDisable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x79338EA3, "sceSysregAtaIoEnable" )]
		int sceSysregAtaIoEnable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCADB92AA, "sceSysregAtaIoDisable" )]
		int sceSysregAtaIoDisable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD74F1D48, "sceSysregMsifIoEnable" )]
		int sceSysregMsifIoEnable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x18172C6A, "sceSysregMsifIoDisable" )]
		int sceSysregMsifIoDisable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x63B1AADF, "sceSysregLcdcIoEnable" )]
		int sceSysregLcdcIoEnable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF74F14E9, "sceSysregLcdcIoDisable" )]
		int sceSysregLcdcIoDisable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBB26CF1F, "sceSysregAudioIoEnable" )]
		int sceSysregAudioIoEnable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8E2FB536, "sceSysregAudioIoDisable" )]
		int sceSysregAudioIoDisable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0436B60F, "sceSysregIicIoEnable" )]
		int sceSysregIicIoEnable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x58F47EFD, "sceSysregIicIoDisable" )]
		int sceSysregIicIoDisable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4C49A8BC, "sceSysregSircsIoEnable" )]
		int sceSysregSircsIoEnable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x26FA0928, "sceSysregSircsIoDisable" )]
		int sceSysregSircsIoDisable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF844DDF3, "sceSysreg_driver_F844DDF3" )]
		int sceSysreg_driver_F844DDF3(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x29A119A1, "sceSysreg_driver_29A119A1" )]
		int sceSysreg_driver_29A119A1(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x77DED992, "sceSysregKeyIoEnable" )]
		int sceSysregKeyIoEnable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6879790B, "sceSysregKeyIoDisable" )]
		int sceSysregKeyIoDisable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7A5D2D15, "sceSysregPwmIoEnable" )]
		int sceSysregPwmIoEnable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x25B0AC52, "sceSysregPwmIoDisable" )]
		int sceSysregPwmIoDisable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7FD7A631, "sceSysregUartIoEnable" )]
		int sceSysregUartIoEnable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBB823481, "sceSysregUartIoDisable" )]
		int sceSysregUartIoDisable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8C5C53DE, "sceSysregSpiIoEnable" )]
		int sceSysregSpiIoEnable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA16E55F4, "sceSysregSpiIoDisable" )]
		int sceSysregSpiIoDisable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB627582E, "sceSysregGpioIoEnable" )]
		int sceSysregGpioIoEnable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1E9C3607, "sceSysregGpioIoDisable" )]
		int sceSysregGpioIoDisable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x55B18B84, "sceSysreg_driver_55B18B84" )]
		int sceSysreg_driver_55B18B84(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2112E686, "sceSysreg_driver_2112E686" )]
		int sceSysreg_driver_2112E686(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7B9E9A53, "sceSysreg_driver_7B9E9A53" )]
		int sceSysreg_driver_7B9E9A53(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7BDF0556, "sceSysreg_driver_7BDF0556" )]
		int sceSysreg_driver_7BDF0556(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8D0FED1E, "sceSysreg_driver_8D0FED1E" )]
		int sceSysreg_driver_8D0FED1E(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA46E9CA8, "sceSysreg_driver_A46E9CA8" )]
		int sceSysreg_driver_A46E9CA8(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x633595F2, "sceSysreg_driver_633595F2" )]
		int sceSysreg_driver_633595F2(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x32471457, "sceSysregUsbQueryIntr" )]
		int sceSysregUsbQueryIntr(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x692F31FF, "sceSysregUsbAcquireIntr" )]
		int sceSysregUsbAcquireIntr(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD43E98F6, "sceSysreg_driver_D43E98F6" )]
		int sceSysreg_driver_D43E98F6(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBF91FBDA, "sceSysreg_driver_BF91FBDA" )]
		int sceSysreg_driver_BF91FBDA(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x36A75390, "sceSysreg_driver_36A75390" )]
		int sceSysreg_driver_36A75390(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x25673620, "sceSysregIntrInit" )]
		int sceSysregIntrInit(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4EE8E2C8, "sceSysregIntrEnd" )]
		int sceSysregIntrEnd(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x61BFF85F, "sceSysregInterruptToOther" )]
		int sceSysregInterruptToOther(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9FC87ED4, "sceSysregSemaTryLock" )]
		int sceSysregSemaTryLock(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8BE2D520, "sceSysregSemaUnlock" )]
		int sceSysregSemaUnlock(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x083F56E2, "sceSysregEnableIntr" )]
		int sceSysregEnableIntr(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7C5B543C, "sceSysregDisableIntr" )]
		int sceSysregDisableIntr(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3EA188AD, "sceSysregRequestIntr" )]
		int sceSysregRequestIntr(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5664F8B5, "sceSysreg_driver_5664F8B5" )]
		int sceSysreg_driver_5664F8B5(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x44704E1D, "sceSysreg_driver_44704E1D" )]
		int sceSysreg_driver_44704E1D(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x584AD989, "sceSysreg_driver_584AD989" )]
		int sceSysreg_driver_584AD989(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x377F035F, "sceSysreg_driver_377F035F" )]
		int sceSysreg_driver_377F035F(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xAB3185FD, "sceSysreg_driver_AB3185FD" )]
		int sceSysreg_driver_AB3185FD(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0EA487FA, "sceSysreg_driver_0EA487FA" )]
		int sceSysreg_driver_0EA487FA(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x136E8F5A, "sceSysreg_driver_136E8F5A" )]
		int sceSysreg_driver_136E8F5A(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF4811E00, "sceSysreg_driver_F4811E00" )]
		int sceSysreg_driver_F4811E00(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - 9CB2A273 */
