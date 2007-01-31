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
	public interface IEmulationHost
	{
		IEmulationInstance CurrentInstance
		{
			get;
		}

		IDebugger Debugger
		{
			get;
		}

		void AttachDebugger();
	}
}
