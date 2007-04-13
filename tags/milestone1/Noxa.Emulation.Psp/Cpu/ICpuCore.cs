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
	/// An <see cref="ICpu"/> core.
	/// </summary>
	public interface ICpuCore
	{
		/// <summary>
		/// The name of the core.
		/// </summary>
		string Name
		{
			get;
		}

		/// <summary>
		/// The current program counter.
		/// </summary>
		int ProgramCounter
		{
			get;
			set;
		}

		/// <summary>
		/// Set the value of a general register.
		/// </summary>
		/// <param name="register">The ordinal of the register to set.</param>
		/// <param name="value">The new value of the register.</param>
		void SetGeneralRegister( int register, int value );

		/// <summary>
		/// Get the current CPU context.
		/// </summary>
		object Context
		{
			get;
			set;
		}

		/// <summary>
		/// The current interrupts masking flag.
		/// </summary>
		uint InterruptsFlag
		{
			get;
			set;
		}
	}
}
