// ----------------------------------------------------------------------------
// Shared Utility Library
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace Noxa
{
	public struct Range
	{
		public int Location;
		public int Length;

		public Range( int location, int length )
		{
			this.Location = location;
			this.Length = length;
		}

		public override string ToString()
		{
			return string.Format( "{0}-{1} ({2})", Location, Location + Length, Length );
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
