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
		Audio = 0,
		Bios = 1,
		Cpu = 2,
		Input = 3,
		UserMedia = 4,
		GameMedia = 5,
		Network = 6,
		Video = 7,
		Other = 9
	}

	public enum ComponentBuild
	{
		Debug = 0,
		Testing = 1,
		Release = 2
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
