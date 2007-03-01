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
using System.Xml;

namespace GenerateStubsV2
{
	class NidList
	{
		public List<PrxFile> Files;

		protected NidList()
		{
		}

		public static NidList FromFile( string xmlFile )
		{
			if( File.Exists( xmlFile ) == false )
			{
				Console.WriteLine( "NidList: File not found: {0}", xmlFile );
				return null;
			}

			XmlDocument doc = new XmlDocument();
			try
			{
				doc.Load( xmlFile );
			}
			catch( Exception ex )
			{
				Console.WriteLine( "NidList: Could not load XML document {0}; possibly malformed.", xmlFile );
				Console.WriteLine( ex.ToString() );
				return null;
			}

			NidList list = new NidList();
			list.Files = new List<PrxFile>( 1024 );
			int fileCount = 0;
			int libraryCount = 0;
			int functionCount = 0;

			foreach( XmlElement fileElement in doc.SelectNodes( "/PSPLIBDOC/PRXFILES/PRXFILE" ) )
			{
				PrxFile file = new PrxFile();
				file.FileName = fileElement.SelectSingleNode( "PRX" ).InnerText;
				file.Name = fileElement.SelectSingleNode( "PRXNAME" ).InnerText;
				Debug.Assert( ( file.Name != null ) && ( file.Name.Length > 0 ) );

				file.Libraries = new List<PrxLibrary>( 10 );
				foreach( XmlElement libraryElement in fileElement.SelectNodes( "LIBRARIES/LIBRARY" ) )
				{
					PrxLibrary lib = new PrxLibrary();
					lib.File = file;

					lib.Name = libraryElement.SelectSingleNode( "NAME" ).InnerText;
					Debug.Assert( ( lib.Name != null ) && ( lib.Name.Length > 0 ) );
					lib.Flags = Convert.ToInt32( libraryElement.SelectSingleNode( "FLAGS" ).InnerText, 16 );

					lib.Functions = new List<PrxFunction>( 64 );
					foreach( XmlElement functionElement in libraryElement.SelectNodes( "FUNCTIONS/FUNCTION" ) )
					{
						PrxFunction func = new PrxFunction();
						func.Library = lib;

						func.Name = functionElement.SelectSingleNode( "NAME" ).InnerText;
						Debug.Assert( ( func.Name != null ) && ( func.Name.Length > 0 ) );
						func.NID = Convert.ToInt32( functionElement.SelectSingleNode( "NID" ).InnerText, 16 );

						functionCount++;
						lib.Functions.Add( func );
					}

					libraryCount++;
					file.Libraries.Add( lib );
				}

				fileCount++;
				list.Files.Add( file );
			}

			Console.WriteLine( "NidList: Loaded {0} functions in {1} libraries from {2} prxs", functionCount, libraryCount, fileCount );

			return list;
		}
	}

	class PrxFile
	{
		public string FileName;
		public string Name;
		public List<PrxLibrary> Libraries;
	}

	class PrxLibrary
	{
		public PrxFile File;

		public string Name;
		public int Flags;  // 0x40010000 = user, 0x00010000 & 0x00090000 = driver
		public List<PrxFunction> Functions;

		public PrxLibrary Merged;

		public const int UserFlag = 0x40000000;
	}

	class PrxFunction
	{
		public PrxLibrary Library;

		public int NID;
		public string Name;

		public FindRequest Source;
	}
}
