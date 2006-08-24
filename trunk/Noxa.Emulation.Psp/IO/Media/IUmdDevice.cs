// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace Noxa.Emulation.Psp.IO.Media
{
	public enum DiscType
	{
		Unknown = 0x00,
		Audio = 0x04,
		Game = 0x10,
		Video = 0x20,
	}

	public interface IUmdDevice : IMediaDevice
	{
		DiscType DiscType
		{
			get;
		}

		bool Load( string path );
	}
}
