// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

using Noxa.Emulation.Psp.Debugging;

namespace Noxa.Emulation.Psp
{
	/// <summary>
	/// Singleton debug helper.
	/// </summary>
	public static class Diag
	{
		/// <summary>
		/// The local debugger instance.
		/// </summary>
		public static DebugHost Instance;

		/// <summary>
		/// <c>true</c> if the debugger is attached.
		/// </summary>
		public static bool IsAttached
		{
			get
			{
				if( Instance == null )
					return false;
				return Instance.IsAttached;
			}
		}
	}
}
