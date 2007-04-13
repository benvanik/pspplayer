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

#include "CodeGenerator.h"

using namespace System::Diagnostics;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::CodeGen;
using namespace Noxa::Emulation::Psp::Cpu;

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
	Label* l1 = g->DefineLabel();
	Label* l2 = g->DefineLabel();
	Label* l3 = g->DefineLabel();

	// if < 0x0800000 && > MainMemoryBound, skip and check framebuffer or do a read from method
	g->cmp( EAX, MainMemoryBase );
	g->jb( l1 );
	g->cmp( EAX, MainMemoryBound );
	g->ja( l1 );

	// else, do a direct main memory read
	g->sub( EAX, MainMemoryBase ); // get to offset in main memory
	g->mov( EAX, g->dword_ptr[ EAX + (int)context->MainMemory ] );
	g->jmp( l3 );

	// case to handle read call
	g->MarkLabel( l1 );

	// if < 0x0400000 && > VideoMemoryBound, skip and do a read from method
	g->cmp( EAX, VideoMemoryBase );
	g->jb( l2 );
	g->cmp( EAX, VideoMemoryBound );
	g->ja( l2 );

	// else, do a direct fb read
	g->sub( EAX, VideoMemoryBase );
	g->mov( EAX, g->dword_ptr[ EAX + (int)context->FrameBuffer ] );
	g->jmp( l3 );

	g->MarkLabel( l2 );

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
	g->MarkLabel( l3 );
}

// EAX = address, EBX = data
void EmitDirectMemoryWrite( R4000GenContext^ context, int address, int width )
{
	Label* l1 = g->DefineLabel();
	Label* l2 = g->DefineLabel();
	Label* l3 = g->DefineLabel();

	// if < 0x0800000 && > MainMemoryBound, skip and do a write from method
	g->cmp( EAX, MainMemoryBase );
	g->jb( l1 );
	g->cmp( EAX, MainMemoryBound );
	g->ja( l1 );

#if 0
	Label* skipTest = g->DefineLabel();
	Label* hitTest = g->DefineLabel();
	g->cmp( EAX, 0x97BF6BC );
	g->je( hitTest );
	g->cmp( EAX, 0x97BF6BD );
	g->je( hitTest );
	g->cmp( EAX, 0x97BF6BE );
	g->je( hitTest );
	g->cmp( EAX, 0x97BF6BF );
	g->je( hitTest );
	g->cmp( EAX, 0x97BF6C0 );
	g->je( hitTest );
	g->cmp( EAX, 0x97BF6C1 );
	g->je( hitTest );
	g->jmp( skipTest );
	g->MarkLabel( hitTest );
	g->int3();
	g->MarkLabel( skipTest );
#endif

	// else, do a direct read
	g->sub( EAX, MainMemoryBase ); // get to offset in main memory
	switch( width )
	{
	case 1:
		g->mov( g->byte_ptr[ EAX + (int)context->MainMemory ], BL );
		break;
	case 2:
		g->mov( g->word_ptr[ EAX + (int)context->MainMemory ], BX );
		break;
	case 4:
		g->mov( g->dword_ptr[ EAX + (int)context->MainMemory ], EBX );
		break;
	}
	g->jmp( l3 );

	// case to handle read call
	g->MarkLabel( l1 );

	// if < 0x0400000 && > VideoMemoryBound, skip and do a read from method
	g->cmp( EAX, VideoMemoryBase );
	g->jb( l2 );
	g->cmp( EAX, VideoMemoryBound );
	g->ja( l2 );
	
	// else, do a direct fb read
	g->sub( EAX, VideoMemoryBase ); // get to offset in fb
	switch( width )
	{
	case 1:
		g->mov( g->byte_ptr[ EAX + (int)context->FrameBuffer ], BL );
		break;
	case 2:
		g->mov( g->word_ptr[ EAX + (int)context->FrameBuffer ], BX );
		break;
	case 4:
		g->mov( g->dword_ptr[ EAX + (int)context->FrameBuffer ], EBX );
		break;
	}
	g->jmp( l3 );

	g->MarkLabel( l2 );

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
	g->MarkLabel( l3 );
}

GenerationResult LB( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
{
	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
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
		//LOADCTXBASE( EDX );
		g->mov( EAX, MREG( CTX, rs ) );
		if( imm != 0 )
			g->add( EAX, SE( imm ) );
		EmitAddressTranslation( g );
		g->mov( ECX, EAX ); // store address in ECX

		// Read existing data in to EAX - dword aligned
		g->and( EAX, 0xFFFFFFFC );
		EmitDirectMemoryRead( context, address );

		// Build final data - done as follows:
		/*
		if( ebx == 0 )
			final = ( oldreg & 0x00FFFFFF ) | ( ( mem << 24 ) & 0xFF000000 );	- m 24
		else if( ebx == 1 )
			final = ( oldreg & 0x0000FFFF ) | ( ( mem << 16 ) & 0xFFFF0000 );	- m 16
		else if( ebx == 2 )
			final = ( oldreg & 0x000000FF ) | ( ( mem << 8 ) & 0xFFFFFF00 );	- m 8
		else if( ebx == 3 )
			final = ( oldreg & 0x00000000 ) | ( ( mem << 0 ) & 0xFFFFFFFF );	- m 0	
		*/

		// With this, we do:
		// ecx = [0...3] (from addr)
		// ecx = xor 3 (invert bits to make easier)
		// ecx *= 8 (so [0...24])
		// ebx = 0xFFFFFFFF << cl
		// ebx is now the mask for the memory!
		// invert to get mask for oldreg!

		g->and( ECX, 0x3 ); // ecx = address (in ecx) & 0x3
		g->xor( ECX, 0x3 );
		g->shl( ECX, 3 );	// *= 8
		g->mov( EBX, 0xFFFFFFFF );
		g->shl( EBX, CL );

		g->shl( EAX, CL );		// shift memory over to match mask
		g->and( EAX, EBX );		// mem (in eax) &= mask in ebx

		g->not( EBX );			// invert mask for oldreg
		g->mov( ECX, MREG( CTX, rt ) );
		g->and( ECX, EBX );		// oldreg (in ecx) &= mask in ebx

		g->or( EAX, ECX );		// mem | oldreg

		g->mov( MREG( CTX, rt ), EAX );
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
		g->mov( EAX, MREG( CTX, rs ) );
		if( imm != 0 )
			g->add( EAX, SE( imm ) );
		EmitAddressTranslation( g );

		EmitDirectMemoryRead( context, address );

		// Byte mask
		g->and( EAX, 0x000000FF );

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
		g->mov( EAX, MREG( CTX, rs ) );
		if( imm != 0 )
			g->add( EAX, SE( imm ) );
		EmitAddressTranslation( g );

		EmitDirectMemoryRead( context, address );

		// Short mask
		g->and( EAX, 0x0000FFFF );

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
		g->mov( EAX, MREG( CTX, rs ) );
		if( imm != 0 )
			g->add( EAX, SE( imm ) );
		EmitAddressTranslation( g );
		g->mov( ECX, EAX ); // store address in ECX

		// Read existing data in to EAX - dword aligned
		g->and( EAX, 0xFFFFFFFC );
		EmitDirectMemoryRead( context, address );

		// Build final data - done as follows:
		/*
		if( ebx == 0 )
			final = ( oldreg & 0x00000000 ) | ( ( mem >> 0 ) & 0xFFFFFFFF );	- m 0
		else if( ebx == 1 )
			final = ( oldreg & 0xFF000000 ) | ( ( mem >> 8 ) & 0x00FFFFFF );	- m 8
		else if( ebx == 2 )
			final = ( oldreg & 0xFFFF0000 ) | ( ( mem >> 16 ) & 0x0000FFFF );	- m 16
		if( ebx == 3 )
			final = ( oldreg & 0xFFFFFF00 ) | ( ( mem >> 24 ) & 0x000000FF );	- m 24
		*/

		// With this, we do:
		// ecx = [0...3] (from addr)
		// ecx *= 8 (so [0...24])
		// ebx = 0xFFFFFFFF >> cl
		// ebx is now the mask for the memory!
		// invert to get mask for oldreg!

		g->and( ECX, 0x3 ); // ecx = address (in ecx) & 0x3
		g->shl( ECX, 3 );	// *= 8
		g->mov( EBX, 0xFFFFFFFF );
		g->shr( EBX, CL );

		g->shr( EAX, CL );		// shift to match mask
		g->and( EAX, EBX );		// mem (in eax) &= mask in ebx

		g->not( EBX );			// invert mask for oldreg
		g->mov( ECX, MREG( CTX, rt ) );
		g->and( ECX, EBX );		// oldreg (in ecx) &= mask in ebx

		g->or( EAX, ECX );		// mem | oldreg

		g->mov( MREG( CTX, rt ), EAX );
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
		//LOADCTXBASE( EDX );
		g->mov( EAX, MREG( CTX, rs ) );
		if( imm != 0 )
			g->add( EAX, SE( imm ) );
		EmitAddressTranslation( g );
		g->mov( ECX, EAX ); // store address in ECX

		// Read existing data in to EAX - dword aligned
		g->and( EAX, 0xFFFFFFFC );
		EmitDirectMemoryRead( context, address );

		// Build final data - done as follows:
		/*
		if( ebx == 0 )
			final = ( ( reg >> 24 ) & 0x000000FF ) | ( mem & 0xFFFFFF00 );	- m 24
		else if( ebx == 1 )
			final = ( ( reg >> 16 ) & 0x0000FFFF ) | ( mem & 0xFFFF0000 );	- m 16
		else if( ebx == 2 )
			final = ( ( reg >> 8 ) & 0x00FFFFFF ) | ( mem & 0xFF000000 );	- m 8
		else if( ebx == 3 )
			final = ( ( reg >> 0 ) & 0xFFFFFFFF ) | ( mem & 0x00000000 );	- m 0
		*/

		// With this, we do:
		// ecx = [0...3] (from addr)
		// ecx = xor 3 (invert to make easier)
		// ecx *= 8 (so [0...24])
		// ebx = 0xFFFFFFFF >> cl
		// ebx is now the mask for the reg!
		// invert to get mask for memory!

		// NOTE: we technically don't need to and the register here,
		// as shifting will do it for us!

		//g->int3();
		g->and( ECX, 0x3 ); // ecx = address (in ecx) & 0x3
		g->xor( ECX, 3 );	// invert
		g->shl( ECX, 3 );	// *= 8
		g->mov( EBX, 0xFFFFFFFF );
		g->shr( EBX, CL );

		g->mov( EDX, MREG( CTX, rt ) );
		g->shr( EDX, CL );		// shift to match mask
		g->and( EDX, EBX );		// reg (in ecx) &= mask in ebx

		g->not( EBX );			// invert mask for memory
		g->and( EAX, EBX );		// mem (in eax) &= mask in ebx

		g->or( EAX, EDX );		// mem | oldreg

		g->mov( EBX, EAX );
		
		// Reget the address!
		g->mov( EAX, MREG( CTX, rs ) );
		if( imm != 0 )
			g->add( EAX, SE( imm ) );
		EmitAddressTranslation( g );
		g->and( EAX, 0xFFFFFFFC );		// word align
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
		//LOADCTXBASE( EDX );
		g->mov( EAX, MREG( CTX, rs ) );
		if( imm != 0 )
			g->add( EAX, SE( imm ) );
		EmitAddressTranslation( g );
		g->mov( ECX, EAX ); // store address in ECX

		// Read existing data in to EAX - dword aligned
		g->and( EAX, 0xFFFFFFFC );
		EmitDirectMemoryRead( context, address );

		// Build final data - done as follows:
		/*
		if( ebx == 0 )
			final = ( ( reg << 0 ) & 0xFFFFFFFF ) | ( mem & 0x00000000 );	- m 24
		else if( ebx == 1 )
			final = ( ( reg << 8 ) & 0xFFFFFF00 ) | ( mem & 0x000000FF );	- m 16
		else if( ebx == 2 )
			final = ( ( reg << 16 ) & 0xFFFF0000 ) | ( mem & 0x0000FFFF );	- m 8
		else if( ebx == 3 )
			final = ( ( reg << 24 ) & 0xFF000000 ) | ( mem & 0x00FFFFFF );	- m 0
		*/

		// With this, we do:
		// ecx = [0...3] (from addr)
		// ecx *= 8 (so [0...24])
		// ebx = 0xFFFFFFFF << cl
		// ebx is now the mask for the reg!
		// invert to get mask for memory!

		// NOTE: we technically don't need to and the register here,
		// as shifting will do it for us!

		//g->int3();
		g->and( ECX, 0x3 ); // ecx = address (in ecx) & 0x3
		g->shl( ECX, 3 );	// *= 8
		g->mov( EBX, 0xFFFFFFFF );
		g->shl( EBX, CL );

		g->mov( EDX, MREG( CTX, rt ) );
		g->shl( EDX, CL );		// shift to match mask
		g->and( EDX, EBX );		// reg (in ecx) &= mask in ebx

		g->not( EBX );			// invert mask for memory
		g->and( EAX, EBX );		// mem (in eax) &= mask in ebx

		g->or( EAX, EDX );		// mem | oldreg

		g->mov( EBX, EAX );

		// Reget the address!
		g->mov( EAX, MREG( CTX, rs ) );
		if( imm != 0 )
			g->add( EAX, SE( imm ) );
		EmitAddressTranslation( g );
		g->and( EAX, 0xFFFFFFFC );		// word align
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
