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
		Rgba8888,
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
		NextFrame = 1,
	}

	/// <summary>
	/// <see cref="IVideoDriver"/> display properties.
	/// </summary>
	public class DisplayProperties
	{
		/// <summary>
		/// The display mode.
		/// </summary>
		public int Mode;

		/// <summary>
		/// The width of the display, in pixels.
		/// </summary>
		public int Width;
		
		/// <summary>
		/// The height of the display, in pixels.
		/// </summary>
		public int Height;
		
		/// <summary>
		/// The pixel format used by the display.
		/// </summary>
		public PixelFormat PixelFormat;
		
		/// <summary>
		/// The sync mode of the display.
		/// </summary>
		public BufferSyncMode SyncMode;
		
		/// <summary>
		/// The address of the frame buffer.
		/// </summary>
		public uint BufferAddress;
		
		/// <summary>
		/// The size of the frame buffer.
		/// </summary>
		public uint BufferSize;

		/// <summary>
		/// Return a copy of this property set.
		/// </summary>
		/// <returns>A copy of this property set.</returns>
		public DisplayProperties Clone()
		{
			DisplayProperties copy = new DisplayProperties();
			copy.BufferAddress = this.BufferAddress;
			copy.BufferSize = this.BufferSize;
			copy.Height = this.Height;
			copy.Mode = this.Mode;
			copy.PixelFormat = this.PixelFormat;
			copy.SyncMode = this.SyncMode;
			copy.Width = this.Width;
			return copy;
		}
	}
}
