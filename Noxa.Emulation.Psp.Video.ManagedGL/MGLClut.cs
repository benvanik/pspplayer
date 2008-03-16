// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2008 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Tao.OpenGl;
using Noxa.Emulation.Psp.Cpu;

namespace Noxa.Emulation.Psp.Video.ManagedGL
{
	unsafe class MGLClut
	{
		private MGLDriver _driver;
		private MGLContext _ctx;
		public MGLClut( MGLDriver driver, MGLContext ctx )
		{
			_driver = driver;
			_ctx = ctx;
		}

		private const int ClutSize = 65536;
		private readonly byte[] _table = new byte[ ClutSize ];

		public uint Pointer;
		public uint Checksum;
		private uint _format;
		private int _shift;
		private uint _mask;
		private uint _start;

		public void Load( uint address, uint mode, uint paletteCount )
		{
			// Format:
			// 00 16-bit BGR 5650
			// 01 16-bit ABGR 5551
			// 10 16-bit ABGR 4444
			// 11 32-bit ABGR 8888
			_format = mode & 0x3;
			_shift = ( int )( ( mode >> 2 ) & 0x1F );
			_mask = ( mode >> 8 ) & 0xFF;
			_start = ( ( mode >> 16 ) & 0x1F ) * 16; // ??
			this.Pointer = address;

			if( address == 0x0 )
				return;

			byte* tablePointer = _driver.MemorySystem.Translate( address );
			fixed( byte* p = &_table[ 0 ] )
			{
				switch( _format )
				{
					case 0:
						this.Checksum = ColorOperations.DecodeBGR5650( tablePointer, p, paletteCount * 16 );
						break;
					case 1:
						this.Checksum = ColorOperations.DecodeABGR5551( tablePointer, p, paletteCount * 16 );
						break;
					case 2:
						this.Checksum = ColorOperations.DecodeABGR4444( tablePointer, p, paletteCount * 16 );
						break;
					case 3:
						this.Checksum = ColorOperations.DecodeABGR8888( tablePointer, p, paletteCount * 8 );
						break;
				}

				// Checksum so that we can tell if it changed
				//uint* cp = ( uint* )tablePointer;
				//uint checksum = 0;
				//for( int n = 0; n < ( entryCount * entryWidth ); n++ )
				//	checksum += *( cp++ );
				//if( this.Checksum != checksum )
				//{
					// Checksums don't match! Invalidate all CLUT textures!
					/*LLEntry<TextureEntry*>* e = context->TextureCache->GetEnumerator();
					while( e != NULL )
					{
						LLEntry<TextureEntry*>* next = e->Next;
						if( ( e->Value->PixelStorage & 0x4 ) == 0x4 )
						{
							// Check to see if it was from this clut
							if( e->Value->ClutPointer == context->ClutPointer )
							{
								// Kill it!
								GLuint freeIds[] = { e->Value->TextureID };
								glDeleteTextures( 1, freeIds );
								context->TextureCache->Remove( ( uint )e->Value->Address );
							}
						}
						e = next;
					}*/
					//context->TextureCache->Clear();
				//}
				//this.Checksum = checksum;
			}
		}

		public void Decode4( byte* pin, byte* pout, uint width, uint height, uint lineWidth )
		{
			// Tricky, as each byte contains 2 indices (4 bits each)
			byte* input = ( byte* )pin;
			uint* output = ( uint* )pout;
			uint diff = ( width - lineWidth ) / 2;
			Debug.Assert( diff >= 0 );
			fixed( byte* p = &_table[ 0 ] )
			{
				uint* table = ( uint* )p;
				for( uint y = 0; y < height; y++ )
				{
					for( uint x = 0; x < width; x += 2 )
					{
						output[ x ] = table[ ( ( _start + ( *input & 0x0F ) ) >> _shift ) & _mask ];
						output[ x + 1 ] = table[ ( ( _start + ( *input >> 4 ) ) >> _shift ) & _mask ];
						input++;
					}
					input -= width / 2;
					input += lineWidth / 2;
					output += width;
				}
			}
		}

		public void Decode8( byte* pin, byte* pout, uint width, uint height, uint lineWidth )
		{
			byte* input = ( byte* )pin;
			uint* output = ( uint* )pout;
			fixed( byte* p = &_table[ 0 ] )
			{
				uint* table = ( uint* )p;
				for( uint y = 0; y < height; y++ )
				{
					for( uint x = 0; x < width; x++ )
						output[ x ] = table[ ( ( _start + input[ x ] ) >> _shift ) & _mask ];
					input += lineWidth;
					output += width;
				}
			}
		}

		public void Decode16( byte* pin, byte* pout, uint width, uint height, uint lineWidth )
		{
			ushort* input = ( ushort* )pin;
			uint* output = ( uint* )pout;
			fixed( byte* p = &_table[ 0 ] )
			{
				uint* table = ( uint* )p;
				for( uint y = 0; y < height; y++ )
				{
					for( uint x = 0; x < width; x++ )
						output[ x ] = table[ ( ( _start + input[ x ] ) >> _shift ) & _mask ];
					input += lineWidth;
					output += width;
				}
			}
		}

		public void Decode32( byte* pin, byte* pout, uint width, uint height, uint lineWidth )
		{
			uint* input = ( uint* )pin;
			uint* output = ( uint* )pout;
			fixed( byte* p = &_table[ 0 ] )
			{
				uint* table = ( uint* )p;
				for( uint y = 0; y < height; y++ )
				{
					for( uint x = 0; x < width; x++ )
						output[ x ] = table[ ( ( _start + input[ x ] ) >> _shift ) & _mask ];
					input += lineWidth;
					output += width;
				}
			}
		}
	}
}
