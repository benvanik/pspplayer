// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Noxa.Emulation.Psp.Cpu
{
	public interface IMemory
	{
		IMemorySegment DefineSegment( MemoryType type, string name, int baseAddress, int length );
		void RegisterSegment( IMemorySegment segment );
		IMemorySegment FindSegment( string name );
		IMemorySegment FindSegment( int baseAddress );

		unsafe void* MainMemoryPointer
		{
			get;
		}

		unsafe void* FrameBufferPointer
		{
			get;
		}

		int ReadWord( int address );
		long ReadDoubleWord( int address );
		byte[] ReadBytes( int address, int count );
		int ReadStream( int address, Stream destination, int count );
		void WriteWord( int address, int width, int value );
		void WriteDoubleWord( int address, long value );
		void WriteBytes( int address, byte[] bytes );
		void WriteBytes( int address, byte[] bytes, int index, int count );
		void WriteStream( int address, Stream source, int count );

		void Load( Stream stream );
		void Load( string fileName );
		void Save( Stream stream );
		void Save( string fileName );

		uint GetMemoryHash( int address, int count, uint prime );
	}
}
