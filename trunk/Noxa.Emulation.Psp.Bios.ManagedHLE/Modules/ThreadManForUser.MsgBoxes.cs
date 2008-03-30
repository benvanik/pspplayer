// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

using Noxa.Utilities;
using Noxa.Emulation.Psp;
using Noxa.Emulation.Psp.Bios;
using Noxa.Emulation.Psp.Cpu;

namespace Noxa.Emulation.Psp.Bios.ManagedHLE.Modules
{
	partial class ThreadManForUser
	{
		[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 52)]
		unsafe public struct SceKernelMbxInfo
		{
			public int size;
			public fixed char name[32];
			public int attr;
			public int numWaitThreads;
			public int numMessages;
			public int firstMessage;
		};

		[Stateless]
		[BiosFunction( 0x8125221D, "sceKernelCreateMbx" )]
		// SDK location: /user/pspthreadman.h:774
		// SDK declaration: SceUID sceKernelCreateMbx(const char *name, SceUInt attr, SceKernelMbxOptParam *option);
		public int sceKernelCreateMbx( int _name, int attr, int option )
		{
			string name = _kernel.ReadString((uint)_name);

			KMessageBox handle = new KMessageBox(_kernel, name, attr );
			_kernel.AddHandle(handle);

			Log.WriteLine( Verbosity.Normal, Feature.Bios, "sceKernelCreateMbx: opened box {0} with ID {1:X}", name, handle.UID );
			return (int)handle.UID;
		}

		[Stateless]
		[BiosFunction( 0x86255ADA, "sceKernelDeleteMbx" )]
		// SDK location: /user/pspthreadman.h:782
		// SDK declaration: int sceKernelDeleteMbx(SceUID mbxid);
		public int sceKernelDeleteMbx(int mbxid)
		{
			KMessageBox box = _kernel.GetHandle<KMessageBox>(mbxid);
			if (box == null)
			{
				return unchecked((int)0x8002019B);
			}

			box.CancelThreads();
			_kernel.RemoveHandle(box.UID);
			return 0;
		}
		
		[Stateless]
		[BiosFunction( 0xE9B3061E, "sceKernelSendMbx" )]
		// SDK location: /user/pspthreadman.h:806
		// SDK declaration: int sceKernelSendMbx(SceUID mbxid, void *message);
		public int sceKernelSendMbx(int mbxid, int message)
		{
			KMessageBox box = _kernel.GetHandle<KMessageBox>(mbxid);
			
			if (box == null)
			{
				return unchecked((int)0x8002019B);
			}
			
			box.AddMessage(message);
			return 0;
		}

		[Stateless]
		[BiosFunction( 0x18260574, "sceKernelReceiveMbx" )]
		// SDK location: /user/pspthreadman.h:824
		// SDK declaration: int sceKernelReceiveMbx(SceUID mbxid, void **pmessage, SceUInt *timeout);
		public int sceKernelReceiveMbx( int mbxid, int pmessage, int timeout )
		{
			KMessageBox box = _kernel.GetHandle<KMessageBox>(mbxid);
			if (box == null)
			{
				return unchecked((int)0x8002019B);
			}
			
			return box.MaybeWait(pmessage, timeout, false);
		}
		
		[Stateless]
		[BiosFunction( 0xF3986382, "sceKernelReceiveMbxCB" )]
		// SDK location: /user/pspthreadman.h:842
		// SDK declaration: int sceKernelReceiveMbxCB(SceUID mbxid, void **pmessage, SceUInt *timeout);
		public int sceKernelReceiveMbxCB( int mbxid, int pmessage, int timeout )
		{
			KMessageBox box = _kernel.GetHandle<KMessageBox>(mbxid);
			if (box == null)
			{
				return unchecked((int)0x8002019B);
			}

			return box.MaybeWait(pmessage, timeout, true);
		}

		[Stateless]
		[BiosFunction( 0x0D81716A, "sceKernelPollMbx" )]
		// SDK location: /user/pspthreadman.h:859
		// SDK declaration: int sceKernelPollMbx(SceUID mbxid, void **pmessage);
		public int sceKernelPollMbx( int mbxid, int pmessage )
		{
			KMessageBox box = _kernel.GetHandle<KMessageBox>(mbxid);
			if (box == null)
			{
				return unchecked((int)0x8002019B);
			}

			if (box.Messages.Count == 0)
			{
				return unchecked((int)0x800201b2); // SCE_KERNEL_ERROR_MBOX_NOMSG
			}

			box.GetMessage(pmessage);
			return 0;
		}

		[Stateless]
		[BiosFunction( 0x87D4DD36, "sceKernelCancelReceiveMbx" )]
		// SDK location: /user/pspthreadman.h:876
		// SDK declaration: int sceKernelCancelReceiveMbx(SceUID mbxid, int *pnum);
		public int sceKernelCancelReceiveMbx( int mbxid, int pnum )
		{
			KMessageBox box = _kernel.GetHandle<KMessageBox>(mbxid);
			if (box == null)
			{
				return unchecked((int)0x8002019B);
			}

			unsafe
			{
				*(int*)_kernel.MemorySystem.Translate((uint)pnum) = box.WaitingThreads.Count;
			}

			while (box.WaitingThreads.Count > 0)
			{
				KThread thread = box.WaitingThreads.Dequeue();
				thread.Wake(unchecked((int)0x800201a9)); // SCE_KERNEL_ERROR_WAIT_CANCEL
			}

			return 0;
		}

		[Stateless]
		[BiosFunction( 0xA8E8C846, "sceKernelReferMbxStatus" )]
		// SDK location: /user/pspthreadman.h:886
		// SDK declaration: int sceKernelReferMbxStatus(SceUID mbxid, SceKernelMbxInfo *info);
		public int sceKernelReferMbxStatus( int mbxid, int pinfo )
		{
			KMessageBox box = _kernel.GetHandle<KMessageBox>(mbxid);
			if (box == null)
			{
				return unchecked((int)0x8002019B);
			}

			unsafe
			{
				SceKernelMbxInfo* info = (SceKernelMbxInfo*)_kernel.MemorySystem.Translate((uint)pinfo);
				info->size = 52;
				_kernel.WriteString((uint)(pinfo + 4), box.Name.Substring(0, 31));
				info->attr = box.Attributes;
				info->numWaitThreads = box.WaitingThreads.Count;
				info->numMessages = box.Messages.Count;
				info->firstMessage = box.Messages.Head;
			}
			
			return 0;
		}
	}
}
