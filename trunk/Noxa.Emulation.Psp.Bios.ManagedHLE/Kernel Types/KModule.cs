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
	class KModule : KHandle
	{
		public Kernel Kernel;

		public BiosModule MetaModule;
		public Module Module;
		public LoadParameters LoadParameters;
		public LoadResults LoadResults;

		public string Name;

		public uint ModuleInfo;
		public uint ModuleBootStart;
		public uint ModuleRebootBefore;
		public uint ModuleStart;
		public uint ModuleStartThreadParam;
		public uint ModuleStop;
		public uint ModuleStopThreadParam;

		public KModule( Kernel kernel, BiosModule metaModule )
		{
			Kernel = kernel;

			MetaModule = metaModule;

			if( metaModule != null )
			{
				Name = metaModule.Name;

				foreach( StubExport ex in metaModule.Exports )
				{
					switch( ex.NID )
					{
						case 0xF01D73A7:
							ModuleInfo = ex.Address;
							break;
						case 0xD3744BE0:
							ModuleBootStart = ex.Address;
							break;
						case 0x2F064FA6:
							ModuleRebootBefore = ex.Address;
							break;
						case 0xD632ACDB:
							ModuleStart = ex.Address;
							break;
						case 0x0F7C276C:
							ModuleStartThreadParam = ex.Address;
							break;
						case 0xCEE8593C:
							ModuleStop = ex.Address;
							break;
						case 0xCF0CC697:
							ModuleStopThreadParam = ex.Address;
							break;
					}
				}
			}
		}

		public KModule( Kernel kernel, BiosModule metaModule, Module module )
		{
			Kernel = kernel;

			MetaModule = metaModule;
			Module = module;
		}
	}
}
