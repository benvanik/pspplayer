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

namespace Noxa.Emulation.Psp.Player.Debugger
{
	partial class DebuggerTool : WeifenLuo.WinFormsUI.Docking.DockContent
	{
		public readonly InprocDebugger Debugger;

		public DebuggerTool()
		{
			InitializeComponent();
		}

		public DebuggerTool( InprocDebugger debugger )
			: this()
		{
			this.Debugger = debugger;
		}

		public virtual void OnAttached()
		{
		}

		public virtual void OnStarted()
		{
		}
	}
}
