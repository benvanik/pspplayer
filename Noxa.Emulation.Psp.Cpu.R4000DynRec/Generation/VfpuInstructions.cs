// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection.Emit;

namespace Noxa.Emulation.Psp.Cpu.Generation
{
	static class VfpuInstructions
	{
		public static bool GenerateIL( Cpu cpu, Core core0, Memory memory, ILGenerator ilgen, uint instruction, out bool breakOut )
		{
			breakOut = false;

			return false;
		}
	}
}
