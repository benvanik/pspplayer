// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX.Generic;

namespace Noxa.Emulation.Psp.Video.Direct3DM
{
	class CachedTexture
	{
		public int Address;
		public int LineWidth;
		public int Width;
		public int Height;
		public Texture Texture;
	}

	partial class VideoDriver
	{
		protected void SetTextures()
		{
			// TODO: the whole texture caching thing needs to be like the vertex buffer cache (based on memory contents)
			// as right now dynamic textures are not supported

			bool internalMemorySupported = _emulator.Cpu.Capabilities.InternalMemorySupported;
			byte[] internalMemory = _emulator.Cpu.InternalMemory;
			int internalMemoryBaseAddress = _emulator.Cpu.InternalMemoryBaseAddress;

			for( int n = 0; n < _context.Textures.Length; n++ )
			{
				TextureContext context = _context.Textures[ n ];
				if( ( context.Address == 0x0 ) ||
					( context.LineWidth == 0 ) ||
					( context.Width == 0 ) ||
					( context.Height == 0 ) )
				{
					_device.SetTexture( n, null );
					continue;
				}

				CachedTexture texture = null;
				if( _context.CachedTextures.ContainsKey( context.Address ) == true )
					texture = _context.CachedTextures[ context.Address ];
				else
				{
					// Need to generate
					texture = new CachedTexture();
					texture.Address = context.Address;
					texture.LineWidth = context.LineWidth;
					texture.Width = context.Width;
					texture.Height = context.Height;

					int pixelBytes = 2;

					// Read bytes in to temp array
					int length = texture.LineWidth * texture.Height * pixelBytes;
					if( internalMemorySupported == true )
					{
						// Use internal memory as a direct byte array (only works for main memory)
						int offset = context.Address - internalMemoryBaseAddress;

						// If we are trying to read from video memory, this could be negative - need to find a way to do this!
						Debug.Assert( offset > 0 );

						using( MemoryStream stream = new MemoryStream( internalMemory, offset, length, false, false ) )
						using( FileStream fs = File.OpenWrite( string.Format( "c:/test{0:X8}.raw", texture.Address ) ) )
						{
							stream.WriteTo( fs );
						}
						texture.Texture = new Texture( _device, texture.Width, texture.Height, 1, Usage.Dynamic, Format.A4R4G4B4, Pool.Default );
						using( GraphicsBuffer gb = texture.Texture.Lock( 0, null, LockFlags.None ) )
						{
							gb.Write( internalMemory, offset, length );
							texture.Texture.Unlock( 0 );
						}

						_context.CachedTextures.Add( texture.Address, texture );
					}
					else
					{
						// TODO: support texture reads from CPUs that don't support InternalMemory
						Debug.Assert( false );
						throw new NotImplementedException();
					}
				}

				if( texture != null )
					_device.SetTexture( n, texture.Texture );
				else
					_device.SetTexture( n, null );
			}
		}
	}
}
