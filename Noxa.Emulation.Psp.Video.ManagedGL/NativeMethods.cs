// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2008 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Noxa.Emulation.Psp.Video.ManagedGL
{
	static class NativeMethods
	{
		[DllImport( "user32.dll" )]
		public static extern short GetAsyncKeyState( Keys vKey );
	}
}
