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
using System.Diagnostics;

namespace Noxa.Emulation.Psp.Player
{
	partial class IssueReport : Form
	{
		private Host _host;
		private List<ComponentIssue> _issues;

		public IssueReport()
		{
			InitializeComponent();
		}

		public IssueReport( Host host, List<ComponentIssue> issues )
			: this()
		{
			Debug.Assert( host != null );
			Debug.Assert( issues != null );
			Debug.Assert( issues.Count > 0 );

			_host = host;
			_issues = issues;

			imageList.Images.Add( "error", Properties.Resources.ErrorIcon );
			imageList.Images.Add( "warning", Properties.Resources.WarningIcon );

			dontShowWarningsCheckBox.Checked = !Properties.Settings.Default.ShowReportOnWarnings;

			int errorCount = 0;
			int warningCount = 0;
			foreach( ComponentIssue issue in issues )
			{
				if( issue.Level == IssueLevel.Error )
					errorCount++;
				else if( issue.Level == IssueLevel.Warning )
					warningCount++;
			}

			if( errorCount == 0 )
			{
				continuePictureBox.Image = Properties.Resources.WarningIcon;
				continueLabel.Text = "No errors found, but the emulator might not work correctly until the issues are fixed.";
				this.continueButton.Enabled = true;
			}
			else
			{
				continuePictureBox.Image = Properties.Resources.ErrorIcon;
				continueLabel.Text = "Errors found, you will not be able to continue until they have been fixed.";
				this.continueButton.Enabled = false;
			}

			foreach( ComponentIssue issue in issues )
			{
				ListViewItem item = new ListViewItem();
				if( issue.Level == IssueLevel.Error )
				{
					item.Group = listView.Groups[ "errorsGroup" ];
					item.ImageKey = "error";
				}
				else
				{
					item.Group = listView.Groups[ "warningsGroup" ];
					item.ImageKey = "warning";
				}
				item.UseItemStyleForSubItems = false;
				item.Text = issue.Message;
				if( issue.SupportUrl != null )
				{
					ListViewItem.ListViewSubItem subItem = new ListViewItem.ListViewSubItem();
					subItem.Text = "View Link";
					subItem.Font = new Font( listView.Font, FontStyle.Underline );
					subItem.ForeColor = SystemColors.HotTrack;
					item.SubItems.Add( subItem );
				}
				item.Tag = issue;
				listView.Items.Add( item );
			}
		}

		private void listView_ItemActivate( object sender, EventArgs e )
		{
			ListViewItem item = listView.FocusedItem;
			if( item == null )
				return;

			ComponentIssue issue = item.Tag as ComponentIssue;
			Debug.Assert( issue != null );

			listView.SelectedItems.Clear();

			if( issue.SupportUrl == null )
				return;

			Process.Start( issue.SupportUrl );
		}

		private void dontShowWarningsCheckBox_CheckedChanged( object sender, EventArgs e )
		{
			Properties.Settings.Default.ShowReportOnWarnings = !dontShowWarningsCheckBox.Checked;
			Properties.Settings.Default.Save();
		}
	}
}