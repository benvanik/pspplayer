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
	public interface IVideoDriver : IComponentInstance
	{
		DisplayProperties Properties
		{
			get;
		}

		IntPtr ControlHandle
		{
			get;
			set;
		}

		AutoResetEvent Vblank
		{
			get;
		}

		uint Vcount
		{
			get;
		}

		DisplayList FindDisplayList( int displayListId );
		bool Enqueue( DisplayList displayList, bool immediate );
		void Abort( int displayListId );
		void Sync( DisplayList displayList );

		void Sync();

		void Suspend();
		bool Resume();
	}
}
