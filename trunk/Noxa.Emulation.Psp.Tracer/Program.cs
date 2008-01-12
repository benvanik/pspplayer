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
	enum Action
	{
		All,
		LastN,
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
		unsafe static void Main( string[] args )
		{
			string inputFile = @"C:\Dev\Noxa.Emulation\trunk\debug\Current.trace";
			string outputFile = @"test.txt";
			bool traceRegisters = true;
			bool traceFpu = false;
			bool traceVfpu = false;
			Action action = Action.LastN;
			int parameter = 150;

			int lineSize = sizeof( BaseLine );
			if( traceRegisters == true )
				lineSize += sizeof( RegisterLine );
			if( traceFpu == true )
				lineSize += sizeof( FpuLine );
			if( traceVfpu == true )
				lineSize += sizeof( VfpuLine );

			byte[] buffer = new byte[ lineSize ];
			string[] registerNames = new string[] { "$0", "$at", "$v0", "$v1", "$a0", "$a1", "$a2", "$a3", "$t0", "$t1", "$t2", "$t3", "$t4", "$t5", "$t6", "$t7", "$s0", "$s1", "$s2", "$s3", "$s4", "$s5", "$s6", "$s7", "$t8", "$t9", "$k0", "$k1", "$gp", "$sp", "$fp", "$ra" };

			using( FileStream inputStream = File.OpenRead( inputFile ) )
			using( FileStream outputStream = File.OpenWrite( outputFile ) )
			using( StreamWriter writer = new StreamWriter( outputStream ) )
				fixed( byte* basePtr = &buffer[ 0 ] )
				{
					long length = inputStream.Length;
					long position = 0;

					if( action == Action.LastN )
					{
						// Skip to the end, go back 150
						inputStream.Seek( -lineSize * parameter, SeekOrigin.End );
						position = inputStream.Position;
					}

					while( position < length )
					{
						inputStream.Read( buffer, 0, buffer.Length );
						position += buffer.Length;

						byte* ptr = basePtr;
						BaseLine* baseLine = ( ( BaseLine* )ptr );
						ptr += sizeof( BaseLine );

						writer.WriteLine( "({0:X4}) [{1:X8}] {2:X8}", baseLine->ThreadID, baseLine->Address, baseLine->Code );

						if( traceRegisters == true )
						{
							RegisterLine* registers = ( ( RegisterLine* )ptr );
							ptr += sizeof( RegisterLine );

							for( int n = 1; n < 32; n++ )
							{
								writer.Write( registerNames[ n ] + "=" + registers->Registers[ n ].ToString( "X8" ) );
								if( n < 31 )
									writer.Write( ',' );
								if( n % 9 == 0 )
									writer.WriteLine();
							}
							writer.WriteLine();
						}
						if( traceFpu == true )
						{
							FpuLine* fpu = ( ( FpuLine* )ptr );
							ptr += sizeof( FpuLine );
						}
						if( traceVfpu == true )
						{
							VfpuLine* vfpu = ( ( VfpuLine* )ptr );
							ptr += sizeof( VfpuLine );
						}
					}
				}
		}
	}
}
