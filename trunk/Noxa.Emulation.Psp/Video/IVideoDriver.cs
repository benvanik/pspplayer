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
		/// Lock the emulator to V-sync.
		/// </summary>
		bool SpeedLocked
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
		/// Print the statistics to the console.
		/// </summary>
		void PrintStatistics();
	}
}
