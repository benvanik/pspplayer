// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2008 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace Noxa.Emulation.Psp.Video.ManagedGL
{
	unsafe static class ColorOperations
	{
		public static uint DecodeBGR5650( byte* src, byte* dest, uint entryCount )
		{
			ushort* psrc = ( ushort* )src;
			uint* pdest = ( uint* )dest;
			uint checksum = 0;
			for( int n = 0; n < entryCount; n++ )
			{
				ushort source = *psrc;
				uint r = ( byte )( ( ( ( ( source & 0xF800 ) >> 11 ) + 1 ) * 255 ) / 32 );
				uint g = ( byte )( ( ( ( ( source & 0x07E0 ) >> 5 ) + 1 ) * 255 ) / 64 );
				uint b = ( byte )( ( ( ( ( source & 0x001F ) >> 4 ) + 1 ) * 255 ) / 32 );
				*pdest = ( uint )( ( ( uint )0xFF000000 ) | ( r << 16 ) | ( g << 8 ) | b );
				checksum += *pdest;
				psrc++;
				pdest++;
			}
			return checksum;
		}

		public static uint DecodeABGR5551( byte* src, byte* dest, uint entryCount )
		{
			ushort* psrc = ( ushort* )src;
			uint* pdest = ( uint* )dest;
			uint checksum = 0;
			for( int n = 0; n < entryCount; n++ )
			{
				ushort source = *psrc;
				uint r = ( byte )( ( ( ( ( source & 0x7C00 ) >> 10 ) + 1 ) * 255 ) / 32 );
				uint g = ( byte )( ( ( ( ( source & 0x03E0 ) >> 5 ) + 1 ) * 255 ) / 32 );
				uint b = ( byte )( ( ( ( ( source & 0x001F ) >> 0 ) + 1 ) * 255 ) / 32 );
				uint a = ( uint )( ( ( source & 0x8000 ) >> 15 ) > 0 ? 0xFF000000 : 0 );
				*pdest = ( uint )( a | ( r << 16 ) | ( g << 8 ) | b );
				checksum += *pdest;
				psrc++;
				pdest++;
			}
			return checksum;
		}

		public static uint DecodeABGR4444( byte* src, byte* dest, uint entryCount )
		{
			ushort* psrc = ( ushort* )src;
			uint* pdest = ( uint* )dest;
			uint checksum = 0;
			for( int n = 0; n < entryCount; n++ )
			{
				ushort source = *psrc;
				uint r = ( byte )( ( ( ( ( source & 0xF000 ) >> 12 ) + 1 ) * 255 ) / 16 );
				uint g = ( byte )( ( ( ( ( source & 0x0F00 ) >> 8 ) + 1 ) * 255 ) / 16 );
				uint b = ( byte )( ( ( ( ( source & 0x00F0 ) >> 4 ) + 1 ) * 255 ) / 16 );
				uint a = ( byte )( ( ( ( source & 0x000F ) + 1 ) * 255 ) / 16 );
				*pdest = ( uint )( ( a << 24 ) | ( r << 16 ) | ( g << 8 ) | b );
				checksum += *pdest;
				psrc++;
				pdest++;
			}
			return checksum;
		}

		public static uint DecodeABGR8888( byte* src, byte* dest, uint entryCount )
		{
			uint checksum = 0;
			for( int n = 0; n < entryCount; n += 4 )
			{
				*( ( uint* )dest ) = *( ( uint* )src );
				*( ( uint* )( dest + 4 ) ) = *( ( uint* )( src + 4 ) );
				*( ( uint* )( dest + 8 ) ) = *( ( uint* )( src + 8 ) );
				*( ( uint* )( dest + 12 ) ) = *( ( uint* )( src + 12 ) );
				checksum += *( ( uint* )dest ); // just the first word
				dest += 16;
				src += 16;
			}
			return checksum;
		}
	}
}
