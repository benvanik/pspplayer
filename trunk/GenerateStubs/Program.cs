// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace GenerateStubs
{
	class Program
	{
		static void Main( string[] args )
		{
			List<string> functions = new List<string>();

			string outFile;
			if( args.Length == 1 )
				outFile = "out.txt";
			else
				outFile = args[ 1 ];

			using( StreamReader reader = File.OpenText( args[ 0 ] ) )
			{
				//5,0xe81caf8f,int,sceKernelCreateCallback,"const char *name,SceKernelCallbackFunction func,void *arg"
				
				// Column names
				// Must be #, NID, return, name, params
				reader.ReadLine();

				string line;
				while( ( line = reader.ReadLine() ) != null )
				{
					string[] cols = line.Split( new char[] { ',' }, 5 );
					string name = cols[ 3 ];

					string psBase = cols[ 4 ].Trim( '\"' );
					string[] ps;
					if( psBase.Length > 0 )
						ps = psBase.Split( ',' );
					else
						ps = new string[] { };

					string returns = cols[ 2 ].Trim();
					if( ( returns == "void" ) ||
						( returns.Length == 0 ) )
						returns = null;

					//[BiosStub( 0x0, "name", false, 4 )]
					string attribute = string.Format( "[BiosStub( {0}, \"{1}\", {2}, {3} )]",
						cols[ 1 ], name,
						( returns != null ) ? "true" : "false",
						ps.Length );

					//public int sceKernelExitDeleteThread( IMemory memory, int a0, int a1, int a2, int a3, int sp )
					StringBuilder function = new StringBuilder();
					function.AppendLine( attribute );
					function.AppendLine( "[BiosStubIncomplete]" );
					function.AppendFormat( "public int {0}( IMemory memory, int a0, int a1, int a2, int a3, int sp )", name );
					function.AppendLine();
					function.AppendLine( "{" );
					for( int n = 0; n < ps.Length; n++ )
					{
						if( n < 4 )
							function.AppendFormat( "	// a{0} = {1}", n, ps[ n ].Trim() );
						else
						{
							int offset = ( n - 4 ) * 4;
							function.AppendFormat( "	// sp[{0}] = {1}", offset, ps[ n ].Trim() );
						}
						function.AppendLine();
					}
					if( ps.Length > 4 )
					{
						for( int n = 4; n < ps.Length; n++ )
						{
							function.AppendFormat( "	int a{0} = memory.ReadWord( sp + {1} );",
								n, ( n - 4 ) * 4 );
							function.AppendLine();
						}
					}
					if( ps.Length > 0 )
						function.AppendLine( "	" );
					if( returns != null )
						function.AppendLine( "	// " + returns );
					function.AppendLine( "	return 0;" );
					function.AppendLine( "}" );
					functions.Add( function.ToString() );
				}
			}

			using( StreamWriter writer = new StreamWriter( outFile ) )
			{
				foreach( string function in functions )
					writer.WriteLine( function );
			}
		}
	}
}
