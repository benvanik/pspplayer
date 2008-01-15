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
	class KMutex : KHandle
	{
		public Kernel Kernel;

		public string Name;
		public uint Attributes;

		public bool Locked;

		public FastLinkedList<KThread> WaitingThreads;

		public KMutex( Kernel kernel, string name, uint attributes )
		{
			Kernel = kernel;

			Name = name;
			Attributes = attributes;

			WaitingThreads = new FastLinkedList<KThread>();
		}

		// timeoutUs may not be right
		public void Lock( uint timeoutUs )
		{
			if( Locked == false )
				Locked = true;
			else
			{
				KThread thread = Kernel.ActiveThread;
				thread.Wait( this, timeoutUs );
				Kernel.Schedule();
			}
		}

		public void Unlock()
		{
			// Try to wake threads
			if( WaitingThreads.Count > 0 )
			{
				KThread thread = WaitingThreads.Dequeue();
				thread.Wake( 0 );

				// Remain locked, as that thread now holds the lock

				Kernel.Schedule();
			}
			else
				Locked = false;
		}
	}
}
