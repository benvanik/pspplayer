// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Noxa.Emulation.Psp.Games;

namespace Noxa.Emulation.Psp.Bios
{
	public interface IKernel
	{
		GameInformation Game
		{
			get;
			set;
		}

		Stream BootStream
		{
			get;
		}

		void Execute();
	}
}
