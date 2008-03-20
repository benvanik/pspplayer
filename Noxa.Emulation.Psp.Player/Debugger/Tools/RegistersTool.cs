// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2008 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Noxa.Emulation.Psp.Player.Debugger.Tools
{
	partial class RegistersTool : Noxa.Emulation.Psp.Player.Debugger.DebuggerTool
	{
		public RegistersTool()
		{
			this.InitializeComponent();
		}

		public RegistersTool( InprocDebugger debugger )
			: base( debugger )
		{
			this.InitializeComponent();

			Bitmap image = Properties.Resources.RegistersIcon as Bitmap;
			this.Icon = Icon.FromHandle( image.GetHicon() );
		}
	}
}
