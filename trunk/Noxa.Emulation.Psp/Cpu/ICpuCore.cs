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
