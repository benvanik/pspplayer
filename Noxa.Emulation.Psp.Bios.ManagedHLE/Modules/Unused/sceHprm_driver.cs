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
	class sceHprm_driver : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceHprm_driver";
			}
		}

		#endregion

		#region State Management

		public sceHprm_driver( Kernel kernel )
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
		[BiosFunction( 0x1C5BC5A0, "sceHprmInit" )]
		int sceHprmInit(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x588845DA, "sceHprmEnd" )]
		int sceHprmEnd(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x526BB7F4, "sceHprmSuspend" )]
		int sceHprmSuspend(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2C7B8B05, "sceHprmResume" )]
		int sceHprmResume(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD22913DB, "sceHprmSetConnectCallback" )]
		int sceHprmSetConnectCallback(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC7154136, "sceHprmRegisterCallback" )]
		int sceHprmRegisterCallback(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x444ED0B7, "sceHprmUnregisterCallback" )]
		int sceHprmUnregisterCallback(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x208DB1BD, "sceHprmIsRemoteExist" )]
		int sceHprmIsRemoteExist(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7E69EDA4, "sceHprmIsHeadphoneExist" )]
		int sceHprmIsHeadphoneExist(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x219C58F1, "sceHprmIsMicrophoneExist" )]
		int sceHprmIsMicrophoneExist(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4D1E622C, "sceHprmReset" )]
		int sceHprmReset(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7B038374, "sceHprmGetInternalState" )]
		int sceHprmGetInternalState(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF04591FA, "sceHprm_driver_F04591FA" )]
		int sceHprm_driver_F04591FA(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x971AE8FB, "sceHprm_driver_971AE8FB" )]
		int sceHprm_driver_971AE8FB(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBAD0828E, "sceHprmGetModel" )]
		int sceHprmGetModel(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1910B327, "sceHprmPeekCurrentKey" )]
		int sceHprmPeekCurrentKey(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2BCEC83E, "sceHprmPeekLatch" )]
		int sceHprmPeekLatch(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x40D2F9F0, "sceHprmReadLatch" )]
		int sceHprmReadLatch(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - 5ACD0EB4 */
