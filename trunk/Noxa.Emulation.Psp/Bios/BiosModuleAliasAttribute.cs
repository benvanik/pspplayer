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
	/// Marks a BIOS module with an alias.
	/// </summary>
	[AttributeUsage( AttributeTargets.Class, Inherited = false, AllowMultiple = true )]
	public sealed class BiosModuleAliasAttribute : Attribute
	{
		/// <summary>
		/// The alias of the BIOS module.
		/// </summary>
		public readonly string Alias;

		/// <summary>
		/// Marks a BIOS module with an alias.
		/// </summary>
		/// <param name="alias">The alias to mark the module with.</param>
		public BiosModuleAliasAttribute( string alias )
		{
			this.Alias = alias;
		}
	}
}
