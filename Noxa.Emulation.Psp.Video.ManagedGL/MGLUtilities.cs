// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2008 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace Noxa.Emulation.Psp.Video.ManagedGL
{
	unsafe static class MGLUtilities
	{
		#region Math

		public static uint lg2( uint x )
		{
			switch( x )
			{
				case 1:
					return 0;
				case 2:
					return 1;
				case 4:
					return 2;
				case 8:
					return 3;
				case 16:
					return 4;
				case 32:
					return 5;
				case 64:
					return 6;
				case 128:
					return 7;
				case 256:
					return 8;
				case 512:
					return 9;
				case 1024:
					return 10;
				case 2048:
					return 11;
				case 4096:
					return 12;
				case 8192:
					return 13;
				case 16384:
					return 14;
				case 32768:
					return 15;
				case 65536:
					return 16;
				default:
					uint ret = 0xFFFFFFFF;
					do
					{
						ret++;
						x >>= 1;
					} while( x != 0 );
					return ret;
			}
		}

		#endregion

		#region ReadMatrix*

		private static float[] _scratchMatrix = new float[ 16 ];
		public static void ReadMatrix3x4( uint* p, float[] matrix )
		{
			for( int n = 0; n < 12; n++ )
			{
				uint argx = ( *( p++ ) & 0x00FFFFFF ) << 8;
				_scratchMatrix[ n ] = *( ( float* )( ( uint* )( &argx ) ) );
			}

			matrix[ 0 ] = _scratchMatrix[ 0 ];
			matrix[ 1 ] = _scratchMatrix[ 1 ];
			matrix[ 2 ] = _scratchMatrix[ 2 ];
			matrix[ 3 ] = 0.0f;
			matrix[ 4 ] = _scratchMatrix[ 3 ];
			matrix[ 5 ] = _scratchMatrix[ 4 ];
			matrix[ 6 ] = _scratchMatrix[ 5 ];
			matrix[ 7 ] = 0.0f;
			matrix[ 8 ] = _scratchMatrix[ 6 ];
			matrix[ 9 ] = _scratchMatrix[ 7 ];
			matrix[ 10 ] = _scratchMatrix[ 8 ];
			matrix[ 11 ] = 0.0f;
			matrix[ 12 ] = _scratchMatrix[ 9 ];
			matrix[ 13 ] = _scratchMatrix[ 10 ];
			matrix[ 14 ] = _scratchMatrix[ 11 ];
			matrix[ 15 ] = 1.0f;
		}

		#endregion
	}
}
