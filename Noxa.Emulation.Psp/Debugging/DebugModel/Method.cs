// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using Noxa.Emulation.Psp.Bios;

namespace Noxa.Emulation.Psp.Debugging.DebugModel
{
	/// <summary>
	/// Defines the type of a <see cref="Method"/>.
	/// </summary>
	public enum MethodType
	{
		/// <summary>
		/// Method consists of user code.
		/// </summary>
		User,
		/// <summary>
		/// Method is a BIOS stub.
		/// </summary>
		Bios,
	}

	/// <summary>
	/// Represents a method inside of a program debug database.
	/// </summary>
	[Serializable]
	public class Method
	{
		/// <summary>
		/// The type of the method.
		/// </summary>
		public readonly MethodType Type;

		/// <summary>
		/// The start address of the method.
		/// </summary>
		public readonly uint Address;

		/// <summary>
		/// The length, in bytes, of the method.
		/// </summary>
		public readonly uint Length;

		/// <summary>
		/// The name of the method, if available.
		/// </summary>
		public readonly string Name;

		/// <summary>
		/// The BIOS function the method points to, if available.
		/// </summary>
		public readonly BiosFunction Function;

		/// <summary>
		/// Initializes a new <see cref="Method"/> instance with the given parameters.
		/// </summary>
		/// <param name="type">The type of the method.</param>
		/// <param name="address">The start address of the method.</param>
		/// <param name="length">The length of the method, in bytes.</param>
		public Method( MethodType type, uint address, uint length )
		{
			Type = type;
			Address = address;
			Length = length;

			//Debug.Assert( address % 4 == 0 );
			Debug.Assert( length % 4 == 0 );
		}

		/// <summary>
		/// Initializes a new <see cref="Method"/> instance with the given parameters.
		/// </summary>
		/// <param name="type">The type of the method.</param>
		/// <param name="address">The start address of the method.</param>
		/// <param name="length">The length of the method, in bytes.</param>
		/// <param name="name">The name of the method, if available.</param>
		public Method( MethodType type, uint address, uint length, string name )
			: this( type, address, length )
		{
			Name = name;
		}

		/// <summary>
		/// Initializes a new <see cref="Method"/> instance with the given parameters.
		/// </summary>
		/// <param name="type">The type of the method.</param>
		/// <param name="address">The start address of the method.</param>
		/// <param name="length">The length of the method, in bytes.</param>
		/// <param name="function">The BIOS function, if available.</param>
		public Method( MethodType type, uint address, uint length, BiosFunction function )
			: this( type, address, length )
		{
			Function = function;
		}

		/// <summary>
		/// Returns a <see cref="String"/> that represents the current <see cref="Method"/>.
		/// </summary>
		public override string ToString()
		{
			if( this.Function != null )
				return string.Format( "0x{0:X8}-0x{1:X8} ({2,5}b) {3}", this.Address, this.Address + this.Length, this.Length, this.Function.ToString() );
			else if( this.Name != null )
				return string.Format( "0x{0:X8}-0x{1:X8} ({2,5}b) {3}", this.Address, this.Address + this.Length, this.Length, this.Name );
			else
				return string.Format( "0x{0:X8}-0x{1:X8} ({2,5}b)", this.Address, this.Address + this.Length, this.Length );
		}
	}
}
