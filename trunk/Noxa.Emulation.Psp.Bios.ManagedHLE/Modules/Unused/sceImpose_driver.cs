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
	class sceImpose_driver : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceImpose_driver";
			}
		}

		#endregion

		#region State Management

		public sceImpose_driver( Kernel kernel )
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
		[BiosFunction( 0xBDBC42A6, "sceImposeInit" )]
		int sceImposeInit(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC7E36CC7, "sceImposeEnd" )]
		int sceImposeEnd(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1AEED8FE, "sceImposeSuspend" )]
		int sceImposeSuspend(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x86924032, "sceImposeResume" )]
		int sceImposeResume(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1B6E3400, "sceImposeGetStatus" )]
		int sceImposeGetStatus(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9C8C6C81, "sceImposeSetStatus" )]
		int sceImposeSetStatus(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x531C9778, "sceImposeGetParam" )]
		int sceImposeGetParam(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x810FB7FB, "sceImposeSetParam" )]
		int sceImposeSetParam(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB415FC59, "sceImposeChanges" )]
		int sceImposeChanges(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x68676C0A, "sceImpose_driver_68676C0A" )]
		int sceImpose_driver_68676C0A(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x381BD9E7, "sceImposeHomeButton" )]
		int sceImposeHomeButton(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - 355D24CF */
