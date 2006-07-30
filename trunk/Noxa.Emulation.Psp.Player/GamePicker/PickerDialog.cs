using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Noxa.Emulation.Psp.Games;
using System.Diagnostics;

namespace Noxa.Emulation.Psp.Player.GamePicker
{
	partial class PickerDialog : Form
	{
		protected Instance _emulator;
		protected GameEntry _selectedEntry;

		public PickerDialog()
		{
			InitializeComponent();
		}

		public PickerDialog( Instance emulator )
			: this()
		{
			Debug.Assert( emulator != null );

			_emulator = emulator;

			this.FindGames();
		}

		public void LaunchGame( GameInformation game )
		{
			_emulator.SwitchToGame( game );
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		public void FindGames()
		{
			GameLoader loader = new GameLoader();
			GameInformation[] games = loader.FindGames( _emulator );

			this.flowLayoutPanel1.SuspendLayout();
			this.flowLayoutPanel1.Controls.Clear();

			foreach( GameInformation game in games )
			{
				GameEntry entry = new GameEntry( this, game );
				entry.Click += new EventHandler( EntryClick );
				entry.DoubleClick += new EventHandler( EntryDoubleClick );
				entry.Width = this.flowLayoutPanel1.ClientSize.Width - 25;
				this.flowLayoutPanel1.Controls.Add( entry );
			}

			this.flowLayoutPanel1.ResumeLayout( true );
		}

		private void EntryClick( object sender, EventArgs e )
		{
			if( _selectedEntry != null )
				_selectedEntry.IsSelected = false;
			_selectedEntry = sender as GameEntry;
			_selectedEntry.IsSelected = true;
		}

		private void EntryDoubleClick( object sender, EventArgs e )
		{
			GameEntry entry = sender as GameEntry;
			this.LaunchGame( entry.Game );
		}

		private void browseButton_Click( object sender, EventArgs e )
		{
		}

		private void removeButton_Click( object sender, EventArgs e )
		{
		}

		private void clearButton_Click( object sender, EventArgs e )
		{
		}

		private void playButton_Click( object sender, EventArgs e )
		{
			if( _selectedEntry == null )
				return;

			this.LaunchGame( _selectedEntry.Game );
		}

		private void cancelButton_Click( object sender, EventArgs e )
		{
		}
	}
}