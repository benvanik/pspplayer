// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading;

using Noxa.Emulation.Psp.Debugging;
using Noxa.Emulation.Psp.Debugging.Statistics;

namespace Noxa.Emulation.Psp.Video
{
	/// <summary>
	/// A video driver.
	/// </summary>
	public interface IVideoDriver : IComponentInstance, IDebuggable
	{
		/// <summary>
		/// The capability definition instance.
		/// </summary>
		IVideoCapabilities Capabilities
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
		/// The number of V blanks that have elapsed.
		/// </summary>
		ulong Vcount
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
		/// Lock the emulator to V-sync.
		/// </summary>
		bool SpeedLocked
		{
			get;
			set;
		}

		/// <summary>
		/// Callback information.
		/// </summary>
		VideoCallbacks Callbacks
		{
			get;
			set;
		}

		/// <summary>
		/// Resize the video context.
		/// </summary>
		/// <param name="width">The new width of the client area.</param>
		/// <param name="height">The new height of the client area.</param>
		void Resize( int width, int height );

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
		/// Capture the current contents of the screen.
		/// </summary>
		/// <returns>The contents of the screen.</returns>
		Bitmap CaptureScreen();

		/// <summary>
		/// Get the current set of display properties.
		/// </summary>
		/// <returns>A <see cref="DisplayProperties"/> instance with the current display properties.</returns>
		DisplayProperties QueryDisplayProperties();

		/// <summary>
		/// Set the display mode to the given settings.
		/// </summary>
		/// <param name="mode">The display mode.</param>
		/// <param name="width">The width, in pixels, of the display.</param>
		/// <param name="height">The height, in pixels, of the display.</param>
		/// <returns><c>true</c> if the mode change was a success.</returns>
		bool SetMode( int mode, int width, int height );

		/// <summary>
		/// Set the frame buffer to the given settings.
		/// </summary>
		/// <param name="bufferAddress">Base address of the frame buffer.</param>
		/// <param name="bufferSize">Size of the frame buffer, in bytes.</param>
		/// <param name="pixelFormat">The <c>PixelFormat</c> of the frame buffer.</param>
		/// <param name="bufferSyncMode">The <c>BufferSyncMode</c> of the frame buffer.</param>
		/// <returns><c>true</c> if the buffer change was a success.</returns>
		bool SetFrameBuffer( uint bufferAddress, uint bufferSize, PixelFormat pixelFormat, BufferSyncMode bufferSyncMode );

		/// <summary>
		/// Enqueue a new display list.
		/// </summary>
		/// <param name="listAddress">The address of the list in guest space.</param>
		/// <param name="stallAddress">The stall address, or 0 if none.</param>
		/// <param name="callbackId">The callback set to use, or -1 for none.</param>
		/// <param name="contextAddress">If not zero, save/restore the context to/from this address.</param>
		/// <param name="insertAtHead"><c>true</c> to insert the list at the head of the queue.</param>
		/// <returns>A list ID (>0) if successful.</returns>
		int EnqueueList( uint listAddress, uint stallAddress, int callbackId, uint contextAddress, bool insertAtHead );

		/// <summary>
		/// Update a stalled display list.
		/// </summary>
		/// <param name="listId">The ID of the list to update.</param>
		/// <param name="stallAddress">The new stall address, or 0 if none.</param>
		/// <returns><c>0</c> if the call succeeded.</returns>
		int UpdateList( int listId, uint stallAddress );

		/// <summary>
		/// Synchronize with the list processor for the given list.
		/// </summary>
		/// <param name="listId">The ID of the list to synchronize on.</param>
		/// <param name="syncType">The type of synchronization to perform.</param>
		/// <returns><c>0</c> if the call succeeded.</returns>
		int SyncList( int listId, int syncType );

		/// <summary>
		/// Synchronize with the frame.
		/// </summary>
		/// <param name="syncType">The type of synchronization to perform.</param>
		/// <returns><c>0</c> if the call succeeded.</returns>
		int SyncDraw( int syncType );

		/// <summary>
		/// Break processing of display lists.
		/// </summary>
		void Break();

		/// <summary>
		/// Continue processing of display lists.
		/// </summary>
		void Continue();
	}
}
