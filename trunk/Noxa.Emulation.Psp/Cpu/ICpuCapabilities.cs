// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace Noxa.Emulation.Psp.Cpu
{
	/// <summary>
	/// Describes the statistics capabilities an <see cref="ICpu"/> may have.
	/// </summary>
	public enum CpuStatisticsCapabilities
	{
		/// <summary>
		/// No statistics supported.
		/// </summary>
		None = 0,

		/// <summary>
		/// Instructions per second supported.
		/// </summary>
		InstructionsPerSecond = 0x001,
	}

	/// <summary>
	/// <see cref="ICpu"/> capabilities.
	/// </summary>
	public interface ICpuCapabilities
	{
		/// <summary>
		/// <c>true</c> if the audio/video decoder is supported.
		/// </summary>
		bool AvcSupported
		{
			get;
		}

		/// <summary>
		/// <c>true</c> if debugging is supported.
		/// </summary>
		bool DebuggingSupported
		{
			get;
		}

		/// <summary>
		/// The statistics capabilities supported.
		/// </summary>
		CpuStatisticsCapabilities SupportedStatistics
		{
			get;
		}
	}
}
