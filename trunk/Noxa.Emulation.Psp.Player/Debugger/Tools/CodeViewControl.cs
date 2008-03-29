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
using System.Windows.Forms.VisualStyles;
using Noxa.Emulation.Psp.Debugging.DebugModel;
using Noxa.Emulation.Psp.Player.Debugger.Model;

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

		#region Focus/Input

		private int _hoveredIndex = -1;

		protected override void OnMouseDown( MouseEventArgs e )
		{
			base.OnMouseDown( e );
			this.Focus();

			if( e.Clicks == 2 )
			{
				if( e.X < 2 + _gutterWidth + 1 + _addressWidth + 1 )
					return;

				int y = e.Y / _lineHeight;
				int lineIndex = Math.Min( _firstVisibleLine + y, _totalLines );
				int currentLine = lineIndex;
				int lineSum;
				int index = this.IndexOfMethodAt( currentLine, out lineSum );
				if( index == -1 )
					return;
				int lineOffset = currentLine - lineSum;
				if( currentLine <= _lastVisibleLine )
				{
					if( index < _codeCache.Methods.Count )
					{
						MethodBody body = _codeCache.Methods[ index++ ];
						List<Line> lines = ( List<Line> )body.UserCache;
						if( lines == null )
							lines = this.CacheMethod( body );

						Line line = lines[ lineOffset ];
						switch( line.Type )
						{
							case LineType.Label:
								Debug.WriteLine( line.Instruction.Label.ToString() );
								break;
							case LineType.Instruction:
								Debug.WriteLine( line.Instruction.ToString() );
								if( line.Instruction.Reference is CodeReference )
								{
									uint target = line.Instruction.Reference.Address;
									this.NavigateToAddress( target );
								}
								else if( line.Instruction.Reference is Noxa.Emulation.Psp.Player.Debugger.Model.Label )
								{
									uint target = line.Instruction.Reference.Address;
									this.NavigateToAddress( target );
								}
								else if( line.Instruction.Reference is MemoryReference )
								{
									uint target = line.Instruction.Reference.Address;
									_debugger.MemoryTool.NavigateToAddress( target );
									_debugger.MemoryTool.Activate();
								}
								break;
						}
					}
				}
			}
		}

		protected override void OnMouseMove( MouseEventArgs e )
		{
			base.OnMouseMove( e );

			int y = e.Y / _lineHeight;
			_hoveredIndex = Math.Min( _firstVisibleLine + y, _totalLines );

			if( e.X < 2 + _gutterWidth + 1 )
				this.ContextMenuStrip = this.gutterContextMenuStrip;
			else
				this.ContextMenuStrip = this.lineContextMenuStrip;
		}

		protected override void OnMouseUp( MouseEventArgs e )
		{
			if( e.X < 2 + _gutterWidth + 1 )
				this.GutterMouseUp( e );
			else
				base.OnMouseUp( e );
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
		private List<uint> _navigationStack = new List<uint>();
		private int _navigationIndex;

		public void ClearNavigation()
		{
			uint head = _navigationStack[ _navigationStack.Count - 1 ];
			_navigationStack.Clear();
			_navigationStack.Add( head );
			_navigationIndex = 0;
		}

		private uint PeekNavigationStack()
		{
			if( _navigationStack.Count == 0 )
				return 0;
			return _navigationStack[ _navigationIndex ];
		}

		public void NavigateBack()
		{
			if( _navigationIndex <= 0 )
				return;
			_navigationIndex--;
			this.SetAddress( _navigationStack[ _navigationIndex ] );
		}

		public void NavigateForward()
		{
			if( _navigationIndex >= _navigationStack.Count - 1 )
				return;
			_navigationIndex++;
			uint next = this.PeekNavigationStack();
			this.SetAddress( next );
		}

		public void NavigateToAddress( uint address )
		{
			if( _navigationStack.Count == 0 )
			{
				_navigationStack.Add( address );
				_navigationIndex = 0;
			}
			else
			{
				uint current = this.PeekNavigationStack();
				if( current == address )
				{
					this.SetAddress( address );
					return;
				}
				_navigationStack.RemoveRange( _navigationIndex + 1, _navigationStack.Count - _navigationIndex - 1 );
				_navigationStack.Add( address );
				_navigationIndex++;
			}
			this.SetAddress( address );
		}

		public void SetAddress( uint address )
		{
			MethodBody body = _codeCache[ address ];
			if( body == null )
			{
				// TODO: status update?
				_debugger.SetStatusText( "Could not find method for address 0x{0:X8} - unable to display", address );
				this.Invalidate();
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
				this.Invalidate();
				return;
			}

			int targetLine = line - ( _visibleLines / 2 );
			targetLine = Math.Max( Math.Min( targetLine, _totalLines ), 0 );
			this.ScrollToLine( targetLine );
			this.Invalidate();
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
			int refx = 300;

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

							MemoryReference memRef = instr.Reference as MemoryReference;
							if( memRef != null )
							{
								Variable var = _debugger.DebugHost.Database.FindSymbol( memRef.Address ) as Variable;
								string name;
								if( ( var != null ) && ( var.Name != null ) )
									name = var.Name;
								else
									name = memRef.Address.ToString( "X8" );
								g.DrawString( name, _font, _referenceFontBrush, refx, y, _stringFormat );
							}

							// Annotations
							//if( instr.Annotation != null )
							//	g.DrawString( instr.Annotation, _font, _referenceFontBrush, realx + 10, y, _stringFormat );

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

		#region Context Menus

		private int _contextIndex = -1;
		private Operand _contextOperand;

		private Instruction GetContextInstruction()
		{
			if( _contextIndex < 0 )
			{
				if( _hoveredIndex < 0 )
					return null;
				_contextIndex = _hoveredIndex;
			}
			int lineSum;
			int methodIndex = this.IndexOfMethodAt( _contextIndex, out lineSum );
			MethodBody method = _debugger.CodeCache.Methods[ methodIndex ];
			int lineIndex = ( _contextIndex - lineSum );
			List<Line> lines = ( List<Line> )method.UserCache;
			Line line = lines[ lineIndex ];
			return line.Instruction;
		}

		#region Gutter / Breakpoints

		private void GutterMouseUp( MouseEventArgs e )
		{
			if( e.Button == MouseButtons.Right )
				return;

			Instruction instr = this.GetContextInstruction();
			if( instr == null )
				return;

			bool hasBreakpoint = ( instr.Breakpoint != null );
			if( e.Button == MouseButtons.Left )
			{
				if( hasBreakpoint == true )
				{
					// Delete breakpoint
					this.removeBreakpointToolStripMenuItem_Click( this, EventArgs.Empty );
				}
				else
				{
					// Add breakpoint
					this.addBreakpointToolStripMenuItem_Click( this, EventArgs.Empty );
				}
			}
			else if( e.Button == MouseButtons.Middle )
			{
				// Toggle
				this.toggleBreakpointToolStripMenuItem_Click( this, EventArgs.Empty );
			}
		}

		private void gutterContextMenuStrip_Opening( object sender, CancelEventArgs e )
		{
			Instruction instr = this.GetContextInstruction();
			if( instr == null )
				return;

			bool hasBreakpoint = ( instr.Breakpoint != null );

			if( hasBreakpoint == true )
			{
				bool breakpointEnabled = instr.Breakpoint.Enabled;
				if( breakpointEnabled == true )
					this.toggleBreakpointToolStripMenuItem.Text = "D&isable Breakpoint";
				else
					this.toggleBreakpointToolStripMenuItem.Text = "&Enable Breakpoint";
			}
			this.toggleBreakpointToolStripMenuItem.Visible = hasBreakpoint;
			this.renameBreakpointToolStripMenuItem.Visible = hasBreakpoint;
			this.toolStripSeparator1.Visible = hasBreakpoint;
			this.addBreakpointToolStripMenuItem.Visible = !hasBreakpoint;
			this.removeBreakpointToolStripMenuItem.Visible = hasBreakpoint;
		}

		private void addBreakpointToolStripMenuItem_Click( object sender, EventArgs e )
		{
			Instruction instr = this.GetContextInstruction();
			if( instr == null )
				return;

			Breakpoint bp = new Breakpoint( _debugger.AllocateID(), BreakpointType.CodeExecute, instr.Address );
			_debugger.Breakpoints.Add( bp );

			this.ContextReturn();
		}

		private void removeBreakpointToolStripMenuItem_Click( object sender, EventArgs e )
		{
			Instruction instr = this.GetContextInstruction();
			if( instr == null )
				return;

			Breakpoint bp = instr.Breakpoint;
			_debugger.Breakpoints.Remove( bp );

			this.ContextReturn();
		}

		private void renameBreakpointToolStripMenuItem_Click( object sender, EventArgs e )
		{
			Instruction instr = this.GetContextInstruction();
			if( instr == null )
				return;

			//Breakpoint bp = this.Debugger.Breakpoints[ instr.BreakpointID ];
			// TODO: rename breakpoint

			this.ContextReturn();
		}

		private void toggleBreakpointToolStripMenuItem_Click( object sender, EventArgs e )
		{
			Instruction instr = this.GetContextInstruction();
			if( instr == null )
				return;

			if( instr.Breakpoint == null )
				return;

			_debugger.Breakpoints.ToggleBreakpoint( instr.Breakpoint );

			this.ContextReturn();
		}

		#endregion

		#region Line

		private void lineContextMenuStrip_Opening( object sender, CancelEventArgs e )
		{
			//if( _hoveredIndex < 0 )
			//    return;
			//_contextIndex = _hoveredIndex;
			//if( ( _hoveredColumn == Column.Instruction ) &&
			//    ( _hoveredValue != null ) )
			//{
			//    if( _hoveredValue is CachedOperand )
			//        _contextOperand = ( ( CachedOperand )_hoveredValue ).Operand;
			//}
			Instruction instr = this.GetContextInstruction();
			if( instr == null )
				return;

			bool hasOperand = ( _contextOperand != null );
			bool referenceOperand = false;
			if( hasOperand == true )
			{
				// Only allow editing if it's a register
				bool readOnly =
					( _contextOperand.Register == null ) ||
					( _contextOperand.Register.ReadOnly == true );
				this.valueToolStripTextBox.ReadOnly = readOnly;

				// Only allow goto if a reference
				referenceOperand =
					( _contextOperand.Type == OperandType.BranchTarget ) ||
					( _contextOperand.Type == OperandType.JumpTarget ) ||
					( _contextOperand.Type == OperandType.MemoryAccess );

				if( ( referenceOperand == true ) &&
					( _contextOperand.Type == OperandType.MemoryAccess ) )
				{
					this.goToTargetToolStripMenuItem.Text = "&Go to Memory Address";
					this.goToTargetToolStripMenuItem.Image = Properties.Resources.MemoryIcon;
				}
				else
				{
					this.goToTargetToolStripMenuItem.Text = "&Go to Code Address";
					this.goToTargetToolStripMenuItem.Image = Properties.Resources.DisassemblyIcon;
				}

				//this.valueToolStripTextBox.Text = this.RequestOperandValue( instr, _contextOperand, false );
				this.valueToolStripTextBox.Modified = false;
			}
			this.valueToolStripTextBox.Visible = hasOperand;
			this.toolStripSeparator5.Visible = hasOperand;
			this.copyOperandToolStripMenuItem.Visible = hasOperand;
			this.goToTargetToolStripMenuItem.Visible = hasOperand && referenceOperand;
			this.toolStripSeparator4.Visible = hasOperand;
		}

		#region Operands

		private void valueToolStripTextBox_TextChanged( object sender, EventArgs e )
		{
			Instruction instr = this.GetContextInstruction();
			if( instr == null )
				return;

			if( this.valueToolStripTextBox.Modified == true )
			{
				//Debug.WriteLine( "changed " + valueToolStripTextBox.Text );
				//this.UpdateOperandValue( instr, _contextOperand, this.valueToolStripTextBox.Text );
			}
		}

		private void copyOperandToolStripMenuItem_Click( object sender, EventArgs e )
		{
			Instruction instr = this.GetContextInstruction();
			if( instr == null )
				return;

			Clipboard.Clear();
			//Clipboard.SetText( this.RequestOperandValue( instr, _contextOperand, false ), TextDataFormat.Text );

			this.ContextReturn();
		}

		private void goToTargetToolStripMenuItem_Click( object sender, EventArgs e )
		{
			Instruction instr = this.GetContextInstruction();
			if( instr == null )
				return;

			this.ContextReturn();
		}

		#endregion

		#region Clipboard

		private void copyAddressToolStripMenuItem_Click( object sender, EventArgs e )
		{
			Instruction instr = this.GetContextInstruction();
			if( instr == null )
				return;

			Clipboard.Clear();
			//Clipboard.SetText( string.Format( "{0:X8}", instr.Address ) );

			this.ContextReturn();
		}

		private void copyInstructionToolStripMenuItem_Click( object sender, EventArgs e )
		{
			Instruction instr = this.GetContextInstruction();
			if( instr == null )
				return;

			Clipboard.Clear();
			//Clipboard.SetText( instr.ToString() );

			this.ContextReturn();
		}

		private void copyLineToolStripMenuItem_Click( object sender, EventArgs e )
		{
			Instruction instr = this.GetContextInstruction();
			if( instr == null )
				return;

			StringBuilder line = new StringBuilder();
			StringBuilder html = new StringBuilder();

			//string args = instr.GetOperandsString();

			//line.AppendFormat( "[{0:X8}] {1:X8} {2} {3}",
			//	instr.Address, instr.Code, instr.Opcode.ToString(), args );
			//html.AppendFormat( "[<b><font color=\"blue\">{0:X8}</font></b>] <font color=\"grey\">{1:X8}</font> <b>{2}</b> {3}",
			//	instr.Address, instr.Code, instr.Opcode.ToString(), args );

			Clipboard.Clear();
			//Clipboard.SetText( line.ToString(), TextDataFormat.Html );
			//Clipboard.SetText( html.ToString(), TextDataFormat.Text );

			this.ContextReturn();
		}

		#endregion

		#region Control

		private void showNextToolStripMenuItem_Click( object sender, EventArgs e )
		{
			Instruction instr = this.GetContextInstruction();
			if( instr == null )
				return;

			this.NavigateToAddress( _debugger.PC );
			this.Focus();

			this.ContextReturn();
		}

		private void runToCursorToolStripMenuItem_Click( object sender, EventArgs e )
		{
			Instruction instr = this.GetContextInstruction();
			if( instr == null )
				return;

			this.ContextReturn();
		}

		private void setNextToolStripMenuItem_Click( object sender, EventArgs e )
		{
			Instruction instr = this.GetContextInstruction();
			if( instr == null )
				return;

			this.ContextReturn();
		}

		#endregion

		#endregion

		private void ContextReturn()
		{
			this.Invalidate();
			_contextIndex = -1;
			_contextOperand = null;
		}

		private void GeneralContextClosed( object sender, ToolStripDropDownClosedEventArgs e )
		{
			if( e.CloseReason != ToolStripDropDownCloseReason.ItemClicked )
				this.ContextReturn();
		}

		#endregion
	}
}
