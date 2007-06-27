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

using Noxa.Emulation.Psp.Debugging.DebugModel;

namespace Noxa.Emulation.Psp.RemoteDebugger.Tools
{
	internal partial class CallstackViewer : Noxa.Emulation.Psp.RemoteDebugger.Tools.ToolPane
	{
		public CallstackViewer()
		{
			InitializeComponent();
		}

		public void Clear()
		{
			this.callView.BeginUpdate();
			this.callView.Items.Clear();
			this.callView.EndUpdate();
			this.callView.Enabled = false;
		}

		public void SetCallstack( Frame[] callStack )
		{
			this.callView.BeginUpdate();
			this.callView.Items.Clear();
			for( int n = callStack.Length - 1; n >= 0; n-- )
			{
				Frame frame = callStack[ n ];
				string pretty;
				bool grey = false;
				switch( frame.Type )
				{
					case FrameType.BiosBarrier:
						pretty = "[BIOS Barrier]";
						grey = true;
						break;
					case FrameType.CallMarshal:
						pretty = "[Call Marshal]";
						grey = true;
						break;
					case FrameType.Interrupt:
						pretty = "[Interrupt]";
						grey = true;
						break;
					default:
					case FrameType.UserCode:
						// Try to resolve the name
						pretty = "(unknown)";
						break;
				}
				ListViewItem item = new ListViewItem( new string[]{
					"", pretty, string.Format( "0x{0:X8}", frame.Address ) } );
				item.Tag = frame;
				item.UseItemStyleForSubItems = true;
				if( grey == true )
					item.ForeColor = SystemColors.InactiveCaptionText;
				this.callView.Items.Add( item );
			}
			this.callView.EndUpdate();
			this.callView.Enabled = true;
		}
	}
}

