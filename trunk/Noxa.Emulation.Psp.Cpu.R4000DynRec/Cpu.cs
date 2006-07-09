#define VERBOSEEMIT
#define STATS

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Noxa.Emulation.Psp.Utilities;
using Noxa.Emulation.Psp.Bios;
using System.Reflection.Emit;
using Noxa.Emulation.Psp.Cpu.Generation;
using System.Reflection;
using System.Threading;

namespace Noxa.Emulation.Psp.Cpu
{
	class Cpu : ICpu
	{
		public const int DefaultBlockCount = 5000;

		protected IEmulationInstance _emulator;
		protected ComponentParameters _params;
		protected CpuCapabilities _caps;
		protected Clock _clock;
		protected Memory _memory;
		protected Core _core0;
		protected Core _core1;

		protected CodeCache _codeCache;
		protected GenerationContext _context;

		internal int _lastSyscall;
		internal BiosFunction[] _syscalls;

#if STATS
		protected PerformanceTimer _timer;
		protected double _timeSinceLastIpsPrint;
		protected bool _debug = false;

		protected struct RuntimeStatistics
		{
			public int InstructionsExecuted;
			public int CodeBlocksExecuted;
			public int CodeBlocksGenerated;

			public int BreaksFromLength;
			public int BreaksFromBranches;
			public int BreaksFromSyscalls;

			public int AverageRegistersUsed;
			public int MostRegistersUsed;

			public int CodeCacheHits;
			public int CodeCacheMisses;

			public int AverageCodeBlockLength;
			public int LargestCodeBlockLength;

			public double AverageGenerationTime;
			public double InstructionsPerSecond;
			public double RunTime;
		}

		protected RuntimeStatistics _stats = new RuntimeStatistics();
#endif

		public Cpu( IEmulationInstance emulator, ComponentParameters parameters )
		{
			Debug.Assert( emulator != null );
			Debug.Assert( parameters != null );

			_caps = new CpuCapabilities();
			_emulator = emulator;
			_params = parameters;

			_lastSyscall = -1;
			_syscalls = new BiosFunction[ 1024 ];

#if STATS
			_timer = new PerformanceTimer();
			_timeSinceLastIpsPrint = 0.0;
#endif

			_clock = new Clock();
			_memory = new Memory();
			
			// Order matters as the lookup is linear and stupid... should be changed somehow
			//_memory->DefineSegment( MemoryType::PhysicalMemory, "Main Memory", 0x08000000, 0x01FFFFFF );
			//_memory->DefineSegment( MemoryType::PhysicalMemory, "Hardware Vectors", 0x1FC00000, 0x000FFFFF );
			//_memory->DefineSegment( MemoryType::PhysicalMemory, "Scratchpad", 0x00010000, 0x00003FFF );
			//_memory->DefineSegment( MemoryType::PhysicalMemory, "Frame Buffer", 0x04000000, 0x001FFFFF );
			//_memory->DefineSegment( MemoryType::HardwareMapped, "Hardware IO 1", 0x1C000000, 0x03BFFFFF );
			//_memory->DefineSegment( MemoryType::HardwareMapped, "Hardware IO 2", 0x1FD00000, 0x002FFFFF );

			_core0 = new Core( this, 0, "Allegrex", CoreAttributes.HasCp2 );
			_core1 = new Core( this, 1, "Media Engine", CoreAttributes.Default );

			_codeCache = new CodeCache();
			_context = new GenerationContext();
			_context.Cpu = this;
			_context.Core0 = _core0;
			_context.Memory = _memory;
		}

		#region Caps

		protected class CpuCapabilities : ICpuCapabilities
		{
			public Endianess Endianess
			{
				get
				{
					return Endianess.LittleEndian;
				}
			}

			public bool VectorFpuSupported
			{
				get
				{
					return false;
				}
			}

			public bool DmaSupported
			{
				get
				{
					return false;
				}
			}

			public bool AvcSupported
			{
				get
				{
					return false;
				}
			}
		}

		#endregion

		public IEmulationInstance Emulator
		{
			get
			{
				return _emulator;
			}
		}

		public ComponentParameters Parameters
		{
			get
			{
				return _params;
			}
		}

		public ICpuCapabilities Capabilities
		{
			get
			{
				return _caps;
			}
		}

		public IClock Clock
		{
			get
			{
				return _clock;
			}
		}

		public ICpuCore[] Cores
		{
			get
			{
				return new ICpuCore[] { _core0, _core1 };
			}
		}

		public ICpuCore this[ int core ]
		{
			get
			{
				if( core == 0 )
					return _core0;
				else if( core == 1 )
					return _core1;
				return null;
			}
		}

		public IDmaController Dma
		{
			get
			{
				return null;
			}
		}

		public IAvcDecoder Avc
		{
			get
			{
				return null;
			}
		}

		public IMemory Memory
		{
			get
			{
				return _memory;
			}
		}

		public CodeCache CodeCache
		{
			get
			{
				return _codeCache;
			}
		}

		public int RegisterSyscall( uint nid )
		{
			BiosFunction function = _emulator.Bios.FindFunction( nid );
			if( function == null )
				return -1;

			int sid = ++_lastSyscall;
			_syscalls[ sid ] = function;

			return sid;
		}

		public int ExecuteBlock()
		{
			// This happens after syscall

#if STATS
			double blockStart = _timer.Elapsed;
			if( _stats.RunTime == 0.0 )
				_stats.RunTime = _timer.Elapsed;
#endif
			
			int count = 0;
			for( int n = 0; n < DefaultBlockCount; n++ )
			{
				int address = _core0.Pc;
				address = _core0.TranslateAddress( address );
				if( address == 0x8900350 )
					_debug = true;
				//if( address == 0x089004D0 )
				//	_debug = true;
				//if( address == 0x8900350 )
				//	Debugger.Break();
				//if( address == 0x8900374 )
				//	Debugger.Break();
				//if( address == 0x08902744 )
				//	Debugger.Break();

				CodeBlock block = _codeCache.Find( address );
				if( block == null )
				{
					block = GenerateBlock( address );
#if !STATS
				}
#else
					_stats.CodeCacheMisses++;
				}
				else
					_stats.CodeCacheHits++;

				_stats.CodeBlocksExecuted++;
				_stats.InstructionsExecuted += block.InstructionCount;
#endif

				block.ExecutionCount++;

				//Debug.WriteLine( string.Format( "{0:X8}", block.Address ) );

				int ret = block.Pointer( _core0, _memory, _core0.Registers, _syscalls );
				if( ( ret == 0 ) &&
					( block.EndsOnSyscall == false ) )
				{
					// Need to manually update PC
					_core0.Pc += 4 * block.InstructionCount;
				}
				count += block.InstructionCount;
#if DEBUG
				//if( _debug == true )
				//	Debug.WriteLine( " -- " );
#endif

				if( block.EndsOnSyscall == true )
					break;
			}
				
#if STATS
			double blockTime = _timer.Elapsed - blockStart;
			if( blockTime <= 0.0 )
				blockTime = 0.000001;
			
			_timeSinceLastIpsPrint += blockTime;
			if( _timeSinceLastIpsPrint > 1.0 )
			{
				double ips = ( ( double )count / blockTime );
				Debug.WriteLine( string.Format( "IPS: {0}", ( long )ips ) );
				_timeSinceLastIpsPrint = 0.0;
			}
#endif

			return count;
		}

		public void PrintStatistics()
		{
#if STATS
			_stats.AverageRegistersUsed /= _stats.CodeBlocksGenerated;
			_stats.AverageCodeBlockLength /= _stats.CodeBlocksGenerated;
			_stats.AverageGenerationTime /= _stats.CodeBlocksGenerated;
			_stats.RunTime = _timer.Elapsed - _stats.RunTime;
			_stats.InstructionsPerSecond = _stats.InstructionsExecuted / _stats.RunTime;
			StringBuilder sb = new StringBuilder();
			foreach( FieldInfo field in typeof( RuntimeStatistics ).GetFields() )
			{
				object value = field.GetValue( _stats );
				sb.AppendFormat( "{0}: {1}\n", field.Name, value );
			}
			sb.AppendFormat( "\nLB:{0}, LH:{1}, LW:{2}, SB:{3}, SH:{4}, SW:{5}",
				CoreInstructions.Memory.LBC, CoreInstructions.Memory.LHC, CoreInstructions.Memory.LWC,
				CoreInstructions.Memory.SBC, CoreInstructions.Memory.SHC, CoreInstructions.Memory.SWC );
			if( true )
			{
				System.Windows.Forms.MessageBox.Show( sb.ToString() );
			}
			else
			{
				Debug.WriteLine( sb.ToString() );
			}
#endif
		}

		/// <summary>
		/// Maximum number of instructions before bailing.
		/// </summary>
		protected const int MaximumCodeLength = 100;

		private Type[] _methodParams = new Type[] { typeof( Core ), typeof( Memory ), typeof( int[] ), typeof( BiosFunction[] ) };

		private void F( ILGenerator ilgen, int n )
		{
			ilgen.Emit( OpCodes.Ldloc_0 );
			ilgen.Emit( OpCodes.Ldc_I4, n );
			CoreInstructions.EmitLoadRegister( _context, n );
			ilgen.Emit( OpCodes.Box, typeof( Int32 ) );
			ilgen.Emit( OpCodes.Stelem_Ref );
		}

		/// <summary>
		/// Generate and cache a codeblock starting from the given address.
		/// </summary>
		/// <param name="cpu">CPU to generate the code for.</param>
		/// <param name="startAddress">Starting address.</param>
		/// <returns>The newly created code block.</returns>
		protected CodeBlock GenerateBlock( int startAddress )
		{
#if STATS
			double genStart = _timer.Elapsed;
#endif

			CodeBlock block = new CodeBlock();
			block.Address = startAddress;
			block.MethodUsed = GenerationMethod.DynamicMethod;

			DynamicMethod method = new DynamicMethod( "c" + startAddress.ToString( "X" ),
				typeof( int ), _methodParams,
				this.GetType(), true );
			ILGenerator ilgen = method.GetILGenerator();

			_context.Reset( ilgen, startAddress );

			for( int pass = 0; pass <= 1; pass++ )
			{
				// Only have the information to generate the preamble on pass 1
				if( pass == 1 )
				{
					_context.Prepare( 0 );
					GeneratePreamble( _context );
				}

				bool breakOut = false;
				bool checkNullDelay = false;
				int address = startAddress;
				for( int n = 0; n < MaximumCodeLength; n++ )
				{
					//if( address == 0x089044a4 )
					//	Debugger.Break();
					GenerationResult result = GenerationResult.Invalid;

					if( ( pass == 1 ) && ( _debug == true ) )
					{
						//ilgen.Emit( OpCodes.Ldstr, "0={0:X8} 1={1:X8} 2={2:X8} 3={3:X8} 4={4:X8} 5={5:X8} 6={6:X8} 7={7:X8} 8={8:X8} 9={9:X8} 10={10:X8} 11={11:X8} 12={12:X8} 13={13:X8} 14={14:X8} 15={15:X8} 16={16:X8} 17={17:X8} 18={18:X8} 19={19:X8} 20={20:X8} 21={21:X8} 22={22:X8} 23={23:X8} 24={24:X8} 25={25:X8} 26={26:X8} 27={27:X8} 28={28:X8} 29={29:X8} 30={30:X8} 31={31:X8}" );
						//ilgen.Emit( OpCodes.Ldc_I4, 32 );
						//ilgen.Emit( OpCodes.Newarr, typeof( object ) );
						//ilgen.Emit( OpCodes.Stloc_0 );
						//for( int r = 0; r < 32; r++ )
						//    F( ilgen, r );
						//ilgen.Emit( OpCodes.Ldloc_0 );
						//ilgen.Emit( OpCodes.Call, typeof( String ).GetMethod( "Format", new Type[] { typeof( string ), typeof( object[] ) } ) );
						//ilgen.Emit( OpCodes.Call, _context.DebugWriteLine );
					}

					Label nullDelaySkip = ilgen.DefineLabel();
					if( checkNullDelay == true )
					{
						ilgen.Emit( OpCodes.Ldloc_S, ( byte )LocalNullifyDelay );
						ilgen.Emit( OpCodes.Brtrue, nullDelaySkip );
					}

					uint code = ( uint )_memory.ReadWord( address );

					if( code != 0 )
					{
						// Decode instruction
						uint opcode = code >> 26 ;
						switch( opcode )
						{
							case 0: // R type
								{
									byte function = ( byte )( code & 0x3F );
									byte rs = ( byte )( ( code >> 21 ) & 0x1F );
									byte rt = ( byte )( ( code >> 16 ) & 0x1F );
									byte rd = ( byte )( ( code >> 11 ) & 0x1F );
									byte shamt = ( byte )( ( code >> 6 ) & 0x1F );

									GenerateInstructionR instr = CoreInstructions.TableR[ function ];
#if VERBOSEEMIT
									if( pass == 1 )
									{
										EmitDebugInfo( _context, address, code, instr.Method.Name,
											string.Format( "rs:{0} rt:{1} rd:{2} shamt:{3}", rs, rt, rd, shamt ) );
									}
#endif
									result = instr( _context, pass, address + 4, code, ( byte )opcode, rs, rt, rd, shamt, function );
								}
								break;
							case 1: // J type
								{
									uint imm = code & 0x03FFFFFF;
									uint rt = ( code >> 16 ) & 0x1F;

									GenerateInstructionJ instr = CoreInstructions.TableJ[ rt ];
#if VERBOSEEMIT
									if( pass == 1 )
									{
										EmitDebugInfo( _context, address, code, instr.Method.Name,
											string.Format( "imm:{0}", imm ) );
									}
#endif
									result = instr( _context, pass, address + 4, code, ( byte )opcode, imm );
								}
								break;
							case 16: // COP0 type
								{
									byte function = ( byte )( code & 0x3F );

									GenerateInstructionCop0 instr = CoreInstructions.TableCop0[ function ];
#if VERBOSEEMIT
									if( pass == 1 )
									{
										EmitDebugInfo( _context, address, code, instr.Method.Name,
											string.Format( "func:{0}", function ) );
									}
#endif
									result = instr( _context, pass, address + 4, code, function );
								}
								break;
							default: // Common I type
								{
									byte rs = ( byte )( ( code >> 21 ) & 0x1F );
									byte rt = ( byte )( ( code >> 16 ) & 0x1F );
									ushort imm = ( ushort )( code & 0xFFFF );

									GenerateInstructionI instr = CoreInstructions.TableI[ opcode ];
#if VERBOSEEMIT
									if( pass == 1 )
									{
										EmitDebugInfo( _context, address, code, instr.Method.Name,
											string.Format( "rs:{0} rt:{1} imm:{2}", rs, rt, ( int )( short )imm ) );
									}
#endif
									result = instr( _context, pass, address + 4, code, ( byte )opcode, rs, rt, imm );
								}
								break;
						}

						if( result == GenerationResult.Invalid )
						{
							// Failed to generate - we break ASAP!
							Debug.WriteLine( string.Format( "GenerateBlock: failed to generate IL for [0x{0:X8}] {1:X8}", address, code ) );
							break;
						}
					}
					else
					{
#if VERBOSEEMIT
						if( pass == 1 )
							EmitDebugInfo( _context, address, code, "NOP", "" );
#endif
					}

					Label omfg = ilgen.DefineLabel();
					ilgen.Emit( OpCodes.Br, omfg );
					ilgen.MarkLabel( nullDelaySkip );
					if( pass == 1 )
						EmitDebugInfo( _context, address, 0x0, "NOP", "" );
					ilgen.MarkLabel( omfg );

					if( pass == 0 )
					{
						_context.InstructionCount++;
						_context.EndAddress = address;
					}

					address += 4;

					// Syscalls always exit
					if( result == GenerationResult.Syscall )
						break;

					// This is so that we can handle delay slots
					if( breakOut == true )
						break;
					if( result == GenerationResult.Branch )
						breakOut = true;
					if( result == GenerationResult.BranchAndNullifyDelay )
					{
						breakOut = true;
						if( pass == 1 )
							checkNullDelay = true;
					}
				}

				if( pass == 1 )
					GenerateTail( _context );
			}

#if DEBUG
			block.DynamicMethod = method;
#endif

			block.EndsOnSyscall = _context.UseSyscalls;
			block.InstructionCount = _context.InstructionCount;

			block.Pointer = ( DynamicCodeDelegate )method.CreateDelegate( typeof( DynamicCodeDelegate ) );
			_codeCache.Add( block );

#if STATS
			_stats.CodeBlocksGenerated++;

			double genTime = _timer.Elapsed - genStart;
			if( genTime <= 0.0 )
				genTime = 0.000001;
			_stats.AverageGenerationTime += genTime;

			if( _context.UseSyscalls == true )
				_stats.BreaksFromSyscalls++;
			else if( _context.InstructionCount >= MaximumCodeLength )
				_stats.BreaksFromLength++;
			else
				_stats.BreaksFromBranches++;
			_stats.AverageCodeBlockLength += _context.InstructionCount;
			if( _context.InstructionCount > _stats.LargestCodeBlockLength )
				_stats.LargestCodeBlockLength = _context.InstructionCount;
			_stats.AverageRegistersUsed += _context.RegisterCount;
			if( _context.RegisterCount > _stats.MostRegistersUsed )
				_stats.MostRegistersUsed = _context.RegisterCount;
#endif

			return block;
		}

		// Input:
		// 0      1       2                   3
		// core0, memory, generalRegisters[], syscallList[]

		public const int ArgCore0 = 0;			// DO NOT CHANGE
		public const int ArgMemory = 1;			// DO NOT CHANGE
		public const int ArgRegisterList = 2;	// DO NOT CHANGE
		public const int ArgSyscallList = 3;	// DO NOT CHANGE

		public const int LocalTempA = 0;		// DO NOT CHANGE
		public const int LocalTempB = 1;		// DO NOT CHANGE
		public const int LocalPC = 2;			// DO NOT CHANGE
		public const int LocalPCValid = 3;		// DO NOT CHANGE
		public const int LocalNullifyDelay = 4;	// DO NOT CHANGE

		protected static void GeneratePreamble( GenerationContext context )
		{
			ILGenerator ilgen = context.ILGen;

			ilgen.DeclareLocal( typeof( int ) );
			ilgen.DeclareLocal( typeof( int ) );
			if( context.UpdatePc == true )
			{
				ilgen.DeclareLocal( typeof( int ) );
				ilgen.DeclareLocal( typeof( int ) );
				ilgen.DeclareLocal( typeof( int ) );
			}

			if( context.UpdatePc == true )
			{
				ilgen.Emit( OpCodes.Ldc_I4, context.StartAddress );
				ilgen.Emit( OpCodes.Stloc_2 );

				ilgen.Emit( OpCodes.Ldc_I4_0 );
				ilgen.Emit( OpCodes.Stloc_3 );

				ilgen.Emit( OpCodes.Ldc_I4_0 );
				ilgen.Emit( OpCodes.Stloc_S, ( byte )LocalNullifyDelay );
			}
		}

		protected static void GenerateTail( GenerationContext context )
		{
			ILGenerator ilgen = context.ILGen;

			// Note that in the case of a syscall we ignore what we've done to the PC!
			if( ( context.UpdatePc == true ) &&
				( context.UseSyscalls == false ) )
			{
				ilgen.Emit( OpCodes.Ldarg_0 );
				ilgen.Emit( OpCodes.Ldloc_2 );
				ilgen.Emit( OpCodes.Stfld, context.Core0Pc );

				ilgen.Emit( OpCodes.Ldarg_0 );
				ilgen.Emit( OpCodes.Ldloc_S, ( byte )LocalNullifyDelay );
				ilgen.Emit( OpCodes.Stfld, context.Core0NullifyDelay );
			}

			// 1 = pc updated, 0 = pc update needed
			if( context.UpdatePc == true )
				ilgen.Emit( OpCodes.Ldloc_3 );
			else
				ilgen.Emit( OpCodes.Ldc_I4_0 );

			ilgen.Emit( OpCodes.Ret );
		}

		[Conditional( "VERBOSEEMIT" )]
		protected void EmitDebugInfo( GenerationContext context, int address, uint code, string name, string args )
		{
			if( _debug == false )
				return;

			//string line = string.Format( "[0x{0:X8}] {1:X8} {2:8} {3}",
			//	address, code, name, args );
			string line = string.Format( "0x{0:X8}: {1:X8}",
				address, code );

			context.ILGen.Emit( OpCodes.Ldstr, line );
			context.ILGen.Emit( OpCodes.Call, context.DebugWriteLine );
		}

		public void Cleanup()
		{
#if STATS
			_stats = new RuntimeStatistics();
#endif
			//_codeCache.Clear();
			//_core0.Clear();
			//_core1.Clear();
		}
	}
}
