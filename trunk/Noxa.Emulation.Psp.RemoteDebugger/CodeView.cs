// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

using Noxa.Emulation.Psp.RemoteDebugger.Model;

namespace Noxa.Emulation.Psp.RemoteDebugger
{
	partial class CodeView : DockContent
	{
		public EmuDebugger Debugger;

		public CodeView()
		{
			InitializeComponent();
		}

		public CodeView( EmuDebugger debugger )
			: this()
		{
			this.Debugger = debugger;

			this.LoadSettings();

			//this.disassemblyControl1.Enabled = false;

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
	}
}
