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
	/// Represents a method inside of a program debug database.
	/// </summary>
	public abstract class Method
	{
		/// <summary>
		/// The start address of the method.
		/// </summary>
		public readonly int EntryAddress;

		/// <summary>
		/// The length, in bytes, of the method.
		/// </summary>
		public readonly int Length;

		/// <summary>
		/// The name of the method, if available.
		/// </summary>
		public readonly string Name;

		/// <summary>
		/// Initializes a new <see cref="Method"/> instance with the given parameters.
		/// </summary>
		/// <param name="entryAddress">The start address of the method.</param>
		/// <param name="length">The length of the method, in bytes.</param>
		/// <param name="name">The name of the method, if available.</param>
		protected Method( int entryAddress, int length, string name )
		{
			EntryAddress = entryAddress;
			Length = length;
			Name = name;
		}

		/// <summary>
		/// Returns a <see cref="String"/> that represents the current <see cref="Method"/>.
		/// </summary>
		public override string ToString()
		{
			return string.Format( "{0} (0x{1:X8})", this.Name, this.EntryAddress );
		}
	}
}
