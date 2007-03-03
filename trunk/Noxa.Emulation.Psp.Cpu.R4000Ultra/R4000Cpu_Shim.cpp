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
BiosShim^ R4000Cpu::EmitShim( BiosFunction^ function, void* memory, void* registers )
{
	Type^ voidStar = ( void::typeid )->MakePointerType();
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
	// BUT: this only works for int32 returns. With 64 we need to store the value around.
	// MAYBE: we could use stind_i8?
	if( hasReturn == true )
	{
		// Shared code for $v0
		ilgen->Emit( OpCodes::Ldc_I4, ( int )registers );		// load registers
		ilgen->Emit( OpCodes::Conv_I );
		ilgen->Emit( OpCodes::Ldc_I4, 2 << 2 );					// v0 = $2
		ilgen->Emit( OpCodes::Conv_I );
		ilgen->Emit( OpCodes::Add );							// register base + register offset

		if( wideReturn == true )
		{
			// 64 bit return - lower in v0 ($2), upper in v1 ($3)

			// Local for storage - we only need 32 bits
			ilgen->DeclareLocal( int::typeid );

			ilgen->Emit( OpCodes::Ldc_I4, ( int )registers );	// load registers
			ilgen->Emit( OpCodes::Conv_I );
			ilgen->Emit( OpCodes::Ldc_I4, 3 << 2 );				// v1 = $3
			ilgen->Emit( OpCodes::Conv_I );
			ilgen->Emit( OpCodes::Add );						// register base + register offset

			// stack now contains address of $v0, $v1
		}
		else
		{
			// 32 bit return - in v0 ($2)
			
			// stack now contains address of $v0
		}
	}

	// Push module instance (needed for Call below)
	// We do this by loading the module instance from the array on the cpu (arg0)
	int moduleIndex = this->LookupOrAddModule( function->Module );
	ilgen->Emit( OpCodes::Ldarg_0 );
	ilgen->Emit( OpCodes::Ldfld, _privateModuleInstancesFieldInfo );
	ilgen->Emit( OpCodes::Ldc_I4, moduleIndex );
	ilgen->Emit( OpCodes::Ldelem_Ref );

	// Handle IMemory usage - this is always the first parameter
	if( function->UsesMemorySystem == true )
	{
		// Perform a cpu->_memory load (cpu in arg0)
		ilgen->Emit( OpCodes::Ldarg_0 );
		ilgen->Emit( OpCodes::Ldfld, _privateMemoryFieldInfo );
	}

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
			ilgen->Emit( OpCodes::Ldind_I4 );					// load value

			regOffset++;
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
			ilgen->Emit( OpCodes::Ldc_I4, ( int )memory );	// load memory
			ilgen->Emit( OpCodes::Conv_I );
			ilgen->Emit( OpCodes::Add );					// finaladdr = addr + memory ptr
			ilgen->Emit( OpCodes::Ldind_I4 );				// load value

			regOffset++;
		}
	}

	// Invoke method
	ilgen->EmitCall( OpCodes::Call, function->MethodInfo, nullptr );

	// Handle return
	if( hasReturn == true )
	{
		if( wideReturn == true )
		{
			// 64 bit return - lower in v0 ($2), upper in v1 ($3)
			// stack contains long

			// Put ret in local - we truncate to 32 bits so we don't have to do it later
			ilgen->Emit( OpCodes::Dup );
			ilgen->Emit( OpCodes::Conv_I4 );
			ilgen->Emit( OpCodes::Stloc_0 );

			// We need to store $v1 (upper word) - we still have the long on the stack
			ilgen->Emit( OpCodes::Ldc_I4, 32 );
			ilgen->Emit( OpCodes::Shr_Un );					// =>> 32
			ilgen->Emit( OpCodes::Conv_I4 );
			ilgen->Emit( OpCodes::Stind_I4 );				// store in $v1

			// Load back local for $v0 code below
			ilgen->Emit( OpCodes::Ldloc_0 );
		}
		else
		{
			// 32 bit return - in v0 ($2)
			// stack contains int
		}

		// Shared code for $v0
		ilgen->Emit( OpCodes::Stind_I4 );					// store in $v0
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
