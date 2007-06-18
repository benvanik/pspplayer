// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace Noxa.Emulation.Psp.Video
{
	/// <summary>
	/// Defines the addresses of the video callbacks.
	/// </summary>
	public class VideoCallbacks
	{
		/// <summary>
		/// The address of the function to call, in guest space, when a SIGNAL command is issued.
		/// </summary>
		public uint SignalFunction;
		/// <summary>
		/// The argument to be passed to the signal callback.
		/// </summary>
		public uint SignalArgument;

		/// <summary>
		/// The address of the function to call, in guest space, when a list is FINISH'ed.
		/// </summary>
		public uint FinishFunction;
		/// <summary>
		/// The argument to be passed to the finish callback.
		/// </summary>
		public uint FinishArgument;
	}
}
