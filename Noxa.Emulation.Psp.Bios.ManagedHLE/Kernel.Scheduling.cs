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
			this.HandleCompletedTimers();

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

				if( ( earliest != null ) && ( earliest.State == KThreadState.Waiting ) )
				{
					// Wait on it
					System.Threading.Thread.Sleep( 1 );
				}
				else
				{
					Log.WriteLine( Verbosity.Critical, Feature.Bios, "Kernel::Schedule: ran out of threads to run - we're dead!" );
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

			this.HandleCompletedTimers();

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
		private KThreadState _oldThreadState;

		public bool NotifyCallback( KCallback callback, uint argument )
		{
			Debug.Assert( callback != null );
			callback.NotifyCount++;
			callback.NotifyArguments = argument;

			KThread thread = callback.Thread;
			if( thread.NotifiedCallbacks.Find( callback ) == null )
				thread.NotifiedCallbacks.Enqueue( callback );
			//if( ( thread.State == KThreadState.Waiting ) &&
			//    ( thread.CanHandleCallbacks == true ) )
			//{
			//    this.CheckCallbacks( thread );
			//}

			return true;
		}

		public bool CheckCallbacks()
		{
			KThread thread = this.ActiveThread;
			return this.CheckCallbacks( thread );
		}

		public bool CheckCallbacks( KThread thread )
		{
			Debug.Assert( thread != null );

			if( thread.NotifiedCallbacks.Count == 0 )
				return false;

			Debug.Assert( _runningUserCall == false );
			Debug.Assert( _runningCallback == null );

			if( this.ActiveThread != thread )
			{
				Debug.Assert( ( thread.State == KThreadState.Waiting ) || ( thread.State == KThreadState.WaitSuspended ) );
				_oldThread = this.ActiveThread;
				this.ActiveThread = thread;
			}

			_oldThreadState = thread.State;

			Log.WriteLine( Verbosity.Verbose, Feature.Bios, "Kernel::CheckCallbacks: issuing callback {0} on thread {1} (thread was {2})", _runningCallback.UID.ToString( "X" ), this.ActiveThread.UID.ToString( "X" ), _oldThread.UID.ToString( "X" ) );

			KCallback callback = thread.NotifiedCallbacks.Dequeue();

			_runningCallback = callback;

			// Format is arg1, arg2, commonAddress
			// arg1 = passed during notify
			// arg2 is passed during notify and given to use in argument
			Cpu.MarshalCall( this.ActiveThread.ContextID, callback.Address, new uint[] { callback.NotifyCount, callback.NotifyArguments, callback.CommonAddress }, new MarshalCompleteDelegate( this.CallbackComplete ), 0 );

			return true;
		}

		private bool CallbackComplete( int tcsId, int state, int result )
		{
			Debug.Assert( _oldThread != null );
			Debug.Assert( _runningCallback != null );

			// Something ? return value something?
			if( result == 1 )
			{
				// Delete callback
				Log.WriteLine( Verbosity.Normal, Feature.Bios, "Kernel::CallbackComplete: callback {0} deleted by request", _runningCallback.UID.ToString( "X" ) );
				this.DeleteCallback( _runningCallback );
			}

			Log.WriteLine( Verbosity.Verbose, Feature.Bios, "Kernel::CallbackComplete: finished callback {0} on thread {1}; result was {2}", _runningCallback.UID.ToString( "X" ), this.ActiveThread.UID.ToString( "X" ), result.ToString( "X" ) );

			_runningCallback = null;

			Debug.Assert( this.ActiveThread.State == _oldThreadState );

			// See if there are more
			if( this.CheckCallbacks( this.ActiveThread ) == true )
				return true;

			if( _oldThread != null )
			{
				this.ActiveThread = _oldThread;
				_oldThread = null;
			}

			return true;
		}

		public void DeleteCallback( KCallback callback )
		{
			// Unset?? walk callback listings and remove?
			foreach( FastLinkedList<KCallback> list in this.Callbacks )
				list.Remove( callback );

			// Trickier - remove from pending notification threads
			foreach( KThread thread in this.Threads )
				thread.NotifiedCallbacks.Remove( callback );

			this.RemoveHandle( callback.UID );
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
