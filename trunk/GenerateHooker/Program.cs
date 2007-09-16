// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2007 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace GenerateHooker
{
	class Program
	{
		// If true, all functions without an sce prefix will be ignored
		const bool IgnoreNonScePrefixed = false;

		// Load the ignore list
		const bool UseIgnores = true;
		const string IgnoreList = "IgnoreList.txt";

		const bool FullDump = false;

		static void Main( string[] args )
		{
			NidList list = NidList.FromFile( "psplibdoc.xml" );

			List<string> paths = new List<string>();
			paths.AddRange( new string[]{
				//@"C:\Dev\Noxa.Emulation\pspsdk\src",
				@"C:\Dev\pspsdk\src",
			} );
			CodeFinder finder = new CodeFinder();
			foreach( string path in paths )
				finder.AddPath( path );

			// These are ones that give us trouble
			List<string> functionIgnores = new List<string>();
			if( UseIgnores == true )
			{
				foreach( string ignore in File.ReadAllLines( IgnoreList ) )
				{
					if( ignore.Length == 0 )
						continue;
					string trimmed = ignore.Trim();
					if( functionIgnores.Contains( trimmed ) == false )
						functionIgnores.Add( trimmed );
				}

				Console.WriteLine( " - Loaded a list of {0} functions to ignore", functionIgnores.Count );
			}

			// PRXs we don't want
			List<string> prxIgnores = new List<string>();
			prxIgnores.Add( "kd/vaudio.prx" );	// we just want kd/vaudio_game.prx

			List<string> prxForces = new List<string>();

			Dictionary<string, PrxLibrary> dupeList = new Dictionary<string, PrxLibrary>( 1000 );

			foreach( PrxFile prx in list.Files )
			{
				if( prxIgnores.Contains( prx.FileName ) == true )
				{
					Console.WriteLine( " - Ignoring PRX {0} ({1})", prx.Name, prx.FileName );
					continue;
				}

				bool force = ( FullDump == true ) || ( prxForces.Contains( prx.FileName ) == true );

				foreach( PrxLibrary library in prx.Libraries )
				{
					if( force == false )
					{
						if( ( library.Flags & PrxLibrary.UserFlag ) == 0x0 )
						{
							Console.WriteLine( " - Ignoring library {0} in prx {1}; flags {2:X8}", library.Name, prx.Name, library.Flags );
							continue;
						}
					}

					foreach( PrxFunction function in library.Functions )
					{
						if( ( functionIgnores.Contains( function.Name ) == true ) ||
							( function.Name.StartsWith( "_" ) == true ) )
						{
							function.Ignored = true;
							continue;
						}

						if( IgnoreNonScePrefixed == true )
						{
							if( function.Name.StartsWith( "sce" ) == false )
							{
								Console.WriteLine( " - Ignoring {0}::{1} because non-sce", library.Name, function.Name );
								function.Ignored = true;
							}
						}

						if( dupeList.ContainsKey( function.Name ) == true )
						{
							PrxLibrary existing = dupeList[ function.Name ];
							Console.WriteLine( "!! duplicate function found" );
							function.Ignored = true;
							//Debugger.Break();
						}
						else
						{
							dupeList.Add( function.Name, library );
							finder.AddRequest( string.Format( " {0}(", function.Name ), function );
						}
					}
				}
			}

			finder.Search();

			Console.WriteLine( "Found {0} references, missing {1}.", finder.FoundRequests.Count, finder.PendingRequests.Count );

			using( StreamWriter writer = new StreamWriter( "hooks.txt" ) )
			{
				foreach( PrxFile prx in list.Files )
				{
					if( prxIgnores.Contains( prx.FileName ) == true )
						continue;

					foreach( PrxLibrary library in prx.Libraries )
					{
						if( FullDump == false )
						{
							if( ( library.Flags & PrxLibrary.UserFlag ) == 0x0 )
								continue;
						}

						foreach( PrxFunction function in library.Functions )
						{
							if( function.Ignored == true )
								continue;

							if( function.Source != null )
							{
								string returnType;
								string argFormat = string.Empty;
								try
								{
									string[] argTypes;
									string transformed = TransformDeclaration( function.Name, function.Source.DeclarationBlock, out returnType, out argTypes );
									foreach( string arg in argTypes )
										argFormat += EncodeType( arg );
								}
								catch
								{
									argFormat = "xxxxxx";
									returnType = "int";
								}
								//Debug.WriteLine( string.Format( "{0} -> {1};{2}", function.Source.DeclarationBlock, argFormat, EncodeType( returnType ) ) );
								writer.WriteLine( "0;{0};0x{1:X8};{2};{3};{4}", prx.Name, function.NID, function.Name, ( argFormat.Length == 0 ) ? "v" : argFormat, EncodeType( returnType ) );
							}
							else
							{
								// Not found
								//Debug.WriteLine( string.Format( "{0}::{1} not found", prx.Name, function.NID ) );
								writer.WriteLine( "0;{0};0x{1:X8};{2};{3};{4}", prx.Name, function.NID, function.Name, "xxxxxx", "x" );
							}
						}
					}
				}
			}
		}

		/*
		#define TYPE_INT16	'h'
		#define TYPE_INT32	'i'
		#define TYPE_INT64	'l'
		#define TYPE_HEX32	'x'
		#define TYPE_HEX64	'X'
		#define TYPE_OCT32	'o'
		#define TYPE_SINGLE	'f'
		#define TYPE_STRING	's'
		#define TYPE_VOID	'v'*/
		private static char EncodeType( string type )
		{
			type = type.Replace( "const ", "" );
			if( type.Contains( "char*" ) == true )
				return 's';
			else if( type.EndsWith( "*" ) == true )
				return 'x';
			else
			{
				switch( type )
				{
					case "void":
					case "SceVoid":
						return 'v';
					case "u16":
					case "short":
					case "unsigned short":
						return 'h';
					case "int":
					case "unsigned":
					case "unsigned int":
					case "long":
					case "SceSize":
					case "u32":
					case "s32":
					case "time_t": // I think?
					case "clock_t": // I think?
					case "SceUInt":
					case "SceInt32":
						return 'i';
					case "SceOff":
					case "SceInt64":
					case "long long":
					case "unsigned long long":
					case "u64":
						return 'l';
					case "SceUID":
					case "REGHANDLE":
					case "ScePVoid":
						return 'x';
					case "SceMode":
						return 'o';
					case "float":
						return 'f';
					default:
						Debug.WriteLine( string.Format( "unknown type: '{0}'", type ) );
						return 'x';
				}
			}
		}

		private static string TransformDeclaration( string functionName, string input, out string returnType, out string[] argTypes )
		{
			if( input.StartsWith( "void " + functionName ) == true )
			{
				returnType = "void";
			}
			else
			{
				string origrt = input.Substring( 0, input.IndexOf( functionName ) );
				returnType = origrt.Replace( " *", "*" ).Trim();
			}

			string args = "";
			string sargs = input.Substring( input.IndexOf( '(' ) + 1 );
			sargs = sargs.Substring( 0, sargs.LastIndexOf( ')' ) ).Trim();
			if( sargs.Length > 0 )
			{
				string[] aargs = sargs.Split( ',' );
				argTypes = new string[ aargs.Length ];
				for( int n = 0; n < aargs.Length; n++ )
				{
					string targ = aargs[ n ].Trim();

					// Could replace all this with a regex...
					string type;
					string name;
					if( targ.IndexOf( '*' ) >= 0 )
					{
						type = targ.Substring( 0, targ.LastIndexOf( '*' ) + 1 );
						name = targ.Substring( type.Length );
					}
					else
					{
						type = targ.Substring( 0, targ.LastIndexOf( ' ' ) );
						name = targ.Substring( type.Length + 1 );
					}

					type = type.Trim();
					name = name.Trim();

					type = type.Replace( " *", "* " );

					args += string.Format( "{0}{1} {2}", ( args.Length != 0 ) ? ", " : "", type, name );

					argTypes[ n ] = type.Trim();
				}

				args = string.Format( " {0} ", args );
			}
			else
				argTypes = new string[ 0 ];

			return string.Format( "{0} {1}({2})", returnType, functionName, args );
		}
	}
}
