// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2008 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace Noxa.Emulation.Psp.Player.Debugger.UserData
{
	public class TagInfo
	{
		public uint Address;
		public string Value;
	}

	public class CodeTagStore
	{
		public List<TagInfo> MethodNames = new List<TagInfo>();
		public List<TagInfo> LabelNames = new List<TagInfo>();
		public List<TagInfo> Comments = new List<TagInfo>();
	}
}
