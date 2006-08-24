// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Noxa.Emulation.Psp.IO.Media.FileSystem;
using Noxa.Emulation.Psp.IO.Media.FileSystem.Configuration;

namespace Noxa.Emulation.Psp.IO.Media
{
	public class GameHostFileSystem : IComponent
	{
		public ComponentType Type
		{
			get
			{
				return ComponentType.GameMedia;
			}
		}

		public string Name
		{
			get
			{
				return "Game Host File System Media";
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
				return true;
			}
		}

		public IComponentConfiguration CreateConfiguration( ComponentParameters parameters )
		{
			return new FileSystem.Configuration.MediaConfiguration( ComponentType.GameMedia, parameters );
		}

		public IComponentInstance CreateInstance( IEmulationInstance emulator, ComponentParameters parameters )
		{
			string path = parameters.GetValue<string>( MediaConfiguration.PathSetting, null );
			if( ( path == null ) ||
				( path.Length == 0 ) ||
				( Directory.Exists( path ) == false ) )
			{
				// Error!
				return null;
			}

			long capacity = parameters.GetValue<long>( MediaConfiguration.CapacitySetting, 1024 * 1024 * 1800 );

			return new UmdDevice( emulator, parameters, path, capacity );
		}
	}
}
