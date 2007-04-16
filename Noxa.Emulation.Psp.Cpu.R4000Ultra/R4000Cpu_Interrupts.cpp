// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "StdAfx.h"
#include "R4000Cpu.h"
#include "R4000Core.h"
#include "R4000Memory.h"
#include "Tracer.h"
#pragma unmanaged
#include "LL.h"
#pragma managed

#include "R4000Ctx.h"
#include "R4000Generator.h"

using namespace System::Diagnostics;
using namespace System::Reflection;
using namespace System::Reflection::Emit;
using namespace System::Text;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Cpu;

#pragma unmanaged

typedef struct InterruptHandler_t
{
	int		SubNumber;
	int		CallbackAddress;
	int		CallbackArgs;
} InterruptHandler;

LL<InterruptHandler*>	_interrupts[ 67 ];
bool					_inIntHandler;			// true when in a handler
int						_pendingIntNo;			// we need a list of these (sorted by priority), but for now 
												// we assume there cannot be more than one ^_^

extern R4000Ctx*		_cpuCtx;

void PerformInterrupt()
{
	// ???
	assert( false );
	assert( _pendingIntNo != -1 );
	assert( _inIntHandler == false );
	_inIntHandler = true;
	LLEntry<InterruptHandler*>* e = _interrupts[ _pendingIntNo ].GetHead();
	assert( e != NULL );
	while( e != NULL )
	{
		InterruptHandler* handler = e->Value;
		e = e->Next;

		// Fire it? Could use the marshalling feature
	}
	_inIntHandler = false;
	_pendingIntNo = -1;
}

//void niRegisterInterruptHandler( int intNumber, int subNumber, int callbackAddress, int callbackArgs )
//{
//	InterruptHandler* handler = ( InterruptHandler* )malloc( sizeof( InterruptHandler ) );
//	handler->SubNumber = subNumber;
//	handler->CallbackAddress = callbackAddress;
//	handler->CallbackArgs = callbackArgs;
//
//	_interrupts[ intNumber ].Enqueue( handler );
//}
//
//void niUnregisterInterruptHandler( int intNumber, int subNumber )
//{
//	LLEntry<InterruptHandler*>* e = _interrupts[ intNumber ].GetHead();
//	while( e != NULL )
//	{
//		if( e->Value->SubNumber == subNumber )
//		{
//			_interrupts[ intNumber ].Remove( e );
//			break;
//		}
//		e = e->Next;
//	}
//}

//int niGetInterruptState()
//{
//	return _cpuCtx->InterruptMask;
//}
//
//int niSetInterruptState( int newState )
//{
//	int old = _cpuCtx->InterruptMask;
//	_cpuCtx->InterruptMask = newState;
//
//	// Handle any pending interrupts?
//	if( newState != 0 )
//	{
//		if( _pendingIntNo >= 0 )
//			_cpuCtx->StopFlag = CtxInterruptPending;
//	}
//
//	return old;
//}

#pragma managed

uint R4000Cpu::InterruptsMask::get()
{
	return 0;
}

void R4000Cpu::InterruptsMask::set( uint value )
{
}

void R4000Cpu::RegisterInterruptHandler( int interruptNumber, int slot, uint address, uint argument )
{
}

void R4000Cpu::UnregisterInterruptHandler( int interruptNumber, int slot )
{
}

void R4000Cpu::SetPendingInterrupt( int interruptNumber )
{
}
