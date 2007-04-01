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
	/// <summary>
	/// Describes the audio format of a channel.
	/// </summary>
	public enum AudioFormat
	{
		/// <summary>
		/// Stereo.
		/// </summary>
		Stereo	= 0x00,

		/// <summary>
		/// Mono.
		/// </summary>
		Mono	= 0x10,
	}

	/// <summary>
	/// Constants used by the <see cref="IAudioDriver"/>.
	/// </summary>
	public static class AudioConstants
	{
		/// <summary>
		/// The maximum volume of a sound sample.
		/// </summary>
		public const int MaximumVolume = 0x8000;

		/// <summary>
		/// The maximum channel number.
		/// </summary>
		public const int MaximumChannel = 8;

		/// <summary>
		/// Allocate the next available channel.
		/// </summary>
		public const int NextChannel = -1;

		/// <summary>
		/// Minimum sample count.
		/// </summary>
		public const int MinimumSampleCount = 64;

		/// <summary>
		/// Maximum sample count.
		/// </summary>
		public const int MaximumSampleCount = 65472;
	}

	/// <summary>
	/// Audio driver.
	/// </summary>
	public interface IAudioDriver : IComponentInstance
	{
		/// <summary>
		/// Reserve a channel and prepare it for usage.
		/// </summary>
		/// <param name="channel">The channel to reserve or <see cref="AudioConstants.NextChannel"/> for the next available channel.</param>
		/// <param name="sampleCount">The sample count of the audio that will be played.</param>
		/// <param name="format">The format of hte audio that will be played.</param>
		/// <returns>The ID of the channel reserved.</returns>
		int ReserveChannel( int channel, int sampleCount, AudioFormat format );

		/// <summary>
		/// Release a channel.
		/// </summary>
		/// <param name="channel">The ID of the channel to release.</param>
		void ReleaseChannel( int channel );

		/// <summary>
		/// Release all channels.
		/// </summary>
		void ReleaseAllChannels();

		/// <summary>
		/// Output a block of audio at the given address.
		/// </summary>
		/// <param name="channel">The channel ID to play on.</param>
		/// <param name="buffer">A buffer containing the audio data.</param>
		/// <param name="block"><c>true</c> to block until the sample has played.</param>
		/// <param name="volume">The volume to play the sample at.</param>
		void Output( int channel, IntPtr buffer, bool block, int volume );

		/// <summary>
		/// Output a block of audio at the given address.
		/// </summary>
		/// <param name="channel">The channel ID to play on.</param>
		/// <param name="buffer">A buffer containing the audio data.</param>
		/// <param name="block"><c>true</c> to block until the sample has played.</param>
		/// <param name="leftVolume">The volume of the left speaker.</param>
		/// <param name="rightVolume">The volume of the right speaker.</param>
		void Output( int channel, IntPtr buffer, bool block, int leftVolume, int rightVolume );
	}
}
