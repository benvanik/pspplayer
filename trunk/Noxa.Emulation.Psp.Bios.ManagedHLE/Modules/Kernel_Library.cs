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
	class Kernel_Library : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "Kernel_Library";
			}
		}

		#endregion

		#region State Management

		public Kernel_Library( Kernel kernel )
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

		// We should hope - for performance sake, that these
		// are implemented by the CPU natively

		[Stateless]
		[BiosFunction( 0x092968F4, "sceKernelCpuSuspendIntr" )]
		// SDK location: /user/pspintrman.h:77
		// SDK declaration: unsigned int sceKernelCpuSuspendIntr();
		public int sceKernelCpuSuspendIntr()
		{
			uint cpuFlags = _kernel.Cpu.InterruptsMask;
			_kernel.Cpu.InterruptsMask = 0;
			return ( int )cpuFlags;
		}

		[BiosFunction( 0x5F10D406, "sceKernelCpuResumeIntr" )]
		// SDK location: /user/pspintrman.h:84
		// SDK declaration: void sceKernelCpuResumeIntr(unsigned int flags);
		public void sceKernelCpuResumeIntr( int flags )
		{
			_kernel.Cpu.InterruptsMask = ( uint )flags;
		}

		[BiosFunction( 0x3B84732D, "sceKernelCpuResumeIntrWithSync" )]
		// SDK location: /user/pspintrman.h:91
		// SDK declaration: void sceKernelCpuResumeIntrWithSync(unsigned int flags);
		public void sceKernelCpuResumeIntrWithSync( int flags )
		{
			_kernel.Cpu.InterruptsMask = ( uint )flags;
		}

		[Stateless]
		[BiosFunction( 0x47A0B729, "sceKernelIsCpuIntrSuspended" )]
		// SDK location: /user/pspintrman.h:100
		// SDK declaration: int sceKernelIsCpuIntrSuspended(unsigned int flags);
		public int sceKernelIsCpuIntrSuspended( int flags )
		{
			uint cpuFlags = _kernel.Cpu.InterruptsMask;
			return ( ( cpuFlags & ( uint )flags ) != 0 ) ? 1 : 0;
		}

		[Stateless]
		[BiosFunction( 0xB55249D2, "sceKernelIsCpuIntrEnable" )]
		// SDK location: /user/pspintrman.h:107
		// SDK declaration: int sceKernelIsCpuIntrEnable();
		public int sceKernelIsCpuIntrEnable()
		{
			return ( _kernel.Cpu.InterruptsMask != 0 ) ? 1 : 0;
		}

	}
}

/* GenerateStubsV2: auto-generated - 57B381EB */
