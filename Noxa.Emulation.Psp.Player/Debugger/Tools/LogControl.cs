// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2008 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Noxa.Emulation.Psp.Player.Debugger.Tools
{
	partial class LogControl : Control
	{
		class LogLine
		{
			public readonly uint ThreadID;
			public readonly Verbosity Verbosity;
			public readonly Feature Feature;
			public readonly string Value;
			public LogLine( uint threadId, Verbosity verbosity, Feature feature, string value )
			{
				this.ThreadID = threadId;
				this.Verbosity = verbosity;
				this.Feature = feature;
				this.Value = value;
			}
		}

		public InprocDebugger Debugger;
		private VScrollBar _verticalScrollBar;

		private readonly string[] _featureNames;

		private List<LogLine> _lines = new List<LogLine>( 20000 );
		private int _firstVisibleLine;
		private int _lastVisibleLine;
		private int _visibleLines;

		public LogControl()
		{
			this.InitializeComponent();

			this.SetStyle( ControlStyles.UserPaint, true );
			this.SetStyle( ControlStyles.AllPaintingInWmPaint, true );
			this.SetStyle( ControlStyles.ResizeRedraw, true );
			this.SetStyle( ControlStyles.Opaque, true );
			this.SetStyle( ControlStyles.OptimizedDoubleBuffer, true );

			_verticalScrollBar = new VScrollBar();
			_verticalScrollBar.Scroll += new ScrollEventHandler( _verticalScrollBar_Scroll );
			_verticalScrollBar.Dock = DockStyle.Right;
			_verticalScrollBar.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
			_verticalScrollBar.Left = this.ClientSize.Width - 2 - _verticalScrollBar.Width;
			_verticalScrollBar.Top = 2;
			_verticalScrollBar.Height = this.ClientSize.Height - 4;
			this.Controls.Add( _verticalScrollBar );

			this.SetupGraphics();
			this.CalculateSize( true );

			this.copyAllToolStripMenuItem.Click += new EventHandler( copyAllToolStripMenuItem_Click );
			this.clearToolStripMenuItem.Click += new EventHandler( clearToolStripMenuItem_Click );
			this.ContextMenuStrip = contextMenuStrip;

			// Build feature list
			List<string> featureNames = new List<string>();
			foreach( string feature in Enum.GetNames( typeof( Feature ) ) )
				featureNames.Add( new string( ' ', 10 - feature.Length ) + feature );
			_featureNames = featureNames.ToArray();
		}

		#region Context Menu

		private void copyAllToolStripMenuItem_Click( object sender, EventArgs e )
		{
			StringBuilder sb = new StringBuilder();
			lock( _lines )
			{
				foreach( LogLine line in _lines )
					sb.AppendFormat( "{0}: {1}{2}", _featureNames[ ( int )line.Feature ], line.Value, Environment.NewLine );
			}
			Clipboard.Clear();
			Clipboard.SetText( sb.ToString() );
		}

		private void clearToolStripMenuItem_Click( object sender, EventArgs e )
		{
			lock( _lines )
				_lines.Clear();
			this.CalculateSize( true );
		}

		#endregion

		private int _pendingUpdates;

		public void AddLine( Verbosity verbosity, Feature feature, string value )
		{
			uint threadId = ( this.Debugger.DebugHost.BiosHook != null ) ? this.Debugger.DebugHost.BiosHook.ActiveThreadID : 0;
			LogLine line = new LogLine( threadId, verbosity, feature, value );
			lock( _lines )
				_lines.Add( line );
			if( Interlocked.Increment( ref _pendingUpdates ) > 1 )
				return;
			this.BeginInvoke( ( DummyDelegate )this.SafeAddLine );
		}

		private void SafeAddLine()
		{
			_pendingUpdates = 0;

			this.CalculateSize( false );

			// if autoscroll
			if( true )
			{
				this.ScrollToLine( Math.Max( 0, _lines.Count - _visibleLines ) );
			}
		}

		#region Sizing

		protected override void OnResize( EventArgs e )
		{
			base.OnResize( e );

			this.CalculateSize( true );
		}

		private void CalculateSize( bool invalidate )
		{
			_verticalScrollBar.Minimum = 0;
			_verticalScrollBar.Maximum = _lines.Count + 1 - _visibleLines;
			_visibleLines = ( int )Math.Ceiling( ( double )( this.ClientRectangle.Height / _lineHeight ) );

			if( this.UpdateVisibleLines() == true )
			{
				if( invalidate == true )
					this.Invalidate();
			}
		}

		#endregion

		#region Focus/Input

		protected override void OnMouseDown( MouseEventArgs e )
		{
			base.OnMouseDown( e );
			this.Focus();
		}

		#endregion

		#region Scrolling/navigation

		private int _scrollLine;

		private bool ScrollToLine( int targetLine )
		{
			if( _scrollLine == targetLine )
				return false;
			_scrollLine = targetLine;
			if( _verticalScrollBar.Maximum < _lines.Count )
				_verticalScrollBar.Maximum = _lines.Count + 1 - _visibleLines;
			_verticalScrollBar.Value = Math.Min( _verticalScrollBar.Maximum, targetLine );
			if( this.UpdateVisibleLines() == true )
			{
				_verticalScrollBar.Invalidate();
				this.Invalidate();
			}
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
					targetLine = _lines.Count - _visibleLines;
					break;
				case ScrollEventType.LargeDecrement:
					targetLine = Math.Max( 0, targetLine - _visibleLines - 4 );
					break;
				case ScrollEventType.LargeIncrement:
					targetLine = Math.Min( _lines.Count - _visibleLines, targetLine + _visibleLines - 4 );
					break;
				case ScrollEventType.SmallDecrement:
					if( targetLine > 0 )
						targetLine--;
					break;
				case ScrollEventType.SmallIncrement:
					if( targetLine < ( _lines.Count - _visibleLines ) )
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
			targetLine = Math.Max( Math.Min( targetLine, _lines.Count - _visibleLines ), 0 );

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
						targetLine = _lines.Count - _visibleLines;
					else
					{
						// TODO: jump to current method bottom
					}
					break;
				case Keys.PageUp:
					targetLine = Math.Max( 0, targetLine - _visibleLines - 4 );
					break;
				case Keys.PageDown:
					targetLine = Math.Min( _lines.Count - _visibleLines, targetLine + _visibleLines - 4 );
					break;
				case Keys.Up:
					if( targetLine > 0 )
						targetLine--;
					break;
				case Keys.Down:
					if( targetLine < ( _lines.Count - _visibleLines ) )
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
			int lastVisibleLine = Math.Min( _lines.Count - 1, firstVisibleLine + _visibleLines );
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

		internal Font _font = new Font( "Courier New", 8.0f, FontStyle.Regular );
		//internal Font _font = new Font( "Consolas", 8.0f, FontStyle.Bold );
		internal Brush _valueFontBrush = SystemBrushes.ControlText;
		internal Brush _disabledFontBrush = SystemBrushes.GrayText;

		// Critical = 0, Normal = 1, Verbose = 2
		internal Brush[] _labelFontBrushes = new Brush[] { Brushes.Red, Brushes.Black, Brushes.Green };

		private StringFormat _stringFormat;
		private SizeF _charSize;
		private int _lineSpacing;
		private int _lineHeight;

		private Pen _vertGridPen;
		private Brush _labelBrush;
		private Brush _valueBrush;
		private Brush _disabledBrush;

		private int _labelWidth = 100;
		private int _threadWidth = 30;

		private void SetupGraphics()
		{
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
			_labelBrush = new SolidBrush( light );
			_valueBrush = SystemBrushes.Window;
			_disabledBrush = new SolidBrush( light );

			_labelWidth = ( int )( _charSize.Width * 10 + 2 + 0.5f );
			_threadWidth = ( int )( _charSize.Width * 3 + 4 + 0.5f );
		}

		protected override void OnPaintBackground( PaintEventArgs e )
		{
			Brush backBrush = ( this.Enabled == true ) ? _valueBrush : _disabledBrush;

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
			// Label
			e.Graphics.FillRectangle( _labelBrush, 2, 2, _labelWidth, this.ClientRectangle.Height - 4 );
			e.Graphics.DrawLine( _vertGridPen,
				2 + _labelWidth, 2,
				2 + _labelWidth, this.ClientRectangle.Height - 3 );

			// Thread
			e.Graphics.DrawLine( _vertGridPen,
				2 + _labelWidth + 2 + _threadWidth, 2,
				2 + _labelWidth + 2 + _threadWidth, this.ClientRectangle.Height - 3 );
		}

		protected override void OnPaint( PaintEventArgs e )
		{
			this.OnPaintBackground( e );

			e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;

			int x = 2;
			int y = 2;

			Brush valueBrush = ( this.Enabled == true ) ? _valueFontBrush : _disabledFontBrush;

			for( int n = _firstVisibleLine; n <= _lastVisibleLine; n++ )
			{
				LogLine line = _lines[ n ];

				// Label
				e.Graphics.DrawString( _featureNames[ ( int )line.Feature ], _font, _labelFontBrushes[ ( int )line.Verbosity ], x, y, _stringFormat );

				// Thread ID
				e.Graphics.DrawString( line.ThreadID.ToString( "X3" ), _font, valueBrush, x + _labelWidth + 4, y, _stringFormat );

				// Value
				e.Graphics.DrawString( line.Value, _font, valueBrush, x + _labelWidth + 4 + _threadWidth + 3, y, _stringFormat );

				y += _lineHeight;
			}
		}

		#endregion
	}
}
