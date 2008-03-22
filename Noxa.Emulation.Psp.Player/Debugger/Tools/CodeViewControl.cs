// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2008 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Noxa.Emulation.Psp.Player.Debugger.Model;
using System.Windows.Forms.VisualStyles;

namespace Noxa.Emulation.Psp.Player.Debugger.Tools
{
	partial class CodeViewControl : Control
	{
		private InprocDebugger _debugger;
		private CodeCache _codeCache;

		private VScrollBar _verticalScrollBar;

		// --line--
		// {method info block}
		// {generated info}
		//  -line-
		// 00000000 assembly    ; comments
		// ....

		private const int ExtraLinesPerMethod = 1;

		public CodeViewControl()
		{
			this.InitializeComponent();

			this.SetStyle( ControlStyles.UserPaint, true );
			this.SetStyle( ControlStyles.DoubleBuffer, true );
			this.SetStyle( ControlStyles.AllPaintingInWmPaint, true );
			this.SetStyle( ControlStyles.ResizeRedraw, true );

			_verticalScrollBar = new VScrollBar();
			_verticalScrollBar.Scroll += new ScrollEventHandler( _verticalScrollBar_Scroll );
			_verticalScrollBar.Dock = DockStyle.Right;
			_verticalScrollBar.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
			_verticalScrollBar.Left = this.ClientSize.Width - 2 - _verticalScrollBar.Width;
			_verticalScrollBar.Top = 2;
			_verticalScrollBar.Height = this.ClientSize.Height - 4;
			this.Controls.Add( _verticalScrollBar );

			this.SetupGraphics();
		}

		public void Setup( InprocDebugger debugger )
		{
			_debugger = debugger;
			_codeCache = debugger.CodeCache;
			this.CalculateSize();
			this.Invalidate();
		}

		public void InvalidateAll()
		{
			this.CalculateSize();
			this.Invalidate();
		}

		#region cctor

		private static string[] _hexLookup;
		private static string[] _asciiLookup;
		static CodeViewControl()
		{
			_hexLookup = new string[ byte.MaxValue + 1 ];
			_asciiLookup = new string[ byte.MaxValue + 1 ];
			for( int n = 0; n <= byte.MaxValue; n++ )
			{
				_hexLookup[ n ] = n.ToString( "X2" );
				if( n > 0x1F && !( n > 0x7E && n < 0xA0 ) )
					_asciiLookup[ n ] = ( ( char )n ).ToString();
				else
					_asciiLookup[ n ] = ".";
			}
		}

		#endregion

		#region Sizing/locations/etc

		private int _totalLines;
		private Size _surfaceSize;
		private int _visibleLines;
		private int _firstVisibleLine;
		private int _lastVisibleLine;

		protected override void OnResize( EventArgs e )
		{
			base.OnResize( e );

			this.CalculateSize();
			this.Invalidate();
		}

		private void CalculateSize()
		{
			if( _codeCache != null )
			{
				int totalLines = 0;
				foreach( MethodBody body in _codeCache.Methods )
					totalLines += ( int )body.TotalLines + ExtraLinesPerMethod;
				_totalLines = totalLines;
				_surfaceSize = new Size( this.ClientRectangle.Width - _verticalScrollBar.Width, ( _lineHeight * _totalLines ) );
				_verticalScrollBar.Minimum = 0;
				_verticalScrollBar.Maximum = _totalLines;
			}
			_visibleLines = ( int )Math.Ceiling( ( double )( this.ClientRectangle.Height / _lineHeight ) );
		}

		private int IndexOfMethodAt( int y, out int lineSum )
		{
			lineSum = 0;
			if( _codeCache == null )
				return -1;
			for( int n = 0; n < _codeCache.Methods.Count; n++ )
			{
				MethodBody body = _codeCache.Methods[ n ];
				int newLineSum = lineSum + ( int )body.TotalLines + ExtraLinesPerMethod;
				if( y <= newLineSum )
					return n;
				else
					lineSum = newLineSum;
			}
			return -1;
		}

		#endregion

		#region Scrolling/navigation

		private int _scrollLine;

		private void _verticalScrollBar_Scroll( object sender, ScrollEventArgs e )
		{
			int targetLine = _firstVisibleLine;
			switch( e.Type )
			{
				case ScrollEventType.First:
					targetLine = 0;
					break;
				case ScrollEventType.Last:
					targetLine = _totalLines - _visibleLines;
					break;
				case ScrollEventType.LargeDecrement:
					targetLine = Math.Max( 0, targetLine - _visibleLines - 4 );
					break;
				case ScrollEventType.LargeIncrement:
					targetLine = Math.Min( _totalLines - _visibleLines, targetLine + _visibleLines - 4 );
					break;
				case ScrollEventType.SmallDecrement:
					if( targetLine > 0 )
						targetLine--;
					break;
				case ScrollEventType.SmallIncrement:
					if( targetLine < ( _totalLines - _visibleLines ) )
						targetLine++;
					break;
				case ScrollEventType.ThumbPosition:
					targetLine = e.NewValue;
					break;
				case ScrollEventType.ThumbTrack:
					targetLine = e.NewValue;
					break;
			}

			_scrollLine = targetLine;
			e.NewValue = targetLine;
			_verticalScrollBar.Value = targetLine;
			_verticalScrollBar.Enabled = true;
			_verticalScrollBar.Invalidate();
			if( this.UpdateVisibleLines() == true )
				this.Invalidate();
		}

		private bool UpdateVisibleLines()
		{
			int firstVisibleLine = _scrollLine;
			int lastVisibleLine = Math.Min( _totalLines - 1, firstVisibleLine + _visibleLines );
			if( ( firstVisibleLine != _firstVisibleLine ) || ( lastVisibleLine != _lastVisibleLine ) )
			{
				_firstVisibleLine = firstVisibleLine;
				_lastVisibleLine = lastVisibleLine;
				return true;
			}
			else
				return false;
		}

		#endregion

		#region Painting

		internal Font _font = new Font( "Courier New", 8.0f );
		internal Brush _addressFontBrush = SystemBrushes.ControlText;
		internal Brush _instrFontBrush = SystemBrushes.ControlText;
		internal Brush _disabledFontBrush = SystemBrushes.GrayText;

		private StringFormat _stringFormat;
		private SizeF _charSize;
		private int _lineSpacing;
		private int _lineHeight;

		private Pen _vertGridPen;
		private Brush _gutterBrush;
		private Brush _addressBrush;
		private Brush _instrBrush;
		private Brush _vizBrush;

		private int _gutterWidth = 15;
		private int _addressWidth;
		private int _labelWidth = 120;
		private int _opcodeWidth = 65;

		private void SetupGraphics()
		{
			_font = new Font( "Courier New", 9.0f, FontStyle.Regular, GraphicsUnit.Point );
			using( Graphics g = Graphics.FromHwnd( this.Handle ) )
				_charSize = g.MeasureString( "00000000", _font, 100, _stringFormat );
			_charSize.Width /= 8.0f;

			_stringFormat = new StringFormat( StringFormat.GenericTypographic );
			_stringFormat.FormatFlags = StringFormatFlags.MeasureTrailingSpaces;
			_lineSpacing = 0;
			_lineHeight = ( int )( _charSize.Height + _lineSpacing + 0.5f );

			_vertGridPen = SystemPens.ControlLight;

			Color c = SystemColors.ControlLight;
			Color lightest = Color.FromArgb(
				Math.Min( c.R + 20, 255 ),
				Math.Min( c.G + 20, 255 ),
				Math.Min( c.B + 20, 255 ) );
			Color light = Color.FromArgb(
				Math.Min( c.R + 25, 255 ),
				Math.Min( c.G + 25, 255 ),
				Math.Min( c.B + 25, 255 ) );
			_gutterBrush = new SolidBrush( lightest );
			_addressBrush = new SolidBrush( light );
			_instrBrush = SystemBrushes.Window;
			_vizBrush = new SolidBrush( light );

			_addressWidth = ( int )( _charSize.Width * 8 + 2 + 0.5f );
		}

		protected override void OnPaintBackground( PaintEventArgs e )
		{
			Brush backBrush = ( this.Enabled == true ) ? _instrBrush : _vizBrush;

			if( TextBoxRenderer.IsSupported )
			{
				VisualStyleElement state = ( this.Enabled == true ) ? VisualStyleElement.TextBox.TextEdit.Normal : VisualStyleElement.TextBox.TextEdit.Disabled;
				VisualStyleRenderer vsr = new VisualStyleRenderer( state );
				vsr.DrawBackground( e.Graphics, this.ClientRectangle );

				Rectangle rectContent = vsr.GetBackgroundContentRectangle( e.Graphics, this.ClientRectangle );
				e.Graphics.FillRectangle( backBrush, rectContent );
				this.DrawShared( e );
			}
			else
			{
				e.Graphics.FillRectangle( backBrush, this.ClientRectangle );
				this.DrawShared( e );

				ControlPaint.DrawBorder3D( e.Graphics, this.ClientRectangle, Border3DStyle.Sunken );
			}
		}

		private void DrawShared( PaintEventArgs e )
		{
			// Gutter
			e.Graphics.FillRectangle( _gutterBrush, 2, e.ClipRectangle.Y + 2, _gutterWidth, e.ClipRectangle.Height - 4 );
			e.Graphics.DrawLine( _vertGridPen,
				2 + _gutterWidth, e.ClipRectangle.Y + 2,
				2 + _gutterWidth, e.ClipRectangle.Height - 3 );

			// Address
			e.Graphics.FillRectangle( _addressBrush, 2 + _gutterWidth + 1, e.ClipRectangle.Y + 2, _addressWidth, e.ClipRectangle.Height - 4 );
			e.Graphics.DrawLine( _vertGridPen,
				2 + _gutterWidth + 1 + _addressWidth + 1, e.ClipRectangle.Y + 2,
				2 + _gutterWidth + 1 + _addressWidth + 1, e.ClipRectangle.Height - 3 );
		}

		protected override void OnPaint( PaintEventArgs e )
		{
			base.OnPaint( e );

			this.UpdateVisibleLines();

			int currentLine = _firstVisibleLine;
			int lineSum;
			int index = this.IndexOfMethodAt( currentLine, out lineSum );
			if( index == -1 )
				return;

			// Offset position based on where we are in the method
			int x = 0;
			int y = 0;

			int lineOffset = currentLine - lineSum;
			while( currentLine < _lastVisibleLine )
			{
				MethodBody body = _codeCache.Methods[ index++ ];
				currentLine += ( int )body.TotalLines + ExtraLinesPerMethod - lineOffset;
				y = this.DrawMethod( e.Graphics, body, x, y, lineOffset );
				lineOffset = 0;
			}
		}

		private int DrawMethod( Graphics g, MethodBody body, int x, int y, int lineOffset )
		{
			int addressx = x + _gutterWidth + 1;
			int codex = addressx + _addressWidth + 1 + 6;
			int opcodex = codex + _labelWidth / 2 + 6;
			int operandx = opcodex + _opcodeWidth;

			Brush codeBrush = this.Enabled ? _instrFontBrush : _disabledFontBrush;
			Brush addressBrush = this.Enabled ? _addressFontBrush : _disabledFontBrush;

			// -- method info --
			if( lineOffset == 0 )
			{
				g.DrawString( string.Format( "// {0} ====================================================", body.Name ), _font, _instrFontBrush, codex, y );
				y += _lineHeight;
			}
			else
				lineOffset--;

			// -- lines --
			for( int n = lineOffset; n < body.Instructions.Length; n++ )
			{
				Instruction instr = body.Instructions[ n ];

				// Label marker
				if( instr.Label != null )
				{
					g.DrawString( instr.Label.Name + ":", _font, codeBrush, codex, y, _stringFormat );
					y += _lineHeight;
				}

				// Gutter
				// TODO

				// Address
				g.DrawString( string.Format( "{0:X8}", instr.Address ), _font, addressBrush, addressx + 6, y, _stringFormat );

				// Opcode
				g.DrawString( instr.Opcode.ToString(), _font, codeBrush, opcodex, y, _stringFormat );

				// Operands
				g.DrawString( instr.GetResolvedOperandString( body ), _font, codeBrush, operandx, y, _stringFormat );

				y += _lineHeight;
			}

			return y;
		}

		#endregion
	}
}
