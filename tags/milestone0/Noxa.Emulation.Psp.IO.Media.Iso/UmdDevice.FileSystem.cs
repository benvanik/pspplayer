// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Globalization;

namespace Noxa.Emulation.Psp.IO.Media.Iso
{
	partial class UmdDevice
	{
		protected struct NativePrimaryDescriptor
		{
			public string SystemID;
			public string VolumeID;
			public int SectorCount;
			public int SectorSize;
			public int PathTableLength;
			public int PathTableStart;
			public int AltPathTableStart;
			public string VolumeName;
			public string Publisher;
			public string Application;
			public DateTime Created;
			public NativePathEntry Root;

			public static NativePrimaryDescriptor Invalid
			{
				get
				{
					return default( NativePrimaryDescriptor );
				}
			}

			public static NativePrimaryDescriptor? FromStream( BinaryReader reader )
			{
				NativePrimaryDescriptor npd = new NativePrimaryDescriptor();

				// Starts at logical block 16 (0x8000)
				reader.BaseStream.Seek( 16 * 2048, SeekOrigin.Begin );

				reader.BaseStream.Seek( 1, SeekOrigin.Current );
				string magic = ReadString( reader, 5 );
				if( magic != "CD001" )
					return null;

				reader.BaseStream.Seek( 2, SeekOrigin.Current );
				npd.SystemID = ReadString( reader, 32 );
				npd.VolumeID = ReadString( reader, 32 );

				reader.BaseStream.Seek( 8, SeekOrigin.Current );
				npd.SectorCount = reader.ReadInt32();

				reader.BaseStream.Seek( 4 + 32 + 4 + 4, SeekOrigin.Current );
				npd.SectorSize = reader.ReadInt16();

				reader.BaseStream.Seek( 2, SeekOrigin.Current );
				npd.PathTableLength = reader.ReadInt32();

				reader.BaseStream.Seek( 4, SeekOrigin.Current );
				npd.PathTableStart = reader.ReadInt32();
				npd.AltPathTableStart = reader.ReadInt32();

				reader.BaseStream.Seek( 8, SeekOrigin.Current );
				long x = reader.BaseStream.Position;
				NativePathEntry? npev = NativePathEntry.FromReader( reader );
				Debug.Assert( npev != null );
				npd.Root = npev.Value;
				long z = reader.BaseStream.Position - x;
				reader.BaseStream.Seek( 34 - z, SeekOrigin.Current );

				npd.VolumeName = ReadString( reader, 128 );
				npd.Publisher = ReadString( reader, 128 );
				reader.BaseStream.Seek( 128, SeekOrigin.Current );
				npd.Application = ReadString( reader, 128 );

				reader.BaseStream.Seek( 37 + 37 + 37, SeekOrigin.Current );
				string creationString = ReadString( reader, 17 );
				npd.Created = ParseDate( creationString );

				// Seek to the end
				reader.BaseStream.Seek( 17 * 2048, SeekOrigin.Begin );

				return npd;
			}
		}

		protected struct NativePathEntry
		{
			public bool IsRoot;
			public int FirstSector;
			public int ParentEntry;
			public string Name;

			public static NativePathEntry? FromReader( BinaryReader reader )
			{
				NativePathEntry npe = new NativePathEntry();

				byte nameLength = reader.ReadByte();

				byte extraSectors = reader.ReadByte();
				Debug.Assert( extraSectors == 0 );

				npe.FirstSector = reader.ReadInt32();
				npe.ParentEntry = reader.ReadInt16() - 1; // Fixup because we are 0 based

				if( reader.PeekChar() == 0 )
				{
					//reader.BaseStream.Seek( nameLength, SeekOrigin.Current );
					npe.IsRoot = true;
					reader.ReadByte();
				}
				else
					npe.Name = ReadString( reader, nameLength );

				if( nameLength % 2 == 1 )
					reader.BaseStream.Seek( 1, SeekOrigin.Current );

				return npe;
			}
		}

		[Flags]
		protected enum NativeEntryFlags : byte
		{
			Normal = 0x0,
			Hidden = 0x1,
			Directory = 0x2,
		}

		protected struct NativeEntry
		{
			public int FirstSector;
			public int Length;
			public DateTime Timestamp;
			public NativeEntryFlags Flags;
			public string Name;

			public static NativeEntry? FromReader( BinaryReader reader )
			{
				NativeEntry ne = new NativeEntry();

				byte recordLength = reader.ReadByte();
				if( recordLength == 0 )
					return null;

				byte extraSectors = reader.ReadByte();
				Debug.Assert( extraSectors == 0 );

				ne.FirstSector = reader.ReadInt32();

				reader.BaseStream.Seek( 4, SeekOrigin.Current );
				ne.Length = reader.ReadInt32();

				reader.BaseStream.Seek( 4, SeekOrigin.Current );
				ne.Timestamp = ReadDate( reader );

				ne.Flags = ( NativeEntryFlags )reader.ReadByte();

				byte fileUnitSize = reader.ReadByte();
				Debug.Assert( fileUnitSize == 0 );
				byte interleaveGapSize = reader.ReadByte();
				Debug.Assert( interleaveGapSize == 0 );

				reader.BaseStream.Seek( 4, SeekOrigin.Current );
				byte nameLength = reader.ReadByte();
				ne.Name = ReadString( reader, nameLength );
				
				int namePad = ( nameLength % 2 == 0 ) ? 1 : 0;
				reader.BaseStream.Seek( namePad, SeekOrigin.Current );

				int pad = recordLength - 33 - nameLength - namePad;
				reader.BaseStream.Seek( pad, SeekOrigin.Current );

				return ne;
			}
		}

		protected static DateTime ParseDate( string dateString )
		{
			// Offset from GMT in 15 minute intervals
			//sbyte gmt = ( sbyte )dateString[ 16 ];
			// Don't care.... really

			dateString = dateString.Substring( 0, 16 );
			DateTime dt = DateTime.ParseExact( dateString, "yyyyMMddHHmmssff", CultureInfo.InvariantCulture );
			
			return dt;
		}

		protected static DateTime ReadDate( BinaryReader reader )
		{
			byte year = reader.ReadByte();
			byte month = reader.ReadByte();
			byte day = reader.ReadByte();
			byte hour = reader.ReadByte();
			byte minute = reader.ReadByte();
			byte second = reader.ReadByte();
			byte gmt = reader.ReadByte();

			return new DateTime( 1900 + year, month, day, hour, minute, second );
		}

		protected static string ReadString( BinaryReader reader, int length )
		{
			byte[] buffer = reader.ReadBytes( length );
			return Encoding.ASCII.GetString( buffer ).Trim();
		}

		protected MediaFolder ParseIsoFileSystem( string path )
		{
			Debug.Assert( path != null );
			Debug.Assert( File.Exists( path ) == true );

			if( File.Exists( path ) == false )
				return null;

			using( FileStream stream = File.Open( path, FileMode.Open, FileAccess.Read, FileShare.Read ) )
			using( BinaryReader reader = new BinaryReader( stream ) )
			{
				NativePrimaryDescriptor? npdv = NativePrimaryDescriptor.FromStream( reader );
				Debug.Assert( npdv != null );
				if( npdv == null )
					return null;
				NativePrimaryDescriptor npd = npdv.Value;

				List<NativePathEntry> npes = new List<NativePathEntry>();
				stream.Seek( npd.PathTableStart * 2048, SeekOrigin.Begin );
				while( stream.Position < ( npd.PathTableStart * 2048 ) + npd.PathTableLength )
				{
					NativePathEntry? npev = NativePathEntry.FromReader( reader );
					if( npev == null )
						continue;

					npes.Add( npev.Value );
				}

				List<MediaFolder> folders = new List<MediaFolder>( npes.Count );
				for( int n = 0; n < npes.Count; n++ )
				{
					NativePathEntry npe = npes[ n ];

					MediaFolder current = null;
					MediaFolder parent = null;
					if( npe.IsRoot == false )
						parent = folders[ npe.ParentEntry ];

					int m = 0;
					stream.Seek( npe.FirstSector * 2048, SeekOrigin.Begin );
					while( true )
					{
						NativeEntry? nev = NativeEntry.FromReader( reader );
						if( nev == null )
							break;
						NativeEntry ne = nev.Value;

						if( m == 0 )
						{
							// '.' this dir
							MediaItemAttributes attributes = MediaItemAttributes.Normal;
							if( ( ne.Flags & NativeEntryFlags.Hidden ) == NativeEntryFlags.Hidden )
								attributes |= MediaItemAttributes.Hidden;
							current = new MediaFolder( this, parent, npe.Name, attributes, ne.Timestamp );
							folders.Add( current );
						}
						else if( m == 1 )
						{
							// '..' parent dir - ignored
						}
						else
						{
							// Child
							if( ( ne.Flags & NativeEntryFlags.Directory ) == NativeEntryFlags.Directory )
							{
								// Directories are handled when it is their turn to be added
							}
							else
							{
								MediaItemAttributes attributes = MediaItemAttributes.Normal;
								if( ( ne.Flags & NativeEntryFlags.Hidden ) == NativeEntryFlags.Hidden )
									attributes |= MediaItemAttributes.Hidden;
								MediaFile file = new MediaFile( this, current, ne.Name, attributes, ne.Timestamp, ne.FirstSector * 2048, ne.Length );
							}
						}
						m++;
					}
				}

				return folders[ 0 ];
			}
		}
	}
}
