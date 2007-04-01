// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Noxa.Emulation.Psp.Bios
{
	/// <summary>
	/// Module loader.
	/// </summary>
	public interface ILoader
	{
		/// <summary>
		/// The <see cref="IBios"/> instance that owns the loader.
		/// </summary>
		IBios Bios
		{
			get;
		}

		/// <summary>
		/// Load a module (BOOT.BIN/ELF, PRX).
		/// </summary>
		/// <param name="type">The type of module being loaded.</param>
		/// <param name="moduleStream">A <see cref="Stream"/> containing the executable image.</param>
		/// <param name="parameters">Parameters describing how to load the executable image.</param>
		/// <returns>The results of the load. <see cref="LoadResults.Successful"/> will be <c>false</c> if the load failed.</returns>
		LoadResults LoadModule( ModuleType type, Stream moduleStream, LoadParameters parameters );
	}

	/// <summary>
	/// Defines the type of module loaded.
	/// </summary>
	public enum ModuleType
	{
		/// <summary>
		/// A game - BOOT.BIN, BOOT.ELF, etc.
		/// </summary>
		Boot,

		/// <summary>
		/// A library - *.PRX.
		/// </summary>
		Prx,
	}
}
