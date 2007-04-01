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
	/// <summary>
	/// Describes the state of the media in an <see cref="IMediaDevice"/>.
	/// </summary>
	public enum MediaState
	{
		/// <summary>
		/// The device media is present and readable.
		/// </summary>
		Present,

		/// <summary>
		/// The device media is ejected.
		/// </summary>
		Ejected
	}

	/// <summary>
	/// Describes the type of a <see cref="IMediaDevice"/>.
	/// </summary>
	public enum MediaType
	{
		/// <summary>
		/// A Memory Stick.
		/// </summary>
		MemoryStick,

		/// <summary>
		/// A UMD.
		/// </summary>
		Umd
	}

	/// <summary>
	/// A generic media device.
	/// </summary>
	public interface IMediaDevice : IComponentInstance
	{
		/// <summary>
		/// The human-friendly description of the device.
		/// </summary>
		string Description
		{
			get;
		}

		/// <summary>
		/// The current state of the device.
		/// </summary>
		MediaState State
		{
			get;
		}

		/// <summary>
		/// The type of media the device handles.
		/// </summary>
		MediaType MediaType
		{
			get;
		}

		/// <summary>
		/// <c>true</c> if the media in the device is read-only.
		/// </summary>
		bool IsReadOnly
		{
			get;
		}

		/// <summary>
		/// The host path the device sources from.
		/// </summary>
		string HostPath
		{
			get;
		}

		/// <summary>
		/// The guest path of the device.
		/// </summary>
		string DevicePath
		{
			get;
		}

		/// <summary>
		/// The root folder on the device.
		/// </summary>
		IMediaFolder Root
		{
			get;
		}

		/// <summary>
		/// The capacity of the device, in bytes.
		/// </summary>
		long Capacity
		{
			get;
		}

		/// <summary>
		/// The amount of space available on the device, in bytes.
		/// </summary>
		long Available
		{
			get;
		}

		/// <summary>
		/// Refresh the device information.
		/// </summary>
		void Refresh();

		/// <summary>
		/// Eject the media.
		/// </summary>
		void Eject();
	}
}
