// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;
using System.Threading;

namespace GenerateStubsV2
{
	class Program
	{
		// If true, all functions without an sce prefix will be ignored
		const bool IgnoreNonScePrefixed = false;

		// Load the ignore list
		const bool UseIgnores = true;
		const string IgnoreList = "IgnoreList.txt";

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

			IOutput output = new FastHLEOutput( paths, @"C:\Dev\Noxa.Emulation\trunk\Noxa.Emulation.Psp.Bios.FastHLE\Modules\" );

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

			// Libraries that will be merged (in the form of source -> target
			Dictionary<string, string> libraryMerges = new Dictionary<string, string>();
			libraryMerges.Add( "sceUtility_netparam_internal", "sceUtility" );
			libraryMerges.Add( "sceWlanDrv_lib", "sceWlanDrv" );

			Dictionary<string, PrxLibrary> dupeList = new Dictionary<string, PrxLibrary>( 10000 );

			Dictionary<string, PrxLibrary> libraryLookup = new Dictionary<string, PrxLibrary>();
			foreach( PrxFile prx in list.Files )
			{
				if( prxIgnores.Contains( prx.FileName ) == true )
				{
					Console.WriteLine( " - Ignoring PRX {0} ({1})", prx.Name, prx.FileName );
					continue;
				}

				foreach( PrxLibrary library in prx.Libraries )
				{
					if( ( library.Flags & PrxLibrary.UserFlag ) == 0x0 )
					{
						Console.WriteLine( " - Ignoring library {0} in prx {1}; flags {2:X8}", library.Name, prx.Name, library.Flags );
						continue;
					}

					libraryLookup.Add( library.Name, library );

					foreach( PrxFunction function in library.Functions )
					{
						if( functionIgnores.Contains( function.Name ) == true )
							continue;

						if( IgnoreNonScePrefixed == true )
						{
							if( function.Name.StartsWith( "sce" ) == false )
							{
								Console.WriteLine( " - Ignoring {0}::{1} because non-sce", library.Name, function.Name );
							}
						}

						if( dupeList.ContainsKey( function.Name ) == true )
						{
							PrxLibrary existing = dupeList[ function.Name ];
							Console.WriteLine( "!! duplicate function found" );
							Debugger.Break();
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

			// Quick hack up to get merged on to parents
			List<PrxLibrary> skipLibraries = new List<PrxLibrary>();
			foreach( KeyValuePair<string, string> pair in libraryMerges )
			{
				PrxLibrary source = libraryLookup[ pair.Key ];
				PrxLibrary target = libraryLookup[ pair.Value ];
				target.Merged = source;
				skipLibraries.Add( source );
			}

			foreach( PrxFile prx in list.Files )
			{
				if( prxIgnores.Contains( prx.FileName ) == true )
					continue;

				foreach( PrxLibrary library in prx.Libraries )
				{
					if( ( library.Flags & PrxLibrary.UserFlag ) == 0x0 )
						continue;
					if( skipLibraries.Contains( library ) == true )
						continue;

					bool hasBegun = false;

					foreach( PrxFunction function in library.Functions )
					{
						if( function.Source != null )
						{
							if( hasBegun == false )
								output.BeginLibrary( library );
							hasBegun = true;
							output.WriteFunction( function );
						}
					}

					// Repeat for merged
					if( library.Merged != null )
					{
						foreach( PrxFunction function in library.Merged.Functions )
						{
							if( function.Source != null )
							{
								if( hasBegun == false )
									output.BeginLibrary( library );
								hasBegun = true;
								output.WriteFunction( function );
							}
						}
					}

					if( hasBegun == true )
						output.EndLibrary();
				}
			}

			//foreach( FindRequest req in finder.FoundRequests )
			//{
			//    Debug.WriteLine( " -- " + ( req.Tag as PrxFunction ).Name );
			//    Debug.WriteLine( req.CommentBlock );
			//    Debug.WriteLine( req.DeclarationBlock );
			//}
		}
	}
}
