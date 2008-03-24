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

#define INTERRUPT_COUNT 32

typedef struct InterruptHandler_t
{
	int		SubNumber;
	int		CallbackAddress;
	int		CallbackArgs;
} InterruptHandler;

uint					_interruptMask = 0xFFFFFFFF;	// bitmask of enabled interrupts
LL<InterruptHandler*>	_interrupts[ INTERRUPT_COUNT ];
bool					_inIntHandler = false;			// true when in a handler
uint					_pendingInterrupts = 0;			// bitmask of pending interrupts

extern R4000Ctx*		_cpuCtx;

extern void PushState();
extern uint NativeExecute( bool* breakFlag );

void PerformInterrupt()
{
	// Note that we are NOT reentrant - if we get back here somehow, just ignore the interrupt
	if( _inIntHandler == true )
		return;
	_inIntHandler = true;
	for( int n = 0; n < INTERRUPT_COUNT; n++ )
	{
		// Skip disabled interrupts
		if( ( _interruptMask & ( 1 << n ) ) == 0 )
			continue;
		// Skip non-pending interrupts
		if( ( _pendingInterrupts & ( 1 << n ) ) == 0 )
			continue;
		LLEntry<InterruptHandler*>* e = _interrupts[ n ].GetHead();
		if( e == NULL )
			continue;
		while( e != NULL )
		{
			InterruptHandler* handler = e->Value;
			e = e->Next;

			// Fire it
			PushState();
			_cpuCtx->PC = handler->CallbackAddress;
			_cpuCtx->Registers[ 4 ] = handler->SubNumber;
			_cpuCtx->Registers[ 5 ] = handler->CallbackArgs;
			_cpuCtx->Registers[ 31 ] = INTERRUPT_RETURN_DUMMY;
			bool dummy;
			NativeExecute( &dummy );
		}
		_pendingInterrupts &= ~( 1 << n );
	}
	_inIntHandler = false;
}

#pragma managed

uint R4000Cpu::InterruptsMask::get()
{
	return _interruptMask;
}

void R4000Cpu::InterruptsMask::set( uint value )
{
	_interruptMask = value;
	if( ( value & _pendingInterrupts ) != 0 )
	{
		// Handle pending interrupts
		_cpuCtx->StopFlag |= CtxInterruptPending;
	}
}

void R4000Cpu::RegisterInterruptHandler( int interruptNumber, int slot, uint address, uint argument )
{
	InterruptHandler* handler = ( InterruptHandler* )malloc( sizeof( InterruptHandler ) );
	handler->SubNumber = slot;
	handler->CallbackAddress = address;
	handler->CallbackArgs = argument;

	_interrupts[ interruptNumber ].Enqueue( handler );
}

void R4000Cpu::UnregisterInterruptHandler( int interruptNumber, int slot )
{
	LLEntry<InterruptHandler*>* e = _interrupts[ interruptNumber ].GetHead();
	while( e != NULL )
	{
		if( e->Value->SubNumber == slot )
		{
			_interrupts[ interruptNumber ].Remove( e );
			break;
		}
		e = e->Next;
	}
}

void R4000Cpu::SetPendingInterrupt( int interruptNumber )
{
	_pendingInterrupts |= ( 1 << interruptNumber );
	if( _inIntHandler == false )
		_cpuCtx->StopFlag |= CtxInterruptPending;
}
