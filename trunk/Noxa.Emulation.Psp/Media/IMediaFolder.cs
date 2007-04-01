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
	/// A <see cref="IMediaItem"/> container.
	/// </summary>
	public interface IMediaFolder : IMediaItem, IEnumerable<IMediaItem>
	{
		/// <summary>
		/// The items contained within the folder.
		/// </summary>
		IMediaItem[] Items
		{
			get;
		}

		/// <summary>
		/// Get a child item by name.
		/// </summary>
		/// <param name="name">The name of the item to find.</param>
		/// <returns>The <see cref="IMediaItem"/> with the given <paramref name="name"/> or <c>null</c> if not found.</returns>
		IMediaItem this[ string name ]
		{
			get;
		}

		/// <summary>
		/// Find the item at the given sub-path.
		/// </summary>
		/// <param name="path">The path to search.</param>
		/// <returns>The <see cref="IMediaItem"/> at the given sub-path or <c>null</c> if not found.</returns>
		IMediaItem Find( string path );

		/// <summary>
		/// Find the folder at the given sub-path.
		/// </summary>
		/// <param name="path">The path to search.</param>
		/// <returns>The <see cref="IMediaFolder"/> at the given sub-path or <c>null</c> if not found.</returns>
		IMediaFolder FindFolder( string path );

		/// <summary>
		/// Find the file at the given sub-path.
		/// </summary>
		/// <param name="path">The path to search.</param>
		/// <returns>The <see cref="IMediaFile"/> at the given sub-path or <c>null</c> if not found.</returns>
		IMediaFile FindFile( string path );

		/// <summary>
		/// Create a new symbolic link.
		/// </summary>
		/// <param name="name">The name of the symbolic link to create.</param>
		/// <param name="type">The type the link should represent.</param>
		/// <returns>A new <see cref="IMediaItem"/> instance of the new symbolic link or <c>null</c> if an error occurred.</returns>
		IMediaItem CreateSymbolicLink( string name, MediaItemType type );

		/// <summary>
		/// Create a new folder.
		/// </summary>
		/// <param name="name">The name of the folder to create.</param>
		/// <returns>A new <see cref="IMediaFolder"/> instance of the new folder or <c>null</c> if an error occurred.</returns>
		IMediaFolder CreateFolder( string name );

		/// <summary>
		/// Create a new file.
		/// </summary>
		/// <param name="name">The name of the file to create.</param>
		/// <returns>A new <see cref="IMediaFile"/> instance of the new file or <c>null</c> if an error occurred.</returns>
		IMediaFile CreateFile( string name );

		/// <summary>
		/// Move the item with the given name to the destination.
		/// </summary>
		/// <param name="name">The name of the item inside the current folder to move.</param>
		/// <param name="destination">The destination to put the item in.</param>
		/// <returns><c>true</c> if the move succeeded; otherwise <c>false</c>.</returns>
		bool MoveItem( string name, IMediaFolder destination );

		/// <summary>
		/// Copy the item with the given name to the destination.
		/// </summary>
		/// <param name="name">The name of the item inside the current folder to copy.</param>
		/// <param name="destination">The destination to put the item in.</param>
		/// <returns><c>true</c> if the copy succeeded; otherwise <c>false</c>.</returns>
		bool CopyItem( string name, IMediaFolder destination );

		/// <summary>
		/// Delete the item with the given name.
		/// </summary>
		/// <param name="name">The name of the item inside the current folder to delete.</param>
		void DeleteItem( string name );
	}
}
