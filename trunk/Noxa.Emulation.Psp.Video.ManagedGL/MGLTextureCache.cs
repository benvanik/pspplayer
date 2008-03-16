// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2008 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Tao.OpenGl;

namespace Noxa.Emulation.Psp.Video.ManagedGL
{
	unsafe class MGLTextureCache
	{
		private MGLDriver _driver;
		private MGLContext _ctx;

		private const int Maximum = 256;
		private FastLinkedList<MGLTexture> _values;

		public MGLTextureCache( MGLDriver driver, MGLContext ctx )
		{
			_driver = driver;
			_ctx = ctx;
			_values = new FastLinkedList<MGLTexture>();
		}

		public MGLTexture Find( MGLTextureInfo info, out uint checksum )
		{
			checksum = 0;
			LinkedListEntry<MGLTexture> e = _values.HeadEntry;
			while( e != null )
			{
				if( e.Value.Address == info.Address )
				{
					// Check to make sure it's right
					bool match =
						( e.Value.Width == info.Width ) &&
						( e.Value.Height == info.Height ) &&
						( e.Value.LineWidth == info.LineWidth ) &&
						( e.Value.PixelStorage == info.PixelStorage ) &&
						( ( ( ( int )e.Value.PixelStorage & 0x4 ) == 0x4 ) ? ( e.Value.ClutChecksum == _ctx.Clut.Checksum ) : true );
					if( match == true )
					{
						// Cookie check
						//uint cookie = *((uint*)

						if( match == true )
						{
							byte* textureAddress = _driver.MemorySystem.Translate( info.Address );
							checksum = MGLTexture.CalculateChecksum( textureAddress, info.Width, info.Height, info.PixelStorage );
							match = ( checksum == e.Value.Checksum );
						}
					}
					if( match == true )
					{
						// Match - move to head
						_values.MoveToHead( e );
						return e.Value;
					}
					else
					{
						// Mismatch - free
						Gl.glDeleteTextures( 1, ref e.Value.TextureID );
						_driver.InvalidateCurrentTexture();
						_values.Remove( e );
						return null;
					}
				}
				e = e.Next;
			}
			return null;
		}

		public void Add( MGLTexture texture )
		{
			if( _values.Count > Maximum )
			{
				// Evict last entry
				MGLTexture dead = _values.Tail;
				_values.Remove( _values.TailEntry );

				Gl.glDeleteTextures( 1, ref dead.TextureID );
			}

			_values.InsertAtHead( texture );
		}

		public void Clear()
		{
			List<int> textureIds = new List<int>();
			LinkedListEntry<MGLTexture> e = _values.HeadEntry;
			while( e != null )
			{
				textureIds.Add( e.Value.TextureID );
				e = e.Next;
			}
			_values.Clear();
			Gl.glDeleteTextures( textureIds.Count, textureIds.ToArray() );
		}
	}
}
