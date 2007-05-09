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
	class DmacManForKernel : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "DmacManForKernel";
			}
		}

		#endregion

		#region State Management

		public DmacManForKernel( Kernel kernel )
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
		[BiosFunction( 0x1C46158A, "sceKernelDmaExit" )]
		int sceKernelDmaExit(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD8BC3120, "sceKernelDmaChExclude" )]
		int sceKernelDmaChExclude(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2E3BC333, "sceKernelDmaChReserve" )]
		int sceKernelDmaChReserve(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7B9634E1, "sceKernelDmaSoftRequest" )]
		int sceKernelDmaSoftRequest(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x59615199, "sceKernelDmaOpAlloc" )]
		int sceKernelDmaOpAlloc(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x745E19EF, "sceKernelDmaOpFree" )]
		int sceKernelDmaOpFree(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF64BAB99, "sceKernelDmaOpAssign" )]
		int sceKernelDmaOpAssign(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3BDEA96C, "sceKernelDmaOpEnQueue" )]
		int sceKernelDmaOpEnQueue(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x92700CCD, "sceKernelDmaOpDeQueue" )]
		int sceKernelDmaOpDeQueue(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA84B084B, "sceKernelDmaOpAllCancel" )]
		int sceKernelDmaOpAllCancel(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD0358BE9, "sceKernelDmaOpSetCallback" )]
		int sceKernelDmaOpSetCallback(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3FAD5844, "sceKernelDmaOpSetupMemcpy" )]
		int sceKernelDmaOpSetupMemcpy(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCE467D9B, "sceKernelDmaOpSetupNormal" )]
		int sceKernelDmaOpSetupNormal(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7D21A2EF, "sceKernelDmaOpSetupLink" )]
		int sceKernelDmaOpSetupLink(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDB286D65, "sceKernelDmaOpSync" )]
		int sceKernelDmaOpSync(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5AF32783, "DmacManForKernel_5AF32783" )]
		int DmacManForKernel_5AF32783(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE18A93A5, "DmacManForKernel_E18A93A5" )]
		int DmacManForKernel_E18A93A5(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x904110FC, "sceKernelDmaOpAssignMultiple" )]
		int sceKernelDmaOpAssignMultiple(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD3F62265, "DmacManForKernel_D3F62265" )]
		int DmacManForKernel_D3F62265(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA893DA2C, "sceKernelDmaOpFreeLink" )]
		int sceKernelDmaOpFreeLink(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - 9D22F0AD */
