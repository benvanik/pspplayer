using System;
using System.Collections.Generic;
using System.Text;

namespace Noxa.Emulation.Psp.Bios
{
	public interface IStatefulModule : IModule
	{
		void Start();
		void Stop();
		void Clear();
	}
}
