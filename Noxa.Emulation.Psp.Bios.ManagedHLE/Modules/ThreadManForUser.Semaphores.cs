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
	partial class ThreadManForUser
	{
		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD6DA4BA1, "sceKernelCreateSema" )]
		// SDK location: /user/pspthreadman.h:515
		// SDK declaration: SceUID sceKernelCreateSema(const char *name, SceUInt attr, int initVal, int maxVal, SceKernelSemaOptParam *option);
		public int sceKernelCreateSema( int name, int attr, int initVal, int maxVal, int option )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x28B6489C, "sceKernelDeleteSema" )]
		// SDK location: /user/pspthreadman.h:523
		// SDK declaration: int sceKernelDeleteSema(SceUID semaid);
		public int sceKernelDeleteSema( int semaid )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3F53E640, "sceKernelSignalSema" )]
		// SDK location: /user/pspthreadman.h:539
		// SDK declaration: int sceKernelSignalSema(SceUID semaid, int signal);
		public int sceKernelSignalSema( int semaid, int signal )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4E3A1105, "sceKernelWaitSema" )]
		// SDK location: /user/pspthreadman.h:555
		// SDK declaration: int sceKernelWaitSema(SceUID semaid, int signal, SceUInt *timeout);
		public int sceKernelWaitSema( int semaid, int signal, int timeout )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6D212BAC, "sceKernelWaitSemaCB" )]
		// SDK location: /user/pspthreadman.h:571
		// SDK declaration: int sceKernelWaitSemaCB(SceUID semaid, int signal, SceUInt *timeout);
		public int sceKernelWaitSemaCB( int semaid, int signal, int timeout )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x58B1F937, "sceKernelPollSema" )]
		// SDK location: /user/pspthreadman.h:581
		// SDK declaration: int sceKernelPollSema(SceUID semaid, int signal);
		public int sceKernelPollSema( int semaid, int signal )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBC6FEBC5, "sceKernelReferSemaStatus" )]
		// SDK location: /user/pspthreadman.h:591
		// SDK declaration: int sceKernelReferSemaStatus(SceUID semaid, SceKernelSemaInfo *info);
		public int sceKernelReferSemaStatus( int semaid, int info )
		{
			return Module.NotImplementedReturn;
		}
	}
}
