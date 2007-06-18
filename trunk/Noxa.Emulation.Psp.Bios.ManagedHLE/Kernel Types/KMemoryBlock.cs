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
	// NOTE: most memory blocks will not have UIDs!
	
	class KMemoryBlock : KHandle
	{
		public readonly KPartition Partition;
		public string Name;

		public uint Address;
		public uint Size;
		public uint UpperBound
		{
			get
			{
				return Address + Size;
			}
		}

		public bool IsFree;

		public KMemoryBlock( KPartition partition, uint address, uint size, bool isFree )
		{
			Partition = partition;

			Address = address;
			Size = size;

			IsFree = isFree;
		}
	}
}
