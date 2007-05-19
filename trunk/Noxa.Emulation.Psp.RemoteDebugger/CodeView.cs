// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using WeifenLuo.WinFormsUI.Docking;

namespace Noxa.Emulation.Psp.RemoteDebugger
{
	partial class CodeView : DockContent
	{
		public EmuDebugger Debugger;

		public CodeView()
		{
			InitializeComponent();
		}

		public CodeView( EmuDebugger debugger )
			: this()
		{
			this.Debugger = debugger;
	}
	}
}

