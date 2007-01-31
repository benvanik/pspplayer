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
	public class CoreState
	{
		public int ProgramCounter;
		public int[] GeneralRegisters;
		public int Hi;
		public int Lo;
		public bool LL;

		public int[] Cp0Registers;
		public int[] Cp0ControlRegisters;
		public bool Cp0ConditionBit;

		public uint FpuControlRegister;
		public float[] FpuRegisters;
		
		//internal bool InDelaySlot;
		//internal int DelayPc;
		//internal bool DelayNop;
		//internal int InterruptState;
	}
}
