// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace Noxa.Emulation.Psp.Bios
{
	/// <summary>
	/// Marks a function as being an exported BIOS routine and defines the unique ID and
	/// name of a BIOS function.
	/// </summary>
	[global::System.AttributeUsage( AttributeTargets.Method, Inherited = false, AllowMultiple = false )]
	public sealed class BiosFunctionAttribute : Attribute
	{
		/// <summary>
		/// The NID (unique ID) of the function.
		/// </summary>
		public readonly uint NID;

		/// <summary>
		/// The name of the function.
		/// </summary>
		public readonly string Name;

		/// <summary>
		/// Marks a method as a BIOS function.
		/// </summary>
		/// <param name="nid">The NID (unique ID) of the function.</param>
		/// <param name="name">The name of the function.</param>
		public BiosFunctionAttribute( uint nid, string name )
		{
			this.NID = nid;
			this.Name = name;
		}
	}
}
