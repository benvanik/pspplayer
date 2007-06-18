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
	/// Describes <see cref="MemoryError"/> codes.
	/// </summary>
	public enum MemoryErrorCode
	{
		/// <summary>
		/// An undefined error.
		/// </summary>
		GenericError,
		/// <summary>
		/// Attempt to read from an invalid address.
		/// </summary>
		InvalidRead,
		/// <summary>
		/// Attempt to write to an invalid address.
		/// </summary>
		InvalidWrite,
	}

	/// <summary>
	/// Represents an error that the memory system can throw.
	/// </summary>
	[Serializable]
	public class MemoryError : Error
	{
		/// <summary>
		/// The error code.
		/// </summary>
		public readonly MemoryErrorCode Code;

		/// <summary>
		/// The address the memory operation pertains to.
		/// </summary>
		public readonly uint TargetAddress;

		/// <summary>
		/// The width, in bytes, of the operation.
		/// </summary>
		public readonly byte Width;

		/// <summary>
		/// The value being written.
		/// </summary>
		public readonly ulong Value;

		/// <summary>
		/// Initializes a new <see cref="MemoryError"/> instance with the given parameters.
		/// </summary>
		/// <param name="code">The error code.</param>
		/// <param name="pc">The current program counter.</param>
		public MemoryError( MemoryErrorCode code, uint pc )
			: this( code, pc, null )
		{
		}

		/// <summary>
		/// Initializes a new <see cref="MemoryError"/> instance with the given parameters.
		/// </summary>
		/// <param name="code">The error code.</param>
		/// <param name="pc">The current program counter.</param>
		/// <param name="message">An optional message describing the error.</param>
		public MemoryError( MemoryErrorCode code, uint pc, string message )
			: base( message, pc )
		{
			this.Code = code;
		}

		/// <summary>
		/// Initializes a new <see cref="MemoryError"/> instance with the given parameters.
		/// </summary>
		/// <param name="code">The error code.</param>
		/// <param name="pc">The current program counter.</param>
		/// <param name="targetAddress">The address being accessed.</param>
		/// <param name="width">The width of the operation, in bytes.</param>
		public MemoryError( MemoryErrorCode code, uint pc, uint targetAddress, byte width )
			: this( code, pc, null )
		{
			this.TargetAddress = targetAddress;
			this.Width = width;
		}

		/// <summary>
		/// Initializes a new <see cref="MemoryError"/> instance with the given parameters.
		/// </summary>
		/// <param name="code">The error code.</param>
		/// <param name="pc">The current program counter.</param>
		/// <param name="targetAddress">The address being accessed.</param>
		/// <param name="width">The width of the operation, in bytes.</param>
		/// <param name="value">The value being written.</param>
		public MemoryError( MemoryErrorCode code, uint pc, uint targetAddress, byte width, ulong value )
			: this( code, pc, null )
		{
			this.TargetAddress = targetAddress;
			this.Width = width;
			this.Value = value;
		}

		/// <summary>
		/// Returns a <see cref="String"/> that represents the current <see cref="CpuError"/>.
		/// </summary>
		/// <returns>A <see cref="String"/> representing the current <see cref="CpuError"/>.</returns>
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendFormat( "{0}: ", Code );
			if( PC != 0x0 )
				sb.AppendFormat( "[0x{0:X8}] ", PC );
			if( Message != null )
			{
				sb.Append( Message );
				sb.Append( " " );
			}
			switch( Code )
			{
				case MemoryErrorCode.InvalidRead:
					sb.AppendFormat( "read from 0x{0:X8}/{1}", TargetAddress, Width );
					break;
				case MemoryErrorCode.InvalidWrite:
					string hex;
					switch( Width )
					{
						case 1:
							hex = string.Format( "{0:X1}", Value );
							break;
						case 2:
							hex = string.Format( "{0:X2}", Value );
							break;
						default:
						case 4:
							hex = string.Format( "{0:X4}", Value );
							break;
						case 8:
							hex = string.Format( "{0:X8}", Value );
							break;
					}
					sb.AppendFormat( "write {0} (0x{1}) to 0x{2:X8}/{3}", Value, hex, TargetAddress, Width );
					break;
			}
			return sb.ToString();
		}
	}
}
