// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace Noxa.Emulation.Psp.Debugging
{
	public enum BreakpointType
	{
		UserSet,
		Stepping,
	}

	public abstract class Breakpoint
	{
		protected BreakpointType _type;
		protected bool _enabled;
		protected int _address;

		public Breakpoint( int address )
			: this( BreakpointType.UserSet, address )
		{
		}

		public Breakpoint( BreakpointType type, int address )
		{
			_type = type;
			_enabled = true;
			_address = address;
		}

		public BreakpointType Type
		{
			get
			{
				return _type;
			}
		}

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

		public int Address
		{
			get
			{
				return _address;
			}
		}

		protected virtual void OnEnabledChanged()
		{
		}
	}
}
