// ----------------------------------------------------------------------------
// Shared Utility Library
// Copyright (C) 2008 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace Noxa
{
	/// <summary>
	/// An unmanaged ring buffer.
	/// </summary>
	public unsafe class RingBuffer
	{
		private byte* _buffer;
		private int _bufferSize;
		private int _packetSize;
		private int _packetCount;
		private int _readPosition;
		private int _writePosition;

		/// <summary>
		/// Initializes a new <see cref="RingBuffer"/> instance with the given parameters.
		/// </summary>
		/// <param name="buffer">The pointer to the pre-allocated buffer.</param>
		/// <param name="bufferSize">The size of <c>buffer</c> in bytes.</param>
		/// <param name="packetSize">The size of each packet in the buffer.</param>
		/// <param name="packetCount">The number of packets contained within the buffer.</param>
		public RingBuffer( byte* buffer, int bufferSize, int packetSize, int packetCount )
		{
			_buffer = buffer;
			_bufferSize = bufferSize;
			_packetSize = packetSize;
			_packetCount = packetCount;
		}

		/// <summary>
		/// Gets the number of packets currently in the buffer.
		/// </summary>
		public int Count
		{
			get
			{
				int count = _writePosition - _readPosition;
				if( count < 0 )
					count += _packetCount;
				return count;
			}
		}

		/// <summary>
		/// Get a value determining whether or not the buffer is empty.
		/// </summary>
		public bool IsEmpty { get { return ( _readPosition == _writePosition ); } }

		/// <summary>
		/// Get a pointer to the packet starting at the given index, relative to the current read position.
		/// </summary>
		/// <param name="index">The index of the packet.</param>
		/// <returns>A pointer to the packet starting at the given index.</returns>
		public byte* this[ int index ]
		{
			get
			{
				return _buffer + ( ( ( _readPosition + index ) % _packetCount ) * _packetSize );
			}
		}

		/// <summary>
		/// Get the address of the packet at the head of the buffer without incrementing the read position.
		/// </summary>
		/// <returns>The address of the next packet.</returns>
		public byte* Peek()
		{
			return this[ 0 ];
		}

		/// <summary>
		/// Get the address of the packet at the head of the buffer and increment the read position.
		/// </summary>
		/// <returns>The address of the next packet.</returns>
		public byte* Read()
		{
			byte* result = this[ 0 ];
			_readPosition = ( _readPosition + 1 ) % _packetCount;
			return result;
		}

		/// <summary>
		/// Get the address of the write packet and increment the write position by the given packet count.
		/// </summary>
		/// <param name="count">The number of packets that will be written.</param>
		/// <returns>The address of the packet to start writing at.</returns>
		public byte* Write( int count )
		{
			byte* result = _buffer + ( _writePosition * _packetSize );
			_writePosition = ( _writePosition + count ) % _packetCount;
			return result;
		}

		/// <summary>
		/// Get the number of packets currently stored in the ring buffer.
		/// </summary>
		/// <param name="packetCount">Packet capacity of the ring buffer.</param>
		/// <param name="readPosition">Current read position.</param>
		/// <param name="writePosition">Current write position.</param>
		/// <returns>The number of packets stored in the ring buffer.</returns>
		public static int GetCount( int packetCount, int readPosition, int writePosition )
		{
			int count = writePosition - readPosition;
			if( count < 0 )
				count += packetCount;
			return count;
		}

		/// <summary>
		/// Get the address of the packet at the head of the buffer and increment the read position.
		/// </summary>
		/// <param name="buffer">The ringbuffer pointer.</param>
		/// <param name="packetCount">Packet capacity of the ring buffer.</param>
		/// <param name="packetSize">The size of each packet in the ringbuffer.</param>
		/// <param name="readPosition">The current read position of the buffer.</param>
		/// <returns>The address of the next packet.</returns>
		public static byte* Read( byte* buffer, int packetCount, int packetSize, ref int readPosition )
		{
			byte* result = buffer + ( ( readPosition % packetCount ) * packetSize );
			readPosition = ( readPosition + 1 ) % packetCount;
			return result;
		}

		/// <summary>
		/// Get the address of the write packet.
		/// </summary>
		/// <param name="buffer">The ringbuffer pointer.</param>
		/// <param name="packetCount">Packet capacity of the ring buffer.</param>
		/// <param name="packetSize">The size of each packet in the ringbuffer.</param>
		/// <param name="writePosition">The current write position of the buffer.</param>
		/// <returns>The address of the packet to start writing at.</returns>
		public static byte* BeginWrite( byte* buffer, int packetCount, int packetSize, int writePosition )
		{
			return buffer + ( ( writePosition % packetCount ) * packetSize );
		}

		/// <summary>
		/// Increment the write position by the given packet count.
		/// </summary>
		/// <param name="buffer">The ringbuffer pointer.</param>
		/// <param name="packetCount">Packet capacity of the ring buffer.</param>
		/// <param name="packetSize">The size of each packet in the ringbuffer.</param>
		/// <param name="writePosition">The current write position of the buffer.</param>
		/// <param name="count">The number of packets that will be written.</param>
		public static void EndWrite( byte* buffer, int packetCount, int packetSize, ref int writePosition, int count )
		{
			writePosition = ( writePosition + count ) % packetCount;
		}
	}
}
