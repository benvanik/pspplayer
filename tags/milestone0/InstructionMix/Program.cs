// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace InstructionMix
{
	class Program
	{
		static void Main( string[] args )
		{
			if( args.Length == 0 )
			{
				Console.WriteLine( "[--stdin] [dump filename]" );
				Console.WriteLine( "  --stdin: read psp-objdump -d output over stdin" );
				Console.WriteLine( " filename: file containing psp-objdump -d output" );
				return;
			}

			TextReader reader;
			if( args[ 0 ] == "--stdin" )
			{
				reader = Console.In;
			}
			else
			{
				if( File.Exists( args[ 0 ] ) == false )
				{
					Console.WriteLine( "File not found" );
					return;
				}

				string filename = args[ 0 ];
				reader = File.OpenText( filename );
			}

			Dictionary<string, int> mix = new Dictionary<string, int>();

			{
				while( true )
				{
					string line = reader.ReadLine();
					if( line == null )
						break;

					// 8900018:\t27bdffe0 \taddiu\tsp,sp,-32
					if( line.Length < 28 )
						continue;
					line = line.Substring( 20 );
					int tabIndex = line.IndexOf( '\t' );
					if( tabIndex < 0 )
						continue;
					line = line.Substring( 0, tabIndex );
					if( line == string.Empty )
						continue;

					string instruction = line;
					if( mix.ContainsKey( instruction ) == false )
						mix.Add( instruction, 0 );

					int count = mix[ instruction ];
					count++;
					mix[ instruction ] = count;
				}
			}

			List<MixEntry> entries = new List<MixEntry>( mix.Count );
			foreach( KeyValuePair<string, int> pair in mix )
				entries.Add( new MixEntry( pair ) );
			entries.Sort( delegate( MixEntry a, MixEntry b )
			{
				return b.Count.CompareTo( a.Count );
			} );

			foreach( MixEntry entry in entries )
			{
				Console.WriteLine( string.Format( "{0,-5} : {1}", entry.Instruction, entry.Count ) );
			}
		}

		class MixEntry
		{
			public string Instruction;
			public int Count;

			public MixEntry( KeyValuePair<string, int> pair )
			{
				Instruction = pair.Key;
				Count = pair.Value;
			}
		}
	}
}
