// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Drawing;
using System.IO;

using Noxa.Emulation.Psp.Cpu;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Noxa.Emulation.Psp.Video.Direct3DM
{
	enum MatrixType
	{
		World,
		View,
		Projection,
		Texture
	}

	[Flags]
	enum VertexTypes
	{
		None = 0x0,

		TextureMask = 0x3,
		TextureFixed8 = 0x1,
		TextureFixed16 = 0x2,
		TextureFloat = 0x3,

		ColorMask = 0x7 << 2,
		ColorBGR5650 = 0x4 << 2,
		ColorABGR5551 = 0x5 << 2,
		ColorABGR4444 = 0x6 << 2,
		ColorABGR8888 = 0x7 << 2,

		NormalMask = 0x3 << 5,
		NormalFixed8 = 0x1 << 5,
		NormalFixed16 = 0x2 << 5,
		NormalFloat = 0x3 << 5,

		PositionMask = 0x3 << 7,
		PositionFixed8 = 0x1 << 7,
		PositionFixed16 = 0x2 << 7,
		PositionFloat = 0x3 << 7,

		WeightMask = 0x3 << 9,
		WeightFixed8 = 0x1 << 9,
		WeightFixed16 = 0x2 << 9,
		WeightFloat = 0x3 << 9,

		IndexMask = 0x2 << 11,
		Index8 = 0x1 << 11,
		Index16 = 0x2 << 11,
		//100011100
		//0-1: Texture Format (2 values ST/UV)
		//    00: Not present in vertex
		//    01: 8-bit fixed
		//    10: 16-bit fixed
		//    11: 32-bit floats
		//2-4: Color Format (1 value)
		//    000: Not present in vertex
		//    100: 16-bit BGR-5650
		//    101: 16-bit ABGR-5551
		//    110: 16-bit ABGR-4444
		//    111: 32-bit ABGR-8888
		//5-6: Normal Format (3 values XYZ)
		//    00: Not present in vertex
		//    01: 8-bit fixed
		//    10: 16-bit fixed
		//    11: 32-bit floats
		//7-8: Position Format (3 values XYZ)
		//    00: Not present in vertex
		//    01: 8-bit fixed
		//    10: 16-bit fixed
		//    11: 32-bit floats
		//9-10: Weight Format
		//    00: Not present in vertex
		//    01: 8-bit fixed
		//    10: 16-bit fixed
		//    11: 32-bit floats
		//11-12: Index Format
		//    00: Not using indices
		//    01: 8-bit
		//    10: 16-bit
		//14-16: Number of weights (Skinning)
		//    000-111: 1-8 weights
		//18-20: Number of vertices (Morphing)
		//    000-111: 1-8 vertices
		//23: Bypass Transform Pipeline
		//    0: Transformed Coordinates
		//    1: Raw Coordinates
	}

	enum TexturePixelStorage
	{
		BGR5650 = 0,
		ABGR5551 = 1,
		ABGR4444 = 2,
		ABGR8888 = 3,
		Indexed4 = 4,
		Indexed8 = 5,
		Indexed16 = 6,
		Indexed32 = 7,
		DXT1 = 8,
		DXT3 = 9,
		DXT5 = 10,
	}

	class TextureContext
	{
		public int Address;
		public int LineWidth;
		public int Width;
		public int Height;
	}

	class VideoContext
	{
		public MemoryStream MemoryBuffer = new MemoryStream( 1024 * 100 );
		public BinaryReader MemoryReader;

		public uint FrameBufferPointer;
		public int FrameBufferWidth;

		public ClearFlags ClearFlags;
		public int ClearColor;
		public int ClearZDepth;
		public int ClearStencil;

		public Matrix WorldMatrix;
		public Matrix ViewMatrix;
		public Matrix ProjectionMatrix;
		public Matrix TextureMatrix;

		public float[] MatrixTemp = new float[ 4 * 4 ];
		public int MatrixIndex;

		public int VertexBufferAddress;
		public int IndexBufferAddress;

		public VertexTypes VertexType;
		public int VertexTypeValue;
		public bool VerticesTransformed;
		public int MorphingVertexCount;
		public int SkinningWeightCount;

		public uint Temp;

		public bool TexturesValid;
		public int MipMapLevel;
		public bool SwizzleTextures;
		public TexturePixelStorage TextureStorageMode;
		public TextureContext[] Textures = new TextureContext[]{
			new TextureContext(), new TextureContext(), new TextureContext(), new TextureContext(),
			new TextureContext(), new TextureContext(), new TextureContext(), new TextureContext(),
		};
		public const uint MaximumCachedTextures = 4703;
		public Dictionary<int, CachedTexture> CachedTextures = new Dictionary<int, CachedTexture>( ( int )MaximumCachedTextures );

		public const uint MaximumCachedVertexBuffers = 4703;
		public Dictionary<uint, CachedVertexBuffer> CachedVertexBuffers = new Dictionary<uint, CachedVertexBuffer>( ( int )MaximumCachedVertexBuffers );
		public Dictionary<int, int> VertexTypeSizes = new Dictionary<int, int>( 128 );
	}

	partial class VideoDriver
	{
		protected VideoContext _context;

		protected void InitializeContext()
		{
			_context = new VideoContext();
			_context.MemoryReader = new BinaryReader( _context.MemoryBuffer );
		}

		protected void CleanupContext()
		{
			_context = null;
		}
		
		protected void ParseList( DisplayList list )
		{
			IMemory memory = _emulator.Cpu.Memory;
			bool supportInternalMemory = _emulator.Cpu.Capabilities.InternalMemorySupported;
			byte[] internalMemory = _emulator.Cpu.InternalMemory;
			int internalMemoryBaseAddress = _emulator.Cpu.InternalMemoryBaseAddress;

			//Debug.WriteLine( string.Format( "VideoDriver: got a complete list with {0} packets", list.Packets.Length ) );

			_device.RenderState.CullMode = Cull.None;
			_device.RenderState.Lighting = false;
			_device.RenderState.AlphaBlendEnable = false;

			//_device.Transform.Projection = Matrix.PerspectiveLeftHanded( 480, 272, 0.1f, 40.0f );
			//sceGumPerspective( 75.0f, 16.0f / 9.0f, 0.5f, 1000.0f );
			//_device.Transform.Projection = Matrix.PerspectiveFieldOfViewLeftHanded( ( float )Math.PI / 3.0f, 480.0f / 272.0f, 0.1f, 1000.0f );
			// 75 deg -> rad = 1.30899694 (1 degrees = 0.0174532925 radians)
			//_device.Transform.Projection = Matrix.PerspectiveFieldOfViewLeftHanded( 1.3089969f, 16.0f / 9.0f, 0.5f, 1000.0f );
			//_device.Transform.Projection = _context.ProjectionMatrix;
			//_device.Transform.View = Matrix.LookAtLeftHanded( new Vector3( 0, 0, 1 ), Vector3.Empty, new Vector3( 0, 1, 0 ) );
			//_device.Transform.View = Matrix.Identity;
			//_device.Transform.World = Matrix.Identity;
			
			for( int n = 0; n < list.Packets.Count; n++ )
			{
				VideoPacket packet = list.Packets[ n ];

				switch( packet.Command )
				{
					//case VideoCommand.LTE:
					//case VideoCommand.LTE0:
					//case VideoCommand.LTE1:
					//case VideoCommand.LTE2:
					//case VideoCommand.LTE3:
					//case VideoCommand.CPE:
					//case VideoCommand.TME:
					//case VideoCommand.FGE:
					//case VideoCommand.DTE:
					//case VideoCommand.ABE:
					//case VideoCommand.ATE:
					//case VideoCommand.ZTE:
					//case VideoCommand.STE:
					//case VideoCommand.PCE:
					//case VideoCommand.CTE:
					//case VideoCommand.LOE:
					//    break;

					case VideoCommand.CLEAR:
						// This is a bad command as all it does is tell us what the psp cleared, not what actually needs
						// to be cleared - pspsdk will actually create a billboard (it's the SPRITE that comes through)
						// to do the target clear!
						if( ( packet.Argument & 0x1 ) == 0x1 )
						{
							bool clearAny = false;
							ClearFlags clearFlags = ClearFlags.Target;
							if( ( packet.Argument & 0x100 ) != 0 )
							{
								// Clear color buffer
								clearFlags = ClearFlags.Target;
								clearAny = true;
							}
							if( ( packet.Argument & 0x200 ) != 0 )
							{
								// Clear stencil/alpha buffer
								//if( clearAny == true )
								//    clearFlags |= ClearFlags.Stencil;
								//else
								//    clearFlags = ClearFlags.Stencil;
								//clearAny = true;
							}
							if( ( packet.Argument & 0x400 ) != 0 )
							{
								// Clear depth buffer
								//if( clearAny == true )
								//    clearFlags |= ClearFlags.ZBuffer;
								//else
								//    clearFlags = ClearFlags.ZBuffer;
								//clearAny = true;
							}
							// Done at start of the next frame
							_context.ClearFlags = clearFlags;
						}
						break;

					case VideoCommand.SHADE:
						// 0 = flat, 1 = shaded
						if( packet.Argument == 0 )
							_device.RenderState.ShadeMode = ShadeMode.Flat;
						else
							_device.RenderState.ShadeMode = ShadeMode.Gouraud;
						break;
					case VideoCommand.BCE:
						_device.RenderState.CullMode = ( packet.Argument == 0 ? Cull.None : _device.RenderState.CullMode );
						break;
					case VideoCommand.FFACE:
						// 0 = clockwise visible, 1 = counter-clockwise visible
						if( packet.Argument == 0 )
							_device.RenderState.CullMode = Cull.CounterClockwise;
						else
							_device.RenderState.CullMode = Cull.Clockwise;
						break;
					case VideoCommand.AAE:
						// Anti-aliasing is ignored - controlled by the video driver properties
						break;

					case VideoCommand.FGE:
						_device.RenderState.FogEnable = ( packet.Argument == 1 );
						break;
					case VideoCommand.FCOL:
						{
							int color = ( packet.Argument & 0x0000FF00 ) | unchecked( ( int )0xFF000000 );
							color |= ( ( packet.Argument & 0x00FF0000 ) >> 16 );
							color |= ( ( packet.Argument & 0x000000FF ) << 16 );
							_device.RenderState.FogColorValue = color;
						}
						break;
					case VideoCommand.FDIST:
						_device.RenderState.FogStart = packet.ArgumentF;
						break;
					case VideoCommand.FFAR:
						_device.RenderState.FogEnd = packet.ArgumentF;
						break;

					case VideoCommand.ALA:
						{
							int ambient = _device.RenderState.AmbientColor;
							ambient &= 0x00FFFFFF;
							ambient |= packet.Argument << 24;
							_device.RenderState.AmbientColor = ambient;
						}
						break;
					case VideoCommand.ALC:
						{
							// Ambient color in BGR format - need to flip
							int ambient = _device.RenderState.AmbientColor;
							ambient &= unchecked( ( int )0xFF000000 ) | ( ( packet.Argument & 0x00FF0000 ) >> 16 ) | ( packet.Argument & 0x0000FF00 ) | ( ( packet.Argument & 0x000000FF ) << 16 );
							_device.RenderState.AmbientColor = ambient;
						}
						break;
					case VideoCommand.AMA:
						{
							// TODO: ambient color
							int ambient = _device.Material.Ambient.ToArgb();
							ambient &= 0x00FFFFFF;
							ambient |= packet.Argument << 24;
							//_device.Material.Ambient = Color.FromArgb( ambient );
						}
						break;
					case VideoCommand.AMC:
						{
							// TODO: ambient color
							// Ambient color in BGR format - need to flip
							int ambient = _device.Material.Ambient.ToArgb();
							ambient &= unchecked( ( int )0xFF000000 ) | ( ( packet.Argument & 0x00FF0000 ) >> 16 ) | ( packet.Argument & 0x0000FF00 ) | ( ( packet.Argument & 0x000000FF ) << 16 );
							//_device.Material.Ambient = Color.FromArgb( ambient );
						}
						break;

					case VideoCommand.FBP:
						_context.Temp = ( uint )packet.Argument;
						break;
					case VideoCommand.FBW:
						// BUG: Not sure this is correct - pointer seems to go nowhere
						_context.FrameBufferPointer = _context.Temp | ( ( ( uint )packet.Argument & 0x00FF0000 ) << 8 );
						_context.FrameBufferWidth = ( packet.Argument & 0x0000FFFF );
						break;

					case VideoCommand.VTYPE:
						int type = packet.Argument;
						_context.VerticesTransformed = ( type >> 23 ) == 0;
						_context.SkinningWeightCount = ( type >> 14 ) & 0x3;
						_context.MorphingVertexCount = ( type >> 18 ) & 0x3;
						_context.VertexType = ( VertexTypes )( type & 0x1FFF );
						_context.VertexTypeValue = type & 0x1FFF;
						break;
					case VideoCommand.VADDR:
						_context.VertexBufferAddress = packet.Argument;
						break;
					case VideoCommand.IADDR:
						_context.IndexBufferAddress = packet.Argument;
						break;
					case VideoCommand.PRIM:
						{
							int vertexCount = packet.Argument & 0xFFFF;
							PrimitiveType primitiveType = PrimitiveType.LineList;
							bool areSprites = false;
							int primitiveCount = 0;
							switch( ( packet.Argument >> 16 ) & 0x7 )
							{
								case 0x0: // Points
									primitiveType = PrimitiveType.PointList;
									primitiveCount = vertexCount;
									break;
								case 0x1: // Lines
									primitiveType = PrimitiveType.LineList;
									primitiveCount = vertexCount / 2;
									break;
								case 0x2: // Line strips
									primitiveType = PrimitiveType.LineStrip;
									primitiveCount = vertexCount - 1;
									break;
								case 0x3: // Triangles
									primitiveType = PrimitiveType.TriangleList;
									primitiveCount = vertexCount / 3;
									break;
								case 0x4: // Triangle strips
									primitiveType = PrimitiveType.TriangleStrip;
									primitiveCount = vertexCount - 2;
									break;
								case 0x5: // Triangle fans
									primitiveType = PrimitiveType.TriangleFan;
									primitiveCount = vertexCount - 2;
									break;
								case 0x6: // Sprites (2D rectangles)
									areSprites = true;
									break;
							}
							if( areSprites == true )
							{
								// TODO: Sprite support
								//Debugger.Break();
								//Debug.WriteLine( string.Format( "PRIM: {0} vertices for sprites - not supported", vertexCount ) );
							}
							else
							{
								// This could be optimized somehow
								int vertexSize;
								if( _context.VertexTypeSizes.TryGetValue( _context.VertexTypeValue, out vertexSize ) == false )
								{
									vertexSize = DetermineVertexSize( _context.VertexType );
									_context.VertexTypeSizes.Add( _context.VertexTypeValue, vertexSize );
								}

								//Debug.WriteLine( string.Format( "PRIM: {0} vertices of type {1} ({2} prims) in format 0x{3:X8} ({4}B/vertex)", vertexCount, primitiveType, primitiveCount, ( uint )_context.VertexType, vertexSize ) );

								int bufferLength = vertexCount * vertexSize;

								// Get
								uint hash = memory.GetMemoryHash( _context.VertexBufferAddress, bufferLength, VideoContext.MaximumCachedVertexBuffers );

								CachedVertexBuffer vb;
								if( _context.CachedVertexBuffers.TryGetValue( hash, out vb ) == false )
								{
									if( supportInternalMemory == true )
									{
										// Use internal memory as a direct byte array (only works for main memory)
										int offset = _context.VertexBufferAddress - internalMemoryBaseAddress;

										// If we are trying to read from video memory, this could be negative - need to find a way to do this!
										Debug.Assert( offset > 0 );

										vb = this.BuildVertexBuffer( vertexCount, vertexSize, internalMemory, offset, bufferLength );
									}
									else
									{
										// TODO: vertex buffer memory reads when the CPU doesn't support InternalMemory
										//memory.ReadStream( _context.VertexBufferAddress, _context.MemoryBuffer, vertexCount * vertexSize );
										Debug.Assert( false );
										throw new NotImplementedException( "Need InternalMemory support in the CPU" );
									}

									Debug.Assert( vb != null );
									if( vb != null )
										_context.CachedVertexBuffers.Add( hash, vb );
								}

								Debug.Assert( vb != null );
								if( vb != null )
								{
									//this.SetTextures();

									_device.VertexFormat = vb.Format;
									_device.SetStreamSource( 0, vb.Buffer, 0, vb.StrideSize );
									_device.DrawPrimitives( primitiveType, 0, primitiveCount );
								}
							}
						}
						break;

					case VideoCommand.TMODE:
						{
							_context.SwizzleTextures = ( packet.Argument & 0x1 ) == 1 ? true : false;
							_context.MipMapLevel = ( packet.Argument >> 16 ) & 0x4;
						}
						break;
					case VideoCommand.TPSM:
						{
							_context.TextureStorageMode = ( TexturePixelStorage )packet.Argument;
						}
						break;
					case VideoCommand.TEC:
						{
							// TODO: texture environment color
							int color = ( packet.Argument & 0x0000FF00 ) | unchecked( ( int )0xFF000000 );
							color |= ( ( packet.Argument & 0x00FF0000 ) >> 16 );
							color |= ( ( packet.Argument & 0x000000FF ) << 16 );
							//_device.Material.Diffuse = Color.FromArgb( color );
						}
						break;
					case VideoCommand.TFLT:
						{
							// bits 0-2 have minifying filter
							// bits 8-10 have magnifying filter
							switch( packet.Argument & 0x7 )
							{
								case 0x0: // Nearest
									break;
								case 0x1: // Linear
									break;
								case 0x4: // Nearest; mipmap nearest
									break;
								case 0x5: // Linear; mipmap nearest
									break;
								case 0x6: // Nearest; mipmap linear
									break;
								case 0x7: // Linear; mipmap linear
									break;
							}
							switch( ( packet.Argument >> 8 ) & 0x7 )
							{
								case 0x0: // Nearest
									break;
								case 0x1: // Linear
									break;
								case 0x4: // Nearest; mipmap nearest
									break;
								case 0x5: // Linear; mipmap nearest
									break;
								case 0x6: // Nearest; mipmap linear
									break;
								case 0x7: // Linear; mipmap linear
									break;
							}
						}
						break;
					case VideoCommand.TFUNC:
						// TODO: texture function
						break;
					case VideoCommand.TFLUSH:
						_context.TexturesValid = false;
						break;
					case VideoCommand.USCALE:
						// (float) should be 1
						break;
					case VideoCommand.VSCALE:
						// (float) should be 1
						break;
					case VideoCommand.UOFFSET:
						// (float) should be 0
						break;
					case VideoCommand.VOFFSET:
						// (float) should be 0
						break;
					case VideoCommand.TBP0:
						{
							_context.Textures[ 0 ].Address = packet.Argument;
						}
						break;
					case VideoCommand.TBW0:
						{
							_context.Textures[ 0 ].Address |= ( packet.Argument << 8 ) & unchecked( ( int )0xFF000000 ); // | with bufferPointer from TBP
							_context.Textures[ 0 ].LineWidth = packet.Argument & 0xFFFF;
						}
						break;
					case VideoCommand.TSIZE0:
						{
							_context.Textures[ 0 ].Width = ( int )Math.Pow( 2, ( packet.Argument & 0xFF ) );
							_context.Textures[ 0 ].Height = ( int )Math.Pow( 2, ( ( packet.Argument >> 8 ) & 0xFF ) );
						}
						break;

					case VideoCommand.PMS:
					case VideoCommand.VMS:
					case VideoCommand.WMS:
					case VideoCommand.TMS:
						// Are these needed?
						_context.MatrixIndex = 0;
						break;
					case VideoCommand.PROJ: // 4x4
						_context.MatrixTemp[ _context.MatrixIndex++ ] = packet.ArgumentF;
						if( _context.MatrixIndex == 16 )
						{
							_context.MatrixIndex = 0;
							_context.ProjectionMatrix = BuildMatrix4x4( _context.MatrixTemp );
							_device.Transform.Projection = _context.ProjectionMatrix;
						}
						break;
					case VideoCommand.VIEW: // 3x4
						_context.MatrixTemp[ _context.MatrixIndex++ ] = packet.ArgumentF;
						if( _context.MatrixIndex == 12 )
						{
							_context.MatrixIndex = 0;
							_context.ViewMatrix = BuildMatrix3x4( _context.MatrixTemp );
							_device.Transform.View = _context.ViewMatrix;
						}
						break;
					case VideoCommand.WORLD: // 3x4
						_context.MatrixTemp[ _context.MatrixIndex++ ] = packet.ArgumentF;
						if( _context.MatrixIndex == 12 )
						{
							_context.MatrixIndex = 0;
							_context.WorldMatrix = BuildMatrix3x4( _context.MatrixTemp );
							_device.Transform.World = _context.WorldMatrix;
						}
						break;
					case VideoCommand.TMATRIX: // 3x4
						_context.MatrixTemp[ _context.MatrixIndex++ ] = packet.ArgumentF;
						if( _context.MatrixIndex == 12 )
						{
							_context.MatrixIndex = 0;
							_context.TextureMatrix = BuildMatrix3x4( _context.MatrixTemp );
							_device.Transform.Texture0 = _context.TextureMatrix;
						}
						break;

						// Ignored because the reader in the kernel handles these
					case VideoCommand.FINISH:
					case VideoCommand.END:
					case VideoCommand.Unknown0x11:
					case VideoCommand.BASE:
					case VideoCommand.JUMP:
						break;

					default:
						Debug.WriteLine( "VideoDriver::ParseList: unknown video packet: " + packet.ToString() );
						break;
				}
			}
		}

		#region Helpers

		protected int DetermineVertexSize( VertexTypes vertexType )
		{
			int size = 0;

			VertexTypes positionMask = vertexType & VertexTypes.PositionMask;
			if( positionMask == VertexTypes.PositionFixed8 )
				size += 1 + 1 + 1;
			else if( positionMask == VertexTypes.PositionFixed16 )
				//size += 2 + 2 + 2;
				size += 4 + 4 + 4;
			else if( positionMask == VertexTypes.PositionFloat )
				size += 4 + 4 + 4;

			VertexTypes normalMask = vertexType & VertexTypes.NormalMask;
			if( normalMask == VertexTypes.NormalFixed8 )
				size += 1 + 1 + 1;
			else if( normalMask == VertexTypes.NormalFixed16 )
				size += 2 + 4 + 4;
			else if( normalMask == VertexTypes.NormalFloat )
				size += 4 + 4 + 4;

			VertexTypes textureType = vertexType & VertexTypes.TextureMask;
			if( textureType == VertexTypes.TextureFixed8 )
				size += 1 + 1;
			else if( textureType == VertexTypes.TextureFixed16 )
				size += 2 + 2;
			else if( textureType == VertexTypes.TextureFloat )
				size += 4 + 4;

			// TODO: figure out weight format
			VertexTypes weightType = vertexType & VertexTypes.WeightMask;
			if( weightType == VertexTypes.WeightFixed8 )
				size += 1;
			else if( weightType == VertexTypes.WeightFixed16 )
				size += 2;
			else if( weightType == VertexTypes.WeightFloat )
				size += 4;

			VertexTypes colorType = vertexType & VertexTypes.ColorMask;
			if( colorType == VertexTypes.ColorBGR5650 )
				size += 2;
			else if( colorType == VertexTypes.ColorABGR4444 )
				size += 2;
			else if( colorType == VertexTypes.ColorABGR5551 )
				size += 2;
			else if( colorType == VertexTypes.ColorABGR8888 )
				size += 4;

			VertexTypes indexType = vertexType & VertexTypes.IndexMask;
			if( indexType == VertexTypes.Index8 )
				size += 1;
			else if( indexType == VertexTypes.Index16 )
				size += 2;

			return size;
		}

		protected Matrix BuildMatrix3x4( float[] values )
		{
			Matrix m = new Matrix();
			m.M11 = values[ 0 ];
			m.M12 = values[ 1 ];
			m.M13 = values[ 2 ];
			m.M21 = values[ 3 ];
			m.M22 = values[ 4 ];
			m.M23 = values[ 5 ];
			m.M31 = values[ 6 ];
			m.M32 = values[ 7 ];
			m.M33 = values[ 8 ];
			m.M41 = values[ 9 ];
			m.M42 = values[ 10 ];
			m.M43 = values[ 11 ];
			m.M44 = 1.0f;
			return m;
		}

		protected Matrix BuildMatrix4x4( float[] values )
		{
			Matrix m = new Matrix();
			m.M11 = values[ 0 ];
			m.M12 = values[ 1 ];
			m.M13 = values[ 2 ];
			m.M14 = values[ 3 ];
			m.M21 = values[ 4 ];
			m.M22 = values[ 5 ];
			m.M23 = values[ 6 ];
			m.M24 = values[ 7 ];
			m.M31 = values[ 8 ];
			m.M32 = values[ 9 ];
			m.M33 = values[ 10 ];
			m.M34 = values[ 11 ];
			m.M41 = values[ 12 ];
			m.M42 = values[ 13 ];
			m.M43 = values[ 14 ];
			m.M44 = values[ 15 ];
			return m;
		}

		#endregion
	}
}
