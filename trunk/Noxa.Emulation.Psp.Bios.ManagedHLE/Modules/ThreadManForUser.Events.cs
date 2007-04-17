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
		[BiosFunction( 0x55C20A00, "sceKernelCreateEventFlag" )]
		// SDK location: /user/pspthreadman.h:645
		// SDK declaration: SceUID sceKernelCreateEventFlag(const char *name, int attr, int bits, SceKernelEventFlagOptParam *opt);
		public int sceKernelCreateEventFlag( int name, int attr, int bits, int opt )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xEF9E4C70, "sceKernelDeleteEventFlag" )]
		// SDK location: /user/pspthreadman.h:709
		// SDK declaration: int sceKernelDeleteEventFlag(int evid);
		public int sceKernelDeleteEventFlag( int evid )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1FB15A32, "sceKernelSetEventFlag" )]
		// SDK location: /user/pspthreadman.h:655
		// SDK declaration: int sceKernelSetEventFlag(SceUID evid, u32 bits);
		public int sceKernelSetEventFlag( int evid, int bits )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x812346E4, "sceKernelClearEventFlag" )]
		// SDK location: /user/pspthreadman.h:665
		// SDK declaration: int sceKernelClearEventFlag(SceUID evid, u32 bits);
		public int sceKernelClearEventFlag( int evid, int bits )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x402FCF22, "sceKernelWaitEventFlag" )]
		// SDK location: /user/pspthreadman.h:688
		// SDK declaration: int sceKernelWaitEventFlag(int evid, u32 bits, u32 wait, u32 *outBits, SceUInt *timeout);
		public int sceKernelWaitEventFlag( int evid, int bits, int wait, int outBits, int timeout )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x328C546A, "sceKernelWaitEventFlagCB" )]
		// SDK location: /user/pspthreadman.h:700
		// SDK declaration: int sceKernelWaitEventFlagCB(int evid, u32 bits, u32 wait, u32 *outBits, SceUInt *timeout);
		public int sceKernelWaitEventFlagCB( int evid, int bits, int wait, int outBits, int timeout )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x30FD48F0, "sceKernelPollEventFlag" )]
		// SDK location: /user/pspthreadman.h:676
		// SDK declaration: int sceKernelPollEventFlag(int evid, u32 bits, u32 wait, u32 *outBits);
		public int sceKernelPollEventFlag( int evid, int bits, int wait, int outBits )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA66B0120, "sceKernelReferEventFlagStatus" )]
		// SDK location: /user/pspthreadman.h:719
		// SDK declaration: int sceKernelReferEventFlagStatus(SceUID event, SceKernelEventFlagInfo *status);
		public int sceKernelReferEventFlagStatus( int evid, int status )
		{
			return Module.NotImplementedReturn;
		}
	}
}
