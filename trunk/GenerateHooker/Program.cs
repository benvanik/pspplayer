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
						//if( ( library.Flags & PrxLibrary.UserFlag ) == 0x0 )
						//    continue;

						foreach( PrxFunction function in library.Functions )
						{
							if( function.Ignored == true )
								continue;

							if( function.Source != null )
							{
								bool hasReturn;
								string transformed = TransformDeclaration( function.Name, function.Source.DeclarationBlock, out hasReturn );
								writer.WriteLine( "1;{0};0x{1:X8};{2};{3};{4}", prx.Name, function.NID, function.Name, "iiii", "i" );
							}
							else
							{
								// Not found
								//Debug.WriteLine( string.Format( "{0}::{1} not found", prx.Name, function.NID ) );
							}
						}
					}
				}
			}
		}

		private const string Int64Name = "long";
		private static List<string> _int64types = new List<string>( new string[] { "SceOff", "SceInt64", "unsigned long long", "long long", } );

		private static string TransformDeclaration( string functionName, string input, out bool hasReturn )
		{
			string returnType;
			if( input.StartsWith( "void " + functionName ) == true )
			{
				returnType = "void";
				hasReturn = false;
			}
			else
			{
				string origrt = input.Substring( 0, input.IndexOf( ' ' ) );
				if( origrt == "unsigned" )
				{
					// Hrm just hope it isn't unsigned long long ;)
					if( input.StartsWith( "unsigned long long" ) == true )
						origrt = "unsigned long long";
				}
				else if( origrt == "long" )
				{
					if( input.StartsWith( "long long" ) == true )
						origrt = "long long";
				}
				if( _int64types.Contains( origrt ) == true )
					returnType = Int64Name;
				else
					returnType = "int";
				hasReturn = true;
			}

			string args = "";
			string sargs = input.Substring( input.IndexOf( '(' ) + 1 );
			sargs = sargs.Substring( 0, sargs.LastIndexOf( ')' ) ).Trim();
			if( sargs.Length > 0 )
			{
				string[] aargs = sargs.Split( ',' );
				foreach( string arg in aargs )
				{
					string targ = arg.Trim();

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

					if( type.IndexOf( '*' ) >= 0 )
						type = type.Replace( "*", "" ).Trim();
					bool is64 = _int64types.Contains( type );
					// TODO: figure out if 64 or not!
					string convtype = ( is64 == true ) ? Int64Name : "int";

					args += string.Format( "{0}{1} {2}", ( args.Length != 0 ) ? ", " : "", convtype, name );
				}

				args = string.Format( " {0} ", args );
			}

			string output = string.Format( "{0} {1}({2})", returnType, functionName, args );
			//Debug.WriteLine( " ------------------------------------ " );
			//Debug.WriteLine( input );
			//Debug.WriteLine( output );
			return output;
		}
	}
}
