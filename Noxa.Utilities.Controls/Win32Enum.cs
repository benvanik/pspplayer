using System;

/// <summary>
/// Win32 enumerations, flags, codes, values
/// </summary>
namespace Noxa.Utilities.Controls
{
	/// <summary>
	/// Custom draw draw stage values
	/// </summary>
	internal enum W32_CDDS : int
	{
		CDDS_PREPAINT = 0x00000001,
		CDDS_POSTPAINT = 0x00000002,
		CDDS_PREERASE = 0x00000003,
		CDDS_POSTERASE = 0x00000004,
		CDDS_ITEM = 0x00010000,
		CDDS_ITEMPREPAINT = ( CDDS_ITEM | CDDS_PREPAINT ),
		CDDS_ITEMPOSTPAINT = ( CDDS_ITEM | CDDS_POSTPAINT ),
		CDDS_ITEMPREERASE = ( CDDS_ITEM | CDDS_PREERASE ),
		CDDS_ITEMPOSTERASE = ( CDDS_ITEM | CDDS_POSTERASE ),
		CDDS_SUBITEM = 0x00020000,
		CDDS_SUBITEMPREPAINT = ( CDDS_SUBITEM | CDDS_ITEMPREPAINT ),
		CDDS_SUBITEMPOSTPAINT = ( CDDS_SUBITEM | CDDS_ITEMPOSTPAINT ),
		CDDS_SUBITEMPREERASE = ( CDDS_SUBITEM | CDDS_ITEMPREERASE ),
		CDDS_SUBITEMPOSTERASE = ( CDDS_SUBITEM | CDDS_ITEMPOSTERASE ),
	}

	/// <summary>
	/// Custom draw state information
	/// </summary>
	internal enum W32_CDIS : int
	{
		CDIS_SELECTED = 0x0001,
		CDIS_GRAYED = 0x0002,
		CDIS_DISABLED = 0x0004,
		CDIS_CHECKED = 0x0008,
		CDIS_FOCUS = 0x0010,
		CDIS_DEFAULT = 0x0020,
		CDIS_HOT = 0x0040,
		CDIS_MARKED = 0x0080,
		CDIS_INDETERMINATE = 0x0100,
		CDIS_SHOWKEYBOARDCUES = 0x0200
	}

	/// <summary>
	/// Custom draw return values
	/// </summary>
	internal enum W32_CDRF : int
	{
		CDRF_DODEFAULT = 0x0000,
		CDRF_NEWFONT = 0x0002,
		CDRF_SKIPDEFAULT = 0x0004,
		CDRF_NOTIFYPOSTPAINT = 0x0010,
		CDRF_NOTIFYITEMDRAW = 0x0020,
		CDRF_NOTIFYSUBITEMDRAW = 0x0020,
		CDRF_NOTIFYPOSTERASE = 0x0040
	}

	/// <summary>
	/// GetWindowLong flags
	/// </summary>
	internal enum W32_GWL : int
	{
		GWL_WNDPROC = ( -4 ),
		GWL_HINSTANCE = ( -6 ),
		GWL_HWNDPARENT = ( -8 ),
		GWL_STYLE = ( -16 ),
		GWL_EXSTYLE = ( -20 ),
		GWL_USERDATA = ( -21 ),
		GWL_ID = ( -12 )
	}

	/// <summary>
	/// Header control item format
	/// </summary>
	internal enum W32_HDF : int
	{
		HDF_LEFT = 0x0000,
		HDF_RIGHT = 0x0001,
		HDF_CENTER = 0x0002,
		HDF_JUSTIFYMASK = 0x0003,
		HDF_NOJUSTIFY = 0xFFFC,
		HDF_RTLREADING = 0x0004,
		HDF_SORTDOWN = 0x0200,
		HDF_SORTUP = 0x0400,
		HDF_SORTED = 0x0600,
		HDF_NOSORT = 0xF1FF,
		HDF_IMAGE = 0x0800,
		HDF_BITMAP_ON_RIGHT = 0x1000,
		HDF_BITMAP = 0x2000,
		HDF_STRING = 0x4000,
		HDF_OWNERDRAW = 0x8000
	}

	/// <summary>
	/// Header control filter type 
	/// </summary>
	internal enum W32_HDFT : int
	{
		HDFT_ISSTRING = 0x0000,
		HDFT_ISNUMBER = 0x0001,
		HDFT_HASNOVALUE = 0x8000
	}

	/// <summary>
	/// Header control item masks
	/// </summary>
	internal enum W32_HDI : int
	{
		HDI_WIDTH = 0x0001,
		HDI_HEIGHT = HDI_WIDTH,
		HDI_TEXT = 0x0002,
		HDI_FORMAT = 0x0004,
		HDI_LPARAM = 0x0008,
		HDI_BITMAP = 0x0010,
		HDI_IMAGE = 0x0020,
		HDI_DI_SETITEM = 0x0040,
		HDI_ORDER = 0x0080,
		HDI_FILTER = 0x0100
	}

	/// <summary>
	/// Header control styles
	/// </summary>
	internal enum W32_HDS : int
	{
		HDS_HORZ = 0x0000,
		HDS_BUTTONS = 0x0002,
		HDS_HOTTRACK = 0x0004,
		HDS_HIDDEN = 0x0008,
		HDS_DRAGDROP = 0x0040,
		HDS_FULLDRAG = 0x0080,
		HDS_FILTERBAR = 0x0100
	}

	/// <summary>
	/// Header control hittest results
	/// </summary>
	internal enum W32_HHT : int
	{
		HHT_NOWHERE = 0x0001,
		HHT_ONHEADER = 0x0002,
		HHT_ONDIVIDER = 0x0004,
		HHT_ONDIVOPEN = 0x0008,
		HHT_ONFILTER = 0x0010,
		HHT_ONFILTERBUTTON = 0x0020,
		HHT_ABOVE = 0x0100,
		HHT_BELOW = 0x0200,
		HHT_TORIGHT = 0x0400,
		HHT_TOLEFT = 0x0800
	}

	/// <summary>
	/// ListView item masks
	/// </summary>
	internal enum W32_LVIF : int
	{
		LVIF_TEXT = 0x0001,
		LVIF_IMAGE = 0x0002,
		LVIF_PARAM = 0x0004,
		LVIF_STATE = 0x0008,
		LVIF_INDENT = 0x0010,
		LVIF_NORECOMPUTE = 0x0800
	}

	/// <summary>
	/// ListView item rectangle type
	/// </summary>
	internal enum W32_LVIR : int
	{
		LVIR_BOUNDS = 0x0000,
		LVIR_ICON = 0x0001,
		LVIR_LABEL = 0x0002,
		LVIR_SELECTBOUNDS = 0x0003
	}

	/// <summary>
	/// ListView item states
	/// </summary>
	internal enum W32_LVIS : int
	{
		LVIS_FOCUSED = 0x0001,
		LVIS_SELECTED = 0x0002,
		LVIS_CUT = 0x0004,
		LVIS_DROPHILITED = 0x0008,
		LVIS_ACTIVATING = 0x0020,
		LVIS_OVERLAYMASK = 0x0F00,
		LVIS_STATEIMAGEMASK = 0xF000
	}
}
