// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Noxa.Emulation.Psp.Debugging.Statistics
{
	/// <summary>
	/// Provides a data provider for <see cref="Counter"/> instances.
	/// </summary>
	public abstract class CounterSource : MarshalByRefObject
	{
		/// <summary>
		/// A list of <see cref="Counter"/> instances registered with this source.
		/// </summary>
		protected List<Counter> _counters;

		/// <summary>
		/// The name of this instance.
		/// </summary>
		public readonly string Name;

		/// <summary>
		/// Initializes a new <see cref="CounterSource"/> instance with the given parameters.
		/// </summary>
		/// <param name="name">The name of the source.</param>
		public CounterSource( string name )
		{
			_counters = new List<Counter>();
			this.Name = name;
		}

		/// <summary>
		/// Register a counter.
		/// </summary>
		/// <param name="counter">The <see cref="Counter"/> instance to register.</param>
		protected void RegisterCounter( Counter counter )
		{
			Debug.Assert( counter != null );
			_counters.Add( counter );
		}

		/// <summary>
		/// Get the counters associated with this instance.
		/// </summary>
		/// <returns>A list of <see cref="Counter"/> instances associated to this source.</returns>
		public Counter[] GetCounters()
		{
			return _counters.ToArray();
		}

		/// <summary>
		/// Sample the counters associated with this source.
		/// </summary>
		public abstract void Sample();
	}
}
