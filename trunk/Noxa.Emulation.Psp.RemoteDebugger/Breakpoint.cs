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

namespace Noxa.Emulation.Psp.RemoteDebugger
{
	/// <summary>
	/// Describes the type of <see cref="Breakpoint"/>.
	/// </summary>
	enum BreakpointType
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
	}

	/// <summary>
	/// A breakpoint.
	/// </summary>
	class Breakpoint
	{
		/// <summary>
		/// The <see cref="BreakpointManager"/> instance that owns this breakpoint.
		/// </summary>
		public readonly BreakpointManager Manager;

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
		/// The BIOS function of the breakpoint.
		/// </summary>
		public readonly BiosFunction Function;

		/// <summary>
		/// The enabled state of the breakpoint.
		/// </summary>
		protected bool _enabled;

		/// <summary>
		/// The optional user-defined name of the breakpoint.
		/// </summary>
		public string Name;

		private static int _ids = 0;

		private Breakpoint( BreakpointManager manager, BreakpointType type )
		{
			this.Manager = manager;
			this.ID = Interlocked.Increment( ref _ids );
			this.Type = type;
			this.Name = null;
		}

		/// <summary>
		/// Initializes a new <see cref="Breakpoint"/> instance with the given parameters.
		/// </summary>
		/// <param name="manager">The manager that owns this instance.</param>
		/// <param name="type">The breakpoint type.</param>
		/// <param name="address">The address of the breakpoint.</param>
		public Breakpoint( BreakpointManager manager, BreakpointType type, uint address )
			: this( manager, type )
		{
			this.Address = address;
		}

		/// <summary>
		/// Initializes a new <see cref="Breakpoint"/> instance with the given parameters.
		/// </summary>
		/// <param name="manager">The manager that owns this instance.</param>
		/// <param name="address">The memory address to break at.</param>
		/// <param name="accessType">The access type that triggers the breakpoint.</param>
		public Breakpoint( BreakpointManager manager, uint address, MemoryAccessType accessType )
			: this( manager, BreakpointType.MemoryAccess )
		{
			this.AccessType = accessType;
			this.Address = address;
		}

		/// <summary>
		/// Initializes a new <see cref="Breakpoint"/> instance with the given parameters.
		/// </summary>
		/// <param name="manager">The manager that owns this instance.</param>
		/// <param name="function">The BIOS function to break on.</param>
		public Breakpoint( BreakpointManager manager, BiosFunction function )
			: this( manager, BreakpointType.BiosFunction )
		{
			this.Function = function;
		}

		/// <summary>
		/// The current enabled state of the breakpoint.
		/// </summary>
		public bool Enabled
		{
			get
			{
				return _enabled;
			}
			set
			{
				if( _enabled != value )
				{
					_enabled = value;
					this.Manager.OnBreakpointToggled( this );
				}
			}
		}
	}
}
