// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

using Noxa.Emulation.Psp.Debugging.Hooks;

namespace Noxa.Emulation.Psp.Debugging
{
	/// <summary>
	/// Defines an instance as debuggable.
	/// </summary>
	public interface IDebuggable
	{
		/// <summary>
		/// The debug hook that allows for inspection.
		/// </summary>
		IHook DebugHook
		{
			get;
		}

		/// <summary>
		/// <c>true</c> if the instance supports debugging.
		/// </summary>
		bool SupportsDebugging
		{
			get;
		}

		/// <summary>
		/// <c>true</c> if a debugging functionality is enabled.
		/// </summary>
		bool DebuggingEnabled
		{
			get;
		}

		/// <summary>
		/// Enable debugging support. Must be called before starting.
		/// </summary>
		void EnableDebugging();
	}
}
