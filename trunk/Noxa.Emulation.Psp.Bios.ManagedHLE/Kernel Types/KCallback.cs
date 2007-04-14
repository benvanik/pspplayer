// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace Noxa.Emulation.Psp.Bios.ManagedHLE
{
	class KCallback
	{
		public Kernel Kernel;

		public string Name;
		public KThread Thread;
		
		//typedef int (*SceKernelCallbackFunction)(int count, int arg, void *common);

		public uint Address;
		public uint CommonAddress;

		public uint NotifyCount;
		public uint NotifyArguments;

		public KCallback( Kernel kernel, string name, KThread thread, uint address, uint commonAddress )
		{
			Kernel = kernel;
			Name = name;
			Thread = thread;
			Address = address;
			CommonAddress = commonAddress;

			NotifyCount = 0;
			NotifyArguments = 0;
		}
	}
}
