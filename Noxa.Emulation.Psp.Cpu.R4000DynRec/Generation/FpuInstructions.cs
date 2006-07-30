using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection.Emit;
using System.Diagnostics;

namespace Noxa.Emulation.Psp.Cpu.Generation
{
	static class FpuInstructions
	{
		public static GenerateInstructionFpu[] TableFpu = SetupTableFpu();

		#region Setup

		private static GenerateInstructionFpu[] SetupTableFpu()
		{
			GenerateInstructionFpu[] instrs = new GenerateInstructionFpu[ 64 ];

			// Populate table
			GenerateInstructionFpu unk = new GenerateInstructionFpu( UnknownFpu );
			for( int n = 0; n < instrs.Length; n++ )
				instrs[ n ] = unk;

			instrs[ 0 ] = new GenerateInstructionFpu( Arithmetic.ADD );
			instrs[ 1 ] = new GenerateInstructionFpu( Arithmetic.SUB );
			instrs[ 2 ] = new GenerateInstructionFpu( Arithmetic.MUL );
			instrs[ 3 ] = new GenerateInstructionFpu( Arithmetic.DIV );
			instrs[ 4 ] = new GenerateInstructionFpu( Arithmetic.SQRT );
			instrs[ 5 ] = new GenerateInstructionFpu( Arithmetic.ABS );
			instrs[ 6 ] = new GenerateInstructionFpu( Arithmetic.MOV );
			instrs[ 7 ] = new GenerateInstructionFpu( Arithmetic.NEG );
			instrs[ 8 ] = new GenerateInstructionFpu( Arithmetic.ROUNDL );
			instrs[ 9 ] = new GenerateInstructionFpu( Arithmetic.TRUNCL );
			instrs[ 10 ] = new GenerateInstructionFpu( Arithmetic.CEILL );
			instrs[ 11 ] = new GenerateInstructionFpu( Arithmetic.FLOORL );
			instrs[ 12 ] = new GenerateInstructionFpu( Arithmetic.ROUNDW );
			instrs[ 13 ] = new GenerateInstructionFpu( Arithmetic.TRUNCW );
			instrs[ 14 ] = new GenerateInstructionFpu( Arithmetic.CEILW );
			instrs[ 15 ] = new GenerateInstructionFpu( Arithmetic.FLOORW );
			instrs[ 32 ] = new GenerateInstructionFpu( Arithmetic.CVTS );
			instrs[ 33 ] = new GenerateInstructionFpu( Arithmetic.CVTD );
			instrs[ 36 ] = new GenerateInstructionFpu( Arithmetic.CVTW );
			instrs[ 37 ] = new GenerateInstructionFpu( Arithmetic.CVTL );

			for( int n = 48; n <= 63; n++ )
				instrs[ n ] = new GenerateInstructionFpu( Conditional.Compare );

			return instrs;
		}

		#endregion

		#region Unknown helpers

		public static GenerationResult UnknownFpu( GenerationContext context, int pass, int address, uint code, byte fmt, byte fs, byte ft, byte fd, byte function )
		{
			return GenerationResult.Invalid;
		}

		#endregion

		public static void EmitLoadRegister( GenerationContext context, byte reg, byte fmt )
		{
			if( ( fmt == 0 ) || ( fmt == 4 ) )
			{
				context.ILGen.Emit( OpCodes.Ldarg_0 );
				context.ILGen.Emit( OpCodes.Ldfld, context.Core0Cp1 );
				context.ILGen.Emit( OpCodes.Ldfld, context.Cp1Registers );
				context.ILGen.Emit( OpCodes.Ldc_I4, ( int )reg );
				context.ILGen.Emit( OpCodes.Ldelem_R4 );

				//if( fmt == 4 )
				//	context.ILGen.Emit( OpCodes.Conv_I4 );
			}
			else
			{
				Debug.Assert( false );
			}
		}

		public static void EmitLoadRegisters( GenerationContext context, byte r1, byte r2, byte fmt )
		{
			// Could optimize this a bit probably
			EmitLoadRegister( context, r1, fmt );
			EmitLoadRegister( context, r2, fmt );
		}

		public static void EmitStoreRegister( GenerationContext context, byte reg, byte fmt )
		{
			if( ( fmt == 0 ) || ( fmt == 4 ) )
			{
				if( fmt == 4 )
				{
					context.ILGen.Emit( OpCodes.Conv_I4 );
					context.ILGen.Emit( OpCodes.Conv_R4 );
				}
				context.ILGen.Emit( OpCodes.Stloc_S, ( byte )Cpu.LocalTempD1 );
				context.ILGen.Emit( OpCodes.Ldarg_0 );
				context.ILGen.Emit( OpCodes.Ldfld, context.Core0Cp1 );
				context.ILGen.Emit( OpCodes.Ldfld, context.Cp1Registers );
				context.ILGen.Emit( OpCodes.Ldc_I4, ( int )reg );
				context.ILGen.Emit( OpCodes.Ldloc_S, ( byte )Cpu.LocalTempD1 );
				context.ILGen.Emit( OpCodes.Stelem_R4 );
			}
			else
			{
				Debug.Assert( false );
			}
		}

		public static void EmitConvertFormat( GenerationContext context, byte originalFmt, byte targetFmt )
		{
			if( originalFmt == targetFmt )
				return;
			else if( targetFmt == 0 )
			{
				context.ILGen.Emit( OpCodes.Conv_R4 );
			}
			else if( targetFmt == 1 )
			{
				Debug.Assert( false );
			}
			else if( targetFmt == 4 )
			{
				context.ILGen.Emit( OpCodes.Conv_I4 );
				context.ILGen.Emit( OpCodes.Conv_R4 );
			}
			else if( targetFmt == 5 )
			{
				Debug.Assert( false );
			}
		}

		public static class Arithmetic
		{
			public static GenerationResult ADD( GenerationContext context, int pass, int address, uint code, byte fmt, byte fs, byte ft, byte fd, byte function )
			{
				if( pass == 0 )
				{
				}
				else if( pass == 1 )
				{
					EmitLoadRegisters( context, fs, ft, fmt );
					context.ILGen.Emit( OpCodes.Add );
					EmitStoreRegister( context, fd, fmt );
				}
				return GenerationResult.Success;
			}

			public static GenerationResult SUB( GenerationContext context, int pass, int address, uint code, byte fmt, byte fs, byte ft, byte fd, byte function )
			{
				if( pass == 0 )
				{
				}
				else if( pass == 1 )
				{
					EmitLoadRegisters( context, fs, ft, fmt );
					context.ILGen.Emit( OpCodes.Sub );
					EmitStoreRegister( context, fd, fmt );
				}
				return GenerationResult.Success;
			}

			public static GenerationResult MUL( GenerationContext context, int pass, int address, uint code, byte fmt, byte fs, byte ft, byte fd, byte function )
			{
				if( pass == 0 )
				{
				}
				else if( pass == 1 )
				{
					EmitLoadRegisters( context, fs, ft, fmt );
					context.ILGen.Emit( OpCodes.Mul );
					EmitStoreRegister( context, fd, fmt );
				}
				return GenerationResult.Success;
			}

			public static GenerationResult DIV( GenerationContext context, int pass, int address, uint code, byte fmt, byte fs, byte ft, byte fd, byte function )
			{
				if( pass == 0 )
				{
				}
				else if( pass == 1 )
				{
					EmitLoadRegisters( context, fs, ft, fmt );
					context.ILGen.Emit( OpCodes.Div );
					EmitStoreRegister( context, fd, fmt );
				}
				return GenerationResult.Success;
			}

			public static GenerationResult SQRT( GenerationContext context, int pass, int address, uint code, byte fmt, byte fs, byte ft, byte fd, byte function )
			{
				if( pass == 0 )
				{
				}
				else if( pass == 1 )
				{
					EmitLoadRegister( context, fs, fmt );
					context.ILGen.Emit( OpCodes.Call, typeof( Math ).GetMethod( "Sqrt" ) );
					EmitStoreRegister( context, fd, fmt );
				}
				return GenerationResult.Success;
			}

			public static GenerationResult ABS( GenerationContext context, int pass, int address, uint code, byte fmt, byte fs, byte ft, byte fd, byte function )
			{
				if( pass == 0 )
				{
				}
				else if( pass == 1 )
				{
					EmitLoadRegister( context, fs, fmt );
					context.ILGen.Emit( OpCodes.Call, typeof( Math ).GetMethod( "Abs" ) );
					EmitStoreRegister( context, fd, fmt );
				}
				return GenerationResult.Success;
			}

			public static GenerationResult MOV( GenerationContext context, int pass, int address, uint code, byte fmt, byte fs, byte ft, byte fd, byte function )
			{
				if( pass == 0 )
				{
				}
				else if( pass == 1 )
				{
					EmitLoadRegister( context, fs, fmt );
					EmitStoreRegister( context, fd, fmt );
				}
				return GenerationResult.Success;
			}

			public static GenerationResult NEG( GenerationContext context, int pass, int address, uint code, byte fmt, byte fs, byte ft, byte fd, byte function )
			{
				if( pass == 0 )
				{
				}
				else if( pass == 1 )
				{
					EmitLoadRegister( context, fs, fmt );
					context.ILGen.Emit( OpCodes.Neg );
					EmitStoreRegister( context, fd, fmt );
				}
				return GenerationResult.Success;
			}

			public static GenerationResult ROUNDL( GenerationContext context, int pass, int address, uint code, byte fmt, byte fs, byte ft, byte fd, byte function )
			{
				if( pass == 0 )
				{
				}
				else if( pass == 1 )
				{
				}
				return GenerationResult.Invalid;
			}

			public static GenerationResult TRUNCL( GenerationContext context, int pass, int address, uint code, byte fmt, byte fs, byte ft, byte fd, byte function )
			{
				if( pass == 0 )
				{
				}
				else if( pass == 1 )
				{
				}
				return GenerationResult.Invalid;
			}

			public static GenerationResult CEILL( GenerationContext context, int pass, int address, uint code, byte fmt, byte fs, byte ft, byte fd, byte function )
			{
				if( pass == 0 )
				{
				}
				else if( pass == 1 )
				{
				}
				return GenerationResult.Invalid;
			}

			public static GenerationResult FLOORL( GenerationContext context, int pass, int address, uint code, byte fmt, byte fs, byte ft, byte fd, byte function )
			{
				if( pass == 0 )
				{
				}
				else if( pass == 1 )
				{
				}
				return GenerationResult.Invalid;
			}

			public static GenerationResult ROUNDW( GenerationContext context, int pass, int address, uint code, byte fmt, byte fs, byte ft, byte fd, byte function )
			{
				if( pass == 0 )
				{
				}
				else if( pass == 1 )
				{
					EmitLoadRegister( context, fs, fmt );

					context.ILGen.Emit( OpCodes.Conv_R8 );
					context.ILGen.Emit( OpCodes.Ldc_I4, ( int )MidpointRounding.ToEven );
					context.ILGen.Emit( OpCodes.Call, typeof( Math ).GetMethod( "Round" ) );
					context.ILGen.Emit( OpCodes.Conv_R4 );

					EmitConvertFormat( context, fmt, 4 );
					EmitStoreRegister( context, fd, 4 );
				}
				return GenerationResult.Success;
			}

			public static GenerationResult TRUNCW( GenerationContext context, int pass, int address, uint code, byte fmt, byte fs, byte ft, byte fd, byte function )
			{
				if( pass == 0 )
				{
				}
				else if( pass == 1 )
				{
					EmitLoadRegister( context, fs, fmt );

					context.ILGen.Emit( OpCodes.Conv_R8 );
					context.ILGen.Emit( OpCodes.Call, typeof( Math ).GetMethod( "Truncate", new Type[] { typeof( double ) } ) );
					context.ILGen.Emit( OpCodes.Conv_R4 );

					EmitConvertFormat( context, fmt, 4 );
					EmitStoreRegister( context, fd, 4 );
				}
				return GenerationResult.Success;
			}

			public static GenerationResult CEILW( GenerationContext context, int pass, int address, uint code, byte fmt, byte fs, byte ft, byte fd, byte function )
			{
				if( pass == 0 )
				{
				}
				else if( pass == 1 )
				{
					EmitLoadRegister( context, fs, fmt );

					context.ILGen.Emit( OpCodes.Conv_R8 );
					context.ILGen.Emit( OpCodes.Call, typeof( Math ).GetMethod( "Ceiling" ) );
					context.ILGen.Emit( OpCodes.Conv_R4 );
					
					EmitConvertFormat( context, fmt, 4 );
					EmitStoreRegister( context, fd, 4 );
				}
				return GenerationResult.Success;
			}

			public static GenerationResult FLOORW( GenerationContext context, int pass, int address, uint code, byte fmt, byte fs, byte ft, byte fd, byte function )
			{
				if( pass == 0 )
				{
				}
				else if( pass == 1 )
				{
					EmitLoadRegister( context, fs, fmt );

					context.ILGen.Emit( OpCodes.Conv_R8 );
					context.ILGen.Emit( OpCodes.Call, typeof( Math ).GetMethod( "Floor" ) );
					context.ILGen.Emit( OpCodes.Conv_R4 );

					EmitConvertFormat( context, fmt, 4 );
					EmitStoreRegister( context, fd, 4 );
				}
				return GenerationResult.Success;
			}

			public static GenerationResult CVTS( GenerationContext context, int pass, int address, uint code, byte fmt, byte fs, byte ft, byte fd, byte function )
			{
				if( pass == 0 )
				{
				}
				else if( pass == 1 )
				{
					EmitLoadRegister( context, fs, fmt );
					EmitConvertFormat( context, fmt, 0 );
					EmitStoreRegister( context, fd, 0 );
				}
				return GenerationResult.Success;
			}

			public static GenerationResult CVTD( GenerationContext context, int pass, int address, uint code, byte fmt, byte fs, byte ft, byte fd, byte function )
			{
				if( pass == 0 )
				{
				}
				else if( pass == 1 )
				{
				}
				return GenerationResult.Invalid;
			}

			public static GenerationResult CVTW( GenerationContext context, int pass, int address, uint code, byte fmt, byte fs, byte ft, byte fd, byte function )
			{
				if( pass == 0 )
				{
				}
				else if( pass == 1 )
				{
					EmitLoadRegister( context, fs, fmt );
					EmitConvertFormat( context, fmt, 4 );
					EmitStoreRegister( context, fd, 4 );
				}
				return GenerationResult.Success;
			}

			public static GenerationResult CVTL( GenerationContext context, int pass, int address, uint code, byte fmt, byte fs, byte ft, byte fd, byte function )
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

		public static class Conditional
		{
			public static GenerationResult Compare( GenerationContext context, int pass, int address, uint code, byte fmt, byte fs, byte ft, byte fd, byte function )
			{
				if( pass == 0 )
				{
				}
				else if( pass == 1 )
				{
					// TODO: optimize this so we don't rely on locals

					EmitLoadRegisters( context, fs, ft, fmt );

					// fs in D1, ft in D2
					context.ILGen.Emit( OpCodes.Stloc_S, ( byte )Cpu.LocalTempD2 );
					context.ILGen.Emit( OpCodes.Stloc_S, ( byte )Cpu.LocalTempD1 );

					uint fc = ( code >> 4 ) & 0x03;
					uint cond = code & 0x0F;
					bool lessBit = ( ( cond >> 2 ) & 0x1 ) == 1 ? true : false;
					bool equalBit = ( ( cond >> 1 ) & 0x1 ) == 1 ? true : false;
					bool unorderedBit = ( cond & 0x1 ) == 1 ? true : false;

					Label notNaN = context.ILGen.DefineLabel();
					Label done = context.ILGen.DefineLabel();

					// NAN (less = false, equal = false, unordered = true)
					context.ILGen.Emit( OpCodes.Ldloc_S, ( byte )Cpu.LocalTempD1 );
					context.ILGen.Emit( OpCodes.Call, typeof( float ).GetMethod( "IsNaN" ) );
					context.ILGen.Emit( OpCodes.Brfalse_S, notNaN );
					context.ILGen.Emit( OpCodes.Ldloc_S, ( byte )Cpu.LocalTempD2 );
					context.ILGen.Emit( OpCodes.Call, typeof( float ).GetMethod( "IsNaN" ) );
					context.ILGen.Emit( OpCodes.Brfalse_S, notNaN );

					// Calculate condition bit
					if( unorderedBit == true )
						context.ILGen.Emit( OpCodes.Ldc_I4_1 );
					else
						context.ILGen.Emit( OpCodes.Ldc_I4_0 );
					context.ILGen.Emit( OpCodes.Conv_U1 );

					context.ILGen.Emit( OpCodes.Br_S, done );
					context.ILGen.MarkLabel( notNaN );

					// Normal (less = fs < ft, equal = fs == ft, unordered = false)
					if( lessBit == true )
					{
						context.ILGen.Emit( OpCodes.Ldloc_S, ( byte )Cpu.LocalTempD1 );
						context.ILGen.Emit( OpCodes.Ldloc_S, ( byte )Cpu.LocalTempD2 );
						context.ILGen.Emit( OpCodes.Clt );
					}
					if( equalBit == true )
					{
						context.ILGen.Emit( OpCodes.Ldloc_S, ( byte )Cpu.LocalTempD1 );
						context.ILGen.Emit( OpCodes.Ldloc_S, ( byte )Cpu.LocalTempD2 );
						context.ILGen.Emit( OpCodes.Ceq );
					}
					if( ( lessBit == true ) && ( equalBit == true ) )
						context.ILGen.Emit( OpCodes.Or );
					Debug.Assert( ( lessBit == true ) || ( equalBit == true ) );
					context.ILGen.Emit( OpCodes.Conv_U1 );

					context.ILGen.MarkLabel( done );

					CoreInstructions.EmitStoreCopConditionBit( context, 1 );
				}
				return GenerationResult.Success;
			}
		}
	}
}
