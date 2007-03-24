// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

using Noxa.Emulation.Psp.Cpu;
using Noxa.Emulation.Psp.Debugging;

namespace Noxa.Emulation.Psp.Bios
{
	public interface IBios : IComponentInstance
	{
		IKernel Kernel
		{
			get;
		}

		IModule[] Modules
		{
			get;
		}

		BiosFunction[] Functions
		{
			get;
		}

		IModule FindModule( string name );
		BiosFunction FindFunction( uint nid );

		void RegisterFunction( BiosFunction function );
		void UnregisterFunction( uint nid );

		bool DebuggingEnabled
		{
			get;
		}

		IDebugger Debugger
		{
			get;
		}

		IBiosHook DebugHook
		{
			get;
		}

		void EnableDebugging( IDebugger debugger );
	}
}
