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
using Noxa.Emulation.Psp.Player.Properties;
using Noxa.Utilities.Controls;

namespace Noxa.Emulation.Psp.Player.GamePicker
{
	public partial class AdvancedGameListing : UserControl
	{
		public event EventHandler SelectionChanged;

		private bool _regionEnabled = true;
		private int _filterOriginalPad;

		public AdvancedGameListing()
		{
			InitializeComponent();
			
			regionComboBox.SelectedIndex = 0;
		}

		protected override void OnLoad( EventArgs e )
		{
			base.OnLoad( e );

			listView_Resize( null, EventArgs.Empty );

			_filterOriginalPad = regionComboBox.Left - filterTextBox.Right;
		}

		public override string Text
		{
			get
			{
				return groupLabel.Text;
			}
			set
			{
				groupLabel.Text = value;
			}
		}

		public bool RegionEnabled
		{
			get
			{
				return _regionEnabled;
			}
			set
			{
				if( _regionEnabled != value )
				{
					_regionEnabled = value;
					if( value == false )
					{
						listView.Columns.Remove( regionColumnHeader );
						regionComboBox.Visible = false;
						filterTextBox.Width += regionComboBox.Right - filterTextBox.Right;
					}
					else
					{
						listView.Columns.Add( regionColumnHeader );
						regionComboBox.Visible = true;
						filterTextBox.Width = regionComboBox.Left - _filterOriginalPad - filterTextBox.Left;
					}
				}
			}
		}

		private void listView_Resize( object sender, EventArgs e )
		{
			int width = listView.ClientSize.Width - iconColumnHeader.Width - 1;
			if( _regionEnabled == true )
				width -= regionColumnHeader.Width;
			titleColumnHeader.Width = width;
		}

		private List<GameInformation> _games = new List<GameInformation>();
		private Dictionary<GameInformation, ListViewItem> _items = new Dictionary<GameInformation, ListViewItem>();
		private Dictionary<GameInformation, int> _imageLookup = new Dictionary<GameInformation, int>();

		public bool HasGames
		{
			get
			{
				return _games.Count > 0;
			}
		}

		private void PostChange()
		{
			_games.Sort( delegate( GameInformation x, GameInformation y )
			{
				return string.Compare( x.Parameters.Title, y.Parameters.Title, true );
			} );

			this.FilterGames();
		}

		public void AddGames( List<GameInformation> games )
		{
			Debug.Assert( games != null );

			foreach( GameInformation game in games )
				this.AddGame( game, true );

			this.PostChange();
		}

		public void AddGame( GameInformation game )
		{
			Debug.Assert( game != null );

			this.AddGame( game, true );

			this.PostChange();
		}

		private Image BuildGameImage( GameInformation game )
		{
			Image gameImage;
			if( game.Icon != null )
				gameImage = Image.FromStream( game.Icon );
			else
				gameImage = Image.FromStream( new MemoryStream( Resources.InvalidIcon, false ) );

			//Image mediaImage;
			//switch( game.GameType )
			//{
			//    default:
			//    case GameType.Eboot:
			//        mediaImage = Resources.SmallMemoryStickIcon;
			//        break;
			//    case GameType.UmdGame:
			//        mediaImage = Resources.SmallUmdIcon;
			//        break;
			//}

			Image regionImage = null;
			if( game.GameType == GameType.UmdGame )
			{
				string regionChar = game.Parameters.DiscID.Substring( 2, 1 );
				switch( regionChar )
				{
					case "U":
						regionImage = regionsImageList.Images[ "US" ];
						break;
					case "E":
						regionImage = regionsImageList.Images[ "UK" ];
						break;
					case "J":
						regionImage = regionsImageList.Images[ "JP" ];
						break;
					case "K":
						regionImage = regionsImageList.Images[ "KR" ];
						break;
					default:
						regionImage = regionsImageList.Images[ "Unknown" ];
						break;
				}
			}

			Bitmap combined = new Bitmap( gamesImageList.ImageSize.Width, gamesImageList.ImageSize.Height );
			using( Graphics g = Graphics.FromImage( combined ) )
			{
				g.DrawImage( gameImage, 0, 0, combined.Width, combined.Height );
				//g.DrawImage( mediaImage, combined.Width - mediaImage.Width, combined.Height - mediaImage.Height );
				if( regionImage != null )
					g.DrawImageUnscaled( regionImage, combined.Width - regionImage.Width - 2, combined.Height - regionImage.Height - 2 );
			}

			gameImage.Dispose();

			return combined;
		}

		private void AddGame( GameInformation game, bool batch )
		{
			_games.Add( game );

			ListViewItem item = new ListViewItem();
			item.Tag = game;

			Image gameImage = this.BuildGameImage( game );
			gamesImageList.Images.Add( gameImage );
			item.ImageIndex = gamesImageList.Images.Count - 1;
			_imageLookup.Add( game, item.ImageIndex );

			string regionName = string.Empty;
			if( game.GameType == GameType.UmdGame )
			{
				string regionChar = game.Parameters.DiscID.Substring( 2, 1 );
				switch( regionChar )
				{
					case "U":
						regionName = "US";
						break;
					case "E":
						regionName = "UK";
						break;
					case "J":
						regionName = "JP";
						break;
					case "K":
						regionName = "KR";
						break;
					default:
						regionName = "??";
						break;
				}
			}

			item.SubItems.AddRange( new ListViewItem.ListViewSubItem[]{
				new ListViewItem.ListViewSubItem( item, game.Parameters.Title ),
				new ListViewItem.ListViewSubItem( item, regionName ),
			} );
			item.ToolTipText = Path.GetFileName( game.HostPath );

			_items.Add( game, item );
		}

		public void RemoveGame( GameInformation game )
		{
			ListViewItem item = _items[ game ];
			int imageIndex = _imageLookup[ game ];

			_games.Remove( game );
			_imageLookup.Remove( game );
			_items.Remove( game );

			listView.BeginUpdate();
			if( listView.Items.Contains( item ) == true )
				listView.Items.Remove( item );
			listView.EndUpdate();

			//gamesImageList.Images.RemoveAt( imageIndex );
		}

		public void ClearGames()
		{
			_games.Clear();
			_imageLookup.Clear();
			_items.Clear();
			gamesImageList.Images.Clear();

			listView.BeginUpdate();
			listView.Items.Clear();
			listView.EndUpdate();
		}

		private bool ContainsAny( string value, string[] filters )
		{
			string fixedValue = value.ToLowerInvariant();
			foreach( string filter in filters )
			{
				if( fixedValue.Contains( filter ) == true )
					return true;
			}
			return false;
		}

		public void FilterGames()
		{
			string[] filters = null;
			string filter = this.filterTextBox.Text.ToLowerInvariant().Trim();
			if( filter == string.Empty )
				filter = null;
			else
			{
				filters = filter.Split( ' ' );
				List<string> ft = new List<string>( filters.Length );
				foreach( string f in filters )
				{
					if( f == string.Empty )
						continue;
					if( ft.Contains( f ) == false )
						ft.Add( f );
				}
				filters = ft.ToArray();
			}

			string region = regionComboBox.SelectedItem as string;
			if( region.ToLowerInvariant() == "all" )
				region = string.Empty;

			this.listView.BeginUpdate();
			this.listView.Items.Clear();
			foreach( GameInformation game in _games )
			{
				ListViewItem item = _items[ game ];
				if( filter != null )
				{
					bool valid =
						ContainsAny( game.Parameters.Title, filters ) ||
						( ( game.HostPath != null ) && ContainsAny( Path.GetFileNameWithoutExtension( game.HostPath ), filters ) );
					if( valid == false )
						continue;
				}
				if( ( game.GameType == GameType.UmdGame ) &&
					( region != string.Empty ) )
				{
					if( item.SubItems[ 2 ].Text.Contains( region.ToUpperInvariant() ) == false )
						continue;
				}

				this.listView.Items.Add( item );
				//this.listView.SetItemDisplay( item.Index, iconColumnHeader.Index, ImagedListView.ItemDisplay.Image, imageIndex );
			}
			this.listView.EndUpdate();

			bool hasFilter = ( filter != null ) || ( region != string.Empty );
			clearFilterButton.Enabled = hasFilter;
		}

		private void filterTextBox_TextChanged( object sender, EventArgs e )
		{
			this.FilterGames();
		}

		public GameInformation SelectedGame
		{
			get
			{
				if( listView.SelectedItems.Count == 0 )
					return null;
				ListViewItem item = listView.SelectedItems[ 0 ];
				return ( GameInformation )item.Tag;
			}
			set
			{
				if( value != null )
				{
					ListViewItem item = _items[ value ];
					listView.BeginUpdate();
					listView.SelectedItems.Clear();
					item.EnsureVisible();
					item.Selected = true;
					listView.EndUpdate();
				}
				else
					listView.SelectedItems.Clear();
			}
		}

		private void listView_ItemSelectionChanged( object sender, ListViewItemSelectionChangedEventArgs e )
		{
			if( this.SelectionChanged != null )
				this.SelectionChanged( this, EventArgs.Empty );
		}

		private void regionComboBox_SelectedIndexChanged( object sender, EventArgs e )
		{
			this.FilterGames();
		}

		private void clearFilterButton_Click( object sender, EventArgs e )
		{
			this.filterTextBox.Text = null;
			this.regionComboBox.SelectedIndex = 0;
		}
	}
}
