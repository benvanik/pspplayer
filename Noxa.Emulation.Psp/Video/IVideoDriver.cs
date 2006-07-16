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

		void Suspend();
		bool Resume();
	}
}
