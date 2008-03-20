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
	/// Defines the location of a register.
	/// </summary>
	public enum RegisterSet
	{
		/// <summary>
		/// The general purpose registers.
		/// </summary>
		Gpr,
		/// <summary>
		/// The floating-point coprocessor registers.
		/// </summary>
		Fpu,
		/// <summary>
		/// The vector floating-point coprocessor registers.
		/// </summary>
		Vfpu,
	}

	/// <summary>
	/// Hook inside of the CPU that allows the debugger to extract information.
	/// </summary>
	public interface ICpuHook : IHook
	{
		#region Breakpoints

		/// <summary>
		/// Add the given breakpoint.
		/// </summary>
		/// <param name="breakpoint">The breakpoint to add.</param>
		void AddBreakpoint( Breakpoint breakpoint );

		/// <summary>
		/// Find a breakpoint by ID.
		/// </summary>
		/// <param name="id">The ID of the breakpoint to retrieve.</param>
		/// <returns>The <see cref="Breakpoint"/> associated with the given <paramref name="id"/> or <c>null</c> if not found.</returns>
		Breakpoint FindBreakpoint( int id );

		/// <summary>
		/// Update the CPU copy of the breakpoint.
		/// </summary>
		/// <param name="newBreakpoint">The new value of the breakpoint.</param>
		/// <returns><c>true</c> if the operation succeeded.</returns>
		/// <remarks>
		/// Ensure that the ID matches the one previously added via <see cref="ICpuHook.AddBreakpoint"/>.
		/// </remarks>
		bool UpdateBreakpoint( Breakpoint newBreakpoint );

		/// <summary>
		/// Remove the given breakpoint.
		/// </summary>
		/// <param name="id">The ID of the breakpoint to remove.</param>
		void RemoveBreakpoint( int id );

		#endregion

		#region CPU

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

		#region Register Accessors

		/// <summary>
		/// Get the value of the given register in the given set.
		/// </summary>
		/// <typeparam name="T"><c>uint</c> for GPR and <c>float</c> for FPU/VFPU.</typeparam>
		/// <param name="set">The register set that the register is in.</param>
		/// <param name="ordinal">The ordinal of the register within its set.</param>
		/// <returns>The value of the register.</returns>
		T GetRegister<T>( RegisterSet set, int ordinal );

		/// <summary>
		/// Set the value of the given register in the given set.
		/// </summary>
		/// <typeparam name="T"><c>uint</c> for GPR and <c>float</c> for FPU/VFPU.</typeparam>
		/// <param name="set">The register set that the register is in.</param>
		/// <param name="ordinal">The ordinal of the register within its set.</param>
		/// <param name="value">The new value of the register.</param>
		void SetRegister<T>( RegisterSet set, int ordinal, T value );

		#endregion

		/// <summary>
		/// Attempt to extract the callstack if frame tracking is enabled.
		/// </summary>
		/// <returns>The callstack or <c>null</c> if frame tracking is disabled.</returns>
		Frame[] GetCallstack();

		#endregion

		#region Memory

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
		/// Get the pointer to the given memory address; only works when in the same process.
		/// </summary>
		/// <param name="address">The address (in user space) to get.</param>
		/// <returns>A pointer to the given <paramref name="address"/>.</returns>
		IntPtr GetMemoryPointer( uint address );

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

		/// <summary>
		/// Get the body of the given method.
		/// </summary>
		/// <param name="method">The method to retreive.</param>
		/// <returns>The words that make up the body of the method.</returns>
		uint[] GetMethodBody( Method method );

		#endregion
	}
}
