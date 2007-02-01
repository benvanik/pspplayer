// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Noxa.Emulation.Psp.Cpu
{
	public enum MemoryType
	{
		PhysicalMemory,
		HardwareMapped
	}

	public delegate void MemoryChangeDelegate( IMemorySegment segment, int address, int width, int value );

	public interface IMemorySegment
	{
		MemoryType MemoryType
		{
			get;
		}

		string Name
		{
			get;
		}

		int BaseAddress
		{
			get;
		}

		int Length
		{
			get;
		}

		event MemoryChangeDelegate MemoryChanged;

		int ReadWord( int address );
		byte[] ReadBytes( int address, int count );
		int ReadStream( int address, Stream destination, int count );
		void WriteWord( int address, int width, int value );
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
