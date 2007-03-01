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
using System.Text.RegularExpressions;

namespace GenerateStubsV2
{
	class FindRequest
	{
		public string Query;
		public PrxFunction Function;

		public bool Found;

		public string FileName;
		public int LineNumber;
		public string CommentBlock;
		public string DeclarationBlock;

		public FindRequest( string query, PrxFunction function )
		{
			Query = query;
			Function = function;
		}
	}

	class CodeFinder
	{
		public List<string> Paths;
		public List<FindRequest> PendingRequests;
		public List<FindRequest> FoundRequests;

		public const string SearchPattern = "*.h";

		public CodeFinder()
		{
			Paths = new List<string>( 10 );
			PendingRequests = new List<FindRequest>( 1024 );
			FoundRequests = new List<FindRequest>( 1024 );
		}

		public void AddPath( string path )
		{
			Paths.Add( path );
		}

		public void AddRequest( string query, PrxFunction function )
		{
			PendingRequests.Add( new FindRequest( query, function ) );
		}

		private Regex _attributeRegex = new Regex( @"__attribute__\(\(.+\)\)" );
		private Regex _voidRegex = new Regex( @"\([ ]*void[ ]*\)" );
		private Regex _spaceRegex = new Regex( @"[ ]{2,}" );

		private string StripLine( string input )
		{
			int c0 = input.IndexOf( "/*" );
			if( c0 >= 0 )
			{
				int end = input.IndexOf( "*/" );
				input = input.Substring( 0, c0 ) + input.Substring( end + 2 );
			}

			int c1 = input.IndexOf( "//" );
			if( c1 >= 0 )
			{
				input = input.Substring( 0, c1 );
			}

			if( input.Contains( "__attribute__" ) == true )
			{
				input = _attributeRegex.Replace( input, "" );
			}

			if( input.Contains( "void" ) == true )
			{
				input = _voidRegex.Replace( input, "()" );
			}
			
			input = input.Replace( "extern", "" );
			input = input.Replace( "__extension__", "" );

			input = input.Replace( '\t', ' ' );
			input = _spaceRegex.Replace( input, " " );

			return input.Trim();
		}

		public void Search()
		{
			// Basic flow:
			// foreach path
			//    foreach file w/ right extension
			//        load file
			//        foreach pending request
			//            search file for request

			foreach( string path in Paths )
			{
				foreach( string fileName in Directory.GetFiles( path, SearchPattern, SearchOption.AllDirectories ) )
				{
					string[] lines = File.ReadAllLines( fileName );

					Console.Write( "Searching {0}... ", fileName );

					int found = 0;
					for( int n = 0; n < lines.Length; n++ )
					{
						string line = lines[ n ];
						if( line.Length == 0 )
							continue;

						// Ignore lines that start with some special chars
						if( ( line[ 0 ] == ' ' ) ||
							( line[ 0 ] == '\t' ) ||
							( line[ 0 ] == '*' ) ||
							( line[ 0 ] == '#' ) ||
							( line[ 0 ] == '/' ) )
							continue;

						if( line.Contains( "__inline__" ) )
							continue;

						// ...or ones with special words
						string trimmed = line.Trim();
						if( trimmed.StartsWith( "return" ) ||
							trimmed.StartsWith( "if(" ) ||
							trimmed.StartsWith( "static" ) )
							continue;

						FindRequest foundReq = null;
						foreach( FindRequest req in PendingRequests )
						{
							if( line.Contains( req.Query ) == true )
							{
								// Probably found!

								req.FileName = fileName;
								req.LineNumber = n + 1;

								// Try to get the code - may be multiline
								req.DeclarationBlock = StripLine( line.Trim() );
								if( req.DeclarationBlock.IndexOf( ';' ) < 0 )
								{
									for( int m = 1; m < 10; m++ )
									{
										string nextLine = StripLine( lines[ n + m ].Trim() );
										req.DeclarationBlock = string.Format( "{0} {1}", req.DeclarationBlock, nextLine );
										if( nextLine.IndexOf( ';' ) >= 0 )
											break;
									}
								}

								// If the block still contains a ?, ignore it, cause it's bogus
								if( req.DeclarationBlock.IndexOf( '?' ) >= 0 )
									continue;

								// Walk up the lines until we find the top of the comment/etc
								if( lines[ n - 1 ].Contains( "*/" ) == true )
								{
									req.CommentBlock = "";
									for( int m = 1; m < 20; m++ )
									{
										if( n - m < 0 )
											break;
										string cline = lines[ n - m ];

										req.CommentBlock = cline + Environment.NewLine + req.CommentBlock;

										if( cline.Contains( "/*" ) == true )
											break;
									}

									req.CommentBlock = req.CommentBlock.Trim();
								}

								// Found
								req.Found = true;
								req.Function.Source = req;

								found++;
								foundReq = req;
								break;
							}
						}

						if( foundReq != null )
						{
							PendingRequests.Remove( foundReq );
							FoundRequests.Add( foundReq );
						}
					}

					if( found > 0 )
						Console.WriteLine( "{0} found", found );
					else
						Console.WriteLine();
				}
			}
		}
	}
}
