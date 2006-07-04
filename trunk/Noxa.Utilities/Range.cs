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
