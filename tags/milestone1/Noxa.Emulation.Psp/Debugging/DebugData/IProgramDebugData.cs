// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

using Noxa.Emulation.Psp.Debugging.DebugModel;

namespace Noxa.Emulation.Psp.Debugging.DebugData
{
	/// <summary>
	/// Program debug database.
	/// </summary>
	public interface IProgramDebugData
	{
		/// <summary>
		/// A list of methods in the program.
		/// </summary>
		Method[] Methods
		{
			get;
		}

		/// <summary>
		/// Find a method by address.
		/// </summary>
		/// <param name="address">The address to look for.</param>
		/// <returns>The <see cref="Method"/> at or containing the given address, or <c>null</c> if it was not found.</returns>
		/// <remarks>
		/// Depending on the implementation, this may or may not return methods that contain
		/// the given address. Some implementations may only return ones that start with it.
		/// </remarks>
		Method FindMethod( int address );
	}
}
