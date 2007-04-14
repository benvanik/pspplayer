// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Noxa.Emulation.Psp.Cpu;
using Noxa.Emulation.Psp.Debugging;
using Noxa.Emulation.Psp.Games;

namespace Noxa.Emulation.Psp.Bios.ManagedHLE
{
	partial class Bios
	{
		public bool DebuggingEnabled
		{
			get
			{
				return false;
			}
		}

		public IDebugger Debugger
		{
			get
			{
				return null;
			}
		}

		public IBiosHook DebugHook
		{
			get
			{
				return null;
			}
		}

		public void EnableDebugging( IDebugger debugger )
		{
			throw new NotImplementedException();
		}
	}
}
