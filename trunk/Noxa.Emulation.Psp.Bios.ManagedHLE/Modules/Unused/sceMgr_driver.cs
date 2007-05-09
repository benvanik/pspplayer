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
	class sceMgr_driver : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceMgr_driver";
			}
		}

		#endregion

		#region State Management

		public sceMgr_driver( Kernel kernel )
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
		[BiosFunction( 0xECBC76EF, "sceMgrInit" )]
		int sceMgrInit(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDB1160A2, "sceMgrEnd" )]
		int sceMgrEnd(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x604EF590, "sceMgrReset" )]
		int sceMgrReset(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBB36AD19, "sceMgrLock" )]
		int sceMgrLock(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x62BF32DD, "sceMgrPollLock" )]
		int sceMgrPollLock(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDBEDF631, "sceMgrUnLock" )]
		int sceMgrUnLock(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF30E22CF, "sceMgrAllocKeyReg" )]
		int sceMgrAllocKeyReg(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1B8F3192, "sceMgrFreeKeyReg" )]
		int sceMgrFreeKeyReg(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x896EEE57, "sceMgrGetLeafID" )]
		int sceMgrGetLeafID(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x733917D9, "sceMgrGetInitialEKB" )]
		int sceMgrGetInitialEKB(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4B166A14, "sceMgrGetDefaultEKB" )]
		int sceMgrGetDefaultEKB(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x782EB996, "sceMgrInitSeed" )]
		int sceMgrInitSeed(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9ACFEAF6, "sceMgrRandom" )]
		int sceMgrRandom(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4985A5A8, "sceMgrCreateKc" )]
		int sceMgrCreateKc(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x62F480BF, "sceMgrEncryptKcWithKse" )]
		int sceMgrEncryptKcWithKse(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDC515D30, "sceMgrDecryptKcWithKse" )]
		int sceMgrDecryptKcWithKse(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC3677CF4, "sceMgrDecryptEKB" )]
		int sceMgrDecryptEKB(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8407D6DC, "sceMgrDecryptLocalEKB" )]
		int sceMgrDecryptLocalEKB(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB434A376, "sceMgr_driver_B434A376" )]
		int sceMgr_driver_B434A376(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x26EAC6A5, "sceMgrEncryptKc" )]
		int sceMgrEncryptKc(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x02F7AB01, "sceMgrDecryptKc" )]
		int sceMgrDecryptKc(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5576E156, "sceMgrEncryptKicv" )]
		int sceMgrEncryptKicv(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE22F96A0, "sceMgrDecryptKicv" )]
		int sceMgrDecryptKicv(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC408E3C0, "sceMgrGenerateKb" )]
		int sceMgrGenerateKb(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x59894938, "sceMgrGenerateICV" )]
		int sceMgrGenerateICV(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0846B97A, "sceMgr_driver_0846B97A" )]
		int sceMgr_driver_0846B97A(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x34235E6B, "sceMgr_driver_34235E6B" )]
		int sceMgr_driver_34235E6B(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6B908200, "sceMgr_driver_6B908200" )]
		int sceMgr_driver_6B908200(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5A260472, "sceMgrGenerateKse" )]
		int sceMgrGenerateKse(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4AC11456, "sceMgrEncryptWithKse" )]
		int sceMgrEncryptWithKse(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2DA3D191, "sceMgrDecryptWithKse" )]
		int sceMgrDecryptWithKse(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2BC130E3, "sceMgr_driver_2BC130E3" )]
		int sceMgr_driver_2BC130E3(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBDE09225, "sceMgr_driver_BDE09225" )]
		int sceMgr_driver_BDE09225(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x54166582, "sceMgrStartSession" )]
		int sceMgrStartSession(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF5DFD97B, "sceMgrDESDecrypt" )]
		int sceMgrDESDecrypt(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2DAD213D, "sceMgrDESEncrypt" )]
		int sceMgrDESEncrypt(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3583068E, "sceMgrDESDecryptBlock" )]
		int sceMgrDESDecryptBlock(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC507A285, "sceMgrDESEncryptBlock" )]
		int sceMgrDESEncryptBlock(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3054F8F1, "sceMgrAESDecrypt" )]
		int sceMgrAESDecrypt(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8A916574, "sceMgrAESEncrypt" )]
		int sceMgrAESEncrypt(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD46CF098, "sceMgrMSInit" )]
		int sceMgrMSInit(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9C886B02, "sceMgrMSReset" )]
		int sceMgrMSReset(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC9B02F91, "sceMgrMSEnd" )]
		int sceMgrMSEnd(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC655D92B, "sceMgr_driver_C655D92B" )]
		int sceMgr_driver_C655D92B(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF26C410A, "sceMgr_driver_F26C410A" )]
		int sceMgr_driver_F26C410A(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA9F72903, "sceMgr_driver_A9F72903" )]
		int sceMgr_driver_A9F72903(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6CCA97DB, "sceMgr_driver_6CCA97DB" )]
		int sceMgr_driver_6CCA97DB(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA85906FC, "sceMgr_driver_A85906FC" )]
		int sceMgr_driver_A85906FC(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x91C76957, "sceMgrMSMount" )]
		int sceMgrMSMount(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC8380898, "sceMgrMSAuth" )]
		int sceMgrMSAuth(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4F4AE686, "sceMgr_driver_4F4AE686" )]
		int sceMgr_driver_4F4AE686(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x843ECFA2, "sceMgr_driver_843ECFA2" )]
		int sceMgr_driver_843ECFA2(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x839F4A67, "sceMgr_driver_839F4A67" )]
		int sceMgr_driver_839F4A67(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xAC136724, "sceMgr_driver_AC136724" )]
		int sceMgr_driver_AC136724(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x656D8FB1, "sceMgr_driver_656D8FB1" )]
		int sceMgr_driver_656D8FB1(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0ED46FF9, "sceMgr_driver_0ED46FF9" )]
		int sceMgr_driver_0ED46FF9(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x016A843B, "sceMgr_driver_016A843B" )]
		int sceMgr_driver_016A843B(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3EE1140B, "sceMgr_driver_3EE1140B" )]
		int sceMgr_driver_3EE1140B(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD3A061BE, "sceMgr_driver_D3A061BE" )]
		int sceMgr_driver_D3A061BE(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB7F69809, "sceMgr_driver_B7F69809" )]
		int sceMgr_driver_B7F69809(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA01FF50C, "sceMgr_driver_A01FF50C" )]
		int sceMgr_driver_A01FF50C(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7D2AC9FE, "sceMgr_driver_7D2AC9FE" )]
		int sceMgr_driver_7D2AC9FE(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD1023CC2, "sceMgr_driver_D1023CC2" )]
		int sceMgr_driver_D1023CC2(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFD2D2952, "sceMgr_driver_FD2D2952" )]
		int sceMgr_driver_FD2D2952(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE1C0DAFE, "sceMgr_driver_E1C0DAFE" )]
		int sceMgr_driver_E1C0DAFE(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x31B4BD28, "sceMgr_driver_31B4BD28" )]
		int sceMgr_driver_31B4BD28(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x194C594A, "sceMgrGetHwVersion" )]
		int sceMgrGetHwVersion(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - C6E47BE8 */
