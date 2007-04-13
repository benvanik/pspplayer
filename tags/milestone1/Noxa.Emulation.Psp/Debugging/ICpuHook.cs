// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

using Noxa.Emulation.Psp.Debugging.DebugModel;

namespace Noxa.Emulation.Psp.Debugging
{
	/// <summary>
	/// Hook inside of the CPU that allows the debugger to extract information.
	/// </summary>
	public interface ICpuHook
	{
		/// <summary>
		/// Get the processor state for the given core.
		/// </summary>
		/// <param name="core">Core ordinal.</param>
		/// <returns>State information for the given core.</returns>
		CoreState GetCoreState( int core );

		/// <summary>
		/// Attempt to extract the callstack if frame tracking is enabled.
		/// </summary>
		/// <returns>The callstack or <c>null</c> if frame tracking is disabled.</returns>
		Frame[] GetCallstack();
	}
}
