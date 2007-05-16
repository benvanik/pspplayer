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
using System.IO;
using System.Drawing.Drawing2D;
using Noxa.Emulation.Psp.Games;
using Noxa.Emulation.Psp.Player.Properties;
using Noxa.Emulation.Psp.Player.ServiceTools;

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

		public GameEntry( GameInformation game )
			: this()
		{
			Debug.Assert( game != null );

			_game = game;

			this.titleLabel.Text = _game.Parameters.Title;

			switch( _game.GameType )
			{
				case GameType.Eboot:
					{
						this.infoLabel.Text = "";
					}
					break;
				case GameType.UmdGame:
					{
						Debug.Assert( _game.Tag != null );
						string hostPath = ( string )_game.Tag;
						this.infoLabel.Text = string.Format( "{0}", Path.GetFileName( hostPath ) );
						string tooltip = string.Format( "{0}", hostPath );
						this.toolTip1.SetToolTip( this.infoLabel, tooltip );
						this.toolTip1.SetToolTip( this.gamePicture, tooltip );
					}
					break;
			}

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
				//g.DrawImage( mediaImage, combined.Width - mediaImage.Width, combined.Height - mediaImage.Height );
			}
			this.gamePicture.Image = combined;

			this.TabStop = true;
			this.SetStyle( ControlStyles.StandardClick, true );
			this.SetStyle( ControlStyles.StandardDoubleClick, true );
			this.SetStyle( ControlStyles.OptimizedDoubleBuffer, true );
			this.SetStyle( ControlStyles.Selectable, true );

			this.SetupPainting();
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

		#region Pens/brushes/colors/etc

		private Pen p_innerBorder = null;
		private Pen p_outerBorder = null;
		private Color c_borderColor = Color.Empty;
		private Pen p_borderPen = null;
		private Brush b_borderBrush = null;
		private Brush b_gradientBar = null;
		private Pen p_gradientPen = null;

		private Pen p_selectedBorder = null;
		private Pen p_darkBorderPen = null;
		private Pen p_orangeOuter = null;
		private Pen p_orangeInner = null;
		private Brush b_orangeInner = null;
		private Pen p_itemBottom = null;

		private void SetupPainting()
		{
			p_innerBorder = new Pen( Color.FromArgb( 109, 139, 164 ), 1.0f );
			p_outerBorder = new Pen( Color.FromArgb( 127, 157, 185 ), 1.0f );
			c_borderColor = Color.FromArgb( 240, 240, 234 );
			p_borderPen = new Pen( c_borderColor, 1.0f );
			b_borderBrush = new SolidBrush( c_borderColor );
			
			p_selectedBorder = new Pen( Color.FromArgb( 173, 190, 204 ), 1.0f );
			p_darkBorderPen = new Pen( Color.FromArgb( 145, 155, 156 ), 1.0f );
			p_orangeOuter = new Pen( Color.FromArgb( 230, 139, 44 ), 1.0f );
			p_orangeInner = new Pen( Color.FromArgb( 255, 200, 60 ), 1.0f );
			b_orangeInner = new SolidBrush( p_orangeInner.Color );
			p_itemBottom = new Pen( Color.FromArgb( 236, 235, 228 ), 1.0f );
		}

		#endregion

		protected override void OnSizeChanged( EventArgs e )
		{
			if( b_gradientBar != null )
				b_gradientBar.Dispose();
			b_gradientBar = null;
			if( p_gradientPen != null )
				p_gradientPen.Dispose();
			p_gradientPen = null;
			base.OnSizeChanged( e );
		}

		protected override void OnPaintBackground( PaintEventArgs e )
		{
			base.OnPaintBackground( e );

			Rectangle bounds = this.ClientRectangle;
			bounds.Inflate( -1, -1 );

			if( b_gradientBar == null )
			{
				b_gradientBar = new LinearGradientBrush( new Point( 0, 0 ), new Point( bounds.Width, 0 ), Color.White, p_itemBottom.Color );
				p_gradientPen = new Pen( b_gradientBar );
			}

			// Inner fill
			if( this.IsSelected == false )
				e.Graphics.FillRectangle( b_gradientBar, 1, 1, bounds.Width - 1, bounds.Height - 1 );
			else
				e.Graphics.FillRectangle( Brushes.White, 1, 1, bounds.Width - 1, bounds.Height - 1 );

			// Corners
			e.Graphics.DrawCurve( p_outerBorder, new PointF[] {
                new PointF( 0, 3 ), new PointF( 1, 1 ), new PointF( 3, 0 )
            } );
			e.Graphics.DrawCurve( p_outerBorder, new PointF[] {
                new PointF( 0, bounds.Height - 3 ), new PointF( 1, bounds.Height - 1 ), new PointF( 3, bounds.Height )
            } );
			e.Graphics.DrawCurve( p_outerBorder, new PointF[] {
                new PointF( bounds.Width - 3, 0 ), new PointF( bounds.Width - 1, 1 ), new PointF( bounds.Width, 3 )
            } );
			e.Graphics.DrawCurve( p_outerBorder, new PointF[] {
                new PointF( bounds.Width - 3, bounds.Height ), new PointF( bounds.Width - 1, bounds.Height - 1 ), new PointF( bounds.Width, bounds.Height - 3 )
            } );

			// Outer borders
			Pen borderPen = ( this.IsSelected == true ? p_darkBorderPen : p_outerBorder );
			e.Graphics.DrawLine( borderPen, 3, 0, bounds.Width - 3, 0 );
			e.Graphics.DrawLine( borderPen, 3, bounds.Height, bounds.Width - 3, bounds.Height );
			e.Graphics.DrawLine( borderPen, 0, 3, 0, bounds.Height - 3 );
			e.Graphics.DrawLine( borderPen, bounds.Width, 3, bounds.Width, bounds.Height - 3 );

			// Focus border
			if( this.Focused == true )
			{
			    Rectangle rect = this.ClientRectangle;
				rect.Inflate( new Size( -2, -2 ) );
				rect.Height -= 1;
				if( this.IsSelected == true )
					rect.Width -= 3;
				else
					rect.Width -= 1;
			    ControlPaint.DrawFocusRectangle( e.Graphics, rect );
			}

			// Orange selection thingie
			if( this.IsSelected == true )
			{
				// Inside
				e.Graphics.FillRectangle( b_orangeInner, bounds.Width - 2, 1, 2, bounds.Height - 1 );
				// Border
				e.Graphics.DrawLines( p_orangeOuter, new Point[]{
                    new Point( bounds.Width - 2, 0 ),
                    new Point( bounds.Width, 2 ),
                    new Point( bounds.Width, bounds.Height - 2 ),
                    new Point( bounds.Width - 2, bounds.Height )
                } );
			}
		}

		protected override void OnEnter( EventArgs e )
		{
			this.Invalidate();
			base.OnEnter( e );
		}

		protected override void OnLeave( EventArgs e )
		{
			this.Invalidate();
			base.OnLeave( e );
		}

		protected override void OnGotFocus( EventArgs e )
		{
			this.Invalidate();
			base.OnGotFocus( e );
		}

		protected override void OnLostFocus( EventArgs e )
		{
			this.Invalidate();
			base.OnLostFocus( e );
		}

		protected override void OnKeyPress( KeyPressEventArgs e )
		{
			switch( e.KeyChar )
			{
				case ( char )Keys.Enter:
					this.OnDoubleClick( EventArgs.Empty );
					break;
				default:
					this.OnClick( EventArgs.Empty );
					break;
			}
			base.OnKeyPress( e );
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

		private void checkCompatibilityToolStripMenuItem_Click( object sender, EventArgs e )
		{
			SubmitGame dialog = new SubmitGame( _game );
			dialog.ShowDialog( this.FindForm() );
		}

		private void removeToolStripMenuItem_Click( object sender, EventArgs e )
		{
			MessageBox.Show( "TODO" );
		}
	}
}
