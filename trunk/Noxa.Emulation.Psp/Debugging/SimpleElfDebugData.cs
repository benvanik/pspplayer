// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Noxa.Emulation.Psp.Games;

namespace Noxa.Emulation.Psp.Debugging
{
	class SimpleElfDebugData : IProgramDebugData
	{
		private List<ElfMethod> _methods;
		private Method[] _methodCache;
		private Dictionary<int, ElfMethod> _methodLookup;

		public SimpleElfDebugData( Stream source )
		{
			_methods = GetMethods( source );
			if( _methods == null )
				throw new InvalidDataException( "The given ELF program did not contain symbol information or was not valid." );
			_methodLookup = new Dictionary<int, ElfMethod>( _methods.Count );
			foreach( ElfMethod method in _methods )
				_methodLookup.Add( method.EntryAddress, method );
		}

		public Method[] Methods
		{
			get
			{
				if( _methodCache == null )
					_methodCache = ( Method[] )_methods.ToArray();
				return _methodCache;
			}
		}

		#region Method detection/etc

		private List<ElfMethod> GetMethods( Stream source )
		{
			List<ElfMethod> methods = new List<ElfMethod>();

			ElfFile elf = new ElfFile( source );
			bool relocate = ( elf.EntryAddress < 0x08900000 );
			
			// Get functions
			List<Noxa.Emulation.Psp.Games.ElfFile.ElfSymbol> symbols = elf.Symbols;
			foreach( Noxa.Emulation.Psp.Games.ElfFile.ElfSymbol symbol in symbols )
			{
				// Some NOTYPES are valid, like ctor/dtor aux, frame_dummy, etc.
				// Unfortunately, so many are bad that I can't just include them :(

				if( symbol.SymbolType == ElfFile.ElfSymbolType.Function )
				{
					// Skip ones with no address (do we skip 0 length too?)
					if( symbol.Value == 0 )
						continue;

					int address = ( int )symbol.Value;
					if( relocate == true )
						address += 0x08900000;

					ElfMethod method = new ElfMethod( address, ( int )symbol.Size, symbol.Name );
					methods.Add( method );
				}
			}

			return methods;
		}

		#endregion

		public Method FindMethod( int address )
		{
			if( _methodLookup.ContainsKey( address ) == true )
				return _methodLookup[ address ];
			else
			{
				for( int n = 0; n < _methods.Count; n++ )
				{
					ElfMethod method = _methods[ n ];
					if( ( method.EntryAddress <= address ) &&
						( ( method.EntryAddress + method.Length ) > address ) )
						return method;
				}
				return null;
			}
		}

		#region ElfMethod

		private class ElfMethod : Method
		{
			public ElfMethod( int entryAddress, int length, string name )
				: base( entryAddress, length, name )
			{
			}
		}

		#endregion
	}
}
