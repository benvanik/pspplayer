// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2008 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Noxa.Emulation.Psp.Debugging.Statistics;

namespace Noxa.Emulation.Psp.Video.ManagedGL
{
	class MGLStatisticsSource : CounterSource
	{
		public readonly MGLDriver Driver;

		public readonly Counter Frames;
		public readonly Counter SkippedFrames;
		public readonly Counter DisplayLists;
		public readonly Counter AbortedDisplayLists;

		public MGLStatisticsSource( MGLDriver driver )
			: base( "Video Driver" )
		{
			this.Driver = driver;

			this.Frames = new Counter( "Frames", "The number of frames rendered." );
			this.SkippedFrames = new Counter( "Skipped Frames", "The number of frames skipped due to frame skipping." );
			this.DisplayLists = new Counter( "Display Lists", "The number of display lists processed." );
			this.AbortedDisplayLists = new Counter( "Aborted Lists", "The number of display lists aborted by the game." );

			this.RegisterCounter( this.Frames );
			this.RegisterCounter( this.SkippedFrames );
			this.RegisterCounter( this.DisplayLists );
			this.RegisterCounter( this.AbortedDisplayLists );
		}

		public override void Sample()
		{
			this.Frames.Update( ( double )MGLStatistics.ProcessedFrames );
			this.SkippedFrames.Update( ( double )MGLStatistics.SkippedFrames );
			this.DisplayLists.Update( ( double )MGLStatistics.DisplayListsProcessed );
			this.AbortedDisplayLists.Update( ( double )MGLStatistics.AbortedLists );
		}
	}
}
