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
	class InitForKernel : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "InitForKernel";
			}
		}

		#endregion

		#region State Management

		public InitForKernel( Kernel kernel )
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
		[BiosFunction( 0x2C6E9FE9, "sceKernelGetChunk" )]
		int sceKernelGetChunk(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1D3256BA, "sceKernelRegisterChunk" )]
		int sceKernelRegisterChunk(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCE88E870, "sceKernelReleaseChunk" )]
		int sceKernelReleaseChunk(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7A2333AD, "sceKernelInitApitype" )]
		int sceKernelInitApitype(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x27932388, "sceKernelBootFrom" )]
		int sceKernelBootFrom(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA6E71B93, "sceKernelInitFileName" )]
		int sceKernelInitFileName(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - F4C70C19 */
