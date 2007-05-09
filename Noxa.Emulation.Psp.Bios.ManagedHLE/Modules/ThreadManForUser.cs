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
	partial class ThreadManForUser : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "ThreadManForUser";
			}
		}

		#endregion

		#region State Management

		public ThreadManForUser( Kernel kernel )
			: base( kernel )
		{
		}

		public override void Start()
		{
		}

		public override void Stop()
		{
		}

		#endregion

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x627E6F3A, "sceKernelReferSystemStatus" )]
		// SDK location: /user/pspthreadman.h:1100
		// SDK declaration: int sceKernelReferSystemStatus(SceKernelSystemStatus *status);
		public int sceKernelReferSystemStatus( int status )
		{
			//typedef struct SceKernelSystemStatus {
			//    SceSize 	size;
			//    SceUInt 	status;
			//    SceKernelSysClock 	idleClocks;
			//    SceUInt 	comesOutOfIdleCount;
			//    SceUInt 	threadSwitchCount;
			//    SceUInt 	vfpuSwitchCount;
			//} SceKernelSystemStatus;

			/*
			SysClock idleClocks;
			idleClocks.QuadPart = ( int64 )0;// _kernel->IdleClocks;

			// Ensure 28 bytes
			if( memory->ReadWord( status ) != 28 )
			{
				Debug::WriteLine( String::Format( "ThreadManForUser: sceKernelReferSystemStatus app passed struct with size {0}, expected 28",
					memory->ReadWord( status ) ) );
				return -1;
			}
			memory->WriteWord( status +  4, 4, 1 ); // ?????
			memory->WriteWord( status +  8, 4, ( int )idleClocks.LowPart );
			memory->WriteWord( status + 12, 4, ( int )idleClocks.HighPart );
			//memory->WriteWord( status + 16, 4, ( int )_kernel->Statistics->LeaveIdleCount );
			//memory->WriteWord( status + 20, 4, ( int )_kernel->Statistics->ThreadSwitchCount );
			//memory->WriteWord( status + 24, 4, ( int )_kernel->Statistics->VfpuSwitchCount );
			memory->WriteWord( status + 16, 4, 0 );
			memory->WriteWord( status + 20, 4, 0 );
			memory->WriteWord( status + 24, 4, 0 );
			*/

			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x94416130, "sceKernelGetThreadmanIdList" )]
		// SDK location: /user/pspthreadman.h:1075
		// SDK declaration: int sceKernelGetThreadmanIdList(enum SceKernelIdListType type, SceUID *readbuf, int readbufsize, int *idcount);
		public int sceKernelGetThreadmanIdList( int type, int readbuf, int readbufsize, int idcount ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x57CF62DD, "sceKernelGetThreadmanIdType" )]
		// SDK location: /user/pspthreadman.h:1688
		// SDK declaration: enum SceKernelIdListType sceKernelGetThreadmanIdType(SceUID uid);
		public int sceKernelGetThreadmanIdType( int uid ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x64D4540E, "sceKernelReferThreadProfiler" )]
		public int sceKernelReferThreadProfiler(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8218B4DD, "sceKernelReferGlobalProfiler" )]
		public int sceKernelReferGlobalProfiler(){ return Module.NotImplementedReturn; }
	}
}

/* GenerateStubsV2: auto-generated - B40C8E82 */
