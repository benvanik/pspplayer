using System;
using System.Collections.Generic;
using System.Text;

namespace Noxa.Emulation.Psp.IO.Media
{
	public enum DiscType
	{
		Unknown,
		Audio,
		Game,
		Video,
	}

	public interface IUmdDevice : IMediaDevice
	{
		DiscType DiscType
		{
			get;
		}
	}
}
