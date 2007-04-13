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
	/// <see cref="IVideoDriver"/> statistics.
	/// </summary>
	public interface IVideoStatistics
	{
		/// <summary>
		/// The number of frames per second, on average.
		/// </summary>
		int FramesPerSecond
		{
			get;
		}

		// tris per second, fb writes per second
		// also per frame
	}
}
