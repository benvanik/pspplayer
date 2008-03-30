// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2008 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace Noxa.Emulation.Psp.Debugging.DebugModel
{
	/// <summary>
	/// Module info.
	/// </summary>
	public class ModuleInfo
	{
		/// <summary>
		/// The ID of the module.
		/// </summary>
		public uint ModuleID;

		/// <summary>
		/// The name of the module.
		/// </summary>
		public string Name;

		/// <summary>
		/// The path of the module file.
		/// </summary>
		public string Path;

		/// <summary>
		/// The entry address, as defined by the module.
		/// </summary>
		public uint EntryAddress;

		/// <summary>
		/// The lower memory address of the module after loading.
		/// </summary>
		public uint LowerBounds;

		/// <summary>
		/// The upper memory address of the module after loading.
		/// </summary>
		public uint UpperBounds;
	}
}
