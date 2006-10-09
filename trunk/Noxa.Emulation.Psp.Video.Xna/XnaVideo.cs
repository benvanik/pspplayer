// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Noxa.Emulation.Psp.Video.Xna;
using Noxa.Emulation.Psp.Video.Xna.Configuration;
using System.Diagnostics;

namespace Noxa.Emulation.Psp.Video
{
	public class XnaVideo : IComponent
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
				return "Managed XNA Video";
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
				return true;
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

		private void EnsureDefaults( ComponentParameters parameters )
		{
			if( parameters.ContainsKey( XnaSettings.Multithreaded ) == false )
				parameters[ XnaSettings.Multithreaded ] = ( Environment.ProcessorCount > 1 );
		}

		private bool EnsureValid( ComponentParameters parameters )
		{
			if( ( Environment.ProcessorCount == 1 ) &&
				( ( bool )parameters[ XnaSettings.Multithreaded ] == true ) )
			{
				Debug.Assert( false, "Multiple cores are probably required for multithreading" );
				return false;
			}

			return true;
		}

		public IComponentConfiguration CreateConfiguration( ComponentParameters parameters )
		{
			this.EnsureDefaults( parameters );
			return XnaConfigurationFactory.Create( parameters );
		}

		public IComponentInstance CreateInstance( IEmulationInstance emulator, ComponentParameters parameters )
		{
			this.EnsureDefaults( parameters );

			if( this.EnsureValid( parameters ) == false )
				return null;

			return new VideoDriver( emulator, parameters );
		}
	}
}
