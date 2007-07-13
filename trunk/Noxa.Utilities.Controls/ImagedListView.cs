// ----------------------------------------------------------------------------
// Shared Utility Library
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing;

namespace Noxa.Utilities.Controls
{
	public class ImagedListView : DoubleBufferedListView
	{
		public ImagedListView()
		{
			this.ExtendedStyle |= ListViewExtendedStyle.SubItemImages;
		}

		private enum LVIF
		{
			LVIF_TEXT = 0x0001,
			LVIF_IMAGE = 0x0002,
		}

		[StructLayoutAttribute( LayoutKind.Sequential )]
		private struct LV_ITEM
		{
			public uint mask;
			public int iItem;
			public int iSubItem;
			public uint state;
			public uint stateMask;
			public StringBuilder pszText;
			public int cchTextMax;
			public int iImage;
			public IntPtr lParam;
		}

		[DllImport( "user32.dll", CharSet = CharSet.Auto )]
		private static extern bool SendMessage( IntPtr handle, int msg, int wparam, ref LV_ITEM lparam );

		public enum ItemDisplay
		{
			Image,
			Text,
			ImageAndText,
		}

		public void SetItemDisplay( int row, int column, ItemDisplay displayMode )
		{
			this.SetItemDisplay( row, column, displayMode, 0 );
		}

		private string GetItemText( int row, int column )
		{
			LV_ITEM item = new LV_ITEM();
			item.iSubItem = column;
			item.pszText = new StringBuilder( 512 );
			item.cchTextMax = 512;
			SendMessage( this.Handle, ( int )LVM.LVM_GETITEMTEXT, row, ref item );
			return item.pszText.ToString();
		}

		public void SetItemDisplay( int row, int column, ItemDisplay displayMode, int imageIndex )
		{
			LV_ITEM item = new LV_ITEM();
			item.iItem = row;
			item.iSubItem = column;
			switch( displayMode )
			{
				case ItemDisplay.Text:
					item.mask = ( uint )LVIF.LVIF_TEXT;
					item.pszText = new StringBuilder( this.GetItemText( row, column ) );
					break;
				case ItemDisplay.Image:
					item.mask = ( uint )LVIF.LVIF_IMAGE;
					break;
				case ItemDisplay.ImageAndText:
					item.mask = ( uint )( LVIF.LVIF_IMAGE | LVIF.LVIF_TEXT );
					item.pszText = new StringBuilder( this.GetItemText( row, column ) );
					break;
			}
			item.iImage = imageIndex;
			SendMessage( this.Handle, ( int )LVM.LVM_SETITEM, 0, ref item );
		}
	}
}
