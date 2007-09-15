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
using WeifenLuo.WinFormsUI.Docking;

using Noxa.Emulation.Psp.RemoteDebugger.Model;
using Noxa.Emulation.Psp.Debugging.DebugData;
using Noxa.Emulation.Psp.Debugging.DebugModel;
using Noxa.Emulation.Psp.Debugging.Hooks;

namespace Noxa.Emulation.Psp.RemoteDebugger
{
	partial class CodeView : DockContent
	{
		public EmuDebugger Debugger;

		public RegisterSet CurrentRegisterSet = RegisterSet.Gpr;

		public CodeView()
		{
			InitializeComponent();
		}

		public CodeView( EmuDebugger debugger )
			: this()
		{
			this.Debugger = debugger;

			this.LoadSettings();

			this.disassemblyControl1.Debugger = debugger;
			this.disassemblyControl1.RegisterValueChanged += new EventHandler( disassemblyControl1_RegisterValueChanged );

			this.Disable();
		}

		private void LoadSettings()
		{
			// Hex by default
			this.hexToolStripButton.Checked = false;
			this.hexToolStripButton_Click( this, EventArgs.Empty );
		}

		private void SaveSettings()
		{
		}

		private void hexToolStripButton_Click( object sender, EventArgs e )
		{
			this.hexToolStripButton.Checked = !this.hexToolStripButton.Checked;
			this.disassemblyControl1.DisplayHex = hexToolStripButton.Checked;
		}

		public void Disable()
		{
			this.disassemblyControl1.Enabled = false;
			this.splitContainer1.Panel1.Enabled = false;
			this.splitContainer1.Panel2.Enabled = false;
		}

		public void SetAddress( uint address )
		{
			this.disassemblyControl1.Enabled = true;
			this.splitContainer1.Panel1.Enabled = true;
			this.splitContainer1.Panel2.Enabled = true;

			IDebugDatabase db = Debugger.Host.Database;
			Debug.Assert( db != null );

			Method method = db.FindSymbol( address ) as Method;
			Debug.Assert( method != null );

			if( ( this.disassemblyControl1.Enabled == true ) &&
				( this.disassemblyControl1.MethodBody != null ) &&
				( this.disassemblyControl1.MethodBody.Address == method.Address ) )
			{
				this.disassemblyControl1.SetAddress( address );
			}
			else
			{
				MethodBody methodBody = this.BuildMethodBody( method );
				Debug.Assert( methodBody != null );

				this.disassemblyControl1.SetMethod( methodBody );
				this.disassemblyControl1.SetAddress( address );

				this.ShowRegisters( this.CurrentRegisterSet );
			}
		}

		private MethodBody BuildMethodBody( Method method )
		{
			Debug.Assert( Debugger.Host.CpuHook != null );
			uint[] codes = Debugger.Host.CpuHook.GetMethodBody( method );

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
			CoreState state = this.Debugger.Host.CpuHook.GetCoreState( 0 );
			this.SetAddress( state.ProgramCounter );
		}

		#region Registers

		private void ShowRegisters( RegisterSet set )
		{
			CoreState state = this.Debugger.Host.CpuHook.GetCoreState( 0 );
			this.pcTextBox.Text = string.Format( "0x{0:X8}", state.ProgramCounter );

			if( this.CurrentRegisterSet != set )
			{
				this.CurrentRegisterSet = set;
				switch( set )
				{
					case RegisterSet.Gpr:
						this.registerToggleToolStripSplitButton.Text = "GPR";
						break;
					case RegisterSet.Fpu:
						this.registerToggleToolStripSplitButton.Text = "FPU";
						break;
					case RegisterSet.Vfpu:
						this.registerToggleToolStripSplitButton.Text = "VFPU";
						break;
				}
			}

			RegisterBank bank;
			switch( set )
			{
				default:
				case RegisterSet.Gpr:
					bank = RegisterBanks.General;
					break;
				case RegisterSet.Fpu:
					bank = RegisterBanks.Fpu;
					break;
				case RegisterSet.Vfpu:
					bank = RegisterBanks.Vfpu;
					break;
			}

			this.registersListView.BeginUpdate();
			this.registersListView.Items.Clear();
			foreach( Register register in bank.Registers )
			{
				string prettyValue = string.Empty;
				string rawValue = string.Empty;
				switch( set )
				{
					case RegisterSet.Gpr:
						uint uv = this.Debugger.Host.CpuHook.GetRegister<uint>( set, register.Ordinal );
						prettyValue = uv.ToString();
						rawValue = string.Format( "{0:X8}", uv );
						break;
					case RegisterSet.Fpu:
					case RegisterSet.Vfpu:
						float fv = this.Debugger.Host.CpuHook.GetRegister<float>( set, register.Ordinal );
						prettyValue = fv.ToString();
						break;
				}
				ListViewItem item = new ListViewItem( new string[]{
					register.ToString(), prettyValue, rawValue,
				} );
				this.registersListView.Items.Add( item );
			}
			this.registersListView.EndUpdate();
		}

		private void disassemblyControl1_RegisterValueChanged( object sender, EventArgs e )
		{
			this.ShowRegisters( this.CurrentRegisterSet );
		}

		private void registerToggleToolStripSplitButton_ButtonClick( object sender, EventArgs e )
		{
			switch( this.CurrentRegisterSet )
			{
				case RegisterSet.Gpr:
					this.ShowRegisters( RegisterSet.Fpu );
					break;
				case RegisterSet.Fpu:
					this.ShowRegisters( RegisterSet.Vfpu );
					break;
				case RegisterSet.Vfpu:
					this.ShowRegisters( RegisterSet.Gpr );
					break;
			}
		}

		private void generalRegistersToolStripMenuItem_Click( object sender, EventArgs e )
		{
			this.ShowRegisters( RegisterSet.Gpr );
		}

		private void fPURegistersToolStripMenuItem_Click( object sender, EventArgs e )
		{
			this.ShowRegisters( RegisterSet.Fpu );
		}

		private void vFPURegistersToolStripMenuItem_Click( object sender, EventArgs e )
		{
			this.ShowRegisters( RegisterSet.Vfpu );
		}

		#endregion

		private void TestData()
		{
			uint[] codes = new uint[]{
				0x49170001,
				0xde00083f,
				0x10400004,
				0xcba80002,
				0xd0070068,
				0x64484828,
				0xdc08f096,
				0xd0008484,
				0xdc07f918,
				0xd0008585,
				0x64482808,
				0xdc00f004,
				0xd0008686,
				0xdc00f001,
				0xd0008787,
				0xf188a489,
				0xf189a08a,
				0xe88a0000,
				0xe88a0005,
				0xe88a000a,
				0x03e00008,
				0x27bd0010,
				0x00801021,
				0x44086000,
				0x48e80001,
				0xc8a00000,
				0xc8a00005,
				0xc8a0000a,
				0x65018000,
			};

			uint address = 0x08010000;
			List<Instruction> instrs = new List<Instruction>();
			for( int m = 0; m < 10; m++ )
			{
				for( int n = 0; n < codes.Length; n++ )
				{
					Instruction instr = new Instruction( address, codes[ n ] );
					instrs.Add( instr );

					address += 4;
				}
			}
			MethodBody b = new MethodBody( address, 4 * ( uint )instrs.Count, instrs.ToArray() );

			this.disassemblyControl1.SetMethod( b );
		}
	}
}

