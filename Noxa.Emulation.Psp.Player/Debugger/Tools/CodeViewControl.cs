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

		private const int ExtraLinesPerMethod = 2;

		public bool UseHex { get; set; }

		public CodeViewControl()
		{
			this.InitializeComponent();

			this.SetStyle( ControlStyles.UserPaint, true );
			this.SetStyle( ControlStyles.AllPaintingInWmPaint, true );
			this.SetStyle( ControlStyles.ResizeRedraw, true );
			this.SetStyle( ControlStyles.Opaque, true );
			this.SetStyle( ControlStyles.OptimizedDoubleBuffer, true );
			this.SetStyle( ControlStyles.Selectable, true );

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

		#region Method Caching

		enum LineType
		{
			Header,
			Info,
			Footer,
			Label,
			Instruction,
		}

		class Line
		{
			public LineType Type;
			public Instruction Instruction;
		}

		private List<Line> CacheMethod( MethodBody body )
		{
			List<Line> lines = new List<Line>( ( int )( body.TotalLines + ExtraLinesPerMethod ) );

			Line line = new Line();
			line.Type = LineType.Header;
			lines.Add( line );

			foreach( Instruction instruction in body.Instructions )
			{
				if( instruction.Label != null )
				{
					line = new Line();
					line.Type = LineType.Label;
					line.Instruction = instruction;
					lines.Add( line );
				}

				line = new Line();
				line.Type = LineType.Instruction;
				line.Instruction = instruction;
				lines.Add( line );
			}

			line = new Line();
			line.Type = LineType.Footer;
			lines.Add( line );

			body.UserCache = lines;
			body.UserLines = ( uint )lines.Count;
			return lines;
		}

		#endregion

		#region Focus

		protected override void OnMouseDown( MouseEventArgs e )
		{
			base.OnMouseDown( e );
			this.Focus();
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
				{
					// TODO: cache here?
					body.UserTop = totalLines;
					body.UserLines = body.TotalLines + ExtraLinesPerMethod;
					totalLines += ( int )body.UserLines;
				}
				_totalLines = totalLines;
				_surfaceSize = new Size( this.ClientRectangle.Width - _verticalScrollBar.Width, ( _lineHeight * _totalLines ) );
				_verticalScrollBar.Minimum = 0;
				_verticalScrollBar.Maximum = _totalLines;
			}
			_visibleLines = ( int )Math.Ceiling( ( double )( this.ClientRectangle.Height / _lineHeight ) );

			if( this.UpdateVisibleLines() == true )
				this.Invalidate();
		}

		private int IndexOfMethodAt( int y, out int lineSum )
		{
			lineSum = 0;
			if( _codeCache == null )
				return -1;
			for( int n = 0; n < _codeCache.Methods.Count; n++ )
			{
				MethodBody body = _codeCache.Methods[ n ];
				int newLineSum = lineSum + ( int )body.UserLines;
				if( y < newLineSum )
					return n;
				else
					lineSum = newLineSum;
			}
			return -1;
		}

		#endregion

		#region Scrolling/navigation

		private int _scrollLine;

		public void SetAddress( uint address )
		{
			MethodBody body = _codeCache[ address ];
			if( body == null )
			{
				// TODO: status update?
				return;
			}

			int line = body.UserTop;
			foreach( Instruction instruction in body.Instructions )
			{
				if( instruction.Address == address )
					break;
				line++;
			}

			if( ( line >= _firstVisibleLine ) && ( line < _lastVisibleLine ) )
			{
				// Already in view
				return;
			}

			int targetLine = line - ( _visibleLines / 2 );
			targetLine = Math.Max( Math.Min( targetLine, _totalLines ), 0 );
			this.ScrollToLine( targetLine );
		}

		private bool ScrollToLine( int targetLine )
		{
			if( _scrollLine == targetLine )
				return false;
			_scrollLine = targetLine;
			_verticalScrollBar.Value = targetLine;
			_verticalScrollBar.Enabled = true;
			_verticalScrollBar.Invalidate();
			if( this.UpdateVisibleLines() == true )
				this.Invalidate();
			return true;
		}

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

			e.NewValue = targetLine;
			this.ScrollToLine( targetLine );
		}

		protected override void OnMouseWheel( MouseEventArgs e )
		{
			base.OnMouseWheel( e );

			int targetLine = _firstVisibleLine;
			int units = Math.Min( 1, e.Delta / SystemInformation.MouseWheelScrollDelta );
			if( ( Control.ModifierKeys & Keys.Shift ) == Keys.Shift )
				targetLine -= ( units * ( _visibleLines - 4 ) );
			else
				targetLine -= ( units * SystemInformation.MouseWheelScrollLines );
			targetLine = Math.Max( Math.Min( targetLine, _totalLines ), 0 );

			this.ScrollToLine( targetLine );
		}

		protected override void OnKeyDown( KeyEventArgs e )
		{
			int targetLine = _firstVisibleLine;
			switch( e.KeyCode )
			{
				case Keys.Home:
					if( ( Control.ModifierKeys & Keys.Control ) == Keys.Control )
						targetLine = 0;
					else
					{
						// TODO: jump to current method top
					}
					break;
				case Keys.End:
					if( ( Control.ModifierKeys & Keys.Control ) == Keys.Control )
						targetLine = _totalLines - _visibleLines;
					else
					{
						// TODO: jump to current method bottom
					}
					break;
				case Keys.PageUp:
					targetLine = Math.Max( 0, targetLine - _visibleLines - 4 );
					break;
				case Keys.PageDown:
					targetLine = Math.Min( _totalLines - _visibleLines, targetLine + _visibleLines - 4 );
					break;
				case Keys.Up:
					if( targetLine > 0 )
						targetLine--;
					break;
				case Keys.Down:
					if( targetLine < ( _totalLines - _visibleLines ) )
						targetLine++;
					break;
			}

			if( this.ScrollToLine( targetLine ) == true )
			{
				e.Handled = true;
				return;
			}

			base.OnKeyDown( e );
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

		private Image _breakpointOnIcon;
		private Image _breakpointOffIcon;
		private Image _statementIcon;
		private Image _statementCallIcon;
		private Image _statementDeadIcon;

		internal Font _font = new Font( "Courier New", 8.0f, FontStyle.Regular );
		//internal Font _font = new Font( "Consolas", 8.0f, FontStyle.Bold );
		internal Brush _addressFontBrush = SystemBrushes.ControlText;
		internal Brush _instrFontBrush = SystemBrushes.ControlText;
		internal Brush _disabledFontBrush = SystemBrushes.GrayText;
		internal Brush _commentFontBrush = Brushes.Green;
		internal Pen _commentLinePen = Pens.Green;
		internal Brush _referenceFontBrush = Brushes.Blue;
		internal Brush _labelFontBrush = Brushes.Blue;

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
		private int _labelWidth = 100;
		private int _opcodeWidth = 50;

		private void SetupGraphics()
		{
			_breakpointOnIcon = Properties.Resources.BreakpointIcon;
			_breakpointOffIcon = Properties.Resources.BreakpointOffIcon;
			_statementIcon = Properties.Resources.StatementIcon;
			_statementCallIcon = Properties.Resources.StatementCallIcon;
			_statementDeadIcon = Properties.Resources.StatementDeadIcon;

			using( Graphics g = Graphics.FromHwnd( this.Handle ) )
			{
				g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
				_charSize = g.MeasureString( "00000000", _font, 100, _stringFormat );
			}
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
			_opcodeWidth = ( int )( _charSize.Width * 8 + 2 + 0.5f );
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
			this.OnPaintBackground( e );

			e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;

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
			while( currentLine <= _lastVisibleLine )
			{
				if( index >= _codeCache.Methods.Count )
					break;
				MethodBody body = _codeCache.Methods[ index++ ];
				List<Line> lines = ( List<Line> )body.UserCache;
				if( lines == null )
					lines = this.CacheMethod( body );

				int remainingLines = _lastVisibleLine - currentLine;
				currentLine += ( int )body.UserLines - lineOffset;
				y = this.DrawMethod( e.Graphics, body, lines, x, y, lineOffset, remainingLines );
				lineOffset = 0;
			}
		}

		private int DrawMethod( Graphics g, MethodBody body, List<Line> lines, int x, int y, int lineOffset, int maxLines )
		{
			int addressx = x + _gutterWidth + 1;
			int codex = addressx + _addressWidth + 1 + 6;
			int opcodex = codex + _labelWidth / 3 + 6;
			int operandx = opcodex + _opcodeWidth;

			Brush codeBrush = this.Enabled ? _instrFontBrush : _disabledFontBrush;
			Brush addressBrush = this.Enabled ? _addressFontBrush : _disabledFontBrush;

			// -- lines --
			for( int n = lineOffset; n < lines.Count; n++ )
			{
				Line line = lines[ n ];
				Instruction instr = line.Instruction;

				switch( line.Type )
				{
					case LineType.Header:
						{
							g.DrawString( string.Format( "// {0}", body.Name ), _font, _commentFontBrush, codex, y );
							g.DrawLine( _commentLinePen, codex + ( _charSize.Width * ( body.Name.Length + 4 ) ), y + ( _charSize.Height / 2.0f ), this.ClientRectangle.Width - 10, y + ( _charSize.Height / 2.0f ) );
						}
						break;
					case LineType.Footer:
						{
						}
						break;
					case LineType.Label:
						{
							g.DrawString( instr.Label.Name + ":", _font, _labelFontBrush, codex, y, _stringFormat );
						}
						break;
					case LineType.Instruction:
						{
							// Gutter
							if( instr.Breakpoint != null )
							{
								Image icon = ( instr.Breakpoint.Enabled == true ) ? _breakpointOnIcon : _breakpointOffIcon;
								g.DrawImage( icon, x + 2, y, 15, 15 );
							}
							if( ( _debugger.State != DebuggerState.Running ) && ( _debugger.PC == instr.Address ) )
							{
								g.DrawImage( _statementIcon, x + 3, y, 14, 15 );
							}

							// Address
							g.DrawString( string.Format( "{0:X8}", instr.Address ), _font, addressBrush, addressx + 6, y, _stringFormat );

							// Instruction
							int realx = operandx;
							if( instr.Code == 0x0 )
							{
								g.DrawString( "nop", _font, _disabledFontBrush, opcodex, y, _stringFormat );
							}
							else
							{
								g.DrawString( instr.Opcode.ToString(), _font, codeBrush, opcodex, y, _stringFormat );

								// Operands
								for( int m = 0; m < instr.Operands.Length; m++ )
								{
									Operand op = instr.Operands[ m ];
									string resolved = instr.GetResolvedOperandString( op, this.UseHex );
									Brush fontBrush = codeBrush;
									switch( op.Type )
									{
										case OperandType.BranchTarget:
											fontBrush = _referenceFontBrush;
											break;
										case OperandType.JumpTarget:
											fontBrush = _referenceFontBrush;
											break;
									}
									g.DrawString( resolved, _font, fontBrush, realx, y, _stringFormat );
									realx += ( int )_charSize.Width * resolved.Length;

									bool last = ( m == instr.Operands.Length - 1 );
									if( last == false )
									{
										g.DrawString( ", ", _font, codeBrush, realx - 2, y, _stringFormat );
										realx += ( int )_charSize.Width * 2 - 2;
									}
								}
							}

							// Annotations
							if( instr.Annotation != null )
								g.DrawString( instr.Annotation, _font, _referenceFontBrush, realx + 10, y, _stringFormat );

							// Comments
							// TODO _commentFontBrush
						}
						break;
				}

				y += _lineHeight;

				maxLines--;
				if( maxLines == 0 )
					break;
			}

			return y;
		}

		#endregion
	}
}
