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
	class Uart4ForKernel : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "Uart4ForKernel";
			}
		}

		#endregion

		#region State Management

		public Uart4ForKernel( Kernel kernel )
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
		[BiosFunction( 0xD00E3D06, "sceKernelUart4Init" )]
		int sceKernelUart4Init(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA68555F7, "sceKernelUart4SetBps" )]
		int sceKernelUart4SetBps(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC703109F, "sceKernelUart4PostChar" )]
		int sceKernelUart4PostChar(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC0BE3133, "sceKernelUart4SendChar" )]
		int sceKernelUart4SendChar(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD91079E3, "sceKernelUart4SendBytes" )]
		int sceKernelUart4SendBytes(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x69BCBEF1, "sceKernelUart4SendCharFix" )]
		int sceKernelUart4SendCharFix(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2518C2F3, "sceKernelUart4SendBytesFix" )]
		int sceKernelUart4SendBytesFix(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2FD96F62, "sceKernelUart4ReceiveBytesFix" )]
		int sceKernelUart4ReceiveBytesFix(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - A7F11E6E */
