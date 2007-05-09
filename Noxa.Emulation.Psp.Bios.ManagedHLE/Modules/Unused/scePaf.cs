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
	class scePaf : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "scePaf";
			}
		}

		#endregion

		#region State Management

		public scePaf( Kernel kernel )
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
		[BiosFunction( 0xC9831AFF, "scePaf_C9831AFF" )]
		int scePaf_C9831AFF(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBFE9E90B, "scePaf_BFE9E90B" )]
		int scePaf_BFE9E90B(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5FAC9869, "scePaf_5FAC9869" )]
		int scePaf_5FAC9869(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFCB4E053, "scePaf_FCB4E053" )]
		int scePaf_FCB4E053(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x26DE971C, "scePaf_26DE971C" )]
		int scePaf_26DE971C(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x613E9AA2, "scePaf_613E9AA2" )]
		int scePaf_613E9AA2(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x40C95283, "scePaf_40C95283" )]
		int scePaf_40C95283(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB61E88F2, "scePaf_B61E88F2" )]
		int scePaf_B61E88F2(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x545FE2DA, "scePaf_545FE2DA" )]
		int scePaf_545FE2DA(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7EC15225, "scePaf_7EC15225" )]
		int scePaf_7EC15225(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x60DECA7E, "scePaf_60DECA7E" )]
		int scePaf_60DECA7E(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFD4C9F47, "scePaf_FD4C9F47" )]
		int scePaf_FD4C9F47(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF95EA3F1, "scePaf_F95EA3F1" )]
		int scePaf_F95EA3F1(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6829D7AF, "scePaf_6829D7AF" )]
		int scePaf_6829D7AF(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCA79D58B, "scePaf_CA79D58B" )]
		int scePaf_CA79D58B(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x66FE90D7, "scePaf_66FE90D7" )]
		int scePaf_66FE90D7(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x980228BA, "scePaf_980228BA" )]
		int scePaf_980228BA(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x296897BC, "scePaf_296897BC" )]
		int scePaf_296897BC(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDEDF238F, "scePaf_DEDF238F" )]
		int scePaf_DEDF238F(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7BED034E, "scePaf_7BED034E" )]
		int scePaf_7BED034E(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB3D58D25, "scePaf_B3D58D25" )]
		int scePaf_B3D58D25(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x44AAF96C, "scePaf_44AAF96C" )]
		int scePaf_44AAF96C(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x49A81B18, "scePaf_49A81B18" )]
		int scePaf_49A81B18(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFF2F98C6, "scePaf_FF2F98C6" )]
		int scePaf_FF2F98C6(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x77D981F5, "scePaf_77D981F5" )]
		int scePaf_77D981F5(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x45D851D1, "scePaf_45D851D1" )]
		int scePaf_45D851D1(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x71712601, "scePaf_71712601" )]
		int scePaf_71712601(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x71460F7C, "scePaf_71460F7C" )]
		int scePaf_71460F7C(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xABBBB335, "scePaf_ABBBB335" )]
		int scePaf_ABBBB335(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x07A5F495, "scePaf_07A5F495" )]
		int scePaf_07A5F495(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF1552447, "scePaf_F1552447" )]
		int scePaf_F1552447(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x83944053, "scePaf_83944053" )]
		int scePaf_83944053(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2FDC80B3, "scePaf_2FDC80B3" )]
		int scePaf_2FDC80B3(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x993E9FDC, "scePaf_993E9FDC" )]
		int scePaf_993E9FDC(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3188E7DB, "scePaf_3188E7DB" )]
		int scePaf_3188E7DB(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDC38941B, "scePaf_DC38941B" )]
		int scePaf_DC38941B(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF0B4CAE7, "scePaf_F0B4CAE7" )]
		int scePaf_F0B4CAE7(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6C234A6A, "scePaf_6C234A6A" )]
		int scePaf_6C234A6A(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x37A98AE9, "scePaf_37A98AE9" )]
		int scePaf_37A98AE9(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3DD2A27B, "scePaf_3DD2A27B" )]
		int scePaf_3DD2A27B(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9870A25B, "scePaf_9870A25B" )]
		int scePaf_9870A25B(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x503BA324, "scePaf_503BA324" )]
		int scePaf_503BA324(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2FA84441, "scePaf_2FA84441" )]
		int scePaf_2FA84441(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x84BD418F, "scePaf_84BD418F" )]
		int scePaf_84BD418F(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x902515FB, "scePaf_902515FB" )]
		int scePaf_902515FB(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3586BE05, "scePaf_3586BE05" )]
		int scePaf_3586BE05(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2FA0EDDC, "scePaf_2FA0EDDC" )]
		int scePaf_2FA0EDDC(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3FBD9639, "scePaf_3FBD9639" )]
		int scePaf_3FBD9639(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6BA9C299, "scePaf_6BA9C299" )]
		int scePaf_6BA9C299(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF1B52E86, "scePaf_F1B52E86" )]
		int scePaf_F1B52E86(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x10B901E7, "scePaf_10B901E7" )]
		int scePaf_10B901E7(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4370175A, "scePaf_4370175A" )]
		int scePaf_4370175A(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x809A4F83, "scePaf_809A4F83" )]
		int scePaf_809A4F83(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA82E3C19, "scePaf_A82E3C19" )]
		int scePaf_A82E3C19(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xED2B47FA, "scePaf_ED2B47FA" )]
		int scePaf_ED2B47FA(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x26168DD3, "scePaf_26168DD3" )]
		int scePaf_26168DD3(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x626D68A1, "scePaf_626D68A1" )]
		int scePaf_626D68A1(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFBA47E77, "scePaf_FBA47E77" )]
		int scePaf_FBA47E77(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x44A0BCE4, "scePaf_44A0BCE4" )]
		int scePaf_44A0BCE4(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4B1A374C, "scePaf_4B1A374C" )]
		int scePaf_4B1A374C(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1D5D9A68, "scePaf_1D5D9A68" )]
		int scePaf_1D5D9A68(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x51AAAAF4, "scePaf_51AAAAF4" )]
		int scePaf_51AAAAF4(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x54C0DD23, "scePaf_54C0DD23" )]
		int scePaf_54C0DD23(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9F10613F, "scePaf_9F10613F" )]
		int scePaf_9F10613F(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8F12B63A, "scePaf_8F12B63A" )]
		int scePaf_8F12B63A(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9D0192FD, "scePaf_9D0192FD" )]
		int scePaf_9D0192FD(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFEAFC77A, "scePaf_FEAFC77A" )]
		int scePaf_FEAFC77A(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x99A5CD38, "scePaf_99A5CD38" )]
		int scePaf_99A5CD38(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCE699963, "scePaf_CE699963" )]
		int scePaf_CE699963(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCB2198AB, "scePaf_CB2198AB" )]
		int scePaf_CB2198AB(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x11EF5210, "scePaf_11EF5210" )]
		int scePaf_11EF5210(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB7E6052B, "scePaf_B7E6052B" )]
		int scePaf_B7E6052B(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF13EBB78, "scePaf_F13EBB78" )]
		int scePaf_F13EBB78(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4E67BB08, "scePaf_4E67BB08" )]
		int scePaf_4E67BB08(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB3D2C3BB, "scePaf_B3D2C3BB" )]
		int scePaf_B3D2C3BB(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x962FA087, "scePaf_962FA087" )]
		int scePaf_962FA087(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD4C6CC7A, "scePaf_D4C6CC7A" )]
		int scePaf_D4C6CC7A(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x927854C5, "scePaf_927854C5" )]
		int scePaf_927854C5(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD02C0D2E, "scePaf_D02C0D2E" )]
		int scePaf_D02C0D2E(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2FF0A287, "scePaf_2FF0A287" )]
		int scePaf_2FF0A287(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF47CA1C2, "scePaf_F47CA1C2" )]
		int scePaf_F47CA1C2(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xEAA5AFF6, "scePaf_EAA5AFF6" )]
		int scePaf_EAA5AFF6(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x58BFC2AC, "scePaf_58BFC2AC" )]
		int scePaf_58BFC2AC(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x72514A05, "scePaf_72514A05" )]
		int scePaf_72514A05(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA69BABE3, "scePaf_A69BABE3" )]
		int scePaf_A69BABE3(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9430557B, "scePaf_9430557B" )]
		int scePaf_9430557B(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x607713A6, "scePaf_607713A6" )]
		int scePaf_607713A6(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5EFC5387, "scePaf_5EFC5387" )]
		int scePaf_5EFC5387(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x79C3CBF7, "scePaf_79C3CBF7" )]
		int scePaf_79C3CBF7(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3A4934E3, "scePaf_3A4934E3" )]
		int scePaf_3A4934E3(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4FF65BDC, "scePaf_4FF65BDC" )]
		int scePaf_4FF65BDC(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x26DF747F, "scePaf_26DF747F" )]
		int scePaf_26DF747F(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x41BCFB59, "scePaf_41BCFB59" )]
		int scePaf_41BCFB59(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFA05BF7B, "scePaf_FA05BF7B" )]
		int scePaf_FA05BF7B(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xAAA8D3DA, "scePaf_AAA8D3DA" )]
		int scePaf_AAA8D3DA(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA18DB948, "scePaf_A18DB948" )]
		int scePaf_A18DB948(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2313DDCA, "scePaf_2313DDCA" )]
		int scePaf_2313DDCA(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4FC2470C, "scePaf_4FC2470C" )]
		int scePaf_4FC2470C(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB77039E4, "scePaf_B77039E4" )]
		int scePaf_B77039E4(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0C6EEA21, "scePaf_0C6EEA21" )]
		int scePaf_0C6EEA21(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCB7B666E, "scePaf_CB7B666E" )]
		int scePaf_CB7B666E(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7B6F0E32, "scePaf_7B6F0E32" )]
		int scePaf_7B6F0E32(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x256218C2, "scePaf_256218C2" )]
		int scePaf_256218C2(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x96C46364, "scePaf_96C46364" )]
		int scePaf_96C46364(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7C31F453, "scePaf_7C31F453" )]
		int scePaf_7C31F453(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x503FA456, "scePaf_503FA456" )]
		int scePaf_503FA456(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD5F5E68F, "scePaf_D5F5E68F" )]
		int scePaf_D5F5E68F(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCAE6374A, "scePaf_CAE6374A" )]
		int scePaf_CAE6374A(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x84A71447, "scePaf_84A71447" )]
		int scePaf_84A71447(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD2463C79, "scePaf_D2463C79" )]
		int scePaf_D2463C79(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xED3E4EB1, "scePaf_ED3E4EB1" )]
		int scePaf_ED3E4EB1(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8DB4B066, "scePaf_8DB4B066" )]
		int scePaf_8DB4B066(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB104EC1B, "scePaf_B104EC1B" )]
		int scePaf_B104EC1B(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB3659499, "scePaf_B3659499" )]
		int scePaf_B3659499(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6E94BA7B, "scePaf_6E94BA7B" )]
		int scePaf_6E94BA7B(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFF74C932, "scePaf_FF74C932" )]
		int scePaf_FF74C932(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF6F3E99E, "scePaf_F6F3E99E" )]
		int scePaf_F6F3E99E(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA72594EF, "scePaf_A72594EF" )]
		int scePaf_A72594EF(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5075C799, "scePaf_5075C799" )]
		int scePaf_5075C799(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA76B9604, "scePaf_A76B9604" )]
		int scePaf_A76B9604(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE1C76B22, "scePaf_E1C76B22" )]
		int scePaf_E1C76B22(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x44EB8C62, "scePaf_44EB8C62" )]
		int scePaf_44EB8C62(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC2C25056, "scePaf_C2C25056" )]
		int scePaf_C2C25056(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xAEEF2A7C, "scePaf_AEEF2A7C" )]
		int scePaf_AEEF2A7C(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x55CC4749, "scePaf_55CC4749" )]
		int scePaf_55CC4749(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x37DD37B8, "scePaf_37DD37B8" )]
		int scePaf_37DD37B8(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x16033F84, "scePaf_16033F84" )]
		int scePaf_16033F84(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA0A25707, "scePaf_A0A25707" )]
		int scePaf_A0A25707(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF6AB6655, "scePaf_F6AB6655" )]
		int scePaf_F6AB6655(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC6CF9352, "scePaf_C6CF9352" )]
		int scePaf_C6CF9352(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0A189A1A, "scePaf_0A189A1A" )]
		int scePaf_0A189A1A(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCB7CCB28, "scePaf_CB7CCB28" )]
		int scePaf_CB7CCB28(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7B099943, "scePaf_7B099943" )]
		int scePaf_7B099943(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE9BF6BEB, "scePaf_E9BF6BEB" )]
		int scePaf_E9BF6BEB(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCEAC29F3, "scePaf_CEAC29F3" )]
		int scePaf_CEAC29F3(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x275BCDC6, "scePaf_275BCDC6" )]
		int scePaf_275BCDC6(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x65C19AC0, "scePaf_65C19AC0" )]
		int scePaf_65C19AC0(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8BA04801, "scePaf_8BA04801" )]
		int scePaf_8BA04801(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x93A60B78, "scePaf_93A60B78" )]
		int scePaf_93A60B78(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xAEE1CBAD, "scePaf_AEE1CBAD" )]
		int scePaf_AEE1CBAD(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x19A779E2, "scePaf_19A779E2" )]
		int scePaf_19A779E2(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCDC81771, "scePaf_CDC81771" )]
		int scePaf_CDC81771(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC1FAF826, "scePaf_C1FAF826" )]
		int scePaf_C1FAF826(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9C9B6732, "scePaf_9C9B6732" )]
		int scePaf_9C9B6732(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x78054045, "scePaf_78054045" )]
		int scePaf_78054045(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDF9138A8, "scePaf_DF9138A8" )]
		int scePaf_DF9138A8(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF7FB133E, "scePaf_F7FB133E" )]
		int scePaf_F7FB133E(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCFF2B69C, "scePaf_CFF2B69C" )]
		int scePaf_CFF2B69C(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xEF52945B, "scePaf_EF52945B" )]
		int scePaf_EF52945B(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBAA88CDA, "scePaf_BAA88CDA" )]
		int scePaf_BAA88CDA(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBDC894D8, "scePaf_BDC894D8" )]
		int scePaf_BDC894D8(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5045C01F, "scePaf_5045C01F" )]
		int scePaf_5045C01F(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA89157EE, "scePaf_A89157EE" )]
		int scePaf_A89157EE(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1F0B8678, "scePaf_1F0B8678" )]
		int scePaf_1F0B8678(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x33096E46, "scePaf_33096E46" )]
		int scePaf_33096E46(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x47E35A50, "scePaf_47E35A50" )]
		int scePaf_47E35A50(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x25391E36, "scePaf_25391E36" )]
		int scePaf_25391E36(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x50FFFCD2, "scePaf_50FFFCD2" )]
		int scePaf_50FFFCD2(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x206C1F70, "scePaf_206C1F70" )]
		int scePaf_206C1F70(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0C66B594, "scePaf_0C66B594" )]
		int scePaf_0C66B594(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x39F89900, "scePaf_39F89900" )]
		int scePaf_39F89900(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB6841F37, "scePaf_B6841F37" )]
		int scePaf_B6841F37(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x360956CC, "scePaf_360956CC" )]
		int scePaf_360956CC(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0AEDCDA1, "scePaf_0AEDCDA1" )]
		int scePaf_0AEDCDA1(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x222526B6, "scePaf_222526B6" )]
		int scePaf_222526B6(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x41A7BDD2, "scePaf_41A7BDD2" )]
		int scePaf_41A7BDD2(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD07AF3FA, "scePaf_D07AF3FA" )]
		int scePaf_D07AF3FA(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x29DC646F, "scePaf_29DC646F" )]
		int scePaf_29DC646F(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2A6499A5, "scePaf_2A6499A5" )]
		int scePaf_2A6499A5(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4FCA0DE9, "scePaf_4FCA0DE9" )]
		int scePaf_4FCA0DE9(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE796C85B, "scePaf_E796C85B" )]
		int scePaf_E796C85B(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBAFF6806, "scePaf_BAFF6806" )]
		int scePaf_BAFF6806(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xAA978F61, "scePaf_AA978F61" )]
		int scePaf_AA978F61(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFCF42D6D, "scePaf_FCF42D6D" )]
		int scePaf_FCF42D6D(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5DB527D4, "scePaf_5DB527D4" )]
		int scePaf_5DB527D4(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2E6CA751, "scePaf_2E6CA751" )]
		int scePaf_2E6CA751(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3F52824F, "scePaf_3F52824F" )]
		int scePaf_3F52824F(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1C286704, "scePaf_1C286704" )]
		int scePaf_1C286704(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x672036FE, "scePaf_672036FE" )]
		int scePaf_672036FE(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x08A02EFF, "scePaf_08A02EFF" )]
		int scePaf_08A02EFF(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x01A35C15, "scePaf_01A35C15" )]
		int scePaf_01A35C15(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x470ABD60, "scePaf_470ABD60" )]
		int scePaf_470ABD60(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB42D784E, "scePaf_B42D784E" )]
		int scePaf_B42D784E(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x84485F54, "scePaf_84485F54" )]
		int scePaf_84485F54(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x87516C3B, "scePaf_87516C3B" )]
		int scePaf_87516C3B(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x715E8DAB, "scePaf_715E8DAB" )]
		int scePaf_715E8DAB(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD35B454A, "scePaf_D35B454A" )]
		int scePaf_D35B454A(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCFB1A402, "scePaf_CFB1A402" )]
		int scePaf_CFB1A402(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5D7F1F27, "scePaf_5D7F1F27" )]
		int scePaf_5D7F1F27(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3291BDDD, "scePaf_3291BDDD" )]
		int scePaf_3291BDDD(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x515EE222, "scePaf_515EE222" )]
		int scePaf_515EE222(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3987B5C4, "scePaf_3987B5C4" )]
		int scePaf_3987B5C4(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF89ECA28, "scePaf_F89ECA28" )]
		int scePaf_F89ECA28(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4AF8E417, "scePaf_4AF8E417" )]
		int scePaf_4AF8E417(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x76D5A950, "scePaf_76D5A950" )]
		int scePaf_76D5A950(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF577BC82, "scePaf_F577BC82" )]
		int scePaf_F577BC82(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x369E597A, "scePaf_369E597A" )]
		int scePaf_369E597A(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x293A01D8, "scePaf_293A01D8" )]
		int scePaf_293A01D8(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x45386654, "scePaf_45386654" )]
		int scePaf_45386654(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x73D49419, "scePaf_73D49419" )]
		int scePaf_73D49419(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xAD007AB6, "scePaf_AD007AB6" )]
		int scePaf_AD007AB6(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF49F3C59, "scePaf_F49F3C59" )]
		int scePaf_F49F3C59(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x05EEE8A6, "scePaf_05EEE8A6" )]
		int scePaf_05EEE8A6(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x15EF8E18, "scePaf_15EF8E18" )]
		int scePaf_15EF8E18(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFE84212F, "scePaf_FE84212F" )]
		int scePaf_FE84212F(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9D92765E, "scePaf_9D92765E" )]
		int scePaf_9D92765E(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x98A6C193, "scePaf_98A6C193" )]
		int scePaf_98A6C193(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1943B875, "scePaf_1943B875" )]
		int scePaf_1943B875(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD7074BD1, "scePaf_D7074BD1" )]
		int scePaf_D7074BD1(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBD49CAEF, "scePaf_BD49CAEF" )]
		int scePaf_BD49CAEF(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6C4976AF, "scePaf_6C4976AF" )]
		int scePaf_6C4976AF(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x85112756, "scePaf_85112756" )]
		int scePaf_85112756(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFDB39519, "scePaf_FDB39519" )]
		int scePaf_FDB39519(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDA3A6D88, "scePaf_DA3A6D88" )]
		int scePaf_DA3A6D88(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x199896A3, "scePaf_199896A3" )]
		int scePaf_199896A3(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5458E412, "scePaf_5458E412" )]
		int scePaf_5458E412(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x43374E9E, "scePaf_43374E9E" )]
		int scePaf_43374E9E(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6797988C, "scePaf_6797988C" )]
		int scePaf_6797988C(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x449C8821, "scePaf_449C8821" )]
		int scePaf_449C8821(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6C386158, "scePaf_6C386158" )]
		int scePaf_6C386158(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8E751FCC, "scePaf_8E751FCC" )]
		int scePaf_8E751FCC(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF32223AC, "scePaf_F32223AC" )]
		int scePaf_F32223AC(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x40B21F4F, "scePaf_40B21F4F" )]
		int scePaf_40B21F4F(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x501E9BC9, "scePaf_501E9BC9" )]
		int scePaf_501E9BC9(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4E60E34C, "scePaf_4E60E34C" )]
		int scePaf_4E60E34C(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x39C55C03, "scePaf_39C55C03" )]
		int scePaf_39C55C03(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x65920294, "scePaf_65920294" )]
		int scePaf_65920294(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x37DDBDD8, "scePaf_37DDBDD8" )]
		int scePaf_37DDBDD8(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x92A46B32, "scePaf_92A46B32" )]
		int scePaf_92A46B32(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x905347E9, "scePaf_905347E9" )]
		int scePaf_905347E9(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDF887DED, "scePaf_DF887DED" )]
		int scePaf_DF887DED(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDC544921, "scePaf_DC544921" )]
		int scePaf_DC544921(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x585B93BC, "scePaf_585B93BC" )]
		int scePaf_585B93BC(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1AD33D73, "scePaf_1AD33D73" )]
		int scePaf_1AD33D73(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA4F53FED, "scePaf_A4F53FED" )]
		int scePaf_A4F53FED(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x01CF521E, "scePaf_01CF521E" )]
		int scePaf_01CF521E(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3C68DCB7, "scePaf_3C68DCB7" )]
		int scePaf_3C68DCB7(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3F81333A, "scePaf_3F81333A" )]
		int scePaf_3F81333A(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9A8E343E, "scePaf_9A8E343E" )]
		int scePaf_9A8E343E(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x95040C08, "scePaf_95040C08" )]
		int scePaf_95040C08(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0CFDBE2F, "scePaf_0CFDBE2F" )]
		int scePaf_0CFDBE2F(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3829AC24, "scePaf_3829AC24" )]
		int scePaf_3829AC24(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1F0E9712, "scePaf_1F0E9712" )]
		int scePaf_1F0E9712(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD24E4016, "scePaf_D24E4016" )]
		int scePaf_D24E4016(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x409E25A3, "scePaf_409E25A3" )]
		int scePaf_409E25A3(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDC8B0309, "scePaf_DC8B0309" )]
		int scePaf_DC8B0309(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xEB553D60, "scePaf_EB553D60" )]
		int scePaf_EB553D60(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4EA868E6, "scePaf_4EA868E6" )]
		int scePaf_4EA868E6(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8AB03D91, "scePaf_8AB03D91" )]
		int scePaf_8AB03D91(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5CA7F421, "scePaf_5CA7F421" )]
		int scePaf_5CA7F421(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB7865607, "scePaf_B7865607" )]
		int scePaf_B7865607(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x95948FB6, "scePaf_95948FB6" )]
		int scePaf_95948FB6(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBA433BEF, "scePaf_BA433BEF" )]
		int scePaf_BA433BEF(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xED89C94B, "scePaf_ED89C94B" )]
		int scePaf_ED89C94B(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0E8A3B52, "scePaf_0E8A3B52" )]
		int scePaf_0E8A3B52(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD587826C, "scePaf_D587826C" )]
		int scePaf_D587826C(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x61B105C0, "scePaf_61B105C0" )]
		int scePaf_61B105C0(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDAD477C1, "scePaf_DAD477C1" )]
		int scePaf_DAD477C1(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD91B2651, "scePaf_D91B2651" )]
		int scePaf_D91B2651(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA02E66FA, "scePaf_A02E66FA" )]
		int scePaf_A02E66FA(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2D79697E, "scePaf_2D79697E" )]
		int scePaf_2D79697E(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xEDF1C662, "scePaf_EDF1C662" )]
		int scePaf_EDF1C662(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA7818BCE, "scePaf_A7818BCE" )]
		int scePaf_A7818BCE(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x695BCD34, "scePaf_695BCD34" )]
		int scePaf_695BCD34(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC4D819AA, "scePaf_C4D819AA" )]
		int scePaf_C4D819AA(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x70B92642, "scePaf_70B92642" )]
		int scePaf_70B92642(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x447974D1, "scePaf_447974D1" )]
		int scePaf_447974D1(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x63EA1815, "scePaf_63EA1815" )]
		int scePaf_63EA1815(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCB65103E, "scePaf_CB65103E" )]
		int scePaf_CB65103E(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x32B27ADD, "scePaf_32B27ADD" )]
		int scePaf_32B27ADD(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xADC146F0, "scePaf_ADC146F0" )]
		int scePaf_ADC146F0(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD5A254F9, "scePaf_D5A254F9" )]
		int scePaf_D5A254F9(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x42F43213, "scePaf_42F43213" )]
		int scePaf_42F43213(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5D242D11, "scePaf_5D242D11" )]
		int scePaf_5D242D11(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x11361F12, "scePaf_11361F12" )]
		int scePaf_11361F12(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x51FF9D24, "scePaf_51FF9D24" )]
		int scePaf_51FF9D24(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB2ED6CAA, "scePaf_B2ED6CAA" )]
		int scePaf_B2ED6CAA(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x79F4AE65, "scePaf_79F4AE65" )]
		int scePaf_79F4AE65(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x905CF2A7, "scePaf_905CF2A7" )]
		int scePaf_905CF2A7(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE732CF74, "scePaf_E732CF74" )]
		int scePaf_E732CF74(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB711DC69, "scePaf_B711DC69" )]
		int scePaf_B711DC69(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0ED80B09, "scePaf_0ED80B09" )]
		int scePaf_0ED80B09(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBB457D6F, "scePaf_BB457D6F" )]
		int scePaf_BB457D6F(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB6F52424, "scePaf_B6F52424" )]
		int scePaf_B6F52424(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBA6B8B02, "scePaf_BA6B8B02" )]
		int scePaf_BA6B8B02(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB9BE69FD, "scePaf_B9BE69FD" )]
		int scePaf_B9BE69FD(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFA758B05, "scePaf_FA758B05" )]
		int scePaf_FA758B05(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFA87D725, "scePaf_FA87D725" )]
		int scePaf_FA87D725(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFC37FABD, "scePaf_FC37FABD" )]
		int scePaf_FC37FABD(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x64775957, "scePaf_64775957" )]
		int scePaf_64775957(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7F2442DC, "scePaf_7F2442DC" )]
		int scePaf_7F2442DC(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x86B41BA3, "scePaf_86B41BA3" )]
		int scePaf_86B41BA3(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x541F36E9, "scePaf_541F36E9" )]
		int scePaf_541F36E9(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x86C87E62, "scePaf_86C87E62" )]
		int scePaf_86C87E62(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF1546D14, "scePaf_F1546D14" )]
		int scePaf_F1546D14(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6840D208, "scePaf_6840D208" )]
		int scePaf_6840D208(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD1EE972C, "scePaf_D1EE972C" )]
		int scePaf_D1EE972C(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1232B7A3, "scePaf_1232B7A3" )]
		int scePaf_1232B7A3(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8D7FC7DB, "scePaf_8D7FC7DB" )]
		int scePaf_8D7FC7DB(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5DE0445F, "scePaf_5DE0445F" )]
		int scePaf_5DE0445F(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x09AC6844, "scePaf_09AC6844" )]
		int scePaf_09AC6844(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD2E0D705, "scePaf_D2E0D705" )]
		int scePaf_D2E0D705(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xAE02F241, "scePaf_AE02F241" )]
		int scePaf_AE02F241(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD1159069, "scePaf_D1159069" )]
		int scePaf_D1159069(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBFFAEE83, "scePaf_BFFAEE83" )]
		int scePaf_BFFAEE83(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDDA68BE8, "scePaf_DDA68BE8" )]
		int scePaf_DDA68BE8(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9AFD4AD4, "scePaf_9AFD4AD4" )]
		int scePaf_9AFD4AD4(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6E1B5B50, "scePaf_6E1B5B50" )]
		int scePaf_6E1B5B50(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8DEB3DB3, "scePaf_8DEB3DB3" )]
		int scePaf_8DEB3DB3(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBA09CF46, "scePaf_BA09CF46" )]
		int scePaf_BA09CF46(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6DD28F44, "scePaf_6DD28F44" )]
		int scePaf_6DD28F44(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4ADAEF94, "scePaf_4ADAEF94" )]
		int scePaf_4ADAEF94(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x95708BC0, "scePaf_95708BC0" )]
		int scePaf_95708BC0(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC85155C9, "scePaf_C85155C9" )]
		int scePaf_C85155C9(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDEF9EBE7, "scePaf_DEF9EBE7" )]
		int scePaf_DEF9EBE7(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF23F3226, "scePaf_F23F3226" )]
		int scePaf_F23F3226(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD153385E, "scePaf_D153385E" )]
		int scePaf_D153385E(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x86A5AE7D, "scePaf_86A5AE7D" )]
		int scePaf_86A5AE7D(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5A8D4BDB, "scePaf_5A8D4BDB" )]
		int scePaf_5A8D4BDB(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2120282A, "scePaf_2120282A" )]
		int scePaf_2120282A(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8159EB28, "scePaf_8159EB28" )]
		int scePaf_8159EB28(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5CE11C03, "scePaf_5CE11C03" )]
		int scePaf_5CE11C03(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x05073826, "scePaf_05073826" )]
		int scePaf_05073826(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA4B48F00, "scePaf_A4B48F00" )]
		int scePaf_A4B48F00(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x224C33F0, "scePaf_224C33F0" )]
		int scePaf_224C33F0(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3DBE861F, "scePaf_3DBE861F" )]
		int scePaf_3DBE861F(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFA78AF39, "scePaf_FA78AF39" )]
		int scePaf_FA78AF39(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA3761F27, "scePaf_A3761F27" )]
		int scePaf_A3761F27(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDE75C2BA, "scePaf_DE75C2BA" )]
		int scePaf_DE75C2BA(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4129A915, "scePaf_4129A915" )]
		int scePaf_4129A915(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8A51BA1E, "scePaf_8A51BA1E" )]
		int scePaf_8A51BA1E(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0FFA3ECE, "scePaf_0FFA3ECE" )]
		int scePaf_0FFA3ECE(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x89582FD0, "scePaf_89582FD0" )]
		int scePaf_89582FD0(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x84913A3A, "scePaf_84913A3A" )]
		int scePaf_84913A3A(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x06EA7CDE, "scePaf_06EA7CDE" )]
		int scePaf_06EA7CDE(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9563887D, "scePaf_9563887D" )]
		int scePaf_9563887D(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x21B885B8, "scePaf_21B885B8" )]
		int scePaf_21B885B8(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB0FD305A, "scePaf_B0FD305A" )]
		int scePaf_B0FD305A(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x777635B9, "scePaf_777635B9" )]
		int scePaf_777635B9(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF8301DBC, "scePaf_F8301DBC" )]
		int scePaf_F8301DBC(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5A818E41, "scePaf_5A818E41" )]
		int scePaf_5A818E41(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA9970EF5, "scePaf_A9970EF5" )]
		int scePaf_A9970EF5(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6741C662, "scePaf_6741C662" )]
		int scePaf_6741C662(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x14CE7F6F, "scePaf_14CE7F6F" )]
		int scePaf_14CE7F6F(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDFC8650C, "scePaf_DFC8650C" )]
		int scePaf_DFC8650C(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xECE59E24, "scePaf_ECE59E24" )]
		int scePaf_ECE59E24(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4D810FDC, "scePaf_4D810FDC" )]
		int scePaf_4D810FDC(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xABCBC1C5, "scePaf_ABCBC1C5" )]
		int scePaf_ABCBC1C5(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7C2C2742, "scePaf_7C2C2742" )]
		int scePaf_7C2C2742(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD0AD10FF, "scePaf_D0AD10FF" )]
		int scePaf_D0AD10FF(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2A2EB136, "scePaf_2A2EB136" )]
		int scePaf_2A2EB136(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x20F5234E, "scePaf_20F5234E" )]
		int scePaf_20F5234E(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6F74F263, "scePaf_6F74F263" )]
		int scePaf_6F74F263(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD6232F26, "scePaf_D6232F26" )]
		int scePaf_D6232F26(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xED83B18F, "scePaf_ED83B18F" )]
		int scePaf_ED83B18F(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD6C1FF36, "scePaf_D6C1FF36" )]
		int scePaf_D6C1FF36(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7952AE83, "scePaf_7952AE83" )]
		int scePaf_7952AE83(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFDB98CAC, "scePaf_FDB98CAC" )]
		int scePaf_FDB98CAC(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBA1F20EE, "scePaf_BA1F20EE" )]
		int scePaf_BA1F20EE(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD96449B4, "scePaf_D96449B4" )]
		int scePaf_D96449B4(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x71EDAD83, "scePaf_71EDAD83" )]
		int scePaf_71EDAD83(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2CB6FBBE, "scePaf_2CB6FBBE" )]
		int scePaf_2CB6FBBE(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x442F1731, "scePaf_442F1731" )]
		int scePaf_442F1731(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE418D7E0, "scePaf_E418D7E0" )]
		int scePaf_E418D7E0(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC8FD64AA, "scePaf_C8FD64AA" )]
		int scePaf_C8FD64AA(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDB230BE1, "scePaf_DB230BE1" )]
		int scePaf_DB230BE1(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0FB7D433, "scePaf_0FB7D433" )]
		int scePaf_0FB7D433(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC2E4423A, "scePaf_C2E4423A" )]
		int scePaf_C2E4423A(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA574DB92, "scePaf_A574DB92" )]
		int scePaf_A574DB92(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC6F44EF9, "scePaf_C6F44EF9" )]
		int scePaf_C6F44EF9(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6B678D44, "scePaf_6B678D44" )]
		int scePaf_6B678D44(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE15A64EA, "scePaf_E15A64EA" )]
		int scePaf_E15A64EA(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6A772BF7, "scePaf_6A772BF7" )]
		int scePaf_6A772BF7(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD180ACBA, "scePaf_D180ACBA" )]
		int scePaf_D180ACBA(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4C5F6657, "scePaf_4C5F6657" )]
		int scePaf_4C5F6657(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDFB912CB, "scePaf_DFB912CB" )]
		int scePaf_DFB912CB(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBAA76FDE, "scePaf_BAA76FDE" )]
		int scePaf_BAA76FDE(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6CF44237, "scePaf_6CF44237" )]
		int scePaf_6CF44237(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCFAF88AB, "scePaf_CFAF88AB" )]
		int scePaf_CFAF88AB(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD34D0B82, "scePaf_D34D0B82" )]
		int scePaf_D34D0B82(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8CDEDC6F, "scePaf_8CDEDC6F" )]
		int scePaf_8CDEDC6F(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x22B0D7D8, "scePaf_22B0D7D8" )]
		int scePaf_22B0D7D8(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCA8CDF32, "scePaf_CA8CDF32" )]
		int scePaf_CA8CDF32(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFCABD0E7, "scePaf_FCABD0E7" )]
		int scePaf_FCABD0E7(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xEE10AEF1, "scePaf_EE10AEF1" )]
		int scePaf_EE10AEF1(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD99BEC81, "scePaf_D99BEC81" )]
		int scePaf_D99BEC81(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4D4805AF, "scePaf_4D4805AF" )]
		int scePaf_4D4805AF(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x28F8DE83, "scePaf_28F8DE83" )]
		int scePaf_28F8DE83(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB1F587EF, "scePaf_B1F587EF" )]
		int scePaf_B1F587EF(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8C1ED07B, "scePaf_8C1ED07B" )]
		int scePaf_8C1ED07B(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x04EF78CD, "scePaf_04EF78CD" )]
		int scePaf_04EF78CD(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x75A553A2, "scePaf_75A553A2" )]
		int scePaf_75A553A2(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF4404EAF, "scePaf_F4404EAF" )]
		int scePaf_F4404EAF(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD63D4561, "scePaf_D63D4561" )]
		int scePaf_D63D4561(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x81667058, "scePaf_81667058" )]
		int scePaf_81667058(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x149CDEB9, "scePaf_149CDEB9" )]
		int scePaf_149CDEB9(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x62081F34, "scePaf_62081F34" )]
		int scePaf_62081F34(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBECD1827, "scePaf_BECD1827" )]
		int scePaf_BECD1827(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA8711520, "scePaf_A8711520" )]
		int scePaf_A8711520(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFCDD55E3, "scePaf_FCDD55E3" )]
		int scePaf_FCDD55E3(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2EABF548, "scePaf_2EABF548" )]
		int scePaf_2EABF548(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x50F72192, "scePaf_50F72192" )]
		int scePaf_50F72192(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x73712598, "scePaf_73712598" )]
		int scePaf_73712598(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8B439A23, "scePaf_8B439A23" )]
		int scePaf_8B439A23(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7435C5D5, "scePaf_7435C5D5" )]
		int scePaf_7435C5D5(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCC103AD2, "scePaf_CC103AD2" )]
		int scePaf_CC103AD2(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x566BC690, "scePaf_566BC690" )]
		int scePaf_566BC690(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x68569AAC, "scePaf_68569AAC" )]
		int scePaf_68569AAC(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x76491EEA, "scePaf_76491EEA" )]
		int scePaf_76491EEA(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7771BC86, "scePaf_7771BC86" )]
		int scePaf_7771BC86(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x06BB62B7, "scePaf_06BB62B7" )]
		int scePaf_06BB62B7(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x30F5E7DF, "scePaf_30F5E7DF" )]
		int scePaf_30F5E7DF(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x97B9C21E, "scePaf_97B9C21E" )]
		int scePaf_97B9C21E(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD16E5935, "scePaf_D16E5935" )]
		int scePaf_D16E5935(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9D496842, "scePaf_9D496842" )]
		int scePaf_9D496842(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2750CF68, "scePaf_2750CF68" )]
		int scePaf_2750CF68(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFB3A9617, "scePaf_FB3A9617" )]
		int scePaf_FB3A9617(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x35F003C0, "scePaf_35F003C0" )]
		int scePaf_35F003C0(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xEBDC149C, "scePaf_EBDC149C" )]
		int scePaf_EBDC149C(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x08DD2D50, "scePaf_08DD2D50" )]
		int scePaf_08DD2D50(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x846E1F0A, "scePaf_846E1F0A" )]
		int scePaf_846E1F0A(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4AB0D3F4, "scePaf_4AB0D3F4" )]
		int scePaf_4AB0D3F4(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9122393C, "scePaf_9122393C" )]
		int scePaf_9122393C(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5C4BF618, "scePaf_5C4BF618" )]
		int scePaf_5C4BF618(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x93538664, "scePaf_93538664" )]
		int scePaf_93538664(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x952CE8AF, "scePaf_952CE8AF" )]
		int scePaf_952CE8AF(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x21DBFF5B, "scePaf_21DBFF5B" )]
		int scePaf_21DBFF5B(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x359CBCAF, "scePaf_359CBCAF" )]
		int scePaf_359CBCAF(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3684D457, "scePaf_3684D457" )]
		int scePaf_3684D457(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0598FFD6, "scePaf_0598FFD6" )]
		int scePaf_0598FFD6(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCAF82B9B, "scePaf_CAF82B9B" )]
		int scePaf_CAF82B9B(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5B202AED, "scePaf_5B202AED" )]
		int scePaf_5B202AED(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x50DC7446, "scePaf_50DC7446" )]
		int scePaf_50DC7446(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x36CB5601, "scePaf_36CB5601" )]
		int scePaf_36CB5601(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBA32D6E6, "scePaf_BA32D6E6" )]
		int scePaf_BA32D6E6(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xEAB7BD23, "scePaf_EAB7BD23" )]
		int scePaf_EAB7BD23(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x669DE9B1, "scePaf_669DE9B1" )]
		int scePaf_669DE9B1(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x940D9797, "scePaf_940D9797" )]
		int scePaf_940D9797(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4708AED4, "scePaf_4708AED4" )]
		int scePaf_4708AED4(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x32A70C80, "scePaf_32A70C80" )]
		int scePaf_32A70C80(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCAF05F1D, "scePaf_CAF05F1D" )]
		int scePaf_CAF05F1D(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x49CE42F7, "scePaf_49CE42F7" )]
		int scePaf_49CE42F7(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFC6B7234, "scePaf_FC6B7234" )]
		int scePaf_FC6B7234(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x63B24F4D, "scePaf_63B24F4D" )]
		int scePaf_63B24F4D(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6F33D1DF, "scePaf_6F33D1DF" )]
		int scePaf_6F33D1DF(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3481F595, "scePaf_3481F595" )]
		int scePaf_3481F595(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x986FEBD1, "scePaf_986FEBD1" )]
		int scePaf_986FEBD1(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x458A9C95, "scePaf_458A9C95" )]
		int scePaf_458A9C95(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD3D12890, "scePaf_D3D12890" )]
		int scePaf_D3D12890(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9D1633BC, "scePaf_9D1633BC" )]
		int scePaf_9D1633BC(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x98E1BC87, "scePaf_98E1BC87" )]
		int scePaf_98E1BC87(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x84240C1D, "scePaf_84240C1D" )]
		int scePaf_84240C1D(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5D786CDD, "scePaf_5D786CDD" )]
		int scePaf_5D786CDD(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6E95EFAA, "scePaf_6E95EFAA" )]
		int scePaf_6E95EFAA(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x88D73AED, "scePaf_88D73AED" )]
		int scePaf_88D73AED(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD4FAE7D0, "scePaf_D4FAE7D0" )]
		int scePaf_D4FAE7D0(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x338C3774, "scePaf_338C3774" )]
		int scePaf_338C3774(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDC2CA0F7, "scePaf_DC2CA0F7" )]
		int scePaf_DC2CA0F7(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7F7791A1, "scePaf_7F7791A1" )]
		int scePaf_7F7791A1(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF28A9DB3, "scePaf_F28A9DB3" )]
		int scePaf_F28A9DB3(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBB3CBD19, "scePaf_BB3CBD19" )]
		int scePaf_BB3CBD19(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x27F64616, "scePaf_27F64616" )]
		int scePaf_27F64616(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x72129CFB, "scePaf_72129CFB" )]
		int scePaf_72129CFB(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDAB59652, "scePaf_DAB59652" )]
		int scePaf_DAB59652(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1A45C860, "scePaf_1A45C860" )]
		int scePaf_1A45C860(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD7215544, "scePaf_D7215544" )]
		int scePaf_D7215544(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5DFB5858, "scePaf_5DFB5858" )]
		int scePaf_5DFB5858(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5EBC648F, "scePaf_5EBC648F" )]
		int scePaf_5EBC648F(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF247A0BD, "scePaf_F247A0BD" )]
		int scePaf_F247A0BD(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x910A6B7B, "scePaf_910A6B7B" )]
		int scePaf_910A6B7B(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5470B915, "scePaf_5470B915" )]
		int scePaf_5470B915(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD05C226F, "scePaf_D05C226F" )]
		int scePaf_D05C226F(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCE21D21F, "scePaf_CE21D21F" )]
		int scePaf_CE21D21F(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8860B0FD, "scePaf_8860B0FD" )]
		int scePaf_8860B0FD(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2AA842B0, "scePaf_2AA842B0" )]
		int scePaf_2AA842B0(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD5372E90, "scePaf_D5372E90" )]
		int scePaf_D5372E90(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD7C2E0AD, "scePaf_D7C2E0AD" )]
		int scePaf_D7C2E0AD(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF4AB3180, "scePaf_F4AB3180" )]
		int scePaf_F4AB3180(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xEA026663, "scePaf_EA026663" )]
		int scePaf_EA026663(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF609FA81, "scePaf_F609FA81" )]
		int scePaf_F609FA81(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBBDE1C17, "scePaf_BBDE1C17" )]
		int scePaf_BBDE1C17(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8117E6C8, "scePaf_8117E6C8" )]
		int scePaf_8117E6C8(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1811AD32, "scePaf_1811AD32" )]
		int scePaf_1811AD32(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6CE39EF8, "scePaf_6CE39EF8" )]
		int scePaf_6CE39EF8(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB09203B0, "scePaf_B09203B0" )]
		int scePaf_B09203B0(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x58C69BDA, "scePaf_58C69BDA" )]
		int scePaf_58C69BDA(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE32B3558, "scePaf_E32B3558" )]
		int scePaf_E32B3558(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x86237EB3, "scePaf_86237EB3" )]
		int scePaf_86237EB3(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBEF9FF15, "scePaf_BEF9FF15" )]
		int scePaf_BEF9FF15(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFE4D4112, "scePaf_FE4D4112" )]
		int scePaf_FE4D4112(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDAA609BB, "scePaf_DAA609BB" )]
		int scePaf_DAA609BB(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x69CA1A2D, "scePaf_69CA1A2D" )]
		int scePaf_69CA1A2D(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD3F5956D, "scePaf_D3F5956D" )]
		int scePaf_D3F5956D(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x602EB0CE, "scePaf_602EB0CE" )]
		int scePaf_602EB0CE(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6A14BB04, "scePaf_6A14BB04" )]
		int scePaf_6A14BB04(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x95960A88, "scePaf_95960A88" )]
		int scePaf_95960A88(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD6CD39AF, "scePaf_D6CD39AF" )]
		int scePaf_D6CD39AF(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x79E57CAF, "scePaf_79E57CAF" )]
		int scePaf_79E57CAF(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9DED6118, "scePaf_9DED6118" )]
		int scePaf_9DED6118(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x997D2809, "scePaf_997D2809" )]
		int scePaf_997D2809(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7D83FB20, "scePaf_7D83FB20" )]
		int scePaf_7D83FB20(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE1242646, "scePaf_E1242646" )]
		int scePaf_E1242646(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0F3527DD, "scePaf_0F3527DD" )]
		int scePaf_0F3527DD(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x10C1DB70, "scePaf_10C1DB70" )]
		int scePaf_10C1DB70(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x39B36025, "scePaf_39B36025" )]
		int scePaf_39B36025(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x01C2FC36, "scePaf_01C2FC36" )]
		int scePaf_01C2FC36(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x17308FC9, "scePaf_17308FC9" )]
		int scePaf_17308FC9(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF3967AF5, "scePaf_F3967AF5" )]
		int scePaf_F3967AF5(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x34C973B6, "scePaf_34C973B6" )]
		int scePaf_34C973B6(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4C8C2E25, "scePaf_4C8C2E25" )]
		int scePaf_4C8C2E25(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x81FD71A9, "scePaf_81FD71A9" )]
		int scePaf_81FD71A9(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBBBF145C, "scePaf_BBBF145C" )]
		int scePaf_BBBF145C(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB52E43FB, "scePaf_B52E43FB" )]
		int scePaf_B52E43FB(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x95C97571, "scePaf_95C97571" )]
		int scePaf_95C97571(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF273FE50, "scePaf_F273FE50" )]
		int scePaf_F273FE50(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xAD7A45F9, "scePaf_AD7A45F9" )]
		int scePaf_AD7A45F9(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x67C7567E, "scePaf_67C7567E" )]
		int scePaf_67C7567E(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x56F4D69F, "scePaf_56F4D69F" )]
		int scePaf_56F4D69F(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD7E925A1, "scePaf_D7E925A1" )]
		int scePaf_D7E925A1(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x296ED0EF, "scePaf_296ED0EF" )]
		int scePaf_296ED0EF(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDBD69B7B, "scePaf_DBD69B7B" )]
		int scePaf_DBD69B7B(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7DDE0658, "scePaf_7DDE0658" )]
		int scePaf_7DDE0658(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCC3CFABF, "scePaf_CC3CFABF" )]
		int scePaf_CC3CFABF(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD2C94856, "scePaf_D2C94856" )]
		int scePaf_D2C94856(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xEF346632, "scePaf_EF346632" )]
		int scePaf_EF346632(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x79293604, "scePaf_79293604" )]
		int scePaf_79293604(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xAECF91A9, "scePaf_AECF91A9" )]
		int scePaf_AECF91A9(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3342C5F5, "scePaf_3342C5F5" )]
		int scePaf_3342C5F5(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x48B5C1C0, "scePaf_48B5C1C0" )]
		int scePaf_48B5C1C0(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD188831A, "scePaf_D188831A" )]
		int scePaf_D188831A(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2D08CA09, "scePaf_2D08CA09" )]
		int scePaf_2D08CA09(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x55C7628A, "scePaf_55C7628A" )]
		int scePaf_55C7628A(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA9CE8FBB, "scePaf_A9CE8FBB" )]
		int scePaf_A9CE8FBB(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD6F088EF, "scePaf_D6F088EF" )]
		int scePaf_D6F088EF(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF2E5FDEB, "scePaf_F2E5FDEB" )]
		int scePaf_F2E5FDEB(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6E3E2085, "scePaf_6E3E2085" )]
		int scePaf_6E3E2085(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6676FAA6, "scePaf_6676FAA6" )]
		int scePaf_6676FAA6(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7D9A4824, "scePaf_7D9A4824" )]
		int scePaf_7D9A4824(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD8739C4C, "scePaf_D8739C4C" )]
		int scePaf_D8739C4C(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x34C7D69A, "scePaf_34C7D69A" )]
		int scePaf_34C7D69A(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCDB85442, "scePaf_CDB85442" )]
		int scePaf_CDB85442(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDFD3E855, "scePaf_DFD3E855" )]
		int scePaf_DFD3E855(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2AA80564, "scePaf_2AA80564" )]
		int scePaf_2AA80564(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6BAC756B, "scePaf_6BAC756B" )]
		int scePaf_6BAC756B(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3A844B7B, "scePaf_3A844B7B" )]
		int scePaf_3A844B7B(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9B8CBC5F, "scePaf_9B8CBC5F" )]
		int scePaf_9B8CBC5F(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBE5B0791, "scePaf_BE5B0791" )]
		int scePaf_BE5B0791(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFCD816FA, "scePaf_FCD816FA" )]
		int scePaf_FCD816FA(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x548AA4E7, "scePaf_548AA4E7" )]
		int scePaf_548AA4E7(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x21A0C6E0, "scePaf_21A0C6E0" )]
		int scePaf_21A0C6E0(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC01BF169, "scePaf_C01BF169" )]
		int scePaf_C01BF169(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xAD1721B8, "scePaf_AD1721B8" )]
		int scePaf_AD1721B8(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x910BCFE1, "scePaf_910BCFE1" )]
		int scePaf_910BCFE1(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x75DBA857, "scePaf_75DBA857" )]
		int scePaf_75DBA857(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x849ED670, "scePaf_849ED670" )]
		int scePaf_849ED670(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5148EFE2, "scePaf_5148EFE2" )]
		int scePaf_5148EFE2(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD36F3AAA, "scePaf_D36F3AAA" )]
		int scePaf_D36F3AAA(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1A468B86, "scePaf_1A468B86" )]
		int scePaf_1A468B86(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8E757709, "scePaf_8E757709" )]
		int scePaf_8E757709(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7BBD70F0, "scePaf_7BBD70F0" )]
		int scePaf_7BBD70F0(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDFFE9ED6, "scePaf_DFFE9ED6" )]
		int scePaf_DFFE9ED6(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x72A0044D, "scePaf_72A0044D" )]
		int scePaf_72A0044D(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4612DB4B, "scePaf_4612DB4B" )]
		int scePaf_4612DB4B(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x78219D10, "scePaf_78219D10" )]
		int scePaf_78219D10(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x97FDA760, "scePaf_97FDA760" )]
		int scePaf_97FDA760(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x298246CD, "scePaf_298246CD" )]
		int scePaf_298246CD(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x12C50A9D, "scePaf_12C50A9D" )]
		int scePaf_12C50A9D(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3D451DE8, "scePaf_3D451DE8" )]
		int scePaf_3D451DE8(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8FE88010, "scePaf_8FE88010" )]
		int scePaf_8FE88010(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x21A183A7, "scePaf_21A183A7" )]
		int scePaf_21A183A7(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xABEBF18D, "scePaf_ABEBF18D" )]
		int scePaf_ABEBF18D(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x94136997, "scePaf_94136997" )]
		int scePaf_94136997(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA25BDAA2, "scePaf_A25BDAA2" )]
		int scePaf_A25BDAA2(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9F60CB01, "scePaf_9F60CB01" )]
		int scePaf_9F60CB01(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x50B0C0A1, "scePaf_50B0C0A1" )]
		int scePaf_50B0C0A1(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7E304DCF, "scePaf_7E304DCF" )]
		int scePaf_7E304DCF(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1F43B8A2, "scePaf_1F43B8A2" )]
		int scePaf_1F43B8A2(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x891B7F7F, "scePaf_891B7F7F" )]
		int scePaf_891B7F7F(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x14D80C40, "scePaf_14D80C40" )]
		int scePaf_14D80C40(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x87E5F9C7, "scePaf_87E5F9C7" )]
		int scePaf_87E5F9C7(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD3AB4076, "scePaf_D3AB4076" )]
		int scePaf_D3AB4076(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x537D04C6, "scePaf_537D04C6" )]
		int scePaf_537D04C6(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7AFD6B74, "png_create_info_struct" )]
		int png_create_info_struct(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6528115D, "png_create_read_struct" )]
		int png_create_read_struct(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x211A764A, "png_destroy_read_struct" )]
		int png_destroy_read_struct(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7894B61F, "png_error" )]
		int png_error(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x38F12307, "png_get_IHDR" )]
		int png_get_IHDR(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE6432407, "png_get_PLTE" )]
		int png_get_PLTE(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x07216006, "png_get_tRNS" )]
		int png_get_tRNS(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB2290D1A, "png_read_end" )]
		int png_read_end(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE327C974, "png_read_info" )]
		int png_read_info(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1BDFF258, "png_set_read_fn" )]
		int png_set_read_fn(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDB1549DF, "png_start_read_image" )]
		int png_start_read_image(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDDD91620, "png_read_image" )]
		int png_read_image(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD4F9270D, "png_read_update_info" )]
		int png_read_update_info(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7333E75A, "png_set_packing" )]
		int png_set_packing(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1A1F232F, "png_set_sig_bytes" )]
		int png_set_sig_bytes(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC6624F24, "png_set_packswap" )]
		int png_set_packswap(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFBC76D05, "sce_png_read" )]
		int sce_png_read(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC6A8BEE2, "scePaf_C6A8BEE2" )]
		int scePaf_C6A8BEE2(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8406F469, "scePaf_8406F469" )]
		int scePaf_8406F469(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB4D1CBBF, "scePaf_B4D1CBBF" )]
		int scePaf_B4D1CBBF(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8D3EAEA3, "scePaf_8D3EAEA3" )]
		int scePaf_8D3EAEA3(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC8E7B03F, "scePaf_C8E7B03F" )]
		int scePaf_C8E7B03F(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD4DAB044, "scePaf_D4DAB044" )]
		int scePaf_D4DAB044(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x15939BB8, "scePaf_15939BB8" )]
		int scePaf_15939BB8(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8D736C8F, "scePaf_8D736C8F" )]
		int scePaf_8D736C8F(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE0CF8091, "scePaf_E0CF8091" )]
		int scePaf_E0CF8091(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBEB47224, "scePaf_BEB47224" )]
		int scePaf_BEB47224(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD9392CCB, "scePaf_D9392CCB" )]
		int scePaf_D9392CCB(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF1B73D12, "scePaf_F1B73D12" )]
		int scePaf_F1B73D12(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x00D1378F, "scePaf_00D1378F" )]
		int scePaf_00D1378F(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD6CDB3BB, "scePaf_D6CDB3BB" )]
		int scePaf_D6CDB3BB(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x60CBB63F, "scePaf_60CBB63F" )]
		int scePaf_60CBB63F(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF93D4E1F, "scePaf_F93D4E1F" )]
		int scePaf_F93D4E1F(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB9383A25, "scePaf_B9383A25" )]
		int scePaf_B9383A25(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB7D3C112, "scePaf_B7D3C112" )]
		int scePaf_B7D3C112(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4BDEB2A8, "scePaf_4BDEB2A8" )]
		int scePaf_4BDEB2A8(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x41B724A5, "scePaf_41B724A5" )]
		int scePaf_41B724A5(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD5F3BAB1, "scePaf_D5F3BAB1" )]
		int scePaf_D5F3BAB1(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xEA39A7C1, "scePaf_EA39A7C1" )]
		int scePaf_EA39A7C1(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6B1E90C7, "scePaf_6B1E90C7" )]
		int scePaf_6B1E90C7(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x66907719, "scePaf_66907719" )]
		int scePaf_66907719(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xAAD0EE78, "scePaf_AAD0EE78" )]
		int scePaf_AAD0EE78(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE69C3B1B, "scePaf_E69C3B1B" )]
		int scePaf_E69C3B1B(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE618F7A8, "scePaf_E618F7A8" )]
		int scePaf_E618F7A8(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x78F94D84, "scePaf_78F94D84" )]
		int scePaf_78F94D84(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5822F12A, "scePaf_5822F12A" )]
		int scePaf_5822F12A(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFD9DE7B9, "scePaf_FD9DE7B9" )]
		int scePaf_FD9DE7B9(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xAD43F865, "scePaf_AD43F865" )]
		int scePaf_AD43F865(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x872F948B, "scePaf_872F948B" )]
		int scePaf_872F948B(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD726F4A2, "scePaf_D726F4A2" )]
		int scePaf_D726F4A2(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA044BFC0, "scePaf_A044BFC0" )]
		int scePaf_A044BFC0(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF1CF9579, "scePaf_F1CF9579" )]
		int scePaf_F1CF9579(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x132148FD, "scePaf_132148FD" )]
		int scePaf_132148FD(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x533F3856, "scePaf_533F3856" )]
		int scePaf_533F3856(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x139D9047, "scePaf_139D9047" )]
		int scePaf_139D9047(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8D3B3587, "scePaf_8D3B3587" )]
		int scePaf_8D3B3587(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x27760582, "scePaf_27760582" )]
		int scePaf_27760582(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFD3FF4AA, "scePaf_FD3FF4AA" )]
		int scePaf_FD3FF4AA(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA0ED77C4, "scePaf_A0ED77C4" )]
		int scePaf_A0ED77C4(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x35F25BA1, "scePaf_35F25BA1" )]
		int scePaf_35F25BA1(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x75062BCF, "scePaf_75062BCF" )]
		int scePaf_75062BCF(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x79493F8F, "scePaf_79493F8F" )]
		int scePaf_79493F8F(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3E203AEB, "Heap_Alloc" )]
		int Heap_Alloc(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9948CDB3, "Heap_Free" )]
		int Heap_Free(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF84A99C8, "scePaf_F84A99C8" )]
		int scePaf_F84A99C8(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6D491648, "Heap_Create" )]
		int Heap_Create(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8A861692, "Heap_Destroy" )]
		int Heap_Destroy(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC4BAD564, "scePaf_C4BAD564" )]
		int scePaf_C4BAD564(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD27DE19A, "scePaf_D27DE19A" )]
		int scePaf_D27DE19A(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBAE16A39, "scePaf_BAE16A39" )]
		int scePaf_BAE16A39(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2CFAE498, "scePaf_2CFAE498" )]
		int scePaf_2CFAE498(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x58846DF3, "scePaf_58846DF3" )]
		int scePaf_58846DF3(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE357380E, "scePaf_E357380E" )]
		int scePaf_E357380E(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4B4F39FC, "scePaf_4B4F39FC" )]
		int scePaf_4B4F39FC(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5C5F727C, "scePaf_5C5F727C" )]
		int scePaf_5C5F727C(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0F2615A4, "scePaf_0F2615A4" )]
		int scePaf_0F2615A4(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xEA4C24B3, "scePaf_EA4C24B3" )]
		int scePaf_EA4C24B3(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x28C54317, "scePaf_28C54317" )]
		int scePaf_28C54317(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCBA9AAB1, "scePaf_CBA9AAB1" )]
		int scePaf_CBA9AAB1(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2C5D3E63, "scePaf_2C5D3E63" )]
		int scePaf_2C5D3E63(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x38316A7D, "scePaf_38316A7D" )]
		int scePaf_38316A7D(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x96610AE6, "scePaf_96610AE6" )]
		int scePaf_96610AE6(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA08B0F31, "scePaf_A08B0F31" )]
		int scePaf_A08B0F31(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFB3DB2FF, "paf_fini" )]
		int paf_fini(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE13B6DAE, "sinf" )]
		int sinf(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xEB6D20A5, "cosf" )]
		int cosf(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x09F5941E, "sqrtf" )]
		int sqrtf(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x481C9ADA, "malloc" )]
		int malloc(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xAD8AF84F, "free" )]
		int free(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xAB7592FF, "memcpy" )]
		int memcpy(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x10F3BB61, "memset" )]
		int memset(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x81D0D1F7, "memcmp" )]
		int memcmp(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7EDCC45E, "floorf" )]
		int floorf(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x820C80E6, "acosf" )]
		int acosf(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCAB439DF, "printf" )]
		int printf(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - 4CB51F65 */
