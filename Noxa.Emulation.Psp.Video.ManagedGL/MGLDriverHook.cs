// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2008 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Noxa.Emulation.Psp.Debugging.Hooks;

namespace Noxa.Emulation.Psp.Video.ManagedGL
{
	class MGLDriverHook : IVideoHook
	{
		public readonly MGLDriver Driver;

		public MGLDriverHook( MGLDriver driver )
		{
			this.Driver = driver;
		}
	}
}
