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
	class LoadExecForUser : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "LoadExecForUser";
			}
		}

		#endregion

		#region State Management

		public LoadExecForUser( Kernel kernel )
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
		[BiosFunction( 0xBD2F1094, "sceKernelLoadExec" )]
		// SDK location: /user/psploadexec.h:80
		// SDK declaration: int sceKernelLoadExec(const char *file, struct SceKernelLoadExecParam *param);
		int sceKernelLoadExec( int file, int param ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x05572A5F, "sceKernelExitGame" )]
		// SDK location: /user/psploadexec.h:57
		// SDK declaration: void sceKernelExitGame();
		void sceKernelExitGame(){}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2AC9954B, "sceKernelExitGameWithStatus" )]
		// manual add
		void sceKernelExitGameWithStatus( int status )
		{
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4AC57943, "sceKernelRegisterExitCallback" )]
		// SDK location: /user/psploadexec.h:49
		// SDK declaration: int sceKernelRegisterExitCallback(int cbid);
		int sceKernelRegisterExitCallback( int cbid ){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - 7EE3B66B */
