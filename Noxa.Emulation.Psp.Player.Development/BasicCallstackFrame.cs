// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

using Noxa.Emulation.Psp.Debugging;

namespace Noxa.Emulation.Psp.Player.Development
{
	class BasicCallstackFrame : CallstackFrame
	{
		public BasicCallstackFrame( int address, string name )
			: base( address, name )
		{
		}
	}
}
