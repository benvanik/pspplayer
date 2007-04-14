// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using Noxa.Emulation.Psp.Cpu;

namespace Noxa.Emulation.Psp.Bios.ManagedHLE
{
	unsafe class MemorySystem
	{
		public const uint MainMemoryBase = 0x08000000;
		public const uint MainMemorySize = 0x01FFFFFF;
		public const uint MainMemoryBound = ( MainMemoryBase + MainMemorySize );
		public const uint ScratchPadBase = 0x00010000;
		public const uint ScratchPadSize = 0x00003FFF;
		public const uint ScratchPadBound = ( ScratchPadBase + ScratchPadSize );
		public const uint VideoMemoryBase = 0x04000000;
		public const uint VideoMemorySize = 0x001FFFFF;
		public const uint VideoMemoryBound = ( VideoMemoryBase + VideoMemorySize );

		public byte* MainMemory;
		public byte* VideoMemory;
		public byte* ScratchMemory;

		public MemorySystem( IMemory memory )
		{
			Debug.Assert( memory != null );
			MainMemory = ( byte* )memory.MainMemoryPointer;
			VideoMemory = ( byte* )memory.VideoMemoryPointer;
			ScratchMemory = ( byte* )0;
		}

		public unsafe byte* TranslateMainMemory( int guestAddress )
		{
			Debug.Assert( ( guestAddress & MainMemoryBase ) != 0 );
			return ( MainMemory + ( ( guestAddress & 0x3FFFFFFF ) - MainMemoryBase ) );
		}

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
					dest++;
					*dest = 0;
					// Is this right???
				}
			}
		}

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
					dest++;
					src++;
					dest[ 0 ] = src[ 0 ];
				}
			}
		}
	}
}
