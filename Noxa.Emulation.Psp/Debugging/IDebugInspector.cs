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

	public abstract class Error
	{
	}

	public enum CpuErrorCode
	{
		GenericError,
		GenerationError,
		RuntimeError,
		SyscallError,
	}

	public class CpuError : Error
	{
		protected CpuErrorCode _code;
		protected string _message;

		public CpuError( CpuErrorCode code )
			: this( code, null )
		{
		}

		public CpuError( CpuErrorCode code, string message )
		{
			_code = code;
			_message = message;
		}

		public CpuErrorCode Code
		{
			get
			{
				return _code;
			}
		}

		public string Message
		{
			get
			{
				return _message;
			}
		}
	}

	public enum BiosErrorCode
	{
		GenericError,
	}

	public class BiosError : Error
	{
		protected BiosErrorCode _code;
		protected string _message;

		public BiosError( BiosErrorCode code )
			: this( code, null )
		{
		}

		public BiosError( BiosErrorCode code, string message )
		{
			_code = code;
			_message = message;
		}

		public BiosErrorCode Code
		{
			get
			{
				return _code;
			}
		}

		public string Message
		{
			get
			{
				return _message;
			}
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
