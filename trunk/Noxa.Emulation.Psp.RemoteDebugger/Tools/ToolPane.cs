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

namespace Noxa.Emulation.Psp.RemoteDebugger.Tools
{
	partial class ToolPane : DockContent
	{
		public ToolPane()
		{
			InitializeComponent();
		}

		public virtual void OnStarted()
		{
		}

		public virtual void OnStopped()
		{
		}
	}
}

