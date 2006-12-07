// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Noxa.Emulation.Psp.Bios;
using System.Reflection.Emit;

namespace Noxa.Emulation.Psp.Cpu
{
	delegate int DynamicCodeDelegate( Core core0, Memory memory, int[] generalRegisters, BiosFunction[] syscallList );

	enum GenerationMethod
	{
		DynamicMethod,
		CustomType
	}

	class CodeBlock
	{
		public int Address;
		public int InstructionCount;
		public DynamicCodeDelegate Pointer;
		public GenerationMethod MethodUsed;
		public bool EndsOnSyscall;
		public int ExecutionCount;

#if DEBUG
		public DynamicMethod DynamicMethod;
#endif
	}

	class CodeCache
	{
		// Cache is structured so that there are 3 levels - the address is shifted right 2 bits
		// (since all instructions are word-aligned) and then chunked in to 3 10-bit blocks.
		// This gives 3 levels of 1024 (2^10) entries.

		public CodeBlock[][][] Lookup;
		public object SyncRoot;

		public CodeCache()
		{
			Lookup = new CodeBlock[ 1024 ][][];
			SyncRoot = new object();
		}

		public void Add( CodeBlock block )
		{
			uint addr = ( uint )block.Address;
			addr >>= 2;

			uint b0 = addr >> 20;
			uint b1 = ( addr >> 10 ) & 0x3FF;
			uint b2 = addr & 0x3FF;

			lock( SyncRoot )
			{
				CodeBlock[][] block0 = Lookup[ b0 ];
				if( block0 == null )
				{
					block0 = new CodeBlock[ 1024 ][];
					Lookup[ b0 ] = block0;
				}

				CodeBlock[] block1 = block0[ b1 ];
				if( block1 == null )
				{
					block1 = new CodeBlock[ 1024 ];
					block0[ b1 ] = block1;
				}

				block1[ b2 ] = block;
			}
		}

		public CodeBlock Find( int address )
		{
			uint addr = ( uint )address;
			addr >>= 2;

			uint b0 = addr >> 20;
			uint b1 = ( addr >> 10 ) & 0x3FF;
			uint b2 = addr & 0x3FF;

			lock( SyncRoot )
			{
				CodeBlock[][] block0 = Lookup[ b0 ];
				if( block0 == null )
					return null;

				CodeBlock[] block1 = block0[ b1 ];
				if( block1 == null )
					return null;

				CodeBlock block = block1[ b2 ];

				return block;
			}
		}

		public void Invalidate( int address )
		{
			uint addr = ( uint )address;
			addr >>= 2;

			uint b0 = addr >> 20;
			uint b1 = ( addr >> 10 ) & 0x3FF;

			lock( SyncRoot )
			{
				CodeBlock[][] block0 = Lookup[ b0 ];
				if( block0 == null )
					return;

				CodeBlock[] block1 = block0[ b1 ];
				if( block1 == null )
					return;

				for( int n = 0; n < block1.Length; n++ )
				{
					CodeBlock block = block1[ n ];
					if( ( block.Address <= address ) &&
						( block.Address + block.InstructionCount >= address ) )
						block1[ n ] = null;
				}
			}
		}

		public void Clear()
		{
			lock( SyncRoot )
				Lookup = new CodeBlock[ 1024 ][][];
		}
	}
}
