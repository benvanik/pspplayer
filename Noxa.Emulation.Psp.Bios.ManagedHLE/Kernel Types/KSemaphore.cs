// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace Noxa.Emulation.Psp.Bios.ManagedHLE
{
	class KSemaphore : KHandle
	{
		public Kernel Kernel;

		public string Name;
		public uint Attributes;

		public int InitialCount;
		public int CurrentCount;
		public int MaximumCount;

		public FastLinkedList<KThread> WaitingThreads;

		public KSemaphore( Kernel kernel, string name, uint attributes, int initialCount, int maximumCount )
		{
			Kernel = kernel;

			Name = name;
			Attributes = attributes;

			InitialCount = initialCount;
			CurrentCount = initialCount;
			MaximumCount = maximumCount;

			WaitingThreads = new FastLinkedList<KThread>();
		}

		public void Signal( int count )
		{
			CurrentCount += count;

			// Try to wake threads
			bool wokeThreads = false;
			LinkedListEntry<KThread> e = WaitingThreads.HeadEntry;
			while( e != null )
			{
				int needed = ( int )e.Value.WaitArgument;
				if( CurrentCount >= needed )
				{
					e.Value.Wake( 0 );
					CurrentCount -= needed;
					wokeThreads = true;
				}
				else
					break;
			}
			if( wokeThreads == true )
				Kernel.Schedule();
		}

		public void Wait( bool allowCallbacks, int count, uint timeoutUs )
		{
			if( CurrentCount >= count )
			{
				CurrentCount -= count;
			}
			else
			{
				KThread thread = Kernel.ActiveThread;
				thread.Wait( this, count, timeoutUs, allowCallbacks );
				Kernel.Schedule();
			}
		}
	}
}
