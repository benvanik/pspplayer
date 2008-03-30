// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

using Noxa.Emulation.Psp.Debugging.DebugModel;

namespace Noxa.Emulation.Psp.Debugging.Hooks
{
	/// <summary>
	/// Hook inside of the BIOS that allows the debugger to extract information.
	/// </summary>
	public interface IBiosHook : IHook
	{
		/// <summary>
		/// Gets the ID of the active thread.
		/// </summary>
		uint ActiveThreadID { get; }

		/// <summary>
		/// Get the states of all the living threads.
		/// </summary>
		/// <returns>The information of all the living threads.</returns>
		ThreadInfo[] GetThreads();

		/// <summary>
		/// Force wake the given thread.
		/// </summary>
		/// <param name="threadId">The ID of the thread to wake.</param>
		void WakeThread( uint threadId );

		/// <summary>
		/// Delay the given thread.
		/// </summary>
		/// <param name="threadId">The ID of the thread to delay.</param>
		/// <param name="delayMs">The time, in milliseconds, to delay the thread.</param>
		void DelayThread( uint threadId, uint delayMs );

		/// <summary>
		/// Suspend the given thread.
		/// </summary>
		/// <param name="threadId">The ID of the thread to suspend.</param>
		void SuspendThread( uint threadId );

		/// <summary>
		/// Resume the given thread.
		/// </summary>
		/// <param name="threadId">The ID of the thread to resume.</param>
		void ResumeThread( uint threadId );

		/// <summary>
		/// Kill the given thread.
		/// </summary>
		/// <param name="threadId">The ID of the thread to kill.</param>
		void KillThread( uint threadId );
	}
}
