// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;

using Noxa.Emulation.Psp.Cpu;

namespace Noxa.Emulation.Psp.Video.Direct3DM
{
	class DummyFrameBuffer : IMemorySegment
	{
		private VideoDriver _driver;

		private Rectangle _rect;

		public DummyFrameBuffer( VideoDriver driver )
		{
			Debug.Assert( driver != null );

			_driver = driver;
			_rect = new Rectangle( 0, 0, 512, 272 );
		}

		public Rectangle Dimensions
		{
			get
			{
				return _rect;
			}
		}

		public void Reset()
		{
		}

		public void Flush()
		{
		}

		#region IMemorySegment Members

		public MemoryType MemoryType
		{
			get
			{
				return MemoryType.HardwareMapped;
			}
		}

		public string Name
		{
			get
			{
				return "Frame Buffer";
			}
		}

		public int BaseAddress
		{
			get
			{
				return 0x04000000;
			}
		}

		public int Length
		{
			get
			{
				return 0x001FFFFF;
			}
		}

		public event MemoryChangeDelegate MemoryChanged;

		protected void OnMemoryChanged( IMemorySegment segment, int address, int width, int value )
		{
			MemoryChangeDelegate handler = this.MemoryChanged;
			if( handler != null )
				handler( segment, address, width, value );
		}

		public int ReadWord( int address )
		{
			throw new NotImplementedException();
		}

		public byte[] ReadBytes( int address, int count )
		{
			throw new NotImplementedException();
		}

		public int ReadStream( int address, System.IO.Stream destination, int count )
		{
			throw new NotImplementedException();
		}

		public void WriteWord( int address, int width, int value )
		{
		}

		public void WriteBytes( int address, byte[] bytes )
		{
		}

		public void WriteBytes( int address, byte[] bytes, int index, int count )
		{
		}

		public void WriteStream( int address, System.IO.Stream source, int count )
		{
			throw new NotImplementedException();
		}

		public void Load( System.IO.Stream stream )
		{
			throw new NotImplementedException();
		}

		public void Load( string fileName )
		{
			throw new NotImplementedException();
		}

		public void Save( System.IO.Stream stream )
		{
			throw new NotImplementedException();
		}

		public void Save( string fileName )
		{
			throw new NotImplementedException();
		}

		public uint GetMemoryHash( int address, int count, uint prime )
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
