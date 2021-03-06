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

extern void BreakHandler( uint pc );

typedef struct ThreadContext_t
{
	R4000Ctx		Ctx;
	LL<R4000Ctx*>	CtxStack;		// Used by call system

	gcref<ContextSafetyDelegate^>	SafetyCallback;
	int				SafetyState;

	bool			MarshalSwitchback;
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
	gcref<MarshalCompleteDelegate^>*	ResultCallback;
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

HANDLE					_waitHandle;

R4000Ctx*				_cpuCtx;
R4000Cache*				_cache;
bouncefn				_bounceFn;

int						_currentTcsId;
ThreadContext*			_currentTcs;

SwitchRequest			_switchRequest;
LL<SwitchRequest*>		_marshalRequests;

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
	_switchRequest.ResultCallback = new gcref<MarshalCompleteDelegate^>();

	InitializeCriticalSection( &_cs );

	_threadContexts = gcnew List<IntPtr>( 128 );

	_waitHandle = CreateEvent( NULL, FALSE, FALSE, NULL );
	_timerQueue = gcnew TimerQueue();
}

void R4000Cpu::DestroyThreading()
{
	_cpuCtx = NULL;
	_threadContexts = nullptr;

	DeleteCriticalSection( &_cs );

	SAFEDELETE( _switchRequest.ResultCallback );

	delete _timerQueue;
	_timerQueue = nullptr;
	CloseHandle( _waitHandle );
	_waitHandle = NULL;
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

	// FPU regs
	uint nan = 0x7F800001;
	for( int n = 0; n < 32; n++ )
		context->Ctx.Cp1Registers[ n * 4 ] = ( float& )nan;

	// Set safety callback
	context->Ctx.Registers[ 31 ] = BIOS_SAFETY_DUMMY;

	return index;
}

void R4000Cpu::ReleaseContextStorage( int tcsId )
{
	if( _threadContexts == nullptr )
		return;
	Debug::Assert( tcsId >= 0 );
	Debug::Assert( tcsId < _threadContexts->Count );
	//Debug::Assert( _currentTcsId != tcsId );
	if( _currentTcsId == tcsId )
	{
		Log::WriteLine( Verbosity::Critical, Feature::Cpu, "ReleaseContextStorage: trying to release the active context - leaking context!" );
		return;
	}
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

	if( reg == -1 )
		return context->Ctx.PC;
	else
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
	_cpuCtx->StopFlag |= CtxContextSwitch;
}

void R4000Cpu::MarshalCall( int tcsId, uint address, array<uint>^ arguments, MarshalCompleteDelegate^ resultCallback, int state )
{
	if( tcsId < -1 )
	{
		// Happens
		Log::WriteLine( Verbosity::Critical, Feature::Cpu, "MarshalCall: attempted to marshal call on to thread with tcsId={0}", tcsId );
		return;
	}

	// Current thread
	if( tcsId == -1 )
		tcsId = _currentTcsId;

	LOCK;
	ThreadContext* targetContext = ( ThreadContext* )_threadContexts[ tcsId ].ToPointer();
	UNLOCK;

	SwitchRequest* marshalRequest = new SwitchRequest();
	
	// Marshal context setup
	marshalRequest->Previous = _currentTcs;
	marshalRequest->PreviousID = _currentTcsId;
	marshalRequest->Target = targetContext;
	marshalRequest->TargetID = tcsId;
	
	marshalRequest->MakeCall = true;
	marshalRequest->CallAddress = address;
	marshalRequest->CallArgumentCount = arguments->Length;
	for( int n = 0; n < arguments->Length; n++ )
		marshalRequest->CallArguments[ n ] = arguments[ n ];

	marshalRequest->ResultCallbackValid = ( resultCallback != nullptr );
	marshalRequest->ResultCallback = new gcref<MarshalCompleteDelegate^>();
	( *marshalRequest->ResultCallback ) = resultCallback;
	marshalRequest->CallbackState = state;

	_marshalRequests.Enqueue( marshalRequest );

	// Put the request in
	_cpuCtx->StopFlag |= CtxMarshal;
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

bool MakeResultCallback( SwitchRequest* marshalRequest, int v0 )
{
	MarshalCompleteDelegate^ del = ( *marshalRequest->ResultCallback );
	return del( _currentTcsId, marshalRequest->CallbackState, v0 );
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

	//_cpuCtx->StopFlag = CtxContinue;
}

void PerformSwitch( SwitchRequest* request )
{
	//assert( request->Target != NULL );
	if( request->Target == NULL )
	{
		// Bad?
		return;
	}

	// Save old context
	if( _currentTcs != NULL )
	{
		memcpy( &_currentTcs->Ctx, _cpuCtx, sizeof( R4000Ctx ) );
		//_currentTcs->Ctx.StopFlag = CtxContinue;
	}
	int oldIntMask = _cpuCtx->InterruptMask;

	// Switch to new context
	_currentTcsId = request->TargetID;
	_currentTcs = request->Target;

	// Restore new context
	memcpy( _cpuCtx, &_currentTcs->Ctx, sizeof( R4000Ctx ) );
	_cpuCtx->InterruptMask = oldIntMask;

	// Clear so that we can't switch badly
	request->Target = NULL;
	request->TargetID = -1;
}

void PerformSwitch()
{
	PerformSwitch( &_switchRequest );
}

void PerformSwitchBack( SwitchRequest* request, SwitchType type )
{
	assert( request->Previous != NULL );

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
	_currentTcs->Ctx.StopFlag |= CtxContinue;

	int oldIntMask = _cpuCtx->InterruptMask;

	// Switch back to old context
	_currentTcsId = request->PreviousID;
	_currentTcs = request->Previous;

	// Restore old context
	memcpy( _cpuCtx, &_currentTcs->Ctx, sizeof( R4000Ctx ) );
	_cpuCtx->InterruptMask = oldIntMask;

	// Clear for safety
	request->Previous = NULL;
	request->PreviousID = -1;
	request->MakeCall = false;
}

void PerformSwitchBack( SwitchType type )
{
	PerformSwitchBack( &_switchRequest, type );
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
	if( ( _cpuCtx->StopFlag & CtxContextSwitch ) == CtxContextSwitch )
	{
		PerformSwitch();
		_cpuCtx->StopFlag &= ~CtxContextSwitch;
	}

	// Check for callback end
	if( _cpuCtx->PC == CALL_RETURN_DUMMY )
	{
		SwitchRequest* marshalRequest = _marshalRequests.Dequeue();
		
		int v0 = _cpuCtx->Registers[ 2 ];
		//int v1 = _cpuCtx->Registers[ 3 ];
		PopState();

		// Switch state back
		if( _currentTcs->MarshalSwitchback == true )
			PerformSwitchBack( marshalRequest, SwitchNormal );
		
		// Make callback
		if( marshalRequest->ResultCallbackValid == true )
		{
			bool exitEarly = MakeResultCallback( marshalRequest, v0 );
			if( exitEarly == true )
			{
				*breakFlag = false;
				SAFEDELETE( marshalRequest->ResultCallback );
				delete marshalRequest;
				return 0;
			}
		}
		SAFEDELETE( marshalRequest->ResultCallback );
		delete marshalRequest;
	}
	else if( _cpuCtx->PC == BIOS_SAFETY_DUMMY )
	{
		// BIOS wants to know about the code hitting this location
		MakeSafetyCallback( _currentTcsId, _currentTcs );
		return 0;
	}
	else if( _cpuCtx->PC == INTERRUPT_RETURN_DUMMY )
	{
		PopState();
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
	while( _cpuCtx->StopFlag != CtxContinue )
	{
		if( ( _cpuCtx->StopFlag & CtxBreakRequest ) == CtxBreakRequest )
		{
			_cpuCtx->StopFlag &= ~CtxBreakRequest;
			// This happens when the BIOS requests a break
			// Not sure when this happens, really ^_^
			*breakFlag = true;
		}
		else if( ( _cpuCtx->StopFlag & CtxMarshal ) == CtxMarshal )
		{
			_cpuCtx->StopFlag &= ~CtxMarshal;
			// We need to marshal a callback on to another thread and then handle
			// what happens when it ends
			SwitchRequest* marshalRequest = _marshalRequests.PeekHead();
			assert( marshalRequest->MakeCall == true );
			_currentTcs->MarshalSwitchback = ( _currentTcsId != marshalRequest->TargetID );
			if( _currentTcs->MarshalSwitchback == true )
				PerformSwitch( marshalRequest );
			PushState();
			_cpuCtx->PC = marshalRequest->CallAddress;
			for( int n = 0; n < marshalRequest->CallArgumentCount; n++ )
				_cpuCtx->Registers[ n + 4 ] = marshalRequest->CallArguments[ n ];
			_cpuCtx->Registers[ 31 ] = CALL_RETURN_DUMMY;
			goto executeStart;
		}
		else if( ( _cpuCtx->StopFlag & CtxContextSwitch ) == CtxContextSwitch )
		{
			_cpuCtx->StopFlag &= ~CtxContextSwitch;
			// Means that a context switch is pending and we should do it and continue
			PerformSwitch();
		}
		else if( ( _cpuCtx->StopFlag & CtxInterruptPending ) == CtxInterruptPending )
		{
			_cpuCtx->StopFlag &= ~CtxInterruptPending;
			// An interrupt is pending and we need to redirect to the handler
			PerformInterrupt();
		}
		else if( ( _cpuCtx->StopFlag & CtxBreakAndWait ) == CtxBreakAndWait )
		{
			_cpuCtx->StopFlag &= ~CtxBreakAndWait;
			// BreakAndWait request
			BreakHandler( _cpuCtx->PC );
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
	// Don't overwrite a marshal and stuff
	if( _cpuCtx == NULL )
		return;
	_cpuCtx->StopFlag |= CtxBreakRequest;
}

void R4000Cpu::Resume()
{
	this->ResumeInternal( false );
}

void R4000Cpu::BreakTimeout( Noxa::Timer^ timer )
{
	this->ResumeInternal( true );
}

void R4000Cpu::ResumeInternal( bool timedOut )
{
	// MAKE SURE TO UPDATE niResume!!

	// May have been started already
	if( _broken == false )
		return;
	_broken = false;

	if( _resumeCallback != nullptr )
		_resumeCallback( timedOut, _resumeState );
	
	PulseEvent( _waitHandle );
}

void R4000Cpu::BreakAndWait()
{
	Debug::Assert( _broken == false );
	_broken = true;

	_resumeCallback = nullptr;

	_cpuCtx->StopFlag |= CtxBreakAndWait;
}

void R4000Cpu::BreakAndWait( int timeoutMs )
{
	this->BreakAndWait( timeoutMs, nullptr, nullptr );
}

void R4000Cpu::BreakAndWait( int timeoutMs, CpuResumeCallback^ callback, Object^ state )
{
	Debug::Assert( _broken == false );
	_broken = true;

	_resumeCallback = callback;
	_resumeState = state;

	// Start timer if sane
	if( ( timeoutMs > 0 ) &&
		( timeoutMs < INFINITE ) )
	{
		TimerCallback^ cb = gcnew TimerCallback( this, &R4000Cpu::BreakTimeout );
		_timerQueue->CreateOneShotTimer( cb, timeoutMs, TimerExecutionContext::TimerThread, false );
	}

	_cpuCtx->StopFlag |= CtxBreakAndWait;
}

void R4000Cpu::Stop()
{
	this->BreakExecution();
}
