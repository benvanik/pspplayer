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
	/// Describes <see cref="BiosError"/> codes.
	/// </summary>
	public enum BiosErrorCode
	{
		/// <summary>
		/// An undefined error.
		/// </summary>
		GenericError,
	}

	/// <summary>
	/// Represents an error that the BIOS can throw.
	/// </summary>
	[Serializable]
	public class BiosError : Error
	{
		/// <summary>
		/// The error code.
		/// </summary>
		public readonly BiosErrorCode Code;

		/// <summary>
		/// Initializes a new <see cref="BiosError"/> instance with the given parameters.
		/// </summary>
		/// <param name="code">The error code.</param>
		public BiosError( BiosErrorCode code )
			: this( code, null )
		{
		}

		/// <summary>
		/// Initializes a new <see cref="BiosError"/> instance with the given parameters.
		/// </summary>
		/// <param name="code">The error code.</param>
		/// <param name="message">An optional message describing the error.</param>
		public BiosError( BiosErrorCode code, string message )
			: base( message )
		{
			this.Code = code;
		}
	}
}
