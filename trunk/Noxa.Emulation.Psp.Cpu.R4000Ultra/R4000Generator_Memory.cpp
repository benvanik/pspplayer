// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "StdAfx.h"
#include "R4000Generator.h"
#include "R4000Cpu.h"
#include "R4000Core.h"
#include "R4000Memory.h"
#include "R4000GenContext.h"

#include "Loader.hpp"
#include "CodeGenerator.hpp"

using namespace System::Diagnostics;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Cpu;
using namespace SoftWire;

#define g context->Generator

void EmitAddressTranslation( R4000Generator *gen )
{
	gen->and( gen->eax, 0x3FFFFFFF );
}

int __readMemoryThunk( int targetAddress )
{
	return R4000Cpu::GlobalCpu->Memory->ReadWord( targetAddress );
}

void __writeMemoryThunk( int targetAddress, int width, int value )
{
	R4000Cpu^ cpu = R4000Cpu::GlobalCpu;
	Debug::Assert( cpu != nullptr );
	if( cpu != nullptr )
		cpu->Memory->WriteWord( targetAddress, width, value );
	return;
}

// EAX = address, result in EAX
void EmitDirectMemoryRead( R4000GenContext^ context, int address )
{
	char label1[20];
	sprintf_s( label1, 20, "l%Xs1r", address - 4 );
	char label2[20];
	sprintf_s( label2, 20, "l%Xs2r", address - 4 );
	char label3[20];
	sprintf_s( label3, 20, "l%Xs3r", address - 4 );

	// if < 0x0800000 && > 0x09FFFFFF, skip and check framebuffer or do a read from method
	g->cmp( EAX, 0x08000000 );
	g->jb( label1 );
	g->cmp( EAX, 0x09FFFFFF );
	g->ja( label1 );

	// else, do a direct read
	g->sub( EAX, 0x08000000 ); // get to offset in main memory
	g->mov( EAX, g->dword_ptr[ EAX + (int)context->Memory->MainMemory ] );
	g->jmp( label3 );

	// case to handle read call
	g->label( label1 );

	// if < 0x0400000 && > 0x041FFFFF, skip and do a read from method
	g->cmp( EAX, 0x04000000 );
	g->jb( label2 );
	g->cmp( EAX, 0x041FFFFF );
	g->ja( label2 );
	
	// else, do a direct fb read
#ifdef IGNOREFRAMEBUFFER
	// Don't do anything!
	g->jmp( label3 );
#else
	// Not implemented! Fall through to memory call
#endif

	g->label( label2 );

	// TEST
#if 0
	char skipTest[20];
	sprintf_s( skipTest, 20, "l%Xt", address - 4 );
	g->cmp( EAX, 0x8bff28 );
	g->jne( skipTest );
	g->int3();
	g->label( skipTest );
#endif

	g->push( EAX );

	g->mov( EBX, (int)&__readMemoryThunk );
	g->call( EBX );

	g->add( ESP, 4 );

	// done
	g->label( label3 );
}

// EAX = address, EBX = data
void EmitDirectMemoryWrite( R4000GenContext^ context, int address, int width )
{
	char label1[20];
	sprintf( label1, "l%Xs1w", address - 4 );
	char label2[20];
	sprintf( label2, "l%Xs2w", address - 4 );

	// if < 0x0800000 && > 0x09FFFFFF, skip and do a write from method
	g->cmp( EAX, 0x08000000 );
	g->jb( label1 );
	g->cmp( EAX, 0x09FFFFFF );
	g->ja( label1 );

	// else, do a direct read
	g->sub( EAX, 0x08000000 ); // get to offset in main memory
	switch( width )
	{
	case 1:
		g->mov( g->byte_ptr[ EAX + (int)context->Memory->MainMemory ], BL );
		break;
	case 2:
		g->mov( g->word_ptr[ EAX + (int)context->Memory->MainMemory ], BX );
		break;
	case 4:
		g->mov( g->dword_ptr[ EAX + (int)context->Memory->MainMemory ], EBX );
		break;
	}
	g->jmp( label2 );

	// case to handle read call
	g->label( label1 );

	switch( width )
	{
	case 1:
		g->movzx( EBX, BL );
		break;
	case 2:
		g->movzx( EBX, BX );
		break;
	}
	g->push( EBX );
	g->push( (uint)width );
	g->push( EAX );
	g->mov( EBX, (int)&__writeMemoryThunk );
	g->call( EBX );
	g->add( ESP, 12 );

	// done
	g->label( label2 );
}

GenerationResult LB( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
{
	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		LOADCTXBASE( EDX );
		g->mov( EAX, MREG( CTX, rs ) );
		if( imm != 0 )
			g->add( EAX, SE( imm ) );
		EmitAddressTranslation( g );

		EmitDirectMemoryRead( context, address );

		// Byte mask & sign extend
		g->movsx( EAX, AL );

		g->mov( MREG( CTX, rt ), EAX );
	}
	return GenerationResult::Success;
}

GenerationResult LH( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
{
	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		LOADCTXBASE( EDX );
		g->mov( EAX, MREG( CTX, rs ) );
		if( imm != 0 )
			g->add( EAX, SE( imm ) );
		EmitAddressTranslation( g );

		EmitDirectMemoryRead( context, address );

		// Short mask & sign extend
		g->movsx( EAX, AX );

		g->mov( MREG( CTX, rt ), EAX );
	}
	return GenerationResult::Success;
}

GenerationResult LWL( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
{
	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		LOADCTXBASE( EDX );
		g->mov( EAX, MREG( CTX, rs ) );
		if( imm != 0 )
			g->add( EAX, SE( imm ) );
		EmitAddressTranslation( g );
		g->mov( ECX, EAX ); // store address in ECX

		// Read existing data in to EAX - dword aligned
		g->and( EAX, 0xFFFFFFFC );
		EmitDirectMemoryRead( context, address );

		// Build final data - done as follows:
		/*if( ebx == 0 )
			final = ( oldreg & 0x000000FF ) | ( ( mem & 0x00FFFFFF ) << 8 );
		else if( ebx == 1 )
			final = ( oldreg & 0x0000FFFF ) | ( ( mem & 0x0000FFFF ) << 16 );
		else if( ebx == 2 )
			final = ( oldreg & 0x00FFFFFF ) | ( ( mem & 0x000000FF ) << 24 );
		else if( ebx == 3 )
			final = mem;*/
		char ebx0[20], ebx1[20], ebx2[20], done[20];
		sprintf_s( ebx0, 20, "l%0Xx0", address - 4 );
		sprintf_s( ebx1, 20, "l%0Xx1", address - 4 );
		sprintf_s( ebx2, 20, "l%0Xx2", address - 4 );
		sprintf_s( done, 20, "l%0Xd", address - 4 );

		g->mov( EBX, ECX );
		g->and( EBX, 0x3 ); // ebx = address & 0x3

		// We use bl as byte offset and bh as comparer for switch
		// if bl == 0
		g->mov( BH, 0 );
		g->cmp( BL, BH );
		g->je( ebx0 );

		// if bl == 1
		g->inc( BH );
		g->cmp( BL, BH );
		g->je( ebx1 );

		// if bl == 2
		g->inc( BH );
		g->cmp( BL, BH );
		g->je( ebx2 );

		// EAX = memory value
		// EBX = put data to write here
		
		// case ebx == 3 (fallthrough from above) final = mem;
		g->mov( EBX, EAX );
		g->jmp( done );

		// case ebx == 0 final = ( oldreg & 0x000000FF ) | ( ( mem & 0x00FFFFFF ) << 8 );
		g->label( ebx0 );
		g->and( EAX, 0x00FFFFFF );
		g->shl( EAX, 8 );
		g->mov( EBX, MREG( CTX, rt ) );
		g->and( EBX, 0x000000FF );
		g->or( EBX, EAX );
		g->jmp( done );

		// case ebx == 1 final = ( oldreg & 0x0000FFFF ) | ( ( mem & 0x0000FFFF ) << 16 );
		g->label( ebx1 );
		g->and( EAX, 0x0000FFFF );
		g->shl( EAX, 16 );
		g->mov( EBX, MREG( CTX, rt ) );
		g->and( EBX, 0x0000FFFF );
		g->or( EBX, EAX );
		g->jmp( done );

		// case ebx == 2 final = ( oldreg & 0x00FFFFFF ) | ( ( mem & 0x000000FF ) << 24 );
		g->label( ebx2 );
		g->and( EAX, 0x000000FF );
		g->shl( EAX, 24 );
		g->mov( EBX, MREG( CTX, rt ) );
		g->and( EBX, 0x00FFFFFF );
		g->or( EBX, EAX );
		// No jump - fall through

		// Store data back
		g->label( done );
		g->mov( MREG( CTX, rt ), EBX );
	}
	return GenerationResult::Success;
}

GenerationResult LW( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
{
	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		LOADCTXBASE( EDX );
		g->mov( EAX, MREG( CTX, rs ) );
		if( imm != 0 )
			g->add( EAX, SE( imm ) );
		EmitAddressTranslation( g );

		EmitDirectMemoryRead( context, address );

		g->mov( MREG( CTX, rt ), EAX );
	}
	return GenerationResult::Success;
}

GenerationResult LBU( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
{
	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		LOADCTXBASE( EDX );
		g->mov( EAX, MREG( CTX, rs ) );
		if( imm != 0 )
			g->add( EAX, SE( imm ) );
		EmitAddressTranslation( g );

		EmitDirectMemoryRead( context, address );

		// Byte mask
		g->movzx( EAX, AL );

		g->mov( MREG( CTX, rt ), EAX );
	}
	return GenerationResult::Success;
}

GenerationResult LHU( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
{
	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		LOADCTXBASE( EDX );
		g->mov( EAX, MREG( CTX, rs ) );
		if( imm != 0 )
			g->add( EAX, SE( imm ) );
		EmitAddressTranslation( g );

		EmitDirectMemoryRead( context, address );

		// Short mask
		g->movzx( EAX, AX );

		g->mov( MREG( CTX, rt ), EAX );
	}
	return GenerationResult::Success;
}

GenerationResult LWR( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
{
	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		LOADCTXBASE( EDX );
		g->mov( EAX, MREG( CTX, rs ) );
		if( imm != 0 )
			g->add( EAX, SE( imm ) );
		EmitAddressTranslation( g );
		g->mov( ECX, EAX ); // store address in ECX

		// Read existing data in to EAX - dword aligned
		g->and( EAX, 0xFFFFFFFC );
		EmitDirectMemoryRead( context, address );

		// Build final data - done as follows:
		/*if( ebx == 0 )
			final = mem;
		else if( ebx == 1 )
			final = ( oldreg & 0xFF000000 ) | ( ( mem & 0xFFFFFF00 ) >> 8 );
		else if( ebx == 2 )
			final = ( oldreg & 0xFFFF0000 ) | ( ( mem & 0xFFFF0000 ) >> 16 );
		else if( ebx == 3 )
			final = ( oldreg & 0xFFFFFF00 ) | ( ( mem & 0xFF000000 ) >> 24 );*/
		char ebx3[20], ebx2[20], ebx1[20], done[20];
		sprintf_s( ebx3, 20, "l%0Xx3", address - 4 );
		sprintf_s( ebx2, 20, "l%0Xx2", address - 4 );
		sprintf_s( ebx1, 20, "l%0Xx1", address - 4 );
		sprintf_s( done, 20, "l%0Xd", address - 4 );

		g->mov( EBX, ECX );
		g->and( EBX, 0x3 ); // ebx = address & 0x3

		// We use bl as byte offset and bh as comparer for switch
		// if bl == 3
		g->mov( BH, 3 );
		g->cmp( BL, BH );
		g->je( ebx3 );

		// if bl == 2
		g->dec( BH );
		g->cmp( BL, BH );
		g->je( ebx2 );

		// if bl == 1
		g->dec( BH );
		g->cmp( BL, BH );
		g->je( ebx1 );

		// EAX = memory value
		// EBX = put data to write here
		
		// case ebx == 0 (fallthrough from above) final = mem;
		g->mov( EBX, EAX );
		g->jmp( done );

		// case ebx == 3 final = ( oldreg & 0xFF000000 ) | ( ( mem & 0xFFFFFF00 ) >> 8 );
		g->label( ebx3 );
		g->and( EAX, 0xFFFFFF00 );
		g->shr( EAX, 8 );
		g->mov( EBX, MREG( CTX, rt ) );
		g->and( EBX, 0xFF000000 );
		g->or( EBX, EAX );
		g->jmp( done );

		// case ebx == 2 final = ( oldreg & 0xFFFF0000 ) | ( ( mem & 0xFFFF0000 ) >> 16 );
		g->label( ebx2 );
		g->and( EAX, 0xFFFF0000 );
		g->shr( EAX, 16 );
		g->mov( EBX, MREG( CTX, rt ) );
		g->and( EBX, 0xFFFF0000 );
		g->or( EBX, EAX );
		g->jmp( done );

		// case ebx == 1 final = ( oldreg & 0xFFFFFF00 ) | ( ( mem & 0xFF000000 ) >> 24 );
		g->label( ebx1 );
		g->and( EAX, 0xFF000000 );
		g->shr( EAX, 24 );
		g->mov( EBX, MREG( CTX, rt ) );
		g->and( EBX, 0xFFFFFF00 );
		g->or( EBX, EAX );
		// No jump - fall through

		// Store data back
		g->label( done );
		g->mov( MREG( CTX, rt ), EBX );
	}
	return GenerationResult::Success;
}

GenerationResult SB( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
{
	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		LOADCTXBASE( EDX );
		g->mov( EAX, MREG( CTX, rs ) );
		if( imm != 0 )
			g->add( EAX, SE( imm ) );
		EmitAddressTranslation( g );
		g->mov( EBX, MREG( CTX, rt ) );

		EmitDirectMemoryWrite( context, address, 1 );
	}
	return GenerationResult::Success;
}

GenerationResult SH( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
{
	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		LOADCTXBASE( EDX );
		g->mov( EAX, MREG( CTX, rs ) );
		if( imm != 0 )
			g->add( EAX, SE( imm ) );
		EmitAddressTranslation( g );
		g->mov( EBX, MREG( CTX, rt ) );

		EmitDirectMemoryWrite( context, address, 2 );
	}
	return GenerationResult::Success;
}

GenerationResult SWL( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
{
	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		LOADCTXBASE( EDX );
		g->mov( EAX, MREG( CTX, rs ) );
		if( imm != 0 )
			g->add( EAX, SE( imm ) );
		EmitAddressTranslation( g );
		g->mov( ECX, EAX ); // store address in ECX

		// Read existing data in to EAX - dword aligned
		g->and( EAX, 0xFFFFFFFC );
		EmitDirectMemoryRead( context, address );

		// Build final data - done as follows:
		/*if( ebx == 0 )
			final = ( mem & 0xFFFFFF00 ) | ( ( source >> 24 ) & 0x000000FF );
		else if( ebx == 1 )
			final = ( mem & 0xFFFF0000 ) | ( ( source >> 16 ) & 0x0000FFFF );
		else if( ebx == 2 )
			final = ( mem & 0xFF000000 ) | ( ( source >> 8 ) & 0x00FFFFFF );
		else if( ebx == 3 )
			final = source;*/
		char ebx0[20], ebx1[20], ebx2[20], done[20];
		sprintf_s( ebx0, 20, "l%0Xx0", address - 4 );
		sprintf_s( ebx1, 20, "l%0Xx1", address - 4 );
		sprintf_s( ebx2, 20, "l%0Xx2", address - 4 );
		sprintf_s( done, 20, "l%0Xd", address - 4 );

		g->mov( EBX, ECX );
		g->and( EBX, 0x3 ); // ebx = address & 0x3

		// We use bl as byte offset and bh as comparer for switch
		// if bl == 0
		g->mov( BH, 0 );
		g->cmp( BL, BH );
		g->je( ebx0 );

		// if bl == 1
		g->inc( BH );
		g->cmp( BL, BH );
		g->je( ebx1 );

		// if bl == 2
		g->inc( BH );
		g->cmp( BL, BH );
		g->je( ebx2 );

		// EAX = memory value
		// EBX = put data to write here
		// ECX = address
		
		// case ebx == 3 (fallthrough from above)
		g->mov( EBX, MREG( CTX, rt ) );
		g->jmp( done );

		// case ebx == 0
		g->label( ebx0 );
		g->and( EAX, 0xFFFFFF00 );
		g->mov( EBX, MREG( CTX, rt ) );
		g->shr( EBX, 24 );
		g->and( EBX, 0x000000FF );
		g->or( EBX, EAX );
		g->jmp( done );

		// case ebx == 1
		g->label( ebx1 );
		g->and( EAX, 0xFFFF0000 );
		g->mov( EBX, MREG( CTX, rt ) );
		g->shr( EBX, 16 );
		g->and( EBX, 0x0000FFFF );
		g->or( EBX, EAX );
		g->jmp( done );

		// case ebx == 2
		g->label( ebx2 );
		g->and( EAX, 0xFF000000 );
		g->mov( EBX, MREG( CTX, rt ) );
		g->shr( EBX, 8 );
		g->and( EBX, 0x00FFFFFF );
		g->or( EBX, EAX );
		// No jump - fall through

		// Store data back
		g->label( done );
		g->mov( EAX, ECX );
		g->and( EAX, 0xFFFFFFFC );
		// Write EBX to address EAX
		EmitDirectMemoryWrite( context, address, 4 );
	}
	return GenerationResult::Success;
}

GenerationResult SW( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
{
	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		LOADCTXBASE( EDX );
		g->mov( EAX, MREG( CTX, rs ) );
		if( imm != 0 )
			g->add( EAX, SE( imm ) );
		EmitAddressTranslation( g );
		g->mov( EBX, MREG( CTX, rt ) );

		EmitDirectMemoryWrite( context, address, 4 );
	}
	return GenerationResult::Success;
}

GenerationResult SWR( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
{
	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		LOADCTXBASE( EDX );
		g->mov( EAX, MREG( CTX, rs ) );
		if( imm != 0 )
			g->add( EAX, SE( imm ) );
		EmitAddressTranslation( g );
		g->mov( ECX, EAX ); // store address in ECX

		// Read existing data in to EAX - dword aligned
		g->and( EAX, 0xFFFFFFFC );
		EmitDirectMemoryRead( context, address );

		// Build final data - done as follows:
		/*if( ebx == 0 )
			final = source;
		else if( ebx == 1 )
			final = ( mem & 0x000000FF ) | ( ( source << 8 ) & 0xFFFFFF00 );
		else if( ebx == 2 )
			final = ( mem & 0x0000FFFF ) | ( ( source << 16 ) & 0xFFFF0000 );
		else if( ebx == 3 )
			final = ( mem & 0x00FFFFFF ) | ( ( source << 24 ) & 0xFF000000 );
			*/
		char ebx3[20], ebx2[20], ebx1[20], done[20];
		sprintf_s( ebx3, 20, "l%0Xx3", address - 4 );
		sprintf_s( ebx2, 20, "l%0Xx2", address - 4 );
		sprintf_s( ebx1, 20, "l%0Xx1", address - 4 );
		sprintf_s( done, 20, "l%0Xd", address - 4 );

		g->mov( EBX, ECX );
		g->and( EBX, 0x3 ); // ebx = address & 0x3

		// We use bl as byte offset and bh as comparer for switch
		// REVERSE OF SWL as we want fallthrough case to be the same
		// if bl == 3
		g->mov( BH, 3 );
		g->cmp( BL, BH );
		g->je( ebx3 );

		// if bl == 2
		g->dec( BH );
		g->cmp( BL, BH );
		g->je( ebx2 );

		// if bl == 1
		g->dec( BH );
		g->cmp( BL, BH );
		g->je( ebx1 );

		// EAX = memory value
		// EBX = put data to write here
		// ECX = address
		
		// case ebx == 0 (fallthrough from above)
		g->mov( EBX, MREG( CTX, rt ) );
		g->jmp( done );

		// case ebx == 3
		g->label( ebx3 );
		g->and( EAX, 0x000000FF );
		g->mov( EBX, MREG( CTX, rt ) );
		g->shl( EBX, 8 );
		g->and( EBX, 0xFFFFFF00 );
		g->or( EBX, EAX );
		g->jmp( done );

		// case ebx == 2
		g->label( ebx2 );
		g->and( EAX, 0x0000FFFF );
		g->mov( EBX, MREG( CTX, rt ) );
		g->shr( EBX, 16 );
		g->and( EBX, 0xFFFF0000 );
		g->or( EBX, EAX );
		g->jmp( done );

		// case ebx == 1
		g->label( ebx1 );
		g->and( EAX, 0x00FFFFFF );
		g->mov( EBX, MREG( CTX, rt ) );
		g->shr( EBX, 24 );
		g->and( EBX, 0xFF000000 );
		g->or( EBX, EAX );
		// No jump - fall through

		// Store data back
		g->label( done );
		g->mov( EAX, ECX );
		g->and( EAX, 0xFFFFFFFC );
		// Write EBX to address EAX
		EmitDirectMemoryWrite( context, address, 4 );
	}
	return GenerationResult::Success;
}

GenerationResult CACHE( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
{
	// Not implemented on purpose
	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
	}
	return GenerationResult::Success;
}

GenerationResult LL( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
{
	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
	}
	return GenerationResult::Invalid;
}

GenerationResult SC( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
{
	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
	}
	return GenerationResult::Invalid;
}

GenerationResult LWCz( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
{
	byte cop = ( byte )( opcode & 0x3 );
	if( ( cop == 0 ) || ( cop == 2 ) )
		return GenerationResult::Invalid;

	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		LOADCTXBASE( EDX );
		g->mov( EAX, MREG( CTX, rs ) );
		if( imm != 0 )
			g->add( EAX, SE( imm ) );
		EmitAddressTranslation( g );

		EmitDirectMemoryRead( context, address );

		switch( cop )
		{
		case 0:
			//g->mov( MCP0REG( rt ), EAX );
			break;
		case 1:
			g->mov( MCP1REG( CTX, rt, 0 ), EAX );
			break;
		case 2:
			//g->mov( MCP2REG( rt ), EAX );
			break;
		}
	}
	return GenerationResult::Success;
}

GenerationResult SWCz( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
{
	byte cop = ( byte )( opcode & 0x3 );
	if( ( cop == 0 ) || ( cop == 2 ) )
		return GenerationResult::Invalid;

	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		LOADCTXBASE( EDX );
		g->mov( EAX, MREG( CTX, rs ) );
		if( imm != 0 )
			g->add( EAX, SE( imm ) );
		EmitAddressTranslation( g );
		
		switch( cop )
		{
		case 0:
			break;
		case 1:
			g->mov( EBX, MCP1REG( CTX, rt, 0 ) );
			break;
		case 2:
			break;
		}

		EmitDirectMemoryWrite( context, address, 4 );
	}
	return GenerationResult::Success;
}
