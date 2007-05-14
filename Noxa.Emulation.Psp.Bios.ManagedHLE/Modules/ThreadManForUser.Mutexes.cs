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
		[BiosFunction( 0xB7D098C6, "sceKernelCreateMutex" )]
		// totally guessed - attr and option may not be right, and there may be no name
		// pretty sure param 0 is a pointer though, it just points to \0
		public int sceKernelCreateMutex( int name, int attr, int option )
		{
			string sname = _kernel.ReadString( ( uint )name );
			KMutex mutex = new KMutex( _kernel, sname, ( uint )attr );
			_kernel.AddHandle( mutex );

			return ( int )mutex.UID;
		}

		[Stateless]
		[BiosFunction( 0xF8170FBE, "sceKernelDeleteMutex" )]
		// manual add
		public int sceKernelDeleteMutex( int mid )
		{
			KMutex mutex = _kernel.GetHandle<KMutex>( mid );
			if( mutex == null )
			{
				Log.WriteLine( Verbosity.Normal, Feature.Bios, "sceKernelDeleteMutex: could not find mutex with ID {0}", mid );
				return -1;
			}

			// Wake waiting threads???
			_kernel.RemoveHandle( mutex.UID );

			return 0;
		}

		[BiosFunction( 0xB011B11F, "sceKernelLockMutex" )]
		// not sure what the params here are
		public int sceKernelLockMutex( int mid, int unknown, int timeout )
		{
			KMutex mutex = _kernel.GetHandle<KMutex>( mid );
			if( mutex == null )
			{
				Log.WriteLine( Verbosity.Normal, Feature.Bios, "sceKernelLockMutex: could not find mutex with ID {0}", mid );
				return -1;
			}

			uint timeoutUs = 0;
			unsafe
			{
				if( timeout != 0 )
					timeoutUs = *( ( uint* )_memorySystem.Translate( ( uint )timeout ) );
			}

			mutex.Lock( timeoutUs );

			return 0;
		}

		[BiosFunction( 0x6B30100F, "sceKernelUnlockMutex" )]
		// manual add
		public int sceKernelUnlockMutex( int mid )
		{
			KMutex mutex = _kernel.GetHandle<KMutex>( mid );
			if( mutex == null )
			{
				Log.WriteLine( Verbosity.Normal, Feature.Bios, "sceKernelUnlockMutex: could not find mutex with ID {0}", mid );
				return -1;
			}

			mutex.Unlock();

			return 0;
		}

		[NotImplemented]
		[BiosFunction( 0x87D9223C, "sceKernelCancelMutex" )]
		// manual add
		public int sceKernelCancelMutex( int mid )
		{
			KMutex mutex = _kernel.GetHandle<KMutex>( mid );
			if( mutex == null )
			{
				Log.WriteLine( Verbosity.Normal, Feature.Bios, "sceKernelCancelMutex: could not find mutex with ID {0}", mid );
				return -1;
			}

			return 0;
		}
	}
}
