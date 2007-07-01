// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Noxa.Emulation.Psp.Debugging.DebugModel
{
	/// <summary>
	/// Represents a variable inside of a program debug database.
	/// </summary>
	[Serializable]
	public class Variable
	{
		/// <summary>
		/// The start address of the variable.
		/// </summary>
		public readonly uint Address;

		/// <summary>
		/// The length, in bytes, of the variable.
		/// </summary>
		public readonly uint Length;

		/// <summary>
		/// The name of the variable, if available.
		/// </summary>
		public readonly string Name;

		/// <summary>
		/// Initializes a new <see cref="Variable"/> instance with the given parameters.
		/// </summary>
		/// <param name="address">The start address of the variable.</param>
		/// <param name="length">The length of the variable, in bytes.</param>
		/// <param name="name">The name of the method, if available.</param>
		public Variable( uint address, uint length, string name )
		{
			Address = address;
			Length = length;
			Name = name;
		}
	}
}
