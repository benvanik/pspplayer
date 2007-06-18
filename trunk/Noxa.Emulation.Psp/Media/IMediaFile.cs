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
	/// Describes file open modes for <see cref="IMediaFile"/> operations.
	/// </summary>
	public enum MediaFileMode
	{
		/// <summary>
		/// Normal reading mode.
		/// </summary>
		Normal,

		/// <summary>
		/// Appending write mode.
		/// </summary>
		Append,

		/// <summary>
		/// Truncating write mode.
		/// </summary>
		Truncate
	}

	/// <summary>
	/// Describes file access modes for <see cref="IMediaFile"/> operations.
	/// </summary>
	public enum MediaFileAccess
	{
		/// <summary>
		/// Read-only access.
		/// </summary>
		Read,

		/// <summary>
		/// Write-only access.
		/// </summary>
		Write,

		/// <summary>
		/// Read-write access.
		/// </summary>
		ReadWrite
	}

	/// <summary>
	/// A media file.
	/// </summary>
	public interface IMediaFile : IMediaItem
	{
		/// <summary>
		/// The LBN of the file on disc. Only applies to UMD-based devices.
		/// </summary>
		long LogicalBlockNumber
		{
			get;
		}

		/// <summary>
		/// The length of the file, in bytes.
		/// </summary>
		long Length
		{
			get;
		}

		/// <summary>
		/// Open the file for reading.
		/// </summary>
		/// <returns>A read-only <see cref="Stream"/> for the file or <c>null</c> if an error occurred.</returns>
		Stream OpenRead();

		/// <summary>
		/// Open the file.
		/// </summary>
		/// <param name="mode">The file open mode.</param>
		/// <param name="access">The file access mode.</param>
		/// <returns>A <see cref="Stream"/> with the desired mode for the file or <c>null</c> if an error occurred.</returns>
		Stream Open( MediaFileMode mode, MediaFileAccess access );
	}
}
