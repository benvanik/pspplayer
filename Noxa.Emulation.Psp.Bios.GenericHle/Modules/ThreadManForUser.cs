// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Noxa.Emulation.Psp.Cpu;
using System.Diagnostics;

namespace Noxa.Emulation.Psp.Bios.GenericHle.Modules
{
	class ThreadManForUser : IModule
	{
		#region IModule Members

		protected HleInstance _hle;
		protected Kernel _kernel;

		public ThreadManForUser( HleInstance hle )
		{
			Debug.Assert( hle != null );

			_hle = hle;
			_kernel = hle.Kernel as Kernel;
		}

		public string Name
		{
			get
			{
				return "ThreadManForUser";
			}
		}

		#endregion

		#region Thread Management

		[BiosStub( 0x446d8de6, "sceKernelCreateThread", true, 6 )]
		public int sceKernelCreateThread( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = const char *name
			// a1 = SceKernelThreadEntry entry
			// a2 = int initPriority
			// a3 = int stackSize
			// sp[0] = SceUInt attr
			// sp[4] = SceKernelThreadOptParam *option
			int a4 = memory.ReadWord( sp + 0 );
			int a5 = memory.ReadWord( sp + 4 );

			string name = Kernel.ReadString( memory, a0 );

			int uid = _kernel.AllocateUid();
			KernelThread thread = new KernelThread( uid, name, a1, a2, ( uint )a3, a4 );
			_kernel.AddHandle( thread );
			_kernel.CreateThread( thread );

			// SceUID
			return uid;
		}

		[BiosStub( 0xf475845d, "sceKernelStartThread", true, 3 )]
		public int sceKernelStartThread( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceUID thid
			// a1 = SceSize arglen
			// a2 = void *argp

			KernelThread thread = _kernel.FindThread( a0 );
			if( thread == null )
				return -1;

			// TODO: Switch partition based on kernel/user status?
			thread.Start( _kernel.Partitions[ 1 ], a1, a2 );

			_kernel.ContextSwitch();

			// int
			return 0;
		}

		[BiosStub( 0x9fa03cd3, "sceKernelDeleteThread", true, 1 )]
		public int sceKernelDeleteThread( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceUID thid

			KernelThread thread = _kernel.FindThread( a0 );
			if( thread == null )
				return -1;

			_kernel.DeleteThread( thread );
			_kernel.RemoveHandle( thread );

			// int
			return 0;
		}

		[BiosStub( 0x809ce29b, "sceKernelExitDeleteThread", true, 1 )]
		public int sceKernelExitDeleteThread( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = int status

			KernelThread thread = _kernel.FindThread( a0 );
			if( thread == null )
				return -1;

			thread.Exit( a0 );
			_kernel.ContextSwitch();

			_kernel.DeleteThread( thread );
			_kernel.RemoveHandle( thread );

			// int
			return 0;
		}

		[BiosStub( 0x532a522e, "_sceKernelExitThread", false, 0 )]
		[BiosStubIncomplete]
		public int _sceKernelExitThread( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xaa73c935, "sceKernelExitThread", true, 1 )]
		public int sceKernelExitThread( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = int status

			KernelThread thread = _kernel.ActiveThread;
			if( thread == null )
				return -1;

			thread.Exit( a0 );
			_kernel.ContextSwitch();

			// int
			return 0;
		}

		[BiosStub( 0x616403ba, "sceKernelTerminateThread", true, 1 )]
		public int sceKernelTerminateThread( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceUID thid

			KernelThread thread = _kernel.FindThread( a0 );
			if( thread == null )
				return -1;

			thread.Exit( 0 );

			_kernel.ContextSwitch();

			// int
			return 0;
		}

		[BiosStub( 0x383f7bcc, "sceKernelTerminateDeleteThread", true, 1 )]
		public int sceKernelTerminateDeleteThread( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceUID thid

			KernelThread thread = _kernel.FindThread( a0 );
			if( thread == null )
				return -1;

			thread.Exit( 0 );
			_kernel.ContextSwitch();

			_kernel.DeleteThread( thread );
			_kernel.RemoveHandle( thread );

			// int
			return 0;
		}

		[BiosStub( 0x3ad58b8c, "sceKernelSuspendDispatchThread", true, 0 )]
		[BiosStubIncomplete]
		public int sceKernelSuspendDispatchThread( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// ?
			return 0;
		}

		[BiosStub( 0x27e22ec2, "sceKernelResumeDispatchThread", true, 1 )]
		[BiosStubIncomplete]
		public int sceKernelResumeDispatchThread( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = old dispatch result

			// ?
			return 0;
		}

		[BiosStub( 0xea748e31, "sceKernelChangeCurrentThreadAttr", true, 2 )]
		public int sceKernelChangeCurrentThreadAttr( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = int unknown
			// a1 = SceUInt attr

			KernelThread thread = _kernel.ActiveThread;
			if( thread == null )
				return -1;

			thread.Attributes = ( KernelThreadAttributes )a1;

			// int
			return 0;
		}

		[BiosStub( 0x71bc9871, "sceKernelChangeThreadPriority", true, 2 )]
		public int sceKernelChangeThreadPriority( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceUID thid
			// a1 = int priority

			KernelThread thread = _kernel.FindThread( a0 );
			if( thread == null )
				return -1;

			thread.Priority = a1;

			// int
			return 0;
		}

		[BiosStub( 0x912354a7, "sceKernelRotateThreadReadyQueue", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelRotateThreadReadyQueue( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x2c34e053, "sceKernelReleaseWaitThread", true, 1 )]
		public int sceKernelReleaseWaitThread( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceUID thid

			KernelThread thread = _kernel.FindThread( a0 );
			if( thread == null )
				return -1;

			thread.State = KernelThreadState.Ready;
			thread.WaitId = 0;
			thread.ReleaseCount++;

			// int
			return 0;
		}

		[BiosStub( 0x293b45b8, "sceKernelGetThreadId", true, 0 )]
		public int sceKernelGetThreadId( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			KernelThread thread = _kernel.ActiveThread;
			if( thread == null )
				return -1;

			// int
			return thread.Uid;
		}

		[BiosStub( 0x94aa61ee, "sceKernelGetThreadCurrentPriority", true, 0 )]
		public int sceKernelGetThreadCurrentPriority( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			KernelThread thread = _kernel.ActiveThread;
			if( thread == null )
				return -1;

			// int
			return thread.Priority;
		}

		[BiosStub( 0x3b183e26, "sceKernelGetThreadExitStatus", true, 1 )]
		public int sceKernelGetThreadExitStatus( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceUID thid

			KernelThread thread = _kernel.FindThread( a0 );
			if( thread == null )
				return -1;

			// int
			return thread.ExitCode;
		}

		[BiosStub( 0xd13bde95, "sceKernelCheckThreadStack", true, 0 )]
		[BiosStubIncomplete]
		public int sceKernelCheckThreadStack( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			KernelThread thread = _kernel.ActiveThread;
			if( thread == null )
				return -1;

			// TODO: sceKernelCheckThreadStack

			// int
			return 0;
		}

		[BiosStub( 0x52089ca1, "sceKernelGetThreadStackFreeSize", true, 1 )]
		[BiosStubIncomplete]
		public int sceKernelGetThreadStackFreeSize( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceUID thid (or 0 for current)

			KernelThread thread;
			if( a0 == 0 )
				thread = _kernel.ActiveThread;
			else
				thread = _kernel.FindThread( a0 );
			if( thread == null )
				return -1;

			// TODO: Find a good way to calculate stack size free
			// perhaps get stack pointer - base?

			// int
			return ( int )thread.StackSize;
		}

		[BiosStub( 0x17c1684e, "sceKernelReferThreadStatus", true, 2 )]
		public int sceKernelReferThreadStatus( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceUID thid
			// a1 = SceKernelThreadInfo *info

			KernelThread thread = _kernel.FindThread( a0 );
			if( thread == null )
				return -1;

			//typedef struct SceKernelThreadInfo {
			//    SceSize     size;
			//    char    	name[32];
			//    SceUInt     attr;
			//    int     	status;
			//    SceKernelThreadEntry    entry;
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
			//} SceKernelThreadInfo;

			// Ensure 104 bytes
			if( memory.ReadWord( a1 ) != 104 )
			{
				Debug.WriteLine( string.Format( "ThreadManForUser: sceKernelReferThreadStatus app passed struct with size {0}, expected 104",
					memory.ReadWord( a1 ) ) );
				return -1;
			}
			Kernel.WriteString( memory, a1 + 4, thread.Name );
			memory.WriteWord( a1 + 36, 4, ( int )thread.Attributes );
			memory.WriteWord( a1 + 40, 4, ( int )thread.State );
			memory.WriteWord( a1 + 44, 4, thread.EntryAddress );
			memory.WriteWord( a1 + 48, 4, ( int )thread.StackBlock.Address );
			memory.WriteWord( a1 + 52, 4, ( int )thread.StackSize );
			memory.WriteWord( a1 + 56, 4, 0 ); // TODO: get thread gp pointer
			memory.WriteWord( a1 + 60, 4, thread.InitialPriority );
			memory.WriteWord( a1 + 64, 4, thread.Priority );
			memory.WriteWord( a1 + 68, 4, ( int )thread.WaitType );
			memory.WriteWord( a1 + 72, 4, thread.WaitId );
			memory.WriteWord( a1 + 76, 4, ( int )thread.WakeupCount );
			memory.WriteWord( a1 + 80, 4, thread.ExitCode );
			memory.WriteWord( a1 + 84, 4, ( int )thread.RunClocks.Low );
			memory.WriteWord( a1 + 88, 4, ( int )thread.RunClocks.Hi );
			memory.WriteWord( a1 + 92, 4, ( int )thread.InterruptPreemptionCount );
			memory.WriteWord( a1 + 96, 4, ( int )thread.ThreadPreemptionCount );
			memory.WriteWord( a1 + 100, 4, ( int )thread.ReleaseCount );

			// int
			return 0;
		}

		[BiosStub( 0xffc36a14, "sceKernelReferThreadRunStatus", true, 2 )]
		public int sceKernelReferThreadRunStatus( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceUID thid
			// a1 = SceKernelThreadRunStatus *status

			KernelThread thread = _kernel.FindThread( a0 );
			if( thread == null )
				return -1;

			//typedef struct SceKernelThreadRunStatus {
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
			//} SceKernelThreadRunStatus;

			// Ensure 44 bytes
			if( memory.ReadWord( a1 ) != 44 )
			{
				Debug.WriteLine( string.Format( "ThreadManForUser: sceKernelReferThreadRunStatus app passed struct with size {0}, expected 44",
					memory.ReadWord( a1 ) ) );
				return -1;
			}
			memory.WriteWord( a1 +  4, 4, ( int )thread.State );
			memory.WriteWord( a1 +  8, 4, thread.Priority );
			memory.WriteWord( a1 + 12, 4, ( int )thread.WaitType );
			memory.WriteWord( a1 + 16, 4, thread.WaitId );
			memory.WriteWord( a1 + 20, 4, ( int )thread.WakeupCount );
			memory.WriteWord( a1 + 24, 4, ( int )thread.RunClocks.Low );
			memory.WriteWord( a1 + 28, 4, ( int )thread.RunClocks.Hi );
			memory.WriteWord( a1 + 32, 4, ( int )thread.InterruptPreemptionCount );
			memory.WriteWord( a1 + 36, 4, ( int )thread.ThreadPreemptionCount );
			memory.WriteWord( a1 + 40, 4, ( int )thread.ReleaseCount );

			// int
			return 0;
		}

		[BiosStub( 0x627e6f3a, "sceKernelReferSystemStatus", true, 1 )]
		public int sceKernelReferSystemStatus( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceKernelSystemStatus *status

			//typedef struct SceKernelSystemStatus {
			//    SceSize 	size;
			//    SceUInt 	status;
			//    SceKernelSysClock 	idleClocks;
			//    SceUInt 	comesOutOfIdleCount;
			//    SceUInt 	threadSwitchCount;
			//    SceUInt 	vfpuSwitchCount;
			//} SceKernelSystemStatus;

			// Ensure 28 bytes
			if( memory.ReadWord( a0 ) != 28 )
			{
				Debug.WriteLine( string.Format( "ThreadManForUser: sceKernelReferSystemStatus app passed struct with size {0}, expected 28",
					memory.ReadWord( a0 ) ) );
				return -1;
			}
			memory.WriteWord( a0 + 4, 4, _kernel.Status.Status );
			memory.WriteWord( a0 + 8, 4, ( int )_kernel.Status.IdleClocks.Low );
			memory.WriteWord( a0 + 12, 4, ( int )_kernel.Status.IdleClocks.Hi );
			memory.WriteWord( a0 + 16, 4, ( int )_kernel.Status.LeaveIdleCount );
			memory.WriteWord( a0 + 20, 4, ( int )_kernel.Status.ThreadSwitchCount );
			memory.WriteWord( a0 + 24, 4, ( int )_kernel.Status.VfpuSwitchCount );

			// int
			return 0;
		}

		[BiosStub( 0x94416130, "sceKernelGetThreadmanIdList", true, 4 )]
		[BiosStubIncomplete]
		public int sceKernelGetThreadmanIdList( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = enum SceKernelIdListType type
			// a1 = SceUID *readbuf
			// a2 = int readbufsize
			// a3 = int *idcount

			// int
			return 0;
		}

		#endregion

		#region Thread Control

		[BiosStub( 0x9ace131e, "sceKernelSleepThread", true, 0 )]
		public int sceKernelSleepThread( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			KernelThread thread = _kernel.ActiveThread;
			if( thread == null )
				return -1;

			thread.State = KernelThreadState.Waiting;
			thread.WaitId = 0;
			thread.WaitType = KernelThreadWait.Sleep;
			thread.CanHandleCallbacks = false;
			_kernel.ContextSwitch();

			// int
			return 0;
		}

		[BiosStub( 0x82826f70, "sceKernelSleepThreadCB", false, 0 )]
		public int sceKernelSleepThreadCB( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			KernelThread thread = _kernel.ActiveThread;
			if( thread == null )
				return -1;

			thread.State = KernelThreadState.Waiting;
			thread.WaitId = 0;
			thread.WaitType = KernelThreadWait.Sleep;
			thread.CanHandleCallbacks = true;
			_kernel.ContextSwitch();

			return 0;
		}

		[BiosStub( 0xd59ead2f, "sceKernelWakeupThread", true, 1 )]
		public int sceKernelWakeupThread( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceUID thid

			KernelThread thread = _kernel.FindThread( a0 );
			if( thread == null )
				return -1;

			thread.State = KernelThreadState.Ready;
			thread.WakeupCount++;
			_kernel.ContextSwitch();

			// int
			return 0;
		}

		[BiosStub( 0xfccfad26, "sceKernelCancelWakeupThread", true, 1 )]
		public int sceKernelCancelWakeupThread( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceUID thid

			KernelThread thread = _kernel.FindThread( a0 );
			if( thread == null )
				return -1;

			// TODO: ensure sceKernelCancelWakeupThread is right
			if( thread.State == KernelThreadState.Ready )
			{
				thread.State = KernelThreadState.Waiting;
				thread.WaitId = 0;
				thread.WaitType = KernelThreadWait.Sleep;
			}
			else
				return -1;

			// int
			return 0;
		}

		[BiosStub( 0x9944f31f, "sceKernelSuspendThread", true, 1 )]
		public int sceKernelSuspendThread( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceUID thid

			KernelThread thread = _kernel.FindThread( a0 );
			if( thread == null )
				return -1;

			thread.State = KernelThreadState.Suspended;
			_kernel.ContextSwitch();

			// int
			return 0;
		}

		[BiosStub( 0x75156e8f, "sceKernelResumeThread", true, 1 )]
		public int sceKernelResumeThread( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceUID thid

			KernelThread thread = _kernel.FindThread( a0 );
			if( thread == null )
				return -1;

			thread.State = KernelThreadState.Ready;
			_kernel.ContextSwitch();

			// int
			return 0;
		}

		[BiosStub( 0x278c0df5, "sceKernelWaitThreadEnd", true, 2 )]
		public int sceKernelWaitThreadEnd( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceUID thid
			// a1 = SceUInt *timeout

			KernelThread thread = _kernel.ActiveThread;
			if( thread == null )
				return -1;

			thread.State = KernelThreadState.Waiting;
			thread.WaitId = a0;
			thread.WaitType = KernelThreadWait.ThreadEnd;
			if( a1 != 0 )
				thread.WaitTimeout = memory.ReadWord( a1 );
			else
				thread.WaitTimeout = 0;
			thread.CanHandleCallbacks = false;
			_kernel.ContextSwitch();

			// int
			return 0;
		}

		[BiosStub( 0x840e8133, "sceKernelWaitThreadEndCB", true, 2 )]
		public int sceKernelWaitThreadEndCB( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceUID thid
			// a1 = SceUInt *timeout

			KernelThread thread = _kernel.ActiveThread;
			if( thread == null )
				return -1;

			thread.State = KernelThreadState.Waiting;
			thread.WaitId = a0;
			thread.WaitType = KernelThreadWait.ThreadEnd;
			if( a1 != 0 )
				thread.WaitTimeout = memory.ReadWord( a1 );
			else
				thread.WaitTimeout = 0;
			thread.CanHandleCallbacks = true;
			_kernel.ContextSwitch();

			// int
			return 0;
		}

		[BiosStub( 0xceadeb47, "sceKernelDelayThread", false, 1 )]
		public int sceKernelDelayThread( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceUInt delay (microseconds)

			KernelThread thread = _kernel.ActiveThread;
			if( thread == null )
				return -1;

			thread.State = KernelThreadState.Waiting;
			thread.WaitType = KernelThreadWait.Delay;
			thread.WaitId = 0;
			thread.WaitTimeout = a0;
			thread.CanHandleCallbacks = false;
			_kernel.ContextSwitch();

			return 0;
		}

		[BiosStub( 0x68da9e36, "sceKernelDelayThreadCB", false, 1 )]
		public int sceKernelDelayThreadCB( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceUInt delay (microseconds)

			KernelThread thread = _kernel.ActiveThread;
			if( thread == null )
				return -1;

			thread.State = KernelThreadState.Waiting;
			thread.WaitType = KernelThreadWait.Delay;
			thread.WaitId = 0;
			thread.WaitTimeout = a0;
			thread.CanHandleCallbacks = true;
			_kernel.ContextSwitch();

			return 0;
		}

		[BiosStub( 0xbd123d9e, "sceKernelDelaySysClockThread", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelDelaySysClockThread( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x1181e963, "sceKernelDelaySysClockThreadCB", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelDelaySysClockThreadCB( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		#endregion

		#region VTimer

		[BiosStub( 0x20fff560, "sceKernelCreateVTimer", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelCreateVTimer( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x328f9e52, "sceKernelDeleteVTimer", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelDeleteVTimer( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xb3a59970, "sceKernelGetVTimerBase", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelGetVTimerBase( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xb7c18b77, "sceKernelGetVTimerBaseWide", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelGetVTimerBaseWide( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x034a921f, "sceKernelGetVTimerTime", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelGetVTimerTime( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xc0b3ffd2, "sceKernelGetVTimerTimeWide", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelGetVTimerTimeWide( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x542ad630, "sceKernelSetVTimerTime", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelSetVTimerTime( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xfb6425c3, "sceKernelSetVTimerTimeWide", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelSetVTimerTimeWide( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xc68d9437, "sceKernelStartVTimer", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelStartVTimer( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xd0aeee87, "sceKernelStopVTimer", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelStopVTimer( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xd8b299ae, "sceKernelSetVTimerHandler", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelSetVTimerHandler( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x53b00e9a, "sceKernelSetVTimerHandlerWide", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelSetVTimerHandlerWide( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xd2d615ef, "sceKernelCancelVTimerHandler", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelCancelVTimerHandler( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x5f32beaa, "sceKernelReferVTimerStatus", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelReferVTimerStatus( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		#endregion

		#region Callbacks

		[BiosStub( 0x6e9ea350, "_sceKernelReturnFromCallback", false, 0 )]
		[BiosStubIncomplete]
		public int _sceKernelReturnFromCallback( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x0c106e53, "sceKernelRegisterThreadEventHandler", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelRegisterThreadEventHandler( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x72f3c145, "sceKernelReleaseThreadEventHandler", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelReleaseThreadEventHandler( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x369eeb6b, "sceKernelReferThreadEventHandlerStatus", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelReferThreadEventHandlerStatus( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xe81caf8f, "sceKernelCreateCallback", true, 3 )]
		public int sceKernelCreateCallback( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = const char *name
			// a1 = SceKernelCallbackFunction func
			// a2 = void *arg

			KernelThread thread = _kernel.ActiveThread;
			if( thread == null )
				return -1;

			KernelCallback callback = new KernelCallback();
			callback.HandleType = KernelHandleType.Callback;
			callback.Uid = _kernel.AllocateUid();
			callback.Name = Kernel.ReadString( memory, a0 );
			callback.FunctionAddress = a1;
			callback.CommonAddress = a2;
			callback.Thread = thread;

			thread.Callbacks.Add( callback );
			_kernel.AddHandle( callback );

			// SceUID
			return callback.Uid;
		}

		[BiosStub( 0xedba5844, "sceKernelDeleteCallback", true, 1 )]
		public int sceKernelDeleteCallback( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceUID cbid

			KernelCallback callback = _kernel.FindHandle( a0 ) as KernelCallback;
			if( callback == null )
				return -1;

			callback.Thread.Callbacks.Remove( callback );
			_kernel.RemoveHandle( callback );

			// int
			return 0;
		}

		[BiosStub( 0xc11ba8c4, "sceKernelNotifyCallback", true, 2 )]
		[BiosStubIncomplete]
		public int sceKernelNotifyCallback( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceUID cbid
			// a1 = int arg

			KernelCallback callback = _kernel.FindHandle( a0 ) as KernelCallback;
			if( callback == null )
				return -1;

			// int
			return 0;
		}

		[BiosStub( 0xba4051d6, "sceKernelCancelCallback", true, 1 )]
		[BiosStubIncomplete]
		public int sceKernelCancelCallback( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceUID cbid

			KernelCallback callback = _kernel.FindHandle( a0 ) as KernelCallback;
			if( callback == null )
				return -1;

			// int
			return 0;
		}

		[BiosStub( 0x2a3d44ff, "sceKernelGetCallbackCount", true, 1 )]
		[BiosStubIncomplete]
		public int sceKernelGetCallbackCount( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceUID cbid

			KernelCallback callback = _kernel.FindHandle( a0 ) as KernelCallback;
			if( callback == null )
				return -1;

			// int
			return 0;
		}

		[BiosStub( 0x349d6d6c, "sceKernelCheckCallback", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelCheckCallback( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x730ed8bc, "sceKernelReferCallbackStatus", true, 2 )]
		public int sceKernelReferCallbackStatus( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceUID cbid
			// a1 = SceKernelCallbackInfo *status

			KernelCallback callback = _kernel.FindHandle( a0 ) as KernelCallback;
			if( callback == null )
				return -1;

			if( memory.ReadWord( a1 ) == 56 )
			{
				Kernel.WriteString( memory, a1 + 4, callback.Name );
				memory.WriteWord( a1 + 36, 4, callback.Thread.Uid );
				memory.WriteWord( a1 + 40, 4, callback.FunctionAddress );
				memory.WriteWord( a1 + 44, 4, callback.CommonAddress );
				memory.WriteWord( a1 + 48, 4, callback.NotifyCount );
				memory.WriteWord( a1 + 52, 4, callback.NotifyArguments );
			}
			else
			{
				Debug.WriteLine( string.Format( "sceKernelReferCallbackStatus: expected SceKernelCallbackInfo of size 56, not {0}", memory.ReadWord( a1 ) ) );
				return -1;
			}

			// int
			return 0;
		}

		#endregion

		#region Semaphores

		[BiosStub( 0xd6da4ba1, "sceKernelCreateSema", true, 5 )]
		[BiosStubIncomplete]
		public int sceKernelCreateSema( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = const char *name
			// a1 = SceUInt attr
			// a2 = int initVal
			// a3 = int maxVal
			// sp[0] = SceKernelSemaOptParam *option
			int a4 = memory.ReadWord( sp + 0 );

			// SceUID
			return 0;
		}

		[BiosStub( 0x28b6489c, "sceKernelDeleteSema", true, 1 )]
		[BiosStubIncomplete]
		public int sceKernelDeleteSema( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceUID semaid

			// int
			return 0;
		}

		[BiosStub( 0x3f53e640, "sceKernelSignalSema", true, 2 )]
		[BiosStubIncomplete]
		public int sceKernelSignalSema( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceUID semaid
			// a1 = int signal

			// int
			return 0;
		}

		[BiosStub( 0x4e3a1105, "sceKernelWaitSema", true, 3 )]
		[BiosStubIncomplete]
		public int sceKernelWaitSema( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceUID semaid
			// a1 = int signal
			// a2 = SceUInt *timeout

			// int
			return 0;
		}

		[BiosStub( 0x6d212bac, "sceKernelWaitSemaCB", true, 3 )]
		[BiosStubIncomplete]
		public int sceKernelWaitSemaCB( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceUID semaid
			// a1 = int signal
			// a2 = SceUInt *timeout

			// int
			return 0;
		}

		[BiosStub( 0x58b1f937, "sceKernelPollSema", true, 2 )]
		[BiosStubIncomplete]
		public int sceKernelPollSema( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceUID semaid
			// a1 = int signal

			// int
			return 0;
		}

		[BiosStub( 0x8ffdf9a2, "sceKernelCancelSema", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelCancelSema( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xbc6febc5, "sceKernelReferSemaStatus", true, 2 )]
		[BiosStubIncomplete]
		public int sceKernelReferSemaStatus( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceUID semaid
			// a1 = SceKernelSemaInfo *info

			// int
			return 0;
		}

		#endregion

		#region Events

		[BiosStub( 0x55c20a00, "sceKernelCreateEventFlag", true, 4 )]
		public int sceKernelCreateEventFlag( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = const char *name
			// a1 = int unk1
			// a2 = int bits
			// a3 = int unk3

			KernelEvent ev = new KernelEvent();
			ev.HandleType = KernelHandleType.Event;
			ev.Uid = _kernel.AllocateUid();
			ev.Name = Kernel.ReadString( memory, a0 );
			ev.BitMask = a2;

			_kernel.AddHandle( ev );

			// SceUID
			return ev.Uid;
		}

		[BiosStub( 0xef9e4c70, "sceKernelDeleteEventFlag", true, 1 )]
		public int sceKernelDeleteEventFlag( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = int evid

			KernelHandle ev = _kernel.FindHandle( a0 );
			if( ( ev == null ) ||
				( ev.HandleType != KernelHandleType.Event ) )
				return -1;

			_kernel.RemoveHandle( ev );

			// int
			return 0;
		}

		[BiosStub( 0x1fb15a32, "sceKernelSetEventFlag", true, 2 )]
		public int sceKernelSetEventFlag( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = int evid
			// a1 = u32 bits

			KernelEvent ev = _kernel.FindHandle( a0 ) as KernelEvent;
			if( ev == null )
				return -1;

			ev.BitMask = a1;
			_kernel.SignalEvent( ev );

			// int
			return 0;
		}

		[BiosStub( 0x812346e4, "sceKernelClearEventFlag", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelClearEventFlag( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x402fcf22, "sceKernelWaitEventFlag", true, 5 )]
		public int sceKernelWaitEventFlag( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = int evid
			// a1 = u32 bits
			// a2 = u32 wait
			// a3 = u32 *outBits
			// sp[0] = void *arg
			int a4 = memory.ReadWord( sp + 0 );

			KernelEvent ev = _kernel.FindHandle( a0 ) as KernelEvent;
			if( ev == null )
				return -1;

			if( a2 == 0 )
			{
				// Immediate
				memory.WriteWord( a3, 4, ev.BitMask );
			}
			else
			{
				KernelThread thread = _kernel.ActiveThread;
				_kernel.WaitThreadOnEvent( thread, ev, a1, a3 );
			}

			// int
			return 0;
		}

		[BiosStub( 0x328c546a, "sceKernelWaitEventFlagCB", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelWaitEventFlagCB( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x30fd48f0, "sceKernelPollEventFlag", true, 4 )]
		[BiosStubIncomplete]
		public int sceKernelPollEventFlag( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = int evid
			// a1 = u32 bits
			// a2 = u32 wait
			// a3 = u32 *outBits

			KernelEvent ev = _kernel.FindHandle( a0 ) as KernelEvent;
			if( ev == null )
				return -1;

			// This should be really similar to waiteventflag

			// int
			return 0;
		}

		[BiosStub( 0xcd203292, "sceKernelCancelEventFlag", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelCancelEventFlag( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xa66b0120, "sceKernelReferEventFlagStatus", true, 2 )]
		[BiosStubIncomplete]
		public int sceKernelReferEventFlagStatus( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceUID event
			// a1 = SceKernelEventFlagInfo *status

			KernelEvent ev = _kernel.FindHandle( a0 ) as KernelEvent;
			if( ev == null )
				return -1;

			// int
			return 0;
		}

		#endregion

		#region Message Boxes

		[BiosStub( 0x8125221d, "sceKernelCreateMbx", true, 3 )]
		[BiosStubIncomplete]
		public int sceKernelCreateMbx( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = const char *name
			// a1 = SceUInt attr
			// a2 = SceKernelMbxOptParam *option

			// SceUID
			return 0;
		}

		[BiosStub( 0x86255ada, "sceKernelDeleteMbx", true, 1 )]
		[BiosStubIncomplete]
		public int sceKernelDeleteMbx( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceUID mbxid

			// int
			return 0;
		}

		[BiosStub( 0xe9b3061e, "sceKernelSendMbx", true, 2 )]
		[BiosStubIncomplete]
		public int sceKernelSendMbx( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceUID mbxid
			// a1 = void *message

			// int
			return 0;
		}

		[BiosStub( 0x18260574, "sceKernelReceiveMbx", true, 3 )]
		[BiosStubIncomplete]
		public int sceKernelReceiveMbx( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceUID mbxid
			// a1 = void * *pmessage
			// a2 = SceUInt *timeout

			// int
			return 0;
		}

		[BiosStub( 0xf3986382, "sceKernelReceiveMbxCB", true, 3 )]
		[BiosStubIncomplete]
		public int sceKernelReceiveMbxCB( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceUID mbxid
			// a1 = void * *pmessage
			// a2 = SceUInt *timeout

			// int
			return 0;
		}

		[BiosStub( 0x0d81716a, "sceKernelPollMbx", true, 2 )]
		[BiosStubIncomplete]
		public int sceKernelPollMbx( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceUID mbxid
			// a1 = void * *pmessage

			// int
			return 0;
		}

		[BiosStub( 0x87d4dd36, "sceKernelCancelReceiveMbx", true, 2 )]
		[BiosStubIncomplete]
		public int sceKernelCancelReceiveMbx( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceUID mbxid
			// a1 = int *pnum

			// int
			return 0;
		}

		[BiosStub( 0xa8e8c846, "sceKernelReferMbxStatus", true, 2 )]
		[BiosStubIncomplete]
		public int sceKernelReferMbxStatus( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceUID mbxid
			// a1 = SceKernelMbxInfo *info

			// int
			return 0;
		}

		#endregion

		#region Pipes

		[BiosStub( 0x7c0dc2a0, "sceKernelCreateMsgPipe", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelCreateMsgPipe( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xf0b7da1c, "sceKernelDeleteMsgPipe", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelDeleteMsgPipe( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x876dbfad, "sceKernelSendMsgPipe", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelSendMsgPipe( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x7c41f2c2, "sceKernelSendMsgPipeCB", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelSendMsgPipeCB( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x884c9f90, "sceKernelTrySendMsgPipe", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelTrySendMsgPipe( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x74829b76, "sceKernelReceiveMsgPipe", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelReceiveMsgPipe( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xfbfa697d, "sceKernelReceiveMsgPipeCB", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelReceiveMsgPipeCB( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xdf52098f, "sceKernelTryReceiveMsgPipe", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelTryReceiveMsgPipe( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x349b864d, "sceKernelCancelMsgPipe", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelCancelMsgPipe( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x33be4024, "sceKernelReferMsgPipeStatus", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelReferMsgPipeStatus( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		#endregion

		#region VPL / FPL

		[BiosStub( 0x56c039b5, "sceKernelCreateVpl", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelCreateVpl( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x89b3d48c, "sceKernelDeleteVpl", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelDeleteVpl( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xbed27435, "sceKernelAllocateVpl", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelAllocateVpl( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xec0a693f, "sceKernelAllocateVplCB", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelAllocateVplCB( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xaf36d708, "sceKernelTryAllocateVpl", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelTryAllocateVpl( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xb736e9ff, "sceKernelFreeVpl", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelFreeVpl( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x1d371b8a, "sceKernelCancelVpl", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelCancelVpl( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x39810265, "sceKernelReferVplStatus", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelReferVplStatus( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xc07bb470, "sceKernelCreateFpl", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelCreateFpl( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xed1410e0, "sceKernelDeleteFpl", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelDeleteFpl( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xd979e9bf, "sceKernelAllocateFpl", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelAllocateFpl( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xe7282cb6, "sceKernelAllocateFplCB", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelAllocateFplCB( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x623ae665, "sceKernelTryAllocateFpl", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelTryAllocateFpl( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xf6414a71, "sceKernelFreeFpl", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelFreeFpl( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xa8aa591f, "sceKernelCancelFpl", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelCancelFpl( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xd8199e4c, "sceKernelReferFplStatus", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelReferFplStatus( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		#endregion

		#region Time / Alarms

		[BiosStub( 0x0e927aed, "_sceKernelReturnFromTimerHandler", false, 0 )]
		[BiosStubIncomplete]
		public int _sceKernelReturnFromTimerHandler( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x110dec9a, "sceKernelUSec2SysClock", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelUSec2SysClock( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xc8cd158c, "sceKernelUSec2SysClockWide", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelUSec2SysClockWide( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xba6b92e2, "sceKernelSysClock2USec", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelSysClock2USec( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xe1619d7c, "sceKernelSysClock2USecWide", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelSysClock2USecWide( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xdb738f35, "sceKernelGetSystemTime", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelGetSystemTime( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x82bc5777, "sceKernelGetSystemTimeWide", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelGetSystemTimeWide( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x369ed59d, "sceKernelGetSystemTimeLow", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelGetSystemTimeLow( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x6652b8ca, "sceKernelSetAlarm", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelSetAlarm( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xb2c25152, "sceKernelSetSysClockAlarm", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelSetSysClockAlarm( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x7e65b999, "sceKernelCancelAlarm", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelCancelAlarm( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xdaa3f564, "sceKernelReferAlarmStatus", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelReferAlarmStatus( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		#endregion

		#region System / Profiler

		[BiosStub( 0x57cf62dd, "sceKernelGetThreadmanIdType", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelGetThreadmanIdType( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x64d4540e, "sceKernelReferThreadProfiler", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelReferThreadProfiler( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x8218b4dd, "sceKernelReferGlobalProfiler", false, 0 )]
		[BiosStubIncomplete]
		public int sceKernelReferGlobalProfiler( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		#endregion
	}
}
