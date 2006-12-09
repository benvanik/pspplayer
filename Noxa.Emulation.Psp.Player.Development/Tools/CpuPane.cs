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
using System.Text;
using System.Windows.Forms;

using Noxa.Utilities.Controls;

using Noxa.Emulation.Psp.Cpu;

namespace Noxa.Emulation.Psp.Player.Development.Tools
{
	partial class CpuPane : Noxa.Emulation.Psp.Player.Development.Tools.ToolPane
	{
		private Studio _studio;
		private ICpu _cpu;
		private ICpuCore _core0;

		private static string[] GeneralRegisterLabels = new string[]{
			" $0", "$at", "$v0", "$v1", "$a0", "$a1", "$a2", "$a3", "$t0", "$t1", "$t2", "$t3", "$t4", "$t5", "$t6", "$t7",
			"$s0", "$s1", "$s2", "$s3", "$s4", "$s5", "$s6", "$s7", "$t8", "$t9", "$k0", "$k1", "$gp", "$sp", "$fp", "$ra", 
		};

		private static string[] Cp0RegisterLabels = new string[]{
			"$0", "$1", "$2", "$3", "$4", "$5", "$6", "$7", "$8", "$9", "$10", "$11", "$12", "$13", "$14", "$15",
			"$16", "$17", "$18", "$19", "$20", "$21", "$22", "$23", "$24", "$25", "$26", "$27", "$28", "$29", "$30", "$31", 
		};

		private bool _useFriendlyLabels;

		public CpuPane()
		{
			InitializeComponent();
		}

		public CpuPane( Studio studio )
			: this()
		{
			Debug.Assert( studio != null );
			if( studio == null )
				throw new ArgumentNullException( "studio" );
			_studio = studio;

			this.Icon = IconUtilities.ConvertToIcon( Properties.Resources.RegistersIcon );

			_cpu = _studio.Debugger.Host.CurrentInstance.Cpu;
			_core0 = _cpu.Cores[ 0 ];

			_studio.GlobalRefreshRequested += new EventHandler( StudioGlobalRefreshRequested );
			_cpu.BreakpointTriggered += new EventHandler<Noxa.Emulation.Psp.Debugging.BreakpointEventArgs>( CpuBreakpointTriggered );

			this.generalRegistersLabel_SizeChanged( this, EventArgs.Empty );

			_useFriendlyLabels = true;
			this.friendlyCheckbox.Checked = _useFriendlyLabels;

			this.UpdateValues();
		}

		private void StudioGlobalRefreshRequested( object sender, EventArgs e )
		{
			this.UpdateValues();
		}

		private void CpuBreakpointTriggered( object sender, Noxa.Emulation.Psp.Debugging.BreakpointEventArgs e )
		{
			this.UpdateValues();
		}

		private delegate void DummyDelegate();
		public void UpdateValues()
		{
			if( this.InvokeRequired == true )
				this.Invoke( new DummyDelegate( this.UpdateValuesInternal ) );
			else
				this.UpdateValuesInternal();
		}

		private void UpdateValuesInternal()
		{
			CoreState state = _core0.State;

			this.pcLabel.Text = string.Format( "0x{0:X8}", state.ProgramCounter );

			{
				StringBuilder sb = new StringBuilder();

				int perLine = generalRegistersLabel.ClientSize.Width / _generalRegistersUnitWidth;
				if( perLine == 0 )
					perLine = 1;

				string format;
				if( _studio.UseHex == true )
					format = "{0,3}: 0x{1:X8} ";
				else
					format = "{0,3}: {1,10} ";

				int thisLine = 0;
				sb.AppendFormat( format, "$HI", state.Hi );
				thisLine++;
				if( thisLine >= perLine )
				{
					thisLine = 0;
					sb.AppendLine();
				}
				sb.AppendFormat( format, "$LO", state.Lo );
				thisLine++;
				if( thisLine >= perLine )
				{
					thisLine = 0;
					sb.AppendLine();
				}
				sb.AppendFormat( "{0,3}: {1,10} ", "$LL", state.LL ? 1 : 0 );
				thisLine = 0;
				sb.AppendLine();

				for( int n = 0; n < 32; n++ )
				{
					string label;
					if( _useFriendlyLabels == true )
						label = GeneralRegisterLabels[ n ];
					else
						label = string.Format( "${0}", n );
					int value = state.GeneralRegisters[ n ];

					sb.AppendFormat( format, label, value );
					thisLine++;
					if( thisLine >= perLine )
					{
						thisLine = 0;
						sb.AppendLine();
					}
				}

				generalRegistersLabel.Text = sb.ToString();
			}

			{
				StringBuilder sb = new StringBuilder();

				int perLine = generalRegistersLabel.ClientSize.Width / _generalRegistersUnitWidth;
				if( perLine == 0 )
					perLine = 1;

				string format;
				if( _studio.UseHex == true )
					format = "{0,3}: 0x{1:X8} ";
				else
					format = "{0,3}: {1,10} ";

				int thisLine = 0;
				sb.AppendFormat( "{0}: {1,10} ", "COND", state.Cp0ConditionBit ? 1 : 0 );
				sb.AppendLine();

				sb.AppendLine( "General:" );
				for( int n = 0; n < 32; n++ )
				{
					string label;
					if( _useFriendlyLabels == true )
						label = Cp0RegisterLabels[ n ];
					else
						label = string.Format( "${0}", n );
					int value = state.Cp0Registers[ n ];

					sb.AppendFormat( format, label, value );
					thisLine++;
					if( thisLine >= perLine )
					{
						thisLine = 0;
						sb.AppendLine();
					}
				}
				if( thisLine != 0 )
					sb.AppendLine();

				sb.AppendLine( "Control:" );
				for( int n = 0; n < 32; n++ )
				{
					string label;
					//if( _useFriendlyLabels == true )
					//	label = Cp0RegisterLabels[ n ];
					//else
					label = string.Format( "${0}", n );
					int value = state.Cp0ControlRegisters[ n ];

					sb.AppendFormat( format, label, value );
					thisLine++;
					if( thisLine >= perLine )
					{
						thisLine = 0;
						sb.AppendLine();
					}
				}

				cp0RegistersLabel.Text = sb.ToString();
			}

			{
				StringBuilder sb = new StringBuilder();

				int perLine = generalRegistersLabel.ClientSize.Width / _generalRegistersUnitWidth;
				if( perLine == 0 )
					perLine = 1;

				string format = "{0,3}: {1,10} ";

				int thisLine = 0;
				sb.AppendFormat( "{0}: 0x{1:X8} ", "CTRL", state.FpuControlRegister );
				sb.AppendLine();

				for( int n = 0; n < 32; n++ )
				{
					string label = string.Format( "${0}", n );
					float value = state.FpuRegisters[ n ];

					sb.AppendFormat( format, label, value );
					thisLine++;
					if( thisLine >= perLine )
					{
						thisLine = 0;
						sb.AppendLine();
					}
				}

				fpuRegistersLabel.Text = sb.ToString();
			}
		}

		private int _generalRegistersUnitWidth;

		private void generalRegistersLabel_SizeChanged( object sender, EventArgs e )
		{
			string test = " $0: 0x00000000 ";
			using( Graphics g = Graphics.FromHwnd( this.generalRegistersLabel.Handle ) )
			{
				SizeF size = g.MeasureString( test, this.generalRegistersLabel.Font );
				_generalRegistersUnitWidth = ( int )size.Width;
			}

			this.UpdateValues();
		}

		private void friendlyCheckbox_CheckedChanged( object sender, EventArgs e )
		{
			_useFriendlyLabels = friendlyCheckbox.Checked;
			this.UpdateValues();
		}
	}
}

