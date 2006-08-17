using System;
using System.Collections.Generic;
using System.Text;

namespace Noxa.Emulation.Psp
{
	public interface IComponentInstance
	{
		ComponentParameters Parameters
		{
			get;
		}

		IEmulationInstance Emulator
		{
			get;
		}

		Type Factory
		{
			get;
		}

		void Cleanup();
	}
}
