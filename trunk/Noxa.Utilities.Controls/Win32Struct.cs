using System;
using System.Runtime.InteropServices;

/// <summary>
/// Win32 structures
/// </summary>
namespace Noxa.Utilities.Controls
{
	/// <summary>
	/// RECT structure
	/// </summary>
	[StructLayout( LayoutKind.Sequential )]
	internal struct RECT
	{
		public int left;
		public int top;
		public int right;
		public int bottom;
	}

	/// <summary>
	/// Base notify message header
	/// </summary>
	[StructLayout( LayoutKind.Sequential )]
	internal struct NMHDR
	{
		public IntPtr hwndFrom;
		public int idFrom;
		public int code;
	}

	/// <summary>
	/// Standard notify message header
	/// </summary>
	[StructLayout( LayoutKind.Sequential )]
	internal struct NMHEADER
	{
		public NMHDR hdr;
		public int iItem;
		public int iButton;
		public IntPtr pitem;
	}

	/// <summary>
	/// Custom draw notify message
	/// </summary>
	[StructLayout( LayoutKind.Sequential )]
	internal struct NMCUSTOMDRAW
	{
		public NMHDR hdr;
		public int dwDrawStage;
		public IntPtr hdc;
		public RECT rc;
		public int dwItemSpec;
		public int uItemState;
		public IntPtr lItemlParam;
	}

	/// <summary>
	/// ListView specialized custom draw message 
	/// </summary>
	[StructLayout( LayoutKind.Sequential )]
	internal struct NMLVCUSTOMDRAW
	{
		public NMCUSTOMDRAW nmcd;
		public int clrText;
		public int clrTextBk;
		public int iSubItem;
		public uint dwItemType;
		public int clrFace;
		public int iIconEffect;
		public int iIconPhase;
		public int iPartId;
		public int iStateId;
		public RECT rcText;
		public uint uAlign;
	}

	/// <summary>
	/// ListView item data
	/// </summary>
	[StructLayout( LayoutKind.Sequential )]
	internal struct LVITEM
	{
		public W32_LVIF mask;
		public int iItem;
		public int iSubItem;
		public uint state;
		public uint stateMask;
		public String pszText;
		public int cchTextMax;
		public int iImage;
		public int lParam;
		public int iIndent;
	}

	/// <summary>
	/// Header item data
	/// </summary>
	[StructLayout( LayoutKind.Sequential )]
	internal struct HDITEM
	{
		public W32_HDI mask;
		public int cxy;
		public String pszText;
		public IntPtr hbm;
		public int cchTextMax;
		public W32_HDF fmt;
		public int lParam;
		public int iImage;
		public int iOrder;
		public uint type;
		public IntPtr pvFilter;
	}

	/// <summary>
	/// Header hittest information
	/// </summary>
	[StructLayout( LayoutKind.Sequential )]
	internal struct HDHITTESTINFO
	{
		public long x;
		public long y;
		public uint flags;
		public int iItem;
	}

	/// <summary>
	/// Structure for header layout
	/// </summary>
	[StructLayout( LayoutKind.Sequential )]
	internal struct HDLAYOUT
	{
		public IntPtr prc;   // RECT*
		public IntPtr pwpos; // WINDOWPOS*
	}

	/// <summary>
	/// Header filter text data
	/// </summary>
	[StructLayout( LayoutKind.Sequential )]
	internal struct HDTEXTFILTER
	{
		public String pszText;
		public int cchTextMax;
	}

	/// <summary>
	/// Window position structure
	/// </summary>
	[StructLayout( LayoutKind.Sequential )]
	internal struct WINDOWPOS
	{
		IntPtr hwnd;
		IntPtr hwndInsertAfter;
		int x;
		int y;
		int cx;
		int cy;
		uint flags;
	}
}
