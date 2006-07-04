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
	}

	public interface ICpu : IComponentInstance
	{
		ICpuCapabilities Capabilities
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

		int RegisterSyscall( uint nid );

		int ExecuteBlock();

		void PrintStatistics();
	}
}
