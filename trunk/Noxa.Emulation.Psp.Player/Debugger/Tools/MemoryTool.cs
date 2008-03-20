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
using Be.Windows.Forms;
using Noxa.Emulation.Psp.Cpu;

namespace Noxa.Emulation.Psp.Player.Debugger.Tools
{
	unsafe partial class MemoryTool : Noxa.Emulation.Psp.Player.Debugger.DebuggerTool
	{
		private MappedByteProvider _vramProvider;
		private MappedByteProvider _userProvider;

		public MemoryTool()
		{
			this.InitializeComponent();
		}

		public MemoryTool( InprocDebugger debugger )
			: base( debugger )
		{
			this.InitializeComponent();

			Bitmap image = Properties.Resources.MemoryIcon as Bitmap;
			this.Icon = Icon.FromHandle( image.GetHicon() );
		}

		public override void OnStarted()
		{
			_vramProvider = new MappedByteProvider( ( byte* )this.Debugger.DebugHost.CpuHook.GetMemoryPointer( MemorySystem.VideoMemoryBase ), MemorySystem.VideoMemoryBase, MemorySystem.VideoMemorySize );
			_userProvider = new MappedByteProvider( ( byte* )this.Debugger.DebugHost.CpuHook.GetMemoryPointer( MemorySystem.MainMemoryBase ), MemorySystem.MainMemoryBase, MemorySystem.MainMemorySize );

			sectionComboBox.Items.Add( "User: 0x08000000-0x09FFFFFF" );
			sectionComboBox.Items.Add( "VRAM: 0x04000000-0x041FFFFF" );
			sectionComboBox.SelectedIndex = 0;
		}

		private void sectionComboBox_SelectedIndexChanged( object sender, EventArgs e )
		{
			switch( sectionComboBox.SelectedIndex )
			{
				case 0:
					hexBox.ByteProvider = _userProvider;
					break;
				case 1:
					hexBox.ByteProvider = _vramProvider;
					break;
			}
		}
	}
}
