// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace Noxa.Emulation.Psp
{
	#pragma warning disable 1591

	public class NullComponent : IComponent
	{
		public ComponentType Type
		{
			get
			{
				return ComponentType.Other;
			}
		}

		public string Name
		{
			get
			{
				return "Null";
			}
		}

		public Version Version
		{
			get
			{
				return null;
			}
		}

		public string Author
		{
			get
			{
				return null;
			}
		}

		public string Website
		{
			get
			{
				return null;
			}
		}

		public string RssFeed
		{
			get
			{
				return null;
			}
		}

		public ComponentBuild Build
		{
			get
			{
				return ComponentBuild.Release;
			}
		}

		public override string ToString()
		{
			return "Disabled";
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
			return null;
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
			return null;
		}
	}

	#pragma warning restore
}
