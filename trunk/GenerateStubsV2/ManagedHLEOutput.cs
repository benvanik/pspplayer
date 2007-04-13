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
	class ManagedHLEOutput : IOutput
	{
		public const string FileExt = ".cs";
		public const string Int64Name = "long";

		private List<string> _searchPaths;
		private string _outputPath;
		private string _tempFile;
		private StreamWriter _writer;
		private CRC32 _crc;
		private PrxLibrary _library;

		private string _header;
		private string _footer;
		private string _indent;

		private const string VersionLine = "GenerateStubsV2: auto-generated";

		public const string FileHeader = "FileHeaderCS.txt";
		public const string FileFooter = "FileFooterCS.txt";
		public const int IndentLevel = 2;

		private List<string> _int64types;

		public ManagedHLEOutput( List<string> searchPaths, string outputPath )
		{
			_searchPaths = searchPaths;
			_outputPath = outputPath;

			_crc = new CRC32();

			_header = File.ReadAllText( FileHeader );
			_footer = File.ReadAllText( FileFooter );

			_indent = new string( '\t', IndentLevel );

			_int64types = new List<string>();
			_int64types.AddRange( new string[]{
			    "SceOff", "SceInt64", "unsigned long long", "long long",
			} );
		}

		private bool CompareVersion( string fileName, string myHash )
		{
			using( StreamReader reader = File.OpenText( fileName ) )
			{
				while( reader.EndOfStream == false )
				{
					string line = reader.ReadLine();
					if( line.Contains( VersionLine ) == true )
						return line.Contains( myHash );
				}
			}
			return false;
		}

		public string GenerateFileName( PrxLibrary library )
		{
			string fileName = library.Name;
			char[] badChars = Path.GetInvalidFileNameChars();
			for( int n = 0; n < badChars.Length; n++ )
			{
				if( fileName.IndexOf( badChars[ n ] ) >= 0 )
				{
					Debugger.Break();
				}
			}

			fileName += FileExt;
			return Path.Combine( _outputPath, fileName );
		}

		public void BeginLibrary( PrxLibrary library )
		{
			_tempFile = Path.GetTempFileName();
			_writer = new StreamWriter( _tempFile );

			_library = library;

			Regex nameRegex = new Regex( "{MODULENAME}" );
			string myHeader = nameRegex.Replace( _header, library.Name );

			_writer.Write( myHeader );

			// ----- Write header
		}

		public void EndLibrary()
		{
			// ----- Write footer

			_writer.Write( _footer );

			_writer.Close();

			uint crc;
			using( FileStream stream = File.OpenRead( _tempFile ) )
				crc = _crc.GetCrc32( stream );
			string crcString = string.Format( "{0:X8}", crc );

			string version = string.Format( "/* {0} - {1} */", VersionLine, crcString );

			// Go back in and write the version at the very end
			using( StreamWriter stream = new StreamWriter( _tempFile, true ) )
			{
				stream.WriteLine();
				stream.WriteLine( version );
			}

			string fileName = GenerateFileName( _library );
			if( File.Exists( fileName ) == true )
			{
				bool versionsMatch = CompareVersion( fileName, crcString );
				if( versionsMatch == true )
				{
					Console.WriteLine( " > Library {0} in existing file {1} is up to date; skipping", _library.Name, Path.GetFileName( fileName ) );
				}
				else
				{
					string newFileName = Path.Combine( _outputPath, string.Format( "{0}-{1}.h", Path.GetFileNameWithoutExtension( fileName ), crcString ) );
					File.Move( _tempFile, newFileName );
					Console.WriteLine( " > WARNING: file exists, library {0} written to {1} instead - but diffs exist!", _library.Name, Path.GetFileName( newFileName ) );
				}
			}
			else
			{
				File.Move( _tempFile, fileName );
				Console.WriteLine( " > Library {0} written to new file {1}", _library.Name, Path.GetFileName( fileName ) );
			}
		}

		public string TransformDeclaration( string functionName, string input, out bool hasReturn )
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

		public void WriteFunction( PrxFunction function )
		{
			// /* Comment string */
			// // source
			// // original decl
			// [attrs]
			// transformed decl

			//if( function.Source.CommentBlock != null )
			//	_writer.WriteLine( "{0}{1}", _indent, function.Source.CommentBlock );

			// We trim out the nasty source file path
			string sourceFile = function.Source.FileName;
			foreach( string path in _searchPaths )
			{
				if( sourceFile.StartsWith( path ) == true )
				{
					sourceFile = sourceFile.Replace( path, "" );
					break;
				}
			}
			sourceFile = sourceFile.Replace( '\\', '/' );

			// Attributes
			_writer.WriteLine( "{0}[NotImplemented]", _indent );
			_writer.WriteLine( "{0}[Stateless]", _indent );
			_writer.WriteLine( "{0}[BiosFunction( 0x{1:X8}, \"{2}\" )]", _indent, function.NID, function.Name );

			// Original decl & source
			_writer.WriteLine( "{0}// SDK location: {1}:{2}", _indent, sourceFile, function.Source.LineNumber );
			_writer.WriteLine( "{0}// SDK declaration: {1}", _indent, function.Source.DeclarationBlock );

			bool hasReturn;
			string transformed = TransformDeclaration( function.Name, function.Source.DeclarationBlock, out hasReturn );

			// Function body
			_writer.Write( "{0}{1}", _indent, transformed );
			if( hasReturn == true )
				_writer.WriteLine( "{ return Module.NotImplementedReturn; }" );
			else
				_writer.WriteLine( "{}" );

			_writer.WriteLine();
		}
	}
}
