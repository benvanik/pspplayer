// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#define FLUSHONWRITE

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Diagnostics;

namespace Noxa.Emulation.Psp.Cpu
{
	static class Tracer
	{
		private static FileStream _stream;
		private static StreamWriter _writer;

		public static void OpenFile( string fileName )
		{
			_stream = File.OpenWrite( fileName );
			_writer = new StreamWriter( _stream );
		}

		public static void CloseFile()
		{
			if( _writer != null )
				_writer.Dispose();
			_writer = null;
			_stream = null;
		}

		public static void WriteLine( string line )
		{
			Debug.Assert( _writer != null );

			_writer.WriteLine( line );
#if FLUSHONWRITE
			Flush();
#endif
		}

		public static void WriteLine( string format, params object[] arg )
		{
			Debug.Assert( _writer != null );

			_writer.WriteLine( format, arg );
#if FLUSHONWRITE
			Flush();
#endif
		}

		public static void Flush()
		{
			Debug.Assert( _writer != null );
			_writer.Flush();
		}
	}
}
