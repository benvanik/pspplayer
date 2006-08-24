// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Noxa.Emulation.Psp
{
	public static class PspDebug
	{
		#region Logging

		[Conditional( "DEBUG" )]
		public static void WriteLine( string message )
		{
			Debug.WriteLine( "PspDebug: " + message );
		}

		[Conditional( "DEBUG" )]
		public static void WriteLine( string format, params object[] args )
		{
			Debug.WriteLine( "PspDebug: " + string.Format( format, args ) );
		}

		#endregion
	}
}
