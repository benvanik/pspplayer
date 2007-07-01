// ----------------------------------------------------------------------------
// Shared Utility Library
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Noxa
{
	public struct Range
	{
		public readonly int Offset;
		public readonly int Size;
		public readonly int Extents;
		
		public Range( int offset, int size )
		{
			Debug.Assert( size >= 0 );
			this.Offset = offset;
			this.Size = size;
			this.Extents = offset + size;
		}
		
		public bool Contains( int index )
		{
			return ( index >= Offset ) && ( index < Extents );
		}
		
		public override string ToString()
		{
			return string.Format( "{0}-{1} ({2})", this.Offset, this.Extents, this.Size );
		}

		public static Range Empty
		{
			get
			{
				return new Range( 0, 0 );
			}
		}
	}
}
