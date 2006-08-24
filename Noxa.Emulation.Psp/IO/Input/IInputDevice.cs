// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace Noxa.Emulation.Psp.IO.Input
{
	[Flags]
	public enum PadButtons
	{
		None			= 0x000000,
		Select			= 0x000001,
		Start			= 0x000008,
		DigitalUp		= 0x000010,
		DigitalRight	= 0x000020,
		DigitalDown		= 0x000040,
		DigitalLeft		= 0x000080,
		LeftTrigger		= 0x000100,
		RightTrigger	= 0x000200,
		Triangle		= 0x001000,
		Circle			= 0x002000,
		Cross			= 0x004000,
		Square			= 0x008000,
		Home			= 0x010000,
		Hold			= 0x020000,
		MusicNote		= 0x800000,
	}

	public interface IInputDevice : IIODriver
	{
		event EventHandler Connected;
		event EventHandler Disconnected;

		IntPtr WindowHandle
		{
			get;
			set;
		}

		bool IsConnected
		{
			get;
		}

		int AnalogX
		{
			get;
		}

		int AnalogY
		{
			get;
		}

		PadButtons Buttons
		{
			get;
		}

		void Poll();
	}
}
