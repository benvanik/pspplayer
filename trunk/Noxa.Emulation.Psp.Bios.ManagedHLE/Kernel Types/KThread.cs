// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Noxa.Emulation.Psp.Bios.ManagedHLE
{
	enum KThreadWait
	{
		Sleep,			// Wait until woken up
		Delay,			// Wait until a specific time
		Semaphore,		// Waiting on semaphore signal
		Event,			// Wait until an event matches
		Mbx,			// mailbox something
		Vpl,			// Wait until VPL has space
		Fpl,			// Wait until FPL has space
		Mpp,			// message pipe stuff
		Join,			// Wait until the target thread ends
		EventHandler,	// message pump something
		Unknown2,		// ?? callbacks?
	}

	enum KThreadState
	{
		Running = 1,
		Ready = 2,
		Waiting = 4,
		Suspended = 8,
		Stopped = 16,
		Dead = 32,
	}

	[Flags]
	enum KThreadAttributes : uint
	{
		VFPU = 0x00004000,			// Allow VFPU usage
		User = 0x80000000,			// Start thread in user mode
		UsbWlan = 0xA0000000,		// Thread is part of USB/WLAN API
		Vsh = 0xC0000000,			// Thread is part of VSH API
		ScratchSram = 0x00008000,	// Allow scratchpad usage
		NoFillStack = 0x00100000,	// Don't fill stack with 0xFF on create
		ClearStack = 0x00200000,	// Clear stack when thread deleted
	}

	partial class KThread : KHandle, IDisposable
	{
		public Kernel Kernel;

		public string Name;
		public uint EntryAddress;
		public uint GlobalPointer;
		public int InitialPriority;
		public int Priority;			// 0-32 - lower = higher
		public KThreadAttributes Attributes;

		public KThreadState State;
		public bool Suspended;
		public int ExitCode;

		public KPartition Partition;
		public KMemoryBlock StackBlock;
		public KMemoryBlock TlsBlock;

		public FastLinkedList<KThread> ExitWaiters;

		public int ContextID;		// TCS ID in CPU

		public long RunClocks;
		public uint WakeupCount;
		public uint ReleaseCount;
		public uint InterruptPreemptionCount;
		public uint ThreadPreemptionCount;

		public bool CanHandleCallbacks;

		// Wait junk
		public KThreadWait WaitingOn;
		public long WaitTimestamp;			// Time wait was set
		public uint WaitTimeout;			// 0 for infinite - either a timeout or an end time of KThreadWaitDelay - in ticks!
		public KHandle WaitHandle;			// Event/pool/etc waiting on
		public KWaitType WaitEventMode;		// Event wait mode (if WaitingOn = KThreadWaitEvent)
		public uint WaitArgument;
		public uint WaitAddress;			// For output parameters

		public KThread( Kernel kernel, KPartition partition, string name, uint entryAddress, int priority, KThreadAttributes attributes, uint stackSize )
		{
			Debug.Assert( partition != null );

			Kernel = kernel;

			Name = name;
			EntryAddress = entryAddress;
			InitialPriority = priority;
			Priority = priority;
			Attributes = attributes;
			
			State = KThreadState.Stopped;

			ExitWaiters = new FastLinkedList<KThread>();

			RunClocks = 0;
			InterruptPreemptionCount = 0;
			ThreadPreemptionCount = 0;

			Partition = partition;
			//StackBlock = partition.Allocate( KAllocType.High, 0, stackSize );
			StackBlock = partition.Allocate( KAllocType.Low, 0, stackSize );
			Debug.Assert( StackBlock != null );
			StackBlock.Name = string.Format( "Thread '{0}' Stack", name );
			TlsBlock = partition.Allocate( KAllocType.Low, 0, 0x4000 ); // 16k of thread local storage --- enough?
			Debug.Assert( TlsBlock != null );
			TlsBlock.Name = string.Format( "Thread '{0}' TLS", name );
		}

		~KThread()
		{
			this.Dispose();
		}

		public void Dispose()
		{
			GC.SuppressFinalize( this );

			if( State != KThreadState.Dead )
				this.Exit( 0 );
			if( ContextID >= 0 )
				this.Delete();
		}

		public void Start( uint argumentsLength, uint argumentsPointer )
		{
			uint[] registers = new uint[ 32 ];
			registers[ 4 ] = argumentsLength;
			registers[ 5 ] = argumentsPointer;
			registers[ 6 ] = 0;
			if( TlsBlock != null )
				registers[ 26 ] = TlsBlock.Address;		// TLS
			else
				registers[ 26 ] = 0;
			registers[ 28 ] = GlobalPointer;			// gp - set by cpu?
			registers[ 29 ] = StackBlock.UpperBound;

			ContextID = Kernel.Cpu.AllocateContextStorage( EntryAddress, registers );

			State = KThreadState.Ready;

			Kernel.Threads.Add( this );
			this.AddToSchedule();
		}

		public void Exit( int code )
		{
			State = KThreadState.Dead;
			this.RemoveFromSchedule();
			
			ExitCode = code;

			while( ExitWaiters.Count > 0 )
			{
				KThread thread = ExitWaiters.Dequeue();
				thread.Wake( 0 );
			}
		}

		public void Delete()
		{
			Partition.Free( StackBlock );
			if( TlsBlock != null )
				Partition.Free( TlsBlock );
			StackBlock = null;
			TlsBlock = null;

			Kernel.Cpu.ReleaseContextStorage( ContextID );
			ContextID = -1;

			Kernel.Threads.Remove( this );
		}

		public void ChangePriority( int newPriority )
		{
			Priority = newPriority;

			this.RemoveFromSchedule();
			this.AddToSchedule();
		}

		private void AddToSchedule()
		{
			// This can happen sometimes (race conditions)
			bool alreadyScheduled = ( Kernel.SchedulableThreads.Find( this ) != null );
			if( alreadyScheduled == true )
			{
				if( this.State != KThreadState.Running )
					this.State = KThreadState.Ready;
				Log.WriteLine( Verbosity.Verbose, Feature.Bios, "KThread: AddToSchedule found thread already scheduled" );
				return;
			}

			// Find the right place by walking the thread list and inserting before a thread of higher priority
			LinkedListEntry<KThread> e = Kernel.SchedulableThreads.HeadEntry;
			if( e == null )
			{
				// Special case
				Kernel.SchedulableThreads.Enqueue( this );
			}
			else
			{
				do
				{
					if( e.Value.Priority >= this.Priority )
					{
						// Insert here (this will be before any threads with the same priority)
						Kernel.SchedulableThreads.InsertBefore( this, e );
						break;
					}

					if( e.Next == null )
					{
						// Append
						Kernel.SchedulableThreads.Enqueue( this );
						break;
					}

					e = e.Next;
				} while( e != null );
			}
		}

		private void RemoveFromSchedule()
		{
			Kernel.SchedulableThreads.Remove( this );
		}
	}
}
