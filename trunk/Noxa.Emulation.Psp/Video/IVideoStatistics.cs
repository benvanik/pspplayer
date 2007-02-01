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
	public interface IVideoStatistics
	{
		int FramesPerSecond
		{
			get;
		}

		// tris per second, fb writes per second
		// also per frame
	}
}
