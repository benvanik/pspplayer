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
	}

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
	}
}
