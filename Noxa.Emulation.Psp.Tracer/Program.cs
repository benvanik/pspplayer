// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2008 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace Noxa.Emulation.Psp.Tracer
{
	enum Tool
	{
		Write,
		Find,
	}

	unsafe struct BaseLine
	{
		public uint ThreadID;
		public uint Address;
		public uint Code;
		public uint NextPC;
		public uint InDelay;
		public uint NullifyDelay;
	}

	[StructLayout( LayoutKind.Sequential )]
	unsafe struct RegisterLine
	{
		public uint HI;
		public uint LO;
		public fixed uint Registers[ 32 ];
	}

	[StructLayout( LayoutKind.Sequential )]
	unsafe struct FpuLine
	{
		public uint ConditionBit;
		public fixed float Registers[ 32 ];
	}

	[StructLayout( LayoutKind.Sequential )]
	unsafe struct VfpuLine
	{
		public uint ConditionBit;
		public uint Wm;
		public int PfxS;
		public int PfxT;
		public int PfxD;
		public fixed float Registers[ 128 ];
	}

	class Program
	{
		// write [all/first/last/from/range] [parameter1] [parameter2]
		//       first/last: parameter1=lines from start/end
		//       from: parameter1=start instruction #
		//       range: parameter1=start instruction #, parameter2=end instruction #
		// find [register] [parameter1]
		//       register: parameter1=value of register

		private static string[] RegisterNames = new string[] { "$0", "$at", "$v0", "$v1", "$a0", "$a1", "$a2", "$a3", "$t0", "$t1", "$t2", "$t3", "$t4", "$t5", "$t6", "$t7", "$s0", "$s1", "$s2", "$s3", "$s4", "$s5", "$s6", "$s7", "$t8", "$t9", "$k0", "$k1", "$gp", "$sp", "$fp", "$ra" };

		private int _lineSize;
		private byte[] _lineBuffer;

		private bool _traceRegisters;
		private bool _traceFpu;
		private bool _traceVfpu;

		private unsafe void ReadInstruction( byte* ptr, out BaseLine* baseLine, out RegisterLine* registers, out FpuLine* fpu, out VfpuLine* vfpu )
		{
			baseLine = ( ( BaseLine* )ptr );
			ptr += sizeof( BaseLine );

			if( _traceRegisters == true )
			{
				registers = ( ( RegisterLine* )ptr );
				ptr += sizeof( RegisterLine );
			}
			else
				registers = null;
			if( _traceFpu == true )
			{
				fpu = ( ( FpuLine* )ptr );
				ptr += sizeof( FpuLine );
			}
			else
				fpu = null;
			if( _traceVfpu == true )
			{
				vfpu = ( ( VfpuLine* )ptr );
				ptr += sizeof( VfpuLine );
			}
			else
				vfpu = null;
		}

		private unsafe void Write( FileStream inputStream, StreamWriter writer, string mode, int parameter1, int parameter2, byte* basePtr )
		{
			long length = inputStream.Length;
			long position = 0;

			if( mode == "last" )
			{
				// Skip to the end, go back N
				inputStream.Seek( -_lineSize * parameter1, SeekOrigin.End );
				position = inputStream.Position;
			}
			else if( mode == "from" )
			{
				inputStream.Seek( _lineSize * parameter1, SeekOrigin.Begin );
				position = inputStream.Position;
			}
			else if( mode == "range" )
			{
				inputStream.Seek( _lineSize * parameter1, SeekOrigin.Begin );
				position = inputStream.Position;
			}

			int count = 0;
			while( position < length )
			{
				inputStream.Read( _lineBuffer, 0, _lineBuffer.Length );
				position += _lineBuffer.Length;

				BaseLine* baseLine;
				RegisterLine* registers;
				FpuLine* fpu;
				VfpuLine* vfpu;
				this.ReadInstruction( basePtr, out baseLine, out registers, out fpu, out vfpu );

				writer.WriteLine( "({0:X4}) [{1:X8}] {2:X8}", baseLine->ThreadID, baseLine->Address, baseLine->Code );

				if( _traceRegisters == true )
				{
					for( int n = 1; n < 32; n++ )
					{
						writer.Write( RegisterNames[ n ] + "=" + registers->Registers[ n ].ToString( "X8" ) );
						if( n < 31 )
							writer.Write( ',' );
						if( n % 9 == 0 )
							writer.WriteLine();
					}
					writer.WriteLine();
				}
				if( _traceFpu == true )
				{
				}
				if( _traceVfpu == true )
				{
				}

				count++;
				if( mode == "first" )
				{
					// If we've gone past N, finish
					if( count > parameter1 )
						break;
				}
				else if( mode == "range" )
				{
					// If we are past the end, finish
					if( count > ( parameter2 - parameter1 ) )
						break;
				}
			}
		}

		private unsafe void Find( FileStream inputStream, StreamWriter writer, string mode, int parameter1, int parameter2, byte* basePtr )
		{
			long length = inputStream.Length;
			long position = 0;

			int count = 0;
			while( position < length )
			{
				inputStream.Read( _lineBuffer, 0, _lineBuffer.Length );
				position += _lineBuffer.Length;

				BaseLine* baseLine;
				RegisterLine* registers;
				FpuLine* fpu;
				VfpuLine* vfpu;
				this.ReadInstruction( basePtr, out baseLine, out registers, out fpu, out vfpu );

				bool isMatch = false;

				if( _traceRegisters == true )
				{
					for( int n = 1; n < 32; n++ )
					{
						if( registers->Registers[ n ] == parameter1 )
						{
							isMatch = true;
							break;
						}
					}
				}
				if( _traceFpu == true )
				{
				}
				if( _traceVfpu == true )
				{
				}

				if( isMatch == true )
				{
					writer.WriteLine( count.ToString() );
				}

				count++;
			}
		}

		private unsafe void Run( string inputFile, string outputFile, bool traceRegisters, bool traceFpu, bool traceVfpu, Tool tool, string mode, int parameter1, int parameter2 )
		{
			_traceRegisters = traceRegisters;
			_traceFpu = traceFpu;
			_traceVfpu = traceVfpu;

			_lineSize = sizeof( BaseLine );
			if( traceRegisters == true )
				_lineSize += sizeof( RegisterLine );
			if( traceFpu == true )
				_lineSize += sizeof( FpuLine );
			if( traceVfpu == true )
				_lineSize += sizeof( VfpuLine );
			_lineBuffer = new byte[ _lineSize ];

			using( FileStream inputStream = File.OpenRead( inputFile ) )
			using( FileStream outputStream = File.OpenWrite( outputFile ) )
			using( StreamWriter writer = new StreamWriter( outputStream ) )
			{
				fixed( byte* basePtr = &_lineBuffer[ 0 ] )
				{
					switch( tool )
					{
						case Tool.Write:
							this.Write( inputStream, writer, mode, parameter1, parameter2, basePtr );
							break;
						case Tool.Find:
							this.Find( inputStream, writer, mode, parameter1, parameter2, basePtr );
							break;
					}
				}
			}
		}

		unsafe static void Main( string[] args )
		{
			string inputFile = @"Z:\Laptop\Noxa.Emulation\trunk\debug\Current.trace";
			string outputFile = @"testr.txt";
			bool traceRegisters = true;
			bool traceFpu = false;
			bool traceVfpu = false;

			//Tool tool = Tool.Write;
			//string mode = "all";
			//int parameter1 = 1500;
			//int parameter2 = 0;

			Tool tool = Tool.Write;
			string mode = "range";
			int parameter1 = 63100;
			int parameter2 = 63300;

			//Tool tool = Tool.Find;
			//string mode = "register";
			//int parameter1 = 0x08A43438;
			//int parameter2 = 0;

			Program program = new Program();
			program.Run( inputFile, outputFile, traceRegisters, traceFpu, traceVfpu, tool, mode, parameter1, parameter2 );
		}
	}
}
