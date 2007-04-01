// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Noxa.Emulation.Psp.Media;

namespace Noxa.Emulation.Psp.Bios
{
	/// <summary>
	/// Parameters for an <see cref="ILoader"/> operation.
	/// </summary>
	public class LoadParameters
	{
		/// <summary>
		/// The path of the game or module.
		/// </summary>
		public IMediaFolder Path;

		/// <summary>
		/// Used internally to preserve table structure that may later be used by the BIOS.
		/// </summary>
		public bool PreserveData;

		/// <summary>
		/// <c>true</c> to find imports only and not load the module.
		/// </summary>
		public bool ImportsOnly;
	}
}
