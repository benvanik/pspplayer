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
	class sceWlanDrv_driver : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceWlanDrv_driver";
			}
		}

		#endregion

		#region State Management

		public sceWlanDrv_driver( Kernel kernel )
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
		[BiosFunction( 0x6D89822C, "sceWlanDevInit" )]
		int sceWlanDevInit(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xEC9232F0, "sceWlanDevEnd" )]
		int sceWlanDevEnd(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x482CAE9A, "sceWlanDevAttach" )]
		int sceWlanDevAttach(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC9A8CAB7, "sceWlanDevDetach" )]
		int sceWlanDevDetach(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x93440B11, "sceWlanDevIsPowerOn" )]
		int sceWlanDevIsPowerOn(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x19E51F54, "sceWlanDrv_driver_19E51F54" )]
		int sceWlanDrv_driver_19E51F54(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5E7C8D94, "sceWlanDevIsGameMode" )]
		int sceWlanDevIsGameMode(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5ED4049A, "sceWlanGPPrevEstablishActive" )]
		int sceWlanGPPrevEstablishActive(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB4D7CB74, "sceWlanGPSend" )]
		int sceWlanGPSend(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA447103A, "sceWlanGPRecv" )]
		int sceWlanGPRecv(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9658C9F7, "sceWlanGPRegisterCallback" )]
		int sceWlanGPRegisterCallback(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4C7F62E0, "sceWlanGPUnRegisterCallback" )]
		int sceWlanGPUnRegisterCallback(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x81579D36, "sceWlanDrv_driver_81579D36" )]
		int sceWlanDrv_driver_81579D36(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5BAA1FE5, "sceWlanDrv_driver_5BAA1FE5" )]
		int sceWlanDrv_driver_5BAA1FE5(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4C14BACA, "sceWlanDrv_driver_4C14BACA" )]
		int sceWlanDrv_driver_4C14BACA(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2D0FAE4E, "sceWlanDrv_driver_2D0FAE4E" )]
		int sceWlanDrv_driver_2D0FAE4E(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x56F467CA, "sceWlanDrv_driver_56F467CA" )]
		int sceWlanDrv_driver_56F467CA(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFE8A0B46, "sceWlanDrv_driver_FE8A0B46" )]
		int sceWlanDrv_driver_FE8A0B46(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x40B0AA4A, "sceWlanDrv_driver_40B0AA4A" )]
		int sceWlanDrv_driver_40B0AA4A(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7FF54BD2, "sceWlanDevSetGPIO" )]
		int sceWlanDevSetGPIO(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x05FE320C, "sceWlanDevGetStateGPIO" )]
		int sceWlanDevGetStateGPIO(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - 70BED68C */
