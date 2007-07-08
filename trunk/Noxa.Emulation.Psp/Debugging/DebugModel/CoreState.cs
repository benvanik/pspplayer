// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace Noxa.Emulation.Psp.Debugging.DebugModel
{
	#pragma warning disable 1591

	/// <summary>
	/// CPU core state.
	/// </summary>
	[Serializable]
	public class CoreState
	{
		public uint ProgramCounter;
		public uint[] GeneralRegisters;
		public uint Hi;
		public uint Lo;
		public bool LL;

		public uint[] Cp0Registers;
		public uint[] Cp0ControlRegisters;
		public bool Cp0ConditionBit;

		public uint FpuControlRegister;
		public bool FpuConditionBit;
		public float[] FpuRegisters;

		public uint VfpuControlRegister;
		public float[] VfpuRegisters;
		
		//bool InDelaySlot;
		//int DelayPc;
		//bool DelayNop;
		//int InterruptState;
	}

	#pragma warning restore
}
