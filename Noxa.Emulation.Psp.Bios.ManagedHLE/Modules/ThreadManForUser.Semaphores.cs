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
		[BiosFunction( 0xD6DA4BA1, "sceKernelCreateSema" )]
		// SDK location: /user/pspthreadman.h:515
		// SDK declaration: SceUID sceKernelCreateSema(const char *name, SceUInt attr, int initVal, int maxVal, SceKernelSemaOptParam *option);
		public int sceKernelCreateSema( int name, int attr, int initVal, int maxVal, int option )
		{
			string sname = _kernel.ReadString( ( uint )name );
			KSemaphore sema = new KSemaphore( _kernel, sname, ( uint )attr, initVal, maxVal );
			_kernel.AddHandle( sema );

			return ( int )sema.UID;
		}

		[Stateless]
		[BiosFunction( 0x28B6489C, "sceKernelDeleteSema" )]
		// SDK location: /user/pspthreadman.h:523
		// SDK declaration: int sceKernelDeleteSema(SceUID semaid);
		public int sceKernelDeleteSema( int semaid )
		{
			KSemaphore sema = _kernel.GetHandle<KSemaphore>( semaid );
			if( sema == null )
			{
				Log.WriteLine( Verbosity.Normal, Feature.Bios, "sceKernelDeleteSema: could not find semaphore with ID {0}", semaid );
				return -1;
			}

			// Wake waiting threads???
			_kernel.RemoveHandle( sema.UID );

			return 0;
		}

		[DontTrace]
		[BiosFunction( 0x3F53E640, "sceKernelSignalSema" )]
		// SDK location: /user/pspthreadman.h:539
		// SDK declaration: int sceKernelSignalSema(SceUID semaid, int signal);
		public int sceKernelSignalSema( int semaid, int signal )
		{
			KSemaphore sema = _kernel.GetHandleOrNull<KSemaphore>( semaid );
			if( sema == null )
			{
				Log.WriteLine( Verbosity.Normal, Feature.Bios, "sceKernelSignalSema: could not find semaphore with ID {0}", semaid );
				return -1;
			}

			sema.Signal( signal );

			return 0;
		}

		[DontTrace]
		[BiosFunction( 0x4E3A1105, "sceKernelWaitSema" )]
		// SDK location: /user/pspthreadman.h:555
		// SDK declaration: int sceKernelWaitSema(SceUID semaid, int signal, SceUInt *timeout);
		public int sceKernelWaitSema( int semaid, int signal, int timeout )
		{
			KSemaphore sema = _kernel.GetHandleOrNull<KSemaphore>( semaid );
			if( sema == null )
			{
				Log.WriteLine( Verbosity.Normal, Feature.Bios, "sceKernelWaitSema: could not find semaphore with ID {0}", semaid );
				return -1;
			}

			uint timeoutUs = 0;
			unsafe
			{
				if( timeout != 0 )
					timeoutUs = *( ( uint* )_memorySystem.Translate( ( uint )timeout ) );
			}

			sema.Wait( false, signal, timeoutUs );

			return 0;
		}

		[DontTrace]
		[BiosFunction( 0x6D212BAC, "sceKernelWaitSemaCB" )]
		// SDK location: /user/pspthreadman.h:571
		// SDK declaration: int sceKernelWaitSemaCB(SceUID semaid, int signal, SceUInt *timeout);
		public int sceKernelWaitSemaCB( int semaid, int signal, int timeout )
		{
			KSemaphore sema = _kernel.GetHandle<KSemaphore>( semaid );
			if( sema == null )
			{
				Log.WriteLine( Verbosity.Normal, Feature.Bios, "sceKernelWaitSemaCB: could not find semaphore with ID {0}", semaid );
				return -1;
			}

			uint timeoutUs = 0;
			unsafe
			{
				if( timeout != 0 )
					timeoutUs = *( ( uint* )_memorySystem.Translate( ( uint )timeout ) );
			}

			sema.Wait( true, signal, timeoutUs );

			return 0;
		}

		[Stateless]
		[BiosFunction( 0x58B1F937, "sceKernelPollSema" )]
		// SDK location: /user/pspthreadman.h:581
		// SDK declaration: int sceKernelPollSema(SceUID semaid, int signal);
		public int sceKernelPollSema( int semaid, int signal )
		{
			KSemaphore sema = _kernel.GetHandle<KSemaphore>( semaid );
			if( sema == null )
			{
				Log.WriteLine( Verbosity.Normal, Feature.Bios, "sceKernelPollSema: could not find semaphore with ID {0}", semaid );
				return -1;
			}

			// Is this right?
			if( sema.CurrentCount >= signal )
				return 0;
			else
				return unchecked( ( int )KThread.SCE_KERNEL_ERROR_WAIT_TIMEOUT );
		}

		[Stateless]
		[BiosFunction( 0xBC6FEBC5, "sceKernelReferSemaStatus" )]
		// SDK location: /user/pspthreadman.h:591
		// SDK declaration: int sceKernelReferSemaStatus(SceUID semaid, SceKernelSemaInfo *info);
		public unsafe int sceKernelReferSemaStatus( int semaid, int info )
		{
			KSemaphore sema = _kernel.GetHandle<KSemaphore>( semaid );
			if( sema == null )
			{
				Log.WriteLine( Verbosity.Normal, Feature.Bios, "sceKernelReferSemaStatus: could not find semaphore with ID {0}", semaid );
				return -1;
			}

			uint* p = ( uint* )_memorySystem.Translate( ( uint )info );
			Debug.Assert( *p == 56 );
			*( p++ ) = 56;
			_kernel.WriteString( ( uint )info + 4, sema.Name );
			p += 8;
			*( p++ ) = sema.Attributes;
			*( p++ ) = ( uint )sema.InitialCount;
			*( p++ ) = ( uint )sema.CurrentCount;
			*( p++ ) = ( uint )sema.MaximumCount;
			*( p++ ) = ( uint )sema.WaitingThreads.Count;

			return 0;
		}
	}
}
