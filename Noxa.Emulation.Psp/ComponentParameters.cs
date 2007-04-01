// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Xml;

namespace Noxa.Emulation.Psp
{
	/// <summary>
	/// <see cref="IComponentInstance"/> configuration.
	/// </summary>
	public class ComponentParameters : Settings
	{
		/// <summary>
		/// Initializes a new <see cref="ComponentParameters"/> instance.
		/// </summary>
		public ComponentParameters()
		{
		}

		/// <summary>
		/// Initializes a new <see cref="ComponentParameters"/> instance.
		/// </summary>
		/// <param name="source">Internal.</param>
		/// <param name="depth">Internal.</param>
		/// <param name="b">Internal.</param>
		public ComponentParameters( string source, int depth, bool b )
			: base( source, depth, b )
		{
		}

		/// <summary>
		/// Clones a new <see cref="ComponentParameters"/> instance from the given <see cref="ComponentParameters"/> instance.
		/// </summary>
		/// <param name="source">Source instance to clone.</param>
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

		/// <summary>
		/// Clones the <see cref="ComponentParameters"/> instance.
		/// </summary>
		/// <returns>A clone of the current <see cref="ComponentParameters"/> instance.</returns>
		public ComponentParameters Clone()
		{
			return new ComponentParameters( this );
		}
	}
}
