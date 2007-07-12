// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using Noxa.Emulation.Psp.Bios;
using Noxa.Emulation.Psp.Debugging.Hooks;

namespace Noxa.Emulation.Psp.Debugging.DebugModel
{
	/// <summary>
	/// Describes the type of <see cref="Breakpoint"/>.
	/// </summary>
	public enum BreakpointType
	{
		/// <summary>
		/// Fired when a line of code is executed.
		/// </summary>
		CodeExecute,
		/// <summary>
		/// Fired when a BIOS function is executed.
		/// </summary>
		BiosFunction,
		/// <summary>
		/// Fired when a memory address is accessed.
		/// </summary>
		MemoryAccess,
		/// <summary>
		/// Internal stepping breakpoint. Don't use!
		/// </summary>
		Stepping,
	}

	/// <summary>
	/// Describes the mode of a <see cref="Breakpoint"/>.
	/// </summary>
	public enum BreakpointMode
	{
		/// <summary>
		/// Do nothing when hit (act as a counter).
		/// </summary>
		Silent,
		/// <summary>
		/// Break when hit.
		/// </summary>
		Break,
		/// <summary>
		/// Trace a message when hit.
		/// </summary>
		Trace,
	}

	/// <summary>
	/// A breakpoint.
	/// </summary>
	[Serializable]
	public class Breakpoint
	{
		/// <summary>
		/// Unique ID of the breakpoint.
		/// </summary>
		public readonly int ID;

		/// <summary>
		/// The type of the breakpoint.
		/// </summary>
		public readonly BreakpointType Type;

		/// <summary>
		/// The memory access type.
		/// </summary>
		public readonly MemoryAccessType AccessType;

		/// <summary>
		/// The address of the breakpoint.
		/// </summary>
		public readonly uint Address;

		/// <summary>
		/// The current mode of the breakpoint.
		/// </summary>
		public BreakpointMode Mode;

		/// <summary>
		/// The BIOS function of the breakpoint.
		/// </summary>
		public readonly BiosFunctionToken Function;

		/// <summary>
		/// The enabled state of the breakpoint.
		/// </summary>
		public bool Enabled;

		/// <summary>
		/// The optional user-defined name of the breakpoint.
		/// </summary>
		public string Name;

		/// <summary>
		/// The number of times the breakpoint has been hit.
		/// </summary>
		public long HitCount;

		/// <summary>
		/// Internal: Used by the CPU.
		/// </summary>
		[NonSerialized]
		public object Internal;

		/// <summary>
		/// Internal: Used by the CPU.
		/// </summary>
		[NonSerialized]
		public BiosFunction CachedFunction;

		private Breakpoint( int id, BreakpointType type )
		{
			this.ID = id;
			this.Type = type;
			this.Name = null;
			this.Mode = BreakpointMode.Break;
			this.Enabled = true;
		}

		/// <summary>
		/// Initializes a new <see cref="Breakpoint"/> instance with the given parameters.
		/// </summary>
		/// <param name="id">A unique ID used to lookup the breakpoint.</param>
		/// <param name="type">The breakpoint type.</param>
		/// <param name="address">The address of the breakpoint.</param>
		public Breakpoint( int id, BreakpointType type, uint address )
			: this( id, type )
		{
			this.Address = address;
		}

		/// <summary>
		/// Initializes a new <see cref="Breakpoint"/> instance with the given parameters.
		/// </summary>
		/// <param name="id">A unique ID used to lookup the breakpoint.</param>
		/// <param name="address">The memory address to break at.</param>
		/// <param name="accessType">The access type that triggers the breakpoint.</param>
		public Breakpoint( int id, uint address, MemoryAccessType accessType )
			: this( id, BreakpointType.MemoryAccess )
		{
			this.AccessType = accessType;
			this.Address = address;
		}

		/// <summary>
		/// Initializes a new <see cref="Breakpoint"/> instance with the given parameters.
		/// </summary>
		/// <param name="id">A unique ID used to lookup the breakpoint.</param>
		/// <param name="function">The BIOS function to break on.</param>
		public Breakpoint( int id, BiosFunctionToken function )
			: this( id, BreakpointType.BiosFunction )
		{
			this.Function = function;
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> representation of the <see cref="Breakpoint"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> representation of the <see cref="Breakpoint"/>.</returns>
		public override string ToString()
		{
			return string.Format( "{0}{1} ({2} 0x{3:X8}{4})",
				this.ID,
				( this.Name != null ) ? " " + this.Name : "",
				this.Type,
				this.Address,
				( this.Type == BreakpointType.MemoryAccess ) ? " " + this.AccessType.ToString() : "" );

		}
	}
}
