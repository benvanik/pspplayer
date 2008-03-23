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

			this.disassemblyControl.Debugger = debugger;
			//this.disassemblyControl.RegisterValueChanged += new EventHandler( disassemblyControl_RegisterValueChanged );

			this.Disable();

			this.codeView.Setup( debugger );

			// TODO: bind to button
			this.codeView.UseHex = true;
		}

		public void InvalidateAll()
		{
			this.codeView.InvalidateAll();
		}

		public void Disable()
		{
			this.disassemblyControl.Enabled = false;
			this.disassemblyControl.ClearAddress();
			this.codeView.Enabled = false;
		}

		public void SetAddress( uint address, bool isCurrentStatement )
		{
			this.codeView.Enabled = true;
			this.codeView.SetAddress( address );
			
			
			this.disassemblyControl.Enabled = true;

			IDebugDatabase db = this.Debugger.DebugHost.Database;
			Debug.Assert( db != null );

			Method method = db.FindSymbol( address ) as Method;
			Debug.Assert( method != null );

			if( ( this.disassemblyControl.Enabled == true ) &&
				( this.disassemblyControl.MethodBody != null ) &&
				( this.disassemblyControl.MethodBody.Address == method.Address ) )
			{
				if( isCurrentStatement == true )
					this.disassemblyControl.SetCurrentAddress( address );
				else
					this.disassemblyControl.SetAddress( address );
			}
			else
			{
				MethodBody methodBody = this.BuildMethodBody( method );
				Debug.Assert( methodBody != null );

				this.disassemblyControl.SetMethod( methodBody );
				if( isCurrentStatement == true )
					this.disassemblyControl.SetCurrentAddress( address );
				else
					this.disassemblyControl.SetAddress( address );

				//this.ShowRegisters( this.CurrentRegisterSet );
			}
		}

		private MethodBody BuildMethodBody( Method method )
		{
			Debug.Assert( this.Debugger.DebugHost.CpuHook != null );
			uint[] codes = this.Debugger.DebugHost.CpuHook.GetMethodBody( method );

			uint instrAddress = method.Address;
			List<Instruction> instrs = new List<Instruction>( ( int )method.Length / 4 );
			for( int n = 0; n < codes.Length; n++ )
			{
				Instruction instr = new Instruction( instrAddress, codes[ n ] );
				instrs.Add( instr );
				instrAddress += 4;
			}
			MethodBody methodBody = new MethodBody( method.Address, 4 * ( uint )method.Length, instrs.ToArray() );

			return methodBody;
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

		//private void TestData()
		//{
		//    uint[] codes = new uint[]{
		//        0x49170001,
		//        0xde00083f,
		//        0x10400004,
		//        0xcba80002,
		//        0xd0070068,
		//        0x64484828,
		//        0xdc08f096,
		//        0xd0008484,
		//        0xdc07f918,
		//        0xd0008585,
		//        0x64482808,
		//        0xdc00f004,
		//        0xd0008686,
		//        0xdc00f001,
		//        0xd0008787,
		//        0xf188a489,
		//        0xf189a08a,
		//        0xe88a0000,
		//        0xe88a0005,
		//        0xe88a000a,
		//        0x03e00008,
		//        0x27bd0010,
		//        0x00801021,
		//        0x44086000,
		//        0x48e80001,
		//        0xc8a00000,
		//        0xc8a00005,
		//        0xc8a0000a,
		//        0x65018000,
		//    };

		//    uint address = 0x08010000;
		//    List<Instruction> instrs = new List<Instruction>();
		//    for( int m = 0; m < 10; m++ )
		//    {
		//        for( int n = 0; n < codes.Length; n++ )
		//        {
		//            Instruction instr = new Instruction( address, codes[ n ] );
		//            instrs.Add( instr );

		//            address += 4;
		//        }
		//    }
		//    MethodBody b = new MethodBody( address, 4 * ( uint )instrs.Count, instrs.ToArray() );

		//    this.disassemblyControl.SetMethod( b );
		//}
	}
}
