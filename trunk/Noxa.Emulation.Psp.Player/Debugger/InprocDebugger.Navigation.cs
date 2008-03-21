// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2008 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Noxa.Emulation.Psp.Player.Debugger.Dialogs;

namespace Noxa.Emulation.Psp.Player.Debugger
{
	enum NavigationTarget
	{
		Code,
		Memory,
		Log,
	}

	partial class InprocDebugger
	{
		private FindDialog _findDialog;
		private JumpToAddressDialog _jumpToAddressDialog;

		private void SetupNavigation()
		{
			_findDialog = new FindDialog();
			_jumpToAddressDialog = new JumpToAddressDialog();
		}

		#region Find

		#endregion

		#region Jump

		public void ShowJumpToAddressDialog( NavigationTarget target )
		{
			_jumpToAddressDialog.Target = target;
			if( _jumpToAddressDialog.ShowDialog( this.Window ) == System.Windows.Forms.DialogResult.OK )
				this.JumpToAddress( target, _jumpToAddressDialog.Address, false );
		}

		public void JumpToAddress( NavigationTarget target, uint address, bool isCurrentStatement )
		{
			switch( target )
			{
				case NavigationTarget.Code:
					{
						this.CodeTool.Show( this.Window.DockPanel );
						this.CodeTool.BringToFront();
						this.CodeTool.Activate();
						this.CodeTool.SetAddress( address, isCurrentStatement );
						this.CallstackTool.RefreshCallstack();
					}
					break;
				case NavigationTarget.Memory:
					{
					}
					break;
				case NavigationTarget.Log:
					{
					}
					break;
			}
		}

		#endregion
	}
}
