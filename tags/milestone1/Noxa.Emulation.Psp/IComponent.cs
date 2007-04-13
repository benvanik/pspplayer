// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Noxa.Emulation.Psp
{
	/// <summary>
	/// Describes the type of component.
	/// </summary>
	public enum ComponentType
	{
		/// <summary>
		/// Audio driver.
		/// </summary>
		Audio = 0,

		/// <summary>
		/// BIOS.
		/// </summary>
		Bios = 1,

		/// <summary>
		/// CPU.
		/// </summary>
		Cpu = 2,

		/// <summary>
		/// Input driver.
		/// </summary>
		Input = 3,

		/// <summary>
		/// Memory Stick device.
		/// </summary>
		UserMedia = 4,

		/// <summary>
		/// UMD device.
		/// </summary>
		GameMedia = 5,

		/// <summary>
		/// Network IO driver.
		/// </summary>
		Network = 6,

		/// <summary>
		/// Video driver.
		/// </summary>
		Video = 7,

		/// <summary>
		/// Other.
		/// </summary>
		Other = 9,
	}

	/// <summary>
	/// Describes the components build type.
	/// </summary>
	public enum ComponentBuild
	{
		/// <summary>
		/// Debugging build.
		/// </summary>
		Debug = 0,

		/// <summary>
		/// Testing build.
		/// </summary>
		Testing = 1,

		/// <summary>
		/// Release build.
		/// </summary>
		Release = 2,
	}
	
	/// <summary>
	/// An emulation component; also known as a plugin.
	/// </summary>
	public interface IComponent
	{
		/// <summary>
		/// The type of the component.
		/// </summary>
		ComponentType Type
		{
			get;
		}

		/// <summary>
		/// A human-friendly name for the component.
		/// </summary>
		string Name
		{
			get;
		}

		/// <summary>
		/// The version of the component.
		/// </summary>
		Version Version
		{
			get;
		}

		/// <summary>
		/// The author name and email of the component.
		/// </summary>
		string Author
		{
			get;
		}

		/// <summary>
		/// The website where the component can be found.
		/// </summary>
		string Website
		{
			get;
		}

		/// <summary>
		/// An RSS feed containing updates for the component.
		/// </summary>
		string RssFeed
		{
			get;
		}

		/// <summary>
		/// The build type of the component.
		/// </summary>
		ComponentBuild Build
		{
			get;
		}

		/// <summary>
		/// <c>true</c> if the component can be tested.
		/// </summary>
		bool IsTestable
		{
			get;
		}

		/// <summary>
		/// Perform a test of the component and generate a list of issues.
		/// </summary>
		/// <param name="parameters">The parameters that are to be tested.</param>
		/// <returns>A list of <see cref="ComponentIssue"/> instances describing any issues with the component.</returns>
		IList<ComponentIssue> Test( ComponentParameters parameters );

		/// <summary>
		/// <c>true</c> if the component can be configured.
		/// </summary>
		bool IsConfigurable
		{
			get;
		}

		/// <summary>
		/// Create a configuration UI.
		/// </summary>
		/// <param name="parameters">The current parameters.</param>
		/// <returns>The component configuration UI.</returns>
		IComponentConfiguration CreateConfiguration( ComponentParameters parameters );

		/// <summary>
		/// Create an instance of the component.
		/// </summary>
		/// <param name="emulator">The emulator instance that will host the component.</param>
		/// <param name="parameters">The parameters for the component.</param>
		/// <returns>A new <see cref="IComponentInstance"/> with the given parameters.</returns>
		IComponentInstance CreateInstance( IEmulationInstance emulator, ComponentParameters parameters );
	}
}
