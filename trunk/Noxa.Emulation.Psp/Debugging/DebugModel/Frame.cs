// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace Noxa.Emulation.Psp.Debugging.DebugModel
{
	/// <summary>
	/// A single frame in a callstack.
	/// </summary>
	[Serializable]
	public class Frame
	{
		/// <summary>
		/// The entry address.
		/// </summary>
		public readonly int Address;

		/// <summary>
		/// Initializes a new <see cref="Frame"/> instance with the given parameters.
		/// </summary>
		/// <param name="address">The entry address of the method.</param>
		protected Frame( int address )
		{
			this.Address = address;
		}
	}
}
