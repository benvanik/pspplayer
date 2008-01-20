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
		#region VPL

		[Stateless]
		[BiosFunction( 0x56C039B5, "sceKernelCreateVpl" )]
		// SDK location: /user/pspthreadman.h:1256
		// SDK declaration: SceUID sceKernelCreateVpl(const char *name, int part, int attr, unsigned int size, struct SceKernelVplOptParam *opt);
		public int sceKernelCreateVpl( int name, int part, int attr, int size, int opt )
		{
			KPartition partition = _kernel.Partitions[ part ];
			Debug.Assert( partition != null );
			if( partition == null )
				return -1;

			KVariablePool pool = new KVariablePool( _kernel, partition,
				_kernel.ReadString( ( uint )name ),
				( uint )attr, ( uint )size );
			_kernel.AddHandle( pool );

			// option unused?
			Debug.Assert( opt == 0 );

			return ( int )pool.UID;
		}

		[Stateless]
		[BiosFunction( 0x89B3D48C, "sceKernelDeleteVpl" )]
		// SDK location: /user/pspthreadman.h:1265
		// SDK declaration: int sceKernelDeleteVpl(SceUID uid);
		public int sceKernelDeleteVpl( int uid )
		{
			KVariablePool pool = _kernel.GetHandle<KVariablePool>( uid );
			if( pool == null )
				return -1;

			pool.Dispose();

			_kernel.RemoveHandle( pool.UID );

			return 0;
		}

		[BiosFunction( 0xBED27435, "sceKernelAllocateVpl" )]
		// SDK location: /user/pspthreadman.h:1277
		// SDK declaration: int sceKernelAllocateVpl(SceUID uid, unsigned int size, void **data, unsigned int *timeout);
		public int sceKernelAllocateVpl( int uid, int size, int data, int timeout )
		{
			KVariablePool pool = _kernel.GetHandle<KVariablePool>( uid );
			if( pool == null )
				return -1;

			KMemoryBlock block = pool.Allocate();
			if( block != null )
			{
				Debug.Assert( data != 0 );
				unsafe
				{
					uint* pdata = ( uint* )_memorySystem.Translate( ( uint )data );
					*pdata = block.Address;
				}

				return 0;
			}
			else
			{
				uint timeoutUs = 0;
				if( timeout != 0 )
				{
					unsafe
					{
						uint* ptimeout = ( uint* )_memorySystem.Translate( ( uint )timeout );
						timeoutUs = *ptimeout;
					}
				}
				KThread thread = _kernel.ActiveThread;
				Debug.Assert( thread != null );
				thread.Wait( pool, ( uint )data, timeoutUs, false );
				return 0;
			}
		}

		[BiosFunction( 0xEC0A693F, "sceKernelAllocateVplCB" )]
		// SDK location: /user/pspthreadman.h:1289
		// SDK declaration: int sceKernelAllocateVplCB(SceUID uid, unsigned int size, void **data, unsigned int *timeout);
		public int sceKernelAllocateVplCB( int uid, int size, int data, int timeout )
		{
			KVariablePool pool = _kernel.GetHandle<KVariablePool>( uid );
			if( pool == null )
				return -1;

			KMemoryBlock block = pool.Allocate();
			if( block != null )
			{
				Debug.Assert( data != 0 );
				unsafe
				{
					uint* pdata = ( uint* )_memorySystem.Translate( ( uint )data );
					*pdata = block.Address;
				}

				return 0;
			}
			else
			{
				uint timeoutUs = 0;
				if( timeout != 0 )
				{
					unsafe
					{
						uint* ptimeout = ( uint* )_memorySystem.Translate( ( uint )timeout );
						timeoutUs = *ptimeout;
					}
				}
				KThread thread = _kernel.ActiveThread;
				Debug.Assert( thread != null );
				thread.Wait( pool, ( uint )data, timeoutUs, true );
				return 0;
			}
		}

		[Stateless]
		[BiosFunction( 0xAF36D708, "sceKernelTryAllocateVpl" )]
		// SDK location: /user/pspthreadman.h:1300
		// SDK declaration: int sceKernelTryAllocateVpl(SceUID uid, unsigned int size, void **data);
		public int sceKernelTryAllocateVpl( int uid, int size, int data )
		{
			KVariablePool pool = _kernel.GetHandle<KVariablePool>( uid );
			if( pool == null )
				return -1;

			KMemoryBlock block = pool.Allocate();
			if( block == null )
				return -1;

			Debug.Assert( data != 0 );
			unsafe
			{
				uint* pdata = ( uint* )_memorySystem.Translate( ( uint )data );
				*pdata = block.Address;
			}

			return 0;
		}

		[BiosFunction( 0xB736E9FF, "sceKernelFreeVpl" )]
		// SDK location: /user/pspthreadman.h:1310
		// SDK declaration: int sceKernelFreeVpl(SceUID uid, void *data);
		public int sceKernelFreeVpl( int uid, int data )
		{
			KVariablePool pool = _kernel.GetHandle<KVariablePool>( uid );
			if( pool == null )
				return -1;

			bool wokeWaiter = pool.Free( data );
			if( wokeWaiter == true )
				_kernel.Schedule();

			return 0;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1D371B8A, "sceKernelCancelVpl" )]
		// SDK location: /user/pspthreadman.h:1320
		// SDK declaration: int sceKernelCancelVpl(SceUID uid, int *pnum);
		public int sceKernelCancelVpl( int uid, int pnum )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x39810265, "sceKernelReferVplStatus" )]
		// SDK location: /user/pspthreadman.h:1340
		// SDK declaration: int sceKernelReferVplStatus(SceUID uid, SceKernelVplInfo *info);
		public int sceKernelReferVplStatus( int uid, int info )
		{
			return Module.NotImplementedReturn;
		}

		#endregion

		#region FPL

		[Stateless]
		[BiosFunction( 0xC07BB470, "sceKernelCreateFpl" )]
		// SDK location: /user/pspthreadman.h:1360
		// SDK declaration: int sceKernelCreateFpl(const char *name, int part, int attr, unsigned int size, unsigned int blocks, struct SceKernelFplOptParam *opt);
		public int sceKernelCreateFpl( int name, int part, int attr, int size, int blocks, int opt )
		{
			KPartition partition = _kernel.Partitions[ part ];
			Debug.Assert( partition != null );
			if( partition == null )
				return -1;

			KFixedPool pool = new KFixedPool( _kernel, partition,
				_kernel.ReadString( ( uint )name ),
				( uint )attr, ( uint )size, blocks );
			if( pool.AllocateFplBlocks() == false )
			{
				pool.Dispose();
				//return unchecked( ( int )0x800200E0 );
				//return unchecked( ( int )0x800200D9 );
				return -1;
			}
			_kernel.AddHandle( pool );

			// option unused?
			//Debug.Assert( opt == 0 );

			return ( int )pool.UID;
		}

		[Stateless]
		[BiosFunction( 0xED1410E0, "sceKernelDeleteFpl" )]
		// SDK location: /user/pspthreadman.h:1369
		// SDK declaration: int sceKernelDeleteFpl(SceUID uid);
		public int sceKernelDeleteFpl( int uid )
		{
			KFixedPool pool = _kernel.GetHandle<KFixedPool>( uid );
			if( pool == null )
				return -1;

			pool.Dispose();

			_kernel.RemoveHandle( pool.UID );

			return 0;
		}

		[BiosFunction( 0xD979E9BF, "sceKernelAllocateFpl" )]
		// SDK location: /user/pspthreadman.h:1380
		// SDK declaration: int sceKernelAllocateFpl(SceUID uid, void **data, unsigned int *timeout);
		public int sceKernelAllocateFpl( int uid, int data, int timeout )
		{
			KFixedPool pool = _kernel.GetHandle<KFixedPool>( uid );
			if( pool == null )
				return -1;

			KMemoryBlock block = pool.Allocate();
			if( block != null )
			{
				Debug.Assert( data != 0 );
				unsafe
				{
					uint* pdata = ( uint* )_memorySystem.Translate( ( uint )data );
					*pdata = block.Address;
				}

				return 0;
			}
			else
			{
				uint timeoutUs = 0;
				if( timeout != 0 )
				{
					unsafe
					{
						uint* ptimeout = ( uint* )_memorySystem.Translate( ( uint )timeout );
						timeoutUs = *ptimeout;
					}
				}
				KThread thread = _kernel.ActiveThread;
				Debug.Assert( thread != null );
				thread.Wait( pool, ( uint )data, timeoutUs, false );
				return 0;
			}
		}

		[BiosFunction( 0xE7282CB6, "sceKernelAllocateFplCB" )]
		// SDK location: /user/pspthreadman.h:1391
		// SDK declaration: int sceKernelAllocateFplCB(SceUID uid, void **data, unsigned int *timeout);
		public int sceKernelAllocateFplCB( int uid, int data, int timeout )
		{
			KFixedPool pool = _kernel.GetHandle<KFixedPool>( uid );
			if( pool == null )
				return -1;

			KMemoryBlock block = pool.Allocate();
			if( block != null )
			{
				Debug.Assert( data != 0 );
				unsafe
				{
					uint* pdata = ( uint* )_memorySystem.Translate( ( uint )data );
					*pdata = block.Address;
				}

				return 0;
			}
			else
			{
				uint timeoutUs = 0;
				if( timeout != 0 )
				{
					unsafe
					{
						uint* ptimeout = ( uint* )_memorySystem.Translate( ( uint )timeout );
						timeoutUs = *ptimeout;
					}
				}
				KThread thread = _kernel.ActiveThread;
				Debug.Assert( thread != null );
				thread.Wait( pool, ( uint )data, timeoutUs, true );
				return 0;
			}
		}

		[Stateless]
		[BiosFunction( 0x623AE665, "sceKernelTryAllocateFpl" )]
		// SDK location: /user/pspthreadman.h:1401
		// SDK declaration: int sceKernelTryAllocateFpl(SceUID uid, void **data);
		public int sceKernelTryAllocateFpl( int uid, int data )
		{
			KFixedPool pool = _kernel.GetHandle<KFixedPool>( uid );
			if( pool == null )
				return -1;

			KMemoryBlock block = pool.Allocate();
			if( block == null )
				return -1;

			Debug.Assert( data != 0 );
			unsafe
			{
				uint* pdata = ( uint* )_memorySystem.Translate( ( uint )data );
				*pdata = block.Address;
			}

			return 0;
		}

		[BiosFunction( 0xF6414A71, "sceKernelFreeFpl" )]
		// SDK location: /user/pspthreadman.h:1411
		// SDK declaration: int sceKernelFreeFpl(SceUID uid, void *data);
		public int sceKernelFreeFpl( int uid, int data )
		{
			KFixedPool pool = _kernel.GetHandle<KFixedPool>( uid );
			if( pool == null )
				return -1;

			bool wokeWaiter = pool.Free( data );
			if( wokeWaiter == true )
				_kernel.Schedule();

			return 0;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA8AA591F, "sceKernelCancelFpl" )]
		// SDK location: /user/pspthreadman.h:1421
		// SDK declaration: int sceKernelCancelFpl(SceUID uid, int *pnum);
		public int sceKernelCancelFpl( int uid, int pnum )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD8199E4C, "sceKernelReferFplStatus" )]
		// SDK location: /user/pspthreadman.h:1442
		// SDK declaration: int sceKernelReferFplStatus(SceUID uid, SceKernelFplInfo *info);
		public int sceKernelReferFplStatus( int uid, int info )
		{
			return Module.NotImplementedReturn;
		}

		#endregion
	}
}
