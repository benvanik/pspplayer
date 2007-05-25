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
	/// A BIOS function reference.
	/// </summary>
	[Serializable]
	public class BiosFunctionToken
	{
		/// <summary>
		/// The name of the module that contains the function, if available.
		/// </summary>
		public readonly string ModuleName;

		/// <summary>
		/// The function name, if available.
		/// </summary>
		public readonly string MethodName;

		/// <summary>
		/// The NID of the function, if available.
		/// </summary>
		public readonly uint NID;

		/// <summary>
		/// Initializes a new <see cref="BiosFunctionToken"/> instance with the given parameters.
		/// </summary>
		/// <param name="nid">The NID of the function.</param>
		public BiosFunctionToken( uint nid )
		{
			this.NID = nid;
		}

		/// <summary>
		/// Initializes a new <see cref="BiosFunctionToken"/> instance with the given parameters.
		/// </summary>
		/// <param name="moduleName">The name of the module that contains the function.</param>
		/// <param name="methodName">The name of the function.</param>
		public BiosFunctionToken( string moduleName, string methodName )
		{
			this.ModuleName = moduleName;
			this.MethodName = methodName;
		}

		/// <summary>
		/// Returns a <see cref="String"/> that represents the BIOS function.
		/// </summary>
		/// <returns>A <see cref="String"/> representing the function.</returns>
		public override string ToString()
		{
			if( this.NID != 0x0 )
				return string.Format( "0x{0:X8}", this.NID );
			else
				return string.Format( "{0}::{1}", this.ModuleName, this.MethodName );
		}
	}
}
