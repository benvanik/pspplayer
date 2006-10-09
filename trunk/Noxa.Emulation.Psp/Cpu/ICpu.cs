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

		bool InternalMemorySupported
		{
			get;
		}

		CpuStatisticsCapabilities SupportedStatistics
		{
			get;
		}
	}

	public interface ICpuStatistics
	{
		int InstructionsPerSecond
		{
			get;
		}
	}

	public interface ICpu : IComponentInstance
	{
		ICpuCapabilities Capabilities
		{
			get;
		}

		ICpuStatistics Statistics
		{
			get;
		}

		IClock Clock
		{
			get;
		}

		ICpuCore[] Cores
		{
			get;
		}

		ICpuCore this[ int core ]
		{
			get;
		}

		IDmaController Dma
		{
			get;
		}

		IAvcDecoder Avc
		{
			get;
		}

		IMemory Memory
		{
			get;
		}

		byte[] InternalMemory
		{
			get;
		}

		int InternalMemoryBaseAddress
		{
			get;
		}

		int RegisterSyscall( uint nid );

		int ExecuteBlock();

		void PrintStatistics();
	}
}
