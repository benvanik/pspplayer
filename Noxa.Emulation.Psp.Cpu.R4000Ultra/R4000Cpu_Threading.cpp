// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "StdAfx.h"
#include "R4000Cpu.h"
#include "R4000Core.h"
#include "R4000BlockBuilder.h"
#include "R4000Memory.h"
#include "Tracer.h"
#include "gcref.h"
#pragma unmanaged
#include "LL.h"
#pragma managed

#include "R4000Ctx.h"
#include "R4000Generator.h"

// When true, __debugBounce will be used to marshal the bounce, allowing for
// easy breakpoint setting
//#define DEBUGBOUNCE

using namespace System::Collections::Generic;
using namespace System::Diagnostics;
using namespace System::Reflection;
using namespace System::Reflection::Emit;
using namespace System::Text;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Cpu;

#ifdef STATISTICS
extern uint _instructionsExecuted;
extern uint _executionLoops;
extern uint _codeCacheHits;
extern uint _codeCacheMisses;
#endif

typedef struct ThreadContext_t
{
	R4000Ctx		Ctx;
	LL<R4000Ctx*>	CtxStack;		// Used by call system

	gcref<ContextSafetyDelegate^>	SafetyCallback;
	int				SafetyState;
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

	bool			ResultCallbackValid;
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

// From interrupts file
void PerformInterrupt();

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
	ThreadContext* context = new ThreadContext(); // ( ThreadContext* )calloc( 1, sizeof( ThreadContext ) );
	memset( &context->Ctx, 0, sizeof( R4000Ctx ) );
	context->SafetyState = 0;

	LOCK;
	int index = _threadContexts->Count;
	_threadContexts->Add( IntPtr( context ) );
	UNLOCK;

	context->Ctx.PC = pc;
	for( int n = 0; n < 32; n++ )
		context->Ctx.Registers[ n ] = registers[ n ];

	// Preserve GP if possible
	if( _cpuCtx->Registers[ 28 ] != 0 )
		context->Ctx.Registers[ 28 ] = _cpuCtx->Registers[ 28 ];

	// Set safety callback
	context->Ctx.Registers[ 31 ] = BIOS_SAFETY_DUMMY;

	return index;
}

void R4000Cpu::ReleaseContextStorage( int tcsId )
{
	Debug::Assert( tcsId >= 0 );
	Debug::Assert( tcsId < _threadContexts->Count );
	Debug::Assert( _currentTcsId != tcsId );
	LOCK;
	ThreadContext* context = ( ThreadContext* )_threadContexts[ tcsId ].ToPointer();
	_threadContexts->RemoveAt( tcsId );
	UNLOCK;

	SAFEDELETE( context );
}

void R4000Cpu::SetContextSafetyCallback( int tcsId, ContextSafetyDelegate^ callback, int state )
{
	LOCK;
	ThreadContext* context = ( ThreadContext* )_threadContexts[ tcsId ].ToPointer();
	UNLOCK;

	context->SafetyCallback = callback;
	context->SafetyState = state;
}

uint R4000Cpu::GetContextRegister( int tcsId, int reg )
{
	LOCK;
	ThreadContext* context = ( ThreadContext* )_threadContexts[ tcsId ].ToPointer();
	UNLOCK;

	return context->Ctx.Registers[ reg ];
}

void R4000Cpu::SetContextRegister( int tcsId, int reg, uint value )
{
	LOCK;
	ThreadContext* context = ( ThreadContext* )_threadContexts[ tcsId ].ToPointer();
	UNLOCK;

	context->Ctx.Registers[ reg ] = value;
}

void R4000Cpu::SwitchContext( int newTcsId )
{
	// If contexts are the same, ignore
	if( _currentTcsId == newTcsId )
		return;

	LOCK;
	ThreadContext* targetContext = ( ThreadContext* )_threadContexts[ newTcsId ].ToPointer();
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
	ThreadContext* targetContext = ( ThreadContext* )_threadContexts[ tcsId ].ToPointer();
	UNLOCK;

	// Don't support more than one at a time right now
	Debug::Assert( _switchRequest.MakeCall == false );

	// Marshal context setup
	_switchRequest.Previous = _currentTcs;
	_switchRequest.PreviousID = _currentTcsId;
	_switchRequest.Target = targetContext;
	_switchRequest.TargetID = tcsId;
	
	_switchRequest.MakeCall = true;
	_switchRequest.CallAddress = address;
	_switchRequest.CallArgumentCount = arguments->Length;
	for( int n = 0; n < arguments->Length; n++ )
		_switchRequest.CallArguments[ n ] = arguments[ n ];

	_switchRequest.ResultCallbackValid = ( resultCallback != nullptr );
	_switchRequest.ResultCallback = resultCallback;
	_switchRequest.CallbackState = state;

	// Put the request in
	_currentTcs->Ctx.StopFlag = CtxMarshal;
}

CodeBlock* BuildBlock( int pc )
{
	return R4000Cpu::GlobalCpu->_builder->Build( pc );
}

void MakeSafetyCallback( int tcsId, ThreadContext* tcs )
{
	ContextSafetyDelegate^ del = tcs->SafetyCallback;
	if( del != nullptr )
	{
		del( tcsId, tcs->SafetyState );
	}
	else
	{
		// No callback defined - we will probably die if we continue
		assert( false );
	}
}

bool MakeResultCallback( int v0 )
{
	MarshalCompleteDelegate^ del = _switchRequest.ResultCallback;
	return del( _currentTcsId, _switchRequest.CallbackState, v0 );
}

#pragma unmanaged

void PushState()
{
	R4000Ctx* ctxCopy = ( R4000Ctx* )malloc( sizeof( R4000Ctx ) );
	memcpy( ctxCopy, _cpuCtx, sizeof( R4000Ctx ) );
	_currentTcs->CtxStack.Enqueue( ctxCopy );
}

void PopState()
{
	R4000Ctx* ctxCopy = _currentTcs->CtxStack.Pop();
	memcpy( _cpuCtx, ctxCopy, sizeof( R4000Ctx ) );
	SAFEDELETE( ctxCopy );

	_cpuCtx->StopFlag = CtxContinue;
}

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
	_switchRequest.MakeCall = false;
}

#ifdef DEBUGBOUNCE
// UNMANAGED
uint __debugBounce( int codePointer )
{
	return _bounceFn( codePointer );
}
#endif

#pragma managed
void __debugRunPrint( int pc, int codePointer )
{
	Debug::WriteLine( String::Format( "Executing block 0x{0:X8} (codegen at 0x{1:X8})", pc, codePointer ) );
}
#pragma unmanaged

uint NativeExecute( bool* breakFlag )
{
	// If we came in with a switch flag, it's possible we are the first run
	if( _cpuCtx->StopFlag == CtxContextSwitch )
		PerformSwitch();
	_cpuCtx->StopFlag = CtxContinue;

	// Check for callback end
	if( _cpuCtx->PC == CALL_RETURN_DUMMY )
	{
		int v0 = _cpuCtx->Registers[ 2 ];
		//int v1 = _cpuCtx->Registers[ 3 ];
		PopState();

		// Switch state back
		PerformSwitchBack( SwitchNormal );
		
		// Make callback
		_switchRequest.MakeCall = false;
		if( _switchRequest.ResultCallbackValid == true )
		{
			bool exitEarly = MakeResultCallback( v0 );
			if( exitEarly == false )
			{
				*breakFlag = false;
				return 0;
			}
		}
	}
	else if( _cpuCtx->PC == BIOS_SAFETY_DUMMY )
	{
		// BIOS wants to know about the code hitting this location
		MakeSafetyCallback( _currentTcsId, _currentTcs );
		return 0;
	}

	uint instructionCount = 0;

executeStart:		// Arrived at from call/interrupt handling below

#ifdef STATISTICS
	uint startInstructionCount = _instructionsExecuted;
#endif

	// Get/build block
	int pc = _cpuCtx->PC & 0x3FFFFFFF;
	void* codePointer = QuickPointerLookup( pc );
	if( codePointer == NULL )
	{
		CodeBlock* block = BuildBlock( pc );
		assert( block != NULL );
		codePointer = block->Pointer;

#ifdef STATISTICS
		_codeCacheMisses++;
	}
	else
		_codeCacheHits++;
#else
	}
#endif

#ifdef STATISTICS
	CodeBlock* block = _cache->Find( pc );
	assert( block != NULL );
	block->ExecutionCount++;

	_executionLoops++;
#endif

	//__debugRunPrint( pc, ( int )codePointer );

	// Bounce in to it
#ifdef DEBUGBOUNCE
	__debugBounce( ( int )codePointer );
#else
	_bounceFn( ( int )codePointer );
#endif

#ifdef STATISTICS
	instructionCount += _instructionsExecuted - startInstructionCount;
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
			*breakFlag = true;
			break;
		case CtxContextSwitch:
			// Means that a context switch is pending and we should do it and continue
			PerformSwitch();
			break;
		case CtxMarshal:
			// We need to marshal a callback on to another thread and then handle
			// what happens when it ends
			assert( false ); // this is totally wrong!
			assert( _switchRequest.MakeCall == true );
			PerformSwitch();
			PushState();
			_cpuCtx->PC = _switchRequest.CallAddress;
			for( int n = 0; n < _switchRequest.CallArgumentCount; n++ )
				_cpuCtx->Registers[ n + 4 ] = _switchRequest.CallArguments[ n ];
			_cpuCtx->Registers[ 31 ] = CALL_RETURN_DUMMY;
			goto executeStart;
		case CtxInterruptPending:
			// An interrupt is pending and we need to redirect to the handler
			PerformInterrupt();
			break;
		}
	}

	return instructionCount;
}

#pragma managed

void R4000Cpu::Execute(
					   [System::Runtime::InteropServices::Out] bool% breakFlag,
					   [System::Runtime::InteropServices::Out] uint% instructionsExecuted )
{
	bool bf;
	instructionsExecuted = NativeExecute( &bf );
	breakFlag = bf;
}

void R4000Cpu::BreakExecution()
{
	_cpuCtx->StopFlag = CtxBreakRequest;
}

void R4000Cpu::Stop()
{
	this->BreakExecution();
}
