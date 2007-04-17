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
		public int sceKernelReferSystemStatus( int status ){ return Module.NotImplementedReturn; }

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
	}
}

/* GenerateStubsV2: auto-generated - D8829FE8 */
