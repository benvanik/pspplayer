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
using Noxa.Emulation.Psp.Debugging.DebugModel;
using Noxa.Emulation.Psp.Player.Debugger.Model;

namespace Noxa.Emulation.Psp.Player.Debugger.Tools
{
	partial class CallstackTool : Noxa.Emulation.Psp.Player.Debugger.DebuggerTool
	{
		public CallstackTool()
		{
			this.InitializeComponent();
		}

		public CallstackTool( InprocDebugger debugger )
			: base( debugger )
		{
			this.InitializeComponent();

			Bitmap image = Properties.Resources.CallstackIcon as Bitmap;
			this.Icon = Icon.FromHandle( image.GetHicon() );
		}

		public void Clear()
		{
			this.callView.BeginUpdate();
			this.callView.Items.Clear();
			this.callView.EndUpdate();
			this.callView.Enabled = false;
		}

		public void RefreshCallstack()
		{
			Frame[] callStack = this.Debugger.DebugHost.CpuHook.GetCallstack();
			if( ( callStack == null ) || ( callStack.Length == 0 ) )
			{
				this.Clear();
				return;
			}

			this.callView.BeginUpdate();
			this.callView.Items.Clear();
			foreach( Frame frame in callStack )
			{
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
						{
							MethodBody body = this.Debugger.CodeCache[ ( uint )frame.Address ];
							if( body == null )
								pretty = "(unknown)";
							else
								pretty = string.Format( "{0}+0x{1:X}", body.Name, frame.Address - body.Address );
						}
						break;
				}
				ListViewItem item = new ListViewItem( new string[]{
					"", pretty,
					grey ? "" : string.Format( "0x{0:X8}", frame.Address ) } );
				item.Tag = frame;
				item.UseItemStyleForSubItems = true;
				if( grey == true )
					item.ForeColor = SystemColors.InactiveCaptionText;
				this.callView.Items.Add( item );
			}
			this.callView.EndUpdate();
			this.callView.Enabled = true;
		}

		private void callView_MouseDown( object sender, MouseEventArgs e )
		{
			if( e.Clicks == 2 )
			{
				ListViewItem item = this.callView.GetItemAt( e.X, e.Y );
				Frame frame = item.Tag as Frame;
				if( frame.Type == FrameType.UserCode )
					this.Debugger.CodeTool.SetAddress( ( uint )frame.Address, false );
			}
		}
	}
}
