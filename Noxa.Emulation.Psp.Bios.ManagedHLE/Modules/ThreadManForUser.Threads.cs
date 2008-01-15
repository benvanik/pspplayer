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

		//[NotImplemented]
		//[Stateless]
		//[BiosFunction( 0x532A522E, "_sceKernelExitThread" )]
		//// SDK location: /user/pspthreadman.h:1679
		//// SDK declaration: void _sceKernelExitThread();
		//public void _sceKernelExitThread()
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

		[Stateless]
		[BiosFunction( 0x446D8DE6, "sceKernelCreateThread" )]
		// SDK location: /user/pspthreadman.h:169
		// SDK declaration: SceUID sceKernelCreateThread(const char *name, SceKernelThreadEntry entry, int initPriority, int stackSize, SceUInt attr, SceKernelThreadOptParam *option);
		public int sceKernelCreateThread( int name, int entry, int initPriority, int stackSize, int attr, int option )
		{
			KModule module = _kernel.ActiveThread.Module;

			KThread thread = new KThread( _kernel,
				module,
				_kernel.Partitions[ 6 ],
				_kernel.ReadString( ( uint )name ),
				( uint )entry,
				initPriority,
				( KThreadAttributes )attr,
				( uint )stackSize );
			_kernel.AddHandle( thread );

			// Option unused?
			Debug.Assert( option == 0 );

			Log.WriteLine( Verbosity.Normal, Feature.Bios, "sceKernelCreateThread: created thread {0} {1} with entry {2:X8}", thread.UID, thread.Name, thread.EntryAddress );

			return ( int )thread.UID;
		}

		[BiosFunction( 0x9FA03CD3, "sceKernelDeleteThread" )]
		// SDK location: /user/pspthreadman.h:179
		// SDK declaration: int sceKernelDeleteThread(SceUID thid);
		public int sceKernelDeleteThread( int thid )
		{
			KThread thread = _kernel.GetHandle<KThread>( thid );
			if( thread == null )
				return -1;

			// Don't support this
			if( _kernel.ActiveThread == thread )
			{
				thread.Exit( 0 );
				_kernel.Schedule();
				Debug.Assert( _kernel.ActiveThread != thread );
			}
			Debug.Assert( thread.State == KThreadState.Dead );

			Log.WriteLine( Verbosity.Normal, Feature.Bios, "sceKernelDeleteThread: deleting thread {0} {1}", thread.UID, thread.Name );

			thread.Delete();
			_kernel.RemoveHandle( thread.UID );

			return 0;
		}

		[BiosFunction( 0xF475845D, "sceKernelStartThread" )]
		// SDK location: /user/pspthreadman.h:188
		// SDK declaration: int sceKernelStartThread(SceUID thid, SceSize arglen, void *argp);
		public int sceKernelStartThread( int thid, int arglen, int argp )
		{
			KThread thread = _kernel.GetHandle<KThread>( thid );
			if( thread == null )
				return -1;

			Log.WriteLine( Verbosity.Normal, Feature.Bios, "sceKernelStartThread: starting thread {0} {1}", thread.UID, thread.Name );

			thread.Start( ( uint )arglen, ( uint )argp );
			_kernel.Schedule();

			return 0;
		}

		[BiosFunction( 0xAA73C935, "sceKernelExitThread" )]
		// SDK location: /user/pspthreadman.h:195
		// SDK declaration: int sceKernelExitThread(int status);
		public int sceKernelExitThread( int status )
		{
			KThread thread = _kernel.ActiveThread;
			if( thread == null )
				return -1;

			thread.Exit( status );

			return 0;
		}

		[BiosFunction( 0x809CE29B, "sceKernelExitDeleteThread" )]
		// SDK location: /user/pspthreadman.h:202
		// SDK declaration: int sceKernelExitDeleteThread(int status);
		public int sceKernelExitDeleteThread( int status )
		{
			KThread thread = _kernel.ActiveThread;
			if( thread == null )
				return -1;

			int ret = this.sceKernelExitThread( status );
			if( ret < 0 )
				return ret;
			return this.sceKernelDeleteThread( ( int )thread.UID );
		}

		[BiosFunction( 0x616403BA, "sceKernelTerminateThread" )]
		// SDK location: /user/pspthreadman.h:211
		// SDK declaration: int sceKernelTerminateThread(SceUID thid);
		public int sceKernelTerminateThread( int thid )
		{
			KThread thread = _kernel.GetHandle<KThread>( thid );
			if( thread == null )
				return -1;

			thread.Exit( 0 );

			return 0;
		}

		[BiosFunction( 0x383F7BCC, "sceKernelTerminateDeleteThread" )]
		// SDK location: /user/pspthreadman.h:220
		// SDK declaration: int sceKernelTerminateDeleteThread(SceUID thid);
		public int sceKernelTerminateDeleteThread( int thid )
		{
			int ret = this.sceKernelTerminateThread( thid );
			if( ret < 0 )
				return ret;
			return this.sceKernelDeleteThread( thid );
		}

		[Stateless]
		[BiosFunction( 0xEA748E31, "sceKernelChangeCurrentThreadAttr" )]
		// SDK location: /user/pspthreadman.h:364
		// SDK declaration: int sceKernelChangeCurrentThreadAttr(int unknown, SceUInt attr);
		public int sceKernelChangeCurrentThreadAttr( int unknown, int attr )
		{
			KThread thread = _kernel.ActiveThread;
			if( thread == null )
				return -1;

			thread.Attributes &= ~( ( KThreadAttributes )unknown );
			thread.Attributes |= ( KThreadAttributes )attr;

			return 0;
		}

		[BiosFunction( 0x71BC9871, "sceKernelChangeThreadPriority" )]
		// SDK location: /user/pspthreadman.h:381
		// SDK declaration: int sceKernelChangeThreadPriority(SceUID thid, int priority);
		public int sceKernelChangeThreadPriority( int thid, int priority )
		{
			// I think this is right
			KThread thread;
			if( thid == 0 )
				thread = _kernel.ActiveThread;
			else
				thread = _kernel.GetHandle<KThread>( thid );
			if( thread == null )
				return -1;

			if( thread.Priority != priority )
			{
				thread.ChangePriority( priority );
				//_kernel.Schedule();
			}

			return 0;
		}

		[BiosFunction( 0x912354A7, "sceKernelRotateThreadReadyQueue" )]
		// SDK location: /user/pspthreadman.h:390
		// SDK declaration: int sceKernelRotateThreadReadyQueue(int priority);
		public int sceKernelRotateThreadReadyQueue( int priority )
		{
			// At the given priority, move the head thread to the tail
			// This is tricky - we need to find the first and last thread at a given priority
			// then move the first one after the last one

			// Find first thread
			LinkedListEntry<KThread> first = null;
			LinkedListEntry<KThread> e = _kernel.SchedulableThreads.HeadEntry;
			while( e != null )
			{
				if( e.Value.Priority == priority )
				{
					first = e;
					break;
				}
				e = e.Next;
			}
			//Debug.Assert( first != null );
			if( first == null )
			{
				// No threads of the given priority found
				return 0;
			}

			// Find last thread
			LinkedListEntry<KThread> last = first;
			while( e != null )
			{
				if( e.Value.Priority != priority )
					break;
				last = e;
				e = e.Next;
			}
			if( first == last )
			{
				// No change - only one schedulable thread had this priority
				_kernel.Schedule();
				return 0;
			}

			// Perform the move
			_kernel.SchedulableThreads.Remove( first );
			_kernel.SchedulableThreads.InsertAfter( first.Value, last );

			_kernel.Schedule();
			return 0;
		}

		[Stateless]
		[BiosFunction( 0x293B45B8, "sceKernelGetThreadId" )]
		// SDK location: /user/pspthreadman.h:406
		// SDK declaration: int sceKernelGetThreadId();
		public int sceKernelGetThreadId()
		{
			KThread thread = _kernel.ActiveThread;
			if( thread == null )
				return -1;
			return ( int )thread.UID;
		}

		[Stateless]
		[BiosFunction( 0x94AA61EE, "sceKernelGetThreadCurrentPriority" )]
		// SDK location: /user/pspthreadman.h:413
		// SDK declaration: int sceKernelGetThreadCurrentPriority();
		public int sceKernelGetThreadCurrentPriority()
		{
			KThread thread = _kernel.ActiveThread;
			if( thread == null )
				return -1;
			return thread.Priority;
		}

		[Stateless]
		[BiosFunction( 0x3B183E26, "sceKernelGetThreadExitStatus" )]
		// SDK location: /user/pspthreadman.h:422
		// SDK declaration: int sceKernelGetThreadExitStatus(SceUID thid);
		public int sceKernelGetThreadExitStatus( int thid )
		{
			KThread thread = _kernel.GetHandle<KThread>( thid );
			if( thread == null )
				return -1;
			return thread.ExitCode;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD13BDE95, "sceKernelCheckThreadStack" )]
		// SDK location: /user/pspthreadman.h:429
		// SDK declaration: int sceKernelCheckThreadStack();
		public int sceKernelCheckThreadStack()
		{
			// I think this is supposed to check the bounds and make sure it hasn't overrun?
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x52089CA1, "sceKernelGetThreadStackFreeSize" )]
		// SDK location: /user/pspthreadman.h:439
		// SDK declaration: int sceKernelGetThreadStackFreeSize(SceUID thid);
		public int sceKernelGetThreadStackFreeSize( int thid )
		{
			KThread thread = _kernel.GetHandle<KThread>( thid );
			if( thread == null )
				return -1;
			// We have stack block address - do we peek $sp and calc?
			return Module.NotImplementedReturn;
		}

		[Stateless]
		[BiosFunction( 0x17C1684E, "sceKernelReferThreadStatus" )]
		// SDK location: /user/pspthreadman.h:458
		// SDK declaration: int sceKernelReferThreadStatus(SceUID thid, SceKernelThreadInfo *info);
		public unsafe int sceKernelReferThreadStatus( int thid, int info )
		{
			//typedef struct SceKThreadInfo {
			//    SceSize     size;
			//    char    	name[32];
			//    SceUInt     attr;
			//    int     	status;
			//    SceKThreadEntry    entry;
			//    void *  	stack;
			//    int     	stackSize;
			//    void *  	gpReg;
			//    int     	initPriority;
			//    int     	currentPriority;
			//    int     	waitType;
			//    SceUID  	waitId;
			//    int     	wakeupCount;
			//    int     	exitStatus;
			//    SceKernelSysClock   runClocks;
			//    SceUInt     intrPreemptCount;
			//    SceUInt     threadPreemptCount;
			//    SceUInt     releaseCount;
			//} SceKThreadInfo;

			KThread thread = _kernel.GetHandle<KThread>( thid );
			if( thread == null )
				return -1;

			// NOTE: this is wrong! It's not supported!
			long runClocks = thread.RunClocks;

			uint size;

			// Ensure 104 bytes
			uint *p = ( uint* )_memorySystem.Translate( ( uint )info );
			size = *p;
			if( size != 104 && size != 108 )
			{
				Log.WriteLine( Verbosity.Critical, Feature.Bios, "sceKernelReferThreadStatus: app passed struct with size {0}, expected 104", size );
				return -1;
			}
			p++;
			_kernel.WriteString( ( uint )info + 4, thread.Name );
			p += 8;
			*( p++ ) = ( uint )thread.Attributes;
			*( p++ ) = ( uint )thread.State;
			*( p++ ) = thread.EntryAddress;
			*( p++ ) = thread.StackBlock.Address;
			*( p++ ) = thread.StackBlock.Size;
			*( p++ ) = thread.GlobalPointer;
			*( p++ ) = ( uint )thread.InitialPriority;
			*( p++ ) = ( uint )thread.Priority;
			*( p++ ) = ( uint )thread.WaitingOn;
			if( thread.WaitHandle != null )
				*( p++ ) = thread.WaitHandle.UID;
			else
				*( p++ ) = 0;
			*( p++ ) = thread.WakeupCount;
			*( p++ ) = ( uint )thread.ExitCode;
			*( p++ ) = ( uint )runClocks;
			*( p++ ) = ( uint )( ( ulong )runClocks >> 32 );
			*( p++ ) = thread.InterruptPreemptionCount;
			*( p++ ) = thread.ThreadPreemptionCount;
			*( p++ ) = thread.ReleaseCount;
			
			if (size == 108)
				*( p++ ) = 0;

			return 0;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFFC36A14, "sceKernelReferThreadRunStatus" )]
		// SDK location: /user/pspthreadman.h:468
		// SDK declaration: int sceKernelReferThreadRunStatus(SceUID thid, SceKernelThreadRunStatus *status);
		public int sceKernelReferThreadRunStatus( int thid, int status )
		{
			//typedef struct SceKThreadRunStatus {
			//    SceSize 	size;
			//    int 		status;
			//    int 		currentPriority;
			//    int 		waitType;
			//    int 		waitId;
			//    int 		wakeupCount;
			//    SceKernelSysClock runClocks;
			//    SceUInt 	intrPreemptCount;
			//    SceUInt 	threadPreemptCount;
			//    SceUInt 	releaseCount;
			//} SceKThreadRunStatus;

			/*
			SysClock runClocks;
			runClocks.QuadPart = thread->RunClocks;

			// Ensure 44 bytes
			if( memory->ReadWord( status ) != 44 )
			{
				Debug::WriteLine( String::Format( "ThreadManForUser: sceKernelReferThreadRunStatus app passed struct with size {0}, expected 44",
					memory->ReadWord( status ) ) );
				return -1;
			}
			memory->WriteWord( status +  4, 4, ( int )thread->State );
			memory->WriteWord( status +  8, 4, thread->Priority );
			memory->WriteWord( status + 12, 4, ( int )thread->WaitingOn );
			if( thread->WaitingOn == KThreadWaitEvent )
				memory->WriteWord( status + 16, 4, thread->WaitEvent->UID );
			else if( thread->WaitingOn == KThreadWaitJoin )
				memory->WriteWord( status + 16, 4, thread->WaitThread->UID );
			else
				memory->WriteWord( status + 16, 4, 0 );
			memory->WriteWord( status + 20, 4, ( int )thread->WakeupCount );
			memory->WriteWord( status + 24, 4, ( int )runClocks.LowPart );
			memory->WriteWord( status + 28, 4, ( int )runClocks.HighPart );
			memory->WriteWord( status + 32, 4, ( int )thread->InterruptPreemptionCount );
			memory->WriteWord( status + 36, 4, ( int )thread->ThreadPreemptionCount );
			memory->WriteWord( status + 40, 4, ( int )thread->ReleaseCount );
			*/
			return Module.NotImplementedReturn;
		}
	}
}
