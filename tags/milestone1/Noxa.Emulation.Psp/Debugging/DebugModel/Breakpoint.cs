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
	/// Describes the type of <see cref="Breakpoint"/>.
	/// </summary>
	public enum BreakpointType
	{
		/// <summary>
		/// Defined by the user.
		/// </summary>
		UserSet,

		/// <summary>
		/// Set by the debugger for stepping.
		/// </summary>
		Stepping,
	}

	/// <summary>
	/// A breakpoint.
	/// </summary>
	public abstract class Breakpoint
	{
		/// <summary>
		/// The type of the breakpoint.
		/// </summary>
		public readonly BreakpointType Type;

		/// <summary>
		/// The address of the breakpoint.
		/// </summary>
		public readonly int Address;

		/// <summary>
		/// The enabled state of the breakpoint.
		/// </summary>
		protected bool _enabled;

		/// <summary>
		/// The optional user-defined name of the breakpoint.
		/// </summary>
		protected string _name;

		/// <summary>
		/// Initializes a new user-set <see cref="Breakpoint"/> instance with the given parameters.
		/// </summary>
		/// <param name="address">The address of the breakpoint.</param>
		public Breakpoint( int address )
			: this( BreakpointType.UserSet, address )
		{
		}

		/// <summary>
		/// Initializes a new <see cref="Breakpoint"/> instance with the given parameters.
		/// </summary>
		/// <param name="type">The breakpoint type.</param>
		/// <param name="address">The address of the breakpoint.</param>
		public Breakpoint( BreakpointType type, int address )
		{
			this.Type = type;
			this.Address = address;

			_enabled = true;
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
					this.OnEnabledChanged();
				}
			}
		}

		/// <summary>
		/// The optional user-defined name for the breakpoint.
		/// </summary>
		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				bool changed = ( _name != value );
				_name = value;
				if( changed == true )
					this.OnNameChanged();
			}
		}

		/// <summary>
		/// Called when the enabled state of the breakpoint changes.
		/// </summary>
		protected virtual void OnEnabledChanged()
		{
		}

		/// <summary>
		/// Called when the name of the breakpoint changes.
		/// </summary>
		protected virtual void OnNameChanged()
		{
		}
	}
}
