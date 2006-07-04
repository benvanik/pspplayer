using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Noxa.Emulation.Psp.Video.Direct3DM;

namespace Noxa.Emulation.Psp.Video
{
	public class Direct3DMVideo : IComponent
	{
		public ComponentType Type
		{
			get
			{
				return ComponentType.Video;
			}
		}

		public string Name
		{
			get
			{
				return "Managed Direct3D Video";
			}
		}

		public Version Version
		{
			get
			{
				return new Version( 1, 0 );
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
			return new VideoDriver( emulator, parameters );
		}
	}
}
