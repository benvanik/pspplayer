// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2008 Ben Vanik (noxa)
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
using Noxa.Emulation.Psp.Debugging.DebugData;
using Noxa.Emulation.Psp.Debugging.DebugModel;
using Noxa.Emulation.Psp.Player.Debugger.Model;

namespace Noxa.Emulation.Psp.Player.Debugger.Tools
{
	partial class CodeTool : Noxa.Emulation.Psp.Player.Debugger.DebuggerTool
	{
		public CodeTool()
		{
			this.InitializeComponent();
		}

		public CodeTool( InprocDebugger debugger )
			: base( debugger )
		{
			this.InitializeComponent();

			Bitmap image = Properties.Resources.DisassemblyIcon as Bitmap;
			this.Icon = Icon.FromHandle( image.GetHicon() );

			this.Disable();

			this.codeView.Setup( debugger );
			this.registersControl.Setup( debugger );

			// TODO: bind to button
			this.codeView.UseHex = true;
		}

		public void InvalidateAll()
		{
			this.codeView.InvalidateAll();
			this.registersControl.Invalidate();
		}

		public void Disable()
		{
			this.codeView.Enabled = false;
			this.registersControl.Enabled = false;
			this.registersControl.Invalidate();
		}

		public void SetAddress( uint address, bool isCurrentStatement )
		{
			this.codeView.Enabled = true;
			this.codeView.SetAddress( address );
			this.codeView.Focus();
			this.registersControl.Invalidate();
		}

		public void ShowNextStatement()
		{
			CoreState state = this.Debugger.DebugHost.CpuHook.GetCoreState( 0 );
			this.SetAddress( state.ProgramCounter, true );
		}

		#region Registers

		//private void ShowRegisters( RegisterSet set )
		//{
		//    CoreState state = this.Debugger.Host.CpuHook.GetCoreState( 0 );
		//    this.pcTextBox.Text = string.Format( "0x{0:X8}", state.ProgramCounter );

		//    if( this.CurrentRegisterSet != set )
		//    {
		//        this.CurrentRegisterSet = set;
		//        switch( set )
		//        {
		//            case RegisterSet.Gpr:
		//                this.registerToggleToolStripSplitButton.Text = "GPR";
		//                break;
		//            case RegisterSet.Fpu:
		//                this.registerToggleToolStripSplitButton.Text = "FPU";
		//                break;
		//            case RegisterSet.Vfpu:
		//                this.registerToggleToolStripSplitButton.Text = "VFPU";
		//                break;
		//        }
		//    }

		//    RegisterBank bank;
		//    switch( set )
		//    {
		//        default:
		//        case RegisterSet.Gpr:
		//            bank = RegisterBanks.General;
		//            break;
		//        case RegisterSet.Fpu:
		//            bank = RegisterBanks.Fpu;
		//            break;
		//        case RegisterSet.Vfpu:
		//            bank = RegisterBanks.Vfpu;
		//            break;
		//    }

		//    this.registersListView.BeginUpdate();
		//    this.registersListView.Items.Clear();
		//    foreach( Register register in bank.Registers )
		//    {
		//        string prettyValue = string.Empty;
		//        string rawValue = string.Empty;
		//        switch( set )
		//        {
		//            case RegisterSet.Gpr:
		//                uint uv = this.Debugger.Host.CpuHook.GetRegister<uint>( set, register.Ordinal );
		//                prettyValue = uv.ToString();
		//                rawValue = string.Format( "{0:X8}", uv );
		//                break;
		//            case RegisterSet.Fpu:
		//            case RegisterSet.Vfpu:
		//                float fv = this.Debugger.Host.CpuHook.GetRegister<float>( set, register.Ordinal );
		//                prettyValue = fv.ToString();
		//                break;
		//        }
		//        ListViewItem item = new ListViewItem( new string[]{
		//            register.ToString(), prettyValue, rawValue,
		//        } );
		//        this.registersListView.Items.Add( item );
		//    }
		//    this.registersListView.EndUpdate();
		//}

		//private void disassemblyControl1_RegisterValueChanged( object sender, EventArgs e )
		//{
		//    this.ShowRegisters( this.CurrentRegisterSet );
		//}

		//private void registerToggleToolStripSplitButton_ButtonClick( object sender, EventArgs e )
		//{
		//    switch( this.CurrentRegisterSet )
		//    {
		//        case RegisterSet.Gpr:
		//            this.ShowRegisters( RegisterSet.Fpu );
		//            break;
		//        case RegisterSet.Fpu:
		//            this.ShowRegisters( RegisterSet.Vfpu );
		//            break;
		//        case RegisterSet.Vfpu:
		//            this.ShowRegisters( RegisterSet.Gpr );
		//            break;
		//    }
		//}

		//private void generalRegistersToolStripMenuItem_Click( object sender, EventArgs e )
		//{
		//    this.ShowRegisters( RegisterSet.Gpr );
		//}

		//private void fPURegistersToolStripMenuItem_Click( object sender, EventArgs e )
		//{
		//    this.ShowRegisters( RegisterSet.Fpu );
		//}

		//private void vFPURegistersToolStripMenuItem_Click( object sender, EventArgs e )
		//{
		//    this.ShowRegisters( RegisterSet.Vfpu );
		//}

		#endregion
	}
}
