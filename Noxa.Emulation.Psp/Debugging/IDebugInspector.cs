// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

using Noxa.Emulation.Psp.Bios;
using Noxa.Emulation.Psp.Debugging.DebugModel;

namespace Noxa.Emulation.Psp.Debugging
{
	#region Error objects

	/// <summary>
	/// Represents an error that can occur while debugging.
	/// </summary>
	public abstract class Error
	{
	}

	/// <summary>
	/// Describes <see cref="CpuError"/> codes.
	/// </summary>
	public enum CpuErrorCode
	{
		/// <summary>
		/// An undefined error.
		/// </summary>
		GenericError,

		/// <summary>
		/// An error during code generation.
		/// </summary>
		GenerationError,

		/// <summary>
		/// An error executing code.
		/// </summary>
		RuntimeError,

		/// <summary>
		/// An error during a syscall.
		/// </summary>
		SyscallError,
	}

	/// <summary>
	/// Represents an error that the CPU can throw.
	/// </summary>
	public class CpuError : Error
	{
		/// <summary>
		/// The error code.
		/// </summary>
		public readonly CpuErrorCode Code;

		/// <summary>
		/// An optional message describing the error.
		/// </summary>
		public readonly string Message;

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
		public CpuError( CpuErrorCode code, string message )
		{
			this.Code = code;
			this.Message = message;
		}
	}

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
	public class BiosError : Error
	{
		/// <summary>
		/// The error code.
		/// </summary>
		public readonly BiosErrorCode Code;

		/// <summary>
		/// An optional message describing the error.
		/// </summary>
		public readonly string Message;

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
		{
			this.Code = code;
			this.Message = message;
		}
	}

	#endregion

	/// <summary>
	/// Debugger inspection control.
	/// </summary>
	public interface IDebugInspector
	{
		/// <summary>
		/// Handle a stop after a step operation.
		/// </summary>
		/// <param name="address">The address of the next instruction that will be executed.</param>
		void OnStepComplete( int address );

		/// <summary>
		/// Handle a breakpoint that has been hit.
		/// </summary>
		/// <param name="address">The address of the breakpoint.</param>
		void OnBreakpointHit( int address );

		/// <summary>
		/// Handle a CPU error.
		/// </summary>
		/// <param name="address">The address that caused the error.</param>
		/// <param name="error">The error description instance.</param>
		void OnCpuError( int address, CpuError error );

		/// <summary>
		/// Handle a BIOS error.
		/// </summary>
		/// <param name="address">The calling address of the BIOS routine.</param>
		/// <param name="error">The error description instance.</param>
		void OnBiosError( int address, BiosError error );
	}
}
