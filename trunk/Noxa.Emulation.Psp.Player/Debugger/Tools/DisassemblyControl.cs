// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
using Noxa.Emulation.Psp.Debugging;
using Noxa.Emulation.Psp.Debugging.DebugModel;
using Noxa.Emulation.Psp.Debugging.Hooks;
using Noxa.Emulation.Psp.Player.Debugger.Model;

namespace Noxa.Emulation.Psp.Player.Debugger.Tools
{
	partial class DisassemblyControl : ListBox
	{
		public bool _displayHex = true;
		public InprocDebugger Debugger;

		public MethodBody MethodBody;
		public uint CurrentAddress;

		public event EventHandler RegisterValueChanged;

		public DisassemblyControl()
		{
			InitializeComponent();

			this.SetStyle( ControlStyles.AllPaintingInWmPaint, true );
			this.SetStyle( ControlStyles.Opaque, true );
			this.SetStyle( ControlStyles.OptimizedDoubleBuffer, true );
			//this.SetStyle( ControlStyles.UserPaint, true );
			this.SetupGraphics();
		}

		public bool DisplayHex
		{
			get
			{
				return _displayHex;
			}
			set
			{
				if( _displayHex != value )
				{
					_displayHex = value;
					this.SetMethod( this.MethodBody );
				}
			}
		}

		#region Formatting Objects

		private class CachedOpcode
		{
			public readonly Opcode Opcode;
			public readonly RectangleF Bounds;

			public CachedOpcode( DisassemblyControl control, Graphics g, int x, int y, int height, Opcode opcode )
			{
				this.Opcode = opcode;
				this.Bounds = new RectangleF( x, y,
					( int )( g.MeasureString( opcode.ToString(), control._instrFont ).Width ) + 1, height );
			}
		}

		private class CachedOperand
		{
			public readonly Operand Operand;
			public readonly RectangleF Bounds;

			public CachedOperand( DisassemblyControl control, Graphics g, int x, int y, int height, Operand operand )
			{
				this.Operand = operand;
				this.Bounds = new RectangleF( x, y,
					( int )( g.MeasureString( operand.ToString(), control._instrFont ).Width ) + 1, height );
			}
		}

		private class CachedFormatting
		{
			public readonly Instruction Instruction;
			public readonly CachedOpcode Opcode;
			public readonly CachedOperand[] Operands;
			public readonly RectangleF Bounds;

			public static int OpStart;
			public static int OpSepWidth;

			public CachedFormatting( DisassemblyControl control, Graphics g, int x, int y, int height, Instruction instruction )
			{
				int left = x;
				int top = y;

				this.Instruction = instruction;

				x += 5;
				
				this.Opcode = new CachedOpcode( control, g, x, y, height, this.Instruction.Opcode );
				x += OpStart;

				List<CachedOperand> ops = new List<CachedOperand>( this.Instruction.Operands.Length );
				foreach( Operand op in this.Instruction.Operands )
				{
					CachedOperand cop = new CachedOperand( control, g, x, y, height, op );
					x += ( int )cop.Bounds.Width + ( OpSepWidth / 2 ) + 2;
					ops.Add( cop );
				}
				this.Operands = ops.ToArray();

				this.Bounds = new RectangleF( left, top, x, height );
			}
		}

		#endregion

		private Dictionary<int, CachedFormatting> _formatCache;

		public void SetMethod( MethodBody methodBody )
		{
			this.MethodBody = methodBody;
			this.CurrentAddress = 0;

			this.BeginUpdate();

			this.Items.Clear();
			if( methodBody != null )
			{
				using( Graphics g = Graphics.FromHwnd( this.Handle ) )
				{
					Instruction[] instructions = this.MethodBody.Instructions;
					_formatCache = new Dictionary<int, CachedFormatting>( instructions.Length );
					for( int n = 0; n < instructions.Length; n++ )
					{
						Instruction instr = instructions[ n ];
						int index = this.Items.Add( instr );
						Rectangle itemBounds = this.GetItemRectangle( index );
						_formatCache.Add( n, new CachedFormatting( this, g, itemBounds.X + _instrLeft, itemBounds.Y, itemBounds.Height, instr ) );
					}
				}
			}

			this.EndUpdate();
		}

		public void SetCurrentAddress( uint address )
		{
			this.CurrentAddress = address;

			this.SetAddress( address );
		}

		public void SetAddress( uint address )
		{
			uint offset = ( address - this.MethodBody.Address ) >> 2;
			this.SelectedIndex = ( int )offset;

			this.Invalidate();
		}

		#region Drawing

		private Image _breakpointOnIcon;
		private Image _breakpointOffIcon;
		private Image _statementIcon;
		private Image _statementCallIcon;
		private Image _statementDeadIcon;

		private Pen _vertGridPen;
		private Brush _gutterBrush;
		private Brush _addressBrush;
		private Brush _instrBrush;
		private Brush _vizBrush;

		internal Font _addressFont = new Font( "Courier New", 8.0f );
		internal Font _instrFont = new Font( "Courier New", 8.0f );
		internal Brush _addressFontBrush = SystemBrushes.ControlText;
		internal Brush _instrFontBrush = SystemBrushes.ControlText;
		internal Brush _disabledFontBrush = SystemBrushes.GrayText;

		private int _gutterWidth;
		private int _addressWidth;
		private int _instrWidth;
		private int _vizWidth;
		private int _addressLeft;
		private int _instrLeft;
		private int _vizLeft;

		private StringFormat _instrFormat;

		private void MeasureAll()
		{
			using( Graphics g = Graphics.FromHwnd( this.Handle ) )
			{
				_gutterWidth = 15;
				_addressWidth = ( int )( g.MeasureString( "00000000", _addressFont ).Width + 0.5f ) + 5;
				_instrWidth = 260;

				_addressLeft = _gutterWidth + 1;
				_instrLeft = _addressLeft + _addressWidth + 1;
				_vizLeft = _instrLeft + _instrWidth + 1;

				int x = _gutterWidth + _addressWidth + _instrWidth + 3;

				_vizWidth = this.ClientSize.Width - x;

				CachedFormatting.OpStart = ( int )( g.MeasureString( "abcdefghij", _instrFont ).Width + 0.5f );
				CachedFormatting.OpSepWidth = ( int )( g.MeasureString( ", ", _instrFont ).Width + 0.5f );
			}

			_instrFormat = new StringFormat( StringFormatFlags.FitBlackBox | StringFormatFlags.NoWrap );
		}

		private void SetupGraphics()
		{
			_breakpointOnIcon = Properties.Resources.BreakpointIcon;
			_breakpointOffIcon = Properties.Resources.BreakpointOffIcon;
			_statementIcon = Properties.Resources.StatementIcon;
			_statementCallIcon = Properties.Resources.StatementCallIcon;
			_statementDeadIcon = Properties.Resources.StatementDeadIcon;

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

			this.MeasureAll();
		}

		protected override void OnDrawItem( DrawItemEventArgs e )
		{
			if( ( this.Items.Count == 0 ) ||
				( e.Index < 0 ) )
				return;

			Graphics g = e.Graphics;
			e.Graphics.Clip = new Region( e.Bounds );

			int y = e.Bounds.Top;
			int height = e.Bounds.Height;

			int offset = -( this.TopIndex * height );

			Instruction instr = ( Instruction )this.Items[ e.Index ];
			CachedFormatting formatting = _formatCache[ e.Index ];

			int x = 0;

			// Gutter
			g.FillRectangle( _gutterBrush, x, y, _gutterWidth, height );
			{
				if( instr.BreakpointID != Instruction.InvalidBreakpointID )
				{
					Image icon;
					Breakpoint bp = this.Debugger.Breakpoints[ instr.BreakpointID ];
					if( bp.Enabled == true )
						icon = _breakpointOnIcon;
					else
						icon = _breakpointOffIcon;
					g.DrawImage( icon, x, y - 1, 14, 14 );
				}

				// Show icons for address state (such as dead/parent)
				if( this.CurrentAddress == instr.Address )
				{
					Image icon = _statementIcon;
					g.DrawImage( icon, x, y - 1, 14, 14 );
				}
			}
			x += _gutterWidth;

			// -- sep --
			g.DrawLine( _vertGridPen, x, y, x, y + height );
			x += 1;

			// Address
			g.FillRectangle( _addressBrush, x, y, _addressWidth, height );
			{
				string addressString = string.Format( "{0:X8}", instr.Address );
				Brush brush = ( this.Enabled ) ? _addressFontBrush : _disabledFontBrush;
				g.DrawString( addressString, _addressFont, brush, x + 2, y );
			}
			x += _addressWidth;

			// -- sep --
			g.DrawLine( _vertGridPen, x, y, x, y + height );
			x += 1;

			// Instruction
			g.FillRectangle( _instrBrush, x, y, _instrWidth, height );
			{
				Matrix originalTransform = g.Transform;
				g.TranslateTransform( 0.0f, offset );

				Brush brush = ( this.Enabled ) ? _instrFontBrush : _disabledFontBrush;

				g.DrawString( instr.Opcode.ToString(), _instrFont, brush, formatting.Opcode.Bounds, _instrFormat );
				for( int n = 0; n < instr.Operands.Length; n++ )
				{
					RectangleF opBounds = formatting.Operands[ n ].Bounds;
					g.DrawString( instr.Operands[ n ].ToString(), _instrFont, brush, opBounds, _instrFormat );
					if( n != instr.Operands.Length - 1 )
					{
						g.DrawString( ", ", _instrFont, brush, new RectangleF(
							opBounds.Right - ( CachedFormatting.OpSepWidth / 2 ), opBounds.Y, CachedFormatting.OpSepWidth, opBounds.Height ) );
					}
				}

				g.Transform = originalTransform;
			}
			x += _instrWidth;

			// -- sep --
			g.DrawLine( _vertGridPen, x, y, x, y + height );
			x += 1;

			// Visualizer
			g.FillRectangle( _vizBrush, x, y, _vizWidth, height );
			{
			}
			x += _vizWidth;

			if( _hoveredIndex == e.Index )
			{
				switch( _hoveredColumn )
				{
					case Column.Gutter:
						break;
					case Column.Address:
						break;
					case Column.Instruction:
						if( _hoveredValue is CachedOpcode )
						{
							CachedOpcode cop = ( CachedOpcode )_hoveredValue;
							ControlPaint.DrawFocusRectangle( g, new Rectangle( ( int )cop.Bounds.X, ( int )cop.Bounds.Y + offset, ( int )cop.Bounds.Width, ( int )cop.Bounds.Height ) );
						}
						else if( _hoveredValue is CachedOperand )
						{
							CachedOperand cop = ( CachedOperand )_hoveredValue;
							ControlPaint.DrawFocusRectangle( g, new Rectangle( ( int )cop.Bounds.X, ( int )cop.Bounds.Y + offset, ( int )cop.Bounds.Width, ( int )cop.Bounds.Height ) );
						}
						break;
					case Column.Visualizer:
						break;
					default:
					case Column.None:
						break;
				}
			}

			if( _contextIndex == e.Index )
			{
				Rectangle realBounds = e.Bounds;
				ControlPaint.DrawFocusRectangle( g, realBounds );
			}
		}

		protected override void OnResize( EventArgs e )
		{
			base.OnResize( e );
			this.MeasureAll();
		}

		protected override void OnPaintBackground( PaintEventArgs pevent )
		{
			//base.OnPaintBackground( pevent );
		}

		protected override void OnPaint( PaintEventArgs pe )
		{
			base.OnPaint( pe );
		}

		#endregion

		#region Tips / Hovering

		private enum Column
		{
			None,
			Gutter,
			Address,
			Instruction,
			Visualizer,
		}

		private int _hoveredIndex = -1;
		private Column _hoveredColumn = Column.None;
		private object _hoveredValue = null;

		private Column GetColumn( int x )
		{
			if( x < _addressLeft )
				return Column.Gutter;
			else if( x < _instrLeft )
				return Column.Address;
			else if( x < _vizLeft )
				return Column.Instruction;
			else if( x < _vizLeft + _vizWidth )
				return Column.Visualizer;
			else
				return Column.None;
		}

		protected override void OnMouseMove( MouseEventArgs e )
		{
			base.OnMouseMove( e );

			int index = this.IndexFromPoint( e.Location );
			if( index >= 0 )
			{
				bool needsInvalidate = false;
				int oldIndex = _hoveredIndex;

				if( _hoveredIndex != index )
				{
					//Debug.WriteLine( "up " + _hoveredIndex.ToString() );
					//Debug.WriteLine( "down " + _hoveredIndex.ToString() );
					_hoveredIndex = index;
					_hoveredColumn = Column.None;
					_hoveredValue = null;
					needsInvalidate = true;
				}

				int x = e.X;
				int y = e.Y + ( this.TopIndex * this.ItemHeight );

				Column newColumn = this.GetColumn( x );
				if( _hoveredColumn != newColumn )
				{
					//Debug.WriteLine( "out " + _hoveredColumn.ToString() );
					//Debug.WriteLine( "in " + newColumn.ToString() );
					_hoveredColumn = newColumn;
					needsInvalidate = true;
				}

				Instruction instr = ( Instruction )this.Items[ index ];
				string tipText = null;

				switch( _hoveredColumn )
				{
					case Column.Gutter:
						{
							this.ContextMenuStrip = this.gutterContextMenuStrip;
							bool hasBreakpoint = ( instr.BreakpointID != Instruction.InvalidBreakpointID );
							if( hasBreakpoint == true )
							{
								tipText = "Breakpoint ????";
							}
							else
								tipText = "Click to add a breakpoint";
						}
						break;
					case Column.Address:
						this.ContextMenuStrip = this.lineContextMenuStrip;
						break;
					case Column.Instruction:
						{
							this.ContextMenuStrip = this.lineContextMenuStrip;
							object newValue = null;

							// Try to find the opcode/operand the cursor is over
							CachedFormatting formatting = _formatCache[ index ];
							if( formatting.Opcode.Bounds.Contains( x, y ) )
							{
								newValue = formatting.Opcode;
								tipText = string.Format( "0x{0:X8}", instr.Code );
								//Debug.WriteLine( "hover opcode " + formatting.Opcode.Opcode.ToString() );
							}
							else
							{
								for( int n = 0; n < formatting.Operands.Length; n++ )
								{
									CachedOperand cop = formatting.Operands[ n ];
									if( cop.Bounds.Contains( x, y ) == true )
									{
										newValue = cop;
										// Ignore annotations
										if( cop.Operand.Type != OperandType.Annotation )
											tipText = this.RequestOperandValue( instr, cop.Operand, true );
										//Debug.WriteLine( "hover operand " + cop.Operand.ToString() );
										break;
									}
								}
							}

							if( _hoveredValue != newValue )
							{
								//if( newValue == null )
								//	Debug.WriteLine( "hover up" );
								_hoveredValue = newValue;
								needsInvalidate = true;
							}
						}
						break;
					case Column.Visualizer:
						this.ContextMenuStrip = null;
						break;
					default:
					case Column.None:
						this.ContextMenuStrip = null;
						break;
				}

				if( needsInvalidate == true )
				{
					if( ( oldIndex >= 0 ) &&
						( oldIndex != _hoveredIndex ) )
						this.Invalidate( this.GetItemRectangle( oldIndex ) );
					this.Invalidate( this.GetItemRectangle( _hoveredIndex ) );
					this.toolTip1.SetToolTip( this, tipText );
				}
			}
			else
			{
				this.ClearHovered();
			}
		}

		private void ClearHovered()
		{
			this.toolTip1.SetToolTip( this, null );
			if( ( _hoveredIndex >= 0 ) ||
				( _hoveredColumn != Column.None ) ||
				( _hoveredValue != null ) )
			{
				_hoveredIndex = -1;
				_hoveredColumn = Column.None;
				_hoveredValue = null;
				this.Invalidate();
			}
		}

		protected override void OnMouseWheel( MouseEventArgs e )
		{
			base.OnMouseWheel( e );
			//this.ClearHovered();
		}

		protected override void OnMouseLeave( EventArgs e )
		{
			base.OnMouseLeave( e );
			this.ClearHovered();
		}

		private void toolTip1_Draw( object sender, DrawToolTipEventArgs e )
		{
		}

		private void toolTip1_Popup( object sender, PopupEventArgs e )
		{
		}

		#endregion

		protected override void OnSelectedIndexChanged( EventArgs e )
		{
			int oldIndex = this.SelectedIndex;

			base.OnSelectedIndexChanged( e );

			if( oldIndex >= 0 )
			{
				Rectangle oldBounds = this.GetItemRectangle( oldIndex );
				this.Invalidate();
			}
		}

		protected override void OnMouseUp( MouseEventArgs e )
		{
			Column column = this.GetColumn( e.X );
			switch( column )
			{
				case Column.Gutter:
					this.GutterMouseUp( e );
					break;
				case Column.Address:
				case Column.Instruction:
				case Column.Visualizer:
				case Column.None:
					base.OnMouseUp( e );
					break;
			}
		}

		private int _contextIndex = -1;
		private Operand _contextOperand;

		#region Gutter / Breakpoints

		private void GutterMouseUp( MouseEventArgs e )
		{
			if( e.Button == MouseButtons.Right )
				return;

			if( _hoveredIndex < 0 )
				return;
			_contextIndex = _hoveredIndex;
			Instruction instr = ( Instruction )this.Items[ _hoveredIndex ];

			bool hasBreakpoint = ( instr.BreakpointID != Instruction.InvalidBreakpointID );
			
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
			if( _contextIndex < 0 )
			{
				if( _hoveredIndex < 0 )
					return;
				_contextIndex = _hoveredIndex;
			}
			Instruction instr = ( Instruction )this.Items[ _contextIndex ];

			bool hasBreakpoint = ( instr.BreakpointID != Instruction.InvalidBreakpointID );
			bool breakpointEnabled = true;

			if( breakpointEnabled == true )
				this.toggleBreakpointToolStripMenuItem.Text = "D&isable Breakpoint";
			else
				this.toggleBreakpointToolStripMenuItem.Text = "&Enable Breakpoint";
			this.toggleBreakpointToolStripMenuItem.Visible = hasBreakpoint;
			this.renameBreakpointToolStripMenuItem.Visible = hasBreakpoint;
			this.toolStripSeparator1.Visible = hasBreakpoint;
			this.addBreakpointToolStripMenuItem.Visible = !hasBreakpoint;
			this.removeBreakpointToolStripMenuItem.Visible = hasBreakpoint;
		}

		private void addBreakpointToolStripMenuItem_Click( object sender, EventArgs e )
		{
			if( _contextIndex < 0 )
				return;
			Instruction instr = ( Instruction )this.Items[ _contextIndex ];

			Breakpoint bp = new Breakpoint( this.Debugger.AllocateID(), BreakpointType.CodeExecute, instr.Address );
			this.Debugger.Breakpoints.Add( bp );
			instr.BreakpointID = bp.ID;

			this.ContextReturn();
		}

		private void removeBreakpointToolStripMenuItem_Click( object sender, EventArgs e )
		{
			if( _contextIndex < 0 )
				return;
			Instruction instr = ( Instruction )this.Items[ _contextIndex ];

			Breakpoint bp = this.Debugger.Breakpoints[ instr.BreakpointID ];
			this.Debugger.Breakpoints.Remove( bp );
			instr.BreakpointID = Instruction.InvalidBreakpointID;

			this.ContextReturn();
		}

		private void renameBreakpointToolStripMenuItem_Click( object sender, EventArgs e )
		{
			if( _contextIndex < 0 )
				return;
			Instruction instr = ( Instruction )this.Items[ _contextIndex ];

			Breakpoint bp = this.Debugger.Breakpoints[ instr.BreakpointID ];
			// TODO: rename breakpoint

			this.ContextReturn();
		}

		private void toggleBreakpointToolStripMenuItem_Click( object sender, EventArgs e )
		{
			if( _contextIndex < 0 )
				return;
			Instruction instr = ( Instruction )this.Items[ _contextIndex ];

			this.Debugger.Breakpoints.ToggleBreakpoint( instr.BreakpointID );
			
			this.ContextReturn();
		}

		#endregion

		#region Line

		private void lineContextMenuStrip_Opening( object sender, CancelEventArgs e )
		{
			if( _hoveredIndex < 0 )
				return;
			_contextIndex = _hoveredIndex;
			if( ( _hoveredColumn == Column.Instruction ) &&
				( _hoveredValue != null ) )
			{
				if( _hoveredValue is CachedOperand )
					_contextOperand = ( ( CachedOperand )_hoveredValue ).Operand;
			}
			Instruction instr = ( Instruction )this.Items[ _hoveredIndex ];

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

				this.valueToolStripTextBox.Text = this.RequestOperandValue( instr, _contextOperand, false );
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
			if( _contextIndex < 0 )
				return;
			Instruction instr = ( Instruction )this.Items[ _contextIndex ];

			if( this.valueToolStripTextBox.Modified == true )
			{
				//Debug.WriteLine( "changed " + valueToolStripTextBox.Text );
				this.UpdateOperandValue( instr, _contextOperand, this.valueToolStripTextBox.Text );
			}
		}

		private void copyOperandToolStripMenuItem_Click( object sender, EventArgs e )
		{
			if( _contextIndex < 0 )
				return;
			Instruction instr = ( Instruction )this.Items[ _contextIndex ];

			Clipboard.Clear();
			Clipboard.SetText( this.RequestOperandValue( instr, _contextOperand, false ), TextDataFormat.Text );

			this.ContextReturn();
		}

		private void goToTargetToolStripMenuItem_Click( object sender, EventArgs e )
		{
			if( _contextIndex < 0 )
				return;
			Instruction instr = ( Instruction )this.Items[ _contextIndex ];

			this.ContextReturn();
		}

		#endregion

		#region Clipboard

		private void copyAddressToolStripMenuItem_Click( object sender, EventArgs e )
		{
			if( _contextIndex < 0 )
				return;
			Instruction instr = ( Instruction )this.Items[ _contextIndex ];

			Clipboard.Clear();
			Clipboard.SetText( string.Format( "{0:X8}", instr.Address ) );

			this.ContextReturn();
		}

		private void copyInstructionToolStripMenuItem_Click( object sender, EventArgs e )
		{
			if( _contextIndex < 0 )
				return;
			Instruction instr = ( Instruction )this.Items[ _contextIndex ];

			Clipboard.Clear();
			Clipboard.SetText( instr.ToString() );

			this.ContextReturn();
		}

		private void copyLineToolStripMenuItem_Click( object sender, EventArgs e )
		{
			if( _contextIndex < 0 )
				return;
			Instruction instr = ( Instruction )this.Items[ _contextIndex ];

			StringBuilder line = new StringBuilder();
			StringBuilder html = new StringBuilder();

			string args = instr.GetOperandsString();

			line.AppendFormat( "[{0:X8}] {1:X8} {2} {3}",
				instr.Address, instr.Code, instr.Opcode.ToString(), args );
			html.AppendFormat( "[<b><font color=\"blue\">{0:X8}</font></b>] <font color=\"grey\">{1:X8}</font> <b>{2}</b> {3}",
				instr.Address, instr.Code, instr.Opcode.ToString(), args );

			Clipboard.Clear();
			Clipboard.SetText( line.ToString(), TextDataFormat.Html );
			Clipboard.SetText( html.ToString(), TextDataFormat.Text );

			this.ContextReturn();
		}

		#endregion

		#region Control

		private void showNextToolStripMenuItem_Click( object sender, EventArgs e )
		{
			if( _contextIndex < 0 )
				return;
			Instruction instr = ( Instruction )this.Items[ _contextIndex ];

			this.ContextReturn();
		}

		private void runToCursorToolStripMenuItem_Click( object sender, EventArgs e )
		{
			if( _contextIndex < 0 )
				return;
			Instruction instr = ( Instruction )this.Items[ _contextIndex ];

			this.ContextReturn();
		}

		private void setNextToolStripMenuItem_Click( object sender, EventArgs e )
		{
			if( _contextIndex < 0 )
				return;
			Instruction instr = ( Instruction )this.Items[ _contextIndex ];

			this.ContextReturn();
		}

		#endregion

		#endregion

		private void ContextReturn()
		{
			if( _contextIndex < 0 )
				return;
			this.Invalidate( this.GetItemRectangle( _contextIndex ) );
			_contextIndex = -1;
			_contextOperand = null;
		}

		private void GeneralContextClosed( object sender, ToolStripDropDownClosedEventArgs e )
		{
			if( e.CloseReason != ToolStripDropDownCloseReason.ItemClicked )
				this.ContextReturn();
		}

		private string RequestOperandValue( Instruction instruction, Operand op, bool full )
		{
			string intFormat = _displayHex ? string.Format( "0x{{0:X{0}}}", op.Size * 2 ) : "{0}";
			switch( op.Type )
			{
				default:
					return "";
				case OperandType.Annotation:
					return op.Annotation;
				case OperandType.Immediate:
					if( full == true )
					{
						if( _displayHex == true )
							return string.Format( intFormat + " ({0})", op.Immediate );
						else
							return op.Immediate.ToString();
					}
					else
						return string.Format( intFormat, op.Immediate );
				case OperandType.ImmediateFloat:
					return op.ImmediateFloat.ToString();
				case OperandType.Register:
					if( op.Register.Format == RegisterFormat.Integer )
					{
						if( full == true )
						{
							if( _displayHex == true )
								return string.Format( "0x{0:X8} ({0})", this.GetRegister<uint>( op.Register ) );
							else
								return this.GetRegister<uint>( op.Register ).ToString();
						}
						else
							return string.Format( _displayHex ? "0x{0:X8}" : "{0}", this.GetRegister<uint>( op.Register ) );
					}
					else
						return string.Format( "{0}", this.GetRegister<float>( op.Register ) );
				case OperandType.VfpuRegister:
					return string.Format( "{0}", this.GetRegister<float>( op.Register ) );
				case OperandType.MemoryAccess:
					if( full == true )
					{
						uint reg = this.GetRegister<uint>( op.Register );
						return string.Format( "0x{0:X8} (0x{1:X8}{3}{2})", reg + op.Immediate, reg, op.Immediate, op.Immediate < 0 ? "" : "+" );
					}
					else
						return string.Format( "0x{0:X8}", this.GetRegister<uint>( op.Register ) + op.Immediate );
				case OperandType.BranchTarget:
					uint branchTarget = ( uint )( instruction.Address + op.Immediate + 4 );
					if( full == true )
					{
						return string.Format( "0x{0:X8} (0x{1:X8}{3}{2})", branchTarget, instruction.Address + 4, op.Immediate, op.Immediate < 0 ? "" : "+" );
					}
					else
						return string.Format( "0x{0:X8}", branchTarget );
				case OperandType.JumpTarget:
					if( op.Register == null )
						return string.Format( "0x{0:X8}", op.Immediate );
					else
						return string.Format( "0x{0:X8}", this.GetRegister<uint>( op.Register ) );
			}
		}

		private void UpdateOperandValue( Instruction instruction, Operand op, string value )
		{
			switch( op.Type )
			{
				case OperandType.Annotation:
				case OperandType.Immediate:
				case OperandType.ImmediateFloat:
					Debug.Assert( false );
					break;

				case OperandType.JumpTarget:
				case OperandType.Register:
					Debug.Assert( op.Register != null );
					if( op.Register != null )
					{
						if( op.Register.Format == RegisterFormat.Integer )
						{
							int ivalue;
							NumberStyles ns = NumberStyles.Integer;
							if( value.StartsWith( "0x" ) )
							{
								ns = NumberStyles.HexNumber;
								value = value.Substring( 2 );
							}
							else if( value.EndsWith( "h" ) )
							{
								ns = NumberStyles.HexNumber;
								value = value.Substring( 0, value.Length - 2 );
							}
							if( int.TryParse( value, ns, CultureInfo.InvariantCulture, out ivalue ) == true )
								this.SetRegister<uint>( op.Register, ( uint )ivalue );
						}
						else
						{
							float fvalue;
							if( float.TryParse( value, NumberStyles.Float, CultureInfo.InvariantCulture, out fvalue ) == true )
								this.SetRegister<float>( op.Register, fvalue );
						}
					}
					break;
				case OperandType.VfpuRegister:
					Debug.Assert( false, "not implemented yet" );
					break;

				case OperandType.MemoryAccess:
				case OperandType.BranchTarget:
					break;
			}
		}

		private T GetRegister<T>( Register register )
		{
			return this.Debugger.DebugHost.CpuHook.GetRegister<T>( register.Bank.Set, register.Ordinal );
		}

		private void SetRegister<T>( Register register, T value )
		{
			this.Debugger.DebugHost.CpuHook.SetRegister<T>( register.Bank.Set, register.Ordinal, value );

			if( this.RegisterValueChanged != null )
				this.RegisterValueChanged( this, EventArgs.Empty );
		}
	}
}
