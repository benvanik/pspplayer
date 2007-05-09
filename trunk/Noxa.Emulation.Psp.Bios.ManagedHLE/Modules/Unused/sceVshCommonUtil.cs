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
	class sceVshCommonUtil : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceVshCommonUtil";
			}
		}

		#endregion

		#region State Management

		public sceVshCommonUtil( Kernel kernel )
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
		[BiosFunction( 0x7A1C6D74, "sceVshCommonUtil_7A1C6D74" )]
		int sceVshCommonUtil_7A1C6D74(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFC8E3CEF, "sceVshCommonUtil_FC8E3CEF" )]
		int sceVshCommonUtil_FC8E3CEF(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBFC546B6, "sceVshCommonUtil_BFC546B6" )]
		int sceVshCommonUtil_BFC546B6(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7EEFA381, "sceVshCommonUtil_7EEFA381" )]
		int sceVshCommonUtil_7EEFA381(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5A3449D5, "sceVshCommonUtil_5A3449D5" )]
		int sceVshCommonUtil_5A3449D5(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0713D516, "sceVshCommonUtil_0713D516" )]
		int sceVshCommonUtil_0713D516(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x12F5429A, "sceVshCommonUtil_12F5429A" )]
		int sceVshCommonUtil_12F5429A(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3FAF35EC, "sceVshCommonUtil_3FAF35EC" )]
		int sceVshCommonUtil_3FAF35EC(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0AF412FC, "sceVshCommonUtil_0AF412FC" )]
		int sceVshCommonUtil_0AF412FC(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x686AAA1A, "sceVshCommonUtil_686AAA1A" )]
		int sceVshCommonUtil_686AAA1A(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x249059B5, "sceVshCommonUtil_249059B5" )]
		int sceVshCommonUtil_249059B5(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7683BE4D, "sceVshCommonUtil_7683BE4D" )]
		int sceVshCommonUtil_7683BE4D(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5A0B7762, "sceVshCommonUtil_5A0B7762" )]
		int sceVshCommonUtil_5A0B7762(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB296D591, "sceVshCommonUtil_B296D591" )]
		int sceVshCommonUtil_B296D591(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC403411C, "vshRegSysconfInitRegistry" )]
		int vshRegSysconfInitRegistry(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB48D135B, "sceVshCommonUtil_B48D135B" )]
		int sceVshCommonUtil_B48D135B(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x63FFE887, "sceVshCommonUtil_63FFE887" )]
		int sceVshCommonUtil_63FFE887(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x28B6AD3B, "sceVshCommonUtil_28B6AD3B" )]
		int sceVshCommonUtil_28B6AD3B(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2ACAD845, "sceVshCommonUtil_2ACAD845" )]
		int sceVshCommonUtil_2ACAD845(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x98E57F6D, "sceVshCommonUtil_98E57F6D" )]
		int sceVshCommonUtil_98E57F6D(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3088B748, "sceVshCommonUtil_3088B748" )]
		int sceVshCommonUtil_3088B748(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xEE5869D8, "sceVshCommonUtil_EE5869D8" )]
		int sceVshCommonUtil_EE5869D8(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9E7D761E, "sceVshCommonUtil_9E7D761E" )]
		int sceVshCommonUtil_9E7D761E(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0F553311, "sceVshCommonUtil_0F553311" )]
		int sceVshCommonUtil_0F553311(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x15AF8184, "sceSystemFileAddIndexW" )]
		int sceSystemFileAddIndexW(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x27627379, "sceSystemFileCalcSizeW" )]
		int sceSystemFileCalcSizeW(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xEDED68FB, "sceSystemFileFree" )]
		int sceSystemFileFree(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2D564AF6, "sceSystemFileGetIndex" )]
		int sceSystemFileGetIndex(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6B332472, "sceSystemFileGetIndexInfo" )]
		int sceSystemFileGetIndexInfo(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF857F095, "sceSystemFileGetValue" )]
		int sceSystemFileGetValue(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9A2C3824, "sceSystemFileGetValuePtr" )]
		int sceSystemFileGetValuePtr(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x46931A6C, "sceSystemFileInitFormatW" )]
		int sceSystemFileInitFormatW(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB6E7804E, "sceSystemFileLoadAll" )]
		int sceSystemFileLoadAll(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE55CADD9, "sceSystemFileLoadIndex" )]
		int sceSystemFileLoadIndex(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x024CEB82, "sceVshCommonUtil_024CEB82" )]
		int sceVshCommonUtil_024CEB82(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3412B578, "sceVshCommonUtil_3412B578" )]
		int sceVshCommonUtil_3412B578(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3E95607D, "sceVshCommonUtil_3E95607D" )]
		int sceVshCommonUtil_3E95607D(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x53CF01CA, "sceVshCommonUtil_53CF01CA" )]
		int sceVshCommonUtil_53CF01CA(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8727574A, "sceVshCommonUtil_8727574A" )]
		int sceVshCommonUtil_8727574A(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x36BA6A54, "sceVshCommonUtil_36BA6A54" )]
		int sceVshCommonUtil_36BA6A54(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x79FF29A4, "sceVshCommonUtil_79FF29A4" )]
		int sceVshCommonUtil_79FF29A4(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9845E69B, "sceVshCommonUtil_9845E69B" )]
		int sceVshCommonUtil_9845E69B(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBA7237F4, "sceVshCommonUtil_BA7237F4" )]
		int sceVshCommonUtil_BA7237F4(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x29278272, "sceVshCommonUtil_29278272" )]
		int sceVshCommonUtil_29278272(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCFB4A59E, "sceVshCommonUtil_CFB4A59E" )]
		int sceVshCommonUtil_CFB4A59E(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1815FB1F, "sceVshCommonUtil_1815FB1F" )]
		int sceVshCommonUtil_1815FB1F(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x53120F49, "sceVshCommonUtil_53120F49" )]
		int sceVshCommonUtil_53120F49(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDEB009C6, "sceVshCommonUtil_DEB009C6" )]
		int sceVshCommonUtil_DEB009C6(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD9E2759B, "sceVshCommonUtil_D9E2759B" )]
		int sceVshCommonUtil_D9E2759B(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x75477C22, "sceVshCommonUtil_75477C22" )]
		int sceVshCommonUtil_75477C22(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFA969B6A, "sceVshCommonUtil_FA969B6A" )]
		int sceVshCommonUtil_FA969B6A(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD4D66154, "sceVshCommonUtil_D4D66154" )]
		int sceVshCommonUtil_D4D66154(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9DF53F2F, "sceVshCommonUtil_9DF53F2F" )]
		int sceVshCommonUtil_9DF53F2F(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x107EDCD9, "sceVshCommonUtil_107EDCD9" )]
		int sceVshCommonUtil_107EDCD9(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x58234CB0, "vshRegSysconfUpdateRegistry" )]
		int vshRegSysconfUpdateRegistry(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - B2C71663 */
