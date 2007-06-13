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
		/// Initializes a new <see cref="Error"/> instance with the given parameters.
		/// </summary>
		/// <param name="message">An optional message describing the error.</param>
		public Error( string message )
		{
			this.Message = message;
		}
	}

	#region CpuError

	/// <summary>
	/// Describes <see cref="CpuError"/> codes.
	/// </summary>
	public enum CpuErrorCode
	{
		/// <summary>
		/// An undefined error.
		/// </summary>
		Generic,
		/// <summary>
		/// An error during code generation.
		/// </summary>
		Generation,
		/// <summary>
		/// An error executing code.
		/// </summary>
		Runtime,
		/// <summary>
		/// An error during a syscall.
		/// </summary>
		Syscall,
		/// <summary>
		/// An error occured on a memory access attmept.
		/// </summary>
		InvalidAccess,
	}

	/// <summary>
	/// Represents an error that the CPU can throw.
	/// </summary>
	[Serializable]
	public class CpuError : Error
	{
		/// <summary>
		/// The error code.
		/// </summary>
		public readonly CpuErrorCode Code;

		/// <summary>
		/// Initializes a new <see cref="CpuError"/> instance with the given parameters.
		/// </summary>
		/// <param name="code">The error code.</param>
		public CpuError( CpuErrorCode code )
			: this( code, null )
		{
		}

		/// <summary>
		/// Initializes a new <see cref="CpuError"/> instance with the given parameters.
		/// </summary>
		/// <param name="code">The error code.</param>
		/// <param name="message">An optional message describing the error.</param>
		public CpuError( CpuErrorCode code,string message )
			: base( message )
		{
			this.Code = code;
		}

		/// <summary>
		/// Returns a <see cref="String"/> that represents the current <see cref="CpuError"/>.
		/// </summary>
		/// <returns>A <see cref="String"/> representing the current <see cref="CpuError"/>.</returns>
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendFormat( "{0}: ", Code );
			if( Message != null )
				sb.Append( Message );
			return sb.ToString();
		}
	}

	#endregion

	#region BiosError

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

	#endregion
}