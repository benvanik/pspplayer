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
	/// Represents a symbol inside of a program debug database.
	/// </summary>
	[Serializable]
	public abstract class Symbol
	{
		/// <summary>
		/// The start address of the symbol.
		/// </summary>
		public readonly uint Address;

		/// <summary>
		/// The length, in bytes, of the symbol.
		/// </summary>
		public readonly uint Length;

		/// <summary>
		/// Initializes a new <see cref="Symbol"/> instance with the given parameters.
		/// </summary>
		/// <param name="address">The start address of the symbol.</param>
		/// <param name="length">The length of the symbol, in bytes.</param>
		public Symbol( uint address, uint length )
		{
			Address = address;
			Length = length;
		}
	}
}
