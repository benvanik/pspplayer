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
	public enum VideoStatisticsCapabilities
	{
		None = 0,
		FramesPerSecond = 0x001,
	}

	public interface IVideoCapabilities
	{
		VideoStatisticsCapabilities SupportedStatistics
		{
			get;
		}
	}
}
