// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Noxa.Emulation.Psp.Cpu
{
	/// <summary>
	/// A utility class for accessing native host memory buffers.
	/// </summary>
	public unsafe class MemorySystem
	{
		#region Constants

		/// <summary>
		/// The lower bound of main memory.
		/// </summary>
		public const uint MainMemoryBase = 0x08000000;
		/// <summary>
		/// The size, in bytes, of main memory.
		/// </summary>
		public const uint MainMemorySize = 0x01FFFFFF;
		/// <summary>
		/// The upper bound of main memory.
		/// </summary>
		public const uint MainMemoryBound = ( MainMemoryBase + MainMemorySize );
		/// <summary>
		/// The lower bound of the scratch pad.
		/// </summary>
		public const uint ScratchPadBase = 0x00010000;
		/// <summary>
		/// The size, in bytes, of the scratch pad.
		/// </summary>
		public const uint ScratchPadSize = 0x00003FFF;
		/// <summary>
		/// The upper bound of the scratch pad.
		/// </summary>
		public const uint ScratchPadBound = ( ScratchPadBase + ScratchPadSize );
		/// <summary>
		/// The lower bound of video memory.
		/// </summary>
		public const uint VideoMemoryBase = 0x04000000;
		/// <summary>
		/// The size, in bytes, of video memory.
		/// </summary>
		public const uint VideoMemorySize = 0x001FFFFF;
		/// <summary>
		/// The upper bound of video memory.
		/// </summary>
		public const uint VideoMemoryBound = ( VideoMemoryBase + VideoMemorySize );

		#endregion

		/// <summary>
		/// A pointer to the host main memory buffer.
		/// </summary>
		public byte* MainMemory;
		/// <summary>
		/// A pointer to the host video memory buffer.
		/// </summary>
		public byte* VideoMemory;
		/// <summary>
		/// A pointer to the host scratch pad memory buffer.
		/// </summary>
		public byte* ScratchMemory;
		
		/// <summary>
		/// Initialize a new <see cref="MemorySystem"/> instance with the given memory addresses. 
		/// </summary>
		/// <param name="mainMemory">Main memory buffer.</param>
		/// <param name="videoMemory">Video memory buffer.</param>
		/// <param name="scratchMemory">Scratch pad memory buffer.</param>
		public MemorySystem( byte* mainMemory, byte* videoMemory, byte* scratchMemory )
		{
			MainMemory = mainMemory;
			VideoMemory = videoMemory;
			ScratchMemory = scratchMemory;
		}
		
		/// <summary>
		/// Translate a guest address known to be in main memory to a host address.
		/// </summary>
		/// <param name="guestAddress">The address, in guest space. Must be in main memory.</param>
		/// <returns>A pointer to the given guest address in host memory.</returns>
		public unsafe byte* TranslateMainMemory( int guestAddress )
		{
			Debug.Assert( ( guestAddress & MainMemoryBase ) != 0 );
			return ( MainMemory + ( ( guestAddress & 0x3FFFFFFF ) - MainMemoryBase ) );
		}

		/// <summary>
		/// Translate a guest address to a host address.
		/// </summary>
		/// <param name="guestAddress">The address, in guest space.</param>
		/// <returns>A pointer to the given guest address in host memory.</returns>
		public unsafe byte* Translate( uint guestAddress )
		{
			guestAddress &= 0x3FFFFFFF;
			if( ( guestAddress & MainMemoryBase ) != 0 )
			{
				Debug.Assert( ( guestAddress >= MainMemoryBase ) && ( guestAddress < MainMemoryBound ) );
				return ( MainMemory + ( guestAddress - MainMemoryBase ) );
			}
			else if( ( guestAddress & VideoMemoryBase ) != 0 )
			{
				Debug.Assert( ( guestAddress >= VideoMemoryBase ) && ( guestAddress < VideoMemoryBound ) );
				return ( VideoMemory + ( guestAddress - VideoMemoryBase ) );
			}
#if SUPPORTSCRATCHPAD
			else if( ( guestAddress & ScratchPadBase ) != 0 )
			{
				Debug.Assert( ( guestAddress >= ScratchPadBase ) && ( guestAddress < ScratchPadBound ) );
				return ( ScratchPad + ( guestAddress - ScratchPadBase ) );
			}
#endif
			else
			{
				Debug.Assert( false );
				return ( byte* )0;
			}
		}

		/// <summary>
		/// Align a value.
		/// </summary>
		/// <param name="n">Value to align.</param>
		/// <param name="align">Alignment.</param>
		/// <returns>The aligned value.</returns>
		public static uint Align( uint n, uint align )
		{
			return ( n + ( align - 1 ) ) & ~( align - 1 );
		}

		/// <summary>
		/// Copy a region of memory.
		/// </summary>
		/// <param name="src">The source buffer.</param>
		/// <param name="dest">The destination buffer.</param>
		/// <param name="length">The number of bytes to copy.</param>
		public static unsafe void CopyMemory( byte* src, byte* dest, uint length )
		{
			// Taken from System.Buffer.memcpyimpl
			// Looks just like the CRT memcpy!
			// Wouldn't have to do this if the bastards provided proper Marshal Copy or Buffer BlockCopy overloads
			if( length >= 0x10 )
			{
				do
				{
					*( ( int* )dest ) = *( ( int* )src );
					*( ( int* )( dest + 4 ) ) = *( ( int* )( src + 4 ) );
					*( ( int* )( dest + 8 ) ) = *( ( int* )( src + 8 ) );
					*( ( int* )( dest + 12 ) ) = *( ( int* )( src + 12 ) );
					dest += 0x10;
					src += 0x10;
				}
				while( ( length -= 0x10 ) >= 0x10 );
			}
			if( length > 0 )
			{
				if( ( length & 8 ) != 0 )
				{
					*( ( int* )dest ) = *( ( int* )src );
					*( ( int* )( dest + 4 ) ) = *( ( int* )( src + 4 ) );
					dest += 8;
					src += 8;
				}
				if( ( length & 4 ) != 0 )
				{
					*( ( int* )dest ) = *( ( int* )src );
					dest += 4;
					src += 4;
				}
				if( ( length & 2 ) != 0 )
				{
					*( ( short* )dest ) = *( ( short* )src );
					dest += 2;
					src += 2;
				}
				if( ( length & 1 ) != 0 )
				{
					*dest = *src;
					//dest++;
					//src++;
				}
			}
		}
		
		/// <summary>
		/// Zero a region of memory.
		/// </summary>
		/// <param name="dest">The destination buffer.</param>
		/// <param name="length">The number of bytes to zero.</param>
		public static unsafe void ZeroMemory( byte* dest, uint length )
		{
			// Modifed from CopyMemory above
			if( length >= 0x10 )
			{
				do
				{
					*( ( int* )dest ) = 0;
					*( ( int* )( dest + 4 ) ) = 0;
					*( ( int* )( dest + 8 ) ) = 0;
					*( ( int* )( dest + 12 ) ) = 0;
					dest += 0x10;
				}
				while( ( length -= 0x10 ) >= 0x10 );
			}
			if( length > 0 )
			{
				if( ( length & 8 ) != 0 )
				{
					*( ( int* )dest ) = 0;
					*( ( int* )( dest + 4 ) ) = 0;
					dest += 8;
				}
				if( ( length & 4 ) != 0 )
				{
					*( ( int* )dest ) = 0;
					dest += 4;
				}
				if( ( length & 2 ) != 0 )
				{
					*( ( short* )dest ) = 0;
					dest += 2;
				}
				if( ( length & 1 ) != 0 )
				{
					*dest = 0;
					//dest++;
				}
			}
		}

		/// <summary>
		/// Dump the contents of main memory to the given file.
		/// </summary>
		/// <param name="fileName">Target file for memory dump.</param>
		public unsafe void DumpMainMemory( string fileName )
		{
			using( FileStream stream = File.OpenWrite( fileName ) )
			using( BinaryWriter writer = new BinaryWriter( stream ) )
			{
				byte[] buffer = new byte[ 512 ];
				byte* p = this.MainMemory;
				for( int n = 0; n < MainMemorySize / 512; n++ )
				{
					Marshal.Copy( new IntPtr( ( void* )p ), buffer, 0, 512 );
					writer.Write( buffer );
					p += 512;
				}
			}
		}
	}
}
