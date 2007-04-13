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
using Noxa.Emulation.Psp.Media;

namespace Noxa.Emulation.Psp.Games
{
	class PbpReader
	{
		protected Dictionary<PbpEntryType, PbpEntry> _entries = new Dictionary<PbpEntryType, PbpEntry>();

		public enum PbpEntryType
		{
			Param = 0,
			Icon0 = 1,
			Icon1 = 2,
			Pic0 = 3,
			Pic1 = 4,
			Snd0 = 5,
			DataPsp = 6,
			DataPsar = 7
		}

		protected class PbpEntry
		{
			public PbpEntryType EntryType;
			public uint Offset;
			public uint Length;
		}

		public const int PbpEntryCount = 8;

		public PbpReader( Stream stream )
		{
			BinaryReader reader = new BinaryReader( stream );
			if( Parse( reader ) == false )
			{
				// Uh ohes
				Debug.WriteLine( "PbpReader: could not parse PBP" );
			}
		}

		private struct PbpHeader
		{
			public byte[] Magic;
			public uint PbpVersion;
			public uint[] Offsets;

			public PbpHeader( BinaryReader reader )
			{
				this.Magic = reader.ReadBytes( 4 );
				this.PbpVersion = reader.ReadUInt32();
				this.Offsets = new uint[ PbpEntryCount ];
				for( int n = 0; n < PbpEntryCount; n++ )
					this.Offsets[ n ] = reader.ReadUInt32();
			}
		}

		private bool Parse( BinaryReader reader )
		{
			PbpHeader header = new PbpHeader( reader );
			if( ( header.Magic[ 0 ] != 0 ) ||
				( header.Magic[ 1 ] != 'P' ) ||
				( header.Magic[ 2 ] != 'B' ) ||
				( header.Magic[ 3 ] != 'P' ) )
			{
				// Bad
				return false;
			}

			//uint lastOffset = 0;
			for( int n = 0; n < PbpEntryCount; n++ )
			{
				uint offset = header.Offsets[ n ];

				uint length;
				if( n < PbpEntryCount - 1 )
				{
					length = header.Offsets[ n + 1 ] - offset;
				}
				else
					length = ( uint )reader.BaseStream.Length - offset;
				if( length == 0 )
					continue;
				
				PbpEntry entry = new PbpEntry();
				entry.EntryType = ( PbpEntryType )n;
				entry.Offset = offset;
				entry.Length = length;
				_entries.Add( entry.EntryType, entry );
			}

			return true;
		}

		public bool ContainsEntry( PbpEntryType entryType )
		{
			return _entries.ContainsKey( entryType );
		}

		public Stream Read( Stream stream, PbpEntryType entryType )
		{
			if( _entries.ContainsKey( entryType ) == false )
				return null;
			PbpEntry entry = _entries[ entryType ];

			stream.Seek( entry.Offset, SeekOrigin.Begin );
			byte[] buffer = new byte[ entry.Length ];
			stream.Read( buffer, 0, ( int )entry.Length );

			return new MemoryStream( buffer, 0, buffer.Length, false, false );
		}
	}
}
