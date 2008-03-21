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
	partial class InprocDebugger
	{
		private FindDialog _findDialog;
		private JumpToAddressDialog _jumpToAddressDialog;

		private void SetupNavigation()
		{
			_findDialog = new FindDialog();
			_jumpToAddressDialog = new JumpToAddressDialog();
		}
	}
}
