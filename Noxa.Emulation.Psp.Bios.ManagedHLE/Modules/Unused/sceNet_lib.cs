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
	class sceNet_lib : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceNet_lib";
			}
		}

		#endregion

		#region State Management

		public sceNet_lib( Kernel kernel )
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
		[BiosFunction( 0x3B617AA0, "sceNet_lib_3B617AA0" )]
		int sceNet_lib_3B617AA0(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDB88F458, "sceNet_lib_DB88F458" )]
		int sceNet_lib_DB88F458(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB6FC0A5B, "sceNet_lib_B6FC0A5B" )]
		int sceNet_lib_B6FC0A5B(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC431A214, "sceNet_lib_C431A214" )]
		int sceNet_lib_C431A214(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBFCFEFF6, "sceNet_lib_BFCFEFF6" )]
		int sceNet_lib_BFCFEFF6(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE1F4696F, "sceNet_lib_E1F4696F" )]
		int sceNet_lib_E1F4696F(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5216CBF5, "sceNet_lib_5216CBF5" )]
		int sceNet_lib_5216CBF5(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD2422E4D, "sceNet_lib_D2422E4D" )]
		int sceNet_lib_D2422E4D(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD1BE2CE9, "sceNet_lib_D1BE2CE9" )]
		int sceNet_lib_D1BE2CE9(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xAB7DD9A5, "sceNet_lib_AB7DD9A5" )]
		int sceNet_lib_AB7DD9A5(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x80E1933E, "sceNet_lib_80E1933E" )]
		int sceNet_lib_80E1933E(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7BA3ED91, "sceNet_lib_7BA3ED91" )]
		int sceNet_lib_7BA3ED91(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x03164B12, "sceNet_lib_03164B12" )]
		int sceNet_lib_03164B12(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x389728AB, "sceNet_lib_389728AB" )]
		int sceNet_lib_389728AB(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4BF9E1DE, "sceNet_lib_4BF9E1DE" )]
		int sceNet_lib_4BF9E1DE(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD5B64E37, "sceNet_lib_D5B64E37" )]
		int sceNet_lib_D5B64E37(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDA02F383, "sceNet_lib_DA02F383" )]
		int sceNet_lib_DA02F383(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xAFA11338, "sceNet_lib_AFA11338" )]
		int sceNet_lib_AFA11338(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB20F84F8, "sceNet_lib_B20F84F8" )]
		int sceNet_lib_B20F84F8(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x83FE280A, "sceNet_lib_83FE280A" )]
		int sceNet_lib_83FE280A(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4F8F3808, "sceNet_lib_4F8F3808" )]
		int sceNet_lib_4F8F3808(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x891723D5, "sceNet_lib_891723D5" )]
		int sceNet_lib_891723D5(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0DFF67F9, "sceNet_lib_0DFF67F9" )]
		int sceNet_lib_0DFF67F9(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF355C73B, "sceNet_lib_F355C73B" )]
		int sceNet_lib_F355C73B(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA55C914F, "sceNet_lib_A55C914F" )]
		int sceNet_lib_A55C914F(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0D633F53, "sceNet_lib_0D633F53" )]
		int sceNet_lib_0D633F53(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8D33C11D, "sceNetConfigGetEtherAddr" )]
		int sceNetConfigGetEtherAddr(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x522A971B, "sceNet_lib_522A971B" )]
		int sceNet_lib_522A971B(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1858883D, "sceNetRand" )]
		int sceNetRand(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x75D9985C, "sceNet_lib_75D9985C" )]
		int sceNet_lib_75D9985C(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x25CC373A, "sceNet_lib_25CC373A" )]
		int sceNet_lib_25CC373A(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDCBC596E, "sceNet_lib_DCBC596E" )]
		int sceNet_lib_DCBC596E(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7C86FBA4, "sceNet_lib_7C86FBA4" )]
		int sceNet_lib_7C86FBA4(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA8B6205A, "sceNet_lib_A8B6205A" )]
		int sceNet_lib_A8B6205A(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA93A93E9, "sceNet_lib_A93A93E9" )]
		int sceNet_lib_A93A93E9(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6B294EE4, "sceNet_lib_6B294EE4" )]
		int sceNet_lib_6B294EE4(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x51C209B2, "sceNet_lib_51C209B2" )]
		int sceNet_lib_51C209B2(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC9C97945, "sceNet_lib_C9C97945" )]
		int sceNet_lib_C9C97945(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB8C4A858, "sceNet_lib_B8C4A858" )]
		int sceNet_lib_B8C4A858(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x205E8D17, "sceNet_lib_205E8D17" )]
		int sceNet_lib_205E8D17(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF6DB0A0B, "sceNet_lib_F6DB0A0B" )]
		int sceNet_lib_F6DB0A0B(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7574FDA1, "sceNet_lib_7574FDA1" )]
		int sceNet_lib_7574FDA1(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCA3CF5EB, "sceNet_lib_CA3CF5EB" )]
		int sceNet_lib_CA3CF5EB(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x757085B0, "sceNet_lib_757085B0" )]
		int sceNet_lib_757085B0(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x435843CB, "sceNet_lib_435843CB" )]
		int sceNet_lib_435843CB(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD861EF33, "sceNet_lib_D861EF33" )]
		int sceNet_lib_D861EF33(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBB2B3DDB, "sceNet_lib_BB2B3DDB" )]
		int sceNet_lib_BB2B3DDB(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6D5D42D7, "sceNet_lib_6D5D42D7" )]
		int sceNet_lib_6D5D42D7(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC21E18B2, "sceNet_lib_C21E18B2" )]
		int sceNet_lib_C21E18B2(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x45452B7B, "sceNet_lib_45452B7B" )]
		int sceNet_lib_45452B7B(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x94B44F26, "sceNet_lib_94B44F26" )]
		int sceNet_lib_94B44F26(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x515B2F33, "sceNet_lib_515B2F33" )]
		int sceNet_lib_515B2F33(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6DC71518, "sceNet_lib_6DC71518" )]
		int sceNet_lib_6DC71518(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7C3B86C5, "sceNet_lib_7C3B86C5" )]
		int sceNet_lib_7C3B86C5(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x05D525E4, "sceNet_lib_05D525E4" )]
		int sceNet_lib_05D525E4(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1D10419C, "sceNet_lib_1D10419C" )]
		int sceNet_lib_1D10419C(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC2EC2EEA, "sceNet_lib_C2EC2EEA" )]
		int sceNet_lib_C2EC2EEA(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x710BD467, "sceNet_lib_710BD467" )]
		int sceNet_lib_710BD467(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x701DDDC3, "sceNet_lib_701DDDC3" )]
		int sceNet_lib_701DDDC3(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD5A03BC0, "sceNet_lib_D5A03BC0" )]
		int sceNet_lib_D5A03BC0(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFA6DE6A6, "sceNet_lib_FA6DE6A6" )]
		int sceNet_lib_FA6DE6A6(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xEDB11CB4, "sceNet_lib_EDB11CB4" )]
		int sceNet_lib_EDB11CB4(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8C55B410, "sceNet_lib_8C55B410" )]
		int sceNet_lib_8C55B410(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x13A8B98A, "sceNet_lib_13A8B98A" )]
		int sceNet_lib_13A8B98A(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xEA42B353, "sceNet_lib_EA42B353" )]
		int sceNet_lib_EA42B353(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x45945E8D, "sceNet_lib_45945E8D" )]
		int sceNet_lib_45945E8D(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD60225A3, "sceNet_lib_D60225A3" )]
		int sceNet_lib_D60225A3(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xEB6DE71A, "sceNet_lib_EB6DE71A" )]
		int sceNet_lib_EB6DE71A(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xEDCC871E, "sceNet_lib_EDCC871E" )]
		int sceNet_lib_EDCC871E(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4B2B3416, "sceNet_lib_4B2B3416" )]
		int sceNet_lib_4B2B3416(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2B42872F, "sceNet_lib_2B42872F" )]
		int sceNet_lib_2B42872F(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC4261339, "sceNet_lib_C4261339" )]
		int sceNet_lib_C4261339(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x41FD8B5C, "sceNet_lib_41FD8B5C" )]
		int sceNet_lib_41FD8B5C(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x92633D8D, "sceNet_lib_92633D8D" )]
		int sceNet_lib_92633D8D(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB9C780C7, "sceNet_lib_B9C780C7" )]
		int sceNet_lib_B9C780C7(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB68E1EEA, "sceNet_lib_B68E1EEA" )]
		int sceNet_lib_B68E1EEA(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE155112D, "sceNet_lib_E155112D" )]
		int sceNet_lib_E155112D(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x41621EB0, "sceNet_lib_41621EB0" )]
		int sceNet_lib_41621EB0(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2E005032, "sceNet_lib_2E005032" )]
		int sceNet_lib_2E005032(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x33B230BD, "sceNet_lib_33B230BD" )]
		int sceNet_lib_33B230BD(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x976AB1E9, "sceNet_lib_976AB1E9" )]
		int sceNet_lib_976AB1E9(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4C8FD452, "sceNet_lib_4C8FD452" )]
		int sceNet_lib_4C8FD452(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5ED457BE, "sceNet_lib_5ED457BE" )]
		int sceNet_lib_5ED457BE(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x31F3CDA1, "sceNet_lib_31F3CDA1" )]
		int sceNet_lib_31F3CDA1(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1F94AFD9, "sceNet_lib_1F94AFD9" )]
		int sceNet_lib_1F94AFD9(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0A5A8751, "sceNet_lib_0A5A8751" )]
		int sceNet_lib_0A5A8751(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB3A48B7F, "sceNet_lib_B3A48B7F" )]
		int sceNet_lib_B3A48B7F(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x949F1FBB, "sceNet_lib_949F1FBB" )]
		int sceNet_lib_949F1FBB(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x13672F83, "sceNet_lib_13672F83" )]
		int sceNet_lib_13672F83(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5C7C7381, "sceNet_lib_5C7C7381" )]
		int sceNet_lib_5C7C7381(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x86B6DCD9, "sceNet_lib_86B6DCD9" )]
		int sceNet_lib_86B6DCD9(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7AE91FB4, "sceNet_lib_7AE91FB4" )]
		int sceNet_lib_7AE91FB4(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x572AD6ED, "sceNet_lib_572AD6ED" )]
		int sceNet_lib_572AD6ED(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x87DC7A7E, "sceNet_lib_87DC7A7E" )]
		int sceNet_lib_87DC7A7E(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x991FF86D, "sceNet_lib_991FF86D" )]
		int sceNet_lib_991FF86D(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5505D820, "sceNet_lib_5505D820" )]
		int sceNet_lib_5505D820(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - DF5851FD */
