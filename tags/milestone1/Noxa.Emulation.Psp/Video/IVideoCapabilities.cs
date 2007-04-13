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
	/// Describes the statistics capabilities an <see cref="IVideoDriver"/> may have.
	/// </summary>
	public enum VideoStatisticsCapabilities
	{
		/// <summary>
		/// No statistics supported.
		/// </summary>
		None = 0,

		/// <summary>
		/// Frames per second supported.
		/// </summary>
		FramesPerSecond = 0x001,
	}

	/// <summary>
	/// <see cref="IVideoDriver"/> capabilities.
	/// </summary>
	public interface IVideoCapabilities
	{
		/// <summary>
		/// The statistics capabilities supported.
		/// </summary>
		VideoStatisticsCapabilities SupportedStatistics
		{
			get;
		}
	}
}
