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
	/// Generic event instance.
	/// </summary>
	[Serializable]
	public abstract class Event
	{
		/// <summary>
		/// The time at which the event occured.
		/// </summary>
		public readonly DateTime Timestamp;

		/// <summary>
		/// An optional message associated with the event.
		/// </summary>
		public readonly string Message;

		/// <summary>
		/// Initializes a new <see cref="Event"/> instance with the given parameters.
		/// </summary>
		/// <param name="message">An optional message associated with the event.</param>
		public Event( string message )
		{
			this.Timestamp = DateTime.Now;
			if( message == string.Empty )
				this.Message = null;
			else
				this.Message = message;
		}
	}

	#region CpuEvent

	/// <summary>
	/// Defines CPU event types.
	/// </summary>
	public enum CpuEventType
	{
	}

	/// <summary>
	/// Represents a CPU event.
	/// </summary>
	[Serializable]
	public class CpuEvent : Event
	{
		/// <summary>
		/// The type of the CPU event.
		/// </summary>
		public readonly CpuEventType Type;

		/// <summary>
		/// Initializes a new <see cref="CpuEvent"/> instance with the given parameters.
		/// </summary>
		/// <param name="type">The type of the event.</param>
		public CpuEvent( CpuEventType type )
			: this( type, null )
		{
		}

		/// <summary>
		/// Initializes a new <see cref="CpuEvent"/> instance with the given parameters.
		/// </summary>
		/// <param name="type">The type of the event.</param>
		/// <param name="message">An optional message associated with the event.</param>
		public CpuEvent( CpuEventType type, string message )
			: base( message )
		{
			this.Type = type;
		}
	}

	#endregion

	#region BiosEvent

	/// <summary>
	/// Defines BIOS event types.
	/// </summary>
	public enum BiosEventType
	{
		/// <summary>
		/// The active thread changed.
		/// </summary>
		ContextSwitch,
	}

	/// <summary>
	/// Represents a BIOS event.
	/// </summary>
	[Serializable]
	public class BiosEvent : Event
	{
		/// <summary>
		/// The type of the BIOS event.
		/// </summary>
		public readonly BiosEventType Type;

		/// <summary>
		/// Initializes a new <see cref="BiosEvent"/> instance with the given parameters.
		/// </summary>
		/// <param name="type">The type of the event.</param>
		public BiosEvent( BiosEventType type )
			: this( type, null )
		{
		}

		/// <summary>
		/// Initializes a new <see cref="BiosEvent"/> instance with the given parameters.
		/// </summary>
		/// <param name="type">The type of the event.</param>
		/// <param name="message">An optional message associated with the event.</param>
		public BiosEvent( BiosEventType type, string message )
			: base( message )
		{
			this.Type = type;
		}
	}

	#endregion
}
