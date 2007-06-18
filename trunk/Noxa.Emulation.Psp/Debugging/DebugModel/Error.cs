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
	/// Represents an error that can occur while debugging.
	/// </summary>
	[Serializable]
	public abstract class Error
	{
		/// <summary>
		/// An optional message describing the error.
		/// </summary>
		public readonly string Message;

		/// <summary>
		/// Current program counter, if available.
		/// </summary>
		public readonly uint PC;

		/// <summary>
		/// Initializes a new <see cref="Error"/> instance with the given parameters.
		/// </summary>
		/// <param name="message">An optional message describing the error.</param>
		public Error( string message )
		{
			this.Message = message;
		}

		/// <summary>
		/// Initializes a new <see cref="Error"/> instance with the given parameters.
		/// </summary>
		/// <param name="message">An optional message describing the error.</param>
		/// <param name="pc">The current program counter.</param>
		public Error( string message, uint pc )
			: this( message )
		{
			this.PC = pc;
		}
	}
}
