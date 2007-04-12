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

#define DEBUGBOUNCE

CRITICAL_SECTION _cs;
#define LOCK EnterCriticalSection( &_cs )
#define UNLOCK LeaveCriticalSection( &_cs )

#pragma unmanaged

typedef struct ThreadContext_t
{
	R4000Ctx		Ctx;
} ThreadContext;

typedef struct SwitchRequest_t
{
	ThreadContext*	Previous;		// Thread before the switch
	int				PreviousID;
	ThreadContext*	Target;			// Thread to switch to
	int				TargetID;

	bool			MakeCallback;
	uint			CallbackAddress;
	uint			CallbackArgs;
	MarshalCompleteFunction		BiosCallback;
} SwitchRequest;

typedef struct InterruptHandler_t
{
	int		SubNumber;
	int		CallbackAddress;
	int		CallbackArgs;
} InterruptHandler;

enum SwitchType
{
	SwitchNormal,	// Normal full switch
	SwitchMinimal,	// Don't restore important values (PC, etc)
};

Vector<ThreadContext*>	_threadContexts( 1024 );
int						_currentTcsId;
ThreadContext*			_currentTcs;
SwitchRequest			_switchRequest;
bool					_inHandler;				// true when in a callback/handler
int						_pendingIntNo;			// we need a list of these (sorted by priority), but for now 
												// we assume there cannot be more than one ^_^

LL<InterruptHandler*>	_interrupts[ 67 ];

R4000Ctx*				_cpuCtx;
R4000Cache*				_cache;
bouncefn				_bounceFn;

void PerformSwitch()
{
	assert( _switchRequest.Target != NULL );

	// Save old context
	if( _currentTcs != NULL )
	{
		memcpy( &_currentTcs->Ctx, _cpuCtx, sizeof( R4000Ctx ) );
		_currentTcs->Ctx.StopFlag = CtxContinue;
	}
	int oldIntMask = _cpuCtx->InterruptMask;

	// Switch to new context
	_currentTcsId = _switchRequest.TargetID;
	_currentTcs = _switchRequest.Target;

	// Restore new context
	memcpy( _cpuCtx, &_currentTcs->Ctx, sizeof( R4000Ctx ) );
	_cpuCtx->InterruptMask = oldIntMask;

	// Clear so that we can't switch badly
	_switchRequest.Target = NULL;
	_switchRequest.TargetID = -1;
}

void PerformSwitchBack( SwitchType type )
{
	assert( _switchRequest.Previous != NULL );

	// Save new context
	if( type == SwitchMinimal )
	{
		// We don't want to copy the PC (and others??) - this was an interrupt handler/callback
		R4000Ctx* oldCtx = &_currentTcs->Ctx;
		int oldPc = oldCtx->PC;
		memcpy( oldCtx, _cpuCtx, sizeof( R4000Ctx ) );
		oldCtx->PC = oldPc;
	}
	else
	{
		// Copy everything (normal case)
		memcpy( &_currentTcs->Ctx, _cpuCtx, sizeof( R4000Ctx ) );
	}
	_currentTcs->Ctx.StopFlag = CtxContinue;
	int oldIntMask = _cpuCtx->InterruptMask;

	// Switch back to old context
	_currentTcsId = _switchRequest.PreviousID;
	_currentTcs = _switchRequest.Previous;

	// Restore old context
	memcpy( _cpuCtx, &_currentTcs->Ctx, sizeof( R4000Ctx ) );
	_cpuCtx->InterruptMask = oldIntMask;

	// Clear for safety
	_switchRequest.Previous = NULL;
	_switchRequest.PreviousID = -1;
	_switchRequest.MakeCallback = false;
}

#pragma managed
CodeBlock* BuildBlock( int pc )
{
	return R4000Cpu::GlobalCpu->_builder->Build( pc );
}
#pragma unmanaged

#ifdef DEBUGBOUNCE
int __debugBounce( bouncefn f, int pointer )
{
	return f( pointer );
}
#endif

int niExecute( int* breakFlags )
{
	if( breakFlags != NULL )
		*breakFlags = 0;

	// If we came in with a switch flag, it's possible we are the first run
	if( _cpuCtx->StopFlag == CtxContextSwitch )
		PerformSwitch();
	_cpuCtx->StopFlag = CtxContinue;

	// Get/build block
	int pc = _cpuCtx->PC & 0x3FFFFFFF;
	CodeBlock* block = _cache->Find( pc );
	if( block == NULL )
	{
		block = BuildBlock( pc );
#ifdef STATISTICS
		//_stats->CodeCacheMisses++;
#endif
	}
	else
	{
#ifdef STATISTICS
		//_stats->CodeCacheHits++;
#endif
	}
	assert( block != NULL );

#ifdef STATISTICS
	//_stats->ExecutionLoops++;

	block->ExecutionCount++;
#endif

	//Debug::WriteLine( String::Format( "Executing block 0x{0:X8} (codegen at 0x{1:X8})", pc, (uint)block->Pointer ) );

	// Bounce in to it
#ifdef DEBUGBOUNCE
	int x = __debugBounce( _bounceFn, ( int )block->Pointer );
#else
	int x = _bounceFn( ( int )block->Pointer );
#endif

	// See what we need to do now
	CtxStopFlags stopFlag = _cpuCtx->StopFlag;
	_cpuCtx->StopFlag = CtxContinue;
	if( stopFlag != CtxContinue )
	{
		//*breakFlags = stopFlag;
		switch( stopFlag )
		{
		case CtxBreakRequest:
			// This happens when the BIOS requests a break
			// Not sure when this happens, really ^_^
			break;
		case CtxContextSwitch:
			// Means that a context switch is pending and we should do it and continue
			PerformSwitch();
			break;
		case CtxMarshal:
			// We need to marshal a callback on to another thread and then handle
			// what happens when it ends
			assert( false ); // this is totally wrong!
			assert( _switchRequest.MakeCallback == true );
			assert( _inHandler == false );
			PerformSwitch();
			_cpuCtx->PC = _switchRequest.CallbackAddress;
			_inHandler = true;
			// args?
			niExecute( NULL );
			_inHandler = false;
			PerformSwitchBack( SwitchMinimal );
			break;
		case CtxInterruptPending:
			// An interrupt is pending and we need to redirect to the handler
			// ???
			assert( false );
			assert( _pendingIntNo != -1 );
			assert( _inHandler == false );
			_inHandler = true;
			LLEntry<InterruptHandler*>* e = _interrupts[ _pendingIntNo ].GetHead();
			assert( e != NULL );
			while( e != NULL )
			{
				InterruptHandler* handler = e->Value;
				e = e->Next;

				// Fire it?
			}
			_inHandler = false;
			_pendingIntNo = -1;
			break;
		}
	}

	return 0;
}

void niBreakExecute( int flags )
{
	assert( flags == 1 ); // Only thing supported right now
	_cpuCtx->StopFlag = ( CtxStopFlags )flags;
}

int niAllocateContextStorage( int pc, int registers[ 32 ] )
{
	ThreadContext* context = ( ThreadContext* )calloc( 1, sizeof( ThreadContext ) );

	int index = _threadContexts.Add( context );
	context->Ctx.PC = pc;
	memcpy( context->Ctx.Registers, registers, sizeof( int ) * 32 );

	// Preserve GP if possible
	context->Ctx.Registers[ 28 ] = _cpuCtx->Registers[ 28 ];

	return index;
}

void niReleaseContextStorage( int tcsId )
{
	_threadContexts.Remove( tcsId );
}

void niSwitchContext( int newTcsId )
{
	// If contexts are the same, ignore
	if( _currentTcsId == newTcsId )
		return;

	// Create switch request
	_switchRequest.Previous = _currentTcs;
	_switchRequest.PreviousID = _currentTcsId;
	_switchRequest.Target = _threadContexts.GetValue( newTcsId );
	_switchRequest.TargetID = newTcsId;
	_switchRequest.MakeCallback = false;

	// Put the request in
	_cpuCtx->StopFlag = CtxContextSwitch;
}

void niMarshalCallback( int tcsId, int callbackAddress, int callbackArgs, MarshalCompleteFunction biosCallback )
{
	// Cannot marshal on to the current thread
	assert( _currentTcsId != tcsId );
	assert( _inHandler == false );

	// Marshal context setup
	_switchRequest.Previous = _currentTcs;
	_switchRequest.PreviousID = _currentTcsId;
	_switchRequest.Target = _threadContexts.GetValue( tcsId );
	_switchRequest.TargetID = tcsId;
	_switchRequest.MakeCallback = true;
	_switchRequest.CallbackAddress = callbackAddress;
	_switchRequest.CallbackArgs = callbackArgs;
	_switchRequest.BiosCallback = biosCallback;

	// Put the request in
	_currentTcs->Ctx.StopFlag = CtxMarshal;
}

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

	ni->Execute = &niExecute;
	ni->BreakExecute = &niBreakExecute;
	ni->AllocateContextStorage = &niAllocateContextStorage;
	ni->ReleaseContextStorage = &niReleaseContextStorage;
	ni->SwitchContext = &niSwitchContext;
	ni->MarshalCallback = &niMarshalCallback;
	ni->GetInterruptState = &niGetInterruptState;
	ni->SetInterruptState = &niSetInterruptState;
	ni->RegisterInterruptHandler = &niRegisterInterruptHandler;
	ni->UnregisterInterruptHandler = &niUnregisterInterruptHandler;
	ni->SetPendingInterrupt = &niSetPendingInterrupt;

	InitializeCriticalSection( &_cs );

	_cpuCtx = ( R4000Ctx* )_ctx;
	_cache = this->_codeCache;
	_bounceFn = ( bouncefn )this->_bounce;

	// Allocate the first thread context
	_currentTcsId = -1;
	_currentTcs = NULL;
	/*int registers[ 32 ];
	memset( registers, 0, sizeof( int ) * 32 );
	_currentTcsId = niAllocateContextStorage( 0, registers );
	_currentTcs = _threadContexts.GetValue( _currentTcsId );
	memcpy( &_currentTcs->Ctx, _cpuCtx, sizeof( R4000Ctx ) );*/

	memset( &_switchRequest, 0, sizeof( SwitchRequest ) );
}

void R4000Cpu::DestroyNativeInterface()
{
	_cpuCtx = NULL;

	DeleteCriticalSection( &_cs );
}
