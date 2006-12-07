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
	public abstract class CallstackFrame
	{
		protected int _address;
		protected string _name;

		public CallstackFrame( int address, string name )
		{
			_address = address;
			_name = name;
		}

		public int Address
		{
			get
			{
				return _address;
			}
		}

		public string Name
		{
			get
			{
				return _name;
			}
		}
	}
}
