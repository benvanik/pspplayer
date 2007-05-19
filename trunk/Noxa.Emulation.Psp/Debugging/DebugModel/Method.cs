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
	/// Defines the type of a <see cref="Method"/>.
	/// </summary>
	public enum MethodType
	{
		/// <summary>
		/// Method consists of user code.
		/// </summary>
		User,
		/// <summary>
		/// Method is a BIOS stub.
		/// </summary>
		Bios,
	}

	/// <summary>
	/// Represents a method inside of a program debug database.
	/// </summary>
	[Serializable]
	public class Method
	{
		/// <summary>
		/// The type of the method.
		/// </summary>
		public readonly MethodType Type;

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
		/// A list of breakpoint IDs defined inside the method.
		/// </summary>
		public List<int> Breakpoints;

		/// <summary>
		/// Initializes a new <see cref="Method"/> instance with the given parameters.
		/// </summary>
		/// <param name="type">The type of the method.</param>
		/// <param name="entryAddress">The start address of the method.</param>
		/// <param name="length">The length of the method, in bytes.</param>
		/// <param name="name">The name of the method, if available.</param>
		public Method( MethodType type, int entryAddress, int length, string name )
		{
			Type = type;
			EntryAddress = entryAddress;
			Length = length;
			Name = name;

			Breakpoints = new List<int>();
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
