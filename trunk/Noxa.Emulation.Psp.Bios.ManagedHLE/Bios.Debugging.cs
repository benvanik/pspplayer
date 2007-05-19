// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

using Noxa.Emulation.Psp.Cpu;
using Noxa.Emulation.Psp.Debugging;
using Noxa.Emulation.Psp.Debugging.Hooks;
using Noxa.Emulation.Psp.Games;

namespace Noxa.Emulation.Psp.Bios.ManagedHLE
{
	partial class Bios
	{
		private BiosDebugHook _hook;

		public bool DebuggingEnabled
		{
			get
			{
				return ( _hook != null );
			}
		}

		public bool SupportsDebugging
		{
			get
			{
#if DEBUG
				return true;
#else
				return false;
#endif
			}
		}

		public IHook DebugHook
		{
			get
			{
				return _hook;
			}
		}

		public void EnableDebugging()
		{
			Debug.Assert( this.SupportsDebugging == true );
			_hook = new BiosDebugHook( this );
		}
	}
}
