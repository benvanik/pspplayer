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
		/// The <see cref="BiosFunction"/>, if found, of the reference.
		/// </summary>
		public BiosFunction Function;

		/// <summary>
		/// Creates a new successful stub reference.
		/// </summary>
		/// <param name="function">The <see cref="BiosFunction"/> found for the reference.</param>
		/// <returns>A <see cref="StubImport"/>.</returns>
		public static StubImport Success(  BiosFunction function )
		{
			StubImport ret = new StubImport();
			ret.Result = StubReferenceResult.Success;
			ret.ModuleName = function.Module.Name;
			ret.NID = function.NID;
			ret.Function = function;
			return ret;
		}

		/// <summary>
		/// Creates a new stub reference for when the module is not found.
		/// </summary>
		/// <param name="moduleName">The name of the module containing the reference.</param>
		/// <param name="nid">The NID of the reference.</param>
		/// <returns>A <see cref="StubImport"/>.</returns>
		public static StubImport ModuleNotFound( string moduleName, uint nid )
		{
			StubImport ret = new StubImport();
			ret.Result = StubReferenceResult.ModuleNotFound;
			ret.ModuleName = moduleName;
			ret.NID = nid;
			return ret;
		}

		/// <summary>
		/// Creates a new stub reference for when the NID is not found in the module.
		/// </summary>
		/// <param name="moduleName">The name of the module containing the reference.</param>
		/// <param name="nid">The NID of the reference.</param>
		/// <returns>A <see cref="StubImport"/>.</returns>
		public static StubImport NidNotFound( string moduleName, uint nid )
		{
			StubImport ret = new StubImport();
			ret.Result = StubReferenceResult.NidNotFound;
			ret.ModuleName = moduleName;
			ret.NID = nid;
			return ret;
		}

		/// <summary>
		/// Creates a new stub reference for when the NID is not implemented.
		/// </summary>
		/// <param name="function">The <see cref="BiosFunction"/> found for the reference.</param>
		/// <returns>A <see cref="StubImport"/>.</returns>
		public static StubImport NidNotImplemented( BiosFunction function )
		{
			StubImport ret = new StubImport();
			ret.Result = StubReferenceResult.NidNotImplemented;
			ret.ModuleName = function.Module.Name;
			ret.NID = function.NID;
			ret.Function = function;
			return ret;
		}
	}

	/// <summary>
	/// Defines the type of the <see cref="StubExport"/>.
	/// </summary>
	public enum StubExportType
	{
		/// <summary>
		/// Exported as a function pointer.
		/// </summary>
		Function,

		/// <summary>
		/// Exported as a variable.
		/// </summary>
		Variable,
	}

	/// <summary>
	/// NID export information.
	/// </summary>
	public class StubExport
	{
		/// <summary>
		/// The NID (unique ID) of the export.
		/// </summary>
		public uint NID;

		/// <summary>
		/// The type of the export.
		/// </summary>
		public StubExportType Type;

		/// <summary>
		/// The address of the NID.
		/// </summary>
		public uint Address;
	}
}
