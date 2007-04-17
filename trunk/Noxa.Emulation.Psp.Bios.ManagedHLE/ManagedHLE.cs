// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

using Noxa.Emulation.Psp.Bios.ManagedHLE;

namespace Noxa.Emulation.Psp.Bios
{
	public class MHLE : IComponent
	{
		public ComponentType Type
		{
			get
			{
				return ComponentType.Bios;
			}
		}

		public string Name
		{
			get
			{
				return "Managed HLE";
			}
		}

		public Version Version
		{
			get
			{
				return new Version( 1, 0, 0, 0 );
			}
		}

		public string Author
		{
			get
			{
				return "Ben Vanik (ben.vanik@gmail.com)";
			}
		}

		public string Website
		{
			get
			{
				return "http://www.noxa.org";
			}
		}

		public string RssFeed
		{
			get
			{
				return "http://www.noxa.org/rss";
			}
		}

		public ComponentBuild Build
		{
			get
			{
#if DEBUG
				return ComponentBuild.Debug;
#else
				return ComponentBuild.Release;
#endif
			}
		}

		public override string ToString()
		{
			return this.Name;
		}

		public bool IsTestable
		{
			get
			{
				return false;
			}
		}

		public IList<ComponentIssue> Test( ComponentParameters parameters )
		{
			return new List<ComponentIssue>();
		}

		public bool IsConfigurable
		{
			get
			{
				return false;
			}
		}

		public IComponentConfiguration CreateConfiguration( ComponentParameters parameters )
		{
			return null;
		}

		public IComponentInstance CreateInstance( IEmulationInstance emulator, ComponentParameters parameters )
		{
			return new Noxa.Emulation.Psp.Bios.ManagedHLE.Bios( emulator, parameters );
		}
	}
}
