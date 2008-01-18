// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using Noxa.Emulation.Psp;
using Noxa.Emulation.Psp.Bios;
using Noxa.Emulation.Psp.Cpu;
using Noxa.Emulation.Psp.Games;

namespace Noxa.Emulation.Psp.Bios.ManagedHLE
{
	partial class Kernel
	{
		public bool Schedule()
		{
			while( this.SchedulableThreads.Count == 0 )
			{
				// No threads to run? Check for delayed
				KThread earliest = null;
				for( int n = 0; n < this.Threads.Count; n++ )
				{
					KThread thread = this.Threads[ n ];
					if( ( ( thread.State == KThreadState.Waiting ) || ( thread.State == KThreadState.WaitSuspended ) ) &&
						( thread.WaitingOn == KThreadWait.Delay ) )
					{
						if( ( earliest == null ) ||
							( ( thread.WaitTimestamp + thread.WaitTimeout ) > ( earliest.WaitTimestamp + earliest.WaitTimeout ) ) )
							earliest = thread;
					}
				}

				if( earliest != null )
				{
					// Wait on it
					while( earliest.State == KThreadState.Waiting )
					{
						// This happens A LOT - it'd be best if we just spun, but by sleeping we save some time
						System.Threading.Thread.Sleep( 0 );
					}
				}
				else
				{
					//Log.WriteLine( Verbosity.Verbose, Feature.Bios, "Schedule: ran out of threads to run" );
					return false;
				}
			}

			// Find next thread to run
			KThread nextThread = this.SchedulableThreads.Head;
			Debug.Assert( nextThread != null );

			// If it didn't change, don't do anything
			if( nextThread == ActiveThread )
				return false;

			// Switch
			Cpu.SwitchContext( nextThread.ContextID );

			ActiveThread = nextThread;

			return true;
		}

		public void Execute()
		{
			Debug.Assert( ActiveThread != null );

			// Execute active thread
			bool breakFlag;
			uint instructionsExecuted;
			Cpu.Execute( out breakFlag, out instructionsExecuted );
			if( breakFlag == false )
			{
				// Only if not broken by choice
				//Log.WriteLine( Verbosity.Verbose, Feature.Bios, "CPU returned to us after {0} instructions", instructionsExecuted );
			}
		}

		private KThread _oldThread;
		private KCallback _runningCallback;
		private bool _runningUserCall;

		public bool IssueCallback( KCallback callback, uint argument )
		{
			Debug.Assert( callback != null );
			Debug.Assert( _runningUserCall == false );
			Debug.Assert( _runningCallback == null );

			_oldThread = ActiveThread;
			ActiveThread = callback.Thread;

			_runningCallback = callback;

			// Format is arg1, arg2, commonAddress
			// arg1 = ?
			// arg2 is passed during notify and given to use in argument
			Cpu.MarshalCall( ActiveThread.ContextID, callback.Address, new uint[] { callback.NotifyCount, argument, callback.CommonAddress }, new MarshalCompleteDelegate( this.CallbackComplete ), 0 );

			return true;
		}

		private bool CallbackComplete( int tcsId, int state, int result )
		{
			Debug.Assert( _oldThread != null );
			Debug.Assert( _runningCallback != null );

			// Something ? return value something?

			ActiveThread = _oldThread;
			_oldThread = null;
			_runningCallback = null;

			return true;
		}

		public bool IssueUserCall( uint address, uint[] arguments )
		{
			Debug.Assert( _runningUserCall == false );
			Debug.Assert( _runningCallback == null );

			_runningUserCall = true;

			Cpu.MarshalCall( ActiveThread.ContextID, address, arguments, new MarshalCompleteDelegate( this.UserCallComplete ), ( int )address );

			return true;
		}

		private bool UserCallComplete( int tcsId, int state, int result )
		{
			Debug.Assert( _runningUserCall == true );

			uint address = ( uint )state;
			// Something ? return value?

			_runningUserCall = false;

			return true;
		}
	}
}
