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
		[BiosFunction( 0xE81CAF8F, "sceKernelCreateCallback" )]
		// SDK location: /user/pspthreadman.h:985
		// SDK declaration: int sceKernelCreateCallback(const char *name, SceKernelCallbackFunction func, void *arg);
		public int sceKernelCreateCallback( int name, int func, int arg )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xEDBA5844, "sceKernelDeleteCallback" )]
		// SDK location: /user/pspthreadman.h:1005
		// SDK declaration: int sceKernelDeleteCallback(SceUID cb);
		public int sceKernelDeleteCallback( int cb )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC11BA8C4, "sceKernelNotifyCallback" )]
		// SDK location: /user/pspthreadman.h:1015
		// SDK declaration: int sceKernelNotifyCallback(SceUID cb, int arg2);
		public int sceKernelNotifyCallback( int cb, int arg2 )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBA4051D6, "sceKernelCancelCallback" )]
		// SDK location: /user/pspthreadman.h:1024
		// SDK declaration: int sceKernelCancelCallback(SceUID cb);
		public int sceKernelCancelCallback( int cb )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2A3D44FF, "sceKernelGetCallbackCount" )]
		// SDK location: /user/pspthreadman.h:1033
		// SDK declaration: int sceKernelGetCallbackCount(SceUID cb);
		public int sceKernelGetCallbackCount( int cb )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x349D6D6C, "sceKernelCheckCallback" )]
		// SDK location: /user/pspthreadman.h:1040
		// SDK declaration: int sceKernelCheckCallback();
		public int sceKernelCheckCallback()
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x730ED8BC, "sceKernelReferCallbackStatus" )]
		// SDK location: /user/pspthreadman.h:996
		// SDK declaration: int sceKernelReferCallbackStatus(SceUID cb, SceKernelCallbackInfo *status);
		public int sceKernelReferCallbackStatus( int cb, int status )
		{
			return Module.NotImplementedReturn;
		}
	}
}
