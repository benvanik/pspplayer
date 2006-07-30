using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Noxa.Emulation.Psp.Cpu;

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
	}

	partial class VideoDriver
	{
		protected VideoContext _context;

		protected void InitializeContext()
		{
			_context = new VideoContext();
		}

		protected void CleanupContext()
		{
			_context = null;
		}

		protected void ParseList( DisplayList list )
		{
			Debug.WriteLine( string.Format( "VideoDriver: got a complete list with {0} packets", list.Packets.Length ) );

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
					//case VideoCommand.BCE:
					//case VideoCommand.TME:
					//case VideoCommand.FGE:
					//case VideoCommand.DTE:
					//case VideoCommand.ABE:
					//case VideoCommand.ATE:
					//case VideoCommand.ZTE:
					//case VideoCommand.STE:
					//case VideoCommand.AAE:
					//case VideoCommand.PCE:
					//case VideoCommand.CTE:
					//case VideoCommand.LOE:
					//    break;

					case VideoCommand.CLEAR:
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
						int vertexCount = packet.Argument & 0xFFFF;
						PrimitiveType primitiveType = PrimitiveType.LineList;
						bool areSprites = false;
						switch( ( packet.Argument >> 16 ) & 0x3 )
						{
							case 0x0: // Points
								primitiveType = PrimitiveType.PointList;
								break;
							case 0x1: // Lines
								primitiveType = PrimitiveType.LineList;
								break;
							case 0x2: // Line strips
								primitiveType = PrimitiveType.LineStrip;
								break;
							case 0x3: // Triangles
								primitiveType = PrimitiveType.TriangleList;
								break;
							case 0x4: // Triangle strips
								primitiveType = PrimitiveType.TriangleStrip;
								break;
							case 0x5: // Triangle fans
								primitiveType = PrimitiveType.TriangleFan;
								break;
							case 0x6: // Sprites (2D rectangles)
								areSprites = true;
								break;
						}
						if( areSprites == true )
						{
							// TODO: Sprite support
							Debugger.Break();
						}
						else
						{
							// Read all vertices
							int vertexSize = DetermineVertexSize( _context.VertexType );
							Debug.WriteLine( string.Format( "PRIM: {0} vertices of type {1} in format 0x{2:X8} ({3}B/vertex)", vertexCount, primitiveType, ( uint )_context.VertexType, vertexSize ) );

							// TODO: Optimize cpu->ge memory reads
							IMemory memory = _emulator.Cpu.Memory;
							byte[] bytes = memory.ReadBytes( _context.VertexBufferAddress, vertexCount * vertexSize );
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
						}
						break;
					case VideoCommand.VIEW: // 3x4
						_context.MatrixTemp[ _context.MatrixIndex++ ] = packet.ArgumentF;
						if( _context.MatrixIndex == 12 )
						{
							_context.MatrixIndex = 0;
							_context.ViewMatrix = BuildMatrix3x4( _context.MatrixTemp );
						}
						break;
					case VideoCommand.WORLD: // 3x4
						_context.MatrixTemp[ _context.MatrixIndex++ ] = packet.ArgumentF;
						if( _context.MatrixIndex == 12 )
						{
							_context.MatrixIndex = 0;
							_context.WorldMatrix = BuildMatrix3x4( _context.MatrixTemp );
						}
						break;
					case VideoCommand.TMATRIX: // 3x4
						_context.MatrixTemp[ _context.MatrixIndex++ ] = packet.ArgumentF;
						if( _context.MatrixIndex == 12 )
						{
							_context.MatrixIndex = 0;
							_context.TextureMatrix = BuildMatrix3x4( _context.MatrixTemp );
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

		protected int DetermineVertexSize( VertexTypes vertexType )
		{
			int size = 0;

			VertexTypes positionMask = vertexType & VertexTypes.PositionMask;
			if( positionMask == VertexTypes.PositionFixed8 )
				size += 1 + 1 + 1;
			else if( positionMask == VertexTypes.PositionFixed16 )
				size += 2 + 2 + 2;
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

		//protected VertexBuffer BuildVertexBuffer( VertexTypes vertexType, byte[] buffer )
		//{
			//VertexBuffer buffer = new VertexBuffer( _device, stride * vertexCount, Usage.WriteOnly, format, Pool.Managed, new EventHandler( VertexBufferCreated ) );
		//}

		//protected void VertexBufferCreated( object sender, EventArgs e )
		//{
		//}

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
	}
}
