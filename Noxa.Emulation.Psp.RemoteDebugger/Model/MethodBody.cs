// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Noxa.Emulation.Psp.RemoteDebugger.Model
{
	class MethodBody
	{
		public readonly uint Address;
		public readonly uint Length;

		public Instruction[] Instructions;

		public MethodBody( uint address, uint length, Instruction[] instructions )
		{
			this.Address = address;
			this.Length = length;
			this.Instructions = instructions;
		}
	}

	class Instruction
	{
		public readonly uint Address;
		public readonly uint Code;
		public readonly Opcode Opcode;
		public readonly Operand[] Operands;

		//public Reference Reference;

		public const int InvalidBreakpointID = -1;
		public int BreakpointID = InvalidBreakpointID;

		public Instruction( uint address, uint code )
		{
			this.Address = address;
			this.Code = code;

			InstructionEntry instructionEntry = InstructionTables.GetInstruction( code );
			Debug.Assert( instructionEntry != null );
			Debug.Assert( instructionEntry.Formatter != null );

			bool formatted = instructionEntry.Formatter( address, code, instructionEntry, out this.Opcode, out this.Operands );
			Debug.Assert( formatted == true );
		}

		public override string ToString()
		{
			return this.Opcode.ToString() + this.GetOperandsString();
		}

		public string GetOperandsString()
		{
			StringBuilder sb = new StringBuilder();
			for( int n = 0; n < this.Operands.Length; n++ )
			{
				Operand op = this.Operands[ n ];
				bool last = ( n == this.Operands.Length - 1 );
				sb.AppendFormat( " {0}{1}", op.ToString(), last ? "" : "," );
			}
			return sb.ToString();
		}
	}

	class Opcode
	{
		public readonly InstructionEntry InstructionEntry;

		public readonly string Suffix;
		public readonly string NameOverride;

		public Opcode( InstructionEntry instructionEntry )
		{
			this.InstructionEntry = instructionEntry;
		}

		public Opcode( InstructionEntry instructionEntry, string suffix )
			: this( instructionEntry )
		{
			this.Suffix = suffix;
		}

		public Opcode( InstructionEntry instructionEntry, string suffix, string nameOverride )
			: this( instructionEntry, suffix )
		{
			this.NameOverride = nameOverride;
		}

		public override string ToString()
		{
			string name = this.InstructionEntry.Name;
			if( this.NameOverride != null )
				name = this.NameOverride;
			if( this.Suffix != null )
				return string.Format( "{0}{1}", name, this.Suffix );
			else
				return name;
		}
	}

	enum OperandType
	{
		Annotation,
		Immediate,
		ImmediateFloat,
		BranchTarget,
		JumpTarget,
		Register,
		MemoryAccess,
		VfpuRegister,
	}

	enum DataSize
	{
		V_Single,
		V_Pair,
		V_Triple,
		V_Quad,
		V_2x2,
		V_3x3,
		V_4x4,
		V_Invalid
	}

	class Operand
	{
		public readonly OperandType Type;

		public readonly Register Register;
		public readonly int Immediate;
		public readonly float ImmediateFloat;
		public readonly int Size;
		public readonly string Annotation;

		public readonly DataSize DataSize;
		public readonly bool Transposed;

		//public Reference Reference;

		public Operand( string annotation )
		{
			this.Type = OperandType.Annotation;
			this.Annotation = annotation;
		}

		public Operand( Register register )
		{
			this.Type = OperandType.Register;
			this.Register = register;
		}

		public Operand( OperandType type, Register register )
		{
			this.Type = type;
			this.Register = register;
		}

		public Operand( OperandType type, Register register, int offset )
		{
			this.Type = type;
			this.Register = register;
			this.Immediate = offset;
		}

		public Operand( float immediate )
		{
			this.Type = OperandType.ImmediateFloat;
			this.ImmediateFloat = immediate;
		}

		public Operand( int immediate, int size )
		{
			this.Type = OperandType.Immediate;
			this.Immediate = immediate;
			this.Size = size;
		}

		public Operand( int immediate, int size, string annotation )
		{
			this.Type = OperandType.Immediate;
			this.Immediate = immediate;
			this.Size = size;
			this.Annotation = annotation;
		}

		public Operand( OperandType type, int immediate )
		{
			this.Type = type;
			this.Immediate = immediate;
		}

		public Operand( Register register, DataSize dataSize )
			: this( register, dataSize, false )
		{
		}

		public Operand( Register register, DataSize dataSize, bool transposed )
		{
			this.Type = OperandType.VfpuRegister;
			this.Register = register;
			this.DataSize = dataSize;
			this.Transposed = transposed;
		}

		private static string GetVRNotation( Operand op )
		{
			int reg = op.Register.Ordinal;
			int mtx = ( reg >> 2 ) & 7;
			int idx = reg & 3;
			int fsl = 0;
			bool transpose = op.Transposed;
			char c;
			switch( op.DataSize )
			{
				case DataSize.V_Single:
					transpose = false;
					c = 'S';
					fsl = ( reg >> 5 ) & 3;
					break;
				case DataSize.V_Pair:
					c = 'C';
					fsl = ( reg >> 5 ) & 2;
					break;
				case DataSize.V_Triple:
					c = 'C';
					fsl = ( reg >> 6 ) & 1;
					break;
				case DataSize.V_Quad:
					c = 'C';
					fsl = ( reg >> 5 ) & 2;
					break;

				case DataSize.V_2x2:
					c = 'M';
					fsl = ( reg >> 5 ) & 2;
					break;
				case DataSize.V_3x3:
					c = 'M';
					fsl = ( reg >> 6 ) & 1;
					break;
				case DataSize.V_4x4:
					c = 'M';
					fsl = ( reg >> 5 ) & 2;
					break;
				default:
				case DataSize.V_Invalid:
					c = 'I';
					fsl = 0;
					break;
			}
			if( transpose && c == 'C' )
				c = 'R';
			if( transpose && c == 'M' )
				c = 'E';
			return string.Format( "{0}{1}{2}{3}", c, mtx, idx, fsl );
			//sprintf(hej[yo],"%c%i%i%i",c,mtx,idx,fsl);
		}

		// Could be:
		// - an annotation					li $t0, foo [foo]
		// - a register						li $t0, 0 [$t0]
		// - a jump/branch register			be $t0, $t1 [$t1]
		// - an offset register				lw $t0, 123($t1) [$t1 and 123]
		// - an immediate					li $t0, 123 [123]
		// - an immediate with annotation	li $t0, CC[123]  [CC is annotation, 123 is value]
		// - a jump immediate				j 0x1234 [0x1234]
		// - a VFPU register of various sizes/transposed

		public override string ToString()
		{
			switch( this.Type )
			{
				default:
				case OperandType.Annotation:
					return this.Annotation;
				case OperandType.BranchTarget:
					return this.Immediate.ToString();
				case OperandType.JumpTarget:
					if( this.Register != null )
						return this.Register.ToString();
					else
						return string.Format( "0x{0:X8}", this.Immediate );
				case OperandType.MemoryAccess:
					return string.Format( "{0}({1})", this.Immediate, this.Register.ToString() );
				case OperandType.Register:
					return this.Register.ToString();
				case OperandType.VfpuRegister:
					return Operand.GetVRNotation( this );
				case OperandType.Immediate:
					if( this.Annotation != null )
						return string.Format( "{0}[{1}]", this.Annotation, this.Immediate );
					else
						return this.Immediate.ToString();
				case OperandType.ImmediateFloat:
					return this.ImmediateFloat.ToString();
			}
		}
	}

	abstract class Reference
	{
	}

	class MemoryReference : Reference
	{
	}

	class CodeReference : Reference
	{
	}
}
