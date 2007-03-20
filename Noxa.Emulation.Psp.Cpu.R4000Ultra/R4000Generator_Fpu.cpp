// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "StdAfx.h"
#include <math.h>
#include <float.h>
#include <assert.h>
#include "emmintrin.h"
#include "xmmintrin.h"

#include "R4000Generator.h"
#include "R4000Cpu.h"
#include "R4000Core.h"
#include "R4000Memory.h"
#include "R4000GenContext.h"
#include "Tracer.h"

#include "CodeGenerator.h"

using namespace System::Diagnostics;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::CodeGen;
using namespace Noxa::Emulation::Psp::Cpu;

#define g context->Generator

// W = int, S = float
#define CONVERT( srcfmt, tgtfmt, r ) if( srcfmt != tgtfmt ){ \
	if( tgtfmt == 0 ){ /* float */ } \
	else if( tgtfmt == 4 ){ /* int */ } }

// We have both an x86 and an SSE code path - because they weren't that hard, and
// I couldn't find enough info to make a perf conclusion without doing my own tests
// Define SSE to use the SSE set, else x87 is used
#define SSE

// This will, whenever the RC bits from MXCSR are used, ensure they
// are set to the proper rounding mode for the operation that is about
// to be performed - this is WAYYY slow, and should only be used if
// we know that round even (RC=00) is not set coming in
//#define SSE_ENSURERC

// Some things may be faster w/ x87 - define this to try those
// instead of SSE
#define TRYFASTERX87

// Try to prevent -0.0 and other weird cases
//#define CAUTIOUSFPU

// Add a bunch of checks for +/-inf, etc
//#define DEBUGFPU

// -- macro hacks
#ifdef SSE
#ifdef TRYFASTERX87
#define SSEOPT
#endif
#endif

#ifdef DEBUGFPU
#define ASSERTXMM0VALID() { g->push( ( uint )address ); g->call( ( int )assertXmm0 ); g->add( ESP, 4 ); }
#define ASSERTX87VALID() { g->push( ( uint )address ); g->call( ( int )assertFpu ); g->add( ESP, 4 ); }
#define PRINTEAX() { g->push( EAX ); g->push( ( uint )address ); g->call( ( int )printEax ); g->add( ESP, 4 ); g->pop( EAX ); }
#else
#define ASSERTXMM0VALID()
#define ASSERTX87VALID()
#define PRINTEAX()
#endif

#ifdef DEBUGFPU
#pragma unmanaged
char assertLine[150];
void assertXmm0( int address )
{
	// Check XMM0
	float x;
	uint y;
	__asm movd [x], xmm0;
	__asm movd [y], xmm0;
	assert( _finite( x ) != 0 );
	//assert( y != 0x80000000 );
	//assert( x < 1003741824 );
#ifdef TRACE
	sprintf_s( assertLine, 150, "xmm0=%f (0x%0X)\r\n", x, y );
	Tracer::WriteLine( assertLine );
#endif
}
void assertFpu( int address )
{
	// Check top of stack
	float x;
	uint y;
	__asm fst [x];
	__asm fst [y];
	assert( _finite( x ) != 0 );
#ifdef TRACE
	sprintf_s( assertLine, 150, "fs(0)=%f (0x%0X)\r\n", x, y );
	Tracer::WriteLine( assertLine );
#endif
}
void printEax( int address, int eax )
{
	//assert( eax != 0x80000000 );
#ifdef TRACE
	sprintf_s( assertLine, 150, "eax=%d (0x%0X)\r\n", eax, eax );
	Tracer::WriteLine( assertLine );
#endif
}
#pragma managed
#endif

GenerationResult FADD( R4000GenContext^ context, int pass, int address, uint code, byte fmt, byte fs, byte ft, byte fd, byte function )
{
	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
#ifdef SSE
		g->movd( XMM0, MCP1REG( CTX, fs, fmt ) );
		g->addss( XMM0, MCP1REG( CTX, ft, fmt ) );
		ASSERTXMM0VALID();
		g->movd( MCP1REG( CTX, fd, fmt ), XMM0 );
#else
		g->mov( EAX, MCP1REG( CTX, fs, fmt ) );
		g->mov( EBX, MCP1REG( CTX, ft, fmt ) );
		// EAX = EAX + EBX
		g->mov( MREG( CTX, fd ), EAX );
#endif
	}
	return GenerationResult::Success;
}

GenerationResult FSUB( R4000GenContext^ context, int pass, int address, uint code, byte fmt, byte fs, byte ft, byte fd, byte function )
{
	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
#ifdef SSE
		g->movd( XMM0, MCP1REG( CTX, fs, fmt ) );
		g->subss( XMM0, MCP1REG( CTX, ft, fmt ) );
		ASSERTXMM0VALID();
		g->movd( MCP1REG( CTX, fd, fmt ), XMM0 );
#else
		g->mov( EAX, MCP1REG( CTX, fs, fmt ) );
		g->mov( EBX, MCP1REG( CTX, ft, fmt ) );
		// EAX = EAX - EBX
		g->mov( MREG( CTX, fd ), EAX );
#endif
	}
	return GenerationResult::Success;
}

GenerationResult FMUL( R4000GenContext^ context, int pass, int address, uint code, byte fmt, byte fs, byte ft, byte fd, byte function )
{
	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
#ifdef SSE
#if 0
		// ultra super debug version
		g->movd( XMM0, MCP1REG( CTX, fs, fmt ) );
		g->mulss( XMM0, MCP1REG( CTX, ft, fmt ) );
		//ASSERTXMM0VALID();
		g->movd( EAX, XMM0 );
		//PRINTEAX();
		g->cmp( EAX, 0x80000000 );
		char xx[40];
		sprintf_s( xx, 40, "fmul%0X", address );
		g->jne( xx );
		g->movd( XMM0, MCP1REG( CTX, fs, fmt ) );
		ASSERTXMM0VALID();
		g->movd( XMM0, MCP1REG( CTX, ft, fmt ) );
		ASSERTXMM0VALID();
		//g->int3();
		g->mov( EAX, 0 );
		g->label( xx );
		PRINTEAX();
		g->mov( MCP1REG( CTX, fd, fmt ), EAX );
#else
		g->movd( XMM0, MCP1REG( CTX, fs, fmt ) );
		g->mulss( XMM0, MCP1REG( CTX, ft, fmt ) );
#ifdef CAUTIOUSFPU
		// This extra code is for handling -0.0 cases
		g->movd( EAX, XMM0 );
		g->mov( EBX, 0 );
		g->cmp( EAX, 0x80000000 );
		g->cmove( EAX, EBX );
		PRINTEAX();
		g->mov( MCP1REG( CTX, fd, fmt ), EAX );
#else
		ASSERTXMM0VALID();
		g->movd( MCP1REG( CTX, fd, fmt ), XMM0 );
#endif
#endif
#else
		g->mov( EAX, MCP1REG( CTX, fs, fmt ) );
		g->mov( EBX, MCP1REG( CTX, ft, fmt ) );
		// EAX = EAX * EBX
		g->mov( MREG( CTX, fd ), EAX );
#endif
	}
	return GenerationResult::Success;
}

GenerationResult FDIV( R4000GenContext^ context, int pass, int address, uint code, byte fmt, byte fs, byte ft, byte fd, byte function )
{
	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
#ifdef SSE
#ifdef DEBUGFPU
		g->movd( XMM0, MCP1REG( CTX, fs, fmt ) );
		ASSERTXMM0VALID();
		g->movd( XMM0, MCP1REG( CTX, ft, fmt ) );
		ASSERTXMM0VALID();
#endif
		g->movd( XMM0, MCP1REG( CTX, fs, fmt ) );
		g->divss( XMM0, MCP1REG( CTX, ft, fmt ) );
#ifdef CAUTIOUSFPU
		// This extra code is for handling -0.0 cases
		g->movd( EAX, XMM0 );
		g->mov( EBX, 0 );
		g->cmp( EAX, 0x80000000 );
		g->cmove( EAX, EBX );
		PRINTEAX();
		g->mov( MCP1REG( CTX, fd, fmt ), EAX );
#else
		ASSERTXMM0VALID();
		g->movd( MCP1REG( CTX, fd, fmt ), XMM0 );
#endif
#else
		g->mov( EAX, MCP1REG( CTX, fs, fmt ) );
		g->mov( EBX, MCP1REG( CTX, ft, fmt ) );
		// EAX = EAX / EBX
		g->mov( MREG( CTX, fd ), EAX );
#endif
	}
	return GenerationResult::Success;
}

GenerationResult FSQRT( R4000GenContext^ context, int pass, int address, uint code, byte fmt, byte fs, byte ft, byte fd, byte function )
{
	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
#ifdef SSE
		g->sqrtss( XMM0, MCP1REG( CTX, fs, fmt ) );
		ASSERTXMM0VALID();
		g->movd( MCP1REG( CTX, fd, fmt ), XMM0 );
#else
		g->mov( EAX, MCP1REG( CTX, fs, fmt ) );
		// EAX = sqrt( EAX )
		g->mov( MREG( CTX, fd ), EAX );
#endif
	}
	return GenerationResult::Success;
}

GenerationResult FABS( R4000GenContext^ context, int pass, int address, uint code, byte fmt, byte fs, byte ft, byte fd, byte function )
{
	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		// TODO: faster fabs
		g->fld( MCP1REG( CTX, fs, fmt ) );
		g->fabs();
		ASSERTX87VALID();
		g->fstp( MREG( CTX, fd ) );

//#ifdef SSE
//		g->movd( XMM0, MCP1REG( CTX, fs, fmt ) );
//		g->movd( XMM1, 0 );
//		g->cmpltss( XMM0, XMM1 );
//		// XMM0 = FFFFFFFF iff XMM0 < 0
//		//??
//		g->movd( MCP1REG( CTX, fd, fmt ), XMM0 );
//#else
//		g->mov( EAX, MCP1REG( CTX, fs, fmt ) );
//		// EAX = abs( EAX )
//		g->mov( MREG( CTX, fd ), EAX );
//#endif
	}
	return GenerationResult::Success;
}

GenerationResult FMOV( R4000GenContext^ context, int pass, int address, uint code, byte fmt, byte fs, byte ft, byte fd, byte function )
{
	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		g->mov( EAX, MCP1REG( CTX, fs, fmt ) );
		PRINTEAX();
		g->mov( MCP1REG( CTX, fd, fmt ), EAX );
	}
	return GenerationResult::Success;
}

SSE_ALIGN float _negative1 = -1.0f;

GenerationResult FNEG( R4000GenContext^ context, int pass, int address, uint code, byte fmt, byte fs, byte ft, byte fd, byte function )
{
	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
#ifndef SSEOPT
#ifdef CAUTIOUSFPU
		// Safe way (no -0.0)
		char skip[20];
		sprintf_s( skip, 20, "neg%0X", address );
		g->mov( EAX, MCP1REG( CTX, fs, fmt ) );
		g->movd( XMM0, EAX );
		g->mov( EBX, 0 );
		g->cmp( EAX, EBX );
		g->je( skip );
		g->movd( XMM1, g->dword_ptr[ &_negative1 ] );
		g->mulss( XMM0, XMM1 );
		ASSERTXMM0VALID();
		g->label( skip );
		g->movd( MCP1REG( CTX, fd, fmt ), XMM0 );
#else
		// Fast way
		g->movd( XMM0, MCP1REG( CTX, fs, fmt ) );
		g->movd( XMM1, g->dword_ptr[ &_negative1 ] );
		g->mulss( XMM0, XMM1 );
		ASSERTXMM0VALID();
		g->movd( MCP1REG( CTX, fd, fmt ), XMM0 );
#endif
#else
		g->fld( MCP1REG( CTX, fs, fmt ) );
		g->fchs();
		ASSERTX87VALID();
		g->fstp( MCP1REG( CTX, fd, fmt ) );
#endif
	}
	return GenerationResult::Success;
}

GenerationResult ROUNDL( R4000GenContext^ context, int pass, int address, uint code, byte fmt, byte fs, byte ft, byte fd, byte function ){ return GenerationResult::Invalid; }
GenerationResult TRUNCL( R4000GenContext^ context, int pass, int address, uint code, byte fmt, byte fs, byte ft, byte fd, byte function ){ return GenerationResult::Invalid; }
GenerationResult CEILL( R4000GenContext^ context, int pass, int address, uint code, byte fmt, byte fs, byte ft, byte fd, byte function ){ return GenerationResult::Invalid; }
GenerationResult FLOORL( R4000GenContext^ context, int pass, int address, uint code, byte fmt, byte fs, byte ft, byte fd, byte function ){ return GenerationResult::Invalid; }
GenerationResult ROUNDW( R4000GenContext^ context, int pass, int address, uint code, byte fmt, byte fs, byte ft, byte fd, byte function )
{
	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
#ifdef SSE
		// MXCSR rounding mode to round even
#ifdef SSE_ENSURERC
		g->sub( ESP, 8 );
		g->stmxcsr( dword_ptr[ ESP ] );
		g->mov( EBX, dword_ptr[ ESP ] );
		g->and( AH, 0x9F ); // sets RC = 00
		g->mov( dword_ptr[ ESP + 4 ], EBX );
		g->ldmxcsr( dword_ptr[ ESP + 4 ] );
#endif

		g->movd( XMM0, MCP1REG( CTX, fs, fmt ) );
		g->mov( EBX, 0x3f000000 );
		g->movd( XMM1, EBX ); // 0.5f
		g->addss( XMM0, XMM1 );
		// round even via MXCSR
		g->cvtss2si( EAX, XMM0 );
		PRINTEAX();
		g->mov( MCP1REG( CTX, fd, fmt ), EAX );

		//g->movd( XMM0, MCP1REG( CTX, fs, fmt ) );
		//// round even via MXCSR
		//g->cvtss2si( EAX, XMM0 );
		//g->cvtsi2ss( XMM0, EAX );
		//ASSERTXMM0VALID();
		//g->movd( MCP1REG( CTX, fd, fmt ), XMM0 );

		// Restore rounding mode
#ifdef SSE_ENSURERC
		g->ldmxcsr( dword_ptr[ ESP ] );
		g->add( ESP, 8 );
#endif
#else
		g->mov( EAX, MCP1REG( CTX, fs, fmt ) );
		// EAX = round even( EAX )
		g->mov( MREG( CTX, fd ), EAX );
#endif
	}
	return GenerationResult::Success;
}

GenerationResult TRUNCW( R4000GenContext^ context, int pass, int address, uint code, byte fmt, byte fs, byte ft, byte fd, byte function )
{
	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
#ifdef SSE
		g->movd( XMM0, MCP1REG( CTX, fs, fmt ) );
		// round towards zero
		ASSERTXMM0VALID();
		g->cvttss2si( EAX, XMM0 );
		PRINTEAX();
		g->mov( MCP1REG( CTX, fd, fmt ), EAX );
#else
		g->mov( EAX, MCP1REG( CTX, fs, fmt ) );
		// EAX = trunc( EAX )
		g->mov( MREG( CTX, fd ), EAX );
#endif
	}
	return GenerationResult::Success;
}

GenerationResult CEILW( R4000GenContext^ context, int pass, int address, uint code, byte fmt, byte fs, byte ft, byte fd, byte function )
{
	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
//#ifdef SSE
//		g->movd( XMM0, MCP1REG( CTX, fs, fmt ) );
//		g->cvtss2si( EAX, XMM0 );
//		g->cvtsi2ss( XMM0, EAX );
//		g->movd( MCP1REG( CTX, fd, fmt ), XMM0 );
//#else
		g->push( MCP1REG( CTX, fs, fmt ) );
		g->call( (int)ceilf );
		g->add( ESP, 4 );
		ASSERTX87VALID();
		g->fistp( MCP1REG( CTX, fd, fmt ) );
//#endif
	}
	return GenerationResult::Success;
}

GenerationResult FLOORW( R4000GenContext^ context, int pass, int address, uint code, byte fmt, byte fs, byte ft, byte fd, byte function )
{
	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
//#ifdef SSE
//		g->movd( XMM0, MCP1REG( CTX, fs, fmt ) );
//		g->cvttss2si( EAX, XMM0 );
//		g->cvtsi2ss( XMM0, EAX );
//		g->movd( MCP1REG( CTX, fd, fmt ), XMM0 );
//#else
		g->push( MCP1REG( CTX, fs, fmt ) );
		g->call( (int)floorf );
		g->add( ESP, 4 );
		ASSERTX87VALID();
		g->fistp( MCP1REG( CTX, fd, fmt ) );
//#endif
	}
	return GenerationResult::Success;
}

GenerationResult CVTS( R4000GenContext^ context, int pass, int address, uint code, byte fmt, byte fs, byte ft, byte fd, byte function )
{
	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
#ifdef SSE
		g->cvtsi2ss( XMM0, MCP1REG( CTX, fs, fmt ) );
		ASSERTXMM0VALID();
		g->movd( MCP1REG( CTX, fd, 0 ), XMM0 );
#else
		g->fild( MCP1REG( CTX, fs, fmt ) );
		ASSERTX87VALID();
		g->fstp( MCP1REG( CTX, fd, 0 ) );
#endif
	}
	return GenerationResult::Success;
}

GenerationResult CVTD( R4000GenContext^ context, int pass, int address, uint code, byte fmt, byte fs, byte ft, byte fd, byte function ){ return GenerationResult::Invalid; }
GenerationResult CVTW( R4000GenContext^ context, int pass, int address, uint code, byte fmt, byte fs, byte ft, byte fd, byte function )
{
	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
#ifdef SSE
		g->cvtss2si( EAX, MCP1REG( CTX, fs, fmt ) );
		PRINTEAX();
		//g->cmp( EAX, 0x80000000 );
		//g->cmove( EAX, 0x7FFFFFFF );
		g->mov( MCP1REG( CTX, fd, 4 ), EAX );
#else
		g->fld( MCP1REG( CTX, fs, fmt ) );
		ASSERTX87VALID();
		g->fistp( MCP1REG( CTX, fd, fmt ) );
#endif
	}
	return GenerationResult::Success;
}

GenerationResult CVTL( R4000GenContext^ context, int pass, int address, uint code, byte fmt, byte fs, byte ft, byte fd, byte function ){ return GenerationResult::Invalid; }
GenerationResult FCOMPARE( R4000GenContext^ context, int pass, int address, uint code, byte fmt, byte fs, byte ft, byte fd, byte function )
{
	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		uint fc = ( code >> 4 ) & 0x03;
		uint cond = code & 0x0F;
		bool lessBit = ( ( cond >> 2 ) & 0x1 ) == 1 ? true : false;
		bool equalBit = ( ( cond >> 1 ) & 0x1 ) == 1 ? true : false;
		bool unorderedBit = ( cond & 0x1 ) == 1 ? true : false;
		bool un = lessBit && equalBit && unorderedBit;

#ifdef SSE
		// TODO: support all FPU compare ops (ordered, etc)
		// NOTE: could do this with COMISS and SETcc instead!
		g->movd( XMM0, MCP1REG( CTX, fs, fmt ) );
		if( un == true )
		{
			g->cmpunordss( XMM0, MCP1REG( CTX, ft, fmt ) );
			// XMM0 = FFFFFFFF if unordered, else 0
		}
		else if( ( lessBit == true ) && ( equalBit == true ) )
		{
			Debug::Assert( unorderedBit == false );
			g->cmpless( XMM0, MCP1REG( CTX, ft, fmt ) );
			// XMM0 = FFFFFFFF if <=, else 0
		}
		else if( lessBit == true )
		{
			Debug::Assert( unorderedBit == false );
			g->cmpltss( XMM0, MCP1REG( CTX, ft, fmt ) );
			// XMM0 = FFFFFFFF if <, else 0
		}
		else if( equalBit == true )
		{
			Debug::Assert( unorderedBit == false );
			g->cmpeqss( XMM0, MCP1REG( CTX, ft, fmt ) );
			// XMM0 = FFFFFFFF if ==, else 0
		}
		g->movd( EAX, XMM0 );
		g->and( EAX, 0x1 );
		g->mov( MCP1CONDBIT( CTX ), EAX );
#else
		// yuck
#endif
	}
	return GenerationResult::Success;
}
