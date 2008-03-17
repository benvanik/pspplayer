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

extern R4000Ctx* _cpuCtx;

extern uint _interruptMask;
extern bool _inIntHandler;
extern uint _pendingInterrupts;

int niGetInterruptState()
{
	return _interruptMask;
}

int niSetInterruptState( int newState )
{
	int old = _interruptMask;
	_interruptMask = newState;

	if( ( newState & _pendingInterrupts ) != 0 )
	{
		// Handle pending interrupts
		_cpuCtx->StopFlag = CtxInterruptPending;
	}

	return old;
}

void niSetPendingInterrupt( int intNumber )
{
	_pendingInterrupts |= ( 1 << intNumber );
	if( _inIntHandler == false )
		_cpuCtx->StopFlag = CtxInterruptPending;
}

void niResume()
{
}

void niBreakAndWait()
{
}

void niBreakAndWaitTimeout( int timeoutMs )
{
}

void niBreakAndWaitTimeoutCallback( int timeoutMs, CpuResumeFunction callback, void* state )
{
}

#pragma managed

void R4000Cpu::SetupNativeInterface()
{
	CpuApi* ni = _nativeInterface;
	memset( ni, 0, sizeof( CpuApi ) );

	ni->GetInterruptState = &niGetInterruptState;
	ni->SetInterruptState = &niSetInterruptState;
	ni->SetPendingInterrupt = &niSetPendingInterrupt;

	/*ni->Resume = &niResume;
	ni->BreakAndWait = &niBreakAndWait;
	ni->BreakAndWaitTimeout = &niBreakAndWaitTimeout;
	ni->BreakAndWaitTimeoutCallback = &niBreakAndWaitTimeoutCallback;*/
}

void R4000Cpu::DestroyNativeInterface()
{
	memset( _nativeInterface, 0, sizeof( CpuApi ) );
}
