// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

using Noxa.Emulation.Psp.Cpu;
using Noxa.Emulation.Psp.Debugging.DebugData;
using Noxa.Emulation.Psp.Debugging.DebugModel;

namespace Noxa.Emulation.Psp.Bios.ManagedHLE
{
	unsafe partial class Loader
	{
		private void Analyze( IDebugDatabase db, byte* text, uint textLength, uint baseAddress )
		{
			// We start at the base and try to build functions
			// The logic here is similar to my dynarec

			uint methodStart = baseAddress;
			bool inDelay = false;
			uint lastBranchTarget = 0;
			InstructionType lastJumpType = InstructionType.Normal;
			bool lastJumpRegister = false;
			//bool hasSpMod = false;

			uint address = baseAddress;
			uint* pcode = ( uint* )text;
			for( int n = 0; n < textLength / 4; n++, pcode++, address += 4 )
			{
				// addiu $sp, ... ($sp = 29)
				// 29 << 16 = 1D0000
				//bool isSpMod =
				//    ( ( *pcode & 0xFC1F0000 ) == 0x241D0000 ) ||
				//    ( ( *pcode & 0xFC1F0000 ) == 0x201D0000 );
				//if( isSpMod == true )
				//    hasSpMod = true;

				bool lastDelay = inDelay;
				inDelay = false;
				if( lastDelay == true )
				{
					// A function may end here if there are no forward branches
					if( lastJumpType == InstructionType.Branch )
						continue;
					// We don't (usually) end on JAL
					if( lastJumpType == InstructionType.JumpAndLink )
						continue;
					if( lastBranchTarget > address )
						continue;
					//if( ( isSpMod == true ) &&
					//    ( lastJumpType == InstructionType.Jump ) &&
					//    ( lastJumpRegister == false ) )
					//{
					//}

					// End method
					Method method = new Method( MethodType.User, methodStart, address - methodStart );
					db.AddSymbol( method );
					methodStart = address + 4;
					lastBranchTarget = 0;
					//hasSpMod = false;
					continue;
				}

				uint target;
				InstructionType type = AnalyzeJump( address, *pcode, out target );
				switch( type )
				{
					case InstructionType.Branch:
						if( ( target != uint.MaxValue ) &&
							( target > lastBranchTarget ) )
							lastBranchTarget = target;
						break;
					case InstructionType.Jump:
						lastJumpRegister = ( target == uint.MaxValue );
						// Only treat the J as a branch if it's an absolute jump and there are other forward branches
						if( ( lastJumpRegister == false ) &&
							( lastBranchTarget > target ) )
						{
							if( ( target != uint.MaxValue ) &&
								( target > lastBranchTarget ) )
								lastBranchTarget = target;
						}
						break;
					case InstructionType.JumpAndLink:
						break;
				}
				inDelay = ( type != InstructionType.Normal );
				lastJumpType = type;
			}

			if( address - 4 > methodStart )
			{
				// Final end - ideally we'd never have to do this
				Method method = new Method( MethodType.User, methodStart, address - 4 - methodStart );
				db.AddSymbol( method );
				Log.WriteLine( Verbosity.Normal, Feature.Loader, "Analyze didn't finish .text cleanly - last method: {0}", method.ToString() );
			}

			//foreach( Method method in db.GetMethods() )
			//{
			//    Debug.WriteLine( method.ToString() );
			//}
		}

		#region Instructions

		private enum InstructionType
		{
			Normal,
			Branch,
			Jump,
			JumpAndLink,
		}
		private enum AddressBits
		{
			Bits16,
			Bits26,
			Register,
		}
		private struct Instruction
		{
			public readonly string Name;
			public readonly uint Opcode;
			public readonly uint Mask;
			public readonly AddressBits AddressBits;
			public readonly InstructionType Type;
			public Instruction( string name, uint opcode, uint mask, AddressBits addressBits, InstructionType type )
			{
				this.Name = name;
				this.Opcode = opcode;
				this.Mask = mask;
				this.AddressBits = addressBits;
				this.Type = type;
			}
		}

		private readonly static Instruction[] _analysisInstructions = new Instruction[]{
			new Instruction( "beq",			0x10000000, 0xFC000000,	AddressBits.Bits16,		InstructionType.Branch ),
			new Instruction( "beql",		0x50000000, 0xFC000000,	AddressBits.Bits16,		InstructionType.Branch ),
			new Instruction( "bgez",		0x04010000, 0xFC1F0000,	AddressBits.Bits16,		InstructionType.Branch ),
			new Instruction( "bgezal",		0x04110000, 0xFC1F0000,	AddressBits.Bits16,		InstructionType.JumpAndLink ),
			new Instruction( "bgezl",		0x04030000, 0xFC1F0000,	AddressBits.Bits16,		InstructionType.Branch ),
			new Instruction( "bgtz",		0x1C000000, 0xFC1F0000,	AddressBits.Bits16,		InstructionType.Branch ),
			new Instruction( "bgtzl",		0x5C000000, 0xFC1F0000,	AddressBits.Bits16,		InstructionType.Branch ),
			new Instruction( "blez",		0x18000000, 0xFC1F0000,	AddressBits.Bits16,		InstructionType.Branch ),
			new Instruction( "blezl",		0x58000000, 0xFC1F0000,	AddressBits.Bits16,		InstructionType.Branch ),
			new Instruction( "bltz",		0x04000000, 0xFC1F0000,	AddressBits.Bits16,		InstructionType.Branch ),
			new Instruction( "bltzl",		0x04020000, 0xFC1F0000,	AddressBits.Bits16,		InstructionType.Branch ),
			new Instruction( "bltzal",		0x04100000, 0xFC1F0000,	AddressBits.Bits16,		InstructionType.JumpAndLink ),
			new Instruction( "bltzall",		0x04120000, 0xFC1F0000,	AddressBits.Bits16,		InstructionType.JumpAndLink ),
			new Instruction( "bne",			0x14000000, 0xFC000000,	AddressBits.Bits16,		InstructionType.Branch ),
			new Instruction( "bnel",		0x54000000, 0xFC000000,	AddressBits.Bits16,		InstructionType.Branch ),
			new Instruction( "j",			0x08000000, 0xFC000000,	AddressBits.Bits26,		InstructionType.Jump ),
			new Instruction( "jr",			0x00000008, 0xFC1FFFFF,	AddressBits.Register,	InstructionType.Jump ),
			new Instruction( "jalr",		0x00000009, 0xFC1F07FF,	AddressBits.Register,	InstructionType.JumpAndLink ),
			new Instruction( "jal",			0x0C000000, 0xFC000000, AddressBits.Bits26,		InstructionType.JumpAndLink ),
			new Instruction( "bc1f",		0x45000000, 0xFFFF0000, AddressBits.Bits16,		InstructionType.Branch ),
			new Instruction( "bc1fl",		0x45020000, 0xFFFF0000, AddressBits.Bits16,		InstructionType.Branch ),
			new Instruction( "bc1t",		0x45010000, 0xFFFF0000, AddressBits.Bits16,		InstructionType.Branch ),
			new Instruction( "bc1tl",		0x45030000, 0xFFFF0000, AddressBits.Bits16,		InstructionType.Branch ),
			new Instruction( "bvf",			0x49000000, 0xFFE30000, AddressBits.Bits16,		InstructionType.Branch ),
			new Instruction( "bvfl",		0x49020000, 0xFFE30000, AddressBits.Bits16,		InstructionType.Branch ),
			new Instruction( "bvt",			0x49010000, 0xFFE30000, AddressBits.Bits16,		InstructionType.Branch ),
			new Instruction( "bvtl",		0x49030000, 0xFFE30000, AddressBits.Bits16,		InstructionType.Branch ),
		};

		#endregion

		private InstructionType AnalyzeJump( uint address, uint code, out uint target )
		{
			target = uint.MaxValue;

			for( int n = 0; n < _analysisInstructions.Length; n++ )
			{
				Instruction instr = _analysisInstructions[ n ];
				if( ( code & instr.Mask ) == instr.Opcode )
				{
					switch( instr.AddressBits )
					{
						case AddressBits.Bits16:
							int offset = ( short )( code & 0xFFFF );
							target = ( uint )( address + 4 + offset * 4 );
							break;
						case AddressBits.Bits26:
							target = ( code & 0x03FFFFFF ) << 2;
							target += address & 0xF0000000;
							break;
						default:
							target = uint.MaxValue;
							break;
					}
					return instr.Type;
				}
			}
			return InstructionType.Normal;
		}
	}
}
