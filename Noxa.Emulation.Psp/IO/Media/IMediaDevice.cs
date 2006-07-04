using System;
using System.Collections.Generic;
using System.Text;

namespace Noxa.Emulation.Psp.IO.Media
{
	public enum MediaState
	{
		Present,
		Ejected
	}

	public enum MediaType
	{
		MemoryStick,
		Umd
	}

	public interface IMediaDevice : IIODriver
	{
		string Description
		{
			get;
		}

		MediaState State
		{
			get;
		}

		MediaType MediaType
		{
			get;
		}

		bool IsReadOnly
		{
			get;
		}

		string HostPath
		{
			get;
		}

		IMediaFolder Root
		{
			get;
		}

		long Capacity
		{
			get;
		}

		long Available
		{
			get;
		}

		void Refresh();
		void Eject();
	}
}
