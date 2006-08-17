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
	}
}
