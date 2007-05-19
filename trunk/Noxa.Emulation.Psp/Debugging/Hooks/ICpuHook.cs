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
	/// Defines a type of memory access.
	/// </summary>
	public enum MemoryAccessType
	{
		/// <summary>
		/// Read from the given address.
		/// </summary>
		Read,
		/// <summary>
		/// Write to the given address.
		/// </summary>
		Write,
		/// <summary>
		/// Read from or write to the given address.
		/// </summary>
		ReadWrite,
	}

	/// <summary>
	/// Hook inside of the CPU that allows the debugger to extract information.
	/// </summary>
	public interface ICpuHook : IHook
	{
		/// <summary>
		/// Get the processor state for the given core.
		/// </summary>
		/// <param name="core">Core ordinal.</param>
		/// <returns>State information for the given core.</returns>
		CoreState GetCoreState( int core );

		/// <summary>
		/// Set the processor state for the given core.
		/// </summary>
		/// <param name="core">Core ordinal.</param>
		/// <param name="state">State information for the given core.</param>
		void SetCoreState( int core, CoreState state );

		/// <summary>
		/// Attempt to extract the callstack if frame tracking is enabled.
		/// </summary>
		/// <returns>The callstack or <c>null</c> if frame tracking is disabled.</returns>
		Frame[] GetCallstack();

		/// <summary>
		/// Add the given breakpoint.
		/// </summary>
		/// <param name="id">Breakpoint ID.</param>
		/// <param name="address">The address to set the breakpoint at.</param>
		void AddCodeBreakpoint( int id, uint address );

		/// <summary>
		/// Remove the given breakpoint.
		/// </summary>
		/// <param name="id">The ID of the breakpoint to remove.</param>
		void RemoveCodeBreakpoint( int id );

		/// <summary>
		/// Set a breakpoints enabled state.
		/// </summary>
		/// <param name="id">The ID of the breakpoint to modify.</param>
		/// <param name="enabled"><c>true</c> to activate the breakpoint.</param>
		void SetCodeBreakpointState( int id, bool enabled );

		#region Memory

		/// <summary>
		/// Add the given breakpoint.
		/// </summary>
		/// <param name="id">Breakpoint ID.</param>
		/// <param name="address">The address to set the breakpoint at.</param>
		/// <param name="accessType">The memory access type to break on.</param>
		void AddMemoryBreakpoint( int id, uint address, MemoryAccessType accessType );

		/// <summary>
		/// Remove the given breakpoint.
		/// </summary>
		/// <param name="id">The ID of the breakpoint to remove.</param>
		void RemoveMemoryBreakpoint( int id );

		/// <summary>
		/// Set a breakpoints enabled state.
		/// </summary>
		/// <param name="id">The ID of the breakpoint to modify.</param>
		/// <param name="enabled"><c>true</c> to activate the breakpoint.</param>
		void SetMemoryBreakpointState( int id, bool enabled );

		/// <summary>
		/// Get the block of memory in the given range.
		/// </summary>
		/// <param name="startAddress">The address to start reading at.</param>
		/// <param name="length">The total number of bytes to read.</param>
		/// <returns>The block of memory defined by the given range or <c>null</c> if an error occured.</returns>
		byte[] GetMemory( uint startAddress, int length );

		/// <summary>
		/// Set a block of memory.
		/// </summary>
		/// <param name="startAddress">The address to start writing at.</param>
		/// <param name="buffer">The buffer to write.</param>
		/// <param name="offset">Offset in the buffer, in bytes, to start reading from.</param>
		/// <param name="length">The total number of bytes to write.</param>
		void SetMemory( uint startAddress, byte[] buffer, int offset, int length );

		/// <summary>
		/// Search for the given value and return all instances.
		/// </summary>
		/// <param name="value">The value to search for.</param>
		/// <param name="width">The width of <paramref name="value"/> in bytes; valid values are <c>1</c>, <c>2</c>, <c>4</c>, and <c>8</c>.</param>
		/// <returns>A list of all locations where <paramref name="value"/> was found or <c>null</c>.</returns>
		uint[] SearchMemory( ulong value, int width );

		/// <summary>
		/// Compute a checksum of a given block of memory.
		/// </summary>
		/// <param name="startAddress">The address to start computing at.</param>
		/// <param name="length">The total number of bytes to include.</param>
		/// <returns>The checksum of the given memory range.</returns>
		uint Checksum( uint startAddress, int length );

		#endregion
	}
}
