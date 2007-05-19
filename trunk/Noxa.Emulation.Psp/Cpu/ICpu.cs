// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Noxa.Emulation.Psp.Bios;
using Noxa.Emulation.Psp.Debugging;
using Noxa.Emulation.Psp.Debugging.Protocol;
using Noxa.Emulation.Psp.Games;

namespace Noxa.Emulation.Psp.Cpu
{
	/// <summary>
	/// Delegate used to indicate that a thread has jumped back in to nothing.
	/// </summary>
	/// <param name="tcsId">The thread context storage ID that made the jump.</param>
	/// <param name="state">User-defined state argument.</param>
	public delegate void ContextSafetyDelegate( int tcsId, int state );

	/// <summary>
	/// Delegate used to indicate that a callback marshalling operation has completed.
	/// </summary>
	/// <param name="tcsId">The thread context storage ID that the callback was performed on.</param>
	/// <param name="state">User-passed state argument.</param>
	/// <param name="result">The value of $v0 (probably the result).</param>
	/// <returns><c>true</c> if execution should continue, <c>false</c> if it should break.</returns>
	public delegate bool MarshalCompleteDelegate( int tcsId, int state, int result );

	/// <summary>
	/// Delegate used to indicate that CPU execution is about to resume.
	/// </summary>
	/// <param name="timedOut"><c>true</c> if the resume was started by a timeout.</param>
	/// <param name="state">User-defined state argument.</param>
	public delegate void CpuResumeCallback( bool timedOut, object state );

	/// <summary>
	/// A CPU.
	/// </summary>
	public interface ICpu : IComponentInstance, IDebuggable
	{
		/// <summary>
		/// The capability definition instance.
		/// </summary>
		ICpuCapabilities Capabilities
		{
			get;
		}

		/// <summary>
		/// A list of cores in the CPU.
		/// </summary>
		ICpuCore[] Cores
		{
			get;
		}

		/// <summary>
		/// Get a <see cref="ICpuCore"/> by ordinal.
		/// </summary>
		/// <param name="core">The ordinal of the core to retrieve.</param>
		/// <returns>The <see cref="ICpuCore"/> with the given ordinal.</returns>
		ICpuCore this[ int core ]
		{
			get;
		}

		/// <summary>
		/// The memory system.
		/// </summary>
		IMemory Memory
		{
			get;
		}

		/// <summary>
		/// A pointer to the native CPU interface.
		/// </summary>
		IntPtr NativeInterface
		{
			get;
		}

		/// <summary>
		/// Debug controller implementation.
		/// </summary>
		IDebugController DebugController
		{
			get;
		}

		#region Syscalls / Exports

		/// <summary>
		/// Register a syscall.
		/// </summary>
		/// <param name="nid">The NID of the syscall to register.</param>
		/// <returns>The syscall ID used to call the given <paramref name="nid"/>.</returns>
		uint RegisterSyscall( uint nid );

		/// <summary>
		/// Register user module exports.
		/// </summary>
		/// <param name="module">The user module containing the exports to register.</param>
		void RegisterUserExports( BiosModule module );

		/// <summary>
		/// Lookup the address of a user export by NID.
		/// </summary>
		/// <param name="nid">The NID to look up.</param>
		/// <returns>The address of the export with the given NID or <c>0</c> if it was not found.</returns>
		uint LookupUserExport( uint nid );

		#endregion

		#region Interrupts

		/// <summary>
		/// The current interrupts masking flag.
		/// </summary>
		uint InterruptsMask
		{
			get;
			set;
		}

		/// <summary>
		/// Register an interrupt handler.
		/// </summary>
		/// <param name="interruptNumber">Interrupt number (0-67).</param>
		/// <param name="slot">Slot on the interrupt line (0-15).</param>
		/// <param name="address">Address of the user callback code.</param>
		/// <param name="argument">Argument to pass to the user callback code.</param>
		void RegisterInterruptHandler( int interruptNumber, int slot, uint address, uint argument );

		/// <summary>
		/// Unregister an interrupt handler.
		/// </summary>
		/// <param name="interruptNumber">Interrupt number (0-67).</param>
		/// <param name="slot">Slot on the interrupt line (0-15).</param>
		void UnregisterInterruptHandler( int interruptNumber, int slot );

		/// <summary>
		/// Set an interrupt as pending.
		/// </summary>
		/// <param name="interruptNumber">Interrupt number (0-67).</param>
		void SetPendingInterrupt( int interruptNumber );

		#endregion

		#region Threading

		/// <summary>
		/// Allocate thread context storage.
		/// </summary>
		/// <param name="pc">The PC to use.</param>
		/// <param name="registers">The 32 general registers to set initially.</param>
		/// <returns>The ID of the newly allocated thread context storage (<c>tcsId</c>).</returns>
		int AllocateContextStorage( uint pc, uint[] registers );

		/// <summary>
		/// Release a thread context storage.
		/// </summary>
		/// <param name="tcsId">The ID of the thread context storage to release.</param>
		void ReleaseContextStorage( int tcsId );

		/// <summary>
		/// Set a safety callback for a thread context. If the thread returns to nothing, the callback will be called.
		/// </summary>
		/// <param name="tcsId">The ID of the thread context storage to add the callback to.</param>
		/// <param name="callback">The method that will be called.</param>
		/// <param name="state">Caller-defined state to be passed to the handler.</param>
		void SetContextSafetyCallback( int tcsId, ContextSafetyDelegate callback, int state );

		/// <summary>
		/// Get a register value from the given thread context storage block.
		/// </summary>
		/// <param name="tcsId">The ID of the thread context storage to access.</param>
		/// <param name="reg">The ordinal of the register (0-31).</param>
		/// <returns>The value of the register in the given thread context storage block.</returns>
		uint GetContextRegister( int tcsId, int reg );

		/// <summary>
		/// Set a register value in the given thread context storage block.
		/// </summary>
		/// <param name="tcsId">The ID of the thread context storage to access.</param>
		/// <param name="reg">The ordinal of the register (0-31).</param>
		/// <param name="value">The new value of the register.</param>
		void SetContextRegister( int tcsId, int reg, uint value );

		/// <summary>
		/// Switch thread contexts.
		/// </summary>
		/// <param name="newTcsId">The thread context storage ID to switch to.</param>
		void SwitchContext( int newTcsId );

		/// <summary>
		/// Suspend the current thread and execute the given user code, resuming at the prior address.
		/// </summary>
		/// <param name="tcsId">The thread context storage ID to run the callback on.</param>
		/// <param name="address">The user address of the code to execute.</param>
		/// <param name="arguments">A list of arguments to pass to the user code (placed in $a0+).</param>
		/// <param name="resultCallback">Caller-defined handler to execute once the callback has completed.</param>
		/// <param name="state">Caller-defined state to be passed to the handler.</param>
		/// <remarks>
		/// <paramref name="resultCallback"/> is called before execution resumes.
		/// </remarks>
		void MarshalCall( int tcsId, uint address, uint[] arguments, MarshalCompleteDelegate resultCallback, int state );

		#endregion

		/// <summary>
		/// Setup the CPU with the given parameters.
		/// </summary>
		/// <param name="game">The current <see cref="GameInformation"/>.</param>
		/// <param name="bootStream">The boot stream of the current game.</param>
		void SetupGame( GameInformation game, Stream bootStream );

		/// <summary>
		/// Execute a block of instructions.
		/// </summary>
		/// <param name="breakFlag"><c>true</c> if a break was requested.</param>
		/// <param name="instructionsExecuted">The number of instructions executed.</param>
		void Execute( out bool breakFlag, out uint instructionsExecuted );

		/// <summary>
		/// Break execution.
		/// </summary>
		void BreakExecution();

		#region Execution Control

		/// <summary>
		/// Resume CPU execution after a break and wait.
		/// </summary>
		void Resume();

		/// <summary>
		/// Break CPU execution and wait.
		/// </summary>
		/// <returns></returns>
		void BreakAndWait();

		/// <summary>
		/// Break CPU execution and wait, cancelling the wait if the specified timeout period elapses.
		/// </summary>
		/// <param name="timeoutMs">The time, in milliseconds, to wait before resuming.</param>
		void BreakAndWait( int timeoutMs );

		/// <summary>
		/// Break CPU execution and wait, cancelling the wait if the specified timeout period elapses.
		/// Before the CPU resumes, the given callback is called.
		/// </summary>
		/// <param name="timeoutMs">The time, in milliseconds, to wait before resuming.</param>
		/// <param name="callback">The callback to issue before resuming.</param>
		/// <param name="state">User-defined state to pass to the callback.</param>
		void BreakAndWait( int timeoutMs, CpuResumeCallback callback, object state );

		#endregion

		/// <summary>
		/// Stop the CPU.
		/// </summary>
		void Stop();
	}
}
