// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

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

		Branch,
		BranchAndNullifyDelay,

		Jump
	}

	class LabelMarker
	{
		public Label Label;
		public bool Found;
		public int Address;

		public LabelMarker( int address )
		{
			Address = address;
		}
	}

	delegate GenerationResult GenerateInstructionR( GenerationContext context, int pass, int address, uint code, byte opcode, byte rs, byte rt, byte rd, byte shamt, byte function );
	delegate GenerationResult GenerateInstructionI( GenerationContext context, int pass, int address, uint code, byte opcode, byte rs, byte rt, ushort imm );
	delegate GenerationResult GenerateInstructionJ( GenerationContext context, int pass, int address, uint code, byte opcode, uint imm );
	delegate GenerationResult GenerateInstructionCop0( GenerationContext context, int pass, int address, uint code, byte function );
	delegate GenerationResult GenerateInstructionSpecial3( GenerationContext context, int pass, int address, uint code, byte rt, byte rd, byte function, ushort bshfl );
	delegate GenerationResult GenerateInstructionFpu( GenerationContext context, int pass, int address, uint code, byte fmt, byte fs, byte ft, byte fd, byte function );

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

		public Dictionary<int, LabelMarker> BranchLabels = new Dictionary<int, LabelMarker>( 25 );
		public int LastBranchTarget;

		// If InDelay == true and BranchTarget != null, after the delay we branch if local 3 == 1
		public bool InDelay;
		public LabelMarker BranchTarget;

		public int TempBase;

		public FieldInfo Core0Pc;
		public FieldInfo Core0NullifyDelay;
		public FieldInfo Core0Delay;
		public FieldInfo Core0Hi;
		public FieldInfo Core0Lo;
		public FieldInfo Core0LL;
		public FieldInfo Core0BlockCounter;
		public FieldInfo Core0Cp0;
		public FieldInfo Core0Cp1;
		public FieldInfo Core0Cp2;
		public FieldInfo Cp1Registers;
		public MethodInfo Cp1ConditionBitGet;
		public MethodInfo Cp1ConditionBitSet;
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
			Core0BlockCounter = typeof( Core ).GetField( "BlockCounter", BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance );
			Core0Cp0 = typeof( Core ).GetField( "Cp0", BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance );
			Core0Cp1 = typeof( Core ).GetField( "Cp1", BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance );
			Core0Cp2 = typeof( Core ).GetField( "Cp2", BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance );

			Cp1Registers = typeof( Coprocessor1 ).GetField( "Registers", BindingFlags.Public | BindingFlags.GetField | BindingFlags.Instance );
			Cp1ConditionBitGet = typeof( Coprocessor1 ).GetMethod( "get_ConditionBit" );
			Cp1ConditionBitSet = typeof( Coprocessor1 ).GetMethod( "set_ConditionBit" );

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

			BranchLabels.Clear();
			LastBranchTarget = 0;
			BranchTarget = null;

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

			// All labels should be defined at this point so that pass 2 can get some data
			foreach( KeyValuePair<int, LabelMarker> pair in BranchLabels )
			{
				pair.Value.Label = ILGen.DefineLabel();
			}
		}

		public bool IsBranchLocal( int address )
		{
			return ( address >= StartAddress ) &&
				( address <= EndAddress );
		}
	}
}
