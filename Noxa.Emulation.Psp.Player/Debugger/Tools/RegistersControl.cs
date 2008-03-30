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
using System.Globalization;
using System.Media;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Noxa.Emulation.Psp.Debugging.DebugModel;
using Noxa.Emulation.Psp.Debugging.Hooks;
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
			this.SetupEditing();
		}

		public void Setup( InprocDebugger debugger )
		{
			_debugger = debugger;
		}

		enum RegisterType
		{
			General,
			Fpu,
			Vfpu,
		}

		#region Editing

		private TextBox _editBox;
		private bool _isEditing;
		private RegisterType _editType;
		private int _editOrdinal;

		private void SetupEditing()
		{
			_editBox = new TextBox();
			_editBox.Font = _font;
			_editBox.Height = _lineHeight + 2;
			_editBox.Visible = false;
			_editBox.KeyDown += new KeyEventHandler( _editBox_KeyDown );
			_editBox.LostFocus += new EventHandler( _editBox_LostFocus );
			this.Controls.Add( _editBox );
		}

		protected override void OnMouseUp( MouseEventArgs e )
		{
			base.OnMouseUp( e );

			if( e.X < 2 + _labelWidth )
				return;

			// Determine line and such
			RegisterType type = RegisterType.General;
			int line = ( e.Y - 2 ) / _lineHeight;

			// Ignore PC
			if( line == 0 )
				return;

			this.EditRegister( type, line );
		}

		private void EditRegister( RegisterType type, int ordinal )
		{
			if( _isEditing == true )
				this.SaveEdit();
			_isEditing = true;
			_editType = type;
			_editOrdinal = ordinal;

			// Determine top
			int line = 1; // PC
			switch( type )
			{
				case RegisterType.General:
					line += ordinal - 1; // no $0
					break;
				case RegisterType.Fpu:
					line += 2 + ordinal;
					break;
				case RegisterType.Vfpu:
					line += ordinal; // TODO
					break;
			}

			_editBox.Left = 2 + _labelWidth - 3;
			_editBox.Top = _lineHeight * line - 1;
			_editBox.Width = this.ClientRectangle.Width - _editBox.Left;
			_editBox.Text = this.GetRegisterValue( type, ordinal );
			_editBox.Visible = true;
			_editBox.Focus();
			_editBox.SelectAll();
		}

		private void SaveEdit()
		{
			if( _isEditing == false )
				return;
			string value = _editBox.Text;
			bool result = this.SetRegisterValue( _editType, _editOrdinal, value );
			if( result == false )
			{
				SystemSounds.Exclamation.Play();
			}
			_isEditing = false;
			_editBox.Visible = false;
			this.Invalidate();
		}

		private void CancelEdit()
		{
			_isEditing = false;
			_editBox.Visible = false;
			this.Invalidate();
		}

		private void _editBox_KeyDown( object sender, KeyEventArgs e )
		{
			switch( e.KeyCode )
			{
				case Keys.Escape:
					e.Handled = true;
					this.CancelEdit();
					break;
				case Keys.Enter:
					e.Handled = true;
					this.SaveEdit();
					break;
			}
		}

		private void _editBox_LostFocus( object sender, EventArgs e )
		{
			this.SaveEdit();
		}

		private string GetRegisterValue( RegisterType type, int ordinal )
		{
			switch( type )
			{
				default:
				case RegisterType.General:
					return _debugger.DebugHost.CpuHook.GetRegister<uint>( RegisterSet.Gpr, ordinal ).ToString( "X8" );
				case RegisterType.Fpu:
					return _debugger.DebugHost.CpuHook.GetRegister<float>( RegisterSet.Fpu, ordinal ).ToString();
				case RegisterType.Vfpu:
					return _debugger.DebugHost.CpuHook.GetRegister<float>( RegisterSet.Vfpu, ordinal ).ToString();
			}
		}

		private bool SetRegisterValue( RegisterType type, int ordinal, string value )
		{
			switch( type )
			{
				default:
				case RegisterType.General:
					{
						uint uintValue;
						if( value.EndsWith( ".d" ) == true )
						{
							if( value.Length == 2 )
								return false;
							value = value.Substring( 0, value.Length - 2 );
							int intValue;
							if( int.TryParse( value, out intValue ) == false )
								return false;
							uintValue = ( uint )intValue;
						}
						else
						{
							if( uint.TryParse( value, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out uintValue ) == false )
								return false;
						}
						_debugger.DebugHost.CpuHook.SetRegister<uint>( RegisterSet.Gpr, ordinal, uintValue );
						return true;
					}
				case RegisterType.Fpu:
					{
						float floatValue;
						if( float.TryParse( value, out floatValue ) == false )
							return false;
						_debugger.DebugHost.CpuHook.SetRegister<float>( RegisterSet.Fpu, ordinal, floatValue );
						return true;
					}
				case RegisterType.Vfpu:
					{
						float floatValue;
						if( float.TryParse( value, out floatValue ) == false )
							return false;
						_debugger.DebugHost.CpuHook.SetRegister<float>( RegisterSet.Vfpu, ordinal, floatValue );
						return true;
					}
			}
		}

		#endregion

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
			if( _debugger.DebugHost.CpuHook == null )
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

			RegisterType mode = RegisterType.General;
			switch( mode )
			{
				case RegisterType.General:
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
				case RegisterType.Fpu:
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
				case RegisterType.Vfpu:
					break;
			}
		}

		#endregion
	}
}
