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
		public IMediaItem FindPath( string path )
		{
			int colonPos = path.IndexOf( ':' );
			if( colonPos >= 0 )
			{
				// Absolute path
				KDevice kdevice = this.DeviceLookup[ path.Substring( 0, colonPos ) ];
				IMediaDevice device = kdevice.Device;
				if( device.State == MediaState.Present )
				{
					path = path.Substring( colonPos + 1 );
					return device.Root.Find( path );
				}
				else
				{
					Debug.WriteLine( string.Format( "Kernel::FindPath: device {0} is ejected, cannot access {1}", device.Description, path ) );
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
