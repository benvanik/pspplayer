// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

using Noxa.Utilities;
using Noxa.Emulation.Psp;
using Noxa.Emulation.Psp.Bios;
using Noxa.Emulation.Psp.Cpu;

namespace Noxa.Emulation.Psp.Bios.ManagedHLE.Modules
{
	partial class ThreadManForUser
	{
		[Stateless]
		[BiosFunction( 0x55C20A00, "sceKernelCreateEventFlag" )]
		// SDK location: /user/pspthreadman.h:645
		// SDK declaration: SceUID sceKernelCreateEventFlag(const char *name, int attr, int bits, SceKernelEventFlagOptParam *opt);
		public int sceKernelCreateEventFlag( int name, int attr, int bits, int opt )
		{
			KEvent ev = new KEvent( _kernel,
				_kernel.ReadString( ( uint )name ),
				( KEventAttributes )attr,
				( uint )bits );
			_kernel.AddHandle( ev );

			// options unused
			Debug.Assert( opt == 0 );

			return ( int )ev.UID;
		}

		[Stateless]
		[BiosFunction( 0xEF9E4C70, "sceKernelDeleteEventFlag" )]
		// SDK location: /user/pspthreadman.h:709
		// SDK declaration: int sceKernelDeleteEventFlag(int evid);
		public int sceKernelDeleteEventFlag( int evid )
		{
			KEvent ev = _kernel.GetHandle<KEvent>( evid );
			if( ev == null )
				return -1;

			// Can't delete if someone is waiting... need to do something?
			Debug.Assert( ev.WaitingThreads.Count == 0 );
			if( ev.WaitingThreads.Count > 0 )
			{
				// Wake them?
			}

			_kernel.RemoveHandle( ev.UID );

			return 0;
		}

		[BiosFunction( 0x1FB15A32, "sceKernelSetEventFlag" )]
		// SDK location: /user/pspthreadman.h:655
		// SDK declaration: int sceKernelSetEventFlag(SceUID evid, u32 bits);
		public int sceKernelSetEventFlag( int evid, int bits )
		{
			KEvent ev = _kernel.GetHandle<KEvent>( evid );
			if( ev == null )
				return -1;

			ev.Value |= ( uint )bits;
			if( ev.Signal() == true )
			{
				// We woke something!
				_kernel.Schedule();
			}

			return 0;
		}

		[BiosFunction( 0x812346E4, "sceKernelClearEventFlag" )]
		// SDK location: /user/pspthreadman.h:665
		// SDK declaration: int sceKernelClearEventFlag(SceUID evid, u32 bits);
		public int sceKernelClearEventFlag( int evid, int bits )
		{
			KEvent ev = _kernel.GetHandle<KEvent>( evid );
			if( ev == null )
				return -1;

			ev.Value &= ( uint )bits;
			if( ev.Signal() == true )
			{
				// We woke something!
				_kernel.Schedule();
			}

			return 0;
		}

		[BiosFunction( 0x402FCF22, "sceKernelWaitEventFlag" )]
		// SDK location: /user/pspthreadman.h:688
		// SDK declaration: int sceKernelWaitEventFlag(int evid, u32 bits, u32 wait, u32 *outBits, SceUInt *timeout);
		public int sceKernelWaitEventFlag( int evid, int bits, int wait, int outBits, int timeout )
		{
			KEvent ev = _kernel.GetHandle<KEvent>( evid );
			if( ev == null )
				return -1;

			// TODO: event timeouts
			Debug.Assert( timeout == 0 );

			// We may already be set
			if( ev.Matches( ( uint )bits, ( KWaitType )wait ) == true )
			{
				if( outBits != 0 )
				{
					unsafe
					{
						uint* poutBits = ( uint* )_memorySystem.Translate( ( uint )outBits );
						*poutBits = ev.Value;
					}
				}

				if( ( ( KWaitType )wait & KWaitType.ClearAll ) == KWaitType.ClearAll )
					ev.Value = 0;
				else if( ( ( KWaitType )wait & KWaitType.ClearPattern ) == KWaitType.ClearPattern )
					ev.Value = ev.Value & ~( uint )bits;

				return 0;
			}
			else
			{
				// No match - wait us
				KThread thread = _kernel.ActiveThread;
				Debug.Assert( thread != null );
				thread.Wait( ev, ( KWaitType )wait, ( uint )bits, ( uint )outBits, ( uint )timeout, false );

				return 0;
			}
		}

		[BiosFunction( 0x328C546A, "sceKernelWaitEventFlagCB" )]
		// SDK location: /user/pspthreadman.h:700
		// SDK declaration: int sceKernelWaitEventFlagCB(int evid, u32 bits, u32 wait, u32 *outBits, SceUInt *timeout);
		public int sceKernelWaitEventFlagCB( int evid, int bits, int wait, int outBits, int timeout )
		{
			// SAME AS ABOVE!

			KEvent ev = _kernel.GetHandle<KEvent>( evid );
			if( ev == null )
				return -1;

			// TODO: event timeouts
			Debug.Assert( timeout == 0 );

			// We may already be set
			if( ev.Matches( ( uint )bits, ( KWaitType )wait ) == true )
			{
				if( outBits != 0 )
				{
					unsafe
					{
						uint* poutBits = ( uint* )_memorySystem.Translate( ( uint )outBits );
						*poutBits = ev.Value;
					}
				}

				if( ( ( KWaitType )wait & KWaitType.ClearAll ) == KWaitType.ClearAll )
					ev.Value = 0;
				else if( ( ( KWaitType )wait & KWaitType.ClearPattern ) == KWaitType.ClearPattern )
					ev.Value = ev.Value & ~( uint )bits;

				return 0;
			}
			else
			{
				// No match - wait us
				KThread thread = _kernel.ActiveThread;
				Debug.Assert( thread != null );
				thread.Wait( ev, ( KWaitType )wait, ( uint )bits, ( uint )outBits, ( uint )timeout, true );
				
				return 0;
			}
		}

		[Stateless]
		[BiosFunction( 0x30FD48F0, "sceKernelPollEventFlag" )]
		// SDK location: /user/pspthreadman.h:676
		// SDK declaration: int sceKernelPollEventFlag(int evid, u32 bits, u32 wait, u32 *outBits);
		public int sceKernelPollEventFlag( int evid, int bits, int wait, int outBits )
		{
			KEvent ev = _kernel.GetHandle<KEvent>( evid );
			if( ev == null )
				return -1;

			// We may already be set
			if( ev.Matches( ( uint )bits, ( KWaitType )wait ) == true )
			{
				if( outBits != 0 )
				{
					unsafe
					{
						uint* poutBits = ( uint* )_memorySystem.Translate( ( uint )outBits );
						*poutBits = ev.Value;
					}
				}

				if( ( ( KWaitType )wait & KWaitType.ClearAll ) == KWaitType.ClearAll )
					ev.Value = 0;
				else if( ( ( KWaitType )wait & KWaitType.ClearPattern ) == KWaitType.ClearPattern )
					ev.Value = ev.Value & ~( uint )bits;

				return 0;
			}

			// What is the proper return here when we haven't matched?
			return -1;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCD203292, "sceKernelCancelEventFlag" )]
		// manual add
		public int sceKernelCancelEventFlag( int evid ){ return Module.NotImplementedReturn; }

		[Stateless]
		[BiosFunction( 0xA66B0120, "sceKernelReferEventFlagStatus" )]
		// SDK location: /user/pspthreadman.h:719
		// SDK declaration: int sceKernelReferEventFlagStatus(SceUID event, SceKernelEventFlagInfo *status);
		public int sceKernelReferEventFlagStatus( int evid, int status )
		{
			// SceSize  size 
			// char		name [32] 
			// SceUInt  attr 
			// SceUInt  initPattern 
			// SceUInt  currentPattern 
			// int		numWaitThreads 

			KEvent ev = _kernel.GetHandle<KEvent>( evid );
			if( ev == null )
				return -1;

			unsafe
			{
				byte* p = ( byte* )_memorySystem.Translate( ( uint )status );

				int size = *( ( int* )p );
				if( size == 52 )
				{
					_kernel.WriteString( ( uint )( status + 4 ), ev.Name );
					*( ( uint* )( p + 36 ) ) = ( uint )ev.Attributes;
					*( ( uint* )( p + 40 ) ) = ev.InitialValue;
					*( ( uint* )( p + 44 ) ) = ev.Value;
					*( ( int* )( p + 48 ) ) = ev.WaitingThreads.Count;
					return 0;
				}
				else
				{
					Log.WriteLine( Verbosity.Critical, Feature.Bios, "sceKernelReferEventFlagStatus: app passed in SceKernelEventFlagInfo of size {0}; expected size 52", size );
					return -1;
				}
			}
		}
	}
}
