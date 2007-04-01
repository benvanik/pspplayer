// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Noxa.Emulation.Psp.Bios
{
	/// <summary>
	/// Represents a module inside the BIOS that can contain <see cref="BiosFunction"/> instances.
	/// </summary>
	public class BiosModule
	{
		/// <summary>
		/// The name of the module.
		/// </summary>
		public readonly string Name;

		/// <summary>
		/// Initializes a new <see cref="BiosModule"/> instance with the given parameters.
		/// </summary>
		/// <param name="name">The name of the module.</param>
		public BiosModule( string name )
		{
			Debug.Assert( name != null );
			this.Name = name;
		}
	}
}
