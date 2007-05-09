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
	class sceMeCore_driver : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceMeCore_driver";
			}
		}

		#endregion

		#region State Management

		public sceMeCore_driver( Kernel kernel )
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
		[BiosFunction( 0xD1EA3DFD, "sceMeCore_driver_D1EA3DFD" )]
		int sceMeCore_driver_D1EA3DFD(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3D5F109C, "sceMeCore_driver_3D5F109C" )]
		int sceMeCore_driver_3D5F109C(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x04AFF68E, "sceMeRpcLock" )]
		int sceMeRpcLock(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB97B15D7, "sceMeRpcUnlock" )]
		int sceMeRpcUnlock(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4794C05C, "sceMeEnableFunctions" )]
		int sceMeEnableFunctions(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - D508FE2C */
