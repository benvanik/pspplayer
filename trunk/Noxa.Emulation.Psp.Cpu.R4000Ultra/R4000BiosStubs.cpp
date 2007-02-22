// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "StdAfx.h"
#define WIN32_LEAN_AND_MEAN
#include <Windows.h>

#include "R4000Cpu.h"
#include "R4000BiosStubs.h"
#include "R4000Generator.h"

using namespace System;
using namespace Noxa::Emulation::Psp::Bios;
using namespace Noxa::Emulation::Psp::Cpu;

#pragma unmanaged

// sceRtc ----------------------------------------------
int sceRtcGetTickResolution();
void sceRtcGetCurrentTick( LARGE_INTEGER* address );

// sceGeUser -------------------------------------------
//int sceGeEdramGetSize(); <-- inlined
//int sceGeEdramGetAddr(); <-- inlined
extern int sceGeListEnQueue( uint list, uint stall, int cbid, uint arg, int head );
extern int sceGeListDeQueue( int qid );
extern int sceGeListUpdateStallAddr( int qid, uint stall );
extern int sceGeListSync( int qid, int syncType );
extern int sceGeDrawSync( int syncType );

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

#ifdef NATIVEVIDEOINTERFACE
	// Note for video stuff: we must have a native video inteface for it to work!
	bool nativeVideoInterface = ( R4000Cpu::GlobalCpu->Emulator->Video->NativeInterface != IntPtr::Zero );

	if( nativeVideoInterface == true )
	{
		switch( nid )
		{
		// sceGeUser -------------------------------------------
		case 0x1f6752ad:		// sceGeEdramGetSize
			g->mov( EAX, 0x001fffff );
			return true;
		case 0xe47e40e4:		// sceGeEdramGetAddr
			g-> mov( EAX, 0x04000000 );
			return true;
		case 0xab49e76a:		// sceGeListEnQueue
			g->push( ( uint )0 ); // head = false
			g->push( MREG( CTX, 7 ) );
			g->push( MREG( CTX, 6 ) );
			g->push( MREG( CTX, 5 ) );
			g->push( MREG( CTX, 4 ) );
			g->call( ( int )sceGeListEnQueue );
			g->add( ESP, 20 );
			return true;
		case 0x1c0d95a6:		// sceGeListEnQueueHead
			g->push( ( uint )1 ); // head = true
			g->push( MREG( CTX, 7 ) );
			g->push( MREG( CTX, 6 ) );
			g->push( MREG( CTX, 5 ) );
			g->push( MREG( CTX, 4 ) );
			g->call( ( int )sceGeListEnQueue );
			g->add( ESP, 20 );
			return true;
		case 0x5fb86ab0:		// sceGeListDeQueue
			g->push( MREG( CTX, 4 ) );
			g->call( ( int )sceGeListDeQueue );
			g->add( ESP, 4 );
			return true;
		case 0xe0d68148:		// sceGeListUpdateStallAddr
			g->push( MREG( CTX, 5 ) );
			g->push( MREG( CTX, 4 ) );
			g->call( ( int )sceGeListUpdateStallAddr );
			g->add( ESP, 8 );
			return true;
		case 0x03444eb4:		// sceGeListSync
			g->push( MREG( CTX, 5 ) );
			g->push( MREG( CTX, 4 ) );
			g->call( ( int )sceGeListSync );
			g->add( ESP, 8 );
			return true;
		case 0xb287bd61:		// sceGeDrawSync
			g->push( MREG( CTX, 4 ) );
			g->call( ( int )sceGeDrawSync );
			g->add( ESP, 4 );
			return true;
		}
	}
#endif

	return false;
}

#pragma unmanaged

// sceRtc ----------------------------------------------

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
