using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Noxa.Emulation.Psp.Cpu;
using System.IO;
using Microsoft.DirectX.Generic;
using Microsoft.DirectX.Direct3D.CustomVertex;
using System.Drawing;

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

	class VideoContext
	{
		public MemoryStream MemoryBuffer = new MemoryStream( 1024 * 100 );
		public BinaryReader MemoryReader;

		public uint FrameBufferPointer;
		public int FrameBufferWidth;

		public Matrix WorldMatrix;
		public Matrix ViewMatrix;
		public Matrix ProjectionMatrix;
		public Matrix TextureMatrix;

		public float[] MatrixTemp = new float[ 4 * 4 ];
		public int MatrixIndex;

		public int VertexBufferAddress;
		public int IndexBufferAddress;

		public VertexTypes VertexType;
		public bool VerticesTransformed;
		public int MorphingVertexCount;
		public int SkinningWeightCount;

		public uint Temp;

		public const uint MaximumCachedVertexBuffers = 4703;
		public Dictionary<uint, VertexBuffer> CachedVertexBuffers = new Dictionary<uint, VertexBuffer>( ( int )MaximumCachedVertexBuffers );
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

			//Debug.WriteLine( string.Format( "VideoDriver: got a complete list with {0} packets", list.Packets.Length ) );

			_device.RenderState.CullMode = Cull.None;
			_device.RenderState.Lighting = false;
			_device.RenderState.AlphaBlendEnable = false;

			//_device.Transform.Projection = Matrix.PerspectiveLeftHanded( 480, 272, 0.1f, 40.0f );
			//sceGumPerspective( 75.0f, 16.0f / 9.0f, 0.5f, 1000.0f );
			_device.Transform.Projection = Matrix.PerspectiveFieldOfViewLeftHanded( ( float )Math.PI / 3.0f, 480.0f / 272.0f, 0.1f, 1000.0f );
			// 75 deg -> rad = 1.30899694 (1 degrees = 0.0174532925 radians)
			//_device.Transform.Projection = Matrix.PerspectiveFieldOfViewLeftHanded( 1.3089969f, 16.0f / 9.0f, 0.5f, 1000.0f );
			//_device.Transform.Projection = _context.ProjectionMatrix;
			_device.Transform.View = Matrix.LookAtLeftHanded( new Vector3( 0, 0, 1 ), Vector3.Empty, new Vector3( 0, 1, 0 ) );
			//_device.Transform.View = Matrix.Identity;
			_device.Transform.World = Matrix.Identity;
			
			for( int n = 0; n < list.Packets.Length; n++ )
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
							int ambient = _device.RenderState.AmbientValue;
							ambient &= 0x00FFFFFF;
							ambient |= packet.Argument << 24;
							_device.RenderState.AmbientValue = ambient;
						}
						break;
					case VideoCommand.ALC:
						{
							// Ambient color in BGR format - need to flip
							int ambient = _device.RenderState.AmbientValue;
							ambient &= unchecked( ( int )0xFF000000 );
							ambient |= ( ( packet.Argument & 0x00FF0000 ) >> 16 );
							ambient |= ( packet.Argument & 0x0000FF00 );
							ambient |= ( ( packet.Argument & 0x000000FF ) << 16 );
							_device.RenderState.AmbientValue = ambient;
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
								// Read all vertices
								//int vertexSize = DetermineVertexSize( _context.VertexType );
								int vertexSize = 16;
								//Debug.WriteLine( string.Format( "PRIM: {0} vertices of type {1} ({2} prims) in format 0x{3:X8} ({4}B/vertex)", vertexCount, primitiveType, primitiveCount, ( uint )_context.VertexType, vertexSize ) );

								uint hash = memory.GetMemoryHash( _context.VertexBufferAddress, vertexCount * vertexSize, VideoContext.MaximumCachedVertexBuffers );

								VertexBuffer vb = null;
								if( _context.CachedVertexBuffers.ContainsKey( hash ) == true )
								{
									vb = _context.CachedVertexBuffers[ hash ];
								}
								else
								{
									// TODO: Optimize cpu->ge memory reads
									memory.ReadStream( _context.VertexBufferAddress, _context.MemoryBuffer, vertexCount * vertexSize );

									// TODO: Don't use a fucking BinaryReader, you noob ^_^
									// Need to convert bytes to some kind of D3D vertex listing
									PositionColored[] verts = new PositionColored[ vertexCount ];
									BinaryReader reader = _context.MemoryReader;
									{
										for( int m = 0; m < vertexCount; m++ )
										{
											// 4 bytes color - need to swizzle cause in AABBGGRR format
											int c = reader.ReadInt32();
											c = ( c & unchecked( ( int )0xFF00FF00 ) ) | ( ( c & 0x00FF0000 ) >> 16 ) | ( ( c & 0x000000FF ) << 16 );

											// 3 float xyz
											float x = reader.ReadSingle();
											float y = reader.ReadSingle();
											float z = reader.ReadSingle();

											verts[ m ] = new PositionColored(
												x, y, z, c );
										}
									}
									_context.MemoryBuffer.Position = 0;

									vb = this.BuildVertexBuffer( primitiveType, verts );
									_context.CachedVertexBuffers.Add( hash, vb );
								}

								_device.VertexFormat = PositionColored.Format;
								_device.SetStreamSource( 0, vb, 0, PositionColored.StrideSize );
								_device.DrawPrimitives( primitiveType, 0, primitiveCount );
							}
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
							//_device.Transform.Texture0 = _context.TextureMatrix;
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

		private PositionColored[] _tempVerts;

		protected VertexBuffer BuildVertexBuffer( PrimitiveType primitiveType, PositionColored[] vertices )
		{
			_tempVerts = vertices;
			int stride = PositionColored.StrideSize;
			VertexFormats format = PositionColored.Format;
			VertexBuffer buffer = new VertexBuffer( _device, stride * vertices.Length, Usage.None, format, Pool.Default, new EventHandler( VertexBufferCreated ) );
			return buffer;
		}

		protected void VertexBufferCreated( object sender, EventArgs e )
		{
			try
			{
				VertexBuffer buffer = sender as VertexBuffer;
				Debug.Assert( buffer != null );
				Debug.Assert( _tempVerts != null );
				Debug.Assert( _tempVerts.Length > 0 );

				using( GraphicsBuffer<PositionColored> gb = buffer.Lock<PositionColored>( 0, _tempVerts.Length, LockFlags.Discard ) )
				{
					gb.Write( _tempVerts );
					buffer.Unlock();
				}
			}
			catch
			{
				throw;
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
