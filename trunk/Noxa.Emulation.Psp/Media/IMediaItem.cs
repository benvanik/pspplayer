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
	/// Describes the type of an <see cref="IMediaItem"/>.
	/// </summary>
	public enum MediaItemType
	{
		/// <summary>
		/// Item is a file.
		/// </summary>
		File,

		/// <summary>
		/// Item is a folder.
		/// </summary>
		Folder,
	}

	/// <summary>
	/// Describes attributes on an <see cref="IMediaItem"/>.
	/// </summary>
	[Flags]
	public enum MediaItemAttributes
	{
		/// <summary>
		/// Item is normal (visible, writable).
		/// </summary>
		Normal = 0,

		/// <summary>
		/// Item is read only.
		/// </summary>
		ReadOnly = 1,

		/// <summary>
		/// Item is hidden.
		/// </summary>
		Hidden = 2,
	}

	/// <summary>
	/// A media item.
	/// </summary>
	public interface IMediaItem
	{
		/// <summary>
		/// Name of the item.
		/// </summary>
		string Name
		{
			get;
			set;
		}

		/// <summary>
		/// The device that the item is on.
		/// </summary>
		IMediaDevice Device
		{
			get;
		}

		/// <summary>
		/// The parent container of the item.
		/// </summary>
		IMediaFolder Parent
		{
			get;
		}

		/// <summary>
		/// The absolute path of the item.
		/// </summary>
		string AbsolutePath
		{
			get;
		}

		/// <summary>
		/// The attributes of the item.
		/// </summary>
		MediaItemAttributes Attributes
		{
			get;
			set;
		}

		/// <summary>
		/// The time the item was created.
		/// </summary>
		DateTime CreationTime
		{
			get;
			set;
		}

		/// <summary>
		/// The time the item was last modified.
		/// </summary>
		DateTime ModificationTime
		{
			get;
			set;
		}

		/// <summary>
		/// The time the item was last accessed.
		/// </summary>
		DateTime AccessTime
		{
			get;
			set;
		}

		/// <summary>
		/// <c>true</c> if the item is a symbolic link.
		/// </summary>
		bool IsSymbolicLink
		{
			get;
		}

		/// <summary>
		/// Move the item to the destination folder.
		/// </summary>
		/// <param name="destination">The folder the item will be placed in.</param>
		/// <returns><c>true</c> if the move succeeded; otherwise <c>false</c>.</returns>
		bool MoveTo( IMediaFolder destination );

		/// <summary>
		/// Copy the item to the destination folder.
		/// </summary>
		/// <param name="destination">The folder the item will be placed in.</param>
		/// <returns><c>true</c> if the copy succeeded; otherwise <c>false</c>.</returns>
		bool CopyTo( IMediaFolder destination );

		/// <summary>
		/// Delete the item.
		/// </summary>
		void Delete();
	}
}
