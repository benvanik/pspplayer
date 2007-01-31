// ----------------------------------------------------------------------------
// Shared Utility Library
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Noxa.Utilities.Controls
{
	/// <summary>
	/// Whidbey style tab control.
	/// </summary>
	[ToolboxItem( true )]
	[ToolboxBitmap( typeof( WhidbeyTabControl ) )]
	[Designer( typeof( WhidbeyTabControlDesigner ) )]
	public partial class WhidbeyTabControl : UserControl
    {
        #region Data

        private     WhidbeyTabCollection            m_aControls;
        private     Rectangle                       m_nBounds           = new Rectangle();
		private		WhidbeyTabPage					m_oSelected			= null;
		private		WhidbeyTabPage					m_oHovered			= null;
		
		private event EventHandler<WhidbeyTabEventArgs>		m_eSelected;

		#region Pens/brushes/colors/etc

		private		Pen		p_innerBorder		= null;
		private		Pen		p_outerBorder		= null;
		private		Color	c_borderColor		= Color.Empty;
		private		Pen		p_borderPen			= null;
		private		Brush	b_borderBrush		= null;
		private		Brush	b_gradientBar		= null;
		private		Pen		p_gradientPen		= null;

		private		Pen		p_selectedBorder	= null;
		private		Pen		p_darkBorderPen		= null;
		private		Pen		p_orangeOuter		= null;
		private		Pen		p_orangeInner		= null;
		private		Pen		p_itemBottom		= null;

		#endregion
		
		#endregion

		#region Constructors

		/// <summary>
		/// Constructor.
		/// </summary>
        public WhidbeyTabControl()
        {
            InitializeComponent();

            SetStyle( ControlStyles.AllPaintingInWmPaint, true );
            SetStyle( ControlStyles.ResizeRedraw, true );
            SetStyle( ControlStyles.UserPaint, true );
            SetStyle( ControlStyles.Opaque, false );
            SetStyle( ControlStyles.SupportsTransparentBackColor, true );
            SetStyle( ControlStyles.OptimizedDoubleBuffer, true );

            SetStyle( ControlStyles.Selectable, true );
            SetStyle( ControlStyles.ContainerControl, true );

            // This needs to be set or otherwise things get transparent
            this.panelContainer.BackColor = Color.FromArgb( 243, 241, 230 );

            // Setup collection
            m_aControls = new WhidbeyTabCollection( this );
        }

		#endregion

		#region Events

		/// <summary>
		/// Fired when the selected tab changes.
		/// </summary>
		public event EventHandler<WhidbeyTabEventArgs> SelectionChanged
		{
			add
			{
				m_eSelected	+= value;
			}
			remove
			{
				m_eSelected	-= value;
			}
		}

		#endregion

		#region Accessors

		/// <summary>
		/// Get the collection of tab pages this control contains.
		/// </summary>
		[DesignerSerializationVisibility( DesignerSerializationVisibility.Content )]
		public WhidbeyTabCollection TabPages
		{
			get{ return( m_aControls ); }
		}

		/// <summary>
		/// Get or set the selected tab by index.
		/// </summary>
		public int SelectedIndex
		{
			get
			{
				if( m_oSelected != null )
					return( m_aControls.IndexOf( m_oSelected ) );
				else
					return( 0 );
			}
			set
			{
				// Ignore stupid values that the designer sets
				if( ( m_aControls.Count <= value ) ||
					( value == -1 ) )
					return;

				// This will throw an exception if it is an invalid tab
				SelectTab( m_aControls[ value ] );
			}
		}

		/// <summary>
		/// Get or set the selected tab.
		/// </summary>
		public WhidbeyTabPage SelectedTab
		{
			get
			{
				return( m_oSelected );
			}
			set
			{
				// Ignore stupid values that the designer sets
				if( value == null )
					return;

				SelectTab( value );
			}
		}

		#endregion

		#region Methods
		
		#endregion

		#region Internal methods

        #region Painting

		/// <summary>
		/// Setup the pens and brushes used for drawing operations.
		/// </summary>
		private void SetupPaintingTools()
		{
			p_innerBorder = new Pen( Color.FromArgb( 109, 139, 164 ), 1.0f );
			p_outerBorder = new Pen( Color.FromArgb( 127, 157, 185 ), 1.0f );
			c_borderColor = Color.FromArgb( 240, 240, 234 );
			p_borderPen = new Pen( c_borderColor, 1.0f );
			b_borderBrush = new SolidBrush( c_borderColor );
			b_gradientBar = new LinearGradientBrush( new Point( 0, 0 ), new Point( 109, 0 ), Color.White, c_borderColor );
			p_gradientPen = new Pen( b_gradientBar );

			p_selectedBorder = new Pen( Color.FromArgb( 173, 190, 204 ), 1.0f );
			p_darkBorderPen = new Pen( Color.FromArgb( 145, 155, 156 ), 1.0f );
			p_orangeOuter = new Pen( Color.FromArgb( 230, 139, 44 ), 1.0f );
			p_orangeInner = new Pen( Color.FromArgb( 255, 200, 60 ), 1.0f );
			p_itemBottom = new Pen( Color.FromArgb( 236, 235, 228 ), 1.0f );
		}

        /// <summary>
        /// Repaint control.
        /// </summary>
        /// <param name="e">Paint event arguments.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            // TODO: Optimize this by not redrawing tab items if not needed
            // - Simply check intersection with m_nBounds

			if( b_borderBrush == null )
				SetupPaintingTools();

            // -- Border bars (inside) --
            // Top
            e.Graphics.FillRectangle( b_borderBrush, 4, 1, this.Width, 5 );
            // Right
            e.Graphics.FillRectangle( b_borderBrush, this.Width - 6, 1, this.Width - 1, this.Height - 2 );
            // Bottom
            e.Graphics.FillRectangle( b_borderBrush, 107, this.Height - 6, this.Width - 1, 5 );
            // Left
            e.Graphics.FillRectangle( b_borderBrush, 102, 0, 5, this.Height );

            // -- Tab bar background --
            e.Graphics.FillRectangle( b_gradientBar, 2, 2, 107, m_nBounds.Bottom - 2 );
            Region region = new Region( new GraphicsPath( new PointF[]{
                new PointF( 3, m_nBounds.Bottom ), new PointF( 107, m_nBounds.Bottom ), new PointF( 107, m_nBounds.Bottom + 33 )
            }, new byte[] {
                (byte) PathPointType.Line,
                (byte) PathPointType.Line,
                (byte) PathPointType.Line
            } ) );
            e.Graphics.FillRegion( b_gradientBar, region );

            // Fixup for bottom block
            e.Graphics.FillRectangle( b_borderBrush, 100, m_nBounds.Bottom + 31, 2, 3 );

            // -- Border bars (outside line) --
            // Top
            e.Graphics.DrawLine( p_outerBorder, 5, 0, this.Width - 1, 0 );
            // Right
            e.Graphics.DrawLine( p_outerBorder, this.Width - 1, 0, this.Width - 1, this.Height - 1 );
            // Bottom
            e.Graphics.DrawLine( p_outerBorder, 101, this.Height - 1, this.Width - 1, this.Height - 1 );
            // Left
            e.Graphics.DrawLine( p_outerBorder, 101, m_nBounds.Bottom + 35, 101, this.Height );

            // Left side of outer border (to the left to tabs)
            e.Graphics.DrawLine( p_outerBorder, 1, 5, 1, m_nBounds.Bottom - 3 );
            e.Graphics.DrawLine( p_outerBorder, 2, m_nBounds.Bottom - 2, 2, m_nBounds.Bottom - 1 );

            // Top left curve
            e.Graphics.DrawCurve( p_outerBorder, new PointF[] {
                new PointF( 1, 5 ), new PointF( 2.5f, 1.5f ), new PointF( 5, 0 )
            } );

            // This is the bottom curve
            e.Graphics.DrawLine( p_outerBorder, 3, m_nBounds.Bottom, 92, m_nBounds.Bottom + 28 );
            e.Graphics.DrawCurve( p_outerBorder, new PointF[]{
                new PointF( 92, m_nBounds.Bottom + 28 ),
                new PointF( 99f, m_nBounds.Bottom + 31 ),
                new PointF( 101, m_nBounds.Bottom + 36 )
            } );

            // -- Border bars (inside line) --
            // Top
            e.Graphics.DrawLine( p_innerBorder, 107, 6, this.Width - 7, 6 );
            // Right
            e.Graphics.DrawLine( p_innerBorder, this.Width - 7, 6, this.Width - 7, this.Height - 7 );
            // Bottom
            e.Graphics.DrawLine( p_innerBorder, 107, this.Height - 7, this.Width - 7, this.Height - 7 );

            // This is the left side, which is overwritten in areas
            e.Graphics.DrawLine( p_innerBorder, 107, 6, 107, this.Height - 7 );

            // -- Tab section (top and left pixel line fixup)
            e.Graphics.DrawLine( p_gradientPen, 6, 1, 106, 1 );

            // -- Tab stuff
            // Upper-most line
            e.Graphics.DrawLine( Pens.White, 6, 5, 101, 5 );
            // Bottom-most line
            e.Graphics.DrawLine( new Pen( Color.FromArgb( 236, 235, 228 ), 1.0f ), 6, m_nBounds.Bottom - 6, 101, m_nBounds.Bottom - 6 );

            // -- Draw all tabs --
            foreach( WhidbeyTabPage tab in m_aControls )
                PaintTab( e.Graphics, tab );
		}

        /// <summary>
        /// Paint the given tab to the given graphics context.
        /// </summary>
        /// <param name="g">Destination graphics context.</param>
        /// <param name="tab">Tab to paint.</param>
        private void PaintTab(Graphics g, WhidbeyTabPage tab)
        {
            int borderWidth;    // Only used with Selected and Hovered

			// Hide the label if there is an icon
			tab.Label.Visible = ( tab.Image == null );

            if( tab.State == TabState.Selected )
            {
				// Fill background white
				g.FillRectangle( Brushes.White, 3, tab.ItemBounds.Top + 1, 104, tab.ItemBounds.Height - 2 );

				// Draw right border
				using( Pen p_selectedBorder = new Pen( Color.FromArgb( 173, 190, 204 ), 1.0f ) )
					g.DrawLine( p_selectedBorder, tab.ItemBounds.Right - 1, tab.ItemBounds.Top + 1, tab.ItemBounds.Right - 1, tab.ItemBounds.Bottom - 1 );

				// We are selected, so width goes full
                borderWidth = 107;
            }
            else
            {
                // Hovered, so width goes one short
                borderWidth = 106;
            }

            if( ( tab.State == TabState.Hovered ) ||
                ( tab.State == TabState.Selected ) )
            {
                // -- Draw fancy border --
                // Top
				g.DrawLine( p_darkBorderPen, 3, tab.ItemBounds.Top, borderWidth, tab.ItemBounds.Top );
				// Bottom
				g.DrawLine( p_darkBorderPen, 3, tab.ItemBounds.Bottom - 1, borderWidth, tab.ItemBounds.Bottom - 1 );

				// -- Draw left side orange element --
                // Inside
				g.DrawRectangle( p_orangeInner, 1, tab.ItemBounds.Top + 1, 1, tab.ItemBounds.Height - 3 );
				// Border
                g.DrawLines( p_orangeOuter, new Point[]{
                    new Point( 2, tab.ItemBounds.Top ),
                    new Point( 0, tab.ItemBounds.Top + 2 ),
                    new Point( 0, tab.ItemBounds.Bottom - 3 ),
                    new Point( 2, tab.ItemBounds.Bottom - 1 )
                } );
            }
            else
            {
                // Top line
				g.DrawLine( p_itemBottom, 6, tab.ItemBounds.Top, 101, tab.ItemBounds.Top );
				// Bottom line
				g.DrawLine( Pens.White, 6, tab.ItemBounds.Bottom - 1, 101, tab.ItemBounds.Bottom - 1 );
			}

			if( tab.Image != null )
			{
				Rectangle rect = tab.ItemBounds;
				int centerX = rect.X + ( rect.Width / 2 );
				int centerY = rect.Y + ( rect.Height / 2 );
				centerX -= ( tab.Image.Width / 2 );
				centerY -= ( tab.Image.Height / 2 );
				g.DrawImageUnscaled( tab.Image, centerX, centerY );
			}
        }

        #endregion

        #region Input

		#region Designer

		/// <summary>
		/// Called when the mouse button goes down.
		/// </summary>
		/// <param name="e">Information about click.</param>
		protected override void OnMouseDown(MouseEventArgs e)
		{
			// Only work in design mode
			if( this.DesignMode == true )
			{
				// Enumerate tabs and see if it hits one
				foreach( WhidbeyTabPage tab in m_aControls )
				{
					if( tab.ItemBounds.Contains( e.Location ) == true )
					{
						ArrayList items = new ArrayList();
						items.Add( tab );
						ISelectionService selectionSvc = this.GetService( typeof( ISelectionService ) ) as ISelectionService;
						selectionSvc.SetSelectedComponents( items );
						break;
					}
				}
			}

			// Go down
			base.OnMouseDown( e );
		}

		#endregion

		/// <summary>
		/// Called when the mouse is moved over the control or items.
        /// </summary>
        /// <param name="x">X coordinate (relative to control origin).</param>
        /// <param name="y">Y coordinate (relative to control origin).</param>
        private void HandleMouseMove(int x, int y)
        {
            // See if it hits the tab surface
            if( m_nBounds.Contains( x, y ) == false )
            {
                // Not in bounds, so unhover selected and abort
                Unhover();
                return;
            }

            // Enumerate tabs and see if it hits one
            foreach( WhidbeyTabPage tab in m_aControls )
            {
				if( tab.ItemBounds.Contains( x, y ) == true )
				{
                    // Hit! So hover, but only if it's not selected
                    if( tab.State != TabState.Selected )
                        Hover( tab );
                    break;
                }
            }
        }

        /// <summary>
        /// Called when the mouse button is lifted.
        /// </summary>
        /// <param name="x">X coordinate (relative to control origin).</param>
        /// <param name="y">Y coordinate (relative to control origin).</param>
        private void HandleMouseUp(int x, int y)
        {
            // See if it hits the tab surface
            if( m_nBounds.Contains( x, y ) == false )
            {
                // Not in bounds, so unhover selected and abort
                Unhover();
                return;
            }

            // Enumerate tabs and see if it hits one
			foreach( WhidbeyTabPage tab in m_aControls )
			{
				if( tab.ItemBounds.Contains( x, y ) == true )
				{
                    // Hit!
                    SelectTab( tab );
                    break;
                }
            }
        }

        /// <summary>
        /// Called when the mouse leaves the control or an item.
        /// </summary>
        private void HandleMouseLeave()
        {
            Unhover();
		}

		#region Keyboard input

		/// <summary>
		/// Fired on key down.
		/// </summary>
		/// <param name="sender">Ignore.</param>
		/// <param name="e">Ignore.</param>
		private void WhidbeyTabControl_KeyDown(object sender, KeyEventArgs e)
		{
			// This doesn't work for some reason

			/*int n = this.SelectedIndex;
			n++;
			while( this.TabPages[ n ].Enabled == false )
			{
				n++;
				if( n >= this.TabPages.Count )
					n = 0;
				if( n == this.SelectedIndex )
					return;
			}
			if( n != this.SelectedIndex )
				this.SelectedIndex = n;*/
		}

		#endregion

        #region Wrappers from self

        private void WhidbeyTabControl_MouseMove(object sender, MouseEventArgs e)
        {
            this.HandleMouseMove( e.X, e.Y );
        }

        private void WhidbeyTabControl_MouseLeave(object sender, EventArgs e)
        {
            this.HandleMouseLeave();
        }

        private void WhidbeyTabControl_MouseUp(object sender, MouseEventArgs e)
        {
            this.HandleMouseUp( e.X, e.Y );
        }

        #endregion

        #region Wrappers from labels

        private void Label_MouseMove(object sender, MouseEventArgs e)
        {
            Label label = sender as Label;
            if( label == null )
                return;
            this.HandleMouseMove( e.X + label.Left, e.Y + label.Top );
        }

        private void Label_MouseLeave(object sender, EventArgs e)
        {
            this.HandleMouseLeave();
        }

        private void Label_MouseUp(object sender, MouseEventArgs e)
        {
            Label label = sender as Label;
            if( label == null )
                return;
            this.HandleMouseUp( e.X + label.Left, e.Y + label.Top );
        }

        #endregion

        #endregion

        #region Tab logic

        /// <summary>
        /// Rebuild tab bounding rectangles.
        /// </summary>
        private void RebuildTabs()
        {
            // Basic setup of bounds
            m_nBounds.X = 0;
            m_nBounds.Y = 6;
            m_nBounds.Width = 107;
            m_nBounds.Height = 0;

            // Start tabs
            for( int n = 0; n < m_aControls.Count; n++ )
            {
				WhidbeyTabPage	tab	= m_aControls[ n ];

				tab.ItemBounds		= new Rectangle( m_nBounds.X, m_nBounds.Bottom, m_nBounds.Width + 1, 33 );

				tab.Label.Top		= tab.ItemBounds.Top;
				tab.Label.Width		= tab.ItemBounds.Width - 12;
				tab.Label.Height	= tab.ItemBounds.Height;

				m_nBounds.Height	+= 33;
            }

            m_nBounds.Height += 6;
        }

        /// <summary>
        /// Hover the given tab.
        /// </summary>
        /// <param name="tab">Tab to hover.</param>
		private void Hover(WhidbeyTabPage tab)
		{
			// Ignore if disabled
			if( ( tab.State == TabState.Disabled ) ||
				( tab.Enabled == false ) )
				return;

			if( tab.State != TabState.Selected )
                tab.State = TabState.Hovered;
            m_oHovered = tab;

            // Force redraw
			this.Invalidate( tab.ItemBounds );
		}

        /// <summary>
        /// Unhover the currently hovered tab.
        /// </summary>
        private void Unhover()
        {
            if( m_oHovered != null )
            {
				WhidbeyTabPage tab = m_oHovered;
				m_oHovered = null;

                tab.State = TabState.Normal;

                // Force redraw
				this.Invalidate( tab.ItemBounds );
			}
        }

        /// <summary>
        /// Select the given tab.
        /// </summary>
        /// <param name="tab">Tab to select.</param>
		private void SelectTab(WhidbeyTabPage tab)
		{
			// Ignore if disabled
			if( ( tab.State == TabState.Disabled ) ||
				( tab.Enabled == false ) )
				return;

            // Unselect previous
            UnselectTab();
            Unhover();

            // Set selected
            tab.State = TabState.Selected;
            m_oSelected = tab;

            // Add new control to panel
			tab.ShowNotifty( true );
            tab.Visible = true;
			tab.Focus();

            // Force redraw
			this.Invalidate( tab.ItemBounds );

			// Fire event
			EventHandler<WhidbeyTabEventArgs> ev = this.m_eSelected;
			if( ev != null )
				ev( this, new WhidbeyTabEventArgs( tab ) );
		}

        /// <summary>
        /// Select the currently selected tab.
        /// </summary>
        private void UnselectTab()
        {
            if( m_oSelected != null )
            {
				WhidbeyTabPage tab = m_oSelected;
				m_oSelected = null;

                tab.State = TabState.Normal;

                // Drop old control from the panel
				tab.ShowNotifty( false );
                tab.Visible = false;

                // Force redraw
				this.Invalidate( tab.ItemBounds );
			}
        }

		/// <summary>
		/// Called whenever enable state changes on a tab.
		/// </summary>
		/// <param name="tab">Tab to inspect.</param>
		internal void SetTabEnabled(WhidbeyTabPage tab)
		{
			if( tab.Enabled == false )
			{
				// -- Tab is being disabled --

				// If it's selected, deselect
				if( tab == m_oSelected )
				{
					// Find another tab
					bool subFound = false;
					foreach( WhidbeyTabPage page in m_aControls )
					{
						if( ( page != tab ) &&
							( page.Enabled == true ) )
						{
							SelectTab( page );
							subFound = true;
							break;
						}
					}

					// Check to make sure a substitute was found
					if( subFound == false )
						throw new InvalidOperationException( "No valid page was found to replace disabled page; cannot disable the only page." );
				}

				// Set stuff
				tab.Label.ForeColor = Color.Gray;
				tab.State = TabState.Disabled;
			}
			else
			{
				// -- Tab is being enabled --

				// Set stuff
				tab.Label.ForeColor = Color.Black;
				tab.State = TabState.Normal;
			}

			// Force redraw
			this.Invalidate( tab.ItemBounds );
		}

        #endregion

        #region Tab add/remove/etc controls

        /// <summary>
        /// Insert the given tab into the control.
        /// </summary>
        /// <param name="control">Tab to add.</param>
        internal void InsertTab(WhidbeyTabPage tab)
        {
            // Create label for tab
            Label	label		= new Label();
            label.Text			= tab.Text;
            label.TextAlign		= ContentAlignment.MiddleLeft;
            label.BackColor		= Color.Transparent;
			if( tab.State == TabState.Disabled )
				label.ForeColor = Color.Gray;
			else
				label.ForeColor	= Color.Black;
			label.Left			= 12;
            label.MouseMove		+= new MouseEventHandler( Label_MouseMove );
            label.MouseLeave	+= new EventHandler( Label_MouseLeave );
            label.MouseUp		+= new MouseEventHandler( Label_MouseUp );
            this.Controls.Add( label );
			tab.Label = label;

			// Add tab control to container
			this.panelContainer.Controls.Add( tab );
			tab.Dock	= DockStyle.Fill;
			tab.Visible	= false;

			// Rebuild the tab bounds
			this.RebuildTabs();

			// Invalidate everything (a lot can change)
			this.Invalidate();

			// Always set the first to selected
			if( this.m_aControls.IndexOf( tab ) == 0 )
				SelectTab( tab );
		}

        /// <summary>
        /// Remove the given tab from the control.
        /// </summary>
        /// <param name="control">Tab to remove.</param>
		internal void RemoveTab(WhidbeyTabPage tab)
		{
            // Validate
			if( ( tab == null ) ||
				( tab == null ) )
				return;

            // Remove label/from panel
			this.Controls.Remove( tab.Label );
			this.panelContainer.Controls.Remove( tab );
		}

        /// <summary>
        /// Remove all tabs from the control.
        /// </summary>
        internal void ClearTabs()
        {
            // I'm lazy :)
            while( m_aControls.Count > 0 )
                m_aControls.RemoveAt( 0 );
        }

        #endregion
		
		#endregion
    }
}
