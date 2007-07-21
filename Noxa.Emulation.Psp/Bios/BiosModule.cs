// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Noxa.Emulation.Psp.Bios
{
	/// <summary>
	/// Represents a module inside the BIOS that can contain <see cref="BiosFunction"/> instances.
	/// </summary>
	public class BiosModule
	{
		/// <summary>
		/// The name of the module.
		/// </summary>
		public readonly string Name;

		/// <summary>
		/// A list of functions and variables exported by the module.
		/// </summary>
		public readonly StubExport[] Exports;

		private Dictionary<uint, StubExport> _exportVariableLookup;
		private Dictionary<uint, StubExport> _exportFunctionLookup;

		/// <summary>
		/// Initializes a new <see cref="BiosModule"/> instance with the given parameters.
		/// </summary>
		/// <param name="name">The name of the module.</param>
		public BiosModule( string name )
		{
			Debug.Assert( name != null );
			Name = name;
			Exports = new StubExport[] { };
		}

		/// <summary>
		/// Initializes a new <see cref="BiosModule"/> instance with the given parameters.
		/// </summary>
		/// <param name="name">The name of the module.</param>
		/// <param name="exports">A list of functions and variables exported by the module.</param>
		public BiosModule( string name, StubExport[] exports )
		{
			Debug.Assert( name != null );
			Name = name;
			Exports = exports;
			_exportVariableLookup = new Dictionary<uint, StubExport>( exports.Length );
			_exportFunctionLookup = new Dictionary<uint, StubExport>( exports.Length );
			foreach( StubExport export in exports )
			{
				switch( export.Type )
				{
					case StubType.Variable:
						_exportVariableLookup[ export.NID ] = export;
						break;
					case StubType.Function:
						_exportFunctionLookup[ export.NID ] = export;
						break;
				}
			}
		}

		/// <summary>
		/// Lookup an export inside the module.
		/// </summary>
		/// <param name="type">The type of the export to find.</param>
		/// <param name="nid">The NID of the export to look up.</param>
		/// <returns>The <see cref="StubExport"/> or <c>null</c> if it was not found.</returns>
		public StubExport LookupExport( StubType type, uint nid )
		{
			StubExport value;
			switch( type )
			{
				case StubType.Function:
					if( _exportFunctionLookup.TryGetValue( nid, out value ) == true )
						return value;
					break;
				case StubType.Variable:
					if( _exportVariableLookup.TryGetValue( nid, out value ) == true )
						return value;
					break;
			}
			return null;
		}
	}
}
