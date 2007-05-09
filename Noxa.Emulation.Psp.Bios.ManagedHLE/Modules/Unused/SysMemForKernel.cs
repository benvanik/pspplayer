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
	class SysMemForKernel : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "SysMemForKernel";
			}
		}

		#endregion

		#region State Management

		public SysMemForKernel( Kernel kernel )
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
		[BiosFunction( 0x1C1FBFE7, "sceKernelCreateHeap" )]
		int sceKernelCreateHeap(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC9805775, "sceKernelDeleteHeap" )]
		int sceKernelDeleteHeap(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xEB7A74DB, "sceKernelAllocHeapMemoryWithOption" )]
		int sceKernelAllocHeapMemoryWithOption(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x636C953B, "sceKernelAllocHeapMemory" )]
		int sceKernelAllocHeapMemory(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7B749390, "sceKernelFreeHeapMemory" )]
		int sceKernelFreeHeapMemory(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA823047E, "sceKernelHeapTotalFreeSize" )]
		int sceKernelHeapTotalFreeSize(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB2163AA1, "sceKernelGetHeapTypeCB" )]
		int sceKernelGetHeapTypeCB(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xEFF0C6DD, "SysMemForKernel_EFF0C6DD" )]
		int SysMemForKernel_EFF0C6DD(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xEFEEBAC7, "SysMemForKernel_EFEEBAC7" )]
		int SysMemForKernel_EFEEBAC7(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2DB687E9, "sceKernelIsValidHeap" )]
		int sceKernelIsValidHeap(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x55A40B2C, "sceKernelQueryMemoryPartitionInfo" )]
		int sceKernelQueryMemoryPartitionInfo(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE6581468, "sceKernelPartitionMaxFreeMemSize" )]
		int sceKernelPartitionMaxFreeMemSize(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9697CD32, "sceKernelPartitionTotalFreeMemSize" )]
		int sceKernelPartitionTotalFreeMemSize(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA2A65F0E, "sceKernelFillFreeBlock" )]
		int sceKernelFillFreeBlock(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x237DBD4F, "sceKernelAllocPartitionMemory" )]
		int sceKernelAllocPartitionMemory(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xEE867074, "sceKernelSizeLockMemoryBlock" )]
		int sceKernelSizeLockMemoryBlock(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCE5544F4, "sceKernelResizeMemoryBlock" )]
		int sceKernelResizeMemoryBlock(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5EBE73DE, "sceKernelJointMemoryBlock" )]
		int sceKernelJointMemoryBlock(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x915EF4AC, "SysMemForKernel_915EF4AC" )]
		int SysMemForKernel_915EF4AC(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB6D61D02, "sceKernelFreePartitionMemory" )]
		int sceKernelFreePartitionMemory(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2A3E5280, "sceKernelQueryMemoryInfo" )]
		int sceKernelQueryMemoryInfo(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xEB4C0E1B, "sceKernelQueryBlockSize" )]
		int sceKernelQueryBlockSize(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x82CCA14F, "sceKernelQueryMemoryBlockInfo" )]
		int sceKernelQueryMemoryBlockInfo(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9D9A5BA1, "sceKernelGetBlockHeadAddr" )]
		int sceKernelGetBlockHeadAddr(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2F3B7611, "SysMemForKernel_2F3B7611" )]
		int SysMemForKernel_2F3B7611(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6C1DCD41, "sceKernelCallUIDFunction" )]
		int sceKernelCallUIDFunction(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5367923C, "sceKernelCallUIDObjFunction" )]
		int sceKernelCallUIDObjFunction(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCE05CCB7, "SysMemForKernel_CE05CCB7" )]
		int SysMemForKernel_CE05CCB7(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6CD838EC, "sceKernelLookupUIDFunction" )]
		int sceKernelLookupUIDFunction(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xAD09C397, "sceKernelCreateUIDtypeInherit" )]
		int sceKernelCreateUIDtypeInherit(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFEFC8666, "sceKernelCreateUIDtype" )]
		int sceKernelCreateUIDtype(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD1BAB054, "sceKernelDeleteUIDtype" )]
		int sceKernelDeleteUIDtype(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1C221A08, "sceKernelGetUIDname" )]
		int sceKernelGetUIDname(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2E3402CC, "sceKernelRenameUID" )]
		int sceKernelRenameUID(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x39357F07, "sceKernelGetUIDtype" )]
		int sceKernelGetUIDtype(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x89A74008, "sceKernelCreateUID" )]
		int sceKernelCreateUID(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8F20C4C0, "sceKernelDeleteUID" )]
		int sceKernelDeleteUID(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x55BFD686, "sceKernelSearchUIDbyName" )]
		int sceKernelSearchUIDbyName(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCF4DE78C, "sceKernelGetUIDcontrolBlock" )]
		int sceKernelGetUIDcontrolBlock(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x41FFC7F9, "sceKernelGetUIDcontrolBlockWithType" )]
		int sceKernelGetUIDcontrolBlockWithType(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x82D3CEE3, "SysMemForKernel_82D3CEE3" )]
		int SysMemForKernel_82D3CEE3(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDFAEBD5B, "sceKernelIsHold" )]
		int sceKernelIsHold(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7BE95FA0, "sceKernelHoldUID" )]
		int sceKernelHoldUID(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFE8DEBE0, "sceKernelReleaseUID" )]
		int sceKernelReleaseUID(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBD5941B4, "sceKernelSysmemIsValidAccess" )]
		int sceKernelSysmemIsValidAccess(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x80F25772, "sceKernelIsValidUserAccess" )]
		int sceKernelIsValidUserAccess(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3905D956, "sceKernelSysMemCheckCtlBlk" )]
		int sceKernelSysMemCheckCtlBlk(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x26F96157, "sceKernelSysMemDump" )]
		int sceKernelSysMemDump(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6D6200DD, "sceKernelSysMemDumpBlock" )]
		int sceKernelSysMemDumpBlock(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x621037F5, "sceKernelSysMemDumpTail" )]
		int sceKernelSysMemDumpTail(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE400FDB0, "sceKernelSysMemInit" )]
		int sceKernelSysMemInit(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1890BE9C, "sceKernelSysMemMemSize" )]
		int sceKernelSysMemMemSize(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x03072750, "sceKernelSysMemMaxFreeMemSize" )]
		int sceKernelSysMemMaxFreeMemSize(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x811BED79, "sceKernelSysMemTotalFreeMemSize" )]
		int sceKernelSysMemTotalFreeMemSize(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF6C10E27, "sceKernelGetSysMemoryInfo" )]
		int sceKernelGetSysMemoryInfo(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCDA3A2F7, "SysMemForKernel_CDA3A2F7" )]
		int SysMemForKernel_CDA3A2F7(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x960B888C, "SysMemForKernel_960B888C" )]
		int SysMemForKernel_960B888C(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3FC9AE6A, "sceKernelDevkitVersion" )]
		int sceKernelDevkitVersion(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4887802F, "sceKernelMemset32" )]
		int sceKernelMemset32(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA089ECA4, "sceKernelMemset" )]
		int sceKernelMemset(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB2C7AA36, "sceKernelSetDdrMemoryProtection" )]
		int sceKernelSetDdrMemoryProtection(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - BF40CCF1 */
