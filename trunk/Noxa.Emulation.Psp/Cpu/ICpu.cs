// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Noxa.Emulation.Psp.Debugging;
using Noxa.Emulation.Psp.Games;

namespace Noxa.Emulation.Psp.Cpu
{
	/// <summary>
	/// A CPU.
	/// </summary>
	public interface ICpu : IComponentInstance
	{
		/// <summary>
		/// The capability definition instance.
		/// </summary>
		ICpuCapabilities Capabilities
		{
			get;
		}

		/// <summary>
		/// The statistics reporter.
		/// </summary>
		ICpuStatistics Statistics
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
		/// The audio/video decoder, if supported.
		/// </summary>
		IAvcDecoder Avc
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
		/// <c>true</c> if debugging is enabled.
		/// </summary>
		bool DebuggingEnabled
		{
			get;
		}

		/// <summary>
		/// The current debugger instance, if enabled.
		/// </summary>
		IDebugger Debugger
		{
			get;
		}

		/// <summary>
		/// The CPU debug inspection instance.
		/// </summary>
		ICpuHook DebugHook
		{
			get;
		}

		/// <summary>
		/// Enable debugging on the CPU.
		/// </summary>
		/// <param name="debugger">The debugger instance to attach to.</param>
		void EnableDebugging( IDebugger debugger );

		/// <summary>
		/// Register a syscall.
		/// </summary>
		/// <param name="nid">The NID of the syscall to register.</param>
		/// <returns>The syscall ID used to call the given <paramref name="nid"/>.</returns>
		int RegisterSyscall( uint nid );

		/// <summary>
		/// Resume execution.
		/// </summary>
		void Resume();

		/// <summary>
		/// Break execution.
		/// </summary>
		void Break();

		/// <summary>
		/// Setup the CPU with the given parameters.
		/// </summary>
		/// <param name="game">The current <see cref="GameInformation"/>.</param>
		/// <param name="bootStream">The boot stream of the current game.</param>
		void SetupGame( GameInformation game, Stream bootStream );

		/// <summary>
		/// Execute a block of instructions.
		/// </summary>
		/// <returns>The number of instructions executed.</returns>
		int ExecuteBlock();

		/// <summary>
		/// Stop the CPU.
		/// </summary>
		void Stop();

		/// <summary>
		/// Print the statistics to the console.
		/// </summary>
		void PrintStatistics();
	}
}
