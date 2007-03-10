// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace Noxa.Emulation.Psp.Media
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

	public interface IMediaDevice : IComponentInstance
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

		string DevicePath
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
