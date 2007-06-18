// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace Noxa.Emulation.Psp.Bios
{
	/// <summary>
	/// Results of an <see cref="ILoader"/> operation.
	/// </summary>
	public class LoadResults
	{
		/// <summary>
		/// <c>true</c> if the load was successful.
		/// </summary>
		public bool Successful;

		/// <summary>
		/// <c>true</c> if the load was ignored (dupe, etc).
		/// </summary>
		public bool Ignored;

		/// <summary>
		/// The lower memory address of the module after loading.
		/// </summary>
		public uint LowerBounds;

		/// <summary>
		/// The upper memory address of the module after loading.
		/// </summary>
		public uint UpperBounds;

		/// <summary>
		/// The entry address, as defined by the module.
		/// </summary>
		public uint EntryAddress;

		/// <summary>
		/// The name of the module.
		/// </summary>
		public string Name;

		/// <summary>
		/// The $GP, as defined by the module.
		/// </summary>
		public uint GlobalPointer;

		/// <summary>
		/// A list of stubs imported by the module.
		/// </summary>
		public List<StubImport> Imports;

		/// <summary>
		/// A list of stubs exported by the module.
		/// </summary>
		public List<StubExport> Exports;

		/// <summary>
		/// Used internally to preserve important information. Only present if requested in <see cref="LoadParameters"/>.
		/// </summary>
		public IntPtr PreservedData;
	}

	/// <summary>
	/// Defines the type of the <see cref="StubExport"/> or <see cref="StubImport"/>.
	/// </summary>
	public enum StubType
	{
		/// <summary>
		/// Exported or imported as a function pointer.
		/// </summary>
		Function,

		/// <summary>
		/// Exported or imported as a variable.
		/// </summary>
		Variable,
	}

	/// <summary>
	/// Defines the types of <see cref="StubImport"/> results.
	/// </summary>
	public enum StubReferenceResult
	{
		/// <summary>
		/// Stub was found successfully and can be used.
		/// </summary>
		Success,

		/// <summary>
		/// The module containing the stub was not found.
		/// </summary>
		ModuleNotFound,

		/// <summary>
		/// The NID of the stub was not found in the given module.
		/// </summary>
		NidNotFound,

		/// <summary>
		/// The stub was found, but not implemented.
		/// </summary>
		NidNotImplemented,
	}

	/// <summary>
	/// NID import reference.
	/// </summary>
	public class StubImport
	{
		/// <summary>
		/// The result of the reference.
		/// </summary>
		public StubReferenceResult Result;

		/// <summary>
		/// The name of the module that contains the reference.
		/// </summary>
		public string ModuleName;

		/// <summary>
		/// The NID (unique ID) of the reference.
		/// </summary>
		public uint NID;

		/// <summary>
		/// The type of the import.
		/// </summary>
		public StubType Type;

		/// <summary>
		/// The address of the stub.
		/// </summary>
		public uint Address;

		/// <summary>
		/// The <see cref="BiosFunction"/>, if found, of the reference.
		/// </summary>
		public BiosFunction Function;
	}

	/// <summary>
	/// NID export information.
	/// </summary>
	public class StubExport
	{
		/// <summary>
		/// The name of the module that the reference is exported from.
		/// </summary>
		public string ModuleName;

		/// <summary>
		/// The NID (unique ID) of the export.
		/// </summary>
		public uint NID;

		/// <summary>
		/// The type of the export.
		/// </summary>
		public StubType Type;

		/// <summary>
		/// The address of the NID.
		/// </summary>
		public uint Address;

		/// <summary>
		/// <c>true</c> if this export is used by the loader.
		/// </summary>
		public bool IsSystem
		{
			get
			{
				return
					( NID == 0xF01D73A7 ) ||
					( NID == 0xD3744BE0 ) ||
					( NID == 0xD632ACDB ) ||
					( NID == 0x0F7C276C ) ||
					( NID == 0xCEE8593C ) ||
					( NID == 0xCF0CC697 );
			}
		}
	}
}
