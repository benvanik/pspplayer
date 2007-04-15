// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "StdAfx.h"
#include "R4000Cpu.h"
#include "R4000Core.h"
#include "R4000Memory.h"
#include "Tracer.h"

#include "R4000Ctx.h"
#include "R4000Generator.h"
#include "R4000BiosStubs.h"

using namespace System::Diagnostics;
using namespace System::Reflection;
using namespace System::Reflection::Emit;
using namespace System::Text;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Cpu;

// If defined, all syscalls that don't return anything will still set $v0
//#define SAFESYSCALLRETURN

// This is some helper stuff so that the shim can get a module instance when it needs it quickly.
// EmitShim will check the list, and if it can't find the module it will add it. Since the shim
// emitting happens at startup and is a one time cost, I don't care if lookup is slow there -
// there are also only a few modules (under 30), so O(n) is livable.
int R4000Cpu::LookupOrAddModule( IModule^ module )
{
	int n;
	for( n = 0; n < _moduleInstances->Length; n++ )
	{
		if( _moduleInstances[ n ] == nullptr )
			break;
		if( _moduleInstances[ n ] == module )
			return n;
	}

	Debug::Assert( _moduleInstances[ n ] == nullptr );

	_moduleInstances[ n ] = module;
	return n;
}

// The shim is a small call that loads the required number of arguments, calls the
// method, and stores back the return value.
// Since at shim generation time we know the fixed memory and register (in ctx) addresses
// we can even put those right in to the generated code, meaning the shim need not take
// any arguments.
// We still support passing IMemory to the bios functions - as long as it is defined
// as the first parameter will we pass it through.

// Here we are in the x86 dynarec CPU emitting MSIL. I'm craaazzy ----___----
BiosShim^ R4000Cpu::EmitShim( BiosFunction^ function, MemorySystem^ memory, void* registers )
{
	//Type^ voidStar = ( void::typeid )->MakePointerType();
	array<Type^>^ shimArgs = { R4000Cpu::typeid };
	MethodInfo^ mi = function->MethodInfo;

	DynamicMethod^ shim = gcnew DynamicMethod( String::Format( "Shim{0:X8}", function->NID ),
		void::typeid, shimArgs,
		BiosShim::typeid->Module );
	ILGenerator^ ilgen = shim->GetILGenerator();

	bool hasReturn = ( mi->ReturnType != void::typeid );
	bool wideReturn = ( mi->ReturnType == Int64::typeid );

	// TRICK: if we have a return, we'll need a local to store the return from the
	// function while we build the address (cause order is address, value for stind).
	// BUT: if we push the return address stuff here we can just stind the value
	// after the function call! We saved a local! Woo!
	if( hasReturn == true )
	{
		// Load $v0 (+ $v1 if it is a 64 bit return)
		ilgen->Emit( OpCodes::Ldc_I4, ( int )registers + ( 2 << 2 ) );	// load registers[ v0 = $2 ]
		ilgen->Emit( OpCodes::Conv_I );

#if 0
		if( wideReturn == true )
		{
			// 64 bit return - lower in v0 ($2), upper in v1 ($3)

			// Local for storage - we only need 32 bits
			ilgen->DeclareLocal( int::typeid );

			ilgen->Emit( OpCodes::Ldc_I4, ( int )registers + ( 3 << 2 ) );	// load registers[ v1 = $3 ]
			ilgen->Emit( OpCodes::Conv_I );

			// stack now contains address of $v0, $v1
		}
		else
		{
			// 32 bit return - in v0 ($2)
			
			// stack now contains address of $v0
		}
#endif
	}

	// Push module instance (needed for Call below)
	// We do this by loading the module instance from the array on the cpu (arg0)
	int moduleIndex = this->LookupOrAddModule( function->ModuleInstance );
	ilgen->Emit( OpCodes::Ldarg_0 );
	ilgen->Emit( OpCodes::Ldfld, _privateModuleInstancesFieldInfo );
	ilgen->Emit( OpCodes::Ldc_I4, moduleIndex );
	ilgen->Emit( OpCodes::Ldelem_Ref );

	// Handle IMemory usage - this is always the first parameter
	//if( function->UsesMemorySystem == true )
	//{
	//	// Perform a cpu->_memory load (cpu in arg0)
	//	ilgen->Emit( OpCodes::Ldarg_0 );
	//	ilgen->Emit( OpCodes::Ldfld, _privateMemoryFieldInfo );
	//}

	// Push on remaining parameters
	// The first 8 arguments go in $a0 to $t3 ($4 to $12) - rest go on the stack.
	// We have to handle the case of doublewords here - they need to be dword aligned,
	// which means that we may skip a register to do that. For ex, if we have
	// int Foo( int arg1, int64 arg2, int arg3 )
	// the register setup would be:
	//   $a0 = arg1
	//   $a1 = (undefined)
	//   $a2 = arg2 lower word
	//   $a3 = arg2 upper word
	//   $t0 = arg3
	// Not sure what happens if it is on the border with the stack vars - we just assert now.
	// The logic below is that we loop over all params, and have a separate regOffset which is
	// the real register being used. When loading a register, regOffset should be used; when
	// getting parameters, n should be used.
	int regOffset = 0;
	for( int n = 0; n < function->ParameterCount; n++ )
	{
		if( regOffset < 8 )
		{
			bool isWide = function->ParameterWidths[ n ];
			if( isWide == true )
			{
				if( regOffset % 2 == 1 )
				{
					// Unaligned dword argument - increment regOffset to skip over a register!
					regOffset++;
				}
			}

			int registerAddress = ( int )registers + ( ( regOffset + 4 ) << 2 );
			ilgen->Emit( OpCodes::Ldc_I4, registerAddress );	// load register address
			ilgen->Emit( OpCodes::Conv_I );

			if( isWide == false )
			{
				// 32 bit load
				ilgen->Emit( OpCodes::Ldind_I4 );			// load value
				regOffset++;
			}
			else
			{
				// 64 bit load
				ilgen->Emit( OpCodes::Ldind_I8 );
				regOffset += 2;
			}
		}
		else
		{
			// Emit sp + 0/4/8/12
			// value = memory[ ( reg( sp ) - 0x08000000 ) + offset ]

			// Don't currently support dwords - we could easily by just writing
			// twice and +2 to regOffset, but don't cause I won't do it unless
			// I have to
			Debug::Assert( function->ParameterWidths[ n ] == false );

			int spAddress = ( int )registers + ( 29 << 2 );	// sp = $29...
			int spOffset = ( regOffset - 8 ) << 2;			// Offset from sp in words

			ilgen->Emit( OpCodes::Ldc_I4, spAddress );		// load sp address
			ilgen->Emit( OpCodes::Conv_I );
			ilgen->Emit( OpCodes::Ldind_I4 );				// load $sp
			ilgen->Emit( OpCodes::Ldc_I4, MainMemoryBase );	// 0x08000000
			ilgen->Emit( OpCodes::Conv_I );
			ilgen->Emit( OpCodes::Sub );					// addr = $sp - 0x08000000
			ilgen->Emit( OpCodes::Ldc_I4, spOffset );		// offset + 0..4 words
			ilgen->Emit( OpCodes::Conv_I );
			ilgen->Emit( OpCodes::Add );					// addr = ( $sp - 0x08000000 ) + offset
			ilgen->Emit( OpCodes::Ldc_I4, ( int )( memory->MainMemory ) );	// load memory
			ilgen->Emit( OpCodes::Conv_I );
			ilgen->Emit( OpCodes::Add );					// finaladdr = addr + memory ptr
			ilgen->Emit( OpCodes::Ldind_I4 );				// load value

			regOffset++;
		}
	}

	// Invoke method
	ilgen->Emit( OpCodes::Call, function->MethodInfo );

	// Handle return
	if( hasReturn == true )
	{
		if( wideReturn == false )
			ilgen->Emit( OpCodes::Stind_I4 );				// store to $v0
		else
			ilgen->Emit( OpCodes::Stind_I8 );				// store to $v0 + $v1
#if 0
		if( wideReturn == true )
		{
			MethodInfo^ breakInfo = ( Debugger::typeid )->GetMethod( "Break" );
			Debug::Assert( breakInfo != nullptr );
			ilgen->Emit( OpCodes::Call, breakInfo );

			// 64 bit return - lower in v0 ($2), upper in v1 ($3)
			// stack contains v0 addr, v1 addr, long value

			// Put ret in local - we truncate to 32 bits so we don't have to do it later
			ilgen->Emit( OpCodes::Dup );
			ilgen->Emit( OpCodes::Conv_I4 );				// truncates upper
			ilgen->Emit( OpCodes::Stloc_0 );

			// We need to store $v1 (upper word) - we still have the long on the stack
			ilgen->Emit( OpCodes::Ldc_I4, 32 );
			ilgen->Emit( OpCodes::Shr_Un );					// =>> 32
			ilgen->Emit( OpCodes::Conv_I4 );				// truncates upper
			ilgen->Emit( OpCodes::Stind_I4 );				// store in $v1

			// Load back local for $v0 code below
			ilgen->Emit( OpCodes::Ldloc_0 );
		}
		else
		{
			// 32 bit return - in v0 ($2)
			// stack contains v0 addr, int value
		}

		// Shared code for $v0
		ilgen->Emit( OpCodes::Stind_I4 );					// store in $v0
#endif
	}
	else
	{
#ifdef SAFESYSCALLRETURN
		// Always set v0 ($2) to 0, because a lot of our stubs may say they have no return but really do
		ilgen->Emit( OpCodes::Ldc_I4, ( int )registers );	// load registers
		ilgen->Emit( OpCodes::Conv_I );
		ilgen->Emit( OpCodes::Ldc_I4, 2 << 2 );				// v0 = $2
		ilgen->Emit( OpCodes::Conv_I );
		ilgen->Emit( OpCodes::Add );						// register base + register offset
		ilgen->Emit( OpCodes::Ldc_I4_0 );
		ilgen->Emit( OpCodes::Stind_I4 );					// store 0 in $v0
#endif
	}

	ilgen->Emit( OpCodes::Ret );

	BiosShim^ del = ( BiosShim^ )shim->CreateDelegate( BiosShim::typeid );
	Debug::Assert( del != nullptr );

	return del;
}

void* R4000Cpu::EmitShimN( BiosFunction^ function, NativeMemorySystem* memory, void* registers )
{
	R4000Generator* g = _context->Generator;

	MethodInfo^ mi = function->MethodInfo;
	bool hasReturn = ( mi->ReturnType != void::typeid );
	bool wideReturn = ( mi->ReturnType == Int64::typeid );

	g->push( EBP );
	g->mov( EBP, ESP );

	// Push on parameters (with double words aligned on dword boundaries)
	// Since the logic is pretty trivial going forward, and harder to see going back,
	// I just do it forward and then reverse it :)
	Debug::Assert( function->ParameterCount <= 8 );
	int regAddresses[ 8 ];
	memset( regAddresses, 0, sizeof( int ) * 8 );
	int regOffset = 0;
	int x86regOffset = 0;
	for( int n = 0; n < function->ParameterCount; n++ )
	{
		if( regOffset < 8 )
		{
			bool isWide = function->ParameterWidths[ n ];
			if( isWide == true )
			{
				if( regOffset % 2 == 1 )
				{
					// Unaligned dword argument - increment regOffset to skip over a register!
					regOffset++;
				}
			}

			int registerAddress = ( int )registers + ( ( regOffset + 4 ) << 2 );
			regAddresses[ x86regOffset ] = registerAddress;

			if( isWide == false )
			{
				// 32 bit load
				regOffset++;
				x86regOffset++;
			}
			else
			{
				// 64 bit load
				regAddresses[ x86regOffset + 1 ] = registerAddress + 4;
				regOffset += 2;
				x86regOffset += 2;
			}
		}
		else
		{
			// Emit sp + 0/4/8/12
			// value = memory[ ( reg( sp ) - 0x08000000 ) + offset ]
			Debug::Assert( false, "Memory spilling not supported in native shims!" );
			throw gcnew NotImplementedException();

			// Don't currently support dwords - we could easily by just writing
			// twice and +2 to regOffset, but don't cause I won't do it unless
			// I have to
			//Debug::Assert( function->ParameterWidths[ n ] == false );

			//int spAddress = ( int )registers + ( 29 << 2 );	// sp = $29...
			//int spOffset = ( regOffset - 8 ) << 2;			// Offset from sp in words

			//ilgen->Emit( OpCodes::Ldc_I4, spAddress );		// load sp address
			//ilgen->Emit( OpCodes::Conv_I );
			//ilgen->Emit( OpCodes::Ldind_I4 );				// load $sp
			//ilgen->Emit( OpCodes::Ldc_I4, MainMemoryBase );	// 0x08000000
			//ilgen->Emit( OpCodes::Conv_I );
			//ilgen->Emit( OpCodes::Sub );					// addr = $sp - 0x08000000
			//ilgen->Emit( OpCodes::Ldc_I4, spOffset );		// offset + 0..4 words
			//ilgen->Emit( OpCodes::Conv_I );
			//ilgen->Emit( OpCodes::Add );					// addr = ( $sp - 0x08000000 ) + offset
			//ilgen->Emit( OpCodes::Ldc_I4, ( int )memory );	// load memory
			//ilgen->Emit( OpCodes::Conv_I );
			//ilgen->Emit( OpCodes::Add );					// finaladdr = addr + memory ptr
			//ilgen->Emit( OpCodes::Ldind_I4 );				// load value

			//regOffset++;
		}
	}

	// Really push parameters on in reverse order
	for( int n = x86regOffset - 1; n >= 0; n-- )
	{
		if( regAddresses[ n ] != 0 )
			g->push( g->dword_ptr[ regAddresses[ n ] ] );
	}

	// Handle memory usage - this is always the first parameter
	if( function->UsesMemorySystem == true )
		g->push( ( uint )memory );

	// Invoke method
#pragma warning( disable: 4395 )
	g->call( ( uint )function->NativeMethod.ToPointer() );

	g->add( ESP, ( x86regOffset - 1 ) * 4 );

	// Handle return
	if( hasReturn == true )
	{
		int v0address = ( int )( ( byte* )registers + ( 2 << 2 ) );
		g->mov( g->dword_ptr[ v0address ], EAX );				// store to $v0
		if( wideReturn == true )
			g->mov( g->dword_ptr[ v0address + 4 ], EDX );		// store upper to $v1
	}
	else
	{
#ifdef SAFESYSCALLRETURN
		// Always set v0 ($2) to 0, because a lot of our stubs may say they have no return but really do
		int v0address = ( int )( ( byte* )registers + ( 2 << 2 ) );
		g->mov( g->dword_ptr[ v0address ], 0 );
#endif
	}

	g->mov( ESP, EBP );
	g->pop( EBP );

	// This assumes caller address on top of the stack, which it should be
	g->ret();

	FunctionPointer ptr = g->GenerateCode();
	g->Reset();

	return ptr;
}
