// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#define GENTRACE
//#define VERBOSEEMIT // Legacy
//#define REGISTEREMIT // Legacy
#define STATS
#if STATS
// note that instruction count will be wrong without this, but it's slow
#define ACCURATESTATS
#define STATSDIALOG
#endif

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
using Noxa.Emulation.Psp.Debugging;

namespace Noxa.Emulation.Psp.Cpu
{
	partial class Cpu : ICpu
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

		protected IDebugger _debugger;
		protected ExecutionMode _executionMode;
		protected int _executionParameter;
		internal bool _debugging;
		protected AutoResetEvent _debugWait;
		private bool _firstExecute;

		public event EventHandler<BreakpointEventArgs> BreakpointTriggered;

		#region Statistics

#if STATS
		protected double _timeSinceLastIpsPrint;
#endif

		protected PerformanceTimer _timer;
		protected RuntimeStatistics _stats;

		protected class RuntimeStatistics : ICpuStatistics
		{
			public int InstructionsExecuted;
			public int CodeBlocksExecuted;
			public int CodeBlocksGenerated;

			public int BreaksFromLength;
			public int BreaksFromBranches;
			public int BreaksFromJumps;
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

			#region ICpuStatistics Members

			int ICpuStatistics.InstructionsPerSecond
			{
				get
				{
					return ( int )InstructionsPerSecond;
				}
			}

			#endregion
		}

		#endregion

		public Cpu( IEmulationInstance emulator, ComponentParameters parameters )
		{
			Debug.Assert( emulator != null );
			Debug.Assert( parameters != null );

			_caps = new CpuCapabilities();
			_stats = new RuntimeStatistics();
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

			_executionMode = ExecutionMode.Run;
			_firstExecute = true;
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

			public bool InternalMemorySupported
			{
				get
				{
					return true;
				}
			}

			public CpuStatisticsCapabilities SupportedStatistics
			{
				get
				{
#if STATS
					return CpuStatisticsCapabilities.InstructionsPerSecond;
#else
					return CpuStatisticsCapabilities.None;
#endif
				}
			}

			public bool DebuggingSupported
			{
				get
				{
					return true;
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

		public Type Factory
		{
			get
			{
				return typeof( DynRecCpu );
			}
		}

		public ICpuCapabilities Capabilities
		{
			get
			{
				return _caps;
			}
		}

		public ICpuStatistics Statistics
		{
			get
			{
				return _stats;
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

		public byte[] InternalMemory
		{
			get
			{
				return _memory._mainMemory;
			}
		}

		public int InternalMemoryBaseAddress
		{
			get
			{
				return Psp.Cpu.Memory.MainMemoryBaseAddress;
			}
		}

		public CodeCache CodeCache
		{
			get
			{
				return _codeCache;
			}
		}

		public ExecutionMode ExecutionMode
		{
			get
			{
				return _executionMode;
			}
			set
			{
				_executionMode = value;
			}
		}

		public int ExecutionParameter
		{
			get
			{
				return _executionParameter;
			}
			set
			{
				_executionParameter = value;
			}
		}

		public bool DebuggingEnabled
		{
			get
			{
				return _debugging;
			}
		}

		public void EnableDebugging()
		{
			_debugger = _emulator.Host.Debugger;
			Debug.Assert( _debugger != null );
			if( _debugger == null )
				throw new InvalidOperationException( "Debugger is not attached." );

			_debugger.Control.BreakpointAdded += new EventHandler<BreakpointEventArgs>( DebuggerControlBreakpointAdded );
			_debugger.Control.BreakpointRemoved += new EventHandler<BreakpointEventArgs>( DebuggerControlBreakpointRemoved );
			_debugger.Control.BreakpointToggled += new EventHandler<BreakpointEventArgs>( DebuggerControlBreakpointToggled );

			_codeCache.Clear();
			_debugging = true;
			_debugWait = new AutoResetEvent( false );

			_executionMode = ExecutionMode.Step;
		}

		private void DebuggerControlBreakpointAdded( object sender, BreakpointEventArgs e )
		{
			_codeCache.Invalidate( e.Breakpoint.Address );
		}

		private void DebuggerControlBreakpointRemoved( object sender, BreakpointEventArgs e )
		{
			_codeCache.Invalidate( e.Breakpoint.Address );
		}

		private void DebuggerControlBreakpointToggled( object sender, BreakpointEventArgs e )
		{
			_codeCache.Invalidate( e.Breakpoint.Address );
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

		public void Resume()
		{
			// We add special stepping breakpoints

			switch( _executionMode )
			{
				case ExecutionMode.Run:
					break;
				case ExecutionMode.RunUntil:
					_debugger.Control.AddSteppingBreakpoint( _executionParameter );
					break;
				case ExecutionMode.Step:
					_debugger.Control.AddSteppingBreakpoint( _core0.Pc + 4 );
					break;
				case ExecutionMode.StepN:
					throw new NotSupportedException();
			}
			_debugWait.Set();
		}

		public void Break()
		{
			throw new NotImplementedException();
		}

		public int ExecuteBlock()
		{
			// This happens after syscall

			if( _debugging == true )
			{
				// Always break on the first execute
				if( _firstExecute == true )
				{
					_firstExecute = false;
					_debugWait.WaitOne();
				}
			}

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
#endif

				block.ExecutionCount++;

				//Debug.WriteLine( string.Format( "Running block 0x{0:X8}", block.Address ) );

#if ACCURATESTATS
				_core0.BlockCounter = 0;
#endif

				int ret = block.Pointer( _core0, _memory, _core0.Registers, _syscalls );
				if( ( ret == 0 ) &&
					( block.EndsOnSyscall == false ) )
				{
					// Need to manually update PC
					_core0.Pc += 4 * block.InstructionCount;
				}

#if STATS
				_stats.InstructionsExecuted += _core0.BlockCounter;
#endif

#if ACCURATESTATS
				count += _core0.BlockCounter;
#else
				count += block.InstructionCount;
#endif

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

			_stats.InstructionsPerSecond = ( _stats.InstructionsPerSecond * .8 ) + ( ( ( double )count / blockTime ) * .2 );
			
			_timeSinceLastIpsPrint += blockTime;
			if( _timeSinceLastIpsPrint > 1.0 )
			{
				Debug.WriteLine( string.Format( "IPS: {0}", ( long )_stats.InstructionsPerSecond ) );
				_timeSinceLastIpsPrint -= 1.0;
			}
#endif

			return count;
		}

		public void PrintStatistics()
		{
#if STATS
			if( _stats.InstructionsExecuted == 0 )
				return;
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
#if STATSDIALOG
			System.Windows.Forms.MessageBox.Show( sb.ToString() );
#else
			Debug.WriteLine( sb.ToString() );
#endif
#endif
		}

		/// <summary>
		/// Maximum number of instructions before bailing.
		/// </summary>
		protected const int MaximumCodeLength = 600;

		private Type[] _methodParams = new Type[] { typeof( Core ), typeof( Memory ), typeof( int[] ), typeof( BiosFunction[] ) };

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
			
#if GENTRACE
			Debug.WriteLine( string.Format( "Starting generate for block at 0x{0:X8}", startAddress ) );
#endif

			bool jumpDelay = false;
			GenerationResult lastResult = GenerationResult.Success;
			for( int pass = 0; pass <= 1; pass++ )
			{
				// Only have the information to generate the preamble on pass 1
				if( pass == 1 )
				{
					_context.Prepare( 0 );
					GeneratePreamble( _context );
				}

				_context.InDelay = false;
				bool breakOut = false;
				bool checkNullDelay = false;
				int address = startAddress;
				for( int n = 0; n < MaximumCodeLength; n++ )
				{
					GenerationResult result = GenerationResult.Invalid;

#if ACCURATESTATS
					if( pass == 1 )
					{
						ilgen.Emit( OpCodes.Ldloc_S, ( byte )LocalBlockCounter );
						ilgen.Emit( OpCodes.Ldc_I4_1 );
						ilgen.Emit( OpCodes.Add );
						ilgen.Emit( OpCodes.Stloc_S, ( byte )LocalBlockCounter );
					}
#endif

					Label nullDelaySkip = default( Label );
					if( pass == 1 )
					{
						nullDelaySkip = ilgen.DefineLabel();
						if( checkNullDelay == true )
						{
							ilgen.Emit( OpCodes.Ldloc_S, ( byte )LocalNullifyDelay );
							ilgen.Emit( OpCodes.Brtrue, nullDelaySkip );
						}
					}

					if( pass == 0 )
					{
						// We need to mark labels asap as pass 2 could have back branches
						if( _context.BranchLabels.ContainsKey( address ) == true )
						{
							LabelMarker lm = _context.BranchLabels[ address ];
							//Debug.WriteLine( string.Format( "Marking label for branch target {0:X8}", address ) );
							lm.Found = true;
						}
					}
					else if( pass == 1 )
					{
						// Note that it shouldn't be possible to be here if we
						// are in a delay slot, hence the branch being above
						if( _context.BranchLabels.ContainsKey( address ) == true )
						{
							LabelMarker lm = _context.BranchLabels[ address ];
							//Debug.WriteLine( string.Format( "Marking label for branch target {0:X8}", address ) );
							ilgen.MarkLabel( lm.Label );
						}
					}

					// Must be after the nulldelay/branching stuff
					if( ( pass == 1 ) && ( _debugging == true ) )
					{
						Breakpoint bp = _debugger.Control.FindBreakpoint( address );
						if( bp != null )
						{
							this.EmitBreakpoint( _context, bp );
						}
					}

					bool inDelay = _context.InDelay;
					uint code = ( uint )_memory.ReadWord( address );

					if( code != 0 )
					{
						uint opcode = ( code >> 26 ) & 0x3F;
						bool isCop = ( opcode == 0x10 ) || ( opcode == 0x11 ) || ( opcode == 0x12 );

						if( isCop == false )
						{
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
										//if( pass == 1 )
										//	EmitDebugInfo( _context, address, code, instr.Method.Name, string.Format( "rs:{0} rt:{1} rd:{2} shamt:{3}", rs, rt, rd, shamt ) );
										result = instr( _context, pass, address + 4, code, ( byte )opcode, rs, rt, rd, shamt, function );
									}
									break;
								case 1: // J type
									{
										uint imm = code & 0x03FFFFFF;
										uint rt = ( code >> 16 ) & 0x1F;

										GenerateInstructionJ instr = CoreInstructions.TableJ[ rt ];
										//if( pass == 1 )
											//EmitDebugInfo( _context, address, code, instr.Method.Name, string.Format( "imm:{0}", imm ) );
										result = instr( _context, pass, address + 4, code, ( byte )opcode, imm );
									}
									break;
								case 0x1C: // Allegrex special type (follows R convention)
									{
										byte function = ( byte )( code & 0x3F );
										byte rs = ( byte )( ( code >> 21 ) & 0x1F );
										byte rt = ( byte )( ( code >> 16 ) & 0x1F );
										byte rd = ( byte )( ( code >> 11 ) & 0x1F );

										GenerateInstructionR instr = CoreInstructions.TableAllegrex[ function ];
										//if( pass == 1 )
											//EmitDebugInfo( _context, address, code, instr.Method.Name, string.Format( "rs:{0} rt:{1} rd:{2}", rs, rt, rd ) );
										result = instr( _context, pass, address + 4, code, ( byte )opcode, rs, rt, rd, 0, function );
									}
									break;
								case 0x1F: // SPECIAL3 type
									{
										byte rt = ( byte )( ( code >> 16 ) & 0x1F );
										byte rd = ( byte )( ( code >> 11 ) & 0x1F );
										byte function = ( byte )( ( code >> 6 ) & 0x1F );
										uint bshfl = code & 0x3F;

										// Annoying....
										GenerateInstructionSpecial3 instr;
										if( ( bshfl == 0x0 ) ||
											( bshfl == 0x4 ) )
											instr = CoreInstructions.TableSpecial3[ bshfl ];
										else
											instr = CoreInstructions.TableSpecial3[ function ];
										//if( pass == 1 )
											//EmitDebugInfo( _context, address, code, instr.Method.Name, string.Format( "func:{0}", function ) );
										result = instr( _context, pass, address + 4, code, rt, rd, function, ( ushort )bshfl );
									}
									break;
								default: // Common I type
									{
										byte rs = ( byte )( ( code >> 21 ) & 0x1F );
										byte rt = ( byte )( ( code >> 16 ) & 0x1F );
										ushort imm = ( ushort )( code & 0xFFFF );

										GenerateInstructionI instr = CoreInstructions.TableI[ opcode ];
										//if( pass == 1 )
											//EmitDebugInfo( _context, address, code, instr.Method.Name, string.Format( "rs:{0} rt:{1} imm:{2}", rs, rt, ( int )( short )imm ) );
										result = instr( _context, pass, address + 4, code, ( byte )opcode, rs, rt, imm );
									}
									break;
							}
						}
						else
						{
							result = CoreInstructions.Cop.HandleInstruction( _context, pass, address + 4, code );
						}

						if( result == GenerationResult.Invalid )
						{
							// Failed to generate - we break ASAP!
							Debug.WriteLine( string.Format( "GenerateBlock: failed to generate IL for [0x{0:X8}] {1:X8}", address, code ) );
							Debugger.Break();
							break;
						}
					}
					else
					{
						//if( pass == 1 )
							//EmitDebugInfo( _context, address, code, "NOP", "" );
					}

					// This will print out NOP's when we are in a null delay slot
					if( pass == 1 )
					{
						if( checkNullDelay == true )
						{
							Label nullDelayHuhhhh = ilgen.DefineLabel();
							ilgen.Emit( OpCodes.Br, nullDelayHuhhhh );
							ilgen.MarkLabel( nullDelaySkip );
							//EmitDebugInfo( _context, address, 0x0, "NOP", "" );
							ilgen.Emit( OpCodes.Ldc_I4_0 );
							ilgen.Emit( OpCodes.Stloc_S, ( byte )LocalNullifyDelay );
							ilgen.MarkLabel( nullDelayHuhhhh );
						}
					}

					if( pass == 0 )
					{
						_context.InstructionCount++;
						_context.EndAddress = address;
					}

					address += 4;

					// Branch check/delay handling
					if( pass == 1 )
					{
						// Have to use local inDelay because the last instruction could have been the one to set it
						// Could also be in a jump delay, which only happens on non-breakout jumps
						if( jumpDelay == true )
						{
							//Debug.WriteLine( "Marking jump delay tail" );
							GenerateTail( _context );

							_context.InDelay = false;
							jumpDelay = false;
						}
						else if( inDelay == true )
						{
							if( _context.IsBranchLocal( _context.BranchTarget.Address ) == true )
							{
								Label noBranch = ilgen.DefineLabel();
								ilgen.Emit( OpCodes.Ldloc_3 );
								ilgen.Emit( OpCodes.Ldc_I4_0 );
								ilgen.Emit( OpCodes.Beq_S, noBranch );
								ilgen.Emit( OpCodes.Ldc_I4_0 );
								ilgen.Emit( OpCodes.Stloc_3 );
								ilgen.Emit( OpCodes.Br, _context.BranchTarget.Label );
								ilgen.MarkLabel( noBranch );

								_context.InDelay = false;
								_context.BranchTarget = null;
							}
							else
							{
								//Debug.WriteLine( string.Format( "Aborting block early because branch target {0:X8} not found", _context.BranchTarget.Address ) );
								Debug.Assert( _context.BranchTarget.Address != 0 );

								Label noBranch = ilgen.DefineLabel();
								ilgen.Emit( OpCodes.Ldloc_3 );
								ilgen.Emit( OpCodes.Ldc_I4_0 );
								ilgen.Emit( OpCodes.Beq_S, noBranch );
								ilgen.Emit( OpCodes.Ldc_I4_1 );
								ilgen.Emit( OpCodes.Stloc_3 );
								ilgen.Emit( OpCodes.Ldc_I4, _context.BranchTarget.Address );
								ilgen.Emit( OpCodes.Stloc_2 );
								
								// This is an early exit
								GenerateTail( _context );
								ilgen.Emit( OpCodes.Ret );

								ilgen.MarkLabel( noBranch );

								_context.InDelay = false;
								_context.BranchTarget = null;
							}
						}
					}

					// Syscalls always exit
					if( result == GenerationResult.Syscall )
					{
						lastResult = result;
						break;
					}

					// This is so that we can handle delay slots
					if( breakOut == true )
						break;

					switch( result )
					{
						case GenerationResult.Branch:
							_context.InDelay = true;
							lastResult = result;
							break;
						case GenerationResult.Jump:
							// This is tricky - if lastTargetPc > currentPc, don't break out
							if( _context.LastBranchTarget <= address )
							{
								//Debug.WriteLine( string.Format( "Jump breakout at {0:X8}", address ) );
								breakOut = true;
								lastResult = result;
							}
							else
							{
								//Debug.WriteLine( string.Format( "Ignoring jump breakout at {0:X8} because last target is {1:X8}", address, _context.LastBranchTarget ) );
								if( pass == 1 )
								{
									jumpDelay = true;
									_context.InDelay = true;
								}
							}
							break;
						case GenerationResult.BranchAndNullifyDelay:
							_context.InDelay = true;
							if( pass == 1 )
								checkNullDelay = true;
							lastResult = result;
							break;
					}
				}

				if( pass == 1 )
					GenerateTail( _context );
			}

#if DEBUG
			block.DynamicMethod = method;
#endif

			block.EndsOnSyscall = ( lastResult == GenerationResult.Syscall );
			block.InstructionCount = _context.InstructionCount;
			
			block.Pointer = ( DynamicCodeDelegate )method.CreateDelegate( typeof( DynamicCodeDelegate ) );
			_codeCache.Add( block );

#if STATS
			_stats.CodeBlocksGenerated++;

			double genTime = _timer.Elapsed - genStart;
			if( genTime <= 0.0 )
				genTime = 0.000001;
			_stats.AverageGenerationTime += genTime;

			switch( lastResult )
			{
				case GenerationResult.Success:
					_stats.BreaksFromLength++;
					break;
				case GenerationResult.Syscall:
					_stats.BreaksFromSyscalls++;
					break;
				case GenerationResult.Branch:
				case GenerationResult.BranchAndNullifyDelay:
					_stats.BreaksFromBranches++;
					break;
				case GenerationResult.Jump:
					_stats.BreaksFromJumps++;
					break;
			}
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
		public const int LocalTempD1 = 5;
		public const int LocalTempD2 = 6;
		public const int LocalTempF = 7;
		public const int LocalTempL = 8;
#if ACCURATESTATS
		public const int LocalBlockCounter = 9;	// Must always be last
#endif

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
			ilgen.DeclareLocal( typeof( float ) );
			ilgen.DeclareLocal( typeof( float ) );
			ilgen.DeclareLocal( typeof( float ) );
			ilgen.DeclareLocal( typeof( long ) );
#if ACCURATESTATS
			ilgen.DeclareLocal( typeof( int ) );
			ilgen.Emit( OpCodes.Ldc_I4_0 );
			ilgen.Emit( OpCodes.Stloc_S, ( byte )LocalBlockCounter );
#endif

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

#if ACCURATESTATS
			ilgen.Emit( OpCodes.Ldarg_0 );
			ilgen.Emit( OpCodes.Ldloc_S, ( byte )LocalBlockCounter );
			ilgen.Emit( OpCodes.Stfld, context.Core0BlockCounter );
#endif

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

		protected void EmitBreakpoint( GenerationContext context, Breakpoint breakpoint )
		{
			ILGenerator ilgen = context.ILGen;

			Debug.Assert( context.Cpu._debugging == true );

			ilgen.Emit( OpCodes.Ldarg_0 );
			ilgen.Emit( OpCodes.Ldfld, context.Core0Cpu );
			ilgen.Emit( OpCodes.Ldc_I4, breakpoint.Address );
			ilgen.Emit( OpCodes.Call, context.DebugBreak );
		}

		public void DebugBreak( int address )
		{
			Breakpoint bp = _debugger.Control.FindBreakpoint( address );
			Debug.Assert( bp != null );
			if( bp == null )
				return;
			if( bp.Enabled == false )
				return;

			this.BreakpointTriggered( this, new BreakpointEventArgs( bp ) );
			if( bp.Type == BreakpointType.Stepping )
				_debugger.Control.RemoveBreakpoint( bp.Address );
			_debugWait.WaitOne();
		}

		#region Legacy debug code

//        [Conditional( "VERBOSEEMIT" )]
//        internal void EmitDebugInfo( GenerationContext context, int address, uint code, string name, string args )
//        {
//#if DEBUG
//            if( _debug == false )
//                return;
//#endif

//            string line = string.Format( "[0x{0:X8}] {1:X8} {2:8} {3}",
//                address, code, name, args );
//            //string line = string.Format( "0x{0:X8}: {1:X8}",
//            //	address, code );

//            context.ILGen.Emit( OpCodes.Ldstr, line );
//            context.ILGen.Emit( OpCodes.Call, context.DebugWriteLine );
//        }

//        [Conditional( "REGISTEREMIT" )]
//        protected void EmitRegisterPrint( ILGenerator ilgen )
//        {
//            ilgen.Emit( OpCodes.Ldstr, "0={0:X8} 1={1:X8} 2={2:X8} 3={3:X8} 4={4:X8} 5={5:X8} 6={6:X8} 7={7:X8} 8={8:X8} 9={9:X8} 10={10:X8} 11={11:X8} 12={12:X8} 13={13:X8} 14={14:X8} 15={15:X8} 16={16:X8} 17={17:X8} 18={18:X8} 19={19:X8} 20={20:X8} 21={21:X8} 22={22:X8} 23={23:X8} 24={24:X8} 25={25:X8} 26={26:X8} 27={27:X8} 28={28:X8} 29={29:X8} 30={30:X8} 31={31:X8}" );
//            ilgen.Emit( OpCodes.Ldc_I4, 32 );
//            ilgen.Emit( OpCodes.Newarr, typeof( object ) );
//            ilgen.Emit( OpCodes.Stloc_0 );
//            for( int r = 0; r < 32; r++ )
//                EmitRegister( ilgen, r );
//            ilgen.Emit( OpCodes.Ldloc_0 );
//            ilgen.Emit( OpCodes.Call, typeof( String ).GetMethod( "Format", new Type[] { typeof( string ), typeof( object[] ) } ) );
//            ilgen.Emit( OpCodes.Call, _context.DebugWriteLine );
//        }

//        [Conditional( "REGISTEREMIT" )]
//        private void EmitRegister( ILGenerator ilgen, int n )
//        {
//            ilgen.Emit( OpCodes.Ldloc_0 );
//            ilgen.Emit( OpCodes.Ldc_I4, n );
//            CoreInstructions.EmitLoadRegister( _context, n );
//            ilgen.Emit( OpCodes.Box, typeof( Int32 ) );
//            ilgen.Emit( OpCodes.Stelem_Ref );
//        }

		#endregion

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
