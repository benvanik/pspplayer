// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace Noxa.Emulation.Psp.Input
{
	/// <summary>
	/// Describes the state of the pad buttons.
	/// </summary>
	[Flags]
	public enum PadButtons
	{
		/// <summary>
		/// None pressed.
		/// </summary>
		None			= 0x000000,
		/// <summary>
		/// Select button.
		/// </summary>
		Select			= 0x000001,
		/// <summary>
		/// Start button.
		/// </summary>
		Start			= 0x000008,
		/// <summary>
		/// Up button.
		/// </summary>
		DigitalUp		= 0x000010,
		/// <summary>
		/// Right button.
		/// </summary>
		DigitalRight	= 0x000020,
		/// <summary>
		/// Down button.
		/// </summary>
		DigitalDown		= 0x000040,
		/// <summary>
		/// Left button.
		/// </summary>
		DigitalLeft		= 0x000080,
		/// <summary>
		/// Left trigger button.
		/// </summary>
		LeftTrigger		= 0x000100,
		/// <summary>
		/// Right trigger button.
		/// </summary>
		RightTrigger	= 0x000200,
		/// <summary>
		/// Triangle button.
		/// </summary>
		Triangle		= 0x001000,
		/// <summary>
		/// Circle button.
		/// </summary>
		Circle			= 0x002000,
		/// <summary>
		/// Cross button.
		/// </summary>
		Cross			= 0x004000,
		/// <summary>
		/// Square button.
		/// </summary>
		Square			= 0x008000,
		/// <summary>
		/// Home button.
		/// </summary>
		Home			= 0x010000,
		/// <summary>
		/// Power hold button.
		/// </summary>
		Hold			= 0x020000,
		/// <summary>
		/// Music note button.
		/// </summary>
		MusicNote		= 0x800000,
	}

	/// <summary>
	/// An input driver.
	/// </summary>
	public interface IInputDevice : IComponentInstance
	{
		/// <summary>
		/// The window handle of the emulator.
		/// </summary>
		IntPtr WindowHandle
		{
			get;
			set;
		}

		/// <summary>
		/// <c>true</c> if the input device is connected.
		/// </summary>
		bool IsConnected
		{
			get;
		}

		/// <summary>
		/// The absolute analog X position.
		/// </summary>
		int AnalogX
		{
			get;
		}

		/// <summary>
		/// The absolute analog Y position.
		/// </summary>
		int AnalogY
		{
			get;
		}

		/// <summary>
		/// The currently pressed buttons.
		/// </summary>
		PadButtons Buttons
		{
			get;
		}

		/// <summary>
		/// Poll the device to update the analog positions and button mask.
		/// </summary>
		void Poll();
	}
}
