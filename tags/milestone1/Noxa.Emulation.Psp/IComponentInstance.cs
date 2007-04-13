// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace Noxa.Emulation.Psp
{
	/// <summary>
	/// Represents an active instance of a <see cref="IComponent"/>.
	/// </summary>
	public interface IComponentInstance
	{
		/// <summary>
		/// The <see cref="ComponentParameters"/> used to create the instance.
		/// </summary>
		ComponentParameters Parameters
		{
			get;
		}

		/// <summary>
		/// The current <see cref="IEmulationInstance"/> that owns the instance.
		/// </summary>
		IEmulationInstance Emulator
		{
			get;
		}

		/// <summary>
		/// The <see cref="Type"/> factory that created the instance.
		/// </summary>
		Type Factory
		{
			get;
		}

		/// <summary>
		/// Discard any resources used.
		/// </summary>
		void Cleanup();
	}
}
