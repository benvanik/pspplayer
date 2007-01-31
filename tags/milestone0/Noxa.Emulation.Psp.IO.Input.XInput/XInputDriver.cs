// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Noxa.Emulation.Psp.IO.Input.XInput;
using System.Diagnostics;

namespace Noxa.Emulation.Psp.IO.Input
{
	public class XInputDriver : IComponent
	{
		public ComponentType Type
		{
			get
			{
				return ComponentType.Input;
			}
		}

		public string Name
		{
			get
			{
				return "XInput Controller";
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
			List<ComponentIssue> issues = new List<ComponentIssue>();
			if( NativeXInput.LibraryExists == false )
				issues.Add( new ComponentIssue( this, IssueLevel.Error, Strings.ErrorLibraryNotFound, Strings.ErrorLibraryNotFoundUrl ) );
			return issues;
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
			// Ensure library is present
			Debug.Assert( NativeXInput.LibraryExists == true );
			if( NativeXInput.LibraryExists == false )
				return null;

			return new XInputPad( emulator, parameters );
		}
	}
}
