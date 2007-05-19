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
	/// Provides a general <see cref="Counter"/> data sink.
	/// </summary>
	public class CounterSink : MarshalByRefObject
	{
		/// <summary>
		/// A list of <see cref="CounterSource"/> instances registered with this sink.
		/// </summary>
		protected List<CounterSource> _sources;

		/// <summary>
		/// Initializes a new <see cref="CounterSink"/> instance.
		/// </summary>
		public CounterSink()
		{
			_sources = new List<CounterSource>();
		}

		/// <summary>
		/// Register a source.
		/// </summary>
		/// <param name="source">The <see cref="CounterSource"/> instance to register.</param>
		public void RegisterSource( CounterSource source )
		{
			Debug.Assert( source != null );
			_sources.Add( source );
		}

		/// <summary>
		/// Get the sources associated with this instance.
		/// </summary>
		/// <returns>A list of <see cref="CounterSource"/> instances associated to this sink.</returns>
		public CounterSource[] GetSources()
		{
			return _sources.ToArray();
		}

		/// <summary>
		/// Sample all of the <see cref="CounterSource"/> instances associated with this sink.
		/// </summary>
		public void Sample()
		{
			foreach( CounterSource source in _sources )
				source.Sample();
		}
	}
}
