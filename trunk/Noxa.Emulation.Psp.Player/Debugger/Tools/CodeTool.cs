// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2008 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Noxa.Emulation.Psp.Debugging.DebugData;
using Noxa.Emulation.Psp.Debugging.DebugModel;
using Noxa.Emulation.Psp.Player.Debugger.Model;

namespace Noxa.Emulation.Psp.Player.Debugger.Tools
{
	partial class CodeTool : Noxa.Emulation.Psp.Player.Debugger.DebuggerTool
	{
		public CodeTool()
		{
			this.InitializeComponent();
		}

		public CodeTool( InprocDebugger debugger )
			: base( debugger )
		{
			this.InitializeComponent();

			Bitmap image = Properties.Resources.DisassemblyIcon as Bitmap;
			this.Icon = Icon.FromHandle( image.GetHicon() );

			this.Disable();

			this.codeView.Setup( debugger );
			this.registersControl.Setup( debugger );

			// TODO: bind to button
			this.codeView.UseHex = true;
		}

		public void InvalidateAll()
		{
			this.codeView.InvalidateAll();
			this.registersControl.Invalidate();
		}

		public void Disable()
		{
			this.codeView.Enabled = false;
			this.registersControl.Enabled = false;
			this.registersControl.Invalidate();
		}

		public void SetAddress( uint address, bool isCurrentStatement )
		{
			this.codeView.Enabled = true;
			this.codeView.NavigateToAddress( address );
			this.codeView.Focus();
			this.registersControl.Enabled = true;
			this.registersControl.Invalidate();
		}

		public void ShowNextStatement()
		{
			this.SetAddress( this.Debugger.PC, true );
		}
	}
}
