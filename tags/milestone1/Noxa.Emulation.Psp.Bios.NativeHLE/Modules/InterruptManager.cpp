// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "Stdafx.h"
#define WIN32_LEAN_AND_MEAN
#include <Windows.h>

#include "InterruptManager.h"
#include "Kernel.h"
#include "KIntHandler.h"

using namespace System;
using namespace System::Diagnostics;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Bios;
using namespace Noxa::Emulation::Psp::Bios::Modules;
using namespace Noxa::Emulation::Psp::Cpu;

enum PspInterrupts
{
	PSP_GPIO_INT		= 4,
	PSP_ATA_INT			= 5,
	PSP_UMD_INT			= 6,
	PSP_MSCM0_INT		= 7,
	PSP_WLAN_INT		= 8,
	PSP_AUDIO_INT		= 10,
	PSP_I2C_INT			= 12,
	PSP_SIRCS_INT		= 14,
	PSP_SYSTIMER0_INT	= 15,
	PSP_SYSTIMER1_INT	= 16,
	PSP_SYSTIMER2_INT	= 17,
	PSP_SYSTIMER3_INT	= 18,
	PSP_THREAD0_INT		= 19,
	PSP_NAND_INT		= 20,
	PSP_DMACPLUS_INT	= 21,
	PSP_DMA0_INT		= 22,
	PSP_DMA1_INT		= 23,
	PSP_MEMLMD_INT		= 24,
	PSP_GE_INT			= 25,
	PSP_VBLANK_INT		= 30,
	PSP_MECODEC_INT		= 31,
	PSP_HPREMOTE_INT	= 36,
	PSP_MSCM1_INT		= 60,
	PSP_MSCM2_INT		= 61,
	PSP_THREAD1_INT		= 65,
	PSP_INTERRUPT_INT	= 66,
};

enum PspSubInterrupts
{
	PSP_GPIO_SUBINT		= PSP_GPIO_INT,
	PSP_ATA_SUBINT		= PSP_ATA_INT,
	PSP_UMD_SUBINT		= PSP_UMD_INT,
	PSP_DMACPLUS_SUBINT	= PSP_DMACPLUS_INT,
	PSP_GE_SUBINT		= PSP_GE_INT,
	PSP_DISPLAY_SUBINT	= PSP_VBLANK_INT,
};

void* InterruptManager::QueryNativePointer( uint nid )
{
	//switch( nid )
	//{
	//case 0xCA04A2B9:
	//case 0xD61E6961:
	//case 0xFB8E22EC:
	//case 0x8A389411:
	//case 0xD2E8363F:
	//}
	return 0;
}

// int sceKernelRegisterSubIntrHandler(int intno, int no, void *handler, void *arg); (/user/pspintrman.h:119)
int InterruptManager::sceKernelRegisterSubIntrHandler( int intno, int no, int handler, int arg )
{
	KIntHandler* inth = new KIntHandler( _kernel, intno, no, handler, arg );

	assert( _kernel->Interrupts[ intno ][ no ] == NULL );
	_kernel->Interrupts[ intno ][ no ] = inth;

	Debug::WriteLine( String::Format( "sceKernelRegisterSubIntrHandler: registered handler for interrupt {0} (slot {1}), calling code at {2:X8}", intno, no, handler ) );

	return 0;
}

// int sceKernelReleaseSubIntrHandler(int intno, int no); (/user/pspintrman.h:129)
int InterruptManager::sceKernelReleaseSubIntrHandler( int intno, int no )
{
	KIntHandler* inth = _kernel->Interrupts[ intno ][ no ];
	assert( inth != NULL );
	if( inth == NULL )
		return -1;

	_kernel->Interrupts[ intno ][ no ] = NULL;

	delete inth;

	return 0;
}

// int sceKernelEnableSubIntr(int intno, int no); (/user/pspintrman.h:139)
int InterruptManager::sceKernelEnableSubIntr( int intno, int no )
{
	KIntHandler* inth = _kernel->Interrupts[ intno ][ no ];
	assert( inth != NULL );
	if( inth == NULL )
		return -1;

	inth->SetEnabled( true );

	return 0;
}

// int sceKernelDisableSubIntr(int intno, int no); (/user/pspintrman.h:149)
int InterruptManager::sceKernelDisableSubIntr( int intno, int no )
{
	KIntHandler* inth = _kernel->Interrupts[ intno ][ no ];
	assert( inth != NULL );
	if( inth == NULL )
		return -1;

	inth->SetEnabled( false );

	return 0;
}

// int QueryIntrHandlerInfo(SceUID intr_code, SceUID sub_intr_code, PspIntrHandlerOptionParam *data); (/user/pspintrman.h:170)
int InterruptManager::QueryIntrHandlerInfo( IMemory^ memory, int intr_code, int sub_intr_code, int data )
{
	KIntHandler* inth = _kernel->Interrupts[ intr_code ][ sub_intr_code ];
	assert( inth != NULL );
	if( inth == NULL )
		return -1;

	memory->WriteWord( data, 4, 0x38 );		// size
	memory->WriteWord( data + 4, 4, inth->Address );
	memory->WriteWord( data + 8, 4, inth->Argument ); // common
	memory->WriteWord( data + 0xC, 4, 0 ); // gp?
	memory->WriteWord( data + 0x10, 2, intr_code );
	memory->WriteWord( data + 0x12, 2, sub_intr_code );
	memory->WriteWord( data + 0x14, 2, 0 );
	memory->WriteWord( data + 0x16, 2, inth->IsEnabled() ? 1 : 0 );
	memory->WriteWord( data + 0x18, 4, inth->CallCount );
	memory->WriteWord( data + 0x1C, 4, 0 ); // field_1C?
	memory->WriteWord( data + 0x20, 4, 0 ); // total clock lo
	memory->WriteWord( data + 0x24, 4, 0 ); // total clock hi
	memory->WriteWord( data + 0x28, 4, 0 ); // min clock lo
	memory->WriteWord( data + 0x2C, 4, 0 ); // min clock hi
	memory->WriteWord( data + 0x30, 4, 0 ); // max clock lo
	memory->WriteWord( data + 0x34, 4, 0 ); // max clock hi

	Debug::WriteLine( "QueryIntrHandlerInfo: called, but a lot of it isn't implemented - make sure nothing is used!" );

	return 0;
}
