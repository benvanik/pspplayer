// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2008 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace Noxa.Emulation.Psp.Video.ManagedGL
{
	static class MGLStatistics
	{
		public static uint ProcessedFrames;
		public static uint SkippedFrames;
		public static uint DisplayListsProcessed;
		public static uint AbortedLists;
		public static uint StallCount;
		public static uint SignalCount;
		public static uint CommandsProcessed;

		public static uint[] CommandCounts = new uint[ 256 ];

		public static void Print()
		{
			Log.WriteLine( Verbosity.Normal, Feature.Video, "Video Statistics: -----------------------------" );
			Log.WriteLine( Verbosity.Normal, Feature.Video, "{0,-30} = {1}", "ProcessedFrames", ProcessedFrames );
			Log.WriteLine( Verbosity.Normal, Feature.Video, "{0,-30} = {1}", "SkippedFrames", SkippedFrames );
			Log.WriteLine( Verbosity.Normal, Feature.Video, "{0,-30} = {1}", "DisplayListsProcessed", DisplayListsProcessed );
			Log.WriteLine( Verbosity.Normal, Feature.Video, "{0,-30} = {1}", "AbortedLists", AbortedLists );
			Log.WriteLine( Verbosity.Normal, Feature.Video, "{0,-30} = {1}", "StallCount", StallCount );
			Log.WriteLine( Verbosity.Normal, Feature.Video, "{0,-30} = {1}", "SignalCount", SignalCount );
			Log.WriteLine( Verbosity.Normal, Feature.Video, "{0,-30} = {1}", "CommandsProcessed", CommandsProcessed );
			for( int n = 0; n < CommandCounts.Length; n++ )
			{
				if( CommandCounts[ n ] <= 1 )
					continue;
				Log.WriteLine( Verbosity.Verbose, Feature.Video, "{0:X2}: {1}\t{2}", n, CommandCounts[ n ], ( VideoCommand )n );
			}
		}

		public static void Reset()
		{
			ProcessedFrames = 0;
			SkippedFrames = 0;
			DisplayListsProcessed = 0;
			AbortedLists = 0;
			StallCount = 0;
			SignalCount = 0;
			CommandsProcessed = 0;
		}
	}
}
