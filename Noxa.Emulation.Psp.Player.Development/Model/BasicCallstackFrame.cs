// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

using Noxa.Emulation.Psp.Debugging;
using Noxa.Emulation.Psp.Debugging.DebugModel;

namespace Noxa.Emulation.Psp.Player.Development.Model
{
	class BasicCallstackFrame : Frame
	{
		public BasicCallstackFrame( int address, string name )
			: base( address, name )
		{
		}
	}
}
