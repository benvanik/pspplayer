// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Noxa.Emulation.Psp.Media
{
	/// <summary>
	/// Describes an <see cref="IUmdDevice"/> disc type.
	/// </summary>
	public enum DiscType
	{
		/// <summary>
		/// Unknown disc type.
		/// </summary>
		Unknown = 0x00,

		/// <summary>
		/// Audio disc.
		/// </summary>
		Audio = 0x04,

		/// <summary>
		/// Game disc.
		/// </summary>
		Game = 0x10,

		/// <summary>
		/// Video disc.
		/// </summary>
		Video = 0x20,
	}
	
	/// <summary>
	/// A UMD device.
	/// </summary>
	public interface IUmdDevice : IMediaDevice
	{
		/// <summary>
		/// The disc type of the UMD.
		/// </summary>
		DiscType DiscType
		{
			get;
		}

		/// <summary>
		/// Load from the given UMD image.
		/// </summary>
		/// <param name="path">The path of the UMD image to load.</param>
		/// <param name="minimalCache"><c>true</c> to only cache PSP_GAME and SYSDIR.</param>
		/// <returns><c>true</c> if the load was successful; otherwise <c>false</c>.</returns>
		bool Load( string path, bool minimalCache );

		/// <summary>
		/// Open a stream to access the UMD image in block mode.
		/// </summary>
		/// <returns>The UMD image stream.</returns>
		Stream OpenImageStream();

		/// <summary>
		/// Lookup a file by LBN.
		/// </summary>
		/// <param name="lbn">The LBN of the file on disc.</param>
		/// <param name="size">The size of the file on disc.</param>
		/// <returns>The <see cref="IMediaFile"/> that corresponds to the arguments, or <c>null</c> if none matched.</returns>
		IMediaFile Lookup( long lbn, long size );
	}
}
