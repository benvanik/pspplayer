using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using Noxa.Utilities.Controls;

using Noxa.Emulation.Psp.Games;
using Noxa.Emulation.Psp.Player.CompatibilityService;
using Noxa.Emulation.Psp.Player.Properties;

namespace Noxa.Emulation.Psp.Player.ServiceTools
{
	partial class SubmitGame : Form
	{
		private const string TestUsername = "USERNAME HERE";
		private const string TestPassword = "PASSWORD HERE";

		private Service _service;

		private static Game[] _gameList;
		private bool _outstandingRefresh;
		private Image _googleIcon;

		private GameInformation _game;
		private GameRelease _release;
		private byte[] _iconBytes;

		public SubmitGame()
		{
			InitializeComponent();
		}

		public SubmitGame( GameInformation game )
			: this()
		{
			Debug.Assert( game != null );
			if( game == null )
				throw new ArgumentNullException( "game" );
			_game = game;

			this.Icon = IconUtilities.ConvertToIcon( Properties.Resources.ReportIcon );
			_googleIcon = Properties.Resources.Google;

			_service = new Service();
			_service.ListGamesCompleted += new ListGamesCompletedEventHandler( ServiceListGamesCompleted );
			_service.AddGameCompleted += new AddGameCompletedEventHandler( ServiceAddGameCompleted );
			_service.AddReleaseCompleted += new AddReleaseCompletedEventHandler( ServiceAddReleaseCompleted );

			if( _gameList == null )
			{
				_service.ListGamesAsync();
				_outstandingRefresh = true;
			}
			else
			{
				this.ServiceListGamesCompleted( null, null );
			}

			this.titleLabel.Text = _game.Parameters.Title;
			this.discIdLabel.Text = _game.Parameters.DiscID;
			this.firmwareLabel.Text = _game.Parameters.SystemVersion.ToString();
			this.regionLabel.Text = _game.Parameters.Region.ToString();
			this.versionLabel.Text = _game.Parameters.GameVersion.ToString();

			Image gameImage;
			if( _game.Icon != null )
			{
				_game.Icon.Position = 0;
				gameImage = Image.FromStream( _game.Icon );
			}
			else
				gameImage = Image.FromStream( new MemoryStream( Resources.InvalidIcon, false ) );
			this.iconPictureBox.Image = gameImage;

			_release = new GameRelease();
			_release.Title = _game.Parameters.Title;
			_release.DiscID = _game.Parameters.DiscID;
			_release.Region = _game.Parameters.Region;
			_release.SystemVersion = VersionToSingle( _game.Parameters.SystemVersion );
			_release.GameVersion = VersionToSingle( _game.Parameters.GameVersion );
			if( _game.Icon != null )
			{
				_game.Icon.Position = 0;
				using( BinaryReader reader = new BinaryReader( _game.Icon ) )
					_iconBytes = reader.ReadBytes( ( int )_game.Icon.Length );
			}
		}

		private static float VersionToSingle( System.Version version )
		{
			// Nasty...
			string temp = string.Format( "{0}.{1}", version.Major, version.Minor );
			return float.Parse( temp, CultureInfo.InvariantCulture );
		}

		private void ServiceListGamesCompleted( object sender, ListGamesCompletedEventArgs e )
		{
			if( e != null )
			{
				if( ( e.Error != null ) ||
					( e.Result == null ) )
				{
					MessageBox.Show( this, "Unable to contact the service. Please try again later.", "PSP Player" );
					return;
				}
				_gameList = e.Result;
			}
			Game[] games = _gameList;

			this.titleView.Rows.Clear();

			foreach( Game game in games )
			{
				DataGridViewRow row = new DataGridViewRow();
				row.Tag = game;

				{
					DataGridViewLinkCell cell = new DataGridViewLinkCell();
					cell.Value = game.Title;
					row.Cells.Add( cell );
				}
				{
					DataGridViewLinkCell cell = new DataGridViewLinkCell();
					if( game.Website != null )
					{
						cell.Value = "Visit Website";
					}
					else
					{
					}
					row.Cells.Add( cell );
				}
				{
					DataGridViewImageCell cell = new DataGridViewImageCell();
					cell.Value = _googleIcon;
					row.Cells.Add( cell );
				}

				this.titleView.Rows.Add( row );
			}

			this.titleView.Sort( this.TitleColumn, ListSortDirection.Ascending );

			_outstandingRefresh = false;
		}

		private void refreshButton_Click( object sender, EventArgs e )
		{
			if( _outstandingRefresh == true )
				return;

			_service.ListGamesAsync();
		}

		private void titleView_CellContentClick( object sender, DataGridViewCellEventArgs e )
		{
			DataGridViewRow row = this.titleView.Rows[ e.RowIndex ];
			if( row == null )
				return;
			Game game = row.Tag as Game;
			if( game == null )
				return;

			if( e.ColumnIndex == this.TitleColumn.Index )
			{
				string link = string.Format( "{0}GameView.aspx?gameId={1}", ServiceUtilities.Address, game.GameID );
				try
				{
					Process.Start( link );
				}
				catch
				{
				}
			}
			else if( e.ColumnIndex == this.WebsiteColumn.Index )
			{
				if( game.Website == null )
					return;
				try
				{
					Process.Start( game.Website );
				}
				catch
				{
				}
			}
			else if( e.ColumnIndex == this.GoogleColumn.Index )
			{
				string googleServer = "http://www.google.com/search?";
				// TODO: URL encode the title somehow magical
				string googleQuery = string.Format( "{0}q={1}%20PSP", googleServer, game.Title );
				try
				{
					Process.Start( googleQuery );
				}
				catch
				{
				}
			}
		}

		private void titleView_SelectionChanged( object sender, EventArgs e )
		{
			if( this.titleView.SelectedRows.Count == 0 )
				this.submitButton.Enabled = false;
			else
				this.submitButton.Enabled = true;
		}

		private void addButton_Click( object sender, EventArgs e )
		{
			AutoResetEvent closeEvent = new AutoResetEvent( false );

			string username = TestUsername;
			string password = TestPassword;

			PleaseWaitDialog pwd = new PleaseWaitDialog( "Please wait while the game is added...", closeEvent );
			pwd.Show( this );

			_service.AddGameAsync( username, password, _release, _iconBytes, closeEvent );
		}

		private void ServiceAddGameCompleted( object sender, AddGameCompletedEventArgs e )
		{
			AutoResetEvent closeEvent = e.UserState as AutoResetEvent;
			closeEvent.Set();

			if( e.Error != null )
			{
				MessageBox.Show( this, "An error occured while submitting your game. Please check\nyour internet connection and try again later.", "PSP Player", MessageBoxButtons.OK, MessageBoxIcon.Error );
				return;
			}

			switch( e.Result )
			{
				case AddResult.Failed:
					MessageBox.Show( this, "An error occured while submitting your game.", "PSP Player", MessageBoxButtons.OK, MessageBoxIcon.Error );
					return;
				case AddResult.PermissionDenied:
					MessageBox.Show( this, "You do not have permission to add a game.", "PSP Player", MessageBoxButtons.OK, MessageBoxIcon.Error );
					return;
				case AddResult.Redundant:
					MessageBox.Show( this, "A game like this already exists.", "PSP Player", MessageBoxButtons.OK, MessageBoxIcon.Error );
					return;
				default:
					// Good
					break;
			}

			this.Close();
		}

		private void submitButton_Click( object sender, EventArgs e )
		{
			if( this.titleView.SelectedRows.Count == 0 )
				return;

			DataGridViewRow row = this.titleView.SelectedRows[ 0 ];
			Game game = row.Tag as Game;
			if( game == null )
				return;

			AutoResetEvent closeEvent = new AutoResetEvent( false );

			string username = TestUsername;
			string password = TestPassword;

			PleaseWaitDialog pwd = new PleaseWaitDialog( "Please wait while the game is submitted...", closeEvent );
			pwd.Show( this );
			
			_service.AddReleaseAsync( username, password, game.GameID, _release, _iconBytes, closeEvent );
		}

		private void ServiceAddReleaseCompleted( object sender, AddReleaseCompletedEventArgs e )
		{
			AutoResetEvent closeEvent = e.UserState as AutoResetEvent;
			closeEvent.Set();

			if( e.Error != null )
			{
				MessageBox.Show( this, "An error occured while submitting your game. Please check\nyour internet connection and try again later.", "PSP Player", MessageBoxButtons.OK, MessageBoxIcon.Error );
				return;
			}

			switch( e.Result )
			{
				case AddResult.Failed:
					MessageBox.Show( this, "An error occured while submitting your game.", "PSP Player", MessageBoxButtons.OK, MessageBoxIcon.Error );
					return;
				case AddResult.PermissionDenied:
					MessageBox.Show( this, "You do not have permission to add a game.", "PSP Player", MessageBoxButtons.OK, MessageBoxIcon.Error );
					return;
				case AddResult.Redundant:
					MessageBox.Show( this, "A game like this already exists.", "PSP Player", MessageBoxButtons.OK, MessageBoxIcon.Error );
					return;
				default:
					// Good
					break;
			}

			this.Close();
		}

		private void cancelButton_Click( object sender, EventArgs e )
		{
			this.Close();
		}
	}
}