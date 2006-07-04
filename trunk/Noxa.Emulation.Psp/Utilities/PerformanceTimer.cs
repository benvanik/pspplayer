using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Threading;

namespace Noxa.Emulation.Psp.Utilities
{
	public class PerformanceTimer
	{
		#region Interop

		[DllImport( "Kernel32.dll", SetLastError = false )]
		private static extern bool QueryPerformanceCounter( out long lpPerformanceCount );

		[DllImport( "Kernel32.dll" )]
		private static extern bool QueryPerformanceFrequency( out long lpFrequency );

		#endregion

		private double _startTime = 0.0;
		private double _lastTime = -1.0;

		/// <summary>
		/// 1 / frequency of the timer.
		/// </summary>
		private double _rate = 0.0;

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

		public void Reset()
		{
			long temp = 0;
			if( QueryPerformanceCounter( out temp ) == false )
				throw new InvalidOperationException( "Error querying the frequency of the timer" );
			_startTime = ( double )temp;
		}

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
