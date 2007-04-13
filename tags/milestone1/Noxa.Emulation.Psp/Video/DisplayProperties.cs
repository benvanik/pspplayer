// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace Noxa.Emulation.Psp.Video
{
	/// <summary>
	/// Describes the pixel format of a display.
	/// </summary>
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

	/// <summary>
	/// Describes the buffer sync mode of a display.
	/// </summary>
	public enum BufferSyncMode
	{
		/// <summary>
		/// Buffer change effective immediately.
		/// </summary>
		Immediate = 0,
		/// <summary>
		/// Buffer change effective next frame.
		/// </summary>
		NextFrame = 1
	}

	/// <summary>
	/// <see cref="IVideoDriver"/> display properties.
	/// </summary>
	public class DisplayProperties : ICloneable
	{
		private bool _hasChanged;
		private int _mode;
		private int _width;
		private int _height;
		private PixelFormat _pixelFormat;
		private BufferSyncMode _syncMode;
		private uint _bufferAddress;
		private uint _bufferSize;

		/// <summary>
		/// <c>true</c> if the display properties have been changed.
		/// </summary>
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

		/// <summary>
		/// The display mode.
		/// </summary>
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

		/// <summary>
		/// The width of the display, in pixels.
		/// </summary>
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

		/// <summary>
		/// The height of the display, in pixels.
		/// </summary>
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

		/// <summary>
		/// The pixel format used by the display.
		/// </summary>
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

		/// <summary>
		/// The sync mode of the display.
		/// </summary>
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

		/// <summary>
		/// The address of the frame buffer.
		/// </summary>
		public uint BufferAddress
		{
			get
			{
				return _bufferAddress;
			}
			set
			{
				_bufferAddress = value;
			}
		}

		/// <summary>
		/// The size of the frame buffer.
		/// </summary>
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

		/// <summary>
		/// Clone the current <see cref="DisplayProperties"/> instance.
		/// </summary>
		/// <returns>A clone of the current <see cref="DisplayProperties"/> instance.</returns>
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
