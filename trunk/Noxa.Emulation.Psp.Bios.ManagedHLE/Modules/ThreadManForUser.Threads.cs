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
		//[NotImplemented]
		//[Stateless]
		//[BiosFunction( 0x6E9EA350, "_sceKernelReturnFromCallback" )]
		//// SDK location: /user/pspthreadman.h:1453
		//// SDK declaration: void _sceKernelReturnFromCallback();
		//public void _sceKernelReturnFromCallback()
		//{
		//}

		#region Event Handlers

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0C106E53, "sceKernelRegisterThreadEventHandler" )]
		// SDK location: /user/pspthreadman.h:1729
		// SDK declaration: SceUID sceKernelRegisterThreadEventHandler(const char *name, SceUID threadID, int mask, SceKernelThreadEventHandler handler, void *common);
		public int sceKernelRegisterThreadEventHandler( int name, int threadID, int mask, int handler, int common )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x72F3C145, "sceKernelReleaseThreadEventHandler" )]
		// SDK location: /user/pspthreadman.h:1738
		// SDK declaration: int sceKernelReleaseThreadEventHandler(SceUID uid);
		public int sceKernelReleaseThreadEventHandler( int uid )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x369EEB6B, "sceKernelReferThreadEventHandlerStatus" )]
		// SDK location: /user/pspthreadman.h:1748
		// SDK declaration: int sceKernelReferThreadEventHandlerStatus(SceUID uid, struct SceKernelThreadEventHandlerInfo *info);
		public int sceKernelReferThreadEventHandlerStatus( int uid, int info )
		{
			return Module.NotImplementedReturn;
		}
		
		#endregion

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x446D8DE6, "sceKernelCreateThread" )]
		// SDK location: /user/pspthreadman.h:169
		// SDK declaration: SceUID sceKernelCreateThread(const char *name, SceKernelThreadEntry entry, int initPriority, int stackSize, SceUInt attr, SceKernelThreadOptParam *option);
		public int sceKernelCreateThread( int name, int entry, int initPriority, int stackSize, int attr, int option )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9FA03CD3, "sceKernelDeleteThread" )]
		// SDK location: /user/pspthreadman.h:179
		// SDK declaration: int sceKernelDeleteThread(SceUID thid);
		public int sceKernelDeleteThread( int thid )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF475845D, "sceKernelStartThread" )]
		// SDK location: /user/pspthreadman.h:188
		// SDK declaration: int sceKernelStartThread(SceUID thid, SceSize arglen, void *argp);
		public int sceKernelStartThread( int thid, int arglen, int argp )
		{
			return Module.NotImplementedReturn;
		}

		//[NotImplemented]
		//[Stateless]
		//[BiosFunction( 0x532A522E, "_sceKernelExitThread" )]
		//// SDK location: /user/pspthreadman.h:1679
		//// SDK declaration: void _sceKernelExitThread();
		//public void _sceKernelExitThread()
		//{
		//}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xAA73C935, "sceKernelExitThread" )]
		// SDK location: /user/pspthreadman.h:195
		// SDK declaration: int sceKernelExitThread(int status);
		public int sceKernelExitThread( int status )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x809CE29B, "sceKernelExitDeleteThread" )]
		// SDK location: /user/pspthreadman.h:202
		// SDK declaration: int sceKernelExitDeleteThread(int status);
		public int sceKernelExitDeleteThread( int status )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x616403BA, "sceKernelTerminateThread" )]
		// SDK location: /user/pspthreadman.h:211
		// SDK declaration: int sceKernelTerminateThread(SceUID thid);
		public int sceKernelTerminateThread( int thid )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x383F7BCC, "sceKernelTerminateDeleteThread" )]
		// SDK location: /user/pspthreadman.h:220
		// SDK declaration: int sceKernelTerminateDeleteThread(SceUID thid);
		public int sceKernelTerminateDeleteThread( int thid )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xEA748E31, "sceKernelChangeCurrentThreadAttr" )]
		// SDK location: /user/pspthreadman.h:364
		// SDK declaration: int sceKernelChangeCurrentThreadAttr(int unknown, SceUInt attr);
		public int sceKernelChangeCurrentThreadAttr( int unknown, int attr )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x71BC9871, "sceKernelChangeThreadPriority" )]
		// SDK location: /user/pspthreadman.h:381
		// SDK declaration: int sceKernelChangeThreadPriority(SceUID thid, int priority);
		public int sceKernelChangeThreadPriority( int thid, int priority )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x912354A7, "sceKernelRotateThreadReadyQueue" )]
		// SDK location: /user/pspthreadman.h:390
		// SDK declaration: int sceKernelRotateThreadReadyQueue(int priority);
		public int sceKernelRotateThreadReadyQueue( int priority )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x293B45B8, "sceKernelGetThreadId" )]
		// SDK location: /user/pspthreadman.h:406
		// SDK declaration: int sceKernelGetThreadId();
		public int sceKernelGetThreadId()
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x94AA61EE, "sceKernelGetThreadCurrentPriority" )]
		// SDK location: /user/pspthreadman.h:413
		// SDK declaration: int sceKernelGetThreadCurrentPriority();
		public int sceKernelGetThreadCurrentPriority()
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3B183E26, "sceKernelGetThreadExitStatus" )]
		// SDK location: /user/pspthreadman.h:422
		// SDK declaration: int sceKernelGetThreadExitStatus(SceUID thid);
		public int sceKernelGetThreadExitStatus( int thid )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD13BDE95, "sceKernelCheckThreadStack" )]
		// SDK location: /user/pspthreadman.h:429
		// SDK declaration: int sceKernelCheckThreadStack();
		public int sceKernelCheckThreadStack()
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x52089CA1, "sceKernelGetThreadStackFreeSize" )]
		// SDK location: /user/pspthreadman.h:439
		// SDK declaration: int sceKernelGetThreadStackFreeSize(SceUID thid);
		public int sceKernelGetThreadStackFreeSize( int thid )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x17C1684E, "sceKernelReferThreadStatus" )]
		// SDK location: /user/pspthreadman.h:458
		// SDK declaration: int sceKernelReferThreadStatus(SceUID thid, SceKernelThreadInfo *info);
		public int sceKernelReferThreadStatus( int thid, int info )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFFC36A14, "sceKernelReferThreadRunStatus" )]
		// SDK location: /user/pspthreadman.h:468
		// SDK declaration: int sceKernelReferThreadRunStatus(SceUID thid, SceKernelThreadRunStatus *status);
		public int sceKernelReferThreadRunStatus( int thid, int status )
		{
			return Module.NotImplementedReturn;
		}
	}
}
