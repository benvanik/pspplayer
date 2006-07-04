using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Noxa.Utilities.Controls
{
	[Flags]
	public enum ListViewStyle
	{
		Icon = 0x0000,
		Report = 0x0001,
		SmallIcon = 0x0002,
		List = 0x0003,
		TypeMask = 0x0003,
		SingleSelect = 0x0004,
		ShowSelectionAlways = 0x0008,
		SortAscending = 0x0010,
		SortDescending = 0x0020,
		ShareImageLists = 0x0040,
		NoLabelWrap = 0x0080,
		AutoArrange = 0x0100,
		EditLabels = 0x0200,
		OwnerData = 0x1000,
		NoScroll = 0x2000,
		TypeStyleMask = 0xfc00,
		AlignTop = 0x0000,
		AlignLeft = 0x0800,
		AlignMask = 0x0c00,
		OwnerDrawFixed = 0x0400,
		NoColumnHeader = 0x4000,
		NoSortHeader = 0x8000
	} 

	[Flags]
	public enum ListViewExtendedStyle
	{
		GridLines = 0x00000001,
		SubItemImages = 0x00000002,
		Checkboxes = 0x00000004,
		TrackSelect = 0x00000008,
		HeaderDragDrop = 0x00000010,
		FullRowSelect = 0x00000020,
		OneClickActivate = 0x00000040,
		TwoClickActivate = 0x00000080,
		FlatSB = 0x00000100,
		Regional = 0x00000200,
		InfoTip = 0x00000400,
		UnderlineHot = 0x00000800,
		UnderlineCold = 0x00001000,
		MultiWorkAreas = 0x00002000,
		LabelTip = 0x00004000,
		BorderSelect = 0x00008000,
		DoubleBuffer = 0x00010000,
		HideLabels = 0x00020000,
		SingleRow = 0x00040000,
		SnapToGrid = 0x00080000,
		SimpleSelect = 0x00100000
	}

	public class DoubleBufferedListView : ListView
	{
		private enum LVM
		{
			LVM_FIRST = 0x1000,
			LVM_SETEXTENDEDLISTVIEWSTYLE = ( LVM_FIRST + 54 ),
			LVM_GETEXTENDEDLISTVIEWSTYLE = ( LVM_FIRST + 55 ),
		}

		[DllImport( "user32.dll", CharSet = CharSet.Auto )]
		private static extern int SendMessage( IntPtr handle, int messg, int wparam, int lparam );

		public DoubleBufferedListView()
		{
		}

		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams myParams = base.CreateParams;
				myParams.Style |= ( int )ListViewStyle.ShareImageLists | ( int )ControlStyles.OptimizedDoubleBuffer;
				return myParams;
			}
		}

		protected override void OnHandleCreated( EventArgs e )
		{
			base.OnHandleCreated( e );

			this.ExtendedStyle = ListViewExtendedStyle.DoubleBuffer | ListViewExtendedStyle.BorderSelect;
			this.Refresh();
			this.Update();
		}
		
		public ListViewExtendedStyle ExtendedStyle
		{
			get
			{
				return ( ListViewExtendedStyle )SendMessage( this.Handle, ( int )LVM.LVM_GETEXTENDEDLISTVIEWSTYLE, 0, 0 );
			}
			set
			{
				ListViewExtendedStyle styles = this.ExtendedStyle;
				styles |= value;
				SendMessage( this.Handle, ( int )LVM.LVM_SETEXTENDEDLISTVIEWSTYLE, 0, ( int )styles );
			}
		}
	}
}
