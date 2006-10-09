// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Noxa.Emulation.Psp
{
	public enum ComponentType
	{
		Audio,
		Bios,
		Cpu,
		Input,
		UserMedia,
		GameMedia,
		Network,
		Video,
		Other
	}

	public enum ComponentBuild
	{
		Debug,
		Testing,
		Release
	}

	public interface IComponent
	{
		ComponentType Type
		{
			get;
		}

		string Name
		{
			get;
		}

		Version Version
		{
			get;
		}

		string Author
		{
			get;
		}

		string Website
		{
			get;
		}

		string RssFeed
		{
			get;
		}

		ComponentBuild Build
		{
			get;
		}

		bool IsTestable
		{
			get;
		}

		IList<ComponentIssue> Test( ComponentParameters parameters );

		bool IsConfigurable
		{
			get;
		}

		IComponentConfiguration CreateConfiguration( ComponentParameters parameters );

		IComponentInstance CreateInstance( IEmulationInstance emulator, ComponentParameters parameters );
	}
}
