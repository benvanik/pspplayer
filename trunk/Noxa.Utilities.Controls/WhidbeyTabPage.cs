using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Noxa.Utilities.Controls
{
	#region TabState

	/// <summary>
	/// State for tab items.
	/// </summary>
	internal enum TabState
	{
		/// <summary>
		/// Normal state.
		/// </summary>
		Normal,

		/// <summary>
		/// Mouse or cursor is hovered over this item.
		/// </summary>
		Hovered,

		/// <summary>
		/// Item is selected.
		/// </summary>
		Selected,

		/// <summary>
		/// Item is disabled.
		/// </summary>
		Disabled
	}

	#endregion

	/// <summary>
	/// Tab page entry in <see cref="WhidbeyTabControl"/> instances.
	/// </summary>
	[ToolboxItem( false )]
	[TypeConverter( typeof( WhidbeyTabPageConverter ) )]
	public partial class WhidbeyTabPage : Panel
    {
        #region Data

        private     string                          m_szName			= "Unnamed";
		private		Rectangle						m_nBounds			= new Rectangle();
		private		TabState						m_nState			= TabState.Normal;
		private		Label							m_oLabel;
		
		private event EventHandler m_eShowing;
		private event EventHandler m_eHiding;
		
		#endregion

		#region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public WhidbeyTabPage()
            : base()
        {
            InitializeComponent();

			// If we don't do this then the control will be transparent
			// - Maybe this is a bug?
			this.BackColor = Color.FromArgb( 243, 241, 230 );

			this.DoubleBuffered = true;
		}

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Name for item in tab listing.</param>
        public WhidbeyTabPage(string name)
            : this()
        {
			if( name == null )
				throw new ArgumentNullException( "name" );
            m_szName = name;
        }
		
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="name">Name for item in tab listing.</param>
		/// <param name="enabled"><see langword="true"/> to set enabled, <see langword="false"/> to set disabled.</param>
		public WhidbeyTabPage(string name, bool enabled)
			: this( name )
		{
			if( enabled == false )
				m_nState = TabState.Disabled;
		}

		#endregion

		#region Events

		/// <summary>
		/// Fired right before the control is shown (gains tab focus).
		/// </summary>
		public event EventHandler Showing
		{
			add
			{
				m_eShowing += value;
			}
			remove
			{
				m_eShowing -= value;
			}
		}

		/// <summary>
		/// Fired right before the control is hidden (loses tab focus).
		/// </summary>
		public event EventHandler Hiding
		{
			add
			{
				m_eHiding += value;
			}
			remove
			{
				m_eHiding -= value;
			}
		}

		#endregion

		#region Accessors

        #region Internal

		/// <summary>
		/// Get or set the bounds for tab item.
		/// </summary>
		internal Rectangle ItemBounds
		{
			get
			{
				return ( m_nBounds );
			}
			set
			{
				m_nBounds = value;
			}
		}

		/// <summary>
		/// Get or set the state of item.
		/// </summary>
		internal TabState State
		{
			get
			{
				return ( m_nState );
			}
			set
			{
				m_nState = value;
			}
		}

		/// <summary>
		/// Get the label instance displayed on item.
		/// </summary>
		internal Label Label
		{
			get
			{
				return ( m_oLabel );
			}
			set
			{
				m_oLabel = value;
			}
		}

        #endregion

		/// <summary>
		/// Get the name of this page as displayed in the item list.
		/// </summary>
		[Browsable( true )]
		//[DesignerSerializationVisibility( DesignerSerializationVisibility.Visible )]
		[Bindable( true )]
		[EditorBrowsable( EditorBrowsableState.Always )]
		public override string Text
        {
            get
            {
                return( m_szName );
            }
			set
			{
				m_szName = value;

				// Maybe this shouldn't be here?
				if( this.Label != null )
					this.Label.Text = value;
			}
        }

		#endregion

		#region Methods
		
		#endregion

		#region Internal methods

		/// <summary>
		/// Called by parent to call show or hide.
		/// </summary>
		/// <param name="visible">State going into.</param>
		internal void ShowNotifty(bool visible)
		{
			if( visible == true )
			{
				EventHandler ev = this.m_eShowing;
				if( ev != null )
					ev( this, EventArgs.Empty );
			}
			else
			{
				EventHandler ev = this.m_eHiding;
				if( ev != null )
					ev( this, EventArgs.Empty );
			}
		}

		/// <summary>
		/// Fired when <see cref="WhidbeyTabPage.Enabled"/> changes.
		/// </summary>
		/// <param name="e">Ignore.</param>
		protected override void OnEnabledChanged(EventArgs e)
		{
			// Go down
			base.OnEnabledChanged( e );

			// Let parent handle logic (this is ugly I know :)
			if( ( this.Parent != null ) &&
				( this.Parent.Parent is WhidbeyTabControl ) )
				( this.Parent.Parent as WhidbeyTabControl ).SetTabEnabled( this );
		}
		
		#endregion
    }
}
