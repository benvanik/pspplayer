// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace GenerateStubsV2
{
	interface IOutput
	{
		void BeginLibrary( PrxLibrary library );
		void EndLibrary();

		void WriteFunction( PrxFunction function );
	}
}
