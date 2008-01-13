using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Noxa.Emulation.Psp.Bios.ManagedHLE
{
    class KPipe : KHandle
    {
        public Kernel Kernel;
        public string Name;
        public Stream Stream;
        public KPartition Partition;

        public KPipe(Kernel kernel, KPartition partition, string name)
		{
			Kernel = kernel;
            Partition = partition;
            Name = name;
            Stream = new System.IO.MemoryStream();
		}
    }
}
