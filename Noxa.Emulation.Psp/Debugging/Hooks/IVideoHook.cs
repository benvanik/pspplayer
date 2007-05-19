// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace Noxa.Emulation.Psp.Debugging.Hooks
{
	/// <summary>
	/// Hook inside the video driver to allow the debugger to extract information.
	/// </summary>
	public interface IVideoHook : IHook
	{
	}
}
