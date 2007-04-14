// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Security;

namespace Noxa.Emulation.Psp.Utilities
{
	/// <summary>
	/// A high-performance timer.
	/// </summary>
	public class PerformanceTimer
	{
		#region Interop

		[SuppressUnmanagedCodeSecurity]
		[DllImport( "Kernel32.dll", SetLastError = false )]
		private static extern bool QueryPerformanceCounter( out long lpPerformanceCount );

		[SuppressUnmanagedCodeSecurity]
		[DllImport( "Kernel32.dll" )]
		private static extern bool QueryPerformanceFrequency( out long lpFrequency );

		#endregion

		private double _startTime = 0.0;
		private double _lastTime = -1.0;

		/// <summary>
		/// 1 / frequency of the timer.
		/// </summary>
		private double _rate = 0.0;

		/// <summary>
		/// Initializes a new <see cref="PerformanceTimer"/> instance.
		/// </summary>
		public PerformanceTimer()
		{
			long temp = 0;

			// Get timer frequency
			if( ( QueryPerformanceFrequency( out temp ) == false ) || ( temp == 0 ) )
				throw new InvalidOperationException( "Error querying the frequency of the timer" );

			// Do this now so that we can use mults instead of divs later on
			_rate = 1.0 / ( ( double )temp );

			// Start the timer
			this.Reset();
		}

		/// <summary>
		/// Reset the timer to 0.
		/// </summary>
		public void Reset()
		{
			long temp = 0;
			if( QueryPerformanceCounter( out temp ) == false )
				throw new InvalidOperationException( "Error querying the frequency of the timer" );
			_startTime = ( double )temp;
		}

		/// <summary>
		/// Get the amount of time elapsed since the timer was last reset.
		/// </summary>
		public double Elapsed
		{
			get
			{
				// Get current time
				long stop = 0;
				QueryPerformanceCounter( out stop );

				// Return difference
				double result = ( ( ( double )stop ) - _startTime ) * _rate;
				if( result <= _lastTime )
					result = _lastTime + 0.01;

				_lastTime = result;
				return result;
			}
		}
	}
}
