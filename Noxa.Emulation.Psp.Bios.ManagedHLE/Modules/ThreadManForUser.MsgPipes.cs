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
		[Stateless]
		[BiosFunction( 0x7C0DC2A0, "sceKernelCreateMsgPipe" )]
		// SDK location: /user/pspthreadman.h:1114
		// SDK declaration: SceUID sceKernelCreateMsgPipe(const char *name, int part, int attr, void *unk1, void *opt);
		public int sceKernelCreateMsgPipe( int _name, int part, int attr, int unk1, int opt )
		{
            string name = _kernel.ReadString((uint)_name);

            KPartition partition = _kernel.Partitions[part];
            Debug.Assert(partition != null);
            if (partition == null)
            {
                return -1;
            }
            
            KPipe handle = new KPipe(_kernel, partition, name);
            _kernel.AddHandle(handle);

            Log.WriteLine(Verbosity.Normal, Feature.Bios, "sceKernelCreateMsgPipe: opened pipe {0} with ID {1}, for partition {2}", name, handle.UID, part);

            return (int)handle.UID;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF0B7DA1C, "sceKernelDeleteMsgPipe" )]
		// SDK location: /user/pspthreadman.h:1123
		// SDK declaration: int sceKernelDeleteMsgPipe(SceUID uid);
		public int sceKernelDeleteMsgPipe( int uid )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x876DBFAD, "sceKernelSendMsgPipe" )]
		// SDK location: /user/pspthreadman.h:1137
		// SDK declaration: int sceKernelSendMsgPipe(SceUID uid, void *message, unsigned int size, int unk1, void *unk2, unsigned int *timeout);
		public int sceKernelSendMsgPipe( int uid, int message, int size, int unk1, int unk2, int timeout )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7C41F2C2, "sceKernelSendMsgPipeCB" )]
		// SDK location: /user/pspthreadman.h:1151
		// SDK declaration: int sceKernelSendMsgPipeCB(SceUID uid, void *message, unsigned int size, int unk1, void *unk2, unsigned int *timeout);
		public int sceKernelSendMsgPipeCB( int uid, int message, int size, int unk1, int unk2, int timeout )
		{
			return Module.NotImplementedReturn;
		}

		[Stateless]
		[BiosFunction( 0x884C9F90, "sceKernelTrySendMsgPipe" )]
		// SDK location: /user/pspthreadman.h:1164
		// SDK declaration: int sceKernelTrySendMsgPipe(SceUID uid, void *message, unsigned int size, int unk1, void *unk2);
		public int sceKernelTrySendMsgPipe( int uid, int message, int size, int unk1, int unk2 )
		{
            KPipe handle = _kernel.GetHandle<KPipe>(uid);
            if (handle == null)
            {
                Log.WriteLine(Verbosity.Normal, Feature.Bios, "sceKernelTrySendMsgPipe: kernel pipe handle not found: {0}", uid);
                return -1;
            }
            
            if (_memory.ReadStream(message, handle.Stream, size) != size)
            {
                Log.WriteLine(Verbosity.Normal, Feature.Bios, "sceKernelTrySendMsgPipe: could not read enough for {0}", uid);
                return -1;
            }

            Log.WriteLine(Verbosity.Normal, Feature.Bios, "sceKernelTrySendMsgPipe: {0} {1:X8} {2} {3:X8} {4:X8}", uid, message, size, unk1, unk2);
            //handle.Stream.Seek(0, SeekOrigin.Begin);
            return 0;
		}

		[Stateless]
		[BiosFunction( 0x74829B76, "sceKernelReceiveMsgPipe" )]
		// SDK location: /user/pspthreadman.h:1178
		// SDK declaration: int sceKernelReceiveMsgPipe(SceUID uid, void *message, unsigned int size, int unk1, void *unk2, unsigned int *timeout);
		public int sceKernelReceiveMsgPipe( int uid, int message, int size, int unk1, int unk2, int timeout )
		{
            KPipe handle = _kernel.GetHandle<KPipe>(uid);
            if (handle == null)
            {
                Log.WriteLine(Verbosity.Normal, Feature.Bios, "sceKernelReceiveMsgPipe: kernel pipe handle not found: {0}", uid);
                return -1;
            }

            Log.WriteLine(Verbosity.Normal, Feature.Bios, "sceKernelReceiveMsgPipe: {0} {1:X8} {2} {3:X8} {4:X8} {5}", uid, message, size, unk1, unk2, timeout);

            if (handle.Stream.Length < size)
            {
                return -1;
            }
            
            handle.Stream.Seek(0, SeekOrigin.Begin);
            _memory.WriteStream(message, handle.Stream, size);
            handle.Stream.SetLength(0);
            return 0;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFBFA697D, "sceKernelReceiveMsgPipeCB" )]
		// SDK location: /user/pspthreadman.h:1192
		// SDK declaration: int sceKernelReceiveMsgPipeCB(SceUID uid, void *message, unsigned int size, int unk1, void *unk2, unsigned int *timeout);
		public int sceKernelReceiveMsgPipeCB( int uid, int message, int size, int unk1, int unk2, int timeout )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDF52098F, "sceKernelTryReceiveMsgPipe" )]
		// SDK location: /user/pspthreadman.h:1205
		// SDK declaration: int sceKernelTryReceiveMsgPipe(SceUID uid, void *message, unsigned int size, int unk1, void *unk2);
		public int sceKernelTryReceiveMsgPipe( int uid, int message, int size, int unk1, int unk2 )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x349B864D, "sceKernelCancelMsgPipe" )]
		// SDK location: /user/pspthreadman.h:1216
		// SDK declaration: int sceKernelCancelMsgPipe(SceUID uid, int *psend, int *precv);
		public int sceKernelCancelMsgPipe( int uid, int psend, int precv )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x33BE4024, "sceKernelReferMsgPipeStatus" )]
		// SDK location: /user/pspthreadman.h:1237
		// SDK declaration: int sceKernelReferMsgPipeStatus(SceUID uid, SceKernelMppInfo *info);
		public int sceKernelReferMsgPipeStatus( int uid, int info )
		{
			return Module.NotImplementedReturn;
		}
	}
}
