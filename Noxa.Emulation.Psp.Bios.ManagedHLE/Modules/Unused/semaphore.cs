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
	class semaphore : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "semaphore";
			}
		}

		#endregion

		#region State Management

		public semaphore( Kernel kernel )
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
		[BiosFunction( 0x00EEC06A, "sceUtilsBufferCopy" )]
		int sceUtilsBufferCopy(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8EEB7BF2, "semaphore_8EEB7BF2" )]
		int semaphore_8EEB7BF2(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4C537C72, "semaphore_4C537C72" )]
		int semaphore_4C537C72(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x77E97079, "semaphore_77E97079" )]
		int semaphore_77E97079(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - 9BF6F363 */
