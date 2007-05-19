// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

using Noxa.Emulation.Psp.Debugging.DebugModel;

namespace Noxa.Emulation.Psp.Debugging.Protocol
{
	/// <summary>
	/// Debugger control interface. Implemented by the CPU.
	/// </summary>
	public interface IDebugController
	{
		/// <summary>
		/// Resume CPU execution.
		/// </summary>
		void Run();

		/// <summary>
		/// Run until the given address is reached.
		/// </summary>
		/// <param name="address">Address to break at.</param>
		void RunUntil( uint address );

		/// <summary>
		/// Break CPU execution.
		/// </summary>
		void Break();

		/// <summary>
		/// Set the next statement.
		/// </summary>
		/// <param name="address">Address to execute next.</param>
		void SetNext( uint address );

		/// <summary>
		/// Step one instruction.
		/// </summary>
		void Step();

		/// <summary>
		/// Step over the given jump and break on the other side.
		/// </summary>
		void StepOver();

		/// <summary>
		/// Run until control returns to the caller.
		/// </summary>
		void StepOut();
	}
}
