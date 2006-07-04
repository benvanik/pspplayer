using System;
using System.Collections.Generic;
using System.Text;

namespace Noxa.Emulation.Psp.Cpu
{
	public interface ICpuCore
	{
		string Name
		{
			get;
		}

		int ProgramCounter
		{
			get;
			set;
		}

		int[] GeneralRegisters
		{
			get;
		}

		object Context
		{
			get;
			set;
		}
	}
}
