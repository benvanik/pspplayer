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
	public enum Endianess
	{
		LittleEndian,
		BigEndian
	}

	public enum CpuStatisticsCapabilities
	{
		None = 0,
		InstructionsPerSecond = 0x001,
	}

	public interface ICpuCapabilities
	{
		Endianess Endianess
		{
			get;
		}

		bool VectorFpuSupported
		{
			get;
		}

		bool DmaSupported
		{
			get;
		}

		bool AvcSupported
		{
			get;
		}

		CpuStatisticsCapabilities SupportedStatistics
		{
			get;
		}

		bool DebuggingSupported
		{
			get;
		}
	}
}
