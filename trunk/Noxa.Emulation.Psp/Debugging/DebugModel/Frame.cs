// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace Noxa.Emulation.Psp.Debugging.DebugModel
{
	/// <summary>
	/// Defines <see cref="Frame"/> types.
	/// </summary>
	public enum FrameType
	{
		/// <summary>
		/// User code address.
		/// </summary>
		UserCode,
		/// <summary>
		/// Special call marshal frame.
		/// </summary>
		CallMarshal,
		/// <summary>
		/// Special interrupt frame.
		/// </summary>
		Interrupt,
		/// <summary>
		/// General BIOS safety barrier.
		/// </summary>
		BiosBarrier,
	}

	/// <summary>
	/// A single frame in a callstack.
	/// </summary>
	[Serializable]
	public class Frame
	{
		/// <summary>
		/// The frame type.
		/// </summary>
		public readonly FrameType Type;

		/// <summary>
		/// The entry address.
		/// </summary>
		public readonly int Address;

		/// <summary>
		/// Initializes a new <see cref="Frame"/> instance with the given parameters.
		/// </summary>
		/// <param name="type">The type of the frame.</param>
		/// <param name="address">The entry address of the method.</param>
		public Frame( FrameType type, int address )
		{
			this.Type = type;
			this.Address = address;
		}
	}
}
