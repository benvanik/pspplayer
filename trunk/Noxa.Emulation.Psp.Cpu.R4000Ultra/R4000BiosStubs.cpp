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

// sceGeUser -------------------------------------------
//int sceGeEdramGetSize(); <-- inlined
//int sceGeEdramGetAddr(); <-- inlined
int sceGeListEnQueue( const void* list, const void* stall, int cbid, const void* arg );
int sceGeListEnQueueHead( const void* list, const void* stall, int cbid, const void* arg );
int sceGeListDeQueue( int qid );
int sceGeListUpdateStallAddr( int qid, const void* stall );
int sceGeListSync( int qid, int syncType );
int sceGeDrawSync( int syncType );

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

	// sceGeUser -------------------------------------------
	case 0x1f6752ad:		// sceGeEdramGetSize
		g->mov( EAX, 0x001fffff );
		return true;
	case 0xe47e40e4:		// sceGeEdramGetAddr
		g-> mov( EAX, 0x04000000 );
		return true;
	case 0xab49e76a:		// sceGeListEnQueue
		g->push( MREG( CTX, 7 ) );
		g->push( MREG( CTX, 6 ) );
		g->push( MREG( CTX, 5 ) );
		g->push( MREG( CTX, 4 ) );
		g->call( ( int )sceGeListEnQueue );
		g->add( ESP, 16 );
		return true;
	case 0x1c0d95a6:		// sceGeListEnQueueHead
		g->push( MREG( CTX, 7 ) );
		g->push( MREG( CTX, 6 ) );
		g->push( MREG( CTX, 5 ) );
		g->push( MREG( CTX, 4 ) );
		g->call( ( int )sceGeListEnQueueHead );
		g->add( ESP, 16 );
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

// sceGeUser -------------------------------------------

int sceGeListEnQueue( const void* list, const void* stall, int cbid, const void* arg )
{
	return 0;
}

int sceGeListEnQueueHead( const void* list, const void* stall, int cbid, const void* arg )
{
	return 0;
}

int sceGeListDeQueue( int qid )
{
	return 0;
}

int sceGeListUpdateStallAddr( int qid, const void* stall )
{
	return 0;
}

int sceGeListSync( int qid, int syncType )
{
	return 0;
}

int sceGeDrawSync( int syncType )
{
	return 0;
}

#pragma managed
