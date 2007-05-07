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

using Noxa.Emulation.Psp.Bios;
using Noxa.Emulation.Psp.Games;

namespace Noxa.Emulation.Psp.GameTester
{
	class Program
	{
		static void Main( string[] args )
		{
			//string path = @"C:\Dev\Noxa.Emulation\UMD Images";
			string path = @"F:\UMD Images";
			string resultDir = Path.Combine( path, "Reports" );
			foreach( string umdPath in Directory.GetFiles( path, "*.iso" ) )
			{
				Console.WriteLine( "-------------------------------------------------------------------------------" );
				Console.WriteLine( "- UMD ISO: " + Path.GetFileName( umdPath ) );
				Console.WriteLine( "-------------------------------------------------------------------------------" );
				try
				{
					TestHost host = new TestHost();
					host.CreateInstance();
					TestInstance instance = host.CurrentInstance as TestInstance;
					instance.Umd.Load( umdPath, false );
					
					GameInformation game = GameLoader.FindGame( instance.Umd );
					Console.WriteLine( string.Format( "Loading game {0} ({1})", game.Parameters.Title, game.Parameters.DiscID ) );
					
					LoadResults results = instance.SwitchToGame( game );
					if( results.Successful == true )
					{
						Console.WriteLine( "Imports: {0}, Exports: {1}", results.Imports.Count, results.Exports.Count );
						GameLoader.GenerateReport( instance, game, results, resultDir );
					}
					else
					{
						Console.WriteLine( "!!!! Failed to load" );
					}
					instance.Destroy();
				}
				catch( Exception ex )
				{
					Console.WriteLine( "Exception while processing: " + ex.ToString() );
					Debugger.Break();
				}

				Console.WriteLine( "" );

				GC.Collect();
			}
		}
	}
}
