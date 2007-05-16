// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using Noxa.Emulation.Psp.Games;

namespace Noxa.Emulation.Psp.Player.GamePicker
{
	partial class GameListing : UserControl
	{
		protected GameEntry _selectedEntry;
		private VScrollBar _dummyScroller;

		public GameListing()
		{
			InitializeComponent();

			this.HScroll = false;
			this.VScroll = true;

			_dummyScroller = new VScrollBar();
		}

		public event EventHandler SelectionChanged;

		public GameEntry SelectedEntry
		{
			get
			{
				return _selectedEntry;
			}
			set
			{
				if( value == null )
				{
					this.Focus();
					_selectedEntry = null;
				}
				else
				{
					if( this.Controls.Contains( value ) == false )
					{
						this.Focus();
						_selectedEntry = value;
						return;
					}

					_selectedEntry = value;
					_selectedEntry.Focus();
					_selectedEntry.Select();
					_selectedEntry.IsSelected = true;
				}
			}
		}

		public bool HasGames
		{
			get
			{
				return this.Controls.Count > 0;
			}
		}

		public ControlCollection GameEntries
		{
			get
			{
				return flowLayoutPanel1.Controls;
			}
		}

		public void AddGames( List<GameInformation> games )
		{
			Debug.Assert( games != null );

			this.SuspendLayout();
			this.AutoScrollPosition = new Point( 0, 0 );

			int index = 0;
			foreach( GameInformation game in games )
			{
				GameEntry entry = new GameEntry( game );
				entry.TabIndex = index++;
				entry.Click += new EventHandler( EntryClick );
				entry.DoubleClick += new EventHandler( EntryDoubleClick );
				entry.Width = this.ClientSize.Width - _dummyScroller.Width - 5;
				flowLayoutPanel1.Controls.Add( entry );
			}

			this.ResumeLayout( true );
		}

		public GameEntry AddGame( GameInformation game )
		{
			Debug.Assert( game != null );

			this.SuspendLayout();

			int index = this.Controls.Count;

			GameEntry entry = new GameEntry( game );
			entry.TabIndex = index;
			entry.Click += new EventHandler( EntryClick );
			entry.DoubleClick += new EventHandler( EntryDoubleClick );
			entry.Width = this.ClientSize.Width - _dummyScroller.Width - 5;
			flowLayoutPanel1.Controls.Add( entry );

			this.LayoutGames();

			this.ResumeLayout( true );

			return entry;
		}

		public void RemoveGame( GameEntry entry )
		{
			this.SuspendLayout();

			flowLayoutPanel1.Controls.Remove( entry );
			this.LayoutGames();

			if( _selectedEntry == entry )
				this.SelectedEntry = null;

			this.ResumeLayout( true );
		}

		public void ClearGames()
		{
			this.SuspendLayout();
			this.AutoScrollPosition = new Point( 0, 0 );
			foreach( Control control in flowLayoutPanel1.Controls )
			{
				if( control is GameEntry )
					( ( GameEntry )control ).Game.Dispose();
			}
			flowLayoutPanel1.Controls.Clear();
			this.SelectedEntry = null;
			this.ResumeLayout( true );
		}

		public void LayoutGames()
		{
			this.SuspendLayout();

			this.AutoScrollPosition = new Point( 0, 0 );

			List<GameEntry> entries = new List<GameEntry>();
			foreach( GameEntry entry in this.Controls )
				entries.Add( entry );
			flowLayoutPanel1.Controls.Clear();
			entries.Sort( delegate( GameEntry x, GameEntry y )
			{
				return string.Compare( x.Game.Parameters.Title, y.Game.Parameters.Title, true );
			} );

			int index = 0;
			foreach( GameEntry entry in entries )
			{
				entry.TabIndex = index++;
				this.Controls.Add( entry );
			}

			if( _selectedEntry != null )
				flowLayoutPanel1.ScrollControlIntoView( _selectedEntry );

			this.ResumeLayout( true );
		}

		private void EntryClick( object sender, EventArgs e )
		{
			if( _selectedEntry != null )
				_selectedEntry.IsSelected = false;
			_selectedEntry = sender as GameEntry;
			_selectedEntry.IsSelected = true;
			_selectedEntry.Parent.Focus();
			this.ActiveControl = _selectedEntry;

			EventHandler handler = this.SelectionChanged;
			if( handler != null )
				handler( this, EventArgs.Empty );
		}

		private void EntryDoubleClick( object sender, EventArgs e )
		{
			PickerDialog dialog = this.FindForm() as PickerDialog;
			GameEntry entry = sender as GameEntry;
			dialog.LaunchGame( entry.Game );
		}

		protected override void OnGotFocus( EventArgs e )
		{
			this.ProcessTabKey( true );
			base.OnGotFocus( e );
			//if( this.Controls.Count > 0 )
			//{
			//    this.Controls[ 0 ].Focus();
			//    this.Controls[ 0 ].Select();
			//    this.ActiveControl = this.Controls[ 0 ];
			//}
		}
	}
}
