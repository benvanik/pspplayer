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
using namespace Noxa::Emulation::Psp::Bios;
using namespace Noxa::Emulation::Psp::Cpu;
using namespace SoftWire;

#define g context->Generator

GenerationResult SYSCALL( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
{
	int syscall = ( int )( ( code >> 6 ) & 0xFFFFF );

	BiosFunction^ biosFunction = R4000Cpu::GlobalCpu->_syscalls[ syscall ];
	bool willCall;
	bool hasReturn;
	int paramCount;
	if( biosFunction != nullptr )
	{
		willCall = biosFunction->IsImplemented;
		hasReturn = biosFunction->HasReturn;
		paramCount = biosFunction->ParameterCount;

		if( biosFunction->IsImplemented == false )
		{
			if( pass == 0 )
			{
				Debug::WriteLine( String::Format( "R4000Generator: NID 0x{0:X8} {1} is not implemented",
					biosFunction->NID, biosFunction->Name ) );
			}
		}
	}
	else
	{
		willCall = false;
		hasReturn = false;
		paramCount = 0;

		if( pass == 0 )
			Debug::WriteLine( "R4000Generator: unregistered syscall attempt" );
	}
/*
	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
		// It's important that we save what we think is the current PC
		// If we had an UpdatePc, it means a branch has updated it before us
		// and we need to save it - otherwise, save the PC following us
		context.ILGen.Emit( OpCodes.Ldarg_0 );
		if( context.UpdatePc == true )
			context.ILGen.Emit( OpCodes.Ldloc_2 );
		else
			context.ILGen.Emit( OpCodes.Ldc_I4, address + 4 );
		context.ILGen.Emit( OpCodes.Stfld, context.Core0Pc );

		if( willCall == true )
		{
			// Lame, but we need the object this gets called on and
			// there is no way to communicate what we know now to the final IL
			//context.Cpu._syscalls[ syscall ].Target.Target;
			context.ILGen.Emit( OpCodes.Ldarg_3 );
			context.ILGen.Emit( OpCodes.Ldc_I4, syscall );
			context.ILGen.Emit( OpCodes.Ldelem, typeof( BiosFunction ) );
			context.ILGen.Emit( OpCodes.Ldfld, context.BiosFunctionTarget );
			context.ILGen.Emit( OpCodes.Call, context.DelegateTargetGet );
			
			// Memory
			context.ILGen.Emit( OpCodes.Ldarg_1 );

			if( paramCount > 0 )
			{
				EmitLoadRegister( context, 4 );
				if( paramCount > 1 )
				{
					EmitLoadRegister( context, 5 );
					if( paramCount > 2 )
					{
						EmitLoadRegister( context, 6 );
						if( paramCount > 3 )
						{
							EmitLoadRegister( context, 7 );
							if( paramCount > 4 )
							{
								// Maybe this should always go?
								EmitLoadRegister( context, 29 );
							}
							else
								context.ILGen.Emit( OpCodes.Ldc_I4_0 );
						}
						else
						{
							context.ILGen.Emit( OpCodes.Ldc_I4_0 );
							context.ILGen.Emit( OpCodes.Ldc_I4_0 );
						}
					}
					else
					{
						context.ILGen.Emit( OpCodes.Ldc_I4_0 );
						context.ILGen.Emit( OpCodes.Ldc_I4_0 );
						context.ILGen.Emit( OpCodes.Ldc_I4_0 );
					}
				}
				else
				{
					context.ILGen.Emit( OpCodes.Ldc_I4_0 );
					context.ILGen.Emit( OpCodes.Ldc_I4_0 );
					context.ILGen.Emit( OpCodes.Ldc_I4_0 );
					context.ILGen.Emit( OpCodes.Ldc_I4_0 );
				}
			}
			else
			{
				context.ILGen.Emit( OpCodes.Ldc_I4_0 );
				context.ILGen.Emit( OpCodes.Ldc_I4_0 );
				context.ILGen.Emit( OpCodes.Ldc_I4_0 );
				context.ILGen.Emit( OpCodes.Ldc_I4_0 );
				context.ILGen.Emit( OpCodes.Ldc_I4_0 );
			}

			if( biosFunction.Target.Method.IsFinal == true )
				context.ILGen.Emit( OpCodes.Call, biosFunction.Target.Method );
			else
				context.ILGen.Emit( OpCodes.Callvirt, biosFunction.Target.Method );

			// Function returns a value - may need to ignore
			if( hasReturn == true )
				EmitStoreRegister( context, 2 );
			else
				context.ILGen.Emit( OpCodes.Pop );
		}
		else
		{
			// When we fail, we need to make sure to handle the cases where
			// the method has a return or else things could get even worse!
			if( biosFunction != null )
			{
				if( hasReturn == true )
				{
					context.ILGen.Emit( OpCodes.Ldc_I4, -1 );
					EmitStoreRegister( context, 2 );
				}
			}
		}
	}*/
	return GenerationResult::Syscall;
}

GenerationResult BREAK( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
{
	if( pass == 0 )
	{
		Debug::WriteLine( "R4000Generator: BREAK not implemented" );
	}
	else if( pass == 1 )
	{
	}
	return GenerationResult::Success;
}

GenerationResult SYNC( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
{
	// pg 629 - not needed?
	if( pass == 0 )
	{
		Debug::WriteLine( "R4000Generator: SYNC not implemented" );
	}
	else if( pass == 1 )
	{
	}
	return GenerationResult::Success;
}

GenerationResult COP1( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
{
	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
	}
	return GenerationResult::Invalid;
}

GenerationResult COP2( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
{
	if( pass == 0 )
	{
	}
	else if( pass == 1 )
	{
	}
	return GenerationResult::Invalid;
}

GenerationResult HALT( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
{
	if( pass == 0 )
	{
		Debug::WriteLine( "R4000Generator: HALT not implemented" );
	}
	else if( pass == 1 )
	{
	}
	return GenerationResult::Success;
}

GenerationResult MFIC( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
{
	if( pass == 0 )
	{
		Debug::WriteLine( "R4000Generator: MFIC not implemented" );
	}
	else if( pass == 1 )
	{
	}
	return GenerationResult::Success;
}

GenerationResult MTIC( R4000GenContext^ context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
{
	if( pass == 0 )
	{
		Debug::WriteLine( "R4000Generator: MTIC not implemented" );
	}
	else if( pass == 1 )
	{
	}
	return GenerationResult::Success;
}
