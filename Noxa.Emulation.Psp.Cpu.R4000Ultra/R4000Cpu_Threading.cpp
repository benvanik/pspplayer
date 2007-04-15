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
#include "gcref.h"

#include "R4000Ctx.h"
#include "R4000Generator.h"

// When true, __debugBounce will be used to marshal the bounce, allowing for
// easy breakpoint setting
#define DEBUGBOUNCE

using namespace System::Collections::Generic;
using namespace System::Diagnostics;
using namespace System::Reflection;
using namespace System::Reflection::Emit;
using namespace System::Text;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Cpu;

typedef struct ThreadContext_t
{
	R4000Ctx		Ctx;
} ThreadContext;

typedef struct SwitchRequest_t
{
public:
	ThreadContext*	Previous;		// Thread before the switch
	int				PreviousID;
	ThreadContext*	Target;			// Thread to switch to
	int				TargetID;

	bool			MakeCall;
	uint			CallAddress;
	uint			CallArguments[ 12 ];
	int				CallArgumentCount;

	gcref<MarshalCompleteDelegate^>	ResultCallback;
	int				CallbackState;
} SwitchRequest;

enum SwitchType
{
	SwitchNormal,	// Normal full switch
	SwitchMinimal,	// Don't restore important values (PC, etc)
};

#pragma unmanaged

CRITICAL_SECTION _cs;
#define LOCK EnterCriticalSection( &_cs )
#define UNLOCK LeaveCriticalSection( &_cs )

R4000Ctx*				_cpuCtx;
R4000Cache*				_cache;
bouncefn				_bounceFn;

int						_currentTcsId;
ThreadContext*			_currentTcs;

SwitchRequest			_switchRequest;

#pragma managed

void R4000Cpu::SetupThreading()
{
	_cpuCtx = ( R4000Ctx* )_ctx;
	_cache = this->_codeCache;
	_bounceFn = ( bouncefn )this->_bounce;

	_currentTcsId = -1;
	_currentTcs = NULL;

	memset( &_switchRequest, 0, sizeof( SwitchRequest ) );

	InitializeCriticalSection( &_cs );

	_threadContexts = gcnew List<IntPtr>( 128 );
}

void R4000Cpu::DestroyThreading()
{
	_cpuCtx = NULL;
	_threadContexts = nullptr;

	DeleteCriticalSection( &_cs );
}

int R4000Cpu::AllocateContextStorage( uint pc, array<uint>^ registers )
{
	ThreadContext* context = ( ThreadContext* )calloc( 1, sizeof( ThreadContext ) );

	LOCK;
	int index = _threadContexts->Count;
	_threadContexts->Add( IntPtr( context ) );
	UNLOCK;

	context->Ctx.PC = pc;
	memcpy( context->Ctx.Registers, registers, sizeof( int ) * 32 );

	// Preserve GP if possible
	context->Ctx.Registers[ 28 ] = _cpuCtx->Registers[ 28 ];

	return index;
}

void R4000Cpu::ReleaseContextStorage( int tcsId )
{
	Debug::Assert( tcsId >= 0 );
	Debug::Assert( tcsId < _threadContexts->Count );
	LOCK;
	ThreadContext* context = ( ThreadContext* )_threadContexts[ tcsId ];
	_threadContexts->RemoveAt( tcsId );
	UNLOCK;

	SAFEDELETE( context );
}

uint R4000Cpu::GetContextRegister( int tcsId, int reg )
{
	LOCK;
	ThreadContext* context = ( ThreadContext* )_threadContexts[ tcsId ];
	UNLOCK;

	return context->Ctx->Registers[ reg ];
}

void R4000Cpu::SetContextRegister( int tcsId, int reg, uint value )
{
	LOCK;
	ThreadContext* context = ( ThreadContext* )_threadContexts[ tcsId ];
	UNLOCK;

	context->Ctx->Registers[ reg ] = value;
}

void R4000Cpu::SwitchContext( int newTcsId )
{
	// If contexts are the same, ignore
	if( _currentTcsId == newTcsId )
		return;

	LOCK;
	ThreadContext* targetContext = ( ThreadContext* )_threadContexts[ tcsId ];
	UNLOCK;

	// Create switch request
	_switchRequest.Previous = _currentTcs;
	_switchRequest.PreviousID = _currentTcsId;
	_switchRequest.Target = targetContext;
	_switchRequest.TargetID = newTcsId;
	_switchRequest.MakeCall = false;

	// Put the request in
	_cpuCtx->StopFlag = CtxContextSwitch;
}

void R4000Cpu::MarshalCall( int tcsId, uint address, array<uint>^ arguments, MarshalCompleteDelegate^ resultCallback, int state )
{
	LOCK;
	ThreadContext* targetContext = ( ThreadContext* )_threadContexts[ tcsId ];
	UNLOCK;

	// Marshal context setup
	_switchRequest.Previous = _currentTcs;
	_switchRequest.PreviousID = _currentTcsId;
	_switchRequest.Target = targetContext;
	_switchRequest.TargetID = tcsId;
	
	_switchRequest.MakeCall = true;
	_switchRequest.CallbackAddress = address;
	_switchRequest.CallArgumentCount = arguments->Length;
	for( int n = 0; n < arguments->Length; n++ )
		_switchRequest.CallArguments[ n ] = arguments[ n ];

	_switchRequest.ResultCallback = resultCallback;
	_switchRequest.CallbackState = state;

	// Put the request in
	_currentTcs->Ctx.StopFlag = CtxMarshal;
}

#pragma unmanaged

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

#ifdef DEBUGBOUNCE
// UNMANAGED
int __debugBounce( bouncefn f, int pointer )
{
	return f( pointer );
}
#endif

#pragma managed

void R4000Cpu::Execute(
					   [System::Runtime::InteropServices::Out] bool% breakFlag,
					   [System::Runtime::InteropServices::Out] uint% instructionsExecuted )
{
	breakFlag = false;
	instructionsExecuted = 0;

	// If we came in with a switch flag, it's possible we are the first run
	if( _cpuCtx->StopFlag == CtxContextSwitch )
		PerformSwitchM();
	_cpuCtx->StopFlag = CtxContinue;

	// Get/build block
	int pc = _cpuCtx->PC & 0x3FFFFFFF;
	CodeBlock* block = _cache->Find( pc );
	if( block == NULL )
	{
		block = BuildBlock( pc );
#ifdef STATISTICS
		_stats->CodeCacheMisses++;
#endif
	}
	else
	{
#ifdef STATISTICS
		_stats->CodeCacheHits++;
#endif
	}
	assert( block != NULL );

#ifdef STATISTICS
	_stats->ExecutionLoops++;

	block->ExecutionCount++;
#endif

	Debug::WriteLine( String::Format( "Executing block 0x{0:X8} (codegen at 0x{1:X8})", pc, ( uint )block->Pointer ) );

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
		switch( stopFlag )
		{
		case CtxBreakRequest:
			// This happens when the BIOS requests a break
			// Not sure when this happens, really ^_^
			breakFlag = true;
			break;
		case CtxContextSwitch:
			// Means that a context switch is pending and we should do it and continue
			PerformSwitch();
			break;
		case CtxMarshal:
			// We need to marshal a callback on to another thread and then handle
			// what happens when it ends
			Debug::Assert( false ); // this is totally wrong!
			Debug::Assert( _switchRequest.MakeCall == true );
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

void R4000Cpu::BreakExecution()
{
	assert( flags == 1 ); // Only thing supported right now
	_cpuCtx->StopFlag = ( CtxStopFlags )flags;
}

void R4000Cpu::Stop()
{
	this->BreakExecution();
}
