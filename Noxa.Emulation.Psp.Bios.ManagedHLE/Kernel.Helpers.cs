// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using Noxa.Emulation.Psp;
using Noxa.Emulation.Psp.Bios;
using Noxa.Emulation.Psp.Cpu;
using Noxa.Emulation.Psp.Media;

namespace Noxa.Emulation.Psp.Bios.ManagedHLE
{
	partial class Kernel
	{
		public KDevice FindDevice( string path )
		{
			int colonPos = path.IndexOf( ':' );
			if( colonPos >= 0 )
				path = path.Substring( 0, colonPos );
			KDevice device;
			if( DeviceLookup.TryGetValue( path, out device ) == true )
				return device;
			else
			{
				Debug.Assert( false, string.Format( "Unable to find a device matching the alias '{0}'", path ) );
				return null;
			}
		}

		public KDevice FindDevice( IMediaDevice device )
		{
            foreach (KDevice Device in Devices)
                if (Device == device)
                       return Device;
			return null;
		}

		public IMediaItem FindPath( string path )
		{
			// Hack for ToE and maybe others
			path = path.Replace( "host0:UMD", "umd0:" );

			int colonPos = path.IndexOf( ':' );
			if( colonPos >= 0 )
			{
				// Absolute path
				string deviceName = path.Substring( 0, colonPos );
				KDevice kdevice;
				if( this.DeviceLookup.TryGetValue( deviceName, out kdevice ) == false )
				{
					Log.WriteLine( Verbosity.Critical, Feature.Bios, "FindPath({0}): unable to find device {1}", path, deviceName );
					return null;
				}
				IMediaDevice device = kdevice.Device;
				if( device.State == MediaState.Present )
				{
					path = path.Substring( colonPos + 1 );
					if( path.StartsWith( "sce_lbn" ) == true )
					{
						Debug.Assert( device is IUmdDevice );
						IUmdDevice umd = ( IUmdDevice )device;

						// Lookup LBN/etc
						//0x0_size0xbb141
						int sep = path.LastIndexOf( '_' );
						string slbn = path.Substring( 7, path.Length - sep - 7 );
						string ssize = path.Substring( sep + 4 );
						long lbn = long.Parse( slbn );
						long size = long.Parse( ssize );
						return umd.Lookup( lbn, size );
					}
					else
						return device.Root.Find( path );
				}
				else
				{
					Log.WriteLine( Verbosity.Normal, Feature.Bios, "FindPath: device {0} is ejected, cannot access {1}", device.Description, path );
					return null;
				}
			}
			else
			{
				// Relative path
				Debug.Assert( this.CurrentPath != null );
				return this.CurrentPath.Find( path );
			}
		}

		public unsafe string ReadString( uint address )
		{
			byte* p = MemorySystem.Translate( address );
			return new string( ( sbyte* )p );
		}

		public unsafe string ReadString( byte* address, Encoding encoding )
		{
			// Nasty, but it works
			byte[] buffer = new byte[ 512 ];
			byte* p = address;
			bool done = false;
			for( int n = 0; n < 512; n++ )
			{
				byte c = *( p++ );
				buffer[ n ] = c;
				if( c == 0 )
				{
					done = true;
					break;
				}
			}
			if( done == false )
			{
				// Hit end of buffer before finishing
				Debug.Assert( false, "Make buffer larger" );
			}
			return encoding.GetString( buffer );
		}

		public unsafe int WriteString( uint address, string value )
		{
			byte* p = MemorySystem.Translate( address );
			byte* sp = p;
			for( int n = 0; n < value.Length; n++ )
			{
				char c = value[ n ];
				*sp = ( byte )c;
				sp++;
			}
			*sp = 0;
			return value.Length + 1;
		}

		public unsafe DateTime ReadTime( uint address )
		{
			ushort* p = ( ushort* )MemorySystem.Translate( address );

			//unsigned short	year 
			//unsigned short	month 
			//unsigned short	day 
			//unsigned short	hour 
			//unsigned short	minute 
			//unsigned short	second 
			//unsigned int		microsecond
			return new DateTime(
				*p,
				*( p + 1 ),
				*( p + 2 ),
				*( p + 3 ),
				*( p + 4 ),
				*( p + 5 ),
				*( ( int* )( p + 6 ) ) / 1000 );
		}

		public unsafe int WriteTime( uint address, DateTime value )
		{
			ushort* p = ( ushort* )MemorySystem.Translate( address );

			*p = ( ushort )value.Year;
			*( p + 1 ) = ( ushort )value.Month;
			*( p + 2 ) = ( ushort )value.Day;
			*( p + 3 ) = ( ushort )value.Hour;
			*( p + 4 ) = ( ushort )value.Minute;
			*( p + 5 ) = ( ushort )value.Second;
			*( ( int* )( p + 6 ) ) = value.Millisecond * 1000;

			return 16;
		}
	}
}
