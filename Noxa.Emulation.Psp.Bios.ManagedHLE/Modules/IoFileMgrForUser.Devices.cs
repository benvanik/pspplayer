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
using Noxa.Emulation.Psp.Media;

namespace Noxa.Emulation.Psp.Bios.ManagedHLE.Modules
{
	partial class IoFileMgrForUser
	{
		private const int IoctlGetSectorSize = 0x01020003;
		private const int IoctlGetFilePointer = 0x01020004;
		private const int DevctlActivateUmd = 0x01F00003;

		[Stateless]
		[BiosFunction( 0x63632449, "sceIoIoctl" )]
		// SDK location: /user/pspiofilemgr.h:368
		// SDK declaration: int sceIoIoctl(SceUID fd, unsigned int cmd, void *indata, int inlen, void *outdata, int outlen);
		public unsafe int sceIoIoctl( int fd, int cmd, int indata, int inlen, int outdata, int outlen )
		{
			KFile handle = _kernel.GetHandle<KFile>( fd );
			if( handle == null )
			{
				Log.WriteLine( Verbosity.Normal, Feature.Bios, "sceIoIoctl: kernel file handle not found: {0}", fd );
				return -1;
			}

			byte* inp = ( byte* )0;
			byte* outp = ( byte* )0;
			if( indata != 0 )
				inp = _memorySystem.Translate( ( uint )indata );
			if( outdata != 0 )
				outp = _memorySystem.Translate( ( uint )outdata );

			switch( cmd )
			{
				case IoctlGetSectorSize:
					Debug.Assert( outlen == 4 );
					*( uint* )outp = 2048;
					return 0;
				case IoctlGetFilePointer:
					// Not sure if this is 64 or 32
					Debug.Assert( outlen == 8 );
					Debug.Assert( handle.Stream != null );
					*( ulong* )outp = ( ulong )handle.Stream.Position;
					return 0;
				default:
					Log.WriteLine( Verbosity.Normal, Feature.Bios, "sceIoIoctl: unknown command 0x" + cmd.ToString( "X8" ) );
					return 0;
			}
		}

		[Stateless]
		[BiosFunction( 0xE95A012B, "sceIoIoctlAsync" )]
		// SDK location: /user/pspiofilemgr.h:381
		// SDK declaration: int sceIoIoctlAsync(SceUID fd, unsigned int cmd, void *indata, int inlen, void *outdata, int outlen);
		public int sceIoIoctlAsync( int fd, int cmd, int indata, int inlen, int outdata, int outlen )
		{
			return sceIoIoctl( fd, cmd, indata, inlen, outdata, outlen );
		}

		private KCallback _msInsertEjectCallback;

		[BiosFunction( 0x54F5FB11, "sceIoDevctl" )]
		// SDK location: /user/pspiofilemgr.h:306
		// SDK declaration: int sceIoDevctl(const char *dev, unsigned int cmd, void *indata, int inlen, void *outdata, int outlen);
		public unsafe int sceIoDevctl( int dev, int cmd, int indata, int inlen, int outdata, int outlen )
		{
			// Usually callbacks are created right before this
			//Syscall: ThreadManForUser::sceKernelCreateCallback(08AE7638, 08A892D0, 00000000) from 0x08A87F70
			//Syscall: IoFileMgrForUser::sceIoDevctl(08AE7628, 02415821, 08027BA4, 00000004, 00000000, 00000000) from 0x08A87FAC (NI)

			// Issue the last created callback?

			string name = _kernel.ReadString( ( uint )dev );
			// We only support ms for now
			if( ( name != "fatms0:" ) &&
				( name != "mscmhc0:" ) &&
				( name != "ms0:" ) )
			{
				Log.WriteLine( Verbosity.Normal, Feature.Bios, "sceIoDevctl: device {0} not supported", name );
				return 0;
			}

			IMemoryStickDevice memoryStick = _kernel.Emulator.MemoryStick;
			Debug.Assert( memoryStick != null );

			byte* inp = ( byte* )0;
			byte* outp = ( byte* )0;
			if( indata != 0 )
				inp = _memorySystem.Translate( ( uint )indata );
			if( outdata != 0 )
				outp = _memorySystem.Translate( ( uint )outdata );

			switch( cmd )
			{
				case 0x02425818:	// Write free space to indata
					Debug.Assert( inlen == 4 );
					{
						uint capacity = ( uint )memoryStick.Capacity / ( 512 * 64 );
						uint free = ( uint )memoryStick.Available / ( 512 * 64 );
						// 0 * 3 * 4 = total bytes
						// 1 * 3 * 4 = total free bytes
						// 2 * 3 * 4 = slightly less than total free bytes?
						// 124958 42083 41968 512 64
						if( indata != 0 )
						{
							uint* p = ( uint* )_memorySystem.Translate( *( uint* )inp );
							p[ 0 ] = capacity;	// total
							p[ 1 ] = free;		// free
							p[ 2 ] = free - 115;
							p[ 3 ] = 512;	// scalar 1
							p[ 4 ] = 64;	// scalar 2
						}
						// Not sure if this is the right thing to do - outp seems to point to the
						// same address as *inp
						if( outdata != 0 )
						{
							uint* p = ( uint* )outp;
							p[ 0 ] = capacity;	// total
							p[ 1 ] = free;		// free
							p[ 2 ] = free - 115;
							p[ 3 ] = 512;	// scalar 1
							p[ 4 ] = 64;	// scalar 2
						}
					}
					break;
				case 0x0240D81E:		// Refresh directory listings - no params
					break;
				case 0x02025806:		// Get insertion status
					// outdata should be pointer to dword to write 1 = in, 2 = out
					Debug.Assert( outlen == 4 );
					*( uint* )outp = 1;
					break;
				case 0x02415821:		// Register insert/eject callback
					Debug.Assert( inlen == 4 );
					{
						Debug.Assert( _msInsertEjectCallback == null );
						uint cbid = *( uint* )inp;
						_msInsertEjectCallback = _kernel.GetHandle<KCallback>( cbid );
						if( _msInsertEjectCallback != null )
						{
							Log.WriteLine( Verbosity.Normal, Feature.Bios, "Registered MemoryStick insert/eject callback: {0} ({1:X8})", _msInsertEjectCallback.Name, _msInsertEjectCallback.UID );
							_kernel.AddOneShotTimer( new TimerCallback( this.MemoryStickInserted ), _msInsertEjectCallback, 10 );
						}
						else
							Log.WriteLine( Verbosity.Critical, Feature.Bios, "sceIoDevctl: could not find callback {0} for MemoryStick insert/eject", cbid );
					}
					break;
				case 0x02415822:		// Unregister insert/eject callback
					Debug.Assert( inlen == 4 );
					{
						uint cbid = *( uint* )inp;
						Debug.Assert( _msInsertEjectCallback != null );
						Debug.Assert( _msInsertEjectCallback.UID == cbid );
						// We may have multiple callbacks and stuff, but I don't care
						_msInsertEjectCallback = null;
					}
					break;
				case 0x02425823:
					// Unknown - guessing wait on MS insert/eject?
					//_kernel.Schedule();
					//break;
					//return unchecked( ( int )0x80020325 );
					break;

				case DevctlActivateUmd:	// Activate the UMD drive for real?
					// Nothing?
					break;

				default:
					Log.WriteLine( Verbosity.Normal, Feature.Bios, "sceIoDevctl: unknown command 0x" + cmd.ToString( "X8" ) );

					return 0;
			}

			return 0;
		}

		private void MemoryStickInserted( Timer timer )
		{
			KCallback cb = ( KCallback )timer.State;
			cb.NotifyCount++;
			// 1 = inserted, 2 = ejected
			_kernel.IssueCallback( cb, ( uint )1 );
		}

		[Stateless]
		[BiosFunction( 0x08BD7374, "sceIoGetDevType" )]
		// SDK location: /user/pspiofilemgr.h:448
		// SDK declaration: int sceIoGetDevType(SceUID fd);
		public int sceIoGetDevType( int fd )
		{
			KFile handle = _kernel.GetHandle<KFile>( fd );
			if( handle == null )
			{
				Log.WriteLine( Verbosity.Normal, Feature.Bios, "sceIoGetDevType: kernel handle {0} not found", fd );
				return -1;
			}

			if( handle.Device != null )
			{
				switch( handle.Device.Device.MediaType )
				{
					default:
					case MediaType.Umd:
						return 4;
					case MediaType.MemoryStick:
						return 16;
				}
			}
			else
				return 4;
		}

		[Stateless]
		[BiosFunction( 0xB2A628C1, "sceIoAssign" )]
		// SDK location: /user/pspiofilemgr.h:325
		// SDK declaration: int sceIoAssign(const char *dev1, const char *dev2, const char *dev3, int mode, void* unk1, long unk2);
		public int sceIoAssign( int dev1, int dev2, int dev3, int mode, int unk1, int unk2 )
		{
			string dev1s = ( dev1 == 0 ) ? string.Empty : _kernel.ReadString( ( uint )dev1 );
			string dev2s = ( dev2 == 0 ) ? string.Empty : _kernel.ReadString( ( uint )dev2 );
			string dev3s = ( dev3 == 0 ) ? string.Empty : _kernel.ReadString( ( uint )dev3 );
			Log.WriteLine( Verbosity.Normal, Feature.Bios, "sceIoAssign: {0} {1} {2} - not implemented", dev1s, dev2s, dev3s );
			return 0;
		}

		[Stateless]
		[BiosFunction( 0x6D08A871, "sceIoUnassign" )]
		// SDK location: /user/pspiofilemgr.h:334
		// SDK declaration: int sceIoUnassign(const char *dev);
		public int sceIoUnassign( int dev )
		{
			return 0;
		}
	}
}
