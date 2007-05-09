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
	class SysclibForKernel : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "SysclibForKernel";
			}
		}

		#endregion

		#region State Management

		public SysclibForKernel( Kernel kernel )
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
		[BiosFunction( 0x476FD94A, "strcat" )]
		int strcat(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x89B79CB1, "strcspn" )]
		int strcspn(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD1CD40E5, "index" )]
		int index(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x243665ED, "rindex" )]
		int rindex(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x90C5573D, "strnlen" )]
		int strnlen(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0DFB7B6C, "strpbrk" )]
		int strpbrk(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x62AE052F, "strspn" )]
		int strspn(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0D188658, "strstr" )]
		int strstr(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x87F8D2DA, "strtok" )]
		int strtok(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1AB53A58, "strtok_r" )]
		int strtok_r(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x47DD934D, "strtol" )]
		int strtol(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1D83F344, "atob" )]
		int atob(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6A7900E1, "strtoul" )]
		int strtoul(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8AF6B8F8, "SysclibForKernel_8AF6B8F8" )]
		int SysclibForKernel_8AF6B8F8(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDF17F4A2, "SysclibForKernel_DF17F4A2" )]
		int SysclibForKernel_DF17F4A2(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7DEE14DE, "SysclibForKernel_7DEE14DE" )]
		int SysclibForKernel_7DEE14DE(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5E8E5F42, "SysclibForKernel_5E8E5F42" )]
		int SysclibForKernel_5E8E5F42(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC0AB8932, "strcmp" )]
		int strcmp(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xEC6F1CF2, "strcpy" )]
		int strcpy(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB1DC2AE8, "strchr" )]
		int strchr(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4C0E0274, "strrchr" )]
		int strrchr(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7AB35214, "strncmp" )]
		int strncmp(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB49A7697, "strncpy" )]
		int strncpy(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x52DF196C, "strlen" )]
		int strlen(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD3D1A3B9, "strncat" )]
		int strncat(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x68A78817, "memchr" )]
		int memchr(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xAB7592FF, "memcpy" )]
		int memcpy(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x10F3BB61, "memset" )]
		int memset(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x81D0D1F7, "memcmp" )]
		int memcmp(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA48D2592, "memmove" )]
		int memmove(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x097049BD, "bcopy" )]
		int bcopy(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7F8A6F23, "bcmp" )]
		int bcmp(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x86FEFCE9, "bzero" )]
		int bzero(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCE2F7487, "toupper" )]
		int toupper(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3EC5BBF6, "tolower" )]
		int tolower(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x32C767F2, "look_ctype_table" )]
		int look_ctype_table(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD887CACD, "get_ctype_table" )]
		int get_ctype_table(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x87C78FB6, "prnt" )]
		int prnt(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7661E728, "sprintf" )]
		int sprintf(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x909C228B, "setjmp" )]
		int setjmp(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x18FE80DB, "longjmp" )]
		int longjmp(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1493EBD9, "wmemset" )]
		int wmemset(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - 89CB2456 */
