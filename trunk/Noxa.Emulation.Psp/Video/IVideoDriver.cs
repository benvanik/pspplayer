// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Threading;

namespace Noxa.Emulation.Psp.Video
{
	/// <summary>
	/// A video driver.
	/// </summary>
	public interface IVideoDriver : IComponentInstance
	{
		/// <summary>
		/// The capability definition instance.
		/// </summary>
		IVideoCapabilities Capabilities
		{
			get;
		}

		/// <summary>
		/// The statistics reporter.
		/// </summary>
		IVideoStatistics Statistics
		{
			get;
		}

		/// <summary>
		/// The current display properties.
		/// </summary>
		DisplayProperties Properties
		{
			get;
		}

		/// <summary>
		/// The control handle where the video will be displayed.
		/// </summary>
		IntPtr ControlHandle
		{
			get;
			set;
		}

		/// <summary>
		/// Set on each V blank.
		/// </summary>
		AutoResetEvent Vblank
		{
			get;
		}

		/// <summary>
		/// The number of V blanks that have elapsed.
		/// </summary>
		uint Vcount
		{
			get;
		}

		/// <summary>
		/// A pointer to the native video interface.
		/// </summary>
		IntPtr NativeInterface
		{
			get;
		}

		/// <summary>
		/// Find a display list with the given ID.
		/// </summary>
		/// <param name="displayListId">The ID of the display list to find.</param>
		/// <returns>The <see cref="DisplayList"/> with the given <paramref name="displyaListId"/> or <c>null</c> if not found.</returns>
		DisplayList FindDisplayList( int displayListId );

		/// <summary>
		/// Enqueue a display list.
		/// </summary>
		/// <param name="displayList">The display list to enqueue.</param>
		/// <param name="immediate"><c>true</c> if the display list should be inserted at the head of the queue.</param>
		/// <returns><c>true</c> if the list was inserted.</returns>
		bool Enqueue( DisplayList displayList, bool immediate );

		/// <summary>
		/// Abort a previously issued display list.
		/// </summary>
		/// <param name="displayListId">The ID of the display list to abort.</param>
		void Abort( int displayListId );

		/// <summary>
		/// Wait until a display list has been processed.
		/// </summary>
		/// <param name="displayList">The display list to wait on.</param>
		void Sync( DisplayList displayList );

		/// <summary>
		/// Wait until all outstanding display lists have been processed.
		/// </summary>
		void Sync();

		/// <summary>
		/// Suspend the video driver.
		/// </summary>
		void Suspend();

		/// <summary>
		/// Resume the video driver.
		/// </summary>
		/// <returns><c>true</c> if the resume was successful; otherwise <c>false</c>.</returns>
		bool Resume();

		/// <summary>
		/// Print the statistics to the console.
		/// </summary>
		void PrintStatistics();
	}
}
