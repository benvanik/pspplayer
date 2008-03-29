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
			_userProvider = new MappedByteProvider( ( byte* )this.Debugger.DebugHost.CpuHook.GetMemoryPointer( 0x08400000 ), 0x08400000, 0x01BFFFFF );

			sectionComboBox.Items.Add( "User: 0x08400000-0x09FFFFFF" );
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

		private List<uint> _navigationStack = new List<uint>();
		private int _navigationIndex;

		public void ClearNavigation()
		{
			uint head = _navigationStack[ _navigationStack.Count - 1 ];
			_navigationStack.Clear();
			_navigationStack.Add( head );
			_navigationIndex = 0;
		}

		private uint PeekNavigationStack()
		{
			if( _navigationStack.Count == 0 )
				return 0;
			return _navigationStack[ _navigationIndex ];
		}

		public void NavigateBack()
		{
			if( _navigationIndex <= 0 )
				return;
			_navigationIndex--;
			this.SetAddress( _navigationStack[ _navigationIndex ] );
		}

		public void NavigateForward()
		{
			if( _navigationIndex >= _navigationStack.Count - 1 )
				return;
			_navigationIndex++;
			uint next = this.PeekNavigationStack();
			this.SetAddress( next );
		}

		public void NavigateToAddress( uint address )
		{
			if( _navigationStack.Count == 0 )
			{
				_navigationStack.Add( address );
				_navigationIndex = 0;
			}
			else
			{
				uint current = this.PeekNavigationStack();
				if( current == address )
				{
					this.SetAddress( address );
					return;
				}
				_navigationStack.RemoveRange( _navigationIndex + 1, _navigationStack.Count - _navigationIndex - 1 );
				_navigationStack.Add( address );
				_navigationIndex++;
			}
			this.SetAddress( address );
		}

		private void SetAddress( uint address )
		{
			long translated = address - hexBox.ByteProvider.Offset;
			hexBox.ScrollByteIntoView( translated );
			hexBox.Select( translated, 4 );
		}
	}
}
