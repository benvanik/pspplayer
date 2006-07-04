using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Xml;

namespace Noxa.Emulation.Psp
{
	public class ComponentParameters : Settings
	{
		public ComponentParameters()
		{
		}

		public ComponentParameters( string source, int depth, bool b )
			: base( source, depth, b )
		{
		}

		public ComponentParameters( ComponentParameters source )
			: base()
		{
			lock( source._syncRoot )
			{
				foreach( KeyValuePair<string, object> pair in source._settings )
				{
					ICloneable cloneable = pair.Value as ICloneable;
					if( cloneable != null )
						_settings.Add( pair.Key, cloneable.Clone() );
					else
						_settings.Add( pair.Key, pair.Value );
				}
			}
		}

		public ComponentParameters Clone()
		{
			return new ComponentParameters( this );
		}

		public void Merge( ComponentParameters source )
		{
		}
	}
}
