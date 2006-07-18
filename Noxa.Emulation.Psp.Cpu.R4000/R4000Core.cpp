#include "StdAfx.h"
#include "R4000Core.h"

#include "R4000Cpu.h"
#include "Memory.h"
#include "R4000Cp0.h"
#include "R4000Cp1.h"
#include "R4000Cp2.h"

using namespace System::Diagnostics;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Cpu;

R4000Core::R4000Core( R4000Cpu^ cpu, String^ name, int coreId, CoreAttributes attributes )
{
	_cpu = cpu;
	_memory = ( Memory^ )_cpu->Memory;
	_name = name;
	_coreId = coreId;

	_cp0 = gcnew R4000Cp0( this );
	_cp1 = gcnew R4000Cp1( this );

	bool hasCp2 = ( attributes & CoreAttributes::HasCp2 ) == CoreAttributes::HasCp2;
	if( hasCp2 )
		_cp2 = gcnew R4000Cp2( this );
	
	_cops = gcnew array<R4000Coprocessor^>( hasCp2 ? 3 : 2 );
	_cops[ 0 ] = _cp0;
	_cops[ 1 ] = _cp1;
	if( hasCp2 == true )
		_cops[ 2 ] = _cp2;

	Registers = gcnew array<int>( 32 );
	
	this->Clear();
}

void R4000Core::Clear()
{
	Pc = 0;
	for( int n = 0; n < Registers->Length; n++ )
		Registers[ n ] = 0;
	Registers[ 29 ] = 0x087FFFFF;
	Hi = 0;
	Lo = 0;

	Cp0->Clear();
	Cp1->Clear();
	if( Cp2 != nullptr )
		Cp2->Clear();
}

Object^ R4000Core::Context::get()
{
	R4000CoreContext^ context = gcnew R4000CoreContext();
	
	context->ProgramCounter = Pc;
	context->GeneralRegisters = ( array<int>^ )Registers->Clone();
	context->Hi = Hi;
	context->Lo = Lo;
	context->LL = LL;
	context->InDelaySlot = InDelaySlot;
	context->DelayPc = DelayPc;
	context->DelayNop = DelayNop;
	context->InterruptState = InterruptState;

	context->Cp0 = Cp0->Context;
	context->Cp1 = Cp1->Context;

	return context;
}

void R4000Core::Context::set( Object^ value )
{
	R4000CoreContext^ context = ( R4000CoreContext^ )value;

	Pc = context->ProgramCounter;
	Registers = ( array<int>^ )context->GeneralRegisters->Clone();
	Hi = context->Hi;
	Lo = context->Lo;
	LL = context->LL;
	InDelaySlot = context->InDelaySlot;
	DelayPc = context->DelayPc;
	DelayNop = context->DelayNop;
	InterruptState = context->InterruptState;

	Cp0->Context = context->Cp0;
	Cp1->Context = context->Cp1;

	// HACK: clear delay slot
	if( InDelaySlot == true )
	{
		InDelaySlot = false;
		Pc = DelayPc - 4;
		DelayPc = 0;
		DelayNop = false;
	}
}

// Weaksauce is the fact that there is no binary constant literals in C :|
#define HEX__(n) 0x##n##LU
#define B8__(x) ((x&0x0000000FLU)?1:0) +((x&0x000000F0LU)?2:0) +((x&0x00000F00LU)?4:0) +((x&0x0000F000LU)?8:0) +((x&0x000F0000LU)?16:0) +((x&0x00F00000LU)?32:0) +((x&0x0F000000LU)?64:0) +((x&0xF0000000LU)?128:0)

#define B8(d) ((unsigned char)B8__(HEX__(d)))
#define B16(dmsb,dlsb) (((unsigned short)B8(dmsb)<<8) + B8(dlsb))
#define B32(dmsb,db2,db3,dlsb) (((unsigned long)B8(dmsb)<<24) + ((unsigned long)B8(db2)<<16) + ((unsigned long)B8(db3)<<8) + B8(dlsb))

#define DELAYWORDS 2
#define reg( r ) Registers[ r ]
#define storereg( r, value ) Registers[ r ] = ( r == 0 ) ? 0 : value;
#define copreg( cop, r ) _cops[ cop ][ r ]
#define storecopreg( cop, r, value ) _cops[ cop ][ r ] = value;
#define copregf( cop, r ) (float)( (R4000Cp1 ^)_cops[ cop ] )->Registers[ r ]
#define storecopregf( cop, r, value ) ( (R4000Cp1 ^)_cops[ cop ] )->Registers[ r ] = value;
#define copcreg( cop, r ) _cops[ cop ]->GetControlRegister( r )
#define storecopcreg( cop, r, value ) _cops[ cop ]->SetControlRegister( r, value )
#define storelink( offset ) Registers[ 31 ] = Pc + offset;
#define getpc() Pc
#define storepc( newpc ) Pc = newpc;
#define getcopconditionbit( cop ) _cops[ cop ]->ConditionBit
#define zeroextend( v ) (int)(unsigned)v
#define signextend8( v ) (int)(char)v
#define signextend16( v ) (int)(short)v
#define loadword( addr ) _memory->ReadWord( this->TranslateAddress( addr ) )
#define storeword( addr, width, value ) _memory->WriteWord( this->TranslateAddress( addr ), width, value )
#define swizzle( v ) ( ( ( v & 0xFF000000 ) >> 24 ) | ( ( v & 0x00FF0000 ) >> 8 ) | ( ( v & 0x0000FF00 ) << 8 ) | ( ( v & 0x000000FF ) << 24 ) )
#define getlo() Lo
#define gethi() Hi
#define storelo( v ) Lo = v;
#define storehi( v ) Hi = v;
#define getll() LL
#define setll( v ) LL = v;
#define getic() InterruptState
#define setic( v ) InterruptState = v;
#define trap( code ) /* do something? */

#define convf( v ) *((float *) &v)
#define convi( v ) *((int *) &v)

#ifdef _DEBUG
#define valid() validInstruction = true;
#else
#define valid() /* nothing */
#endif

// empty statement
#pragma warning(disable:4390)

bool R4000Core::Process( int instruction )
{
	bool continueBlock = true;

	bool targetPcValid = false;
	bool nullifyDelay = false;
	bool incrementPc = true;

#ifdef _DEBUG
	bool validInstruction = false;
#endif

	// NOP
	if( instruction == 0 )
	{
		valid();
		goto postOp;
	}

	//if( getpc() == 0x89004bc )
	//	Debugger::Break();

	// Cleaner code, but slower code
	int op = ( instruction >> 26 ) & 0x3F;
	int rs = ( instruction >> 21 ) & 0x1F;
	int rt = ( instruction >> 16 ) & 0x1F;
	int rd = ( instruction >> 11 ) & 0x1F;
	int sa = ( instruction >> 6 ) & 0x1F;
	int func = instruction & 0x3F;
	unsigned short imm = instruction & 0xFFFF;
	int target = instruction & 0x3FFFFFF;
	int code = ( instruction >> 6 ) & 0xFFFFF;
	
	int targetPc = getpc() + ( signextend16( imm ) << 2 ) + 4;

	// Handle the COP stuff seperate, don't waste time in the switch
	// if we don't need to
	bool isCop = ( op == 0x10 ) || ( op == 0x11 ) || ( op == 0x12 );

	BiosFunction^ function;
	
	int t;
	int u;
	int v;
	int w;
	float f;
	__int64 z;
	unsigned tt;
	unsigned vv;
	unsigned __int64 zz;
	if( isCop == false )
	{
		switch( op )
		{
		case 0:

			switch( func )
			{
			case B8(0001100):	// syscall
				storepc( getpc() + 4 );
				function = _cpu->_syscalls[ code ];
				if( function != nullptr )
				{
					if( function->IsImplemented == true )
					{
						t = function->Target( _memory, reg( 4 ), reg( 5 ), reg( 6 ), reg( 7 ), reg( 29 ) );
						if( function->HasReturn == true )
							storereg( 2, t );
					}
					else
					{
						Debug::WriteLine( String::Format( "R4000Core: NID 0x{0:X8} {1} is not implemented", function->NID, function->Name ) );
						if( function->HasReturn == true )
							storereg( 2, -1 );
					}
				}
				else
				{
					// Unregistered syscall
					Debug::WriteLine( "R4000Core: Unregistered system call attempt" );
				}
				incrementPc = false;
				continueBlock = false;
				valid(); break;
			case B8(0001111):	// sync
				// 629 not needed?
				valid(); break;
			case B8(0001101):	// break code
				// TODO: would switch to exception handler
				// code
				valid(); break;

			case B8(0100000):	// add rd, rs, rt
			case B8(0100001):	// addu
				storereg( rd, reg( rs ) + reg( rt ) );
				valid(); break;
			case B8(0100100):	// and rd, rs, rt
				storereg( rd, reg( rs ) & reg( rt ) );
				valid(); break;
			case B8(0011010):	// div rs, rt
				t = reg( rs );
				v = reg( rt );
				storelo( t / v );
				storehi( t % v );
				valid(); break;
			case B8(0011011):	// divu rs, rt
				tt = reg( rs );
				vv = reg( rt );
				storelo( (int)( tt / vv ) );
				storehi( (int)( tt % vv ) );
				valid(); break;
			case B8(0010000):	// mfhi rd
				storereg( rd, gethi() );
				valid(); break;
			case B8(0010010):	// mflo rd
				storereg( rd, getlo() );
				valid(); break;
			case B8(0010001):	// mthi rs
				storehi( reg( rs ) );
				valid(); break;
			case B8(0010011):	// mtlo rs
				storelo( reg( rs ) );
				valid(); break;
			case B8(0011000):	// mult rs, rt
				z = (__int64)reg( rs ) * (__int64)reg( rt );
				storehi( (int)( z >> 32 ) );
				storelo( (int)( z & 0xFFFFFFFF ) );
				valid(); break;
			case B8(0011001):	// multu rs, rt
				zz = (unsigned __int64)reg( rs ) * (unsigned  __int64)reg( rt );
				storehi( (int)( zz >> 32 ) );
				storelo( (int)( zz & 0xFFFFFFFF ) );
				valid(); break;
			case B8(0100111):	// nor rd, rs, rt
				storereg( rd, ~( reg( rs ) | reg( rt ) ) );
				valid(); break;
			case B8(0100101):	// or rd, rs, rt
				storereg( rd, reg( rs ) | reg( rt ) );
				valid(); break;
			case B8(0000000):	// sll rd, rt, sa
				// this could be nop
				storereg( rd, reg( rt ) << sa );
				valid(); break;
			case B8(0000100):	// sllv rd, rt, rs
				storereg( rd, reg( rt ) << ( reg( rs ) & B8(011111) ) );
				valid(); break;
			case B8(0101010):	// slt rd, rs, rt
				storereg( rd, ( reg( rs ) < reg( rt ) ) ? 1 : 0 );
				valid(); break;
			case B8(0101011):	// sltu rd, rs, rt
				storereg( rd, ( (unsigned)reg( rs ) < (unsigned)reg( rt ) ) ? 1 : 0 );
				valid(); break;
			case B8(0000011):	// sra rd, rt, sa
				storereg( rd, reg( rt ) >> sa );
				valid(); break;
			case B8(0000111):	// srav rd, rt, rs
				storereg( rd, reg( rt ) >> ( reg( rs ) & B8(011111) ) );
				valid(); break;
			case B8(0000010):	// srl rd, rt, sa
				storereg( rd, (int)((unsigned)reg( rt ) >> sa ) );
				valid(); break;
			case B8(0000110):	// srlv rd, rt, rs
				storereg( rd, (int)((unsigned)reg( rt ) >> ( reg( rs ) & B8(011111) ) ) );
				valid(); break;
			case B8(0100010):	// sub rd, rs, rt
			case B8(0100011):	// subu rd, rs, rt
				storereg( rd, reg( rs ) - reg( rt ) );
				valid(); break;
			case B8(0100110):	// xor rd, rs, rt
				storereg( rd, reg( rs ) ^ reg( rt ) );
				valid(); break;

			case B8(0001001):	// jalr rd, rs (rd can be ommitted, defaults to 31)
				storereg( rd, getpc() + DELAYWORDS * 4 );
				targetPc = reg( rs );
				targetPcValid = true;
				valid(); break;
			case B8(0001000):	// jr rs
				targetPc = reg( rs );
				targetPcValid = true;
				valid(); break;

			case B8(0001011):	// movn rd, rs, rt
				if( reg( rt ) != 0 )
					storereg( rd, reg( rs ) );
				valid(); break;
			case B8(0001010):	// movz rd, rs, rt
				if( reg( rt ) == 0 )
					storereg( rd, reg( rs ) );
				valid(); break;

			case B8(0110100):	// teq rs, rt
				if( reg( rs ) == reg( rt ) )
					trap( code & B16(01111111111) );
				valid(); break;
			case B8(0110000):	// tge rs, rt
				if( reg( rs ) >= reg( rt ) )
					trap( code & B16(01111111111) );
				valid(); break;
			case B8(0110001):	// tgeu rs, rt
				if( (unsigned)reg( rs ) >= (unsigned)reg( rt ) )
					trap( code & B16(01111111111) );
				valid(); break;
			case B8(0110010):	// tlt rs, rt
				if( reg( rs ) < reg( rt ) )
					trap( code & B16(01111111111) );
				valid(); break;
			case B8(0110011):	// tltu rs, rt
				if( (unsigned)reg( rs ) < (unsigned)reg( rt ) )
					trap( code & B16(01111111111) );
				valid(); break;
			case B8(0110110):	// tne rs, rt
				if( reg( rs ) != reg( rt ) )
					trap( code & B16(01111111111) );
				valid(); break;
			}
			break;

		case B8(1):

			switch( rt )
			{
			case B8(000000):	// bltz rs, imm
				if( reg( rs ) >> 31 == 1 )
					targetPcValid = true;
				valid(); break;
			case B8(010000):	// bltzal rs, imm
				storelink( DELAYWORDS * 4 );
				if( reg( rs ) >> 31 == 1 )
					targetPcValid = true;
				valid(); break;
			case B8(010010):	// bltzall rs, imm
				storelink( DELAYWORDS * 4 );
				if( reg( rs ) >> 31 == 1 )
					targetPcValid = true;
				else
					nullifyDelay = true;
				valid(); break;
			case B8(000010):	// bltzl rs, imm
				if( reg( rs ) >> 31 == 1 )
					targetPcValid = true;
				else
					nullifyDelay = true;
				valid(); break;
			case B8(000001):	// bgez rs, imm
				if( reg( rs ) >> 31 == 0 )
					targetPcValid = true;
				valid(); break;
			case B8(010001):	// bgezal rs, imm
				storelink( DELAYWORDS * 4 );
				if( reg( rs ) >> 31 == 0 )
					targetPcValid = true;
				valid(); break;
			case B8(010011):	// bgezall rs, imm
				storelink( DELAYWORDS * 4 );
				if( reg( rs ) >> 31 == 0 )
					targetPcValid = true;
				else
					nullifyDelay = true;
				valid(); break;
			case B8(000011):	// bgezl rs, imm
				if( reg( rs ) >> 31 == 0 )
					targetPcValid = true;
				else
					nullifyDelay = true;
				valid(); break;

			case B8(001100):	// teqi rs, imm
				if( reg( rs ) == signextend16( imm ) )
					trap( 0 );
				valid(); break;
			case B8(001000):	// tgei rs, imm
				if( reg( rs ) >= signextend16( imm ) )
					trap( 0 );
				valid(); break;
			case B8(001001):	// tgeiu rs, imm
				if( (unsigned)reg( rs ) >= (unsigned)signextend16( imm ) )
					trap( 0 );
				valid(); break;
			case B8(001010):	// tlti rs, imm
				if( reg( rs ) < signextend16( imm ) )
					trap( 0 );
				valid(); break;
			case B8(001011):	// tltiu rs, imm
				if( (unsigned)reg( rs ) < (unsigned)signextend16( imm ) )
					trap( 0 );
				valid(); break;
			case B8(001110):	// tnei rs, imm
				if( reg( rs ) != signextend16( imm ) )
					trap( 0 );
				valid(); break;
			}

			break;

		case B8(0011100):		// Special allegrex instructions
			switch( func )
			{
			case B8(0):			// halt
				// TODO: halt to wait for interrupt
				valid(); break;
			case B8(0100100):	// mfic rt, rd (rd = 0)
				storereg( rt, getic() );
				valid(); break;
			case B8(0100110):	// mtic rt, rd (rd = 0)
				setic( reg( rt ) );
				valid(); break;
			}
			break;

		case B8(0000010):		// j target
			targetPc = ( getpc() & 0xF0000000 ) | ( target << 2 );
			targetPcValid = true;
			valid(); break;
		case B8(0000011):		// jal target
			storelink( DELAYWORDS * 4 );
			targetPc = ( getpc() & 0xF0000000 ) | ( target << 2 );
			targetPcValid = true;
			valid(); break;
		
		case B8(0000100):		// beq rs, rt, imm
			if( reg( rs ) == reg( rt ) )
				targetPcValid = true;
			valid(); break;
		case B8(0010100):		// beql rs, rt, imm
			if( reg( rs ) == reg( rt ) )
				targetPcValid = true;
			else
				nullifyDelay = true;
			valid(); break;
		case B8(0000111):		// bgtz rs, imm
			t = reg( rs );
			if( ( ( t >> 31 ) == 0 ) &&
				( t != 0 ) )
				targetPcValid = true;
			valid(); break;
		case B8(0010111):		// bgtzl rs, imm
			t = reg( rs );
			if( ( ( t >> 31 ) == 0 ) &&
				( t != 0 ) )
				targetPcValid = true;
			else
				nullifyDelay = true;
			valid(); break;
		case B8(0000110):		// blez rs, imm
			t = reg( rs );
			if( ( ( ( t >> 31 ) & 0x1 ) == 1 ) ||
				( t == 0 ) )
				targetPcValid = true;
			valid(); break;
		case B8(0010110):		// blezl rs, imm
			t = reg( rs );
			if( ( ( ( t >> 31 ) & 0x1 ) == 1 ) ||
				( t == 0 ) )
				targetPcValid = true;
			else
				nullifyDelay = true;
			valid(); break;
		case B8(0000101):		// bne rs, rt, imm
			if( reg( rs ) != reg( rt ) )
				targetPcValid = true;
			valid(); break;
		case B8(0010101):		// bnel rs, rt, imm
			if( reg( rs ) != reg( rt ) )
				targetPcValid = true;
			else
				nullifyDelay = true;
			valid(); break;

		case B8(0001000):		// addi rt, rs, imm
		case B8(0001001):		// addiu
			storereg( rt, reg( rs ) + signextend16( imm ) );
			valid(); break;
		case B8(0001100):		// andi rt, rs, imm
			storereg( rt, reg( rs ) & zeroextend( imm ) );
			valid(); break;
		case B8(0001101):		// ori rt, rs, imm
			storereg( rt, reg( rs ) | zeroextend( imm ) );
			valid(); break;
		case B8(0001010):		// slti rt, rs, imm
			storereg( rt, ( reg( rs ) < signextend16( imm ) ) ? 1 : 0 );
			valid(); break;
		case B8(0001011):		// sltiu rt, rs, imm
			storereg( rt, ( (unsigned)reg( rs ) < (unsigned)signextend16( imm ) ) ? 1 : 0 );
			valid(); break;
		case B8(0001110):		// xori rt, rs, imm
			storereg( rt, reg( rs ) ^ zeroextend( imm ) );
			valid(); break;

		case B8(0100000):		// lb rt, offset(base)
			t = signextend16( imm ) + reg( rs );
			v = loadword( t ) & 0xFF;
			storereg( rt, signextend8( v ) );
			valid(); break;
		case B8(0100100):		// lbu rt, offset(base)
			t = signextend16( imm ) + reg( rs );
			v = loadword( t ) & 0xFF;
			storereg( rt, v );
			valid(); break;
		case B8(0100001):		// lh rt, offset(base)
			t = signextend16( imm ) + reg( rs );
			v = loadword( t ) & 0xFFFF;
			storereg( rt, signextend16( v ) );
			valid(); break;
		case B8(0100101):		// lhu rt, offset(base)
			t = signextend16( imm ) + reg( rs );
			v = loadword( t ) & 0xFFFF;
			storereg( rt, v );
			valid(); break;
		case B8(0001111):		// lui rt, imm
			storereg( rt, imm << 16 );
			valid(); break;
		case B8(0100011):		// lw rt, offset(base)
			t = signextend16( imm ) + reg( rs );
			v = loadword( t );
			storereg( rt, v );
			valid(); break;
		case B8(0100010):		// lwl rt, offset(base)
			t = signextend16( imm ) + reg( rs );
			v = loadword( t );
			t %= 4;
			v <<= ( 8 * ( 3 - t ) );
			u = 0xFFFFFFFF >> ( 8 * ( t + 1 ) );
			t = reg( rt ) & u;
			storereg( rt, t | v );
			// TODO: ensure lwl right
			valid(); break;
		case B8(0100110):		// lwr rt, offset(base)
			t = signextend16( imm ) + reg( rs );
			v = loadword( t );
			t %= 4;
			v = swizzle( v );
			v >>= ( 8 * t );
			u = 0xFFFFFFFF << ( 8 * ( 4 - t ) );
			t = reg( rt ) & u;
			storereg( rt, t | v );
			// TODO: ensure lwr right
			valid(); break;
		case B8(0101000):		// sb rt, offset(base)
			t = signextend16( imm ) + reg( rs );
			v = reg( rt );
			storeword( t, 1, v & 0xFF );
			valid(); break;
		case B8(0101001):		// sh rt, offset(base)
			t = signextend16( imm ) + reg( rs );
			v = reg( rt );
			storeword( t, 2, v & 0xFFFF );
			valid(); break;
		case B8(0101011):		// sw rt, offset(base)
			t = signextend16( imm ) + reg( rs );
			v = reg( rt );
			storeword( t, 4, v );
			valid(); break;
		case B8(0101010):		// swl rt, offset(base)
			w = t = signextend16( imm ) + reg( rs );
			v = reg( rt );
			t %= 4;
			v <<= ( 8 * ( 3 - t ) );
			u = 0xFFFFFFFF >> ( 8 * ( t + 1 ) );
			t = loadword( t & 0xFFFFFFFC ) & u;
			storeword( w, 4, t | v );
			// TODO: ensure swl right
			valid(); break;
		case B8(0101110):		// swr rt, offset(base)
			w = t = signextend16( imm ) + reg( rs );
			v = reg( rt );
			t %= 4;
			v = swizzle( v );
			v >>= ( 8 * t );
			u = 0xFFFFFFFF << ( 8 * ( 4 - t ) );
			t = loadword( t & 0xFFFFFFFC ) & u;
			storeword( w, 4, t | v );
			// TODO: ensure swr right
			valid(); break;
		case B8(0110001):				// lwcz rt, offset(base)
			t = signextend16( imm ) + reg( rs );
			v = loadword( t );
			f = convf( v );
			storecopregf( 1, rt, f );
			// TODO: LWCz 568
			valid(); break;
		case B8(0111001):				// swcz rt, offset(base)
			t = signextend16( imm ) + reg( rs );
			f = copregf( 1, rt );
			storeword( t, 4, convi( f ) );
			// TODO: SWCz 621
			valid(); break;

		case B8(0110000):		// ll rt, offset(base)   562
			t = signextend16( imm ) + reg( rs );
			v = loadword( t );
			storereg( rt, v );
			setll( 1 );
			valid(); break;
		case B8(0111000):		// sc rt, offset(base)   599
			t = signextend16( imm ) + reg( rs );
			if( getll() == true )
			{
				storeword( t, 4, reg( rt ) );
				storereg( rt, 1 );
			}
			else
				storereg( rt, 0 );
			valid(); break;

			// LDCz 553 ?
			// SDCz 599 ?

			// Ignored instructions (but still valid)
		case B8(0101111):		// cache
			valid(); break;

		default:
			break;
		}
	}
	else
	{
		int copop = instruction >> 28;
		int cop = op & B8(011); // cop0, cop1, or cop2
		// rs = bc sub-opcode
		// rt = branch condition

		switch( copop )
		{
		case B8(00100):

			//if( ( ( instruction >> 25 ) & 0x1 ) == 1 )	// COPz
			//{
			//	int cofun = instruction & 0x1FFFFFF;
				// TODO: cop operation (appendix b)
				// eret - 544
			//}
			//else
			{
				bool condition = getcopconditionbit( cop );
				bool handled = false;
				
				switch( rs )
				{
				case B8(001000):
					switch( rt )
					{
					case B8(000000):		// BCzF
						targetPcValid = ( condition == false );
						handled = true;
						valid(); break;
					case B8(000010):		// BCzFL
						targetPcValid = ( condition == false );
						nullifyDelay = !targetPcValid;
						handled = true;
						valid(); break;
					case B8(000001):		// BCzT
						targetPcValid = condition;
						handled = true;
						valid(); break;
					case B8(000011):		// BCzTL
						targetPcValid = condition;
						nullifyDelay = !targetPcValid;
						handled = true;
						valid(); break;
					}
					break;
				case B8(000000):			// mfcz rt, rd
					storereg( rt, copreg( cop, rd ) );
					valid(); break;
				case B8(000100):			// mtcz rt, rd
					storecopreg( cop, rd, reg( rt ) );
					valid(); break;
				case B8(000010):			// cfcz rt, rd
					storereg( rt, copcreg( cop, rd ) );
					valid(); break;
				case B8(000110):			// ctcz rt, rd
					storecopcreg( cop, rd, reg( rt ) );
					valid(); break;
				}

				if( handled == false )
				{
					if( cop == 1 )
					{
						// Hand off to COP1 to do it's thing
						bool instrValid = _cp1->Process( instruction );
#ifdef _DEBUG
						if( instrValid == true )
							valid();
#endif
					}
				}
			}
			break;
		}
	}

postOp:

#ifdef _DEBUG
	Debug::Assert( validInstruction == true );
#endif

	if( targetPcValid == true )
	{
		// TODO: branch using targetPc
		storepc( getpc() + 4 );
		DelayPc = targetPc;
		InDelaySlot = true;
	}
	else
	{
		if( InDelaySlot == true )
		{
			storepc( DelayPc );
		}
		else
		{
			if( incrementPc == true )
				storepc( getpc() + 4 );
		}
		DelayPc = 0;
		InDelaySlot = false;
	}

	if( nullifyDelay == true )
		DelayNop = true;

	return continueBlock;
}

#pragma warning(default:4390)

int R4000Core::TranslateAddress( int address )
{
	//if( ( address & 0x80000000 ) != 0 )
	//{
	//	// Kernel.... hack!
	//	address = address & 0x7FFFFFFF;
	//}
	//if( ( address & 0x40000000 ) != 0 )
	//{
	//	// Kernel.... hack!
	//	address = address & 0x3FFFFFFF;
	//}
	return address & 0x3FFFFFFF;
}
