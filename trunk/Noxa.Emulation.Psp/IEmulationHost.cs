using System;
using System.Collections.Generic;
using System.Text;

namespace Noxa.Emulation.Psp
{
	public interface IEmulationHost
	{
		IEmulationInstance CurrentInstance
		{
			get;
		}
	}
}
