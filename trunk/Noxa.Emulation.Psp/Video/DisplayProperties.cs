using System;
using System.Collections.Generic;
using System.Text;

namespace Noxa.Emulation.Psp.Video
{
	public enum PixelFormat
	{
		/// <summary>
		/// 16-bit RGB 5:6:5
		/// </summary>
		Rgb565 = 0,
		/// <summary>
		/// 16-bit RGBA 5:5:5:1
		/// </summary>
		Rgba5551,
		/// <summary>
		/// 16-bit RGBA 4:4:4:4
		/// </summary>
		Rgba4444,
		/// <summary>
		/// 32-bit RGBA 8:8:8:8
		/// </summary>
		Rgba8888
	}

	public enum BufferSyncMode
	{
		/// <summary>
		/// Buffer change effective immediately
		/// </summary>
		Immediate = 0,
		/// <summary>
		/// Buffer change effective next frame
		/// </summary>
		NextFrame = 1
	}

	public class DisplayProperties : ICloneable
	{
		protected bool _hasChanged;
		protected int _mode;
		protected int _width;
		protected int _height;
		protected PixelFormat _pixelFormat;
		protected BufferSyncMode _syncMode;
		protected uint _bufferAddress;
		protected uint _bufferSize;

		public bool HasChanged
		{
			get
			{
				return _hasChanged;
			}
			set
			{
				_hasChanged = value;
			}
		}

		public int Mode
		{
			get
			{
				return _mode;
			}
			set
			{
				if( _mode != value )
					_hasChanged = true;
				_mode = value;
			}
		}

		public int Width
		{
			get
			{
				return _width;
			}
			set
			{
				if( _width != value )
					_hasChanged = true;
				_width = value;
			}
		}

		public int Height
		{
			get
			{
				return _height;
			}
			set
			{
				if( _height != value )
					_hasChanged = true;
				_height = value;
			}
		}

		public PixelFormat PixelFormat
		{
			get
			{
				return _pixelFormat;
			}
			set
			{
				if( _pixelFormat != value )
					_hasChanged = true;
				_pixelFormat = value;
			}
		}

		public BufferSyncMode SyncMode
		{
			get
			{
				return _syncMode;
			}
			set
			{
				if( _syncMode != value )
					_hasChanged = true;
				_syncMode = value;
			}
		}

		public uint BufferAddress
		{
			get
			{
				return _bufferAddress;
			}
			set
			{
				if( _bufferAddress != value )
					_hasChanged = true;
				_bufferAddress = value;
			}
		}

		public uint BufferSize
		{
			get
			{
				return _bufferSize;
			}
			set
			{
				if( _bufferSize != value )
					_hasChanged = true;
				_bufferSize = value;
			}
		}

		public object Clone()
		{
			DisplayProperties clone = new DisplayProperties();
			clone.Mode = _mode;
			clone.Width = _width;
			clone.Height = _height;
			clone.PixelFormat = _pixelFormat;
			clone.SyncMode = _syncMode;
			clone.BufferAddress = _bufferAddress;
			clone.BufferSize = _bufferSize;
			clone.HasChanged = false;
			return clone;
		}
	}
}
