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
	public abstract class Breakpoint
	{
		protected bool _enabled;
		protected int _address;

		public Breakpoint( int address )
		{
			_address = address;
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
