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
	class sceVshCommonGui : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceVshCommonGui";
			}
		}

		#endregion

		#region State Management

		public sceVshCommonGui( Kernel kernel )
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
		[BiosFunction( 0x473110B6, "sceVshCommonGui_473110B6" )]
		int sceVshCommonGui_473110B6(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA1C3DDD6, "sceVshCommonGui_A1C3DDD6" )]
		int sceVshCommonGui_A1C3DDD6(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7B52F0E6, "sceVshCommonGui_7B52F0E6" )]
		int sceVshCommonGui_7B52F0E6(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC5B53362, "sceVshCommonGui_C5B53362" )]
		int sceVshCommonGui_C5B53362(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x607D5C82, "sceVshCommonGui_607D5C82" )]
		int sceVshCommonGui_607D5C82(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1CF884A1, "sceVshCommonGui_1CF884A1" )]
		int sceVshCommonGui_1CF884A1(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x80271F41, "sceVshCommonGui_80271F41" )]
		int sceVshCommonGui_80271F41(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD4353F96, "sceVshCommonGui_D4353F96" )]
		int sceVshCommonGui_D4353F96(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4704D5BD, "sceVshCommonGui_4704D5BD" )]
		int sceVshCommonGui_4704D5BD(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x72E652AA, "sceVshCommonGui_72E652AA" )]
		int sceVshCommonGui_72E652AA(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1A6F3382, "sceVshCommonGui_1A6F3382" )]
		int sceVshCommonGui_1A6F3382(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5C161337, "sceVshCommonGui_5C161337" )]
		int sceVshCommonGui_5C161337(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE9930C29, "sceVshCommonGui_E9930C29" )]
		int sceVshCommonGui_E9930C29(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7DB9C54A, "sceVshCommonGui_7DB9C54A" )]
		int sceVshCommonGui_7DB9C54A(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x441D55D7, "sceVshCommonGui_441D55D7" )]
		int sceVshCommonGui_441D55D7(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA0DE7090, "sceVshCommonGui_A0DE7090" )]
		int sceVshCommonGui_A0DE7090(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xEEB86F0E, "sceVshCommonGui_EEB86F0E" )]
		int sceVshCommonGui_EEB86F0E(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x54BC644C, "sceVshCommonGui_54BC644C" )]
		int sceVshCommonGui_54BC644C(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6C81492F, "sceVshCommonGui_6C81492F" )]
		int sceVshCommonGui_6C81492F(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBAA1F762, "sceVshCommonGui_BAA1F762" )]
		int sceVshCommonGui_BAA1F762(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9F196654, "sceVshCommonGui_9F196654" )]
		int sceVshCommonGui_9F196654(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7C7B935B, "sceVshCommonGui_7C7B935B" )]
		int sceVshCommonGui_7C7B935B(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF7A03543, "sceVshCommonGui_F7A03543" )]
		int sceVshCommonGui_F7A03543(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x566AA22F, "sceVshCommonGui_566AA22F" )]
		int sceVshCommonGui_566AA22F(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9FAA1BC2, "sceVshCommonGui_9FAA1BC2" )]
		int sceVshCommonGui_9FAA1BC2(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4016357A, "sceVshCommonGui_4016357A" )]
		int sceVshCommonGui_4016357A(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x01B60A5C, "sceVshCommonGui_01B60A5C" )]
		int sceVshCommonGui_01B60A5C(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5D36BCE6, "sceVshCommonGui_5D36BCE6" )]
		int sceVshCommonGui_5D36BCE6(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x61A1FA3A, "sceVshCommonGui_61A1FA3A" )]
		int sceVshCommonGui_61A1FA3A(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1BCCB74A, "sceVshCommonGui_1BCCB74A" )]
		int sceVshCommonGui_1BCCB74A(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFA9B7ABD, "sceVshCommonGui_FA9B7ABD" )]
		int sceVshCommonGui_FA9B7ABD(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB97EAE7A, "sceVshCommonGui_B97EAE7A" )]
		int sceVshCommonGui_B97EAE7A(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC4D809CB, "sceVshCommonGui_C4D809CB" )]
		int sceVshCommonGui_C4D809CB(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x72C75A14, "sceVshCommonGui_72C75A14" )]
		int sceVshCommonGui_72C75A14(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5074C948, "sceVshCommonGui_5074C948" )]
		int sceVshCommonGui_5074C948(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF4FD56D2, "sceVshCommonGui_F4FD56D2" )]
		int sceVshCommonGui_F4FD56D2(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x16DA723A, "sceVshCommonGui_16DA723A" )]
		int sceVshCommonGui_16DA723A(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x35D57A35, "sceVshCommonGui_35D57A35" )]
		int sceVshCommonGui_35D57A35(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x35E7A91E, "sceVshCommonGui_35E7A91E" )]
		int sceVshCommonGui_35E7A91E(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x57F57F40, "sceVshCommonGui_57F57F40" )]
		int sceVshCommonGui_57F57F40(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8A029A78, "sceVshCommonGui_8A029A78" )]
		int sceVshCommonGui_8A029A78(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA939DDF9, "sceVshCommonGui_A939DDF9" )]
		int sceVshCommonGui_A939DDF9(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x071749CE, "sceVshCommonGui_071749CE" )]
		int sceVshCommonGui_071749CE(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xACFE7BE7, "sceVshCommonGui_ACFE7BE7" )]
		int sceVshCommonGui_ACFE7BE7(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA06734FF, "sceVshCommonGui_A06734FF" )]
		int sceVshCommonGui_A06734FF(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xADB52779, "sceVshCommonGui_ADB52779" )]
		int sceVshCommonGui_ADB52779(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFE305D6D, "sceVshCommonGui_FE305D6D" )]
		int sceVshCommonGui_FE305D6D(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x891E0954, "sceVshCommonGui_891E0954" )]
		int sceVshCommonGui_891E0954(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD27E08EE, "sceVshCommonGui_D27E08EE" )]
		int sceVshCommonGui_D27E08EE(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9C50A821, "sceVshCommonGui_9C50A821" )]
		int sceVshCommonGui_9C50A821(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8EB783F5, "sceVshCommonGui_8EB783F5" )]
		int sceVshCommonGui_8EB783F5(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCC7BFDB1, "sceVshCommonGui_CC7BFDB1" )]
		int sceVshCommonGui_CC7BFDB1(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFA1DE461, "sceVshCommonGui_FA1DE461" )]
		int sceVshCommonGui_FA1DE461(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x88F5E3A8, "sceVshCommonGui_88F5E3A8" )]
		int sceVshCommonGui_88F5E3A8(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x25D6E91D, "sceVshCommonGui_25D6E91D" )]
		int sceVshCommonGui_25D6E91D(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x68778E5A, "sceVshCommonGui_68778E5A" )]
		int sceVshCommonGui_68778E5A(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - E0EA77FC */
