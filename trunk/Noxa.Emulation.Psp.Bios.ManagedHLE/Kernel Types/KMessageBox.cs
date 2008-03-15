using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.IO;

namespace Noxa.Emulation.Psp.Bios.ManagedHLE
{
	class KMessageBox : KHandle
	{
		public Kernel Kernel;

		public string Name;
		public int Attributes;
		public FastLinkedList<int> Messages;
		public FastLinkedList<KThread> WaitingThreads;

		public KMessageBox(Kernel kernel, string name, int attr)
		{
			this.Kernel = kernel;
			this.Name = name;
			this.Attributes = attr;
			this.Messages = new FastLinkedList<int>();
			this.WaitingThreads = new FastLinkedList<KThread>();
		}

		public void AddMessage(int message)
		{
			if (this.WaitingThreads.Count > 0 && this.Messages.Count == 0)
			{
				KThread thread = this.WaitingThreads.Dequeue();
				unsafe
				{
					*(int*)Kernel.MemorySystem.Translate(thread.WaitAddress) = message;
				}
				thread.Wake();
				return;
			}

			this.Messages.Enqueue(message);
		}

		public void GetMessage(int message)
		{
			Debug.Assert(this.Messages.Count > 0);
			unsafe
			{
				*(int*)Kernel.MemorySystem.Translate((uint)message) = this.Messages.Dequeue();
			}
		}

		public void CancelThreads()
		{
			while (this.WaitingThreads.Count > 0)
			{
				KThread thread = this.WaitingThreads.Dequeue();
				thread.Wake(unchecked((int)0x800201b5)); // SCE_KERNEL_ERROR_WAIT_DELETE
			}
		}

		public void Signal()
		{
			// Try to wake threads
			bool wokeThreads = false;

			LinkedListEntry<KThread> entry = WaitingThreads.HeadEntry;
			while( entry != null )
			{
				if (this.Messages.Count > 0)
				{
					this.GetMessage((int)entry.Value.WaitAddress);
					entry.Value.Wake(0);
					wokeThreads = true;
				}
				else
				{
					break;
				}
			}

			if (wokeThreads == true)
			{
				Kernel.Schedule();
			}
		}
		
		public int MaybeWait(int pmessage, int timeout, bool allowCallbacks)
		{
			if (this.Messages.Count > 0)
			{
				this.GetMessage(pmessage);
				return 0;
			}
			
			this.Wait( pmessage, timeout, allowCallbacks );
			return 0;

		}

		public void Wait(int pmessage, int timeout, bool allowCallbacks)
		{
			this.Kernel.ActiveThread.Wait(this, pmessage, timeout, allowCallbacks);
		}
	}
}
