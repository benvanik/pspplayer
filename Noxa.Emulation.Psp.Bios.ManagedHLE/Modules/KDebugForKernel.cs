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
	class KDebugForKernel : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "KDebugForKernel";
			}
		}

		#endregion

		#region State Management

		public KDebugForKernel( Kernel kernel )
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
		[BiosFunction( 0xE7A3874D, "sceKernelRegisterAssertHandler" )]
		public int sceKernelRegisterAssertHandler(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2FF4E9F9, "sceKernelAssert" )]
		public int sceKernelAssert(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9B868276, "sceKernelGetDebugPutchar" )]
		public int sceKernelGetDebugPutchar(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE146606D, "sceKernelRegisterDebugPutchar" )]
		public int sceKernelRegisterDebugPutchar(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7CEB2C09, "sceKernelRegisterKprintfHandler" )]
		public int sceKernelRegisterKprintfHandler(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x84F370BC, "Kprintf" )]
		public int Kprintf(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5CE9838B, "sceKernelDebugWrite" )]
		public int sceKernelDebugWrite(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x66253C4E, "sceKernelRegisterDebugWrite" )]
		public int sceKernelRegisterDebugWrite(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDBB5597F, "sceKernelDebugRead" )]
		public int sceKernelDebugRead(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE6554FDA, "sceKernelRegisterDebugRead" )]
		public int sceKernelRegisterDebugRead(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB9C643C9, "sceKernelDebugEcho" )]
		public int sceKernelDebugEcho(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7D1C74F0, "sceKernelDebugEchoSet" )]
		public int sceKernelDebugEchoSet(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x24C32559, "KDebugForKernel_24C32559" )]
		public int KDebugForKernel_24C32559(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD636B827, "sceKernelRemoveByDebugSection" )]
		public int sceKernelRemoveByDebugSection(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5282DD5E, "KDebugForKernel_5282DD5E" )]
		public int KDebugForKernel_5282DD5E(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9F8703E4, "KDebugForKernel_9F8703E4" )]
		public int KDebugForKernel_9F8703E4(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x333DCEC7, "KDebugForKernel_333DCEC7" )]
		public int KDebugForKernel_333DCEC7(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE892D9A1, "KDebugForKernel_E892D9A1" )]
		public int KDebugForKernel_E892D9A1(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA126F497, "KDebugForKernel_A126F497" )]
		public int KDebugForKernel_A126F497(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB7251823, "KDebugForKernel_B7251823" )]
		public int KDebugForKernel_B7251823(){ return Module.NotImplementedReturn; }
	}
}

/* GenerateStubsV2: auto-generated - BD92B76A */
