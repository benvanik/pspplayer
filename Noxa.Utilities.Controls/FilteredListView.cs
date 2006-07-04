using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Globalization;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace Noxa.Utilities.Controls
{
	/// <summary>
	/// Type of comparison requested by the filter
	/// </summary>
	public enum LVFFilterType
	{
		Equal,
		NotEqual,
		Greater,
		GreaterEqual,
		Less,
		LessEqual
	}

	public enum ColumnHeaderDataType
	{
		String,
		Number,
		Date,
		Custom
	}

	public class TypedColumnHeader : ColumnHeader
	{
		private ColumnHeaderDataType _dataType = ColumnHeaderDataType.String;

		public TypedColumnHeader()
		{
		}

		public TypedColumnHeader( ColumnHeaderDataType dataType )
		{
			_dataType = dataType;
		}

		public ColumnHeaderDataType DataType
		{
			get
			{
				return _dataType;
			}
			set
			{
				_dataType = value;
			}
		}

		public virtual object GetData( ListViewItem item )
		{
			return item.Text;
		}

		public virtual int Compare( object x, object y, bool ignoreCase )
		{
			return this.CompareData( this.GetData( ( ListViewItem )x ), this.GetData( ( ListViewItem )y ), ignoreCase );
		}

		public virtual int CompareData( object x, object y, bool ignoreCase )
		{
			return 0;
		}
	}

	/// <summary>
	/// This is a specialization of a ListView control that defaults to
	/// details mode and includes the ability to filter data on column
	/// headers and to specify column data sort comparison type. 
	/// </summary>
	public class FilteredListView : DoubleBufferedListView
	{
		#region Private class data

		private bool _showFilters = false;              // Display column filters
		
		private int _sortColumn = 0;                  // Sort column
		private bool _sortOrderAscending = true;               // Sort ascending/descending
		private ColumnHeaderDataType srt_datype = ColumnHeaderDataType.String; // Sort data type comparison
		
		private bool _ignoreCase = true;              // Ignore case on filter
		
		private Color _shadeColor = Color.WhiteSmoke;   // Shade color
		private bool _shadeColumns = true;               // Display shade color

		private List<LVFFilter> _filters = new List<LVFFilter>();    // Array of LVFFliters active
		private List<ListViewItem> _shownItems = new List<ListViewItem>();    // Filtered out items
		private List<ListViewItem> _hiddenItems = new List<ListViewItem>();    // Held out items

		private int _activeColumn = 0;                  // Filter button column

		/// <summary>
		/// Instance pointer to the header control interface
		/// </summary>
		private FilteredListViewHeader _headerControl = null;

		/// <summary>
		/// Filter data structure for individual column data.
		/// </summary>
		[StructLayout( LayoutKind.Sequential )]
		internal struct LVFFilter
		{
			internal int Column;
			internal LVFFilterType Compare;
			internal ColumnHeaderDataType Type;
			internal string Text;
			internal object Data;
		}

		#endregion

		private bool _suppressUpdate = false;

		public bool SuppressUpdate
		{
			get
			{
				return _suppressUpdate;
			}
			set
			{
				_suppressUpdate = value;
				this.FilterUpdate();
			}
		}

		public void AddItem( ListViewItem item )
		{
			this.Items.Add( item );
			if( _suppressUpdate == false )
				this.FilterUpdate();
		}

		public void RemoveItem( ListViewItem item )
		{
			if( this.Items.Contains( item ) == true )
				this.Items.Remove( item );
			if( _shownItems.Contains( item ) == true )
				_shownItems.Remove( item );
			if( _hiddenItems.Contains( item ) == true )
				_hiddenItems.Remove( item );
		}

		public void ClearItems()
		{
			this.Items.Clear();
			_shownItems.Clear();
			_hiddenItems.Clear();
		}

		public void UpdateFilters()
		{
			this.FilterUpdate();
		}

		#region Windows API interfaces
		[DllImport( "user32.dll" )]
		internal static extern int SendMessage( IntPtr hWnd, W32_LVM msg, int wParam, int lParam );
		[DllImport( "user32.dll" )]
		internal static extern int SendMessage( IntPtr hWnd, W32_LVM msg, int wParam, ref LVITEM hditem );
		[DllImport( "user32.dll" )]
		internal static extern int SendMessage( IntPtr hWnd, W32_LVM msg, int wParam, ref RECT hditem );
		#endregion

		#region Constructor and Dispose methods

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// Constructor, nothing done here but InitializeComponent as usual.
		/// </summary>
		public FilteredListView()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">True if really disposing?</param>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if( components != null )
					components.Dispose();
			}
			base.Dispose( disposing );
		}

		#endregion

		#region Component Designer generated code

		private System.Windows.Forms.ContextMenuStrip filterContextMenuStrip;
		private System.Windows.Forms.ToolStripMenuItem clearFilterToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
		private System.Windows.Forms.ToolStripMenuItem ignoreCaseToolStripMenuItem;

		/// <summary>
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// NOTE: We have modified this method to include the context
		/// menu and items creation as if this was a form control.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new Container();

			// 
			// ListViewFilter
			// 
			this.Name = "ListViewFilter";
			this.View = View.Details;
			this.ListViewItemSorter = new FilteredListViewSorter( this );
			this.filterContextMenuStrip = new System.Windows.Forms.ContextMenuStrip( this.components );
			this.clearFilterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
			this.ignoreCaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();

			filterContextMenuStrip.SuspendLayout();

			// 
			// filterContextMenuStrip
			// 
			this.filterContextMenuStrip.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.clearFilterToolStripMenuItem,
            this.toolStripMenuItem3,
            this.ignoreCaseToolStripMenuItem} );
			this.filterContextMenuStrip.Name = "filterContextMenuStrip";
			this.filterContextMenuStrip.Size = new System.Drawing.Size( 153, 76 );
			this.filterContextMenuStrip.Opening += new CancelEventHandler( FilterContextMenuStripOpening );
			// 
			// clearFilterToolStripMenuItem
			// 
			this.clearFilterToolStripMenuItem.Name = "clearFilterToolStripMenuItem";
			this.clearFilterToolStripMenuItem.Size = new System.Drawing.Size( 152, 22 );
			this.clearFilterToolStripMenuItem.Text = "&Clear Filter";
			this.clearFilterToolStripMenuItem.Click += new EventHandler( ClearFilterToolStripMenuItemClick );
			this.clearFilterToolStripMenuItem.Font = new Font( this.clearFilterToolStripMenuItem.Font, FontStyle.Bold );
			// 
			// toolStripMenuItem3
			// 
			this.toolStripMenuItem3.Name = "toolStripMenuItem3";
			this.toolStripMenuItem3.Size = new System.Drawing.Size( 149, 6 );
			// 
			// ignoreCaseToolStripMenuItem
			// 
			this.ignoreCaseToolStripMenuItem.Name = "ignoreCaseToolStripMenuItem";
			this.ignoreCaseToolStripMenuItem.Size = new System.Drawing.Size( 152, 22 );
			this.ignoreCaseToolStripMenuItem.Text = "&Ignore Case";
			this.ignoreCaseToolStripMenuItem.Click += new EventHandler( IgnoreCaseToolStripMenuItemClick );

			this.filterContextMenuStrip.ResumeLayout( false );
		}

		#endregion

		#region Overridden System.Windows.Forms.Control methods

		/// <summary>
		/// When the handle changes we need to recreate the Filter
		/// Header instance.  The previous (if any) header instance
		/// was tied to a handle that is now gone.
		/// </summary>
		/// <param name="e">Event</param>
		protected override void OnHandleCreated( System.EventArgs e )
		{
			base.OnHandleCreated( e );

			// now that we have a handle, create the HeaderControl instance
			_headerControl = new FilteredListViewHeader( this, _showFilters,
			  _sortColumn, _sortOrderAscending );

			SendMessage( this.Handle, W32_LVM.LVM_SETSELECTEDCOLUMN, _sortColumn, 0 );
			SendMessage( this.Handle, W32_LVM.LVM_UPDATE, 0, 0 );
		}

		/// <summary>
		/// When our handle is removed we give the hdr_contrl a little
		/// help by telling him now is a good time to release its handle.
		/// From now on, we do not have a hdr_contrl instance active
		/// so we set hdr_contrl to null and don't use it until reset.
		/// </summary>
		/// <param name="e">Event</param>
		protected override void OnHandleDestroyed( System.EventArgs e )
		{
			// free and then delete the ListViewFilterHeader control
			if( _headerControl != null )
			{
				_headerControl.ReleaseHandle();
				_headerControl = null;
			}

			// do this AFTER we tell the ListViewFilterHeader to release
			base.OnHandleDestroyed( e );
		}

		/// <summary>
		/// Only three windows messages are trapped here: WM_ERASEBKGND 
		/// since the OnPaintBackground doesn't work for the ListView,
		/// WM_NOTIFY so that we can get all the ListViewFilterHeader
		/// notificiations, and OCM_NOTIFY for our own notifications.
		/// </summary>
		/// <param name="m">Message</param>
		protected override void WndProc( ref System.Windows.Forms.Message m )
		{
			base.WndProc( ref m );

			// determine if we want to process this message type
			switch( m.Msg )
			{
				// notify messages can come from the header and are used 
				// for column sorting and item filtering if wanted
				case ( int )W32_WM.WM_NOTIFY:

					// get the notify message header from this message LParam
					NMHEADER h1 = ( NMHEADER )m.GetLParam( typeof( NMHEADER ) );

					// process messages ONLY from our header control
					if( ( _headerControl != null ) && ( h1.hdr.hwndFrom == _headerControl.Handle ) )
						NotifyHeaderMessage( h1 );

					break;

				// internal ListView notify messages are reflected to us via OCM
				case ( int )W32_OCM.OCM_NOTIFY:

					// get the notify message header from this message LParam
					NMHEADER h2 = ( NMHEADER )m.GetLParam( typeof( NMHEADER ) );

					// process ONLY messages that are to this ListView control
					if( h2.hdr.hwndFrom == Handle )
					{

						// process only specific ListView notify messages
						switch( h2.hdr.code )
						{

							// draw message, we return a result for the drawstate
							case ( int )W32_NM.NM_CUSTOMDRAW:
								//NMLVCUSTOMDRAW d = ( NMLVCUSTOMDRAW )m.GetLParam( typeof( NMLVCUSTOMDRAW ) );
								//m.Result = ( IntPtr )NotifyCustomDraw( ref d );
								//Marshal.StructureToPtr( d, m.LParam, true );
								break;
						}
					}
					break;
			}
		}

		#endregion

		#region Private methods

		/// <summary>
		/// NM_CUSTOMDRAW processing for the ListView control.  Here all
		/// we really want to do is redraw the shaded background for the
		/// column that is currently sorted, let the base do the work.
		/// </summary>
		/// <param name="d">NMLVCUSTOMDRAW to process</param>
		/// <returns>Return the result of the draw stage</returns>
		private W32_CDRF NotifyCustomDraw( ref NMLVCUSTOMDRAW d )
		{

			// determine the draw stage and set return value
			switch( d.nmcd.dwDrawStage )
			{

				// first request, ask for each item notification if we shade columns
				case ( int )W32_CDDS.CDDS_PREPAINT:
					if( _shadeColumns && ( this.View == View.Details ) )
						return W32_CDRF.CDRF_NOTIFYITEMDRAW;
					break;

				// next request, ask for each sub item notification for this item
				case ( int )W32_CDDS.CDDS_ITEMPREPAINT:
					return W32_CDRF.CDRF_NOTIFYSUBITEMDRAW;

				// here is the real work, and it is simply ensuring that the
				// correct backcolor is set for the shaded column.  we let the
				// regular windows control drawing do the real work.
				case ( int )W32_CDDS.CDDS_SUBITEMPREPAINT:

					// ensure that this is a valid item/subitem request
					if( d.nmcd.dwItemSpec < this.Items.Count )
					{
						// get a reference to the item to be rendered
						ListViewItem i = this.Items[ d.nmcd.dwItemSpec ];

						// is this for a a base item set it's backcolor
						if( d.iSubItem == 0 )
						{
							//d.clrTextBk = ( d.iSubItem == _sortColumn )
							//  ? _shadeColor.ToArgb() : this.BackColor.ToArgb();
							i.BackColor = ( d.iSubItem == _sortColumn )
							  ? _shadeColor : this.BackColor;
						}
						  // ensure that the subitem exits before changing it
						else if( ( d.iSubItem < i.SubItems.Count ) &&
						  ( i.SubItems[ d.iSubItem ] != null ) )
							i.SubItems[ d.iSubItem ].BackColor =
							  ( d.iSubItem == _sortColumn ) ? _shadeColor : this.BackColor;
					}
					break;

				case (int)W32_CDDS.CDDS_SUBITEMPOSTPAINT:
					break;
			}

			// let default drawing do the actual rendering
			return W32_CDRF.CDRF_DODEFAULT | W32_CDRF.CDRF_NOTIFYPOSTPAINT;
		}


		/// <summary>
		/// When the header control sends a notification we need to 
		/// process the click to sort, button click for options menu,
		/// and filter change to update the items currently visible.
		/// </summary>
		/// <param name="h">NMHEADER</param>
		private void NotifyHeaderMessage( NMHEADER h )
		{

			// process only specific header notification messages
			switch( h.hdr.code )
			{

				// a header column was clicked, do the sort
				case ( int )W32_HDN.HDN_ITEMCLICKA:
				case ( int )W32_HDN.HDN_ITEMCLICKW:
					_headerControl.SortColumn = _sortColumn = h.iItem;
					_sortOrderAscending = _headerControl.SortOrder;
					srt_datype = _headerControl.DataType[ _sortColumn ];
					this.Sort();
					SendMessage( this.Handle, W32_LVM.LVM_SETSELECTEDCOLUMN, _sortColumn, 0 );
					SendMessage( this.Handle, W32_LVM.LVM_UPDATE, 0, 0 );
					break;

				// a filter button was clicked display the popup menu
				// to handle setting filter options for the column
				case ( int )W32_HDN.HDN_FILTERBTNCLICK:
					_activeColumn = h.iItem;
					this.filterContextMenuStrip.Show( this, PointToClient( MousePosition ) );
					break;

				// a filter content changed, update the items collection
				case ( int )W32_HDN.HDN_FILTERCHANGE:

					// if this is for item -1 then this is a clear all filters
					if( h.iItem < 0 ) _filters.Clear();

				   // if we are filtered this is a real filter data change
					else if( _showFilters ) FilterBuild( h.iItem );

					// update the items array with new filters applied
					FilterUpdate();
					break;

			}
		}


		/// <summary>
		/// Create a new filter entry for the given column.  This
		/// will replace/add a LVFFilter instance for the column
		/// to the itm_filtrs array for use by FilterUpdate().
		/// </summary>
		/// <param name="c">Column number</param>
		private void FilterBuild( int c )
		{
			// if this column exists in the filters array remove it
			foreach( LVFFilter f in _filters )
				if( f.Column == c )
				{
					_filters.Remove( f );
					break;
				}

			// get the filter text for this column from the header
			string s = _headerControl.Filter[ c ].Trim();

			// if there is any size to it then create a new filter
			if( s.Length > 0 )
			{

				// create a new filter object for this column
				LVFFilter f = new LVFFilter();
				f.Column = c;
				f.Text = s;
				f.Compare = LVFFilterType.Equal;
				f.Type = _headerControl.DataType[ c ];

				// check the first characters of the string to see
				// if this is not a default equality comparison
				switch( s[ 0 ] )
				{
					case '=':
						f.Text = s.Remove( 0, 1 );
						break;
					case '!':
						f.Compare = LVFFilterType.NotEqual;
						f.Text = s.Remove( 0, 1 );
						break;
					case '>':
						if( ( s.Length > 1 ) && ( s[ 1 ] == '=' ) )
						{
							f.Compare = LVFFilterType.GreaterEqual;
							f.Text = s.Remove( 0, 2 );
						}
						else
						{
							f.Compare = LVFFilterType.Greater;
							f.Text = s.Remove( 0, 1 );
						}
						break;
					case '<':
						if( ( s.Length > 1 ) && ( s[ 1 ] == '=' ) )
						{
							f.Compare = LVFFilterType.LessEqual;
							f.Text = s.Remove( 0, 2 );
						}
						else
						{
							f.Compare = LVFFilterType.Less;
							f.Text = s.Remove( 0, 1 );
						}
						break;
				}

				// add this to the array of filters
				_filters.Add( f );

			}
		}


		/// <summary>
		/// Check a compare result against a filter type.  We use
		/// the internal CompareData and ItemText methods to get
		/// the correct text and perform the comparison using the
		/// filter column datatype.  The result from CompareData
		/// is used to check against the Type of filtering applied.
		/// The f (LVFFilter) has been prepared with all the data
		/// about the filtering to be performed; column, text,
		/// datatype, and filtering type by FilterBuild().
		/// </summary>
		/// <param name="r">Result -1/0/1</param>
		/// <param name="f">LVFFilter</param>
		/// <returns></returns>
		private bool FilterCheck( ListViewItem i, LVFFilter f )
		{
			// get the result of the data type comparison
			TypedColumnHeader header = this.Columns[ f.Column ] as TypedColumnHeader;
			Debug.Assert( header != null );

			int r = header.CompareData( header.GetData( i ), f.Data, _ignoreCase );

			// compare the result against the filter comparison type
			// a true result means that it passed filtering and should
			// be left in the list of filtered items.
			switch( f.Compare )
			{
				case LVFFilterType.Equal:
					return ( r == 0 );
				case LVFFilterType.NotEqual:
					return ( r != 0 );
				case LVFFilterType.Greater:
					return ( r > 0 );
				case LVFFilterType.GreaterEqual:
					return ( r >= 0 );
				case LVFFilterType.Less:
					return ( r < 0 );
				case LVFFilterType.LessEqual:
					return ( r <= 0 );
				default:
					return ( r == 0 );
			}
		}

		/// <summary>
		/// Update the Items list since a filter has changed.  This
		/// is done whenever a filter text has changed, the filter
		/// options change (case or datatype).  This is how we remove
		/// items from the ListView.Items[] and put them into a private
		/// ArrayList of 'filtered' items.  The items moved to the
		/// itm_filtrd array will be put back when filters are changed
		/// or disabled.  There is a problem with this however.  The
		/// ListViewItemsCollection has a real hard time processing
		/// many additions to the list.  When a large number of items
		/// are added back in, it takes a long time.  If there is 
		/// another way to Add (I tried AddRange) ListViewItem(s) to
		/// the collection that is faster, I would really like to know.
		/// </summary>
		private void FilterUpdate()
		{
			if( this.Filtered == false )
				return;

			// halt all updates to the list while we are working
			this.BeginUpdate();

			// set a flag to say that no change has occurred
			// so that we don't unnecessarily sort the items
			bool c = false;

			// if there are any filters applied, process them.
			// Items removed are not put into the itm_filtrd 
			// list at this time since we would then have to
			// walk them again in the second loop.  instead,
			// they are added to a holding array (itm_holder)
			// which is later copied into the itm_filtrd list.
			// NOTE: we walk the Items list first, then check
			// each itm_filtrs against that one item.  That is
			// much faster than walking the Items list for each
			// itm_filtrs entry, since there is normally only
			// one or two columns filtered at any time.
			if( _filters.Count != 0 )
			{
				// Update all filter data
				for( int n = 0; n < _filters.Count; n++ )
				{
					LVFFilter filter = ( LVFFilter )_filters[ n ];

					TypedColumnHeader header = this.Columns[ filter.Column ] as TypedColumnHeader;
					Debug.Assert( header != null );

					object filterData = null;
					switch( header.DataType )
					{
						case ColumnHeaderDataType.String:
							filterData = filter.Text;
							break;
						case ColumnHeaderDataType.Number:
							long result = 0;
							string cleaned = filter.Text.Trim( ',', '.' ).ToLowerInvariant();
							cleaned = cleaned.Replace( "kb", "000" );
							cleaned = cleaned.Replace( "mb", "000000" );
							cleaned = cleaned.Replace( "gb", "000000000" );
							if( long.TryParse( cleaned, out result ) == true )
								filterData = result;
							else
								filterData = 0L;
							break;
						case ColumnHeaderDataType.Date:
							DateTime time = DateTime.MaxValue;
							if( filter.Text.ToLowerInvariant() == "today" )
								filterData = DateTime.Now.Date;
							else if( filter.Text.ToLowerInvariant() == "now" )
								filterData = DateTime.Now;
							else if( DateTime.TryParse( filter.Text, CultureInfo.CurrentUICulture, System.Globalization.DateTimeStyles.AllowWhiteSpaces, out time ) == true )
								filterData = time;
							else
								filterData = DateTime.MinValue;
							break;
						case ColumnHeaderDataType.Custom:
							filterData = filter.Text;
							break;
					}
					filter.Data = filterData;
					_filters[ n ] = filter;
				}

				// we will use a listviewitem instance here
				ListViewItem l;

				// prepare the held out items array to contain items
				// removed this pass so we don't check them twice
				_hiddenItems.Clear();

				// check each item in the Items list to see if it
				// should be removed to the filtered list
				for( int i = 0; i < this.Items.Count; ++i )
				{

					// get the item to check against the filters
					l = this.Items[ i ];

					// any filter failure removes from it from the
					// Items list and adds it to the holding list.
					// NOTE: the index (i) is decremented when we
					// remove an item since the list is compressed.
					foreach( LVFFilter f in _filters )
						if( !FilterCheck( l, f ) )
						{
							_hiddenItems.Add( l );
							this.Items.RemoveAt( i-- );
							c = true;
							break;
						}
				}

				// check each item in the currently filtered array
				// to see if it should be put back in the items now.
				// NOTE: this is the slow part when many items need
				// to be put back into the ListViewItemsCollection...
				List<ListViewItem> addBack = new List<ListViewItem>();
				for( int i = 0; i < _shownItems.Count; ++i )
				{

					// get the object as a listviewitem
					l = ( ListViewItem )_shownItems[ i ];

					// set no filter failure flag this item
					bool r = true;

					// all filters must pass, stop at one failure
					foreach( LVFFilter f in _filters )
						if( !FilterCheck( l, f ) )
						{
							r = false;
							break;
						}

					// if we did not fail any filter put it back in Items
					// and remove it from the filtered items: itm_filtrd.
					// NOTE: the index (i) is decremented when we
					// remove an item since the list is compressed.
					if( r )
					{
						addBack.Add( l );
						_shownItems.RemoveAt( i-- );
						c = true;
					}
				}
				this.Items.AddRange( addBack.ToArray() );

				// now add all the holder items into the filtered list
				// and empty the held out items array to remove reference
				_shownItems.AddRange( _hiddenItems );
				_hiddenItems.Clear();

			}

			  // no filters active put any filtered items back into
			// the items list and empty the filtered items array
			else
			{
				// set the do sort flag only if there are entries
				c = ( _shownItems.Count > 0 );

				// put all filtered items back and clear the list
				this.Items.AddRange( _shownItems.ToArray() );
				_shownItems.Clear();

			}

			// resort the items if the item content has changed
			if( c ) this.Sort();

			// ensure that updates are re-enabled
			this.EndUpdate();
		}

		void FilterMenuItemClicked()
		{
			// force a filter build on the specific column
			FilterBuild( _activeColumn );

			// follow with a filter update
			FilterUpdate();

			// set focus to ourself after menu action otherwise
			// focus is left in the filter edit itself...
			this.Focus();
		}

		void ClearFilterToolStripMenuItemClick( object sender, EventArgs e )
		{
			_headerControl.Filter[ _activeColumn ] = "";

			this.FilterMenuItemClicked();
		}

		void IgnoreCaseToolStripMenuItemClick( object sender, EventArgs e )
		{
			_ignoreCase = !_ignoreCase;

			this.FilterMenuItemClicked();
		}

		void FilterContextMenuStripOpening( object sender, CancelEventArgs e )
		{
			// set the checked state of the case insensitive
			this.ignoreCaseToolStripMenuItem.Checked = _ignoreCase;
		}

		#endregion

		#region Internal methods and data

		/// <summary>
		/// Return the Text of an item/subitem as a string.  This is
		/// done to simplify text retrieval in multiple places.  It
		/// would be nice if the ListViewItem itself had this...
		/// </summary>
		/// <param name="i">ListViewItem</param>
		/// <param name="c">Column number</param>
		/// <returns>Text or empty string</returns>
		internal string ItemText( ListViewItem i, int c )
		{
			// ensure we have an item to work with here...
			if( i != null )
			{

				// if column 0 return the text
				if( c == 0 ) return i.Text;

				// not 0, ensure that the subitem is valid and exists
				if( ( c < i.SubItems.Count ) && ( i.SubItems[ c ] != null ) )
					return i.SubItems[ c ].Text;

			}

			// not valid item/subitem return empty string
			return "";

		}

		#endregion

		#region Public properties

		/// <summary>
		/// Non-browsable properties
		/// </summary>

		[Description( "The ListViewFilterHeader instance for access to " +
		   "the Names, Sizes, DataType, and Filter entries by column." ),
		Browsable( false )]
		public FilteredListViewHeader Header
		{
			get
			{
				return _headerControl;
			}
		}


		[Description( "The ColumnHeaderDataType associated with the current column " +
		"that is being sorted." ), Browsable( false )]
		public ColumnHeaderDataType SortType
		{
			get
			{
				return srt_datype;
			}
		}

		/// <summary>
		/// These are all browsable and available in the property pages.
		/// NOTE: I do not know how to do two things: hide the Sorting
		/// property from the page since it causes all kinds of problems,
		/// and how to set a DefaultValue( Color.WhiteSmoke ) for the
		/// ShadeColor property...
		/// </summary>

		[Description( "In Details mode, the header will display filter controls." ),
		Category( "Behavior" ), DefaultValue( false )]
		public bool Filtered
		{
			get
			{
				return _showFilters;
			}
			set
			{
				if( _showFilters != value )
				{
					if( value == false )
					{
						for( int n = 0; n < _headerControl.Filter.Count; n++ )
							_headerControl.Filter[ n ] = "";
						this.FilterUpdate();
					}
					_showFilters = value;
					if( _headerControl != null )
						_headerControl.Filtered = _showFilters;
					this.Refresh();
				}
			}
		}


		[Description( "Treat filter strings case-insensitive for comparison." ),
		Category( "Behavior" ), DefaultValue( false )]
		public bool IgnoreCase
		{
			get
			{
				return _ignoreCase;
			}
			set
			{
				if( _ignoreCase != value )
				{
					_ignoreCase = value;
					FilterUpdate();
				}
			}
		}


		[Description( "In Details mode, the sorted column will be shaded." ),
		Category( "Appearance" ), DefaultValue( true )]
		public bool Shaded
		{
			get
			{
				return _shadeColumns;
			}
			set
			{
				if( _shadeColumns != value )
				{
					_shadeColumns = value;
					this.Refresh();
				}
			}
		}


		[Description( "Color of the shaded column in details mode" ),
		Category( "Appearance" )]
		public Color ShadeColor
		{
			get
			{
				return _shadeColor;
			}
			set
			{
				_shadeColor = value;
				this.Refresh();
			}
		}


		[Description( "The sorted column when in details mode." ),
		Category( "Behavior" ), DefaultValue( 0 )]
		public int SortColumn
		{
			get
			{
				return _sortColumn;
			}
			set
			{
				if( ( _sortColumn != value ) && ( value < this.Columns.Count ) &&
				  ( _headerControl != null ) )
				{
					_sortColumn = _headerControl.SortColumn = value;
					if( this.View == View.Details ) this.Sort();

					SendMessage( this.Handle, W32_LVM.LVM_SETSELECTEDCOLUMN, _sortColumn, 0 );
					SendMessage( this.Handle, W32_LVM.LVM_UPDATE, 0, 0 );
				}
			}
		}


		[Description( "The order of the current sort. True is ascending." ),
		Category( "Behavior" ), DefaultValue( false ), Browsable( false )]
		public bool SortOrder
		{
			get
			{
				return _sortOrderAscending;
			}
			set
			{
				if( ( _sortOrderAscending != value ) && ( _headerControl != null ) )
				{
					_sortOrderAscending = _headerControl.SortOrder = value;
					if( this.View == View.Details ) this.Sort();
				}
			}
		}


		#endregion
	}

	/// <summary>
	/// This class encapsulates the standard HeaderControl that is not
	/// currently implemented in the .NET Framework.  This is specific
	/// for use with a ListViewFilter control in this assembly.  The
	/// primary reason for this is to get access to the Win32 HDM_
	/// messages to get/set values that are not published by the
	/// System.Windows.Forms.ColumnHeader class in .NET.
	/// NOTE: This control works very well in XP since it uses the
	/// HDF_SORTUP/HDF_SORTDN flags to display the up/down icon. But
	/// you must have a manifest that states using CommCtrl version
	/// 6.0 or greater, since that is where it is implemented...
	/// </summary>
	public class FilteredListViewHeader : System.Windows.Forms.NativeWindow
	{
		#region Private class data

		readonly FilteredListView hdr_lstvew;                // owning listview control
		private bool hdr_filter = false;        // show filterbar in header
		private int hdr_column = -1;           // sort column
		private bool hdr_sorder = true;         // sort order
		private HDITEM hdr_hditem = new HDITEM(); // instance container

		#endregion

		#region Windows API interfaces
		[DllImport( "user32.dll" )]
		internal static extern IntPtr GetDlgItem( IntPtr hDlg, int nControlID );
		[DllImport( "user32.dll" )]
		internal static extern int GetWindowLong( IntPtr hWnd, W32_GWL flag );
		[DllImport( "user32.dll" )]
		internal static extern int SetWindowLong( IntPtr hWnd, W32_GWL flag, int dwNewLong );
		[DllImport( "user32.dll" )]
		internal static extern int SendMessage( IntPtr hWnd, W32_HDM msg, int wParam, int lParam );
		[DllImport( "user32.dll" )]
		internal static extern int SendMessage( IntPtr hWnd, W32_HDM msg, int wParam, ref HDITEM hditem );
		[DllImport( "user32.dll" )]
		internal static extern int SendMessage( IntPtr hWnd, W32_HDM msg, int wParam, ref RECT rect );
		#endregion

		#region Constructor

		/// <summary>
		/// Constructor.  Save the owning ListView control object and get our handle
		/// assigned from it's slave controls entry 0, without it no messages are sent.
		/// </summary>
		/// <param name="lv">Owner</param>
		public FilteredListViewHeader( FilteredListView listview, bool filtered,
		  int column, bool order )
		{
			// save the listview instance and set initial properties
			hdr_lstvew = listview;
			hdr_sorder = order;
			hdr_column = column;

			// set the handle to this control.  the first dialog item
			// for a listview is this header control...
			AssignHandle( GetDlgItem( hdr_lstvew.Handle, 0 ) );

			// create the collection properties
			DataType = new ColumnDataTypeCollection( this );
			Filter = new ColumnFilterCollection( this );
			Names = new ColumnNamesCollection( this );
			SizeInfo = new ColumnSizeInfoCollection( this );
			Alignment = new ColumnAlignmentCollection( this );

			// last set the filtered property VIA SET{} METHOD
			Filtered = filtered;
		}

		#endregion

		#region Overridden System.Windows.Forms.NativeWindow methods

		/// <summary>
		/// When the handle changes reset the filter style.
		/// </summary>
		protected override void OnHandleChange()
		{
			base.OnHandleChange();

			// reset the filter settings if there is a handle
			if( Handle != ( IntPtr )0 ) ChangeFiltered();

		}


		#endregion

		#region Private methods

		/// <summary>
		/// Set the window style to reflect the value of the Filtered
		/// property.  This is done when the Handle or property change.  
		/// </summary>
		private void ChangeFiltered()
		{

			// we need to set a new style value for this control to turn
			// on/off the HDS_FILTERBAR flag.  First get the style itself.
			const int HDS_FILTR = ( int )W32_HDS.HDS_FILTERBAR;
			int style = GetWindowLong( Handle, W32_GWL.GWL_STYLE );

			// now that we have the flag see if it is not what is desired
			if( ( ( style & HDS_FILTR ) != 0 ) != hdr_filter )
			{

				// set/reset the flag for the filterbar
				if( hdr_filter ) style |= HDS_FILTR;
				else style ^= HDS_FILTR;
				SetWindowLong( Handle, W32_GWL.GWL_STYLE, style );

				// now we have to resize this control.  we do this by sending
				// a set item message to column 0 to change it's size.  this
				// is a kludge but the invalidate and others just don't work.
				hdr_hditem.mask = W32_HDI.HDI_HEIGHT;
				SendMessage( Handle, W32_HDM.HDM_GETITEMW, 0, ref hdr_hditem );
				hdr_hditem.cxy += ( hdr_filter ) ? 1 : -1;
				SendMessage( Handle, W32_HDM.HDM_SETITEMW, 0, ref hdr_hditem );

			}

			// it can't hurt to set the filter timeout limit to .5 seconds
			SendMessage( Handle, W32_HDM.HDM_SETFILTERCHANGETIMEOUT, 0, 250 );

			// it is necessary to clear all filters which will cause a 
			// notification be sent to the ListView with -1 column.
			SendMessage( Handle, W32_HDM.HDM_CLEARFILTER, -1, 0 );

		}


		/// <summary>
		/// Change the column and order that is sorted.  If the column
		/// is the same as the current sort column, the order is 
		/// toggled.  If it is a different column then it always sets
		/// the order to ascending by default.
		/// </summary>
		/// <param name="Column"></param>
		private void ChangeSort( int c )
		{

			// save the new sort column
			hdr_column = c;

			// setup to walk the format setting for every column
			hdr_hditem.mask = W32_HDI.HDI_FORMAT;

			// turn off the sorted flag for all but the current column
			int i = SendMessage( Handle, W32_HDM.HDM_GETITEMCOUNT, 0, 0 );
			while( i-- > 0 )
			{

				// get the item information (HDITEM structure)
				SendMessage( Handle, W32_HDM.HDM_GETITEMW, i, ref hdr_hditem );

				// is this the column we are to flag as sorted
				if( i == hdr_column )
				{

					// set true when the current mode is down or none set yet
					hdr_sorder = ( ( hdr_hditem.fmt & W32_HDF.HDF_SORTUP ) == 0 );

					// set the new sort mode in the format
					hdr_hditem.fmt &= W32_HDF.HDF_NOSORT;
					hdr_hditem.fmt |= ( hdr_sorder )
					  ? W32_HDF.HDF_SORTUP : W32_HDF.HDF_SORTDOWN;

				}

				  // not the sort column, turn off any existing sort flag
				else if( ( hdr_hditem.fmt & W32_HDF.HDF_SORTED ) != 0 )
				{
					hdr_hditem.fmt &= W32_HDF.HDF_NOSORT;
				}

				  // not the column or any sort flag, skip the format change
				else continue;

				// a change is needed to this column format so set it
				SendMessage( Handle, W32_HDM.HDM_SETITEMW, i, ref hdr_hditem );

			}

			// finally invalidate the listview so that it redraws
			hdr_lstvew.Invalidate( hdr_lstvew.Bounds );

		}


		#endregion

		#region Properties

		/// <summary>
		/// Indexer to the alignment by column.
		/// </summary>
		public readonly ColumnAlignmentCollection Alignment;

		/// <summary>
		/// Indexer to the data types by column.
		/// </summary>
		public readonly ColumnDataTypeCollection DataType;

		/// <summary>
		/// Indexer to the column filter text.
		/// </summary>
		public readonly ColumnFilterCollection Filter;

		/// <summary>
		/// Indexer to the column name text.
		/// </summary>
		public readonly ColumnNamesCollection Names;

		/// <summary>
		/// Indexer to the column size (Width and Left as Height).
		/// </summary>
		public readonly ColumnSizeInfoCollection SizeInfo;

		/// <summary>
		/// When the Filtered property changes update the window style.
		/// </summary>
		public bool Filtered
		{
			get
			{
				return hdr_filter;
			}
			set
			{
				if( hdr_filter != value )
				{
					hdr_filter = value;
					ChangeFiltered();
				}
			}
		}


		/// <summary>
		/// The column number sorted.
		/// </summary>
		public int SortColumn
		{
			get
			{
				return hdr_column;
			}
			set
			{
				hdr_column = value;
				ChangeSort( hdr_column );
			}
		}


		/// <summary>
		/// Sort order (true is ascending).
		/// </summary>
		public bool SortOrder
		{
			get
			{
				return hdr_sorder;
			}
			set
			{
				if( hdr_sorder != value )
				{
					hdr_sorder = value;
					ChangeSort( hdr_column );
				}
			}
		}


		#endregion

		#region Collection classes for indexed column access

		/// <summary>
		/// This is an indexer to the LVDataType values for each column.
		/// The data is stored in the lParam value of the column.
		/// </summary>
		public class ColumnAlignmentCollection
		{
			readonly FilteredListViewHeader col_hdrctl = null;         // owning header control
			private HDITEM col_hditem = new HDITEM(); // HDITEM instance

			/// <summary>
			/// Constructor this must be given the header instance for access
			/// to the Handle property so that messages can be sent to it.
			/// </summary>
			/// <param name="header">HeaderControl</param>
			public ColumnAlignmentCollection( FilteredListViewHeader header )
			{
				col_hdrctl = header;
			}


			/// <summary>
			/// Indexer method to get/set the Alignment for the column.
			/// </summary>
			public HorizontalAlignment this[ int index ]
			{
				get
				{

					// ensure that this is a valid column
					if( index >= this.Count ) return HorizontalAlignment.Left;

					// get the current format for the column
					col_hditem.mask = W32_HDI.HDI_FORMAT;
					SendMessage( col_hdrctl.Handle, W32_HDM.HDM_GETITEMW, index, ref col_hditem );

					// return the current setting
					if( ( col_hditem.fmt & W32_HDF.HDF_CENTER ) != 0 )
						return HorizontalAlignment.Center;
					else if( ( col_hditem.fmt & W32_HDF.HDF_RIGHT ) != 0 )
						return HorizontalAlignment.Right;
					else return HorizontalAlignment.Left;

				}
				set
				{

					// ensure that this is a valid column
					if( index < this.Count )
					{

						// get the current format for the column
						col_hditem.mask = W32_HDI.HDI_FORMAT;
						SendMessage( col_hdrctl.Handle, W32_HDM.HDM_GETITEMW, index, ref col_hditem );

						// turn off any existing alignment values
						col_hditem.fmt &= W32_HDF.HDF_NOJUSTIFY;

						// turn on the correct alignment
						switch( value )
						{
							case HorizontalAlignment.Center:
								col_hditem.fmt |= W32_HDF.HDF_CENTER;
								break;
							case HorizontalAlignment.Right:
								col_hditem.fmt |= W32_HDF.HDF_RIGHT;
								break;
							default:
								col_hditem.fmt |= W32_HDF.HDF_LEFT;
								break;
						}

						// now update the column format
						SendMessage( col_hdrctl.Handle, W32_HDM.HDM_SETITEMW, index, ref col_hditem );

					}
				}
			}


			/// <summary>
			/// Return the number of columns in the header.
			/// </summary>
			public int Count
			{
				get
				{
					return SendMessage( col_hdrctl.Handle, W32_HDM.HDM_GETITEMCOUNT, 0, 0 );
				}
			}

		}


		/// <summary>
		/// This is an indexer to the LVDataType values for each column.
		/// The data is stored in the lParam value of the column.
		/// </summary>
		public class ColumnDataTypeCollection
		{
			readonly FilteredListViewHeader col_hdrctl = null;         // owning header control
			private HDITEM col_hditem = new HDITEM(); // HDITEM instance

			/// <summary>
			/// Constructor this must be given the header instance for access
			/// to the Handle property so that messages can be sent to it.
			/// </summary>
			/// <param name="header">HeaderControl</param>
			public ColumnDataTypeCollection( FilteredListViewHeader header )
			{
				col_hdrctl = header;
			}


			/// <summary>
			/// Indexer method to get/set the LVDataType for the column.
			/// </summary>
			public ColumnHeaderDataType this[ int index ]
			{
				get
				{

					// the lparam of the column header contains the datatype
					col_hditem.mask = W32_HDI.HDI_LPARAM;
					col_hditem.lParam = ( int )ColumnHeaderDataType.String;

					// if it is valid the lparam is updated and then returned
					SendMessage( col_hdrctl.Handle, W32_HDM.HDM_GETITEMW, index, ref col_hditem );
					return ( ColumnHeaderDataType )col_hditem.lParam;
				}
				set
				{

					// ensure that this is a valid column
					if( index < this.Count )
					{

						// simply set the new LVDataType in the lparam and pass it on
						col_hditem.mask = W32_HDI.HDI_LPARAM;
						col_hditem.lParam = ( int )value;
						SendMessage( col_hdrctl.Handle, W32_HDM.HDM_SETITEMW, index, ref col_hditem );
					}
				}
			}


			/// <summary>
			/// Return the number of columns in the header.
			/// </summary>
			public int Count
			{
				get
				{
					return SendMessage( col_hdrctl.Handle, W32_HDM.HDM_GETITEMCOUNT, 0, 0 );
				}
			}

		}


		/// <summary>
		/// This is an indexer to the text values for each column.
		/// </summary>
		public class ColumnNamesCollection
		{
			readonly FilteredListViewHeader col_hdrctl = null;         // owning header control
			private HDITEM col_hditem = new HDITEM(); // HDITEM instance

			/// <summary>
			/// Constructor this must be given the header instance for access
			/// to the Handle property so that messages can be sent to it.
			/// </summary>
			/// <param name="header">HeaderControl</param>
			public ColumnNamesCollection( FilteredListViewHeader header )
			{
				col_hdrctl = header;
			}


			/// <summary>
			/// Indexer method to get/set the text of the column.
			/// </summary>
			public string this[ int index ]
			{
				get
				{

					// set up to retrive the column header text
					col_hditem.mask = W32_HDI.HDI_TEXT;
					col_hditem.pszText = new string( new char[ 64 ] );
					col_hditem.cchTextMax = col_hditem.pszText.Length;

					// if successful the text has been retrieved and returned
					SendMessage( col_hdrctl.Handle, W32_HDM.HDM_GETITEMA, index, ref col_hditem );
					return col_hditem.pszText;
				}
				set
				{

					// this must be a valid column index
					if( index < this.Count )
					{

						// simply set the text and size in the structure and pass it on
						col_hditem.mask = W32_HDI.HDI_TEXT;
						col_hditem.pszText = value;
						col_hditem.cchTextMax = col_hditem.pszText.Length;
						SendMessage( col_hdrctl.Handle, W32_HDM.HDM_SETITEMA, index, ref col_hditem );
					}
				}
			}


			/// <summary>
			/// Return the number of columns in the header.
			/// </summary>
			public int Count
			{
				get
				{
					return SendMessage( col_hdrctl.Handle, W32_HDM.HDM_GETITEMCOUNT, 0, 0 );
				}
			}

		}


		/// <summary>
		/// This is an indexer to the filter values for each column.  This
		/// is the most complex access to the HDITEM data since the filter
		/// data must be get/set via the HDTEXTFILTER structure referenced
		/// in the HDITEM structure.  It is simple to Marshall this, but
		/// figuring that out in the first place took alot of effort.
		/// </summary>
		public class ColumnFilterCollection
		{
			readonly FilteredListViewHeader col_hdrctl = null;               // owning header control
			private HDITEM col_hditem = new HDITEM();       // HDITEM instance
			private HDTEXTFILTER col_txtflt = new HDTEXTFILTER(); // HDTEXTFILTER instance

			/// <summary>
			/// Constructor this must be given the header instance for access
			/// to the Handle property so that messages can be sent to it.
			/// </summary>
			/// <param name="header">HeaderControl</param>
			public ColumnFilterCollection( FilteredListViewHeader header )
			{
				col_hdrctl = header;
			}


			/// <summary>
			/// Indexer method to get/set the filter text for the column.
			/// </summary>
			public string this[ int index ]
			{
				get
				{

					// if the column is invalid return nothing
					if( index >= Count ) return "";

					// this is tricky since it involves marshalling pointers
					// to structures that are used as a reference in another
					// structure.  first initialize the receiving HDTEXTFILTER
					col_txtflt.pszText = new string( new char[ 64 ] );
					col_txtflt.cchTextMax = col_txtflt.pszText.Length;

					// set the HDITEM up to request the current filter content
					col_hditem.mask = W32_HDI.HDI_FILTER;
					col_hditem.type = ( uint )W32_HDFT.HDFT_ISSTRING;

					// marshall memory big enough to contain a HDTEXTFILTER 
					col_hditem.pvFilter = Marshal.AllocCoTaskMem(
					  Marshal.SizeOf( col_txtflt ) );

					// now copy the HDTEXTFILTER structure to the marshalled memory
					Marshal.StructureToPtr( col_txtflt, col_hditem.pvFilter, false );

					// retrieve the header filter string as non-wide string
					SendMessage( col_hdrctl.Handle, W32_HDM.HDM_GETITEMA, index, ref col_hditem );

					// un-marshall the memory back into the HDTEXTFILTER structure
					col_txtflt = ( HDTEXTFILTER )Marshal.PtrToStructure(
					  col_hditem.pvFilter, typeof( HDTEXTFILTER ) );

					// remember to free the marshalled IntPtr memory...
					Marshal.FreeCoTaskMem( col_hditem.pvFilter );

					// return the string in the text filter area
					return col_txtflt.pszText;

				}
				set
				{

					// ensure that the column exists before attempting this
					if( index < this.Count )
					{

						// this is just like the get{} except we don't have to
						// return anything and the message is HDM_SETITEMA. we
						// use the non-unicode methods for both the get and set.
						// reference the get{} method for marshalling details.
						col_txtflt.pszText = value;
						col_txtflt.cchTextMax = 64;
						col_hditem.mask = W32_HDI.HDI_FILTER;
						col_hditem.type = ( uint )W32_HDFT.HDFT_ISSTRING;
						col_hditem.pvFilter = Marshal.AllocCoTaskMem( Marshal.SizeOf( col_txtflt ) );
						Marshal.StructureToPtr( col_txtflt, col_hditem.pvFilter, false );
						SendMessage( col_hdrctl.Handle, W32_HDM.HDM_SETITEMA, index, ref col_hditem );
						Marshal.FreeCoTaskMem( col_hditem.pvFilter );
					}
				}
			}


			/// <summary>
			/// Return the number of columns in the header.
			/// </summary>
			public int Count
			{
				get
				{
					return SendMessage( col_hdrctl.Handle, W32_HDM.HDM_GETITEMCOUNT, 0, 0 );
				}
			}


		}


		/// <summary>
		/// This is an indexer to the READONLY Size values for a column.
		/// NOTE: The Size really contains the Width and Left position,
		/// the Left is sorted in the Height property of the Size class
		/// We do this because a Rectangle is not really necessary.
		/// </summary>
		public class ColumnSizeInfoCollection
		{
			readonly FilteredListViewHeader col_hdrctl = null;       // owning header control
			private RECT col_rectng = new RECT(); // HDITEM instance

			/// <summary>
			/// Constructor this must be given the header instance for access
			/// to the Handle property so that messages can be sent to it.
			/// </summary>
			/// <param name="header">HeaderControl</param>
			public ColumnSizeInfoCollection( FilteredListViewHeader header )
			{
				col_hdrctl = header;
			}


			/// <summary>
			/// Indexer method to get/set the Size for the column.
			/// </summary>
			public Size this[ int index ]
			{
				get
				{

					// if the column is valid get the rectangle
					if( index < Count )
					{
						SendMessage( col_hdrctl.Handle, W32_HDM.HDM_GETITEMRECT,
						  index, ref col_rectng );
						return new Size( ( col_rectng.right - col_rectng.left ),
						  col_rectng.left );
					}

					  // return null size
					else return new Size( 0, 0 );
				}
			}


			/// <summary>
			/// Return the number of columns in the header.
			/// </summary>
			public int Count
			{
				get
				{
					return SendMessage( col_hdrctl.Handle, W32_HDM.HDM_GETITEMCOUNT, 0, 0 );
				}
			}


		}


		#endregion
	}

	/// <summary>
	/// This class is used to compare ListViewItem entries by column
	/// in a particular manner for sorting the by owning ListView.
	/// The standard IComparer is extended with the Compare method
	/// </summary>
	internal class FilteredListViewSorter : IComparer
	{
		#region Private class data

		readonly FilteredListView cmp_lstvew = null; // owning listview

		#endregion

		#region Constructor

		/// <summary>
		/// Constructor, we need the instance of the ListViewFilter.
		/// </summary>
		/// <param name="listview">Owner</param>
		public FilteredListViewSorter( FilteredListView listview )
		{
			cmp_lstvew = listview;
		}


		#endregion

		#region Public methods

		/// <summary>
		/// Compare function for two given objects.  We use the
		/// internal ItemText and CompareData methods to do the
		/// work of getting the strings.  The listview also has
		/// the column, order, and datatype needed.
		/// </summary>
		/// <param name="o1">First object</param>
		/// <param name="o2">Second object</param>
		/// <returns>-1/0/1 result of comparison</returns>
		public int Compare( object o1, object o2 )
		{
			TypedColumnHeader column = cmp_lstvew.Columns[ cmp_lstvew.SortColumn ] as TypedColumnHeader;
			Debug.Assert( column != null );

			int result = column.Compare( o1, o2, true );
			
			// Return +/- result depending upon sort order
			return ( cmp_lstvew.SortOrder ) ? result : -result;
		}

		#endregion
	}
}