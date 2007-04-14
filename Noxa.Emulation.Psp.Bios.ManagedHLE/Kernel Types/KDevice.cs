// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using Noxa.Emulation.Psp.Media;

namespace Noxa.Emulation.Psp.Bios.ManagedHLE
{
	class KDevice : KHandle
	{
		public Kernel Kernel;

		public string Name;
		public string[] Aliases;

		public bool ReadOnly;

		public bool IsBlockDevice;
		public int BlockSize;

		public IMediaDevice Device;

		public KDevice( Kernel kernel, string name, string[] aliases, IMediaDevice device )
		{
			Kernel = kernel;
			Name = name;
			Aliases = aliases;
			Device = device;
			ReadOnly = device.IsReadOnly;
			IsBlockDevice = false;
		}

		public KDevice( Kernel kernel, string name, string[] aliases, bool readOnly, int blockSize )
		{
			Kernel = kernel;
			Name = name;
			Aliases = aliases;
			ReadOnly = readOnly;
			IsBlockDevice = true;
			BlockSize = blockSize;
		}
	}
}
