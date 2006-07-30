using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Noxa.Emulation.Psp.Games;
using System.Diagnostics;
using Noxa.Emulation.Psp.Player.Properties;
using System.IO;

namespace Noxa.Emulation.Psp.Player.GamePicker
{
	partial class GameEntry : UserControl
	{
		protected GameInformation _game;
		protected bool _isSelected;

		public GameEntry()
		{
			InitializeComponent();
		}

		public GameEntry( PickerDialog dialog, GameInformation game )
			: this()
		{
			Debug.Assert( dialog != null );
			Debug.Assert( game != null );

			_game = game;

			this.titleLabel.Text = _game.Parameters.Title;
			this.infoLabel.Text = "";

			Image gameImage;
			if( _game.Icon != null )
				gameImage = Image.FromStream( _game.Icon );
			else
				gameImage = Image.FromStream( new MemoryStream( Resources.InvalidIcon, false ) );

			Image mediaImage;
			switch( _game.GameType )
			{
				default:
				case GameType.Eboot:
					mediaImage = Resources.SmallMemoryStickIcon;
					break;
				case GameType.UmdGame:
					mediaImage = Resources.SmallUmdIcon;
					break;
			}

			Bitmap combined = new Bitmap( this.gamePicture.Width, this.gamePicture.Height );
			using( Graphics g = Graphics.FromImage( combined ) )
			{
				g.DrawImage( gameImage, 0, 0, combined.Width, combined.Height );
				g.DrawImage( mediaImage, combined.Width - mediaImage.Width, combined.Height - mediaImage.Height );
			}
			this.gamePicture.Image = combined;

			this.SetStyle( ControlStyles.StandardClick, true );
			this.SetStyle( ControlStyles.StandardDoubleClick, true );
		}

		public GameInformation Game
		{
			get
			{
				return _game;
			}
		}

		public bool IsSelected
		{
			get
			{
				return _isSelected;
			}
			set
			{
				_isSelected = value;
				this.Invalidate();
			}
		}

		protected override void OnPaintBackground( PaintEventArgs e )
		{
			base.OnPaintBackground( e );

			if( _isSelected == false )
			{
				Rectangle rect = this.ClientRectangle;
				rect.Inflate( new Size( 0, -1 ) );
				rect.Width -= 2;
				e.Graphics.DrawRectangle( SystemPens.ActiveBorder, rect );
			}
		}

		private void gamePicture_Click( object sender, EventArgs e )
		{
			this.OnClick( e );
		}

		private void mediaPicture_Click( object sender, EventArgs e )
		{
			this.OnClick( e );
		}

		private void titleLabel_Click( object sender, EventArgs e )
		{
			this.OnClick( e );
		}

		private void infoLabel_Click( object sender, EventArgs e )
		{
			this.OnClick( e );
		}

		private void infoLabel_DoubleClick( object sender, EventArgs e )
		{
			this.OnDoubleClick( e );
		}

		private void titleLabel_DoubleClick( object sender, EventArgs e )
		{
			this.OnDoubleClick( e );
		}

		private void gamePicture_DoubleClick( object sender, EventArgs e )
		{
			this.OnDoubleClick( e );
		}

		private void mediaPicture_DoubleClick( object sender, EventArgs e )
		{
			this.OnDoubleClick( e );
		}
	}
}
