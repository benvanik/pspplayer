using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection.Emit;
using System.Reflection;
using Noxa.Emulation.Psp.Bios;
using System.Diagnostics;

namespace Noxa.Emulation.Psp.Cpu.Generation
{
	enum GenerationResult
	{
		Success,
		Invalid,
		Syscall,

		/// <summary>
		/// Branch or jump; this means that after the next instruction (delay slot) the block should end.
		/// </summary>
		Branch,
		BranchAndNullifyDelay
	}

	delegate GenerationResult GenerateInstructionR( GenerationContext context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function );
	delegate GenerationResult GenerateInstructionI( GenerationContext context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm );
	delegate GenerationResult GenerateInstructionJ( GenerationContext context, int pass, int address, uint code, byte opcode, uint imm );
	delegate GenerationResult GenerateInstructionCop0( GenerationContext context, int pass, int address, uint code, byte function );

	class GenerationContext
	{
		public int StartAddress;
		public int EndAddress;
		public int InstructionCount;

		public Cpu Cpu;
		public Core Core0;
		public Memory Memory;
		public ILGenerator ILGen;

		public bool UseCore;
		public bool UseMemory;
		public bool UseSyscalls;
		public bool UpdatePc;

		// TODO: Make these bitfields so they are more efficient (are bitfields more efficient?)
		public bool[] ReadRegisters = new bool[ 32 ];
		public bool[] WriteRegisters = new bool[ 32 ];

		// Map of register to local variable index, already offset by RegisterBase
		public int[] Registers = new int[ 32 ];
		public int RegisterCount;

		public int TempBase;

		public FieldInfo Core0Pc;
		public FieldInfo Core0NullifyDelay;
		public FieldInfo Core0Delay;
		public FieldInfo Core0Hi;
		public FieldInfo Core0Lo;
		public FieldInfo Core0LL;
		public FieldInfo MemoryMainBuffer;
		public MethodInfo MemoryReadWord;
		public MethodInfo MemoryWriteWord;
		public FieldInfo BiosFunctionTarget;
		public MethodInfo DelegateTargetGet;
		public MethodInfo DebugWriteLine;
		public MethodInfo StringFormat1;

		public GenerationContext()
		{
			Core0Pc = typeof( Core ).GetField( "Pc", BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance );
			Core0NullifyDelay = typeof( Core ).GetField( "DelayNop", BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance );
			Core0Delay = typeof( Core ).GetField( "InDelaySlot", BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance );
			Core0Hi = typeof( Core ).GetField( "Hi", BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance );
			Core0Lo = typeof( Core ).GetField( "Lo", BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance );
			Core0LL = typeof( Core ).GetField( "LL", BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance );

			MemoryMainBuffer = typeof( Memory ).GetField( "_mainMemory", BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance );
			MemoryReadWord = typeof( Memory ).GetMethod( "ReadWord" );
			MemoryWriteWord = typeof( Memory ).GetMethod( "WriteWord" );

			BiosFunctionTarget = typeof( BiosFunction ).GetField( "Target" );
			DelegateTargetGet = typeof( BiosStubDelegate ).GetMethod( "get_Target" );

			DebugWriteLine = typeof( Debug ).GetMethod( "WriteLine", new Type[] { typeof( string ) } );
			StringFormat1 = typeof( String ).GetMethod( "Format", new Type[] { typeof( string ), typeof( object ) } );
		}

		public void Reset( ILGenerator ilgen, int startAddress )
		{
			ILGen = ilgen;
			StartAddress = startAddress;
			EndAddress = startAddress;
			InstructionCount = 0;

			UseCore = false;
			UseMemory = false;
			UseSyscalls = false;
			UpdatePc = false;

			for( int n = 0; n < ReadRegisters.Length; n++ )
			{
				ReadRegisters[ n ] = false;
				WriteRegisters[ n ] = false;
				Registers[ n ] = -1;
			}

			RegisterCount = 0;
			TempBase = 0;
		}

		public void Prepare( int registerBase )
		{
			// Now we have all the info we need to set ourselves up to be generated
			// This just means we finalize the register listing

			// TODO: a good optimization would be to track in pass 0 if the register
			// is written before it's read - if it is, there is no need to load it

			for( int reg = 0; reg < ReadRegisters.Length; reg++ )
			{
				if( ReadRegisters[ reg ] == false )
					continue;
				if( reg == 0 )
					continue;

				Registers[ reg ] = registerBase + RegisterCount;
				RegisterCount++;
			}

			for( int reg = 0; reg < WriteRegisters.Length; reg++ )
			{
				if( WriteRegisters[ reg ] == false )
					continue;
				if( reg == 0 )
					continue;

				if( Registers[ reg ] == -1 )
				{
					Registers[ reg ] = registerBase + RegisterCount;
					RegisterCount++;
				}
			}

			TempBase = registerBase + RegisterCount;
		}
	}
}
