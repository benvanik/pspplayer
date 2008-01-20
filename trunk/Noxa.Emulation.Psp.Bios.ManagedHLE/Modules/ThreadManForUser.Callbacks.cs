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
		[BiosFunction( 0xE81CAF8F, "sceKernelCreateCallback" )]
		// SDK location: /user/pspthreadman.h:985
		// SDK declaration: int sceKernelCreateCallback(const char *name, SceKernelCallbackFunction func, void *arg);
		public int sceKernelCreateCallback( int name, int func, int arg )
		{
			KThread thread = _kernel.ActiveThread;
			if( thread == null )
				return -1;

			KCallback cb = new KCallback( _kernel,
				_kernel.ReadString( ( uint )name ),
				thread,
				( uint )func,
				( uint )arg );
			_kernel.AddHandle( cb );

			return ( int )cb.UID;
		}

		[Stateless]
		[BiosFunction( 0xEDBA5844, "sceKernelDeleteCallback" )]
		// SDK location: /user/pspthreadman.h:1005
		// SDK declaration: int sceKernelDeleteCallback(SceUID cb);
		public int sceKernelDeleteCallback( int cbid )
		{
			KCallback cb = _kernel.GetHandle<KCallback>( cbid );
			if( cb == null )
				return -1;

			// Unset?? walk callback listings and remove?
			foreach( FastLinkedList<KCallback> list in _kernel.Callbacks )
				list.Remove( cb );

			_kernel.RemoveHandle( cb.UID );

			return 0;
		}

		[BiosFunction( 0xC11BA8C4, "sceKernelNotifyCallback" )]
		// SDK location: /user/pspthreadman.h:1015
		// SDK declaration: int sceKernelNotifyCallback(SceUID cb, int arg2);
		public int sceKernelNotifyCallback( int cbid, int arg2 )
		{
			KCallback cb = _kernel.GetHandle<KCallback>( cbid );
			if( cb == null )
				return -1;

			_kernel.NotifyCallback( cb, ( uint )arg2 );

			// Real return set by marshaller
			return 0;
		}

		[Stateless]
		[BiosFunction( 0xBA4051D6, "sceKernelCancelCallback" )]
		// SDK location: /user/pspthreadman.h:1024
		// SDK declaration: int sceKernelCancelCallback(SceUID cb);
		public int sceKernelCancelCallback( int cbid )
		{
			// We don't allow cancels - screw you!
			return 0;
		}

		[Stateless]
		[BiosFunction( 0x2A3D44FF, "sceKernelGetCallbackCount" )]
		// SDK location: /user/pspthreadman.h:1033
		// SDK declaration: int sceKernelGetCallbackCount(SceUID cb);
		public int sceKernelGetCallbackCount( int cbid )
		{
			KCallback cb = _kernel.GetHandle<KCallback>( cbid );
			if( cb == null )
				return -1;
			return ( int )cb.NotifyCount;
		}

		[Stateless]
		[BiosFunction( 0x349D6D6C, "sceKernelCheckCallback" )]
		// SDK location: /user/pspthreadman.h:1040
		// SDK declaration: int sceKernelCheckCallback();
		public int sceKernelCheckCallback()
		{
			// No clue what this does - related to checkthreadstack?
			if( _kernel.CheckCallbacks() == true )
				return 1;
			else
				return 0;
		}

		[Stateless]
		[BiosFunction( 0x730ED8BC, "sceKernelReferCallbackStatus" )]
		// SDK location: /user/pspthreadman.h:996
		// SDK declaration: int sceKernelReferCallbackStatus(SceUID cb, SceKernelCallbackInfo *status);
		public int sceKernelReferCallbackStatus( int cbid, int status )
		{
			KCallback cb = _kernel.GetHandle<KCallback>( cbid );
			if( cb == null )
				return -1;

			unsafe
			{
				uint* pstatus = ( uint* )_memorySystem.Translate( ( uint )status );
				if( *pstatus == 56 )
				{
					_kernel.WriteString( ( uint )status + 4, cb.Name );
					// Skip over name and size field
					pstatus += 9;
					*pstatus = cb.Thread.UID;
					*( pstatus + 1 ) = cb.Address;
					*( pstatus + 2 ) = cb.CommonAddress;
					*( pstatus + 3 ) = cb.NotifyCount;
					*( pstatus + 4 ) = cb.NotifyArguments;
					return 0;
				}
				else
				{
					Debug.WriteLine( string.Format( "sceKernelReferCallbackStatus: expected SceKernelCallbackInfo size of 56, got {0}", *pstatus ) );
					return -1;
				}
			}
		}
	}
}
