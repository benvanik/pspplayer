// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

using Noxa.Emulation.Psp.Debugging.DebugModel;

namespace Noxa.Emulation.Psp.Debugging.DebugData
{
	class ObjdumpDebugData : IProgramDebugData
	{
		private List<ObjdumpMethod> _methods;
		private Method[] _methodCache;
		private Dictionary<int, ObjdumpMethod> _methodLookup;

		private ObjdumpMethod _textMethod;

		public ObjdumpDebugData( Stream source )
		{
			_methods = GetMethods( source, out _textMethod );
			if( ( _methods == null ) ||
				( _methods.Count == 0 ) )
				throw new InvalidDataException( "The given objdump report was not valid." );
			_methodLookup = new Dictionary<int, ObjdumpMethod>( _methods.Count );
			foreach( ObjdumpMethod method in _methods )
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

		private List<ObjdumpMethod> GetMethods( Stream source, out ObjdumpMethod textMethod )
		{
			textMethod = null;

			Regex methodRegex = new Regex( @"(?<Address>\w{8})\ <(?<Identifier>.+?)>:" );
			Regex instrRegex = new Regex( @"\ (?<Address>\w{7}):\t(?<Code>\w{8})\ \t(?<Opcode>.+?)\t(?<Operands>.+?)$" );

			List<ObjdumpMethod> methods = new List<ObjdumpMethod>( 1024 );

			using( StreamReader reader = new StreamReader( source ) )
			{
				ObjdumpMethod method = null;

				string line;
				while( ( line = reader.ReadLine() ) != null )
				{
					if( line == string.Empty )
						continue;

					Match match = instrRegex.Match( line );
					if( match.Success == true )
					{
						Debug.Assert( method != null );
						if( method == null )
							continue;

						int address = int.Parse( match.Groups[ "Address" ].Value, NumberStyles.HexNumber );
						int code = int.Parse( match.Groups[ "Code" ].Value, NumberStyles.HexNumber );
						string opcode = match.Groups[ "Opcode" ].Value;
						string operands = match.Groups[ "Operands" ].Value.Trim();
						Instruction instr = new Instruction( address, code, opcode, operands );
						method.Instructions.Add( instr.Address, instr );
					}
					else
					{
						match = methodRegex.Match( line );
						if( match.Success == true )
						{
							if( method != null )
								method.SetLength();
							int address = int.Parse( match.Groups[ "Address" ].Value, NumberStyles.HexNumber );
							string name = match.Groups[ "Identifier" ].Value;
							method = new ObjdumpMethod( address, name );
							methods.Add( method );
						}
					}
				}

				if( method != null )
					method.SetLength();
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
					ObjdumpMethod method = _methods[ n ];
					if( ( method.EntryAddress <= address ) &&
						( ( method.EntryAddress + ( ( method.Instructions.Count + 1 ) * 4 ) ) >= address ) )
						return method;
				}
				return null;
			}
		}

		#region ObjdumpMethod

		private class ObjdumpMethod : Method
		{
			public ObjdumpMethod( int entryAddress, string name )
				: base( entryAddress, 0, name )
			{
			}

			internal void SetLength()
			{
				_length = _instructions.Count;
			}
		}

		#endregion
	}
}
