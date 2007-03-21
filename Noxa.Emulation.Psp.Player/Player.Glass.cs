// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#define GLASSENABLED

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using Noxa.Emulation.Psp.Video;
using System.Windows.Forms.VisualStyles;

namespace Noxa.Emulation.Psp.Player
{
	partial class Player
	{
		#region API

		private static class NativeHelpers
		{
			[DllImport( "dwmapi.dll" )]
			public static extern void DwmIsCompositionEnabled( ref bool pfEnabled );

			[DllImport( "dwmapi.dll" )]
			public static extern void DwmExtendFrameIntoClientArea( IntPtr hWnd, ref Margins pMargins );

			[DllImport( "uxtheme.dll" )]
			public static extern int IsThemeActive();

			[DllImport( "uxtheme.dll" )]
			public static extern IntPtr OpenThemeData( IntPtr hWnd, [MarshalAs( UnmanagedType.LPTStr )] string classList );

			[DllImport( "uxtheme.dll" )]
			public static extern void CloseThemeData( IntPtr hTheme );

			[DllImport( "uxtheme.dll" )]
			public extern static uint DrawThemeTextEx( IntPtr hTheme, IntPtr hdc, int partId, int stateId, [MarshalAs( UnmanagedType.LPTStr )] string text, int textLength, FormatValues textFlags, ref RECT pRect, ref DTTOPTS pOptions );

			[Flags]
			public enum FormatValues : uint
			{
				Left = 0,
				Top = 0,
				Center = 1,
				Right = 2,
				VCenter = 4,
				Bottom = 8,
				WordBreak = 16,
				SingleLine = 32,
				ExpandTabs = 64,
				TabStop = 128,
				NoClip = 256,
				ExternalLeading = 512,
				CalcRect = 1024,
				NoPrefix = 2048,
				EditControl = 8192,
				PathEllipses = 16384,
				EndEllipses = 32768,
				ModifyString = 65536,
				RtlReading = 131072,
				WordEllipses = 262144
			}

			[DllImport( "uxtheme.dll" )]
			public extern static uint GetThemeSysFont( IntPtr hTheme, int fontId, out LOGFONT plf );

			#region LOGFONT

			[StructLayout( LayoutKind.Sequential, CharSet = CharSet.Auto )]
			public struct LOGFONT
			{
				public int lfHeight;
				public int lfWidth;
				public int lfEscapement;
				public int lfOrientation;
				public FontWeight lfWeight;
				[MarshalAs( UnmanagedType.U1 )]
				public bool lfItalic;
				[MarshalAs( UnmanagedType.U1 )]
				public bool lfUnderline;
				[MarshalAs( UnmanagedType.U1 )]
				public bool lfStrikeOut;
				public FontCharSet lfCharSet;
				public FontPrecision lfOutPrecision;
				public FontClipPrecision lfClipPrecision;
				public FontQuality lfQuality;
				public FontPitchAndFamily lfPitchAndFamily;
				[MarshalAs( UnmanagedType.ByValTStr, SizeConst = 32 )]
				public string lfFaceName;
			}

			public enum FontWeight : int
			{
				FW_DONTCARE = 0,
				FW_THIN = 100,
				FW_EXTRALIGHT = 200,
				FW_LIGHT = 300,
				FW_NORMAL = 400,
				FW_MEDIUM = 500,
				FW_SEMIBOLD = 600,
				FW_BOLD = 700,
				FW_EXTRABOLD = 800,
				FW_HEAVY = 900,
			}
			public enum FontCharSet : byte
			{
				ANSI_CHARSET = 0,
				DEFAULT_CHARSET = 1,
				SYMBOL_CHARSET = 2,
				SHIFTJIS_CHARSET = 128,
				HANGEUL_CHARSET = 129,
				HANGUL_CHARSET = 129,
				GB2312_CHARSET = 134,
				CHINESEBIG5_CHARSET = 136,
				OEM_CHARSET = 255,
				JOHAB_CHARSET = 130,
				HEBREW_CHARSET = 177,
				ARABIC_CHARSET = 178,
				GREEK_CHARSET = 161,
				TURKISH_CHARSET = 162,
				VIETNAMESE_CHARSET = 163,
				THAI_CHARSET = 222,
				EASTEUROPE_CHARSET = 238,
				RUSSIAN_CHARSET = 204,
				MAC_CHARSET = 77,
				BALTIC_CHARSET = 186,
			}
			public enum FontPrecision : byte
			{
				OUT_DEFAULT_PRECIS = 0,
				OUT_STRING_PRECIS = 1,
				OUT_CHARACTER_PRECIS = 2,
				OUT_STROKE_PRECIS = 3,
				OUT_TT_PRECIS = 4,
				OUT_DEVICE_PRECIS = 5,
				OUT_RASTER_PRECIS = 6,
				OUT_TT_ONLY_PRECIS = 7,
				OUT_OUTLINE_PRECIS = 8,
				OUT_SCREEN_OUTLINE_PRECIS = 9,
				OUT_PS_ONLY_PRECIS = 10,
			}
			public enum FontClipPrecision : byte
			{
				CLIP_DEFAULT_PRECIS = 0,
				CLIP_CHARACTER_PRECIS = 1,
				CLIP_STROKE_PRECIS = 2,
				CLIP_MASK = 0xf,
				CLIP_LH_ANGLES = ( 1 << 4 ),
				CLIP_TT_ALWAYS = ( 2 << 4 ),
				CLIP_DFA_DISABLE = ( 4 << 4 ),
				CLIP_EMBEDDED = ( 8 << 4 ),
			}
			public enum FontQuality : byte
			{
				DEFAULT_QUALITY = 0,
				DRAFT_QUALITY = 1,
				PROOF_QUALITY = 2,
				NONANTIALIASED_QUALITY = 3,
				ANTIALIASED_QUALITY = 4,
				CLEARTYPE_QUALITY = 5,
				CLEARTYPE_NATURAL_QUALITY = 6,
			}
			[Flags]
			public enum FontPitchAndFamily : byte
			{
				DEFAULT_PITCH = 0,
				FIXED_PITCH = 1,
				VARIABLE_PITCH = 2,
				FF_DONTCARE = ( 0 << 4 ),
				FF_ROMAN = ( 1 << 4 ),
				FF_SWISS = ( 2 << 4 ),
				FF_MODERN = ( 3 << 4 ),
				FF_SCRIPT = ( 4 << 4 ),
				FF_DECORATIVE = ( 5 << 4 ),
			}

			#endregion
			
			public enum SysFontType
			{
				/// <summary>
				/// The font used by window captions.
				/// </summary>
				TMT_CAPTIONFONT = 801,
				/// <summary>
				/// The font used by window small captions.
				/// </summary>
				TMT_SMALLCAPTIONFONT = 802,
				/// <summary>
				/// The font used by menus.
				/// </summary>
				TMT_MENUFONT = 803,
				/// <summary>
				/// The font used in status messages.
				/// </summary>
				TMT_STATUSFONT = 804,
				/// <summary>
				/// The font used to display messages in a message box.
				/// </summary>
				TMT_MSGBOXFONT = 805,
				/// <summary>
				/// The font used for icons.
				/// </summary>
				TMT_ICONTITLEFONT = 806,
			}

			[DllImport( "gdi32.dll" )]
			public static extern bool BitBlt( IntPtr hdc, int nXDest, int nYDest, int nWidth,
			   int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, uint dwRop );

			public enum TernaryRasterOperations : uint
			{
				/// <summary>dest = source</summary>
				SRCCOPY = 0x00CC0020,
				/// <summary>dest = source OR dest</summary>
				SRCPAINT = 0x00EE0086,
				/// <summary>dest = source AND dest</summary>
				SRCAND = 0x008800C6,
				/// <summary>dest = source XOR dest</summary>
				SRCINVERT = 0x00660046,
				/// <summary>dest = source AND (NOT dest)</summary>
				SRCERASE = 0x00440328,
				/// <summary>dest = (NOT source)</summary>
				NOTSRCCOPY = 0x00330008,
				/// <summary>dest = (NOT src) AND (NOT dest)</summary>
				NOTSRCERASE = 0x001100A6,
				/// <summary>dest = (source AND pattern)</summary>
				MERGECOPY = 0x00C000CA,
				/// <summary>dest = (NOT source) OR dest</summary>
				MERGEPAINT = 0x00BB0226,
				/// <summary>dest = pattern</summary>
				PATCOPY = 0x00F00021,
				/// <summary>dest = DPSnoo</summary>
				PATPAINT = 0x00FB0A09,
				/// <summary>dest = pattern XOR dest</summary>
				PATINVERT = 0x005A0049,
				/// <summary>dest = (NOT dest)</summary>
				DSTINVERT = 0x00550009,
				/// <summary>dest = BLACK</summary>
				BLACKNESS = 0x00000042,
				/// <summary>dest = WHITE</summary>
				WHITENESS = 0x00FF0062
			}

			[DllImport( "gdi32.dll" )]
			public static extern IntPtr SelectObject( IntPtr hdc, IntPtr hgdiobj );

			[DllImport( "gdi32.dll", SetLastError = true )]
			public static extern IntPtr CreateCompatibleDC( IntPtr hdc );

			[DllImport( "gdi32.dll" )]
			public static extern IntPtr CreateDIBSection( IntPtr hdc, [In] ref BITMAPINFOHEADER pbmi,
				uint iUsage, out IntPtr ppvBits, IntPtr hSection, uint dwOffset );

			[DllImport( "gdi32.dll" )]
			public static extern bool DeleteDC( IntPtr hdc );

			[DllImport( "gdi32.dll" )]
			public static extern bool DeleteObject( IntPtr hObject );

			[StructLayout( LayoutKind.Sequential )]
			public struct RECT
			{
				public int Left;
				public int Top;
				public int Right;
				public int Bottom;

				public RECT( int left, int top, int right, int bottom )
				{
					Left = left;
					Top = top;
					Right = right;
					Bottom = bottom;
				}

				public RECT( Rectangle rect )
				{
					Left = rect.Left;
					Top = rect.Top;
					Right = rect.Right;
					Bottom = rect.Bottom;
				}

				public Rectangle ToRectangle()
				{
					return new Rectangle( Left, Top, Right, Bottom - 1 );
				}
			}

			[StructLayout( LayoutKind.Sequential )]
			public struct COLORREF
			{
				public uint ColorDWORD;

				public COLORREF( System.Drawing.Color color )
				{
					ColorDWORD = ( uint )color.R + ( ( ( uint )color.G ) << 8 ) + ( ( ( uint )color.B ) << 16 );
				}

				public System.Drawing.Color GetColor()
				{
					return System.Drawing.Color.FromArgb( ( int )( 0x000000FFU & ColorDWORD ),
				   ( int )( 0x0000FF00U & ColorDWORD ) >> 8, ( int )( 0x00FF0000U & ColorDWORD ) >> 16 );
				}

				public void SetColor( System.Drawing.Color color )
				{
					ColorDWORD = ( uint )color.R + ( ( ( uint )color.G ) << 8 ) + ( ( ( uint )color.B ) << 16 );
				}
			}

			[StructLayout( LayoutKind.Sequential )]
			public struct POINT
			{
				public int X;
				public int Y;

				public POINT( int x, int y )
				{
					this.X = x;
					this.Y = y;
				}

				public static implicit operator System.Drawing.Point( POINT p )
				{
					return new System.Drawing.Point( p.X, p.Y );
				}

				public static implicit operator POINT( System.Drawing.Point p )
				{
					return new POINT( p.X, p.Y );
				}
			}

			[StructLayout( LayoutKind.Sequential )]
			public struct DTTOPTS
			{
				public int dwSize;
				public int dwFlags;
				public COLORREF crText;
				public COLORREF crBorder;
				public COLORREF crShadow;
				public int iTextShadowType;
				public POINT ptShadowOffset;
				public int iBorderSize;
				public int iFontPropId;
				public int iColorPropId;
				public int iStateId;
				public bool fApplyOverlay;
				public int iGlowSize;
				public int pfnDrawTextCallback;
				public int lParam;
			}

			public static class DttFlags
			{
				public const int TextColor = 1 << 0;
				public const int BorderColor = 1 << 1;
				public const int ShadowColor = 1 << 2;
				public const int ShadowType = 1 << 3;
				public const int ShadowOffset = 1 << 4;
				public const int BorderSize = 1 << 5;
				public const int FontProp = 1 << 6;
				public const int ColorProp = 1 << 7;
				public const int StateID = 1 << 8;
				public const int CalcRect = 1 << 9;
				public const int ApplyOverlay = 1 << 10;
				public const int GlowSize = 1 << 11;
				public const int Callback = 1 << 12;
				public const int Composited = 1 << 13;
			}

			[StructLayout( LayoutKind.Sequential )]
			public struct Margins
			{
				public int Left;
				public int Right;
				public int Top;
				public int Bottom;
			}

			[StructLayout( LayoutKind.Sequential )]
			public struct BITMAPINFOHEADER
			{
				public uint biSize;
				public int biWidth;
				public int biHeight;
				public ushort biPlanes;
				public ushort biBitCount;
				public uint biCompression;
				public uint biSizeImage;
				public int biXPelsPerMeter;
				public int biYPelsPerMeter;
				public uint biClrUsed;
				public uint biClrImportant;
			}

			public const int WmThemeChanged = 794;
			public const int WmNCHitTest = 0x84;
			public const int HtClient = 1;
			public const int HtCaption = 2;
		}

		#endregion

		private class GlassBounds
		{
			public Rectangle Left = Rectangle.Empty;
			public Rectangle Right = Rectangle.Empty;
			public Rectangle Top = Rectangle.Empty;
			public Rectangle Bottom = Rectangle.Empty;

			public bool Contains( Point p )
			{
				return Left.Contains( p ) ||
					Right.Contains( p ) ||
					Top.Contains( p ) ||
					Bottom.Contains( p );
			}

			public void Invalidate( Rectangle bounds, NativeHelpers.Margins margins )
			{
				Top = new Rectangle( 0, 0, bounds.Width, margins.Top );
				Right = new Rectangle( bounds.Width - margins.Right, 0, margins.Right, bounds.Height );
				Bottom = new Rectangle( 0, bounds.Height - margins.Bottom, bounds.Width, margins.Bottom );
				Left = new Rectangle( 0, 0, margins.Left, bounds.Height );
			}
		}

		private IntPtr _theme = IntPtr.Zero;
		private Font _textFont;
		private Bitmap _glassBuffer;

		private bool _glassEnabled;
		private GlassBounds _glassBounds;
		private NativeHelpers.Margins _glassMargins;

		public static bool IsGlassSupported
		{
			get
			{
				if( Environment.OSVersion.Version.Major < 6 )
					return false;
				try
				{
					bool enabled = false;
					NativeHelpers.DwmIsCompositionEnabled( ref enabled );
					return enabled;
				}
				catch
				{
					return false;
				}
			}
		}

		private void SetupGlass()
		{
			_glassMargins = new NativeHelpers.Margins();
			_glassMargins.Top = 0;
			_glassMargins.Bottom = 22;
			_glassMargins.Left = 0;
			_glassMargins.Right = 0;

			bool supported = Player.IsGlassSupported;
			this.DoubleBuffered = !supported;

			if( supported == false )
				return;
#if !GLASSENABLED
			return;
#endif

			_glassEnabled = true;

			_glassBounds = new GlassBounds();
			_glassBounds.Invalidate( this.ClientRectangle, _glassMargins );

			NativeHelpers.DwmExtendFrameIntoClientArea( this.Handle, ref _glassMargins );
		}

		protected override void OnResize( EventArgs e )
		{
			base.OnResize( e );

			if( _glassBounds != null )
				_glassBounds.Invalidate( this.ClientRectangle, _glassMargins );

			if( _glassBuffer != null )
			{
				_glassBuffer.Dispose();
				_glassBuffer = null;
			}

			this.Invalidate( false );
		}

		private void RenderGlassText( Graphics peg, Rectangle statusBounds, string statusText )
		{
			IntPtr hdc = peg.GetHdc();
			IntPtr hcdc = NativeHelpers.CreateCompatibleDC( hdc );

			IntPtr hfont = _textFont.ToHfont();

			NativeHelpers.BITMAPINFOHEADER bmi = new NativeHelpers.BITMAPINFOHEADER();
			bmi.biSize = ( uint )Marshal.SizeOf( bmi );
			bmi.biWidth = statusBounds.Width;
			bmi.biHeight = -statusBounds.Height;
			bmi.biBitCount = 32;
			bmi.biPlanes = 1;

			IntPtr dummy;
			IntPtr hbmp = NativeHelpers.CreateDIBSection( hcdc, ref bmi, 0, out dummy, IntPtr.Zero, 0 );

			NativeHelpers.SelectObject( hcdc, hbmp );
			NativeHelpers.SelectObject( hcdc, hfont );

			NativeHelpers.DTTOPTS dttopts = new NativeHelpers.DTTOPTS();
			dttopts.dwSize = Marshal.SizeOf( typeof( NativeHelpers.DTTOPTS ) );
			dttopts.dwFlags = NativeHelpers.DttFlags.Composited | NativeHelpers.DttFlags.GlowSize;
			dttopts.iGlowSize = 10;

			Rectangle textPadding = new Rectangle( 7, 5, 5, 5 );

			NativeHelpers.FormatValues textFormat = NativeHelpers.FormatValues.SingleLine | NativeHelpers.FormatValues.Left | NativeHelpers.FormatValues.NoPrefix;
			NativeHelpers.RECT textBounds = new NativeHelpers.RECT( textPadding.Left, textPadding.Top, statusBounds.Width - textPadding.Right, statusBounds.Height );
			NativeHelpers.DrawThemeTextEx( _theme, hcdc, 0, 0, statusText, -1, textFormat, ref textBounds, ref dttopts );

			int top = this.ClientSize.Height - _glassMargins.Bottom;
			NativeHelpers.BitBlt( hdc, 0, top, statusBounds.Width, statusBounds.Height, hcdc, 0, 0, ( uint )NativeHelpers.TernaryRasterOperations.SRCCOPY );

			NativeHelpers.DeleteObject( hbmp );
			NativeHelpers.DeleteObject( hfont );

			NativeHelpers.DeleteDC( hcdc );
			peg.ReleaseHdc();
		}

		private void PaintMe()
		{
			using( Graphics g = this.CreateGraphics() )
			{
				if( ( VisualStyleInformation.IsEnabledByUser == true ) &&
					( ( _theme == IntPtr.Zero ) ||
					  ( _textFont == null ) ) )
				{
					if( _theme == IntPtr.Zero )
						_theme = NativeHelpers.OpenThemeData( this.Handle, "WINDOW" );
					Debug.Assert( _theme != IntPtr.Zero );
					if( _theme == IntPtr.Zero )
						return;

					VisualStyleElement windowType = VisualStyleElement.CreateElement( "WINDOW", 0, 0 );
					VisualStyleRenderer r = new VisualStyleRenderer( windowType );
					TextMetrics tm = r.GetTextMetrics( g );

					NativeHelpers.LOGFONT lf;
					NativeHelpers.GetThemeSysFont( _theme, ( int )NativeHelpers.SysFontType.TMT_MSGBOXFONT, out lf );
					_textFont = Font.FromLogFont( lf );
					Debug.Assert( _textFont != null );
					if( _textFont == null )
						_textFont = new Font( "Tahoma", 12 );
					//NONCLIENTMETRICS ncm = { sizeof( NONCLIENTMETRICS ) };
					//SystemParametersInfo(
					//    SPI_GETNONCLIENTMETRICS, sizeof( NONCLIENTMETRICS ),
					//    &ncm, false );
					//lf = ncm.lfMessageFont;
				}

				if( _theme == IntPtr.Zero )
					return;

				if( _glassEnabled == true )
				{
					Rectangle[] rects = new Rectangle[ 4 ];
					rects[ 0 ] = _glassBounds.Left;
					rects[ 1 ] = _glassBounds.Right;
					rects[ 2 ] = _glassBounds.Top;
					rects[ 3 ] = _glassBounds.Bottom;
					g.FillRectangles( Brushes.Black, rects );
				}

				int statusTop = this.ClientSize.Height - _glassMargins.Bottom;
				int statusHeight = _glassMargins.Bottom;
				Rectangle statusBounds = new Rectangle( 0, statusTop, this.ClientSize.Width, statusHeight );

				// 21 pixels of space to draw our stuff - glass may or may not be enabled, so make sure we do it right!

				string statusText = this.GetStatusText();

				if( _glassEnabled == true )
					this.RenderGlassText( g, statusBounds, statusText );
				else
				{
					g.FillRectangle( SystemBrushes.Control, statusBounds );
					g.DrawString( statusText, _textFont, SystemBrushes.WindowText, 5, statusTop + 1 );
				}
			}
		}

		protected override void OnPaintBackground( PaintEventArgs e )
		{
			//base.OnPaintBackground( e );
		}

		protected override void OnPaint( PaintEventArgs e )
		{
			//base.OnPaint( e );
		}

		protected override void WndProc( ref Message m )
		{
			// Stop erase background message
			if( m.Msg == ( int )0x0014 )
			{
				this.PaintMe();

				// Set to null (ignore)
				m.Msg = ( int )0x0000;
			}

			base.WndProc( ref m );

			if( m.Msg == NativeHelpers.WmThemeChanged )
			{
				if( _theme != IntPtr.Zero )
				{
					NativeHelpers.CloseThemeData( _theme );
					_theme = IntPtr.Zero;
				}

				this.Invalidate();
			}

			if( _glassEnabled == true )
			{
				// If a click, on the client, and in the glass area...
				if( ( m.Msg == NativeHelpers.WmNCHitTest ) &&
					( m.Result.ToInt32() == NativeHelpers.HtClient ) &&
					( this.IsOnGlass( m.LParam.ToInt32() ) == true ) )
				{
					// Say we clicked on the title bar
					m.Result = new IntPtr( NativeHelpers.HtCaption );
				}
			}
		}

		private bool IsOnGlass( int lParam )
		{
			int x = ( lParam << 16 ) >> 16; // lo order word
			int y = lParam >> 16; // hi order word

			// Translate screen coordinates to client area
			Point p = this.PointToClient( new Point( x, y ) );

			// Work out if point clicked is on glass
			return _glassBounds.Contains( p );
		}
	}
}
