// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "StdAfx.h"
#include <Windows.h>

#include "R4000BiosStubs.h"
#include "R4000Generator.h"

using namespace System;
using namespace Noxa::Emulation::Psp::Bios;
using namespace Noxa::Emulation::Psp::Cpu;

#pragma unmanaged

// sceRtc ----------------------------------------------
int sceRtcGetTickResolution();
void sceRtcGetCurrentTick( LARGE_INTEGER* address );

#pragma managed

bool R4000BiosStubs::EmitCall( R4000GenContext^ context, R4000Generator *g, int address, int nid )
{
	// Put the return value, if any, in EAX - if you don't return anything, put 0 just in case!

	switch( nid )
	{
	// sceRtc ----------------------------------------------
	case 0xc41c2853:		// sceRtcGetTickResolution
		g->call( ( int )sceRtcGetTickResolution );
		return true;
	case 0x3f7ad767:		// sceRtcGetCurrentTick
		g->push( MREG( CTX, 4 ) );
		g->call( ( int )sceRtcGetCurrentTick );
		g->add( ESP, 4 );
		g->mov( EAX, 0 );
		return true;
	}

	return false;
}

#pragma unmanaged

int sceRtcGetTickResolution()
{
	LARGE_INTEGER frequency;
	QueryPerformanceFrequency( &frequency );
	return ( uint )frequency.LowPart;
}

void sceRtcGetCurrentTick( LARGE_INTEGER* address )
{
	QueryPerformanceCounter( address );
}

#pragma managed
