// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "StdAfx.h"
#define WIN32_LEAN_AND_MEAN
#include <Windows.h>
#include "R4000Cpu.h"
#include "R4000Core.h"
#include "R4000Cache.h"
#include "R4000Memory.h"
#include "R4000Cache.h"
#include "R4000Ctx.h"
#include "R4000BlockBuilder.h"
#pragma unmanaged
#include "Vector.h"
#include "LL.h"
#pragma managed

using namespace System::Diagnostics;
using namespace System::Text;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Cpu;
using namespace Noxa::Emulation::Psp::Cpu::Native;

#pragma unmanaged

int niGetInterruptState()
{
	return _cpuCtx->InterruptMask;
}

int niSetInterruptState( int newState )
{
	int old = _cpuCtx->InterruptMask;
	_cpuCtx->InterruptMask = newState;

	// Handle any pending interrupts?
	if( newState != 0 )
	{
		if( _pendingIntNo >= 0 )
			_cpuCtx->StopFlag = CtxInterruptPending;
	}

	return old;
}

void niRegisterInterruptHandler( int intNumber, int subNumber, int callbackAddress, int callbackArgs )
{
	InterruptHandler* handler = ( InterruptHandler* )malloc( sizeof( InterruptHandler ) );
	handler->SubNumber = subNumber;
	handler->CallbackAddress = callbackAddress;
	handler->CallbackArgs = callbackArgs;

	_interrupts[ intNumber ].Enqueue( handler );
}

void niUnregisterInterruptHandler( int intNumber, int subNumber )
{
	LLEntry<InterruptHandler*>* e = _interrupts[ intNumber ].GetHead();
	while( e != NULL )
	{
		if( e->Value->SubNumber == subNumber )
		{
			_interrupts[ intNumber ].Remove( e );
			break;
		}
		e = e->Next;
	}
}

void niSetPendingInterrupt( int intNumber )
{
	// If no handlers, ignore
	if( _interrupts[ intNumber ].GetCount() == 0 )
		return;

	assert( _pendingIntNo == -1 );	// only support one at a time right now
	_pendingIntNo = intNumber;

	// Put stop request in
	_currentTcs->Ctx.StopFlag = CtxInterruptPending;
}

#pragma managed

void R4000Cpu::SetupNativeInterface()
{
	CpuApi* ni = ( CpuApi* )_nativeInterface;
	memset( ni, 0, sizeof( CpuApi ) );

	//ni->Execute = &niExecute;
	//ni->BreakExecute = &niBreakExecute;
	//ni->AllocateContextStorage = &niAllocateContextStorage;
	//ni->ReleaseContextStorage = &niReleaseContextStorage;
	//ni->SwitchContext = &niSwitchContext;
	//ni->MarshalCallback = &niMarshalCallback;
	ni->GetInterruptState = &niGetInterruptState;
	ni->SetInterruptState = &niSetInterruptState;
	ni->RegisterInterruptHandler = &niRegisterInterruptHandler;
	ni->UnregisterInterruptHandler = &niUnregisterInterruptHandler;
	ni->SetPendingInterrupt = &niSetPendingInterrupt;
}

void R4000Cpu::DestroyNativeInterface()
{
}
