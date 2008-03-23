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
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Noxa.Emulation.Psp.Debugging.DebugModel;
using Noxa.Emulation.Psp.Player.Debugger.Model;

namespace Noxa.Emulation.Psp.Player.Debugger.Tools
{
	partial class RegistersControl : Control
	{
		private InprocDebugger _debugger;

		public RegistersControl()
		{
			this.InitializeComponent();

			this.SetStyle( ControlStyles.UserPaint, true );
			this.SetStyle( ControlStyles.AllPaintingInWmPaint, true );
			this.SetStyle( ControlStyles.ResizeRedraw, true );
			this.SetStyle( ControlStyles.Opaque, true );
			this.SetStyle( ControlStyles.OptimizedDoubleBuffer, true );

			this.SetupGraphics();
		}

		public void Setup( InprocDebugger debugger )
		{
			_debugger = debugger;
		}

		#region Painting

		internal Font _font = new Font( "Courier New", 8.0f, FontStyle.Regular );
		//internal Font _font = new Font( "Consolas", 8.0f, FontStyle.Bold );
		internal Brush _labelFontBrush = Brushes.Blue;
		internal Brush _valueFontBrush = SystemBrushes.ControlText;
		internal Brush _disabledFontBrush = SystemBrushes.GrayText;

		private StringFormat _stringFormat;
		private SizeF _charSize;
		private int _lineSpacing;
		private int _lineHeight;

		private Pen _vertGridPen;
		private Brush _labelBrush;
		private Brush _valueBrush;
		private Brush _disabledBrush;

		private int _labelWidth = 100;

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

			_labelWidth = ( int )( _charSize.Width * 3 + 2 + 0.5f );
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
		}

		protected override void OnPaint( PaintEventArgs e )
		{
			this.OnPaintBackground( e );
			if( _debugger == null )
				return;

			e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;

			int x = 2;
			int y = 2;

			Brush labelBrush = ( this.Enabled == true ) ? _labelFontBrush : _disabledFontBrush;
			Brush valueBrush = ( this.Enabled == true ) ? _valueFontBrush : _disabledFontBrush;

			CoreState state = _debugger.DebugHost.CpuHook.GetCoreState( 0 );

			// PC
			e.Graphics.DrawString( "PC", _font, labelBrush, x, y, _stringFormat );
			e.Graphics.DrawString( state.ProgramCounter.ToString( "X8" ), _font, valueBrush, x + _labelWidth + 3, y, _stringFormat );
			y += _lineHeight;

			DrawMode mode = DrawMode.General;
			switch( mode )
			{
				case DrawMode.General:
					{
						RegisterBank bank = RegisterBanks.General;
						for( int n = 1; n < 32; n++ )
						{
							Register reg = bank.Registers[ n ];

							// Label
							e.Graphics.DrawString( reg.Name, _font, labelBrush, x, y, _stringFormat );

							// Value
							string value = state.GeneralRegisters[ n ].ToString( "X8" );
							e.Graphics.DrawString( value, _font, valueBrush, x + _labelWidth + 3, y, _stringFormat );

							y += _lineHeight;
						}

						// LO
						e.Graphics.DrawString( "$lo", _font, labelBrush, x, y, _stringFormat );
						e.Graphics.DrawString( state.Lo.ToString( "X8" ), _font, valueBrush, x + _labelWidth + 3, y, _stringFormat );
						y += _lineHeight;

						// HI
						e.Graphics.DrawString( "$hi", _font, labelBrush, x, y, _stringFormat );
						e.Graphics.DrawString( state.Hi.ToString( "X8" ), _font, valueBrush, x + _labelWidth + 3, y, _stringFormat );
						y += _lineHeight;
					}
					break;
				case DrawMode.Fpu:
					{
						// Control register
						e.Graphics.DrawString( "fcr", _font, labelBrush, x, y, _stringFormat );
						e.Graphics.DrawString( state.FpuControlRegister.ToString( "X8" ), _font, valueBrush, x + _labelWidth + 3, y, _stringFormat );
						y += _lineHeight;

						// Condition bit
						e.Graphics.DrawString( "fc", _font, labelBrush, x, y, _stringFormat );
						e.Graphics.DrawString( state.FpuConditionBit.ToString(), _font, valueBrush, x + _labelWidth + 3, y, _stringFormat );
						y += _lineHeight;

						// Registers
						RegisterBank bank = RegisterBanks.Fpu;
						for( int n = 0; n < 32; n++ )
						{
							Register reg = bank.Registers[ n ];

							// Label
							e.Graphics.DrawString( reg.Name, _font, labelBrush, x, y, _stringFormat );

							// Value
							string value = state.FpuRegisters[ n ].ToString();
							e.Graphics.DrawString( value, _font, valueBrush, x + _labelWidth + 3, y, _stringFormat );

							y += _lineHeight;
						}
					}
					break;
				case DrawMode.Vfpu:
					break;
			}
		}

		enum DrawMode
		{
			General,
			Fpu,
			Vfpu,
		}

		#endregion
	}
}
