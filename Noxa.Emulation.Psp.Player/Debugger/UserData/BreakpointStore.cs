// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2008 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Noxa.Emulation.Psp.Debugging.DebugModel;
using Noxa.Emulation.Psp.Debugging.Hooks;

namespace Noxa.Emulation.Psp.Player.Debugger.UserData
{
	public class BreakpointInfo
	{
		public BreakpointType Type;
		public MemoryAccessType AccessType;
		public uint Address;
		public BreakpointMode Mode;
		public bool Enabled;
		public string Name;
	}

	public class BreakpointStore
	{
		public List<BreakpointInfo> Infos = new List<BreakpointInfo>();
	}
}
