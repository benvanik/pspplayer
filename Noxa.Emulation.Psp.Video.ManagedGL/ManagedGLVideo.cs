// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2008 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Noxa.Emulation.Psp.Video
{
	public class ManagedGLVideo : IComponent
	{
		public ComponentType Type { get { return ComponentType.Video; } }
		public string Name { get { return "Managed OpenGL Video"; } }
		public Version Version { get { return new Version( 1, 0 ); } }
		public string Author { get { return "Ben Vanik (ben.vanik@gmail.com)"; } }
		public string Website { get { return "http://www.noxa.org"; } }
		public string RssFeed { get { return "http://www.noxa.org/rss"; } }

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

		public bool IsTestable { get { return true; } }

		public IList<ComponentIssue> Test( ComponentParameters parameters )
		{
			// TODO: test to make sure required OpenGL extensions are present
			return new List<ComponentIssue>();
		}

		public bool IsConfigurable { get { return true; } }

		public IComponentConfiguration CreateConfiguration( ComponentParameters parameters )
		{
			return new ManagedGL.Configuration.VideoConfiguration( parameters );
		}

		public IComponentInstance CreateInstance( IEmulationInstance emulator, ComponentParameters parameters )
		{
			return new ManagedGL.MGLDriver( emulator, parameters );
		}
	}
}
