using System;
using System.Collections.Generic;
using System.Text;
using Noxa.Emulation.Psp.Games;

namespace Noxa.Emulation.Psp.Bios
{
	public interface IKernel
	{
		GameInformation Game
		{
			get;
			set;
		}

		void Execute();
	}
}
