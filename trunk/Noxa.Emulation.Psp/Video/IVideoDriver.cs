// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Threading;

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

	public interface IVideoStatistics
	{
		int FramesPerSecond
		{
			get;
		}

		// tris per second, fb writes per second
		// also per frame
	}

	public interface IVideoDriver : IComponentInstance
	{
		IVideoCapabilities Capabilities
		{
			get;
		}

		IVideoStatistics Statistics
		{
			get;
		}

		DisplayProperties Properties
		{
			get;
		}

		IntPtr ControlHandle
		{
			get;
			set;
		}

		AutoResetEvent Vblank
		{
			get;
		}

		uint Vcount
		{
			get;
		}

		DisplayList FindDisplayList( int displayListId );
		bool Enqueue( DisplayList displayList, bool immediate );
		void Abort( int displayListId );
		void Sync( DisplayList displayList );

		void Sync();

		void Suspend();
		bool Resume();
	}
}
