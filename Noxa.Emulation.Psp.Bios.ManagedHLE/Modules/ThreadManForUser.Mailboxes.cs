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
		[BiosFunction( 0x8125221D, "sceKernelCreateMbx" )]
		// SDK location: /user/pspthreadman.h:774
		// SDK declaration: SceUID sceKernelCreateMbx(const char *name, SceUInt attr, SceKernelMbxOptParam *option);
		public int sceKernelCreateMbx( int name, int attr, int option )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x86255ADA, "sceKernelDeleteMbx" )]
		// SDK location: /user/pspthreadman.h:782
		// SDK declaration: int sceKernelDeleteMbx(SceUID mbxid);
		public int sceKernelDeleteMbx( int mbxid )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE9B3061E, "sceKernelSendMbx" )]
		// SDK location: /user/pspthreadman.h:806
		// SDK declaration: int sceKernelSendMbx(SceUID mbxid, void *message);
		public int sceKernelSendMbx( int mbxid, int message )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x18260574, "sceKernelReceiveMbx" )]
		// SDK location: /user/pspthreadman.h:824
		// SDK declaration: int sceKernelReceiveMbx(SceUID mbxid, void **pmessage, SceUInt *timeout);
		public int sceKernelReceiveMbx( int mbxid, int pmessage, int timeout )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF3986382, "sceKernelReceiveMbxCB" )]
		// SDK location: /user/pspthreadman.h:842
		// SDK declaration: int sceKernelReceiveMbxCB(SceUID mbxid, void **pmessage, SceUInt *timeout);
		public int sceKernelReceiveMbxCB( int mbxid, int pmessage, int timeout )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0D81716A, "sceKernelPollMbx" )]
		// SDK location: /user/pspthreadman.h:859
		// SDK declaration: int sceKernelPollMbx(SceUID mbxid, void **pmessage);
		public int sceKernelPollMbx( int mbxid, int pmessage )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x87D4DD36, "sceKernelCancelReceiveMbx" )]
		// SDK location: /user/pspthreadman.h:876
		// SDK declaration: int sceKernelCancelReceiveMbx(SceUID mbxid, int *pnum);
		public int sceKernelCancelReceiveMbx( int mbxid, int pnum )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA8E8C846, "sceKernelReferMbxStatus" )]
		// SDK location: /user/pspthreadman.h:886
		// SDK declaration: int sceKernelReferMbxStatus(SceUID mbxid, SceKernelMbxInfo *info);
		public int sceKernelReferMbxStatus( int mbxid, int info )
		{
			return Module.NotImplementedReturn;
		}
	}
}
