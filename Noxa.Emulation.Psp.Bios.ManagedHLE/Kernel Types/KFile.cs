// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using Noxa.Emulation.Psp.Media;
using System.IO;

namespace Noxa.Emulation.Psp.Bios.ManagedHLE
{
	enum KSpecialFileHandle
	{
		StdIn = 0,
		StdOut = 1,
		StdErr = 2,
	}

	class KFile : KHandle
	{
		public Kernel Kernel;

		public KDevice Device;
		public IMediaItem Item;

		public int FolderOffset;
		public Stream Stream;

		public bool IsOpen;
		public bool CanWrite;
		public bool CanSeek;

		public KFile( Kernel kernel, KDevice device, IMediaItem item )
		{
			Kernel = kernel;
			Device = device;
			Item = item;

			FolderOffset = 0;

			IsOpen = true;
			CanWrite = !( ( item.Attributes & MediaItemAttributes.ReadOnly ) == MediaItemAttributes.ReadOnly );
			CanSeek = false;
		}

		public KFile( Kernel kernel, KDevice device, IMediaItem item, Stream stream )
		{
			Kernel = kernel;
			Device = device;
			Item = item;

			FolderOffset = 0;

			Stream = stream;

			IsOpen = true;
			CanWrite = stream.CanWrite;
			CanSeek = stream.CanSeek;
		}

		public KFile( Kernel kernel )
		{
			Kernel = kernel;
			IsOpen = true;
			CanWrite = true;
			CanSeek = false;
		}
	}

	class KStdFile : KFile
	{
		private string _name;
		private StringBuilder _sb;

		public KStdFile( Kernel kernel, KSpecialFileHandle specialFileHandle )
			: base( kernel )
		{
			UID = ( uint )specialFileHandle;
			switch( specialFileHandle )
			{
				case KSpecialFileHandle.StdIn:
					_name = "stdin";
					break;
				case KSpecialFileHandle.StdOut:
					_name = "stdout";
					break;
				case KSpecialFileHandle.StdErr:
					_name = "stderr";
					break;
			}

			_sb = new StringBuilder();
		}

		public void Write( uint address )
		{
			string s = Kernel.ReadString( address );

			_sb.Append( s );
			if( _sb[ _sb.Length - 1 ] == '\n' )
			{
				string line = _sb.ToString();
				Debug.WriteLine( string.Format( "{0}: {1}", _name, line ) );
				_sb = new StringBuilder();
			}
		}
	}
}
