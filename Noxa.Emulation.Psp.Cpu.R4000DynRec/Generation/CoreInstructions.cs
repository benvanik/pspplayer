using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection.Emit;
using System.Diagnostics;
using System.Reflection;
using Noxa.Emulation.Psp.Bios;

namespace Noxa.Emulation.Psp.Cpu.Generation
{
	static class CoreInstructions
	{
		public static GenerateInstructionR[] TableR = SetupTableR();
		public static GenerateInstructionI[] TableI = SetupTableI();
		public static GenerateInstructionJ[] TableJ = SetupTableJ();
		public static GenerateInstructionCop0[] TableCop0 = SetupTableCop0();
		public static GenerateInstructionSpecial3[] TableSpecial3 = SetupTableSpecial3();

		// void c( core0, memory, generalRegisters[], syscallList[] )

		// LDCz 553 ?
		// SDCz 599 ?

		#region Setup

		private static GenerateInstructionR[] SetupTableR()
		{
			GenerateInstructionR[] instrs = new GenerateInstructionR[ 64 ];

			// Populate table
			GenerateInstructionR unk = new GenerateInstructionR( UnknownR );
			for( int n = 0; n < instrs.Length; n++ )
				instrs[ n ] = unk;

			instrs[ 0 ] = new GenerateInstructionR( Arithmetic.SLL );
			instrs[ 2 ] = new GenerateInstructionR( Arithmetic.SRL );
			instrs[ 3 ] = new GenerateInstructionR( Arithmetic.SRA );
			instrs[ 4 ] = new GenerateInstructionR( Arithmetic.SLLV );
			instrs[ 6 ] = new GenerateInstructionR( Arithmetic.SRLV );
			instrs[ 7 ] = new GenerateInstructionR( Arithmetic.SRAV );
			instrs[ 8 ] = new GenerateInstructionR( Control.JR );
			instrs[ 9 ] = new GenerateInstructionR( Control.JALR );
			instrs[ 10 ] = new GenerateInstructionR( Arithmetic.MOVZ );
			instrs[ 11 ] = new GenerateInstructionR( Arithmetic.MOVN );
			instrs[ 12 ] = new GenerateInstructionR( Special.SYSCALL );
			instrs[ 13 ] = new GenerateInstructionR( Special.BREAK );
			instrs[ 15 ] = new GenerateInstructionR( Special.SYNC );
			instrs[ 16 ] = new GenerateInstructionR( Arithmetic.MFHI );
			instrs[ 17 ] = new GenerateInstructionR( Arithmetic.MTHI );
			instrs[ 18 ] = new GenerateInstructionR( Arithmetic.MFLO );
			instrs[ 19 ] = new GenerateInstructionR( Arithmetic.MTLO );
			instrs[ 24 ] = new GenerateInstructionR( Arithmetic.MULT );
			instrs[ 25 ] = new GenerateInstructionR( Arithmetic.MULTU );
			instrs[ 26 ] = new GenerateInstructionR( Arithmetic.DIV );
			instrs[ 27 ] = new GenerateInstructionR( Arithmetic.DIVU );
			instrs[ 32 ] = new GenerateInstructionR( Arithmetic.ADD );
			instrs[ 33 ] = new GenerateInstructionR( Arithmetic.ADDU );
			instrs[ 34 ] = new GenerateInstructionR( Arithmetic.SUB );
			instrs[ 35 ] = new GenerateInstructionR( Arithmetic.SUBU );
			instrs[ 36 ] = new GenerateInstructionR( Arithmetic.AND );
			instrs[ 37 ] = new GenerateInstructionR( Arithmetic.OR );
			instrs[ 38 ] = new GenerateInstructionR( Arithmetic.XOR );
			instrs[ 39 ] = new GenerateInstructionR( Arithmetic.NOR );
			instrs[ 42 ] = new GenerateInstructionR( Arithmetic.SLT );
			instrs[ 43 ] = new GenerateInstructionR( Arithmetic.SLTU );
			instrs[ 45 ] = new GenerateInstructionR( Arithmetic.MIN );

			return instrs;
		}

		private static GenerateInstructionI[] SetupTableI()
		{
			GenerateInstructionI[] instrs = new GenerateInstructionI[ 64 ];

			// Populate table
			GenerateInstructionI unk = new GenerateInstructionI( UnknownI );
			for( int n = 0; n < instrs.Length; n++ )
				instrs[ n ] = unk;

			instrs[ 2 ] = new GenerateInstructionI( Control.J );
			instrs[ 3 ] = new GenerateInstructionI( Control.JAL );
			instrs[ 4 ] = new GenerateInstructionI( Control.BEQ );
			instrs[ 5 ] = new GenerateInstructionI( Control.BNE );
			instrs[ 6 ] = new GenerateInstructionI( Control.BLEZ );
			instrs[ 7 ] = new GenerateInstructionI( Control.BGTZ );

			instrs[ 8 ] = new GenerateInstructionI( Arithmetic.ADDI );
			instrs[ 9 ] = new GenerateInstructionI( Arithmetic.ADDIU );
			instrs[ 10 ] = new GenerateInstructionI( Arithmetic.SLTI );
			instrs[ 11 ] = new GenerateInstructionI( Arithmetic.SLTIU );
			instrs[ 12 ] = new GenerateInstructionI( Arithmetic.ANDI );
			instrs[ 13 ] = new GenerateInstructionI( Arithmetic.ORI );
			instrs[ 14 ] = new GenerateInstructionI( Arithmetic.XORI );
			instrs[ 15 ] = new GenerateInstructionI( Arithmetic.LUI );

			instrs[ 17 ] = new GenerateInstructionI( Special.COP1 );
			instrs[ 18 ] = new GenerateInstructionI( Special.COP2 );

			instrs[ 20 ] = new GenerateInstructionI( Control.BEQL );
			instrs[ 21 ] = new GenerateInstructionI( Control.BNEL );
			instrs[ 22 ] = new GenerateInstructionI( Control.BLEZL );
			instrs[ 23 ] = new GenerateInstructionI( Control.BGTZL );

			instrs[ 32 ] = new GenerateInstructionI( Memory.LB );
			instrs[ 33 ] = new GenerateInstructionI( Memory.LH );
			instrs[ 34 ] = new GenerateInstructionI( Memory.LWL );
			instrs[ 35 ] = new GenerateInstructionI( Memory.LW );
			instrs[ 36 ] = new GenerateInstructionI( Memory.LBU );
			instrs[ 37 ] = new GenerateInstructionI( Memory.LHU );
			instrs[ 38 ] = new GenerateInstructionI( Memory.LWR );
			instrs[ 40 ] = new GenerateInstructionI( Memory.SB );
			instrs[ 41 ] = new GenerateInstructionI( Memory.SH );
			instrs[ 42 ] = new GenerateInstructionI( Memory.SWL );
			instrs[ 43 ] = new GenerateInstructionI( Memory.SW );
			instrs[ 46 ] = new GenerateInstructionI( Memory.SWR );
			instrs[ 47 ] = new GenerateInstructionI( Memory.CACHE );
			instrs[ 48 ] = new GenerateInstructionI( Memory.LL );
			instrs[ 50 ] = new GenerateInstructionI( Memory.LWC2 );
			instrs[ 56 ] = new GenerateInstructionI( Memory.SC );
			instrs[ 58 ] = new GenerateInstructionI( Memory.SWC2 );

			return instrs;
		}

		private static GenerateInstructionJ[] SetupTableJ()
		{
			GenerateInstructionJ[] instrs = new GenerateInstructionJ[ 64 ];

			// Populate table
			GenerateInstructionJ unk = new GenerateInstructionJ( UnknownJ );
			for( int n = 0; n < instrs.Length; n++ )
				instrs[ n ] = unk;

			instrs[ 0 ] = new GenerateInstructionJ( Control.BLTZ );
			instrs[ 1 ] = new GenerateInstructionJ( Control.BGEZ );
			instrs[ 2 ] = new GenerateInstructionJ( Control.BLTZL );
			instrs[ 3 ] = new GenerateInstructionJ( Control.BGEZL );
			instrs[ 16 ] = new GenerateInstructionJ( Control.BLTZAL );
			instrs[ 17 ] = new GenerateInstructionJ( Control.BGEZAL );
			instrs[ 18 ] = new GenerateInstructionJ( Control.BLTZALL );
			instrs[ 19 ] = new GenerateInstructionJ( Control.BGEZALL );

			return instrs;
		}

		private static GenerateInstructionCop0[] SetupTableCop0()
		{
			GenerateInstructionCop0[] instrs = new GenerateInstructionCop0[ 64 ];

			// Populate table
			GenerateInstructionCop0 unk = new GenerateInstructionCop0( UnknownCop0 );
			for( int n = 0; n < instrs.Length; n++ )
				instrs[ n ] = unk;

			instrs[ 0 ] = new GenerateInstructionCop0( Cop0.MTFC0 );

			return instrs;
		}

		private static GenerateInstructionSpecial3[] SetupTableSpecial3()
		{
			GenerateInstructionSpecial3[] instrs = new GenerateInstructionSpecial3[ 64 ];

			// Populate table
			GenerateInstructionSpecial3 unk = new GenerateInstructionSpecial3( UnknownSpecial3 );
			for( int n = 0; n < instrs.Length; n++ )
				instrs[ n ] = unk;

			instrs[ 16 ] = new GenerateInstructionSpecial3( Arithmetic.SEB );
			instrs[ 24 ] = new GenerateInstructionSpecial3( Arithmetic.SEH );

			return instrs;
		}

		#endregion

		#region Unknown helpers

		public static GenerationResult UnknownR( GenerationContext context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
		{
			return GenerationResult.Invalid;
		}

		public static GenerationResult UnknownI( GenerationContext context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
		{
			return GenerationResult.Invalid;
		}

		public static GenerationResult UnknownJ( GenerationContext context, int pass, int address, uint code, byte opcode, uint imm )
		{
			return GenerationResult.Invalid;
		}

		public static GenerationResult UnknownCop0( GenerationContext context, int pass, int address, uint code, byte function )
		{
			return GenerationResult.Invalid;
		}

		public static GenerationResult UnknownSpecial3( GenerationContext context, int pass, int address, uint code, byte rt, byte rd, byte function, ushort bshfl )
		{
			return GenerationResult.Invalid;
		}

		#endregion

		public static void EmitLoadRegister( GenerationContext context, int reg )
		{
			if( reg != 0 )
			{
				//int map = context.Registers[ reg ];
				//context.ILGen.Emit( OpCodes.Ldloc_S, ( byte )map );
				context.ILGen.Emit( OpCodes.Ldarg_2 );
				context.ILGen.Emit( OpCodes.Ldc_I4, reg );
				context.ILGen.Emit( OpCodes.Ldelem_I4 );
			}
			else
				context.ILGen.Emit( OpCodes.Ldc_I4_0 );
		}

		public static void EmitLoadRegister( GenerationContext context, int reg, int mask )
		{
			if( reg != 0 )
			{
				//int map = context.Registers[ reg ];
				//context.ILGen.Emit( OpCodes.Ldloc_S, ( byte )map );
				context.ILGen.Emit( OpCodes.Ldarg_2 );
				context.ILGen.Emit( OpCodes.Ldc_I4, reg );
				context.ILGen.Emit( OpCodes.Ldelem_I4 );

				context.ILGen.Emit( OpCodes.Ldc_I4, mask );
				context.ILGen.Emit( OpCodes.And );
			}
			else
				context.ILGen.Emit( OpCodes.Ldc_I4_0 );
		}

		public static void EmitStoreRegister( GenerationContext context, int reg )
		{
			if( reg != 0 )
			{
				//int map = context.Registers[ reg ];
				//context.ILGen.Emit( OpCodes.Stloc_S, ( byte )map );
				context.ILGen.Emit( OpCodes.Stloc_1 );
				context.ILGen.Emit( OpCodes.Ldarg_2 );
				context.ILGen.Emit( OpCodes.Ldc_I4, reg );
				context.ILGen.Emit( OpCodes.Ldloc_1 );
				context.ILGen.Emit( OpCodes.Stelem_I4 );
			}
			else
				context.ILGen.Emit( OpCodes.Pop );
		}

		public static void EmitAddressTranslation( GenerationContext context )
		{
			// This is here to check for crazy kernel addresses or other things
			// - technically we shouldn't have to do this (on a real PSP they
			// would cause crashes), but the pspdev stuff sometimes does weird
			// things

			//if( ( address & 0x80000000 ) != 0 )
			//    address = address & 0x7FFFFFFF;
			//if( ( address & 0x40000000 ) != 0 )
			//    address = address & 0x3FFFFFFF;

			context.ILGen.Emit( OpCodes.Ldc_I4, 0x3FFFFFFF );
			context.ILGen.Emit( OpCodes.And );
		}

		public static void EmitDirectMemoryRead( GenerationContext context )
		{
			// If the address is within main memory bounds, we do a direct read on it
			// If it's not, pass to the nromal one
			// Stack: Memory / Address

			Label l1 = context.ILGen.DefineLabel();
			Label l2 = context.ILGen.DefineLabel();
			Label l3 = context.ILGen.DefineLabel();

			//if( ( address >= 0x08000000 ) && ( address < 0x09FFFFFF ) )
			context.ILGen.Emit( OpCodes.Dup );
			context.ILGen.Emit( OpCodes.Dup );
			context.ILGen.Emit( OpCodes.Ldc_I4, 0x08000000 );
			context.ILGen.Emit( OpCodes.Blt, l1 );
			context.ILGen.Emit( OpCodes.Ldc_I4, 0x09FFFFFF );
			context.ILGen.Emit( OpCodes.Bgt, l2 );

			// Address is within main memory range
			// Stack: Memory / Translated address
			context.ILGen.Emit( OpCodes.Ldc_I4, 0x08000000 );
			context.ILGen.Emit( OpCodes.Sub );
			context.ILGen.Emit( OpCodes.Stloc_0 );

			context.ILGen.Emit( OpCodes.Ldfld, context.MemoryMainBuffer );
			context.ILGen.Emit( OpCodes.Ldloc_0 );
			context.ILGen.Emit( OpCodes.Ldelema, typeof( byte ) );
			context.ILGen.Emit( OpCodes.Ldind_I4 );
			context.ILGen.Emit( OpCodes.Br, l3 );

			context.ILGen.MarkLabel( l1 );
			context.ILGen.Emit( OpCodes.Pop );
			context.ILGen.MarkLabel( l2 );

			// Address is not within main memory range
			// Stack: Memory / Translated address

			context.ILGen.Emit( OpCodes.Callvirt, context.MemoryReadWord );

			context.ILGen.MarkLabel( l3 );
		}

		public static void EmitDirectMemoryReadByte( GenerationContext context )
		{
			// If the address is within main memory bounds, we do a direct read on it
			// If it's not, pass to the nromal one
			// Stack: Memory / Address

			Label l1 = context.ILGen.DefineLabel();
			Label l2 = context.ILGen.DefineLabel();
			Label l3 = context.ILGen.DefineLabel();

			//if( ( address >= 0x08000000 ) && ( address < 0x09FFFFFF ) )
			context.ILGen.Emit( OpCodes.Dup );
			context.ILGen.Emit( OpCodes.Dup );
			context.ILGen.Emit( OpCodes.Ldc_I4, 0x08000000 );
			context.ILGen.Emit( OpCodes.Blt, l1 );
			context.ILGen.Emit( OpCodes.Ldc_I4, 0x09FFFFFF );
			context.ILGen.Emit( OpCodes.Bgt, l2 );

			// Address is within main memory range
			// Stack: Memory / Translated address
			context.ILGen.Emit( OpCodes.Ldc_I4, 0x08000000 );
			context.ILGen.Emit( OpCodes.Sub );
			context.ILGen.Emit( OpCodes.Stloc_0 );

			context.ILGen.Emit( OpCodes.Ldfld, context.MemoryMainBuffer );
			context.ILGen.Emit( OpCodes.Ldloc_0 );
			context.ILGen.Emit( OpCodes.Ldelem_U1 );
			context.ILGen.Emit( OpCodes.Br, l3 );

			context.ILGen.MarkLabel( l1 );
			context.ILGen.Emit( OpCodes.Pop );
			context.ILGen.MarkLabel( l2 );

			// Address is not within main memory range
			// Stack: Memory / Translated address

			context.ILGen.Emit( OpCodes.Callvirt, context.MemoryReadWord );

			context.ILGen.MarkLabel( l3 );
		}

		public static void EmitDirectMemoryWrite( GenerationContext context )
		{
			// If the address is within main memory bounds, we do a direct read on it
			// If it's not, pass to the nromal one
			// Stack: Memory / Address / Value

			Label l1 = context.ILGen.DefineLabel();
			Label l2 = context.ILGen.DefineLabel();
			Label l3 = context.ILGen.DefineLabel();

			context.ILGen.Emit( OpCodes.Stloc_1 );

			//if( ( address >= 0x08000000 ) && ( address < 0x09FFFFFF ) )
			context.ILGen.Emit( OpCodes.Dup );
			context.ILGen.Emit( OpCodes.Dup );
			context.ILGen.Emit( OpCodes.Ldc_I4, 0x08000000 );
			context.ILGen.Emit( OpCodes.Blt, l1 );
			context.ILGen.Emit( OpCodes.Ldc_I4, 0x09FFFFFF );
			context.ILGen.Emit( OpCodes.Bgt, l2 );

			// Address is within main memory range
			// Stack: Memory / Translated address
			context.ILGen.Emit( OpCodes.Ldc_I4, 0x08000000 );
			context.ILGen.Emit( OpCodes.Sub );
			context.ILGen.Emit( OpCodes.Stloc_0 );

			context.ILGen.Emit( OpCodes.Ldfld, context.MemoryMainBuffer );
			context.ILGen.Emit( OpCodes.Ldloc_0 );
			context.ILGen.Emit( OpCodes.Ldelema, typeof( byte ) );
			context.ILGen.Emit( OpCodes.Ldloc_1 );
			context.ILGen.Emit( OpCodes.Stind_I4 );
			context.ILGen.Emit( OpCodes.Br, l3 );

			context.ILGen.MarkLabel( l1 );
			context.ILGen.Emit( OpCodes.Pop );
			context.ILGen.MarkLabel( l2 );

			// Address is not within main memory range
			// Stack: Memory / Translated address

			context.ILGen.Emit( OpCodes.Ldc_I4_4 );
			context.ILGen.Emit( OpCodes.Ldloc_1 );
			context.ILGen.Emit( OpCodes.Callvirt, context.MemoryWriteWord );

			context.ILGen.MarkLabel( l3 );
		}

		public static void EmitNop( GenerationContext context )
		{
		}

		public static void DefineBranchTarget( GenerationContext context, int target )
		{
			if( context.BranchLabels.ContainsKey( target ) == false )
			{
				Debug.WriteLine( string.Format( "Defining branch target {0:X8}", target ) );
				LabelMarker lm = new LabelMarker( target );
				context.BranchLabels.Add( target, lm );
			}
			if( context.LastBranchTarget < target )
				context.LastBranchTarget = target;
		}

		#region Arithmetic

		public static class Arithmetic
		{
			public static GenerationResult SLL( GenerationContext context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
			{
				if( pass == 0 )
				{
					context.ReadRegisters[ rt ] = true;
					context.ReadRegisters[ rt ] = true;
					context.WriteRegisters[ rd ] = true;
				}
				else if( pass == 1 )
				{
					EmitLoadRegister( context, rt );
					context.ILGen.Emit( OpCodes.Ldc_I4, ( int )shamt );
					context.ILGen.Emit( OpCodes.Shl );
					EmitStoreRegister( context, rd );
					//rd.Value = rt << shamt;
				}
				return GenerationResult.Success;
			}

			public static GenerationResult SRL( GenerationContext context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
			{
				if( pass == 0 )
				{
					context.ReadRegisters[ rt ] = true;
					context.WriteRegisters[ rd ] = true;
				}
				else if( pass == 1 )
				{
					EmitLoadRegister( context, rt );
					context.ILGen.Emit( OpCodes.Ldc_I4, ( int )shamt );
					context.ILGen.Emit( OpCodes.Shr_Un );
					EmitStoreRegister( context, rd );
					//rd.Value = rt >> shamt;
				}
				return GenerationResult.Success;
			}

			public static GenerationResult SRA( GenerationContext context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
			{
				if( pass == 0 )
				{
					context.ReadRegisters[ rt ] = true;
					context.WriteRegisters[ rd ] = true;
				}
				else if( pass == 1 )
				{
					EmitLoadRegister( context, rt );
					context.ILGen.Emit( OpCodes.Ldc_I4, ( int )shamt );
					context.ILGen.Emit( OpCodes.Shr );
					EmitStoreRegister( context, rd );
					//rd.Value = ( int )rt >> shamt;
				}
				return GenerationResult.Success;
			}

			public static GenerationResult SLLV( GenerationContext context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
			{
				if( pass == 0 )
				{
					context.ReadRegisters[ rs ] = true;
					context.ReadRegisters[ rt ] = true;
					context.WriteRegisters[ rd ] = true;
				}
				else if( pass == 1 )
				{
					EmitLoadRegister( context, rt );
					EmitLoadRegister( context, rs, 0x1F );
					context.ILGen.Emit( OpCodes.Shl );
					EmitStoreRegister( context, rd );
					//rd.Value = rt << ( byte )( rs & 0x1F );
				}
				return GenerationResult.Success;
			}

			public static GenerationResult SRLV( GenerationContext context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
			{
				if( pass == 0 )
				{
					context.ReadRegisters[ rs ] = true;
					context.ReadRegisters[ rt ] = true;
					context.WriteRegisters[ rd ] = true;
				}
				else if( pass == 1 )
				{
					EmitLoadRegister( context, rt );
					EmitLoadRegister( context, rs, 0x1F );
					context.ILGen.Emit( OpCodes.Shr_Un );
					EmitStoreRegister( context, rd );
					//rd.Value = rt >> ( byte )( rs & 0x1F );
				}
				return GenerationResult.Success;
			}

			public static GenerationResult SRAV( GenerationContext context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
			{
				if( pass == 0 )
				{
					context.ReadRegisters[ rs ] = true;
					context.ReadRegisters[ rt ] = true;
					context.WriteRegisters[ rd ] = true;
				}
				else if( pass == 1 )
				{
					EmitLoadRegister( context, rt );
					EmitLoadRegister( context, rs, 0x1F );
					context.ILGen.Emit( OpCodes.Shr );
					EmitStoreRegister( context, rd );
					//rd.Value = rt >> ( byte )( rs & 0x1F );
				}
				return GenerationResult.Success;
			}

			public static GenerationResult MFHI( GenerationContext context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
			{
				if( pass == 0 )
				{
					context.WriteRegisters[ rd ] = true;
					context.UseCore = true;
				}
				else if( pass == 1 )
				{
					context.ILGen.Emit( OpCodes.Ldarg_0 );
					context.ILGen.Emit( OpCodes.Ldfld, context.Core0Hi );
					EmitStoreRegister( context, rd );
					//rd.Value = context.Core0.Hi;
				}
				return GenerationResult.Success;
			}

			public static GenerationResult MTHI( GenerationContext context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
			{
				if( pass == 0 )
				{
					context.ReadRegisters[ rs ] = true;
					context.UseCore = true;
				}
				else if( pass == 1 )
				{
					context.ILGen.Emit( OpCodes.Ldarg_0 );
					EmitLoadRegister( context, rs );
					context.ILGen.Emit( OpCodes.Stfld, context.Core0Hi );
					//context.Hi.Value = rs;
				}
				return GenerationResult.Success;
			}

			public static GenerationResult MFLO( GenerationContext context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
			{
				if( pass == 0 )
				{
					context.WriteRegisters[ rd ] = true;
					context.UseCore = true;
				}
				else if( pass == 1 )
				{
					context.ILGen.Emit( OpCodes.Ldarg_0 );
					context.ILGen.Emit( OpCodes.Ldfld, context.Core0Lo );
					EmitStoreRegister( context, rd );
					//rd.Value = context.Lo;
				}
				return GenerationResult.Success;
			}

			public static GenerationResult MTLO( GenerationContext context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
			{
				if( pass == 0 )
				{
					context.ReadRegisters[ rs ] = true;
					context.UseCore = true;
				}
				else if( pass == 1 )
				{
					context.ILGen.Emit( OpCodes.Ldarg_0 );
					EmitLoadRegister( context, rs );
					context.ILGen.Emit( OpCodes.Stfld, context.Core0Lo );
					//context.Lo.Value = rs;
				}
				return GenerationResult.Success;
			}

			public static GenerationResult MULT( GenerationContext context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
			{
				if( pass == 0 )
				{
					context.ReadRegisters[ rs ] = true;
					context.ReadRegisters[ rt ] = true;
					context.UseCore = true;
				}
				else if( pass == 1 )
				{
					context.ILGen.Emit( OpCodes.Ldarg_0 );

					EmitLoadRegister( context, rs );
					context.ILGen.Emit( OpCodes.Conv_I8 );
					EmitLoadRegister( context, rt );
					context.ILGen.Emit( OpCodes.Conv_I8 );
					context.ILGen.Emit( OpCodes.Mul );

					context.ILGen.Emit( OpCodes.Dup );
					context.ILGen.Emit( OpCodes.Conv_I4 );
					context.ILGen.Emit( OpCodes.Stloc_0 );

					context.ILGen.Emit( OpCodes.Ldc_I4, 32 );
					context.ILGen.Emit( OpCodes.Shr );
					context.ILGen.Emit( OpCodes.Conv_I4 );
					context.ILGen.Emit( OpCodes.Stfld, context.Core0Lo );

					context.ILGen.Emit( OpCodes.Ldarg_0 );
					context.ILGen.Emit( OpCodes.Ldloc_0 );
					context.ILGen.Emit( OpCodes.Stfld, context.Core0Hi );

					//long res = rs * rt;
					//context.Hi.Value = ( int )( res >> 32 );
					//context.Lo.Value = ( int )res;
				}
				return GenerationResult.Success;
			}

			public static GenerationResult MULTU( GenerationContext context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
			{
				if( pass == 0 )
				{
					context.ReadRegisters[ rs ] = true;
					context.ReadRegisters[ rt ] = true;
					context.UseCore = true;
				}
				else if( pass == 1 )
				{
					context.ILGen.Emit( OpCodes.Ldarg_0 );

					EmitLoadRegister( context, rs );
					context.ILGen.Emit( OpCodes.Conv_U8 );
					EmitLoadRegister( context, rt );
					context.ILGen.Emit( OpCodes.Conv_U8 );
					context.ILGen.Emit( OpCodes.Mul_Ovf_Un );

					context.ILGen.Emit( OpCodes.Dup );
					context.ILGen.Emit( OpCodes.Conv_I4 );
					context.ILGen.Emit( OpCodes.Stloc_0 );

					context.ILGen.Emit( OpCodes.Ldc_I4, 32 );
					context.ILGen.Emit( OpCodes.Shr_Un );
					context.ILGen.Emit( OpCodes.Conv_I4 );
					context.ILGen.Emit( OpCodes.Stfld, context.Core0Lo );

					context.ILGen.Emit( OpCodes.Ldarg_0 );
					context.ILGen.Emit( OpCodes.Ldloc_0 );
					context.ILGen.Emit( OpCodes.Stfld, context.Core0Hi );

					//long res = rs * rt;
					//context.Hi.Value = ( int )( res >> 32 );
					//context.Lo.Value = ( int )res;
				}
				return GenerationResult.Success;
			}

			public static GenerationResult DIV( GenerationContext context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
			{
				if( pass == 0 )
				{
					context.ReadRegisters[ rs ] = true;
					context.ReadRegisters[ rt ] = true;
					context.UseCore = true;
				}
				else if( pass == 1 )
				{
					EmitLoadRegister( context, rs );
					context.ILGen.Emit( OpCodes.Stloc_0 );
					EmitLoadRegister( context, rt );
					context.ILGen.Emit( OpCodes.Stloc_1 );

					context.ILGen.Emit( OpCodes.Ldarg_0 );
					context.ILGen.Emit( OpCodes.Ldloc_0 );
					context.ILGen.Emit( OpCodes.Ldloc_1 );
					context.ILGen.Emit( OpCodes.Div );
					context.ILGen.Emit( OpCodes.Stfld, context.Core0Lo );

					context.ILGen.Emit( OpCodes.Ldarg_0 );
					context.ILGen.Emit( OpCodes.Ldloc_0 );
					context.ILGen.Emit( OpCodes.Ldloc_1 );
					context.ILGen.Emit( OpCodes.Rem );
					context.ILGen.Emit( OpCodes.Stfld, context.Core0Hi );

					//long rem;
					//context.Lo.Value = Math.DivRem( rs, rt, out rem );
					//context.Hi.Value = rem;
				}
				return GenerationResult.Success;
			}

			public static GenerationResult DIVU( GenerationContext context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
			{
				if( pass == 0 )
				{
					context.ReadRegisters[ rs ] = true;
					context.ReadRegisters[ rt ] = true;
					context.UseCore = true;
				}
				else if( pass == 1 )
				{
					EmitLoadRegister( context, rs );
					context.ILGen.Emit( OpCodes.Stloc_0 );
					EmitLoadRegister( context, rt );
					context.ILGen.Emit( OpCodes.Stloc_1 );

					context.ILGen.Emit( OpCodes.Ldarg_0 );
					context.ILGen.Emit( OpCodes.Ldloc_0 );
					context.ILGen.Emit( OpCodes.Conv_U4 );
					context.ILGen.Emit( OpCodes.Ldloc_1 );
					context.ILGen.Emit( OpCodes.Conv_U4 );
					context.ILGen.Emit( OpCodes.Div_Un );
					context.ILGen.Emit( OpCodes.Stfld, context.Core0Lo );

					context.ILGen.Emit( OpCodes.Ldarg_0 );
					context.ILGen.Emit( OpCodes.Ldloc_0 );
					context.ILGen.Emit( OpCodes.Conv_U4 );
					context.ILGen.Emit( OpCodes.Ldloc_1 );
					context.ILGen.Emit( OpCodes.Conv_U4 );
					context.ILGen.Emit( OpCodes.Rem_Un );
					context.ILGen.Emit( OpCodes.Stfld, context.Core0Hi );

					//long rem;
					//context.Lo.Value = Math.DivRem( rs, rt, out rem );
					//context.Hi.Value = rem;
				}
				return GenerationResult.Success;
			}

			public static GenerationResult ADD( GenerationContext context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
			{
				if( pass == 0 )
				{
					context.ReadRegisters[ rs ] = true;
					context.ReadRegisters[ rt ] = true;
					context.WriteRegisters[ rd ] = true;
				}
				else if( pass == 1 )
				{
					EmitLoadRegister( context, rs );
					EmitLoadRegister( context, rt );
					context.ILGen.Emit( OpCodes.Add );
					EmitStoreRegister( context, rd );
					//rd.Value = rs + rt;
				}
				return GenerationResult.Success;
			}

			public static GenerationResult ADDU( GenerationContext context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
			{
				if( pass == 0 )
				{
					context.ReadRegisters[ rs ] = true;
					context.ReadRegisters[ rt ] = true;
					context.WriteRegisters[ rd ] = true;
				}
				else if( pass == 1 )
				{
					EmitLoadRegister( context, rs );
					EmitLoadRegister( context, rt );
					context.ILGen.Emit( OpCodes.Add );
					EmitStoreRegister( context, rd );
					//rd.Value = rs + rt;
				}
				return GenerationResult.Success;
			}

			public static GenerationResult SUB( GenerationContext context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
			{
				if( pass == 0 )
				{
					context.ReadRegisters[ rs ] = true;
					context.ReadRegisters[ rt ] = true;
					context.WriteRegisters[ rd ] = true;
				}
				else if( pass == 1 )
				{
					EmitLoadRegister( context, rs );
					EmitLoadRegister( context, rt );
					context.ILGen.Emit( OpCodes.Sub );
					EmitStoreRegister( context, rd );
					//rd.Value = rs - rt;
				}
				return GenerationResult.Success;
			}

			public static GenerationResult SUBU( GenerationContext context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
			{
				if( pass == 0 )
				{
					context.ReadRegisters[ rs ] = true;
					context.ReadRegisters[ rt ] = true;
					context.WriteRegisters[ rd ] = true;
				}
				else if( pass == 1 )
				{
					EmitLoadRegister( context, rs );
					EmitLoadRegister( context, rt );
					context.ILGen.Emit( OpCodes.Sub );
					EmitStoreRegister( context, rd );
					//rd.Value = rs - rt;
				}
				return GenerationResult.Success;
			}

			public static GenerationResult AND( GenerationContext context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
			{
				if( pass == 0 )
				{
					context.ReadRegisters[ rs ] = true;
					context.ReadRegisters[ rt ] = true;
					context.WriteRegisters[ rd ] = true;
				}
				else if( pass == 1 )
				{
					EmitLoadRegister( context, rs );
					EmitLoadRegister( context, rt );
					context.ILGen.Emit( OpCodes.And );
					EmitStoreRegister( context, rd );
					//rd.Value = rs & rt;
				}
				return GenerationResult.Success;
			}

			public static GenerationResult OR( GenerationContext context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
			{
				if( pass == 0 )
				{
					context.ReadRegisters[ rs ] = true;
					context.ReadRegisters[ rt ] = true;
					context.WriteRegisters[ rd ] = true;
				}
				else if( pass == 1 )
				{
					EmitLoadRegister( context, rs );
					EmitLoadRegister( context, rt );
					context.ILGen.Emit( OpCodes.Or );
					EmitStoreRegister( context, rd );
					//rd.Value = rs | rt;
				}
				return GenerationResult.Success;
			}

			public static GenerationResult XOR( GenerationContext context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
			{
				if( pass == 0 )
				{
					context.ReadRegisters[ rs ] = true;
					context.ReadRegisters[ rt ] = true;
					context.WriteRegisters[ rd ] = true;
				}
				else if( pass == 1 )
				{
					EmitLoadRegister( context, rs );
					EmitLoadRegister( context, rt );
					context.ILGen.Emit( OpCodes.Xor );
					EmitStoreRegister( context, rd );
					//rd.Value = rs ^ rt;
				}
				return GenerationResult.Success;
			}

			public static GenerationResult NOR( GenerationContext context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
			{
				if( pass == 0 )
				{
					context.ReadRegisters[ rs ] = true;
					context.ReadRegisters[ rt ] = true;
					context.WriteRegisters[ rd ] = true;
				}
				else if( pass == 1 )
				{
					EmitLoadRegister( context, rs );
					EmitLoadRegister( context, rt );
					context.ILGen.Emit( OpCodes.Or );
					context.ILGen.Emit( OpCodes.Not );
					EmitStoreRegister( context, rd );
					//rd.Value = ~( rs | rt );
				}
				return GenerationResult.Success;
			}

			public static GenerationResult SLT( GenerationContext context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
			{
				if( pass == 0 )
				{
					context.ReadRegisters[ rs ] = true;
					context.ReadRegisters[ rt ] = true;
					context.WriteRegisters[ rd ] = true;
				}
				else if( pass == 1 )
				{
					EmitLoadRegister( context, rs );
					EmitLoadRegister( context, rt );
					context.ILGen.Emit( OpCodes.Clt );
					EmitStoreRegister( context, rd );
					//rd.Value = ( rs < rt ) ? 1 : 0;
				}
				return GenerationResult.Success;
			}

			public static GenerationResult SLTU( GenerationContext context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
			{
				if( pass == 0 )
				{
					context.ReadRegisters[ rs ] = true;
					context.ReadRegisters[ rt ] = true;
					context.WriteRegisters[ rd ] = true;
				}
				else if( pass == 1 )
				{
					EmitLoadRegister( context, rs );
					EmitLoadRegister( context, rt );
					context.ILGen.Emit( OpCodes.Clt_Un );
					EmitStoreRegister( context, rd );
					//rd.Value = ( rs < rt ) ? 1 : 0;
				}
				return GenerationResult.Success;
			}

			public static GenerationResult ADDI( GenerationContext context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
			{
				if( pass == 0 )
				{
					context.ReadRegisters[ rs ] = true;
					context.WriteRegisters[ rt ] = true;
				}
				else if( pass == 1 )
				{
					EmitLoadRegister( context, rs );
					context.ILGen.Emit( OpCodes.Ldc_I4, ( int )( short )imm );
					context.ILGen.Emit( OpCodes.Add );
					EmitStoreRegister( context, rt );
					//rt.Value = rs + ( short )imm;
				}
				return GenerationResult.Success;
			}

			public static GenerationResult ADDIU( GenerationContext context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
			{
				if( pass == 0 )
				{
					context.ReadRegisters[ rs ] = true;
					context.WriteRegisters[ rt ] = true;
				}
				else if( pass == 1 )
				{
					EmitLoadRegister( context, rs );
					context.ILGen.Emit( OpCodes.Ldc_I4, ( int )( short )imm );
					context.ILGen.Emit( OpCodes.Add );
					EmitStoreRegister( context, rt );
					//rt.Value = rs + ( short )imm;
				}
				return GenerationResult.Success;
			}

			public static GenerationResult SLTI( GenerationContext context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
			{
				if( pass == 0 )
				{
					context.ReadRegisters[ rs ] = true;
					context.WriteRegisters[ rt ] = true;
				}
				else if( pass == 1 )
				{
					EmitLoadRegister( context, rs );
					context.ILGen.Emit( OpCodes.Ldc_I4, ( int )( short )imm );
					context.ILGen.Emit( OpCodes.Clt );
					EmitStoreRegister( context, rt );
					//rt.Value = ( rs < ( short )imm ) ? 1 : 0;
				}
				return GenerationResult.Success;
			}

			public static GenerationResult SLTIU( GenerationContext context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
			{
				if( pass == 0 )
				{
					context.ReadRegisters[ rs ] = true;
					context.WriteRegisters[ rt ] = true;
				}
				else if( pass == 1 )
				{
					EmitLoadRegister( context, rs );
					context.ILGen.Emit( OpCodes.Ldc_I4, ( int )( short )imm );
					context.ILGen.Emit( OpCodes.Clt_Un );
					EmitStoreRegister( context, rt );
					//rt.Value = ( rs < imm ) ? 1 : 0;
				}
				return GenerationResult.Success;
			}

			public static GenerationResult ANDI( GenerationContext context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
			{
				if( pass == 0 )
				{
					context.ReadRegisters[ rs ] = true;
					context.WriteRegisters[ rt ] = true;
				}
				else if( pass == 1 )
				{
					EmitLoadRegister( context, rs );
					context.ILGen.Emit( OpCodes.Ldc_I4, ( int )( uint )imm );
					context.ILGen.Emit( OpCodes.And );
					EmitStoreRegister( context, rt );
					//rt.Value = rs & imm;
				}
				return GenerationResult.Success;
			}

			public static GenerationResult ORI( GenerationContext context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
			{
				if( pass == 0 )
				{
					context.ReadRegisters[ rs ] = true;
					context.WriteRegisters[ rt ] = true;
				}
				else if( pass == 1 )
				{
					EmitLoadRegister( context, rs );
					context.ILGen.Emit( OpCodes.Ldc_I4, ( int )( uint )imm );
					context.ILGen.Emit( OpCodes.Or );
					EmitStoreRegister( context, rt );
					//rt.Value = rs | imm;
				}
				return GenerationResult.Success;
			}

			public static GenerationResult XORI( GenerationContext context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
			{
				if( pass == 0 )
				{
					context.ReadRegisters[ rs ] = true;
					context.WriteRegisters[ rt ] = true;
				}
				else if( pass == 1 )
				{
					EmitLoadRegister( context, rs );
					context.ILGen.Emit( OpCodes.Ldc_I4, ( int )( uint )imm );
					context.ILGen.Emit( OpCodes.Xor );
					EmitStoreRegister( context, rt );
					//rt.Value = rs ^ imm;
				}
				return GenerationResult.Success;
			}

			public static GenerationResult LUI( GenerationContext context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
			{
				if( pass == 0 )
				{
					context.WriteRegisters[ rt ] = true;
				}
				else if( pass == 1 )
				{
					context.ILGen.Emit( OpCodes.Ldc_I4, ( int )( ( uint )imm << 16 ) );
					EmitStoreRegister( context, rt );
					//rt.Value = imm << 16;
				}
				return GenerationResult.Success;
			}

			public static GenerationResult MOVZ( GenerationContext context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
			{
				if( pass == 0 )
				{
					context.ReadRegisters[ rs ] = true;
					context.ReadRegisters[ rt ] = true;
					context.WriteRegisters[ rd ] = true;
				}
				else if( pass == 1 )
				{
					Label l1 = context.ILGen.DefineLabel();

					EmitLoadRegister( context, rt );
					context.ILGen.Emit( OpCodes.Ldc_I4_0 );
					context.ILGen.Emit( OpCodes.Bne_Un, l1 );
					EmitLoadRegister( context, rs );
					EmitStoreRegister( context, rd );
					context.ILGen.MarkLabel( l1 );
					//if( reg( rt ) == 0 )
					//	storereg( rd, reg( rs ) );
				}
				return GenerationResult.Success;
			}

			public static GenerationResult MOVN( GenerationContext context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
			{
				if( pass == 0 )
				{
					context.ReadRegisters[ rs ] = true;
					context.ReadRegisters[ rt ] = true;
					context.WriteRegisters[ rd ] = true;
				}
				else if( pass == 1 )
				{
					Label l1 = context.ILGen.DefineLabel();
					EmitLoadRegister( context, rt );
					context.ILGen.Emit( OpCodes.Ldc_I4_0 );
					context.ILGen.Emit( OpCodes.Beq, l1 );
					EmitLoadRegister( context, rs );
					EmitStoreRegister( context, rd );
					context.ILGen.MarkLabel( l1 );
					//if( reg( rt ) != 0 )
					//	storereg( rd, reg( rs ) );
				}
				return GenerationResult.Success;
			}

			public static GenerationResult MIN( GenerationContext context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
			{
				if( pass == 0 )
				{
					context.ReadRegisters[ rs ] = true;
					context.ReadRegisters[ rt ] = true;
					context.WriteRegisters[ rd ] = true;
				}
				else if( pass == 1 )
				{
					//Debug.WriteLine( "would have done MIN, but instead just completely ignored :)" );
					//return GenerationResult.Success;
					Label l1 = context.ILGen.DefineLabel();
					Label l2 = context.ILGen.DefineLabel();
					EmitLoadRegister( context, rs );
					EmitLoadRegister( context, rt );
					context.ILGen.Emit( OpCodes.Blt, l1 );
					EmitLoadRegister( context, rt );
					EmitStoreRegister( context, rd );
					context.ILGen.Emit( OpCodes.Br, l2 );
					context.ILGen.MarkLabel( l1 );
					EmitLoadRegister( context, rs );
					EmitStoreRegister( context, rd );
					context.ILGen.MarkLabel( l2 );
					//if( reg( rt ) >= reg( rs ) )
					//    storereg( rd, rs );
					//else
					//    storereg( rd, rt );
				}
				return GenerationResult.Success;
			}

			public static GenerationResult SEB( GenerationContext context, int pass, int address, uint code, byte rt, byte rd, byte function, ushort bshfl )
			{
				if( pass == 0 )
				{
					context.ReadRegisters[ rt ] = true;
					context.WriteRegisters[ rd ] = true;
				}
				else if( pass == 1 )
				{
					EmitLoadRegister( context, rt );
					context.ILGen.Emit( OpCodes.Conv_I1 );
					context.ILGen.Emit( OpCodes.Conv_I4 );
					EmitStoreRegister( context, rd );
					//storereg( rd, signextend( reg( rt ) ) );
				}
				return GenerationResult.Success;
			}

			public static GenerationResult SEH( GenerationContext context, int pass, int address, uint code, byte rt, byte rd, byte function, ushort bshfl )
			{
				if( pass == 0 )
				{
					context.ReadRegisters[ rt ] = true;
					context.WriteRegisters[ rd ] = true;
				}
				else if( pass == 1 )
				{
					EmitLoadRegister( context, rt );
					context.ILGen.Emit( OpCodes.Conv_I2 );
					context.ILGen.Emit( OpCodes.Conv_I4 );
					EmitStoreRegister( context, rd );
					//storereg( rd, signextend( reg( rt ) ) );
				}
				return GenerationResult.Success;
			}
		}
		#endregion

		#region Memory

		public static class Memory
		{
			// Counters to help track the usage of the different widths
			// So far, 4-byte seems to be the most popular, which means it gets the love
			public static int LBC = 0;
			public static int LHC = 0;
			public static int LWC = 0;
			public static int SBC = 0;
			public static int SHC = 0;
			public static int SWC = 0;

			public static GenerationResult LB( GenerationContext context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
			{
				//long address = rs + ( short ) imm;
				//rt.Value = context.Simulation.Memory.ReadByte( physical.Value );

				if( pass == 0 )
				{
					context.ReadRegisters[ rs ] = true;
					context.WriteRegisters[ rt ] = true;
					context.UseMemory = true;
					LBC++;
				}
				else if( pass == 1 )
				{
					context.ILGen.Emit( OpCodes.Ldarg_1 );

					EmitLoadRegister( context, rs );
					context.ILGen.Emit( OpCodes.Ldc_I4, ( int )( short )imm );
					context.ILGen.Emit( OpCodes.Add );
					EmitAddressTranslation( context );

					EmitDirectMemoryReadByte( context );

					EmitStoreRegister( context, rt );
				}
				return GenerationResult.Success;
			}

			public static GenerationResult LH( GenerationContext context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
			{
				//long address = rs + ( short ) imm;
				//rt.Value = context.Simulation.Memory.ReadInt16( physical.Value );

				if( pass == 0 )
				{
					context.ReadRegisters[ rs ] = true;
					context.WriteRegisters[ rt ] = true;
					context.UseMemory = true;
					LHC++;
				}
				else if( pass == 1 )
				{
					context.ILGen.Emit( OpCodes.Ldarg_1 );

					EmitLoadRegister( context, rs );
					context.ILGen.Emit( OpCodes.Ldc_I4, ( int )( short )imm );
					context.ILGen.Emit( OpCodes.Add );
					EmitAddressTranslation( context );

					EmitDirectMemoryRead( context );

					context.ILGen.Emit( OpCodes.Ldc_I4, 0x0000FFFF );
					context.ILGen.Emit( OpCodes.And );
					context.ILGen.Emit( OpCodes.Conv_I2 );
					context.ILGen.Emit( OpCodes.Conv_I4 );

					EmitStoreRegister( context, rt );
				}
				return GenerationResult.Success;
			}

			public static GenerationResult LWL( GenerationContext context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
			{
				//case B8(0100010):		// lwl rt, offset(base)
				//    t = signextend16( imm ) + reg( rs );
				//    v = loadword( t );
				//    t %= 4;
				//    v <<= ( 8 * ( 3 - t ) );
				//    u = 0xFFFFFFFF >> ( 8 * ( t + 1 ) );
				//    t = reg( rt ) & u;
				//    storereg( rt, t | v );
				//    // TODO: ensure lwl right
				//    break;
				// TODO: LWL
				return GenerationResult.Invalid;
			}

			public static GenerationResult LW( GenerationContext context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
			{
				//long address = rs + ( short ) imm;
				//rt.Value = context.Simulation.Memory.ReadInt32( physical.Value );

				if( pass == 0 )
				{
					context.ReadRegisters[ rs ] = true;
					context.WriteRegisters[ rt ] = true;
					context.UseMemory = true;
					LWC++;
				}
				else if( pass == 1 )
				{
					context.ILGen.Emit( OpCodes.Ldarg_1 );

					EmitLoadRegister( context, rs );
					context.ILGen.Emit( OpCodes.Ldc_I4, ( int )( short )imm );
					context.ILGen.Emit( OpCodes.Add );
					EmitAddressTranslation( context );

					EmitDirectMemoryRead( context );

					EmitStoreRegister( context, rt );
				}
				return GenerationResult.Success;
			}

			public static GenerationResult LBU( GenerationContext context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
			{
				//long address = rs + ( short ) imm;
				//rt.Value = context.Simulation.Memory.ReadByte( physical.Value );

				if( pass == 0 )
				{
					context.ReadRegisters[ rs ] = true;
					context.WriteRegisters[ rt ] = true;
					context.UseMemory = true;
					LBC++;
				}
				else if( pass == 1 )
				{
					context.ILGen.Emit( OpCodes.Ldarg_1 );

					EmitLoadRegister( context, rs );
					context.ILGen.Emit( OpCodes.Ldc_I4, ( int )( short )imm );
					context.ILGen.Emit( OpCodes.Add );
					EmitAddressTranslation( context );

					EmitDirectMemoryRead( context );
					
					context.ILGen.Emit( OpCodes.Ldc_I4, 0x000000FF );
					context.ILGen.Emit( OpCodes.And );
					//context.ILGen.Emit( OpCodes.Conv_U1 );

					EmitStoreRegister( context, rt );
				}
				return GenerationResult.Success;
			}

			public static GenerationResult LHU( GenerationContext context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
			{
				//long address = rs + ( short ) imm;
				//rt.Value = context.Simulation.Memory.ReadUInt16( physical.Value );

				if( pass == 0 )
				{
					context.ReadRegisters[ rs ] = true;
					context.WriteRegisters[ rt ] = true;
					context.UseMemory = true;
					LHC++;
				}
				else if( pass == 1 )
				{
					context.ILGen.Emit( OpCodes.Ldarg_1 );

					EmitLoadRegister( context, rs );
					context.ILGen.Emit( OpCodes.Ldc_I4, ( int )( short )imm );
					context.ILGen.Emit( OpCodes.Add );
					EmitAddressTranslation( context );

					EmitDirectMemoryRead( context );
					
					context.ILGen.Emit( OpCodes.Ldc_I4, 0x0000FFFF );
					context.ILGen.Emit( OpCodes.And );
					//context.ILGen.Emit( OpCodes.Conv_U2 );

					EmitStoreRegister( context, rt );
				}
				return GenerationResult.Success;
			}

			public static GenerationResult LWR( GenerationContext context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
			{
				//case B8(0100110):		// lwr rt, offset(base)
				//    t = signextend16( imm ) + reg( rs );
				//    v = loadword( t );
				//    t %= 4;
				//    v = swizzle( v );
				//    v >>= ( 8 * t );
				//    u = 0xFFFFFFFF << ( 8 * ( 4 - t ) );
				//    t = reg( rt ) & u;
				//    storereg( rt, t | v );
				//    // TODO: ensure lwr right
				//    break;
				// TODO: LWR
				return GenerationResult.Invalid;
			}

			public static GenerationResult SB( GenerationContext context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
			{
				//long address = rs + ( short ) imm;
				//context.Simulation.Memory.Write( physical.Value, ( byte ) rt.Value );

				if( pass == 0 )
				{
					context.ReadRegisters[ rs ] = true;
					context.ReadRegisters[ rt ] = true;
					context.UseMemory = true;
					SBC++;
				}
				else if( pass == 1 )
				{
					context.ILGen.Emit( OpCodes.Ldarg_1 );

					EmitLoadRegister( context, rs );
					context.ILGen.Emit( OpCodes.Ldc_I4, ( int )( short )imm );
					context.ILGen.Emit( OpCodes.Add );
					EmitAddressTranslation( context );

					context.ILGen.Emit( OpCodes.Ldc_I4_1 );

					EmitLoadRegister( context, rt );
					//context.ILGen.Emit( OpCodes.Ldc_I4, 0x000000FF );
					//context.ILGen.Emit( OpCodes.And );

					context.ILGen.Emit( OpCodes.Callvirt, context.MemoryWriteWord );
				}
				return GenerationResult.Success;
			}

			public static GenerationResult SH( GenerationContext context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
			{
				//long address = rs + ( short ) imm;
				//context.Simulation.Memory.Write( physical.Value, ( short ) rt.Value );

				if( pass == 0 )
				{
					context.ReadRegisters[ rs ] = true;
					context.ReadRegisters[ rt ] = true;
					context.UseMemory = true;
					SHC++;
				}
				else if( pass == 1 )
				{
					context.ILGen.Emit( OpCodes.Ldarg_1 );

					EmitLoadRegister( context, rs );
					context.ILGen.Emit( OpCodes.Ldc_I4, ( int )( short )imm );
					context.ILGen.Emit( OpCodes.Add );
					EmitAddressTranslation( context );

					context.ILGen.Emit( OpCodes.Ldc_I4_2 );

					EmitLoadRegister( context, rt );
					//context.ILGen.Emit( OpCodes.Ldc_I4, 0x0000FFFF );
					//context.ILGen.Emit( OpCodes.And );

					context.ILGen.Emit( OpCodes.Callvirt, context.MemoryWriteWord );
				}
				return GenerationResult.Success;
			}

			public static GenerationResult SWL( GenerationContext context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
			{
				//case B8(0101010):		// swl rt, offset(base)
				//    w = t = signextend16( imm ) + reg( rs );
				//    v = reg( rt );
				//    t %= 4;
				//    v <<= ( 8 * ( 3 - t ) );
				//    u = 0xFFFFFFFF >> ( 8 * ( t + 1 ) );
				//    t = loadword( t & 0xFFFFFFFC ) & u;
				//    storeword( w, 4, t | v );
				//    // TODO: ensure swl right
				//    break;
				// TODO: SWL
				return GenerationResult.Invalid;
			}

			public static GenerationResult SW( GenerationContext context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
			{
				//long address = rs + ( short ) imm;
				//context.Simulation.Memory.Write( physical.Value, ( int ) rt.Value );

				if( pass == 0 )
				{
					context.ReadRegisters[ rs ] = true;
					context.ReadRegisters[ rt ] = true;
					context.UseMemory = true;
					SWC++;
				}
				else if( pass == 1 )
				{
					context.ILGen.Emit( OpCodes.Ldarg_1 );

					EmitLoadRegister( context, rs );
					context.ILGen.Emit( OpCodes.Ldc_I4, ( int )( short )imm );
					context.ILGen.Emit( OpCodes.Add );
					EmitAddressTranslation( context );

					EmitLoadRegister( context, rt );
					EmitDirectMemoryWrite( context );
				}
				return GenerationResult.Success;
			}

			public static GenerationResult SWR( GenerationContext context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
			{
				//case B8(0101110):		// swr rt, offset(base)
				//    w = t = signextend16( imm ) + reg( rs );
				//    v = reg( rt );
				//    t %= 4;
				//    v = swizzle( v );
				//    v >>= ( 8 * t );
				//    u = 0xFFFFFFFF << ( 8 * ( 4 - t ) );
				//    t = loadword( t & 0xFFFFFFFC ) & u;
				//    storeword( w, 4, t | v );
				//    // TODO: ensure swr right
				//    break;
				// TODO: SWR
				return GenerationResult.Invalid;
			}

			public static GenerationResult CACHE( GenerationContext context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
			{
				return GenerationResult.Success;
			}

			public static GenerationResult LL( GenerationContext context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
			{
				//case B8(0110000):		// ll rt, offset(base)   562
				//    t = signextend16( imm ) + reg( rs );
				//    v = loadword( t );
				//    storereg( rt, v );
				//    setll( 1 );
				//    break;
				return GenerationResult.Invalid;
			}

			public static GenerationResult LWC2( GenerationContext context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
			{
				return GenerationResult.Invalid;
			}

			public static GenerationResult SC( GenerationContext context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
			{
				//case B8(0111000):		// sc rt, offset(base)   599
				//    t = signextend16( imm ) + reg( rs );
				//    if( getll() == true )
				//    {
				//        storeword( t, 4, reg( rt ) );
				//        storereg( rt, 1 );
				//    }
				//    else
				//        storereg( rt, 0 );
				//    break;
				return GenerationResult.Invalid;
			}

			public static GenerationResult SWC2( GenerationContext context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
			{
				return GenerationResult.Invalid;
			}
		}
		#endregion

		#region Control

		public static class Control
		{
			public const int AddressOffset = 0;

			public static GenerationResult JR( GenerationContext context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
			{
				bool theEnd = ( context.LastBranchTarget <= address );

				if( pass == 0 )
				{
					context.UpdatePc = true;
					context.ReadRegisters[ rs ] = true;
				}
				else if( pass == 1 )
				{
					EmitLoadRegister( context, rs );
					context.ILGen.Emit( OpCodes.Stloc_2 );
					context.ILGen.Emit( OpCodes.Ldc_I4_1 );
					context.ILGen.Emit( OpCodes.Stloc_3 );
				}
				//context.PC.Value = rs;
				return GenerationResult.Jump;
			}

			public static GenerationResult JALR( GenerationContext context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
			{
				bool theEnd = ( context.LastBranchTarget <= address );

				if( pass == 0 )
				{
					context.UpdatePc = true;
					context.ReadRegisters[ rs ] = true;
					context.WriteRegisters[ rd ] = true;
				}
				else if( pass == 1 )
				{
					context.ILGen.Emit( OpCodes.Ldc_I4, address + 4 );
					EmitStoreRegister( context, rd );
					EmitLoadRegister( context, rs );
					context.ILGen.Emit( OpCodes.Stloc_2 );
					context.ILGen.Emit( OpCodes.Ldc_I4_1 );
					context.ILGen.Emit( OpCodes.Stloc_3 );
				}
				//context.Registers[ rd ].Value = context.PC + 4;
				//context.PC.Value = rs;
				return GenerationResult.Jump;
			}

			public static GenerationResult J( GenerationContext context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
			{
				bool theEnd = ( context.LastBranchTarget <= address );

				if( pass == 0 )
				{
					context.UpdatePc = true;
				}
				else if( pass == 1 )
				{
					uint target = code & 0x3FFFFFF;
					uint pc = ( ( uint )address & 0xF0000000 ) | ( target << 2 );
					context.ILGen.Emit( OpCodes.Ldc_I4, ( int )pc );
					context.ILGen.Emit( OpCodes.Stloc_2 );
					context.ILGen.Emit( OpCodes.Ldc_I4_1 );
					context.ILGen.Emit( OpCodes.Stloc_3 );
				}
				//context.PC.Value = ( code & 0x03FFFFFF ) << 2;
				//targetPc = ( getpc() & 0xF0000000 ) | ( target << 2 );
				return GenerationResult.Jump;
			}

			public static GenerationResult JAL( GenerationContext context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
			{
				bool theEnd = ( context.LastBranchTarget <= address );

				if( pass == 0 )
				{
					context.UpdatePc = true;
					context.WriteRegisters[ 31 ] = true;
				}
				else if( pass == 1 )
				{
					context.ILGen.Emit( OpCodes.Ldc_I4, address + 4 );
					EmitStoreRegister( context, 31 );
					uint target = code & 0x3FFFFFF;
					uint pc = ( ( uint )address & 0xF0000000 ) | ( target << 2 );
					context.ILGen.Emit( OpCodes.Ldc_I4, ( int )pc );
					context.ILGen.Emit( OpCodes.Stloc_2 );
					context.ILGen.Emit( OpCodes.Ldc_I4_1 );
					context.ILGen.Emit( OpCodes.Stloc_3 );
				}
				//context.Registers[ 31 ].Value = context.PC + 4;
				//context.PC.Value = ( code & 0x03FFFFFF ) << 2;
				//targetPc = ( getpc() & 0xF0000000 ) | ( target << 2 );
				return GenerationResult.Jump;
			}

			public static GenerationResult BEQ( GenerationContext context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
			{
				int target = ( address + AddressOffset ) + ( ( int )( short )imm << 2 );

				if( pass == 0 )
				{
					context.UpdatePc = true;
					context.ReadRegisters[ rs ] = true;
					context.ReadRegisters[ rt ] = true;
					DefineBranchTarget( context, target );
				}
				else if( pass == 1 )
				{
					LabelMarker targetLabel = context.BranchLabels[ target ];
					//Debug.Assert( targetLabel != default( Label ) );
					context.BranchTarget = targetLabel;

					Label l1 = context.ILGen.DefineLabel();
					EmitLoadRegister( context, rs );
					EmitLoadRegister( context, rt );
					context.ILGen.Emit( OpCodes.Bne_Un_S, l1 );
					context.ILGen.Emit( OpCodes.Ldc_I4_1 );
					context.ILGen.Emit( OpCodes.Stloc_3 );
					context.ILGen.MarkLabel( l1 );
				}
				//-4
				//if( rs.Value == rt.Value )
				//	context.PC.Value = context.PC + ( imm << 2 );
				return GenerationResult.Branch;
			}

			public static GenerationResult BEQL( GenerationContext context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
			{
				int target = ( address + AddressOffset ) + ( ( int )( short )imm << 2 );

				if( pass == 0 )
				{
					context.UpdatePc = true;
					context.ReadRegisters[ rs ] = true;
					context.ReadRegisters[ rt ] = true;
					DefineBranchTarget( context, target );
				}
				else if( pass == 1 )
				{
					LabelMarker targetLabel = context.BranchLabels[ target ];
					//Debug.Assert( targetLabel != default( Label ) );
					context.BranchTarget = targetLabel;
					
					Label l1 = context.ILGen.DefineLabel();
					Label l2 = context.ILGen.DefineLabel();
					EmitLoadRegister( context, rs );
					EmitLoadRegister( context, rt );
					context.ILGen.Emit( OpCodes.Bne_Un, l1 );
					context.ILGen.Emit( OpCodes.Ldc_I4_1 );
					context.ILGen.Emit( OpCodes.Stloc_3 );
					context.ILGen.Emit( OpCodes.Br, l2 );
					context.ILGen.MarkLabel( l1 );
					context.ILGen.Emit( OpCodes.Ldc_I4_1 );
					context.ILGen.Emit( OpCodes.Stloc_S, ( byte )Cpu.LocalNullifyDelay );
					context.ILGen.MarkLabel( l2 );
				}
				//-4
				//if( rs.Value == rt.Value )
				//	context.PC.Value = context.PC + ( imm << 2 );
				//else
				//	null = true;
				return GenerationResult.BranchAndNullifyDelay;
			}

			public static GenerationResult BNE( GenerationContext context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
			{
				int target = ( address + AddressOffset ) + ( ( int )( short )imm << 2 );

				if( pass == 0 )
				{
					context.UpdatePc = true;
					context.ReadRegisters[ rs ] = true;
					context.ReadRegisters[ rt ] = true;
					DefineBranchTarget( context, target );
				}
				else if( pass == 1 )
				{
					LabelMarker targetLabel = context.BranchLabels[ target ];
					//Debug.Assert( targetLabel != default( Label ) );
					context.BranchTarget = targetLabel;
					
					Label l1 = context.ILGen.DefineLabel();
					EmitLoadRegister( context, rs );
					EmitLoadRegister( context, rt );
					context.ILGen.Emit( OpCodes.Beq, l1 );
					context.ILGen.Emit( OpCodes.Ldc_I4_1 );
					context.ILGen.Emit( OpCodes.Stloc_3 );
					context.ILGen.MarkLabel( l1 );
				}
				//-4
				//if( rs.Value != rt.Value )
				//	context.PC.Value = context.PC + ( imm << 2 );
				return GenerationResult.Branch;
			}

			public static GenerationResult BNEL( GenerationContext context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
			{
				int target = ( address + AddressOffset ) + ( ( int )( short )imm << 2 );

				if( pass == 0 )
				{
					context.UpdatePc = true;
					context.ReadRegisters[ rs ] = true;
					context.ReadRegisters[ rt ] = true;
					DefineBranchTarget( context, target );
				}
				else if( pass == 1 )
				{
					LabelMarker targetLabel = context.BranchLabels[ target ];
					//Debug.Assert( targetLabel != default( Label ) );
					context.BranchTarget = targetLabel;
					
					Label l1 = context.ILGen.DefineLabel();
					Label l2 = context.ILGen.DefineLabel();
					EmitLoadRegister( context, rs );
					EmitLoadRegister( context, rt );
					context.ILGen.Emit( OpCodes.Beq, l1 );
					context.ILGen.Emit( OpCodes.Ldc_I4_1 );
					context.ILGen.Emit( OpCodes.Stloc_3 );
					context.ILGen.Emit( OpCodes.Br, l2 );
					context.ILGen.MarkLabel( l1 );
					context.ILGen.Emit( OpCodes.Ldc_I4_1 );
					context.ILGen.Emit( OpCodes.Stloc_S, ( byte )Cpu.LocalNullifyDelay );
					context.ILGen.MarkLabel( l2 );
				}
				//-4
				//if( rs.Value != rt.Value )
				//	context.PC.Value = context.PC + ( imm << 2 );
				//else
				//	null = true;
				return GenerationResult.BranchAndNullifyDelay;
			}

			public static GenerationResult BLEZ( GenerationContext context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
			{
				int target = ( address + AddressOffset ) + ( ( int )( short )imm << 2 );

				if( pass == 0 )
				{
					context.UpdatePc = true;
					context.ReadRegisters[ rs ] = true;
					DefineBranchTarget( context, target );
				}
				else if( pass == 1 )
				{
					LabelMarker targetLabel = context.BranchLabels[ target ];
					//Debug.Assert( targetLabel != default( Label ) );
					context.BranchTarget = targetLabel;
					
					Label l1 = context.ILGen.DefineLabel();
					EmitLoadRegister( context, rs );
					context.ILGen.Emit( OpCodes.Ldc_I4_0 );
					context.ILGen.Emit( OpCodes.Bgt, l1 );
					context.ILGen.Emit( OpCodes.Ldc_I4_1 );
					context.ILGen.Emit( OpCodes.Stloc_3 );
					context.ILGen.MarkLabel( l1 );
				}
				//if( rs.Value <= 0 )
				//	context.PC.Value = context.PC + ( imm << 2 );
				return GenerationResult.Branch;
			}

			public static GenerationResult BLEZL( GenerationContext context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
			{
				int target = ( address + AddressOffset ) + ( ( int )( short )imm << 2 );

				if( pass == 0 )
				{
					context.UpdatePc = true;
					context.ReadRegisters[ rs ] = true;
					DefineBranchTarget( context, target );
				}
				else if( pass == 1 )
				{
					LabelMarker targetLabel = context.BranchLabels[ target ];
					//Debug.Assert( targetLabel != default( Label ) );
					context.BranchTarget = targetLabel;

					Label l1 = context.ILGen.DefineLabel();
					Label l2 = context.ILGen.DefineLabel();
					EmitLoadRegister( context, rs );
					context.ILGen.Emit( OpCodes.Ldc_I4_0 );
					context.ILGen.Emit( OpCodes.Bgt, l1 );
					context.ILGen.Emit( OpCodes.Ldc_I4_1 );
					context.ILGen.Emit( OpCodes.Stloc_3 );
					context.ILGen.Emit( OpCodes.Br, l2 );
					context.ILGen.MarkLabel( l1 );
					context.ILGen.Emit( OpCodes.Ldc_I4_1 );
					context.ILGen.Emit( OpCodes.Stloc_S, ( byte )Cpu.LocalNullifyDelay );
					context.ILGen.MarkLabel( l2 );
				}
				//if( rs.Value <= 0 )
				//	context.PC.Value = context.PC + ( imm << 2 );
				//else
				//	null = true;
				return GenerationResult.BranchAndNullifyDelay;
			}

			public static GenerationResult BGTZ( GenerationContext context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
			{
				int target = ( address + AddressOffset ) + ( ( int )( short )imm << 2 );

				if( pass == 0 )
				{
					context.UpdatePc = true;
					context.ReadRegisters[ rs ] = true;
					DefineBranchTarget( context, target );
				}
				else if( pass == 1 )
				{
					LabelMarker targetLabel = context.BranchLabels[ target ];
					//Debug.Assert( targetLabel != default( Label ) );
					context.BranchTarget = targetLabel;

					Label l1 = context.ILGen.DefineLabel();
					EmitLoadRegister( context, rs );
					context.ILGen.Emit( OpCodes.Ldc_I4_0 );
					context.ILGen.Emit( OpCodes.Ble, l1 );
					context.ILGen.Emit( OpCodes.Ldc_I4_1 );
					context.ILGen.Emit( OpCodes.Stloc_3 );
					context.ILGen.MarkLabel( l1 );
				}
				//if( rs.Value > 0 )
				//	context.PC.Value = context.PC + ( imm << 2 );
				return GenerationResult.Branch;
			}

			public static GenerationResult BGTZL( GenerationContext context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
			{
				int target = ( address + AddressOffset ) + ( ( int )( short )imm << 2 );

				if( pass == 0 )
				{
					context.UpdatePc = true;
					context.ReadRegisters[ rs ] = true;
					DefineBranchTarget( context, target );
				}
				else if( pass == 1 )
				{
					LabelMarker targetLabel = context.BranchLabels[ target ];
					//Debug.Assert( targetLabel != default( Label ) );
					context.BranchTarget = targetLabel;

					Label l1 = context.ILGen.DefineLabel();
					Label l2 = context.ILGen.DefineLabel();
					EmitLoadRegister( context, rs );
					context.ILGen.Emit( OpCodes.Ldc_I4_0 );
					context.ILGen.Emit( OpCodes.Ble, l1 );
					context.ILGen.Emit( OpCodes.Ldc_I4_1 );
					context.ILGen.Emit( OpCodes.Stloc_3 );
					context.ILGen.Emit( OpCodes.Br, l2 );
					context.ILGen.MarkLabel( l1 );
					context.ILGen.Emit( OpCodes.Ldc_I4_1 );
					context.ILGen.Emit( OpCodes.Stloc_S, ( byte )Cpu.LocalNullifyDelay );
					context.ILGen.MarkLabel( l2 );
				}
				//if( rs.Value > 0 )
				//	context.PC.Value = context.PC + ( imm << 2 );
				//else
				//	null = true;
				return GenerationResult.BranchAndNullifyDelay;
			}

			public static GenerationResult BLTZ( GenerationContext context, int pass, int address, uint code, byte opcode, uint imm )
			{
				int target = ( address + AddressOffset ) + ( ( int )( short )imm << 2 );

				int rs = ( int )( ( code >> 31 ) & 0x1F );
				if( pass == 0 )
				{
					context.UpdatePc = true;
					context.ReadRegisters[ rs ] = true;
					DefineBranchTarget( context, target );
				}
				else if( pass == 1 )
				{
					LabelMarker targetLabel = context.BranchLabels[ target ];
					//Debug.Assert( targetLabel != default( Label ) );
					context.BranchTarget = targetLabel;

					Label l1 = context.ILGen.DefineLabel();
					EmitLoadRegister( context, rs );
					//context.ILGen.Emit( OpCodes.Ldc_I4_0 );
					//context.ILGen.Emit( OpCodes.Bge, l1 );
					context.ILGen.Emit( OpCodes.Ldc_I4, 31 );
					context.ILGen.Emit( OpCodes.Shr );
					context.ILGen.Emit( OpCodes.Ldc_I4_1 );
					context.ILGen.Emit( OpCodes.Bne_Un, l1 );
					context.ILGen.Emit( OpCodes.Ldc_I4_1 );
					context.ILGen.Emit( OpCodes.Stloc_3 );
					context.ILGen.MarkLabel( l1 );
				}
				//if( rs.Value < 0 )
				//	context.PC.Value = context.PC + ( imm << 2 );
				return GenerationResult.Branch;
			}

			public static GenerationResult BGEZ( GenerationContext context, int pass, int address, uint code, byte opcode, uint imm )
			{
				int target = ( address + AddressOffset ) + ( ( int )( short )imm << 2 );

				int rs = ( int )( ( code >> 31 ) & 0x1F );
				if( pass == 0 )
				{
					context.UpdatePc = true;
					context.ReadRegisters[ rs ] = true;
					DefineBranchTarget( context, target );
				}
				else if( pass == 1 )
				{
					LabelMarker targetLabel = context.BranchLabels[ target ];
					//Debug.Assert( targetLabel != default( Label ) );
					context.BranchTarget = targetLabel;

					Label l1 = context.ILGen.DefineLabel();
					EmitLoadRegister( context, rs );
					//context.ILGen.Emit( OpCodes.Ldc_I4_0 );
					//context.ILGen.Emit( OpCodes.Blt, l1 );
					context.ILGen.Emit( OpCodes.Ldc_I4, 31 );
					context.ILGen.Emit( OpCodes.Shr );
					context.ILGen.Emit( OpCodes.Ldc_I4_0 );
					context.ILGen.Emit( OpCodes.Bne_Un, l1 );
					context.ILGen.Emit( OpCodes.Ldc_I4_1 );
					context.ILGen.Emit( OpCodes.Stloc_3 );
					context.ILGen.MarkLabel( l1 );
				}
				//if( rs.Value >= 0 )
				//	context.PC.Value = context.PC + ( imm << 2 );
				return GenerationResult.Branch;
			}

			public static GenerationResult BLTZL( GenerationContext context, int pass, int address, uint code, byte opcode, uint imm )
			{
				int target = ( address + AddressOffset ) + ( ( int )( short )imm << 2 );

				int rs = ( int )( ( code >> 31 ) & 0x1F );
				if( pass == 0 )
				{
					context.UpdatePc = true;
					context.ReadRegisters[ rs ] = true;
					DefineBranchTarget( context, target );
				}
				else if( pass == 1 )
				{
					LabelMarker targetLabel = context.BranchLabels[ target ];
					//Debug.Assert( targetLabel != default( Label ) );
					context.BranchTarget = targetLabel;

					Label l1 = context.ILGen.DefineLabel();
					Label l2 = context.ILGen.DefineLabel();
					EmitLoadRegister( context, rs );
					//context.ILGen.Emit( OpCodes.Ldc_I4_0 );
					//context.ILGen.Emit( OpCodes.Bge, l1 );
					context.ILGen.Emit( OpCodes.Ldc_I4, 31 );
					context.ILGen.Emit( OpCodes.Shr );
					context.ILGen.Emit( OpCodes.Ldc_I4_1 );
					context.ILGen.Emit( OpCodes.Bne_Un, l1 );
					context.ILGen.Emit( OpCodes.Ldc_I4_1 );
					context.ILGen.Emit( OpCodes.Stloc_3 );
					context.ILGen.Emit( OpCodes.Br, l2 );
					context.ILGen.MarkLabel( l1 );
					context.ILGen.Emit( OpCodes.Ldc_I4_1 );
					context.ILGen.Emit( OpCodes.Stloc_S, ( byte )Cpu.LocalNullifyDelay );
					context.ILGen.MarkLabel( l2 );
				}
				//if( rs.Value < 0 )
				//	context.PC.Value = context.PC + ( imm << 2 );
				//else
				//	null = true;
				return GenerationResult.BranchAndNullifyDelay;
			}

			public static GenerationResult BGEZL( GenerationContext context, int pass, int address, uint code, byte opcode, uint imm )
			{
				int target = ( address + AddressOffset ) + ( ( int )( short )imm << 2 );

				int rs = ( int )( ( code >> 31 ) & 0x1F );
				if( pass == 0 )
				{
					context.UpdatePc = true;
					context.ReadRegisters[ rs ] = true;
					DefineBranchTarget( context, target );
				}
				else if( pass == 1 )
				{
					LabelMarker targetLabel = context.BranchLabels[ target ];
					//Debug.Assert( targetLabel != default( Label ) );
					context.BranchTarget = targetLabel;

					Label l1 = context.ILGen.DefineLabel();
					Label l2 = context.ILGen.DefineLabel();
					EmitLoadRegister( context, rs );
					//context.ILGen.Emit( OpCodes.Ldc_I4_0 );
					//context.ILGen.Emit( OpCodes.Blt, l1 );
					context.ILGen.Emit( OpCodes.Ldc_I4, 31 );
					context.ILGen.Emit( OpCodes.Shr );
					context.ILGen.Emit( OpCodes.Ldc_I4_0 );
					context.ILGen.Emit( OpCodes.Bne_Un, l1 );
					context.ILGen.Emit( OpCodes.Ldc_I4_1 );
					context.ILGen.Emit( OpCodes.Stloc_3 );
					context.ILGen.Emit( OpCodes.Br, l2 );
					context.ILGen.MarkLabel( l1 );
					context.ILGen.Emit( OpCodes.Ldc_I4_1 );
					context.ILGen.Emit( OpCodes.Stloc_S, ( byte )Cpu.LocalNullifyDelay );
					context.ILGen.MarkLabel( l2 );
				}
				//if( rs.Value >= 0 )
				//	context.PC.Value = context.PC + ( imm << 2 );
				//else
				//	null = true;
				return GenerationResult.BranchAndNullifyDelay;
			}

			public static GenerationResult BLTZAL( GenerationContext context, int pass, int address, uint code, byte opcode, uint imm )
			{
				int target = ( address + AddressOffset ) + ( ( int )( short )imm << 2 );

				int rs = ( int )( ( code >> 31 ) & 0x1F );
				if( pass == 0 )
				{
					context.UpdatePc = true;
					context.ReadRegisters[ rs ] = true;
					context.WriteRegisters[ 31 ] = true;
					DefineBranchTarget( context, target );
				}
				else if( pass == 1 )
				{
					LabelMarker targetLabel = context.BranchLabels[ target ];
					//Debug.Assert( targetLabel != default( Label ) );
					context.BranchTarget = targetLabel;

					Label l1 = context.ILGen.DefineLabel();
					context.ILGen.Emit( OpCodes.Ldc_I4, address + 4 );
					EmitStoreRegister( context, 31 );
					EmitLoadRegister( context, rs );
					//context.ILGen.Emit( OpCodes.Ldc_I4_0 );
					//context.ILGen.Emit( OpCodes.Bge, l1 );
					context.ILGen.Emit( OpCodes.Ldc_I4, 31 );
					context.ILGen.Emit( OpCodes.Shr );
					context.ILGen.Emit( OpCodes.Ldc_I4_1 );
					context.ILGen.Emit( OpCodes.Bne_Un, l1 );
					context.ILGen.Emit( OpCodes.Ldc_I4_1 );
					context.ILGen.Emit( OpCodes.Stloc_3 );
					context.ILGen.MarkLabel( l1 );
				}
				//if( rs.Value < 0 )
				//{
				//	context.Registers[ 31 ].Value = context.PC + 4;
				//	context.PC.Value = context.PC + ( imm << 2 );
				//}
				return GenerationResult.Branch;
			}

			public static GenerationResult BGEZAL( GenerationContext context, int pass, int address, uint code, byte opcode, uint imm )
			{
				int target = ( address + AddressOffset ) + ( ( int )( short )imm << 2 );

				int rs = ( int )( ( code >> 31 ) & 0x1F );
				if( pass == 0 )
				{
					context.UpdatePc = true;
					context.ReadRegisters[ rs ] = true;
					context.WriteRegisters[ 31 ] = true;
					DefineBranchTarget( context, target );
				}
				else if( pass == 1 )
				{
					LabelMarker targetLabel = context.BranchLabels[ target ];
					//Debug.Assert( targetLabel != default( Label ) );
					context.BranchTarget = targetLabel;

					Label l1 = context.ILGen.DefineLabel();
					context.ILGen.Emit( OpCodes.Ldc_I4, address + 4 );
					EmitStoreRegister( context, 31 );
					EmitLoadRegister( context, rs );
					//context.ILGen.Emit( OpCodes.Ldc_I4_0 );
					//context.ILGen.Emit( OpCodes.Blt, l1 );
					context.ILGen.Emit( OpCodes.Ldc_I4, 31 );
					context.ILGen.Emit( OpCodes.Shr );
					context.ILGen.Emit( OpCodes.Ldc_I4_0 );
					context.ILGen.Emit( OpCodes.Bne_Un, l1 );
					context.ILGen.Emit( OpCodes.Ldc_I4_1 );
					context.ILGen.Emit( OpCodes.Stloc_3 );
					context.ILGen.MarkLabel( l1 );
				}
				//if( rs.Value >= 0 )
				//{
				//    context.Registers[ 31 ].Value = context.PC + 4;
				//    context.PC.Value = context.PC + ( imm << 2 );
				//}
				return GenerationResult.Branch;
			}

			public static GenerationResult BLTZALL( GenerationContext context, int pass, int address, uint code, byte opcode, uint imm )
			{
				int target = ( address + AddressOffset ) + ( ( int )( short )imm << 2 );

				int rs = ( int )( ( code >> 31 ) & 0x1F );
				if( pass == 0 )
				{
					context.UpdatePc = true;
					context.ReadRegisters[ rs ] = true;
					context.WriteRegisters[ 31 ] = true;
					DefineBranchTarget( context, target );
				}
				else if( pass == 1 )
				{
					LabelMarker targetLabel = context.BranchLabels[ target ];
					//Debug.Assert( targetLabel != default( Label ) );
					context.BranchTarget = targetLabel;

					Label l1 = context.ILGen.DefineLabel();
					Label l2 = context.ILGen.DefineLabel();
					context.ILGen.Emit( OpCodes.Ldc_I4, address + 4 );
					EmitStoreRegister( context, 31 );
					EmitLoadRegister( context, rs );
					//context.ILGen.Emit( OpCodes.Ldc_I4_0 );
					//context.ILGen.Emit( OpCodes.Bge, l1 );
					context.ILGen.Emit( OpCodes.Ldc_I4, 31 );
					context.ILGen.Emit( OpCodes.Shr );
					context.ILGen.Emit( OpCodes.Ldc_I4_1 );
					context.ILGen.Emit( OpCodes.Bne_Un, l1 );
					context.ILGen.Emit( OpCodes.Ldc_I4_1 );
					context.ILGen.Emit( OpCodes.Stloc_3 );
					context.ILGen.Emit( OpCodes.Br, l2 );
					context.ILGen.MarkLabel( l1 );
					context.ILGen.Emit( OpCodes.Ldc_I4_1 );
					context.ILGen.Emit( OpCodes.Stloc_S, ( byte )Cpu.LocalNullifyDelay );
					context.ILGen.MarkLabel( l2 );
				}
				//if( rs.Value < 0 )
				//{
				//	context.Registers[ 31 ].Value = context.PC + 4;
				//	context.PC.Value = context.PC + ( imm << 2 );
				//}
				//else
				//	null = true;
				return GenerationResult.BranchAndNullifyDelay;
			}

			public static GenerationResult BGEZALL( GenerationContext context, int pass, int address, uint code, byte opcode, uint imm )
			{
				int target = ( address + AddressOffset ) + ( ( int )( short )imm << 2 );

				int rs = ( int )( ( code >> 31 ) & 0x1F );
				if( pass == 0 )
				{
					context.UpdatePc = true;
					context.ReadRegisters[ rs ] = true;
					context.WriteRegisters[ 31 ] = true;
					DefineBranchTarget( context, target );
				}
				else if( pass == 1 )
				{
					LabelMarker targetLabel = context.BranchLabels[ target ];
					//Debug.Assert( targetLabel != default( Label ) );
					context.BranchTarget = targetLabel;

					Label l1 = context.ILGen.DefineLabel();
					Label l2 = context.ILGen.DefineLabel();
					context.ILGen.Emit( OpCodes.Ldc_I4, address + 4 );
					EmitStoreRegister( context, 31 );
					EmitLoadRegister( context, rs );
					//context.ILGen.Emit( OpCodes.Ldc_I4_0 );
					//context.ILGen.Emit( OpCodes.Blt, l1 );
					context.ILGen.Emit( OpCodes.Ldc_I4, 31 );
					context.ILGen.Emit( OpCodes.Shr );
					context.ILGen.Emit( OpCodes.Ldc_I4_0 );
					context.ILGen.Emit( OpCodes.Bne_Un, l1 );
					context.ILGen.Emit( OpCodes.Ldc_I4_1 );
					context.ILGen.Emit( OpCodes.Stloc_3 );
					context.ILGen.Emit( OpCodes.Br, l2 );
					context.ILGen.MarkLabel( l1 );
					context.ILGen.Emit( OpCodes.Ldc_I4_1 );
					context.ILGen.Emit( OpCodes.Stloc_S, ( byte )Cpu.LocalNullifyDelay );
					context.ILGen.MarkLabel( l2 );
				}
				//if( rs.Value >= 0 )
				//{
				//	context.Registers[ 31 ].Value = context.PC + 4;
				//	context.PC.Value = context.PC + ( imm << 2 );
				//}
				//else
				//	null = true;
				return GenerationResult.BranchAndNullifyDelay;
			}
		}
		#endregion

		#region Special

		public static class Special
		{
			public static GenerationResult NOP( GenerationContext context, int pass, int address, uint code, byte rs, byte rt, byte rd )
			{
				EmitNop( context );
				return GenerationResult.Success;
			}

			public static GenerationResult SYSCALL( GenerationContext context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
			{
				// We could probably save a register write by properly obeying the
				// hasReturn flag, but I'm not confident the functions all have that
				// filled properly and I'd hate to introduce bugs like that
				// FUTURE: Ensure BIOS methods all have proper HasReturn and enable selective return storage

				int syscall = ( int )( ( code >> 6 ) & 0xFFFFF );

				BiosFunction biosFunction = context.Cpu._syscalls[ syscall ];
				bool willCall;
				bool hasReturn;
				int paramCount;
				if( biosFunction != null )
				{
					willCall = biosFunction.IsImplemented;
					hasReturn = biosFunction.HasReturn;
					paramCount = biosFunction.ParameterCount;

					if( biosFunction.IsImplemented == false )
					{
						Debug.WriteLine( string.Format( "DynRec: NID 0x{0:X8} {1} is not implemented",
							biosFunction.NID, biosFunction.Name ) );
					}
				}
				else
				{
					willCall = false;
					hasReturn = false;
					paramCount = 0;

					Debug.WriteLine( "DynRec: unregistered syscall attempt" );
				}

				if( pass == 0 )
				{
					context.UseMemory = true;
					context.UseSyscalls = true;

					if( willCall == true )
					{
						if( paramCount > 0 )
						{
							context.ReadRegisters[ 4 ] = true;
							if( paramCount > 1 )
							{
								context.ReadRegisters[ 5 ] = true;
								if( paramCount > 2 )
								{
									context.ReadRegisters[ 6 ] = true;
									if( paramCount > 3 )
									{
										context.ReadRegisters[ 7 ] = true;
										if( paramCount > 4 )
										{
											// Maybe this should always go?
											context.ReadRegisters[ 29 ] = true;
										}
									}
								}
							}
						}
						if( hasReturn == true )
						{
							context.WriteRegisters[ 2 ] = true;
						}
					}
				}
				else if( pass == 1 )
				{
					if( willCall == true )
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
				}
				return GenerationResult.Syscall;
			}

			public static GenerationResult BREAK( GenerationContext context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
			{
				if( pass == 0 )
				{
				}
				else if( pass == 1 )
				{
					context.ILGen.Emit( OpCodes.Call, typeof( Debugger ).GetMethod( "Break" ) );
				}
				return GenerationResult.Success;
			}

			public static GenerationResult SYNC( GenerationContext context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function )
			{
				// pg 629 - not needed?
				if( pass == 0 )
				{
				}
				else if( pass == 1 )
				{
				}
				return GenerationResult.Success;
			}

			public static GenerationResult COP1( GenerationContext context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
			{
				if( pass == 0 )
				{
				}
				else if( pass == 1 )
				{
				}
				return GenerationResult.Invalid;
			}

			public static GenerationResult COP2( GenerationContext context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm )
			{
				if( pass == 0 )
				{
				}
				else if( pass == 1 )
				{
				}
				return GenerationResult.Invalid;
			}
		}

		#endregion

		#region Cop0

		public static class Cop0
		{
			public static GenerationResult MTFC0( GenerationContext context, int pass, int address, uint code, byte function )
			{
				//bool moveTo = ( code & 0x800000 ) != 0;
				//long rd = ( code >> 11 ) & 0x1F;
				//long rt = ( code >> 16 ) & 0x1F;
				//Register rrd = context.Cop0Registers[ rd ];
				//Register rrt = context.Registers[ rt ];

				//if( moveTo == true )
				//    rrd.Value = rrt.Value;
				//else
				//    rrt.Value = rrd.Value;

				return GenerationResult.Invalid;
			}
		}

		#endregion
	}
}
