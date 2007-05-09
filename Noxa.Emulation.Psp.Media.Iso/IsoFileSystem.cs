// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Noxa.Emulation.Psp.Media.Iso.Configuration;
using System.Diagnostics;

namespace Noxa.Emulation.Psp.Media.Iso
{
	public class IsoFileSystem : IComponent
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
				return "UMD ISO Media";
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
				return true;
			}
		}

		public IComponentConfiguration CreateConfiguration( ComponentParameters parameters )
		{
			return new Iso.Configuration.MediaConfiguration( parameters );
		}

		public IComponentInstance CreateInstance( IEmulationInstance emulator, ComponentParameters parameters )
		{
			string path = parameters.GetValue<string>( MediaConfiguration.PathSetting, null );
			if( ( path == null ) ||
				( path.Length == 0 ) ||
				( File.Exists( path ) == false ) )
			{
				// Path not found - this is just a generic UMD
				if( ( path != null ) &&
					( path.Length > 0 ) )
					Log.WriteLine( Verbosity.Critical, Feature.Media, "IsoFileSystem: unable to create instance, path {0} not found", path );
				return new UmdDevice( emulator, parameters );
			}

			return new UmdDevice( emulator, parameters, path );
		}
	}
}
