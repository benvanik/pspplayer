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
	class InterruptManager : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "InterruptManager";
			}
		}

		#endregion

		#region State Management

		public InterruptManager( Kernel kernel )
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
		[BiosFunction( 0xCA04A2B9, "sceKernelRegisterSubIntrHandler" )]
		// SDK location: /user/pspintrman.h:119
		// SDK declaration: int sceKernelRegisterSubIntrHandler(int intno, int no, void *handler, void *arg);
		int sceKernelRegisterSubIntrHandler( int intno, int no, int handler, int arg ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD61E6961, "sceKernelReleaseSubIntrHandler" )]
		// SDK location: /user/pspintrman.h:129
		// SDK declaration: int sceKernelReleaseSubIntrHandler(int intno, int no);
		int sceKernelReleaseSubIntrHandler( int intno, int no ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFB8E22EC, "sceKernelEnableSubIntr" )]
		// SDK location: /user/pspintrman.h:139
		// SDK declaration: int sceKernelEnableSubIntr(int intno, int no);
		int sceKernelEnableSubIntr( int intno, int no ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8A389411, "sceKernelDisableSubIntr" )]
		// SDK location: /user/pspintrman.h:149
		// SDK declaration: int sceKernelDisableSubIntr(int intno, int no);
		int sceKernelDisableSubIntr( int intno, int no ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD2E8363F, "QueryIntrHandlerInfo" )]
		// SDK location: /user/pspintrman.h:170
		// SDK declaration: int QueryIntrHandlerInfo(SceUID intr_code, SceUID sub_intr_code, PspIntrHandlerOptionParam *data);
		int QueryIntrHandlerInfo( int intr_code, int sub_intr_code, int data ){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - 31D5B3BA */
