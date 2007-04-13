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
	/// <summary>
	/// Describes the type of a <see cref="IMemorySegment"/>.
	/// </summary>
	public enum MemoryType
	{
		/// <summary>
		/// Physical memory.
		/// </summary>
		PhysicalMemory,

		/// <summary>
		/// Hardware mapped memory.
		/// </summary>
		HardwareMapped
	}

	/// <summary>
	/// Callback type for memory changes.
	/// </summary>
	/// <param name="segment">Segment that contained the change.</param>
	/// <param name="address">The address of the change.</param>
	/// <param name="width">The width of the value, in bytes.</param>
	/// <param name="value">The new value.</param>
	public delegate void MemoryChangeDelegate( IMemorySegment segment, int address, int width, int value );

	/// <summary>
	/// A memory segment.
	/// </summary>
	public interface IMemorySegment
	{
		/// <summary>
		/// The type of the segment.
		/// </summary>
		MemoryType MemoryType
		{
			get;
		}

		/// <summary>
		/// The human-friendly name of the segment.
		/// </summary>
		string Name
		{
			get;
		}

		/// <summary>
		/// The base address of the segment.
		/// </summary>
		int BaseAddress
		{
			get;
		}

		/// <summary>
		/// The length, in bytes, of the segment.
		/// </summary>
		int Length
		{
			get;
		}

		/// <summary>
		/// Fired when a value in memory changes.
		/// </summary>
		event MemoryChangeDelegate MemoryChanged;

		/// <summary>
		/// Read a 32-bit word from the given address.
		/// </summary>
		/// <param name="address">Address to read from.</param>
		/// <returns>The value at the given address.</returns>
		int ReadWord( int address );

		/// <summary>
		/// Read a 64-bit double word from the given address.
		/// </summary>
		/// <param name="address">Address to read from.</param>
		/// <returns>The value at the given address.</returns>
		long ReadDoubleWord( int address );

		/// <summary>
		/// Read a block of bytes from the given address.
		/// </summary>
		/// <param name="address">Address to start reading from.</param>
		/// <param name="count">The number of bytes to read.</param>
		/// <returns>A buffer containing the values at the in the address range.</returns>
		byte[] ReadBytes( int address, int count );

		/// <summary>
		/// Read a block of bytes from the given address and write them to the given stream.
		/// </summary>
		/// <param name="address">Address to start reading from.</param>
		/// <param name="destination">The <see cref="Stream"/> to write to.</param>
		/// <param name="count">The number of bytes to read.</param>
		/// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
		int ReadStream( int address, Stream destination, int count );

		/// <summary>
		/// Write a 8-,16-, or 32-bit value to the given address.
		/// </summary>
		/// <param name="address">The address to write to.</param>
		/// <param name="width">The width of the value, in bytes.</param>
		/// <param name="value">The value to write.</param>
		void WriteWord( int address, int width, int value );

		/// <summary>
		/// Write a 64-bit double word to the given address.
		/// </summary>
		/// <param name="address">The address to write to.</param>
		/// <param name="value">The value to write.</param>
		void WriteDoubleWord( int address, long value );

		/// <summary>
		/// Write all the bytes from the given array to the given address.
		/// </summary>
		/// <param name="address">The address to begin writing at.</param>
		/// <param name="bytes">The source byte array.</param>
		void WriteBytes( int address, byte[] bytes );

		/// <summary>
		/// Write the given range of bytes from the given array to the given address.
		/// </summary>
		/// <param name="address">The address to begin writing at.</param>
		/// <param name="bytes">The source byte array.</param>
		/// <param name="index">The index to start at in the given byte array.</param>
		/// <param name="count">The number of bytes to write.</param>
		void WriteBytes( int address, byte[] bytes, int index, int count );

		/// <summary>
		/// Write the stream to the given address.
		/// </summary>
		/// <param name="address">The address to begin writing at.</param>
		/// <param name="source">The source stream.</param>
		/// <param name="count">The number of bytes to write.</param>
		void WriteStream( int address, Stream source, int count );

		/// <summary>
		/// Generate a hash of the memory at the given address.
		/// </summary>
		/// <param name="address">The address to start generating the hash at.</param>
		/// <param name="count">The number of bytes to include.</param>
		/// <param name="prime">The prime used while hashing.</param>
		/// <returns>The hash of the memory or <c>0</c> if an error occurred.</returns>
		uint GetMemoryHash( int address, int count, uint prime );
	}
}
