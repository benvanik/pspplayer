using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection.Emit;

namespace Noxa.Emulation.Psp.Cpu.Generation
{
	static class FpuInstructions
	{
		public static bool GenerateIL( Cpu cpu, Core core0, Memory memory, ILGenerator ilgen, uint instruction, out bool breakOut )
		{
			breakOut = false;

			return false;
		}
	}
}
