// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

using Noxa.Emulation.Psp.Debugging;

namespace Noxa.Emulation.Psp
{
	/// <summary>
	/// Emulation host.
	/// </summary>
	public interface IEmulationHost
	{
		/// <summary>
		/// The current <see cref="IEmulationInstance"/>.
		/// </summary>
		IEmulationInstance CurrentInstance
		{
			get;
		}

		/// <summary>
		/// <c>true</c> if the debugger is attached.
		/// </summary>
		bool IsDebuggerAttached
		{
			get;
		}

		/// <summary>
		/// The current <see cref="DebugHost"/>.
		/// </summary>
		DebugHost Debugger
		{
			get;
		}

		/// <summary>
		/// Attach the debugger to the current <see cref="IEmulationInstance"/>.
		/// </summary>
		void AttachDebugger();
	}
}
