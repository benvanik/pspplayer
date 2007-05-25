// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace Noxa.Emulation.Psp.RemoteDebugger.Model
{
	delegate bool InstructionFormatter( uint address, uint code, InstructionEntry entry, out Opcode opcode, out Operand[] operands );

	static class Formatters
	{
		#region Extractors

		private static Register RS( uint code )
		{
			return RegisterBanks.General.Registers[ ( ( code >> 21 ) & 0x1F ) ];
		}
		private static Register RT( uint code )
		{
			return RegisterBanks.General.Registers[ ( ( code >> 16 ) & 0x1F ) ];
		}
		private static Register RD( uint code )
		{
			return RegisterBanks.General.Registers[ ( ( code >> 11 ) & 0x1F ) ];
		}
		private static Register FS( uint code )
		{
			return RegisterBanks.Fpu.Registers[ ( ( code >> 11 ) & 0x1F ) ];
		}
		private static Register FT( uint code )
		{
			return RegisterBanks.Fpu.Registers[ ( ( code >> 16 ) & 0x1F ) ];
		}
		private static Register FD( uint code )
		{
			return RegisterBanks.Fpu.Registers[ ( ( code >> 6 ) & 0x1F ) ];
		}
		private static int POS( uint code )
		{
			return ( int )( ( code >> 6 ) & 0x1F );
		}
		private static int SIZE( uint code )
		{
			return ( int )( ( code >> 11 ) & 0x1F );
		}
		private static int IMM16( uint code )
		{
			return ( int )( code & 0xFFFF );
		}

		private static Register VD( uint code )
		{
			return RegisterBanks.Vfpu.Registers[ ( code & 0x7F ) ];
		}
		private static Register VS( uint code )
		{
			return RegisterBanks.Vfpu.Registers[ ( ( code >> 8 ) & 0x7F ) ];
		}
		private static Register VT( uint code )
		{
			return RegisterBanks.Vfpu.Registers[ ( ( code >> 16 ) & 0x7F ) ];
		}

		private static int Xpose( int v )
		{
			return v ^ 0x20;
		}

		#endregion

		#region General Formatters

		public static bool Syscall( uint address, uint code, InstructionEntry entry, out Opcode opcode, out Operand[] operands )
		{
			uint callno = ( code >> 6 ) & 0xFFFFF; //20 bits
			uint funcnum = callno & 0xFFF;
			uint modulenum = ( callno & 0xFF000 ) >> 12;
			//sprintf(out, "syscall\t  %s",/*PSPHLE::GetModuleName(modulenum),*/PSPHLE::GetFuncName(modulenum, funcnum));
			opcode = new Opcode( entry );
			operands = new Operand[]{
				new Operand( ( int )callno, 4 ),
			};
			return true;
		}

		public static bool mxc1( uint address, uint code, InstructionEntry entry, out Opcode opcode, out Operand[] operands )
		{
			opcode = new Opcode( entry );
			operands = new Operand[]{
				new Operand( RT( code ) ),
				new Operand( FS( code ) ),
			};
			return true;
		}

		public static bool addi( uint address, uint code, InstructionEntry entry, out Opcode opcode, out Operand[] operands )
		{
			// TODO: instrinsic
			// if rs == 0, li rt, imm

			return IType( address, code, entry, out opcode, out operands );
		}
		public static bool addu( uint address, uint code, InstructionEntry entry, out Opcode opcode, out Operand[] operands )
		{
			// TODO: instrinsic
			//if (rs==0 && rt==0)
			//    sprintf(out,"li\t%s, 0",RN(rd));
			//else if (rs == 0)
			//    sprintf(out,"mov\t%s, %s",RN(rd),RN(rt));
			//else if (rt == 0)
			//    sprintf(out,"mov\t%s, %s",RN(rd),RN(rs));

			opcode = new Opcode( entry );
			operands = new Operand[]{
				new Operand( RD( code ) ),
				new Operand( RS( code ) ),
				new Operand( RT( code ) ),
			};
			return true;
		}
		public static bool RelBranch2( uint address, uint code, InstructionEntry entry, out Opcode opcode, out Operand[] operands )
		{
			// TODO: intrinsic
			//int o = op>>26;
			//if (o==4 && rs == rt)//beq
			//    sprintf(out,"b\t->$%08x",off);
			//else if (o==4 && rs == rt)//beq
			//    sprintf(out,"bl\t->$%08x",off);
			//else
			//    sprintf(out, "%s\t%s, %s, ->$%08x",name,RN(rt),RN(rs),off);

			int imm = IMM16( code ) << 2;
			opcode = new Opcode( entry );
			operands = new Operand[]{
				new Operand( RT( code ) ),
				new Operand( RS( code ) ),
				new Operand( OperandType.BranchTarget, imm ),
			};
			return true;
		}
		public static bool RelBranch( uint address, uint code, InstructionEntry entry, out Opcode opcode, out Operand[] operands )
		{
			int imm = IMM16( code ) << 2;
			opcode = new Opcode( entry );
			operands = new Operand[]{
				new Operand( RS( code ) ),
				new Operand( OperandType.BranchTarget, imm ),
			};
			return true;
		}
		public static bool Generic( uint address, uint code, InstructionEntry entry, out Opcode opcode, out Operand[] operands )
		{
			opcode = new Opcode( entry );
			operands = new Operand[]{
			};
			return true;
		}
		public static bool IType( uint address, uint code, InstructionEntry entry, out Opcode opcode, out Operand[] operands )
		{
			opcode = new Opcode( entry );
			operands = new Operand[]{
				new Operand( RT( code ) ),
				new Operand( RS( code ) ),
				new Operand( IMM16( code ), 2 ),
			};
			return true;
		}
		public static bool IType1( uint address, uint code, InstructionEntry entry, out Opcode opcode, out Operand[] operands )
		{
			opcode = new Opcode( entry );
			operands = new Operand[]{
				new Operand( RT( code ) ),
				new Operand( IMM16( code ), 2 ),
			};
			return true;
		}
		public static bool ITypeMem( uint address, uint code, InstructionEntry entry, out Opcode opcode, out Operand[] operands )
		{
			opcode = new Opcode( entry );
			operands = new Operand[]{
				new Operand( RT( code ) ),
				new Operand( OperandType.MemoryAccess, RS( code ), IMM16( code ) ),
			};
			return true;
		}
		public static bool RType2( uint address, uint code, InstructionEntry entry, out Opcode opcode, out Operand[] operands )
		{
			opcode = new Opcode( entry );
			operands = new Operand[]{
				new Operand( RD( code ) ),
				new Operand( RS( code ) ),
			};
			return true;
		}
		public static bool RType3( uint address, uint code, InstructionEntry entry, out Opcode opcode, out Operand[] operands )
		{
			opcode = new Opcode( entry );
			operands = new Operand[]{
				new Operand( RD( code ) ),
				new Operand( RS( code ) ),
				new Operand( RT( code ) ),
			};
			return true;
		}
		public static bool MulDivType( uint address, uint code, InstructionEntry entry, out Opcode opcode, out Operand[] operands )
		{
			opcode = new Opcode( entry );
			operands = new Operand[]{
				new Operand( RS( code ) ),
				new Operand( RT( code ) ),
			};
			return true;
		}
		public static bool ShiftType( uint address, uint code, InstructionEntry entry, out Opcode opcode, out Operand[] operands )
		{
			opcode = new Opcode( entry );
			operands = new Operand[]{
				new Operand( RD( code ) ),
				new Operand( RT( code ) ),
				new Operand( POS( code ), 1 ),
			};
			return true;
		}
		public static bool VarShiftType( uint address, uint code, InstructionEntry entry, out Opcode opcode, out Operand[] operands )
		{
			opcode = new Opcode( entry );
			operands = new Operand[]{
				new Operand( RD( code ) ),
				new Operand( RT( code ) ),
				new Operand( RS( code ) ),
			};
			return true;
		}
		public static bool FPU3op( uint address, uint code, InstructionEntry entry, out Opcode opcode, out Operand[] operands )
		{
			opcode = new Opcode( entry );
			operands = new Operand[]{
				new Operand( FD( code ) ),
				new Operand( FS( code ) ),
				new Operand( FT( code ) ),
			};
			return true;
		}
		public static bool FPU2op( uint address, uint code, InstructionEntry entry, out Opcode opcode, out Operand[] operands )
		{
			opcode = new Opcode( entry );
			operands = new Operand[]{
				new Operand( FD( code ) ),
				new Operand( FS( code ) ),
			};
			return true;
		}
		public static bool FPULS( uint address, uint code, InstructionEntry entry, out Opcode opcode, out Operand[] operands )
		{
			opcode = new Opcode( entry );
			operands = new Operand[]{
				new Operand( FT( code ) ),
				new Operand( OperandType.MemoryAccess, RS( code ), IMM16( code ) ),
			};
			return true;
		}
		public static bool FPUComp( uint address, uint code, InstructionEntry entry, out Opcode opcode, out Operand[] operands )
		{
			opcode = new Opcode( entry );
			operands = new Operand[]{
				new Operand( FS( code ) ),
				new Operand( FT( code ) ),
			};
			return true;
		}
		public static bool FPUBranch( uint address, uint code, InstructionEntry entry, out Opcode opcode, out Operand[] operands )
		{
			int imm = IMM16( code ) << 2;
			opcode = new Opcode( entry );
			operands = new Operand[]{
				new Operand( OperandType.BranchTarget, imm ),
			};
			return true;
		}
		public static bool ori( uint address, uint code, InstructionEntry entry, out Opcode opcode, out Operand[] operands )
		{
			// TODO: instrinsic
			// if rs == 0, li rt, imm

			opcode = new Opcode( entry );
			operands = new Operand[]{
				new Operand( RT( code ) ),
				new Operand( RS( code ) ),
				new Operand( IMM16( code ), 2 ),
			};
			return true;
		}
		public static bool Special3( uint address, uint code, InstructionEntry entry, out Opcode opcode, out Operand[] operands )
		{
			int size = 0;
			switch( code & 0x3F )
			{
				case 0x0: // ext
					size = SIZE( code ) + 1;
					break;
				case 0x4: // ins
					size = ( SIZE( code ) + 1 ) - POS( code );
					break;
			}
			opcode = new Opcode( entry );
			operands = new Operand[]{
				new Operand( RT( code ) ),
				new Operand( RS( code ) ),
				new Operand( POS( code ), 1 ),
				new Operand( size, 1 ),
			};
			return true;
		}

		public static bool ToHiloTransfer( uint address, uint code, InstructionEntry entry, out Opcode opcode, out Operand[] operands )
		{
			opcode = new Opcode( entry );
			operands = new Operand[]{
				new Operand( RS( code ) ),
			};
			return true;
		}
		public static bool FromHiloTransfer( uint address, uint code, InstructionEntry entry, out Opcode opcode, out Operand[] operands )
		{
			opcode = new Opcode( entry );
			operands = new Operand[]{
				new Operand( RD( code ) ),
			};
			return true;
		}
		public static bool JumpType( uint address, uint code, InstructionEntry entry, out Opcode opcode, out Operand[] operands )
		{
			uint offset = ( ( code & 0x03FFFFFF ) << 2 );
			uint addr = ( address & 0xF0000000 ) | offset;
			opcode = new Opcode( entry );
			operands = new Operand[]{
				new Operand( OperandType.JumpTarget, ( int )addr ),
			};
			return true;
		}
		public static bool JumpRegType( uint address, uint code, InstructionEntry entry, out Opcode opcode, out Operand[] operands )
		{
			opcode = new Opcode( entry );
			operands = new Operand[]{
				new Operand( OperandType.JumpTarget, RS( code ) ),
			};
			return true;
		}

		public static bool Allegrex( uint address, uint code, InstructionEntry entry, out Opcode opcode, out Operand[] operands )
		{
			opcode = new Opcode( entry );
			operands = new Operand[]{
				new Operand( RD( code ) ),
				new Operand( RT( code ) ),
			};
			return true;
		}
		public static bool Allegrex2( uint address, uint code, InstructionEntry entry, out Opcode opcode, out Operand[] operands )
		{
			opcode = new Opcode( entry );
			operands = new Operand[]{
				new Operand( RD( code ) ),
				new Operand( RT( code ) ),
			};
			return true;
		}

		#endregion

		#region VFPU Formatters

		public static bool Mftv( uint address, uint code, InstructionEntry entry, out Opcode opcode, out Operand[] operands )
		{
			// This may be broken!
			int vr = ( int )( code & 0xFF );
			bool transposed = ( vr & 0x80 ) != 0;
			opcode = new Opcode( entry, transposed ? "c" : "" );
			operands = new Operand[]{
				new Operand( RT( code ) ),
				new Operand( RegisterBanks.Vfpu.Registers[ ( vr & 0x80 ) ], DataSize.V_Single, transposed ),
			};
			return true;
		}

		public static bool SV( uint address, uint code, InstructionEntry entry, out Opcode opcode, out Operand[] operands )
		{
			int offset = ( short )( code & 0xFFFC );
			int vt = ( int )( ( code >> 16 ) & 0x1F ) | ( int )( ( code & 3 ) << 5 );
			// if vt & 0x80, transposed
			opcode = new Opcode( entry );
			operands = new Operand[]{
				new Operand( RegisterBanks.Vfpu.Registers[ vt ], DataSize.V_Single ),
				new Operand( OperandType.MemoryAccess, RS( code ), offset ),
			};
			return true;
		}
		public static bool SVQ( uint address, uint code, InstructionEntry entry, out Opcode opcode, out Operand[] operands )
		{
			int offset = ( short )( code & 0xFFFC );
			int vt = ( int )( ( ( code >> 16 ) & 0x1F ) ) | ( int )( ( code & 1 ) << 5 );
			opcode = new Opcode( entry );
			Operand op1 = new Operand( RegisterBanks.Vfpu.Registers[ vt ], DataSize.V_Quad );
			Operand op2 = new Operand( OperandType.MemoryAccess, RS( code ), offset );
			if( ( code & 0x2 ) != 0 )
			{
				operands = new Operand[]{
					op1, op2,
					new Operand( "wb" ),
				};
			}
			else
			{
				operands = new Operand[]{
					op1, op2,
				};
			}
			return true;
		}
		public static bool SVLRQ( uint address, uint code, InstructionEntry entry, out Opcode opcode, out Operand[] operands )
		{
			int offset = ( short )( code & 0xFFFC );
			int vt = ( int )( ( ( code >> 16 ) & 0x1f ) ) | ( int )( ( code & 1 ) << 5 );
			int lr = ( int )( code >> 1 ) & 1;
			string suffix = string.Format( "{0}.q", ( lr != 0 ) ? "r" : "l" );
			opcode = new Opcode( entry, suffix );
			operands = new Operand[]{
				new Operand( RegisterBanks.Vfpu.Registers[ vt ], DataSize.V_Quad ),
				new Operand( OperandType.MemoryAccess, RS( code ), offset ),
			};
			return true;
		}

		public static bool MatrixSet1( uint address, uint code, InstructionEntry entry, out Opcode opcode, out Operand[] operands )
		{
			DataSize sz = GetMtxSize( code );
			opcode = new Opcode( entry, VSuff( code ) );
			operands = new Operand[]{
				new Operand( VD( code ), sz ), // Matrix
			};
			return true;
		}
		public static bool MatrixSet2( uint address, uint code, InstructionEntry entry, out Opcode opcode, out Operand[] operands )
		{
			DataSize sz = GetMtxSize( code );
			opcode = new Opcode( entry, VSuff( code ) );
			operands = new Operand[]{
				new Operand( VD( code ), sz ), // Matrix
				new Operand( VS( code ), sz ), // Matrix
			};
			return true;
		}
		public static bool MatrixSet3( uint address, uint code, InstructionEntry entry, out Opcode opcode, out Operand[] operands )
		{
			DataSize sz = GetMtxSize( code );
			opcode = new Opcode( entry, VSuff( code ) );
			operands = new Operand[]{
				new Operand( VD( code ), sz ), // Matrix
				new Operand( VS( code ), sz ), // Matrix
				new Operand( VT( code ), sz ), // Matrix
			};
			return true;
		}
		public static bool MatrixMult( uint address, uint code, InstructionEntry entry, out Opcode opcode, out Operand[] operands )
		{
			DataSize sz = GetMtxSize( code );
			opcode = new Opcode( entry, VSuff( code ) );
			operands = new Operand[]{
				new Operand( VD( code ), sz ), // Matrix
				new Operand( VS( code ), sz, true ), // Matrix
				new Operand( VT( code ), sz ), // Matrix
			};
			return true;
		}

		public static bool VectorDot( uint address, uint code, InstructionEntry entry, out Opcode opcode, out Operand[] operands )
		{
			DataSize sz = GetVecSize( code );
			opcode = new Opcode( entry );
			operands = new Operand[]{
				new Operand( VD( code ), DataSize.V_Single ),
				new Operand( VS( code ), sz ),
				new Operand( VT( code ), sz ),
			};
			return true;
		}
		public static bool Vfad( uint address, uint code, InstructionEntry entry, out Opcode opcode, out Operand[] operands )
		{
			DataSize sz = GetVecSize( code );
			opcode = new Opcode( entry );
			operands = new Operand[]{
				new Operand( VD( code ), DataSize.V_Single ),
				new Operand( VS( code ), sz ),
			};
			return true;
		}
		public static bool VectorSet1( uint address, uint code, InstructionEntry entry, out Opcode opcode, out Operand[] operands )
		{
			DataSize sz = GetVecSize( code );
			opcode = new Opcode( entry, VSuff( code ) );
			operands = new Operand[]{
				new Operand( VD( code ), sz ),
			};
			return true;
		}
		public static bool VectorSet2( uint address, uint code, InstructionEntry entry, out Opcode opcode, out Operand[] operands )
		{
			DataSize sz = GetVecSize( code );
			opcode = new Opcode( entry, VSuff( code ) );
			operands = new Operand[]{
				new Operand( VD( code ), sz ),
				new Operand( VS( code ), sz ),
			};
			return true;
		}
		public static bool VectorSet3( uint address, uint code, InstructionEntry entry, out Opcode opcode, out Operand[] operands )
		{
			DataSize sz = GetVecSize( code );
			opcode = new Opcode( entry, VSuff( code ) );
			operands = new Operand[]{
				new Operand( VD( code ), sz ),
				new Operand( VS( code ), sz ),
				new Operand( VT( code ), sz ),
			};
			return true;
		}
		public static bool VRot( uint address, uint code, InstructionEntry entry, out Opcode opcode, out Operand[] operands )
		{
			int imm = ( int )( code >> 16 ) & 0x1F;
			bool negSin = ( ( imm & 0x10 ) == 0x10 );
			char[] c = new char[] { '.', '.', '.', '.' };
			char[] temp = new char[ 16 ];
			if( ( ( imm >> 2 ) & 3 ) == ( imm & 3 ) )
			{
				for( int n = 0; n < 4; n++ )
					c[ n ] = 'S';
			}
			c[ ( imm >> 2 ) & 3 ] = 'S';
			c[ imm & 3 ] = 'C';
			DataSize sz = GetVecSize( code );
			int numElems = GetNumElements( sz );
			int pos = 0;
			temp[ pos++ ] = '[';
			for( int n = 0; n < numElems; n++ )
			{
				if( c[ n ] == 'S' && negSin )
					temp[ pos++ ] = '-';
				else
					temp[ pos++ ] = ' ';
				temp[ pos++ ] = c[ n ];
				temp[ pos++ ] = ' ';
			}
			temp[ pos++ ] = ']';
			opcode = new Opcode( entry, VSuff( code ) );
			operands = new Operand[]{
				new Operand( VD( code ), sz ),
				new Operand( VS( code ), DataSize.V_Single ),
				new Operand( new string( temp, 0, pos ) ),
			};
			return true;
		}
		public static bool VScl( uint address, uint code, InstructionEntry entry, out Opcode opcode, out Operand[] operands )
		{
			DataSize sz = GetVecSize( code );
			opcode = new Opcode( entry );
			operands = new Operand[]{
				new Operand( VD( code ), sz ),
				new Operand( VS( code ), sz ),
				new Operand( VT( code ), DataSize.V_Single ),
			};
			return true;
		}

		private static string[] vregnames = new string[] { "X", "Y", "Z", "W" };
		private static string[] vconstants = new string[] { "0", "1", "2", "1/2", "3", "1/3", "1/4", "1/6" };
		public static bool VPFXST( uint address, uint code, InstructionEntry entry, out Opcode opcode, out Operand[] operands )
		{
			List<Operand> ops = new List<Operand>();
			int data = ( int )( code & 0xFFFFF );
			for( int n = 0; n < 4; n++ )
			{
				int regnum = ( data >> ( n * 2 ) ) & 3;
				int abs = ( data >> ( 8 + n ) ) & 1;
				int negate = ( data >> ( 16 + n ) ) & 1;
				int constants = ( data >> ( 12 + n ) ) & 1;
				string op = "";
				if( negate != 0 )
					op += "-";
				if( ( abs != 0 ) && ( constants == 0 ) )
					op += "|";
				if( constants == 0 )
					op += vregnames[ regnum ];
				else
				{
					if( abs != 0 )
						regnum += 4;
					op += vconstants[ regnum ];
				}
				if( ( abs != 0 ) && ( constants == 0 ) )
					op += "|";
				if( op.Length > 0 )
					ops.Add( new Operand( op ) );
			}
			opcode = new Opcode( entry );
			operands = ops.ToArray();
			return true;
		}
		private static string[] satNames = new string[] { "", "[0:1]", "X", "[-1:1]" };
		public static bool VPFXD( uint address, uint code, InstructionEntry entry, out Opcode opcode, out Operand[] operands )
		{
			List<Operand> ops = new List<Operand>();
			int data = ( int )( code & 0xFFFFF );
			for( int n = 0; n < 4; n++ )
			{
				int sat = ( data >> ( n * 2 ) ) & 0x3;
				int mask = ( data >> ( 8 + n ) ) & 0x1;
				string op = "";
				if( sat != 0 )
					op += satNames[ sat ];
				if( mask != 0 )
					op += "M";
				if( op.Length > 0 )
					ops.Add( new Operand( op ) );
			}
			opcode = new Opcode( entry );
			operands = ops.ToArray();
			return true;
		}
		public static bool Vcrs( uint address, uint code, InstructionEntry entry, out Opcode opcode, out Operand[] operands )
		{
			DataSize sz = GetVecSize( code );
			if( sz != DataSize.V_Triple )
			{
				// ?
				opcode = new Opcode( entry, ".??????" );
				operands = new Operand[]{
				};
			}
			else
			{
				opcode = new Opcode( entry, VSuff( code ) );
				operands = new Operand[]{
					new Operand( VD( code ), sz ),
					new Operand( VS( code ), sz ),
					new Operand( VT( code ), sz ),
				};
			}
			return true;
		}
		public static bool Viim( uint address, uint code, InstructionEntry entry, out Opcode opcode, out Operand[] operands )
		{
			int imm = ( int )( code & 0xFFFF );
			int type = ( int )( code >> 23 ) & 7;
			opcode = new Opcode( entry );
			if( type == 6 )
			{
				operands = new Operand[]{
					new Operand( VT( code ), DataSize.V_Single ),
					new Operand( imm, 2 ),
				};
			}
			else if( type == 7 )
			{
				operands = new Operand[]{
					new Operand( VT( code ), DataSize.V_Single ),
					new Operand( Float16ToFloat32( ( ushort )imm ) ),
				};
			}
			else
			{
				operands = new Operand[]{
					new Operand( "????" ),
				};
			}
			return true;
		}
		#region VFPU Constants
		private static string[] vfpuconstants = new string[]
		{
			"(undef)",
			"MaxFloat",
			"Sqrt(2)",
			"Sqrt(1/2)",
			"2/Sqrt(PI)",
			"2/PI",
			"1/PI",
			"PI/4",
			"PI/2",
			"PI",
			"e",
			"Log2(e)",
			"Log10(e)",
			"ln(2)",
			"ln(10)",
			"2*PI",
			"PI/6",
			"Log10(2)",
			"Log2(10)",
			"Sqrt(3)/2",
		};
		#endregion
		public static bool Vcst( uint address, uint code, InstructionEntry entry, out Opcode opcode, out Operand[] operands )
		{
			int conNum = ( int )( code >> 16 ) & 0x1F;
			string con;
			if( conNum >= vfpuconstants.Length )
				con = vfpuconstants[ 0 ];
			else
				con = vfpuconstants[ conNum ];
			opcode = new Opcode( entry, VSuff( code ) );
			operands = new Operand[]{
				new Operand( VD( code ), DataSize.V_Single ),
				new Operand( con ),
			};
			return true;
		}
		public static bool CrossQuat( uint address, uint code, InstructionEntry entry, out Opcode opcode, out Operand[] operands )
		{
			DataSize sz = GetVecSize( code );
			string name;
			switch( sz )
			{
				default:
				case DataSize.V_Triple:
					name = "vcrsp";
					//Ah, a regular cross product.
					break;
				case DataSize.V_Quad:
					name = "vqmul";
					//Ah, a quaternion multiplication.
					break;
			}
			opcode = new Opcode( entry, VSuff( code ), name );
			operands = new Operand[]{
				new Operand( VD( code ), sz ),
				new Operand( VS( code ), sz ),
				new Operand( VT( code ), sz ),
			};
			return true;
		}
		public static bool Vtfm( uint address, uint code, InstructionEntry entry, out Opcode opcode, out Operand[] operands )
		{
			Register vd = VD( code );
			Register vs = VS( code );
			Register vt = VT( code );
			uint ins = ( code >> 23 ) & 7;
			DataSize sz = GetVecSize( code );
			DataSize msz = GetMtxSize( code );
			int n = GetNumElements( sz );

			string suffix = string.Format( "{0}{1}", n, VSuff( code ) );
			if( n == ins )
			{
				//homogenous
				opcode = new Opcode( entry, suffix, "vhtfm" );
				operands = new Operand[]{
					new Operand( vd, sz ),
					new Operand( vs, msz ), // Matrix
					new Operand( vt, sz ),
				};
			}
			else if( n == ins + 1 )
			{
				opcode = new Opcode( entry, suffix, "vtfm" );
				operands = new Operand[]{
					new Operand( vd, sz ),
					new Operand( vs, msz ), // Matrix
					new Operand( vt, sz ),
				};
			}
			else
			{
				// ?
				opcode = new Opcode( entry, suffix );
				operands = new Operand[]{
					new Operand( "badvtfm" ),
				};
			}
			return true;
		}
		private static string[] condNames = new string[] { "FL", "EQ", "LT", "LE", "TR", "NE", "GE", "GT", "EZ", "EN", "EI", "ES", "NZ", "NN", "NI", "NS" };
		public static bool Vcmp( uint address, uint code, InstructionEntry entry, out Opcode opcode, out Operand[] operands )
		{
			uint cond = code & 15;
			DataSize sz = GetVecSize( code );
			opcode = new Opcode( entry, VSuff( code ) );
			operands = new Operand[]{
				new Operand( condNames[ cond ] ),
				new Operand( VS( code ), sz ),
				new Operand( VT( code ), sz ),
			};
			return true;
		}
		public static bool Vcmov( uint address, uint code, InstructionEntry entry, out Opcode opcode, out Operand[] operands )
		{
			DataSize sz = GetVecSize( code );
			Register vd = VD( code );
			Register vs = VS( code );
			uint tf = ( code >> 19 ) & 3;
			uint imm3 = ( code >> 16 ) & 7;
			if( tf > 1 )
			{
				// ?????
				opcode = new Opcode( entry, ".??????" );
				operands = new Operand[]{
					new Operand( ( int )tf, 1 ),
				};
			}
			else
			{
				string suffix = string.Format( "{0}{1}",
					( tf == 0 ) ? "t" : "f",
					VSuff( code ) );
				opcode = new Opcode( entry, suffix );
				if( imm3 < 6 )
				{
					operands = new Operand[]{
						new Operand( vd, sz ),
						new Operand( vs, sz ),
						new Operand( ( int )imm3, 1, "CC" ),
					};
				}
				else
				{
					Debug.Assert( imm3 == 6 );
					operands = new Operand[]{
						new Operand( vd, sz ),
						new Operand( vs, sz ),
						new Operand( "CC[...]" ),
					};
				}
			}
			return true;
		}
		public static bool Vflush( uint address, uint code, InstructionEntry entry, out Opcode opcode, out Operand[] operands )
		{
			opcode = new Opcode( entry, ".??????" );
			operands = new Operand[]{
			};
			return true;
		}
		public static bool Vbfy( uint address, uint code, InstructionEntry entry, out Opcode opcode, out Operand[] operands )
		{
			DataSize sz = GetVecSize( code );
			opcode = new Opcode( entry, VSuff( code ) );
			operands = new Operand[]{
				new Operand( VD( code ), sz ),
				new Operand( VS( code ), sz ),
			};
			return true;
		}
		public static bool Vf2i( uint address, uint code, InstructionEntry entry, out Opcode opcode, out Operand[] operands )
		{
			DataSize sz = GetVecSize( code );
			uint imm = ( code >> 16 ) & 0x1F;
			opcode = new Opcode( entry, VSuff( code ) );
			operands = new Operand[]{
				new Operand( VD( code ), sz ),
				new Operand( VS( code ), sz ),
				new Operand( ( int )imm, 1 ),
			};
			return true;
		}
		public static bool Vi2x( uint address, uint code, InstructionEntry entry, out Opcode opcode, out Operand[] operands )
		{
			DataSize sz = GetVecSize( code );
			DataSize dsz = GetHalfSize( sz );
			if( ( ( code >> 16 ) & 3 ) == 0 )
				dsz = DataSize.V_Single;
			opcode = new Opcode( entry, VSuff( code ) );
			operands = new Operand[]{
				new Operand( VD( code ), dsz ),
				new Operand( VS( code ), sz ),
			};
			return true;
		}
		public static bool VBranch( uint address, uint code, InstructionEntry entry, out Opcode opcode, out Operand[] operands )
		{
			uint imm3 = ( code >> 18 ) & 7;
			opcode = new Opcode( entry );
			operands = new Operand[]{
				new Operand( ( int )imm3, 4, "CC" ),
				new Operand( OperandType.BranchTarget, IMM16( code ) ),
			};
			return true;
		}

		#endregion

		#region VFPU Utilities

		private static int GetNumElements( DataSize sz )
		{
			switch( sz )
			{
				case DataSize.V_Single:
					return 1;
				case DataSize.V_Pair:
					return 2;
				case DataSize.V_Triple:
					return 3;
				case DataSize.V_Quad:
					return 4;

				case DataSize.V_2x2:
					return 4;
				case DataSize.V_3x3:
					return 9;
				case DataSize.V_4x4:
					return 16;
			}
			return 0;
		}

		private static DataSize GetHalfSize( DataSize sz )
		{
			switch( sz )
			{
				case DataSize.V_Pair:
					return DataSize.V_Single;
				case DataSize.V_Quad:
					return DataSize.V_Pair;
				case DataSize.V_2x2:
					return DataSize.V_Single;
				case DataSize.V_4x4:
					return DataSize.V_2x2;
			}
			return DataSize.V_Invalid;
		}

		private static DataSize GetVecSize( uint op )
		{
			uint a = ( op >> 7 ) & 1;
			uint b = ( op >> 15 ) & 1;
			a += ( b << 1 );
			switch( a )
			{
				case 0:
					return DataSize.V_Single;
				case 1:
					return DataSize.V_Pair;
				case 2:
					return DataSize.V_Triple;
				case 3:
					return DataSize.V_Quad;
				default:
					return DataSize.V_Invalid;
			}
		}

		private static DataSize GetMtxSize( uint op )
		{
			uint a = ( op >> 7 ) & 1;
			uint b = ( op >> 15 ) & 1;
			a += ( b << 1 );
			switch( a )
			{
				case 1:
					return DataSize.V_2x2;
				case 2:
					return DataSize.V_3x3;
				case 3:
					return DataSize.V_4x4;
				default:
					return DataSize.V_Invalid;
			}
		}

		private static int GetMatrixSide( DataSize sz )
		{
			switch( sz )
			{
				case DataSize.V_2x2:
					return 2;
				case DataSize.V_3x3:
					return 3;
				case DataSize.V_4x4:
					return 4;
				default:
					return 0;
			}
		}

		private static string VSuff( uint op )
		{
			uint a = ( op >> 7 ) & 1;
			uint b = ( op >> 15 ) & 1;
			a += ( b << 1 );
			switch( a )
			{
				case 0:
					return ".s";
				case 1:
					return ".p";
				case 2:
					return ".t";
				case 3:
					return ".q";
				default:
					return "%";
			}
		}

		private const ushort VFPU_FLOAT16_EXP_MAX = 0x1f;
		private const ushort VFPU_SH_FLOAT16_SIGN = 15;
		private const ushort VFPU_MASK_FLOAT16_SIGN = 0x1;
		private const ushort VFPU_SH_FLOAT16_EXP = 10;
		private const ushort VFPU_MASK_FLOAT16_EXP = 0x1f;
		private const ushort VFPU_SH_FLOAT16_FRAC = 0;
		private const ushort VFPU_MASK_FLOAT16_FRAC = 0x3ff;

		[StructLayout( LayoutKind.Explicit, Size = 4 )]
		private struct float2int
		{
			[FieldOffset( 0 )]
			public uint i;
			[FieldOffset( 0 )]
			public float f;
		}

		private static float Float16ToFloat32( ushort l )
		{
			ushort float16 = l;
			uint sign = ( uint )( float16 >> VFPU_SH_FLOAT16_SIGN ) & VFPU_MASK_FLOAT16_SIGN;
			int exponent = ( float16 >> VFPU_SH_FLOAT16_EXP ) & VFPU_MASK_FLOAT16_EXP;
			uint fraction = ( uint )float16 & VFPU_MASK_FLOAT16_FRAC;

			float signf = ( sign == 1 ) ? -1.0f : 1.0f;

			float f;
			if( exponent == VFPU_FLOAT16_EXP_MAX )
			{
				if( fraction == 0 )
					f = float.PositiveInfinity; //(*info->fprintf_func) (info->stream, "%cInf", signchar);
				else
					f = float.NaN; //(*info->fprintf_func) (info->stream, "%cNaN", signchar);
			}
			else if( exponent == 0 && fraction == 0 )
			{
				f = 0.0f * signf;
			}
			else
			{
				if( exponent == 0 )
				{
					do
					{
						fraction <<= 1;
						exponent--;
					}
					while( ( fraction & ( VFPU_MASK_FLOAT16_FRAC + 1 ) ) == 0 );

					fraction &= VFPU_MASK_FLOAT16_FRAC;
				}

				/* Convert to 32-bit single-precision IEEE754. */
				float2int f2i;
				f2i.f = 0.0f;
				f2i.i = sign << 31;
				f2i.i |= ( uint )( exponent + 112 ) << 23;
				f2i.i |= fraction << 13;
				f = f2i.f;
			}
			return f;
		}

		#endregion
	}
}
