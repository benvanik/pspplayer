// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

using Noxa.Emulation.Psp.Games;
using Noxa.Emulation.Psp.Media;

namespace Noxa.Emulation.Psp.Player.GamePicker
{
	partial class PickerDialog : Form
	{
		protected Instance _emulator;
		
		public PickerDialog()
		{
			InitializeComponent();
		}

		public PickerDialog( Instance emulator )
			: this()
		{
			Debug.Assert( emulator != null );

			_emulator = emulator;

			SetEnabledState( true );

			this.FindGames();

			recentGamesListing_SelectionChanged( recentGamesListing, EventArgs.Empty );

			this.DialogResult = DialogResult.Cancel;
		}

		protected override void OnLoad( EventArgs e )
		{
			base.OnLoad( e );

			whidbeyTabControl1.SelectedIndex = 0;
		}

		public void LaunchGame( GameInformation game )
		{
			if( game.Tag != null )
			{
				// Need to have the UMD instance reload to the game
				string gamePath = game.Tag as string;
				Debug.Assert( gamePath != null );

				_emulator.Umd.Eject();
				if( _emulator.Umd.Load( gamePath ) == false )
				{
					// Failed to load
				}

				// Regrab game info
				GameLoader loader = new GameLoader();
				game = loader.FindGame( _emulator.Umd );

				Properties.Settings.Default.LastPlayedGame = gamePath;
				Properties.Settings.Default.Save();
			}

			_emulator.SwitchToGame( game );
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		public void FindGames()
		{
			// Load the memory stick listing
			GameLoader loader = new GameLoader();
			List<GameInformation> eboots = new List<GameInformation>();
			GameInformation[] ebootList = loader.FindGames( _emulator );
			foreach( GameInformation game in ebootList )
			{
				if( game.GameType == GameType.Eboot )
					eboots.Add( game );
			}
			memoryStickListing.AddGames( eboots );

			// Load recent UMDs
			if( Properties.Settings.Default.RecentGames != null )
			{
				string lastPlayed = Properties.Settings.Default.LastPlayedGame;
				foreach( string gamePath in Properties.Settings.Default.RecentGames )
				{
					GameInformation game = this.LoadGameFromUmd( gamePath );
					if( game != null )
					{
						GameEntry entry = recentGamesListing.AddGame( game );
						if( lastPlayed == ( string )game.Tag )
							recentGamesListing.SelectedEntry = entry;
					}
				}
			}
		}

		private GameInformation LoadGameFromUmd( string gamePath )
		{
			try
			{
				if( File.Exists( gamePath ) == false )
					return null;

				Type deviceType = _emulator.Umd.Factory;
				IComponent component = ( IComponent )Activator.CreateInstance( deviceType );
				Debug.Assert( component != null );
				if( component == null )
					throw new InvalidOperationException();

				ComponentParameters parameters = new ComponentParameters();
				parameters[ "path" ] = gamePath;
				IUmdDevice umdDevice = component.CreateInstance( _emulator, parameters ) as IUmdDevice;
				Debug.Assert( umdDevice != null );
				if( umdDevice == null )
					throw new InvalidOperationException();

				GameLoader loader = new GameLoader();
				GameInformation game = loader.FindGame( umdDevice );
				if( game != null )
				{
					game.Tag = gamePath;
				}

				umdDevice.Cleanup();

				return game;
			}
			catch
			{
				Debug.Assert( false );
				return null;
			}
		}

		private void SortRecentGames()
		{
			if( Properties.Settings.Default.RecentGames == null )
				return;

			List<GameInformation> games = new List<GameInformation>();
			foreach( string gamePath in Properties.Settings.Default.RecentGames )
			{
				GameInformation game = this.LoadGameFromUmd( gamePath );
				if( game != null )
					games.Add( game );
			}
			Comparison<GameInformation> del = delegate( GameInformation x, GameInformation y )
			{
				return string.Compare( x.Parameters.Title, y.Parameters.Title, true );
			};
			games.Sort( del );
			Properties.Settings.Default.RecentGames.Clear();
			foreach( GameInformation game in games )
				Properties.Settings.Default.RecentGames.Add( game.Tag as string );
		}

		private void browseButton_Click( object sender, EventArgs e )
		{
			if( openFileDialog.ShowDialog( this ) == DialogResult.OK )
			{
				string gamePath = openFileDialog.FileName;

				GameInformation game = this.LoadGameFromUmd( gamePath );
				if( game != null )
				{
					if( Properties.Settings.Default.RecentGames == null )
						Properties.Settings.Default.RecentGames = new System.Collections.Specialized.StringCollection();
					bool exists = Properties.Settings.Default.RecentGames.Contains( gamePath );
					if( exists == false )
						Properties.Settings.Default.RecentGames.Add( gamePath );
					this.SortRecentGames();
					Properties.Settings.Default.Save();

					if( exists == false )
						recentGamesListing.AddGame( game );

					recentGamesListing_SelectionChanged( recentGamesListing, EventArgs.Empty );
				}
			}
		}

		private void removeButton_Click( object sender, EventArgs e )
		{
			GameListing listing;
			if( whidbeyTabControl1.SelectedTab == whidbeyTabPage1 )
				listing = recentGamesListing;
			else
				listing = memoryStickListing;
			GameEntry entry = listing.SelectedEntry;
			if( ( entry == null ) ||
				( entry.Game.Tag == null ) )
				return;

			string gamePath = entry.Game.Tag as string;
			if( Properties.Settings.Default.RecentGames != null )
			{
				Properties.Settings.Default.RecentGames.Remove( gamePath );
				Properties.Settings.Default.Save();
			}

			listing.RemoveGame( entry );

			recentGamesListing_SelectionChanged( recentGamesListing, EventArgs.Empty );
		}

		private void clearButton_Click( object sender, EventArgs e )
		{
			Properties.Settings.Default.RecentGames = new System.Collections.Specialized.StringCollection();
			Properties.Settings.Default.Save();

			GameListing listing;
			if( whidbeyTabControl1.SelectedTab == whidbeyTabPage1 )
				listing = recentGamesListing;
			else
				listing = memoryStickListing;
			listing.ClearGames();

			recentGamesListing_SelectionChanged( recentGamesListing, EventArgs.Empty );
		}

		private void playButton_Click( object sender, EventArgs e )
		{
			GameListing listing;
			if( whidbeyTabControl1.SelectedTab == whidbeyTabPage1 )
				listing = recentGamesListing;
			else
				listing = memoryStickListing;
			GameEntry entry = listing.SelectedEntry;
			if( entry == null )
				return;

			this.LaunchGame( entry.Game );
		}

		private void cancelButton_Click( object sender, EventArgs e )
		{
		}

		private void SetEnabledState( bool browseEnabled )
		{
			browseButton.Enabled = browseEnabled;
			removeButton.Enabled = browseEnabled;
			clearButton.Enabled = browseEnabled;
		}

		private void whidbeyTabPage1_Showing( object sender, EventArgs e )
		{
			SetEnabledState( true );
			recentGamesListing.Focus();
			recentGamesListing.SelectedEntry = recentGamesListing.SelectedEntry;
			recentGamesListing_SelectionChanged( recentGamesListing, EventArgs.Empty );
		}

		private void whidbeyTabPage2_Showing( object sender, EventArgs e )
		{
			SetEnabledState( false );
			memoryStickListing.Focus();
			memoryStickListing.SelectedEntry = memoryStickListing.SelectedEntry;
			memoryStickListing_SelectionChanged( memoryStickListing, EventArgs.Empty );
		}

		private void recentGamesListing_SelectionChanged( object sender, EventArgs e )
		{
			removeButton.Enabled = ( recentGamesListing.SelectedEntry != null );
			clearButton.Enabled = recentGamesListing.HasGames;
			playButton.Enabled = ( recentGamesListing.SelectedEntry != null );
		}

		private void memoryStickListing_SelectionChanged( object sender, EventArgs e )
		{
			playButton.Enabled = ( memoryStickListing.SelectedEntry != null );
		}
	}
}