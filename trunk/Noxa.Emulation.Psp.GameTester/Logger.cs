// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Noxa.Emulation.Psp.GameTester
{
	class Logger : ILogger
	{
		public Logger()
		{
			Log.Instance = this;
		}

		#region ILogger Members

		public void WriteLine( Verbosity verbosity, Feature feature, string value )
		{
			Debug.WriteLine( string.Format( "{0}: {1}", feature, value ) );
		}

		#endregion
	}
}
