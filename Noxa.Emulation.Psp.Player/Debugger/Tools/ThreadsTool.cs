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
using Noxa.Emulation.Psp.Player.Debugger.Dialogs;

namespace Noxa.Emulation.Psp.Player.Debugger.Tools
{
	partial class ThreadsTool : Noxa.Emulation.Psp.Player.Debugger.DebuggerTool
	{
		private DelayThreadDialog _delayThreadDialog;

		public ThreadsTool()
		{
			this.InitializeComponent();
		}

		public ThreadsTool( InprocDebugger debugger )
			: base( debugger )
		{
			this.InitializeComponent();

			Bitmap image = Properties.Resources.ThreadsIcon as Bitmap;
			this.Icon = Icon.FromHandle( image.GetHicon() );

			this.Clear();

			_delayThreadDialog = new DelayThreadDialog();
		}

		public void Clear()
		{
			this.threadListView.BeginUpdate();
			this.threadListView.Items.Clear();
			this.threadListView.EndUpdate();
			this.threadListView.Enabled = false;
		}

		public void RefreshThreads()
		{
			ThreadInfo[] threadInfos = this.Debugger.DebugHost.BiosHook.GetThreads();
			uint activeThreadId = this.Debugger.DebugHost.BiosHook.ActiveThreadID;

			this.threadListView.BeginUpdate();
			this.threadListView.Items.Clear();
			foreach( ThreadInfo threadInfo in threadInfos )
			{
				string ids = threadInfo.ThreadID.ToString( "X" ) + "/" + threadInfo.InternalThreadID;
				string pc = threadInfo.CurrentPC.ToString( "X8" );
				string state = threadInfo.State.ToString();
				if( threadInfo.IsWaiting == true )
					state += " - " + threadInfo.WaitingDescription;
				ListViewItem item = new ListViewItem( new string[]{
					ids, pc, threadInfo.Name, threadInfo.Priority.ToString(), state
				} );
				item.Tag = threadInfo;
				item.UseItemStyleForSubItems = true;
				if( threadInfo.ThreadID == activeThreadId )
					item.ForeColor = Color.Green;
				else if( threadInfo.IsWaiting == true )
					item.ForeColor = SystemColors.InactiveCaptionText;
				this.threadListView.Items.Add( item );
			}
			this.threadListView.EndUpdate();
			this.threadListView.Enabled = true;
		}

		private ListViewItem _currentItem;

		private void threadListView_MouseDown( object sender, MouseEventArgs e )
		{
			ListViewItem item = this.threadListView.GetItemAt( e.X, e.Y );
			_currentItem = item;
			if( item == null )
				return;
			ThreadInfo threadInfo = item.Tag as ThreadInfo;
			if( threadInfo == null )
				return;
			uint activeThreadId = this.Debugger.DebugHost.BiosHook.ActiveThreadID;
			if( e.Clicks == 2 )
			{
				this.Debugger.CodeTool.SetAddress( threadInfo.CurrentPC, ( activeThreadId == threadInfo.ThreadID ) );
			}
			else
			{
				if( e.Button == MouseButtons.Right )
				{
					bool canControl = ( threadInfo.ThreadID != activeThreadId );
					this.wakeToolStripMenuItem.Enabled = canControl;
					this.delayToolStripMenuItem.Enabled = canControl;
					this.killToolStripMenuItem.Enabled = canControl;
					this.threadContextMenuStrip.Show( this.threadListView, e.Location );
				}
			}
		}

		private void jumpToCurrentPCToolStripMenuItem_Click( object sender, EventArgs e )
		{
			if( _currentItem == null )
				return;
			ThreadInfo threadInfo = _currentItem.Tag as ThreadInfo;
			if( threadInfo == null )
				return;
			uint activeThreadId = this.Debugger.DebugHost.BiosHook.ActiveThreadID;
			this.Debugger.CodeTool.SetAddress( threadInfo.CurrentPC, ( activeThreadId == threadInfo.ThreadID ) );
		}

		private void wakeToolStripMenuItem_Click( object sender, EventArgs e )
		{
			if( _currentItem == null )
				return;
			ThreadInfo threadInfo = _currentItem.Tag as ThreadInfo;
			if( threadInfo == null )
				return;
			this.Debugger.DebugHost.BiosHook.WakeThread( threadInfo.ThreadID );
			this.RefreshThreads();
		}

		private void delayToolStripMenuItem_Click( object sender, EventArgs e )
		{
			if( _currentItem == null )
				return;
			ThreadInfo threadInfo = _currentItem.Tag as ThreadInfo;
			if( threadInfo == null )
				return;
			if( _delayThreadDialog.ShowDialog( this.FindForm() ) == DialogResult.OK )
			{
				this.Debugger.DebugHost.BiosHook.DelayThread( threadInfo.ThreadID, _delayThreadDialog.Time );
				this.RefreshThreads();
			}
		}

		private void killToolStripMenuItem_Click( object sender, EventArgs e )
		{
			if( _currentItem == null )
				return;
			ThreadInfo threadInfo = _currentItem.Tag as ThreadInfo;
			if( threadInfo == null )
				return;
			this.Debugger.DebugHost.BiosHook.KillThread( threadInfo.ThreadID );
			this.RefreshThreads();
		}
	}
}
