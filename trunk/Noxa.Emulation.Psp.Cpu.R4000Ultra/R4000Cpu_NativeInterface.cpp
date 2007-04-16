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

extern int _pendingIntNo;

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

void niSetPendingInterrupt( int intNumber )
{
	assert( _pendingIntNo == -1 );	// only support one at a time right now
	_pendingIntNo = intNumber;

	// Put stop request in
	_cpuCtx->StopFlag = CtxInterruptPending;
}

#pragma managed

void R4000Cpu::SetupNativeInterface()
{
	CpuApi* ni = _nativeInterface;
	memset( ni, 0, sizeof( CpuApi ) );

	ni->GetInterruptState = &niGetInterruptState;
	ni->SetInterruptState = &niSetInterruptState;
	ni->SetPendingInterrupt = &niSetPendingInterrupt;
}

void R4000Cpu::DestroyNativeInterface()
{
	memset( _nativeInterface, 0, sizeof( CpuApi ) );
}
