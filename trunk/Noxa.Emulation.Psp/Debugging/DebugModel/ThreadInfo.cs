// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2008 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace Noxa.Emulation.Psp.Debugging.DebugModel
{
	/// <summary>
	/// Thread info.
	/// </summary>
	[Serializable]
	public class ThreadInfo
	{
		/// <summary>
		/// The BIOS thread ID.
		/// </summary>
		public uint ThreadID;
		/// <summary>
		/// The CPU thread ID, used for CPU inspection.
		/// </summary>
		public int InternalThreadID;

		/// <summary>
		/// The game-defined name for the thread.
		/// </summary>
		public string Name;
		/// <summary>
		/// The attributes the thread was created with.
		/// </summary>
		public ThreadAttributes Attributes;
		/// <summary>
		/// The entry address of the thread.
		/// </summary>
		public uint EntryAddress;
		
		// TODO: Module info

		/// <summary>
		/// The current PC of the thread.
		/// </summary>
		public uint CurrentPC;
		/// <summary>
		/// The priority of the thread.
		/// </summary>
		public int Priority;
		/// <summary>
		/// The current state of the thread.
		/// </summary>
		public ThreadState State;

		/// <summary>
		/// <c>true</c> if the thread is waiting.
		/// </summary>
		public bool IsWaiting;
		/// <summary>
		/// A human-readable description of what the thread is waiting on if <c>IsWaiting</c> is <c>true</c>.
		/// </summary>
		public string WaitingDescription;
	}

	/// <summary>
	/// The current state of a thread.
	/// </summary>
	public enum ThreadState
	{
		/// <summary>
		/// The thread is currently running.
		/// </summary>
		Running = 1,
		/// <summary>
		/// The thread is ready and waiting to run.
		/// </summary>
		Ready = 2,
		/// <summary>
		/// The thread is waiting on something.
		/// </summary>
		Waiting = 4,
		/// <summary>
		/// The thread is suspended.
		/// </summary>
		Suspended = 8,
		/// <summary>
		/// The thread is suspended while waiting on something.
		/// </summary>
		WaitSuspended = 12,
		/// <summary>
		/// The thread is stopped.
		/// </summary>
		Stopped = 16,
		/// <summary>
		/// The thread is dead.
		/// </summary>
		Dead = 32,
	}

	/// <summary>
	/// Attributes describing the behavior/usage of a thread.
	/// </summary>
	[Flags]
	public enum ThreadAttributes : uint
	{
		/// <summary>
		/// Allow VFPU usage.
		/// </summary>
		VFPU = 0x00004000,
		/// <summary>
		/// Start thread in user mode.
		/// </summary>
		User = 0x80000000,
		/// <summary>
		/// Thread is part of USB/WLAN API.
		/// </summary>
		UsbWlan = 0xA0000000,
		/// <summary>
		/// Thread is part of VSH API.
		/// </summary>
		Vsh = 0xC0000000,
		/// <summary>
		/// Allow scratchpad usage.
		/// </summary>
		ScratchSram = 0x00008000,
		/// <summary>
		/// Don't fill stack with 0xFF on create.
		/// </summary>
		NoFillStack = 0x00100000,
		/// <summary>
		/// Clear stack when thread deleted.
		/// </summary>
		ClearStack = 0x00200000,
	}
}
