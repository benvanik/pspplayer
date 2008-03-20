// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2008 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace Noxa.Emulation.Psp.Player.Debugger
{
	enum DebuggerState
	{
		/// <summary>
		/// The debugger is not attached.
		/// </summary>
		Detached,
		/// <summary>
		/// The debugger is idle, waiting to start.
		/// </summary>
		Idle,
		/// <summary>
		/// The debugger is running code.
		/// </summary>
		Running,
		/// <summary>
		/// The debugger is stopped in the middle of execution.
		/// </summary>
		Broken,
	}
}
