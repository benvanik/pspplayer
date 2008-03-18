using System;
using System.Collections.Generic;
using System.Text;

namespace Noxa.Emulation.Psp.Audio
{
	public class FMODAudio : IComponent
	{
		public ComponentType Type { get { return ComponentType.Audio; } }
		public string Name { get { return "FMOD Audio"; } }
		public Version Version { get { return new Version(1, 0); } }
		public string Author { get { return "Rick (rick@gibbed.us)"; } }
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

		public IList<ComponentIssue> Test(ComponentParameters parameters)
		{
			// TODO: test to make sure required OpenGL extensions are present
			return new List<ComponentIssue>();
		}

		public bool IsConfigurable{ get { return false; } }

		public IComponentConfiguration CreateConfiguration(ComponentParameters parameters)
		{
			return null;
		}

		public IComponentInstance CreateInstance(IEmulationInstance emulator, ComponentParameters parameters)
		{
			return new FMODDriver(emulator, parameters);
		}
	}
}
