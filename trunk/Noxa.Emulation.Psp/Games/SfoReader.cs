using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace Noxa.Emulation.Psp.Games
{
	class SfoReader
	{
		protected Dictionary<string, SfoEntry> _entries = new Dictionary<string, SfoEntry>();

		public enum SfoDataType
		{
			Binary = 0,
			String = 2,
			Integer = 4
		}

		public class SfoEntry
		{
			public string Label;
			public SfoDataType DataType;
			public object Data;
		}

		public const string MemoryStickSaveGameCategory = "MS";
		public const string MemoryStickGameCategory = "MG";
		public const string UmdGameCategory = "UG";

		public SfoReader( Stream stream )
		{
			BinaryReader reader = new BinaryReader( stream );
			if( Parse( reader ) == false )
			{
				// Uh ohes
				Debug.WriteLine( "SfoReader could not parse SFO" );
			}
		}

		private struct SfoHeader
		{
			public byte[] Magic;
			public uint PsfVersion;
			public uint LabelOffset;
			public uint DataOffset;
			public uint SectionCount;

			public SfoHeader( BinaryReader reader )
			{
				Magic = reader.ReadBytes( 4 );
				PsfVersion = reader.ReadUInt32();
				LabelOffset = reader.ReadUInt32();
				DataOffset = reader.ReadUInt32();
				SectionCount = reader.ReadUInt32();
			}
		}

		private struct SfoSection
		{
			public ushort LabelOffset;
			public byte DataAlignment;
			public SfoDataType DataType;
			public uint DataFieldUsed;
			public uint DataFieldSize;
			public uint DataOffset;

			public SfoSection( BinaryReader reader )
			{
				LabelOffset = reader.ReadUInt16();
				DataAlignment = reader.ReadByte();
				DataType = ( SfoDataType )reader.ReadByte();
				DataFieldUsed = reader.ReadUInt32();
				DataFieldSize = reader.ReadUInt32();
				DataOffset = reader.ReadUInt32();
			}
		}

		private bool Parse( BinaryReader reader )
		{
			SfoHeader header = new SfoHeader( reader );
			if( ( header.Magic[ 0 ] != 0 ) ||
				( header.Magic[ 1 ] != 'P' ) ||
				( header.Magic[ 2 ] != 'S' ) ||
				( header.Magic[ 3 ] != 'F' ) )
			{
				// Bad
				return false;
			}

			List<SfoSection> sections = new List<SfoSection>( ( int )header.SectionCount );
			for( int n = 0; n < header.SectionCount; n++ )
			{
				SfoSection section = new SfoSection( reader );
				sections.Add( section );
			}

			foreach( SfoSection section in sections )
			{
				SfoEntry entry = new SfoEntry();
				entry.DataType = section.DataType;
				
				// Label
				reader.BaseStream.Seek( header.LabelOffset + section.LabelOffset, SeekOrigin.Begin );
				StringBuilder label = new StringBuilder( 256 );
				for( int n = 0; n < 512; n++ )
				{
					char c = reader.ReadChar();
					if( c == 0 )
						break;
					label.Append( c );
				}
				entry.Label = label.ToString();

				// Data
				switch( entry.DataType )
				{
					case SfoDataType.Binary:
						// Don't decode
						break;
					case SfoDataType.Integer:
						reader.BaseStream.Seek( header.DataOffset + section.DataOffset, SeekOrigin.Begin );
						entry.Data = reader.ReadInt32();
						break;
					case SfoDataType.String:
						reader.BaseStream.Seek( header.DataOffset + section.DataOffset, SeekOrigin.Begin );
						byte[] bytes = reader.ReadBytes( ( int )section.DataFieldUsed );
						entry.Data = Encoding.UTF8.GetString( bytes ).Trim().Replace( "\0", "" );
						break;
				}

				_entries.Add( entry.Label, entry );
			}

			return true;
		}

		public SfoEntry this[ string label ]
		{
			get
			{
				if( _entries.ContainsKey( label ) == true )
					return _entries[ label ];
				else
					return null;
			}
		}
	}
}
