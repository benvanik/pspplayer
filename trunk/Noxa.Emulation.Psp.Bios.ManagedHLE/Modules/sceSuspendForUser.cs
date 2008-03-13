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
	class sceSuspendForUser : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceSuspendForUser";
			}
		}

		#endregion

		#region State Management

		public sceSuspendForUser( Kernel kernel )
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

		// Currently just ignore power switch locking - maybe one day
		// we should obey it so saves don't get corrupt/etc?

		[DontTrace]
		[SuggestNative]
		[Stateless]
		[BiosFunction( 0xEADB1BD7, "sceKernelPowerLock" )]
		// manual add
		public int sceKernelPowerLock( int type )
		{
			return 0;
		}

		[DontTrace]
		[SuggestNative]
		[Stateless]
		[BiosFunction( 0x3AEE7261, "sceKernelPowerUnlock" )]
		// manual add
		public int sceKernelPowerUnlock( int type )
		{
			return 0;
		}

		[DontTrace]
		[SuggestNative]
		[Stateless]
		[BiosFunction( 0x090CCB3F, "sceKernelPowerTick" )]
		// int sceKernelPowerTick(int ticktype); (/include/kernelutils.h:167)
		public int sceKernelPowerTick( int type )
		{
			return 0;
		}

		[SuggestNative]
		[Stateless]
		[BiosFunction( 0x3E0271D3, "sceKernelVolatileMemLock" )]
		// manual add
		public int sceKernelVolatileMemLock( int unk, int ptr, int size )
		{
			return this.sceKernelVolatileMemTryLock( unk, ptr, size );
		}

		[SuggestNative]
		[Stateless]
		[BiosFunction( 0xA14F40B2, "sceKernelVolatileMemTryLock" )]
		// manual add
		public unsafe int sceKernelVolatileMemTryLock( int unk, int ptr, int size )
		{
			byte* pptr = _memorySystem.TranslateMainMemory( ptr );
			*( ( uint* )pptr ) = 0x08400000;
			byte* psize = _memorySystem.TranslateMainMemory( size );
			*( ( uint* )psize ) = 0x00400000;
			return 0;
		}

		[SuggestNative]
		[Stateless]
		[BiosFunction( 0xA569E425, "sceKernelVolatileMemUnlock" )]
		// manual add
		public int sceKernelVolatileMemUnlock()
		{
			return 0;
		}
	}
}

/* GenerateStubsV2: auto-generated - 8C665047 */
