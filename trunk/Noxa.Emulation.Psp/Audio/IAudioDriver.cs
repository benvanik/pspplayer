// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace Noxa.Emulation.Psp.Audio
{
	public enum AudioFormat
	{
		Stereo	= 0x00,
		Mono	= 0x10,
	}

	public static class AudioConstants
	{
		public const int MaximumVolume = 0x8000;
		public const int MaximumChannel = 8;
		public const int NextChannel = -1;

		public const int MinimumSampleCount = 64;
		public const int MaximumSampleCount = 65472;
	}

	public interface IAudioDriver : IComponentInstance
	{
		int ReserveChannel( int channel, int sampleCount, AudioFormat format );
		void ReleaseChannel( int channel );
		void ReleaseAllChannels();

		void Output( IntPtr buffer, bool block, int volume );
		void Output( IntPtr buffer, bool block, int leftVolume, int rightVolume );
	}
}
