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
		[Stateless]
		[BiosFunction( 0x810C4BC3, "sceIoClose" )]
		// SDK location: /user/pspiofilemgr.h:85
		// SDK declaration: int sceIoClose(SceUID fd);
		public int sceIoClose( int fd )
		{
			KFile handle = _kernel.GetHandle<KFile>( fd );
			if( handle == null )
			{
				Debug.WriteLine( string.Format( "sceIoClose: kernel file handle not found: {0}", fd ) );
				return -1;
			}

			Debug.Assert( handle.IsOpen == true );

			if( handle.Stream != null )
				handle.Stream.Close();
			handle.IsOpen = false;
			_kernel.Handles.Remove( handle.UID );

			return 0;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFF5940B6, "sceIoCloseAsync" )]
		// SDK location: /user/pspiofilemgr.h:93
		// SDK declaration: int sceIoCloseAsync(SceUID fd);
		public int sceIoCloseAsync( int fd )
		{
			return Module.NotImplementedReturn;
		}

		[Stateless]
		[BiosFunction( 0x109F50BC, "sceIoOpen" )]
		// SDK location: /user/pspiofilemgr.h:63
		// SDK declaration: SceUID sceIoOpen(const char *file, int flags, SceMode mode);
		public int sceIoOpen( int fileName, int flags, int mode )
		{
			string path = _kernel.ReadString( ( uint )fileName );
			IMediaFile file = ( IMediaFile )_kernel.FindPath( path );
			if( file == null )
			{
				// Create if needed
				if( ( flags & 0x0200 ) != 0 )
				{
					string parentPath = path.Substring( 0, path.LastIndexOf( '/' ) );
					string newName = path.Substring( path.LastIndexOf( '/' ) + 1 );
					IMediaFolder parent = ( IMediaFolder )_kernel.FindPath( parentPath );
					if( parent == null )
					{
						Debug.WriteLine( string.Format( "sceIoOpen: could not find parent to create file '{0}' in on open", path ) );
						return -1;
					}
					file = parent.CreateFile( newName );
				}
				else
				{
					Debug.WriteLine( string.Format( "sceIoOpen: could not find path '{0}'", path ) );
					return -1;
				}
			}
			/*
			 *	#define PSP_O_RDONLY	0x0001
				#define PSP_O_WRONLY	0x0002
				#define PSP_O_RDWR		(PSP_O_RDONLY | PSP_O_WRONLY)
				#define PSP_O_NBLOCK	0x0004
				#define PSP_O_DIROPEN	0x0008	// Internal use for dopen
				#define PSP_O_APPEND	0x0100
				#define PSP_O_CREAT		0x0200
				#define PSP_O_TRUNC		0x0400
				#define	PSP_O_EXCL		0x0800
				#define PSP_O_NOWAIT	0x8000*/
			MediaFileMode fileMode = MediaFileMode.Normal;
			if( ( flags & 0x0100 ) == 0x0100 )
				fileMode = MediaFileMode.Append;
			if( ( flags & 0x0400 ) == 0x0400 )
				fileMode = MediaFileMode.Truncate;
			MediaFileAccess fileAccess = MediaFileAccess.ReadWrite;
			if( ( flags & 0x0001 ) == 0x0001 )
				fileAccess = MediaFileAccess.Read;
			if( ( flags & 0x0002 ) == 0x0002 )
				fileAccess = MediaFileAccess.Write;
			if( ( flags & 0x0003 ) == 0x0003 )
				fileAccess = MediaFileAccess.ReadWrite;

			if( ( flags & 0x0800 ) != 0 )
			{
				// Exclusive O_EXCL
				//int x = 1;
			}
			if( ( flags & 0x8000 ) != 0 )
			{
				// Non-blocking O_NOWAIT
				//int x = 1;
			}
			if( ( flags & 0x0004 ) != 0 )
			{
				// ? O_NBLOCK
				//int x = 1;
			}

			Stream stream = file.Open( fileMode, fileAccess );
			if( stream == null )
			{
				Debug.WriteLine( string.Format( "sceIoOpen: could not open stream on file '{0}' for mode {1} access {2}", path, fileMode, fileAccess ) );
				return -1;
			}

			IMediaDevice device = file.Device;
			KDevice dev = _kernel.FindDevice( file.Device );
			Debug.Assert( dev != null );

			KFile handle = new KFile( _kernel, dev, file, stream );
			_kernel.AddHandle( handle );

			Debug.WriteLine( string.Format( "sceIoOpen: opened file {0} with ID {1}", path, handle.UID ) );
			
			return ( int )handle.UID;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x89AA9906, "sceIoOpenAsync" )]
		// SDK location: /user/pspiofilemgr.h:73
		// SDK declaration: SceUID sceIoOpenAsync(const char *file, int flags, SceMode mode);
		public int sceIoOpenAsync( int file, int flags, int mode )
		{
			return Module.NotImplementedReturn;
		}

		[DontTrace]
		[Stateless]
		[BiosFunction( 0x6A638D83, "sceIoRead" )]
		// SDK location: /user/pspiofilemgr.h:109
		// SDK declaration: int sceIoRead(SceUID fd, void *data, SceSize size);
		public int sceIoRead( int fd, int data, int size )
		{
			KFile handle = _kernel.GetHandle<KFile>( fd );
			if( handle == null )
			{
				Debug.WriteLine( string.Format( "sceIoRead: kernel file handle not found: {0}", fd ) );
				return -1;
			}

			if( fd == 0 )
			{
				// stdin - ignored
				return 0;
			}

			int end = ( int )handle.Stream.Length - ( int )handle.Stream.Position;
			int length = ( size < end ) ? size : end;

			_memory.WriteStream( data, handle.Stream, length );

			return length;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA0B5A7C2, "sceIoReadAsync" )]
		// SDK location: /user/pspiofilemgr.h:125
		// SDK declaration: int sceIoReadAsync(SceUID fd, void *data, SceSize size);
		public int sceIoReadAsync( int fd, int data, int size )
		{
			return Module.NotImplementedReturn;
		}

		[DontTrace]
		[Stateless]
		[BiosFunction( 0x42EC03AC, "sceIoWrite" )]
		// SDK location: /user/pspiofilemgr.h:141
		// SDK declaration: int sceIoWrite(SceUID fd, const void *data, SceSize size);
		public int sceIoWrite( int fd, int data, int size )
		{
			KFile handle = _kernel.GetHandle<KFile>( fd );
			if( handle == null )
			{
				Debug.WriteLine( string.Format( "sceIoWrite: kernel file handle not found: {0}", fd ) );
				return -1;
			}

			if( handle is KStdFile )
			{
				// Handle stdout/err write
				( ( KStdFile )handle ).Write( ( uint )data );
			}
			else
			{
				// Probably not the most efficient way ever
				_memory.ReadStream( data, handle.Stream, size );
			}

			return 0;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0FACAB19, "sceIoWriteAsync" )]
		// SDK location: /user/pspiofilemgr.h:152
		// SDK declaration: int sceIoWriteAsync(SceUID fd, const void *data, SceSize size);
		public int sceIoWriteAsync( int fd, int data, int size )
		{
			return Module.NotImplementedReturn;
		}

		[DontTrace]
		[Stateless]
		[BiosFunction( 0x27EB27B8, "sceIoLseek" )]
		// SDK location: /user/pspiofilemgr.h:169
		// SDK declaration: SceOff sceIoLseek(SceUID fd, SceOff offset, int whence);
		public long sceIoLseek( int fd, long offset, int whence )
		{
			KFile handle = _kernel.GetHandle<KFile>( fd );
			if( handle == null )
			{
				Debug.WriteLine( string.Format( "sceIoLseek: kernel file handle not found: {0}", fd ) );
				return -1;
			}

			SeekOrigin seekOrigin;
			switch( whence )
			{
				default:
				case 0:
					seekOrigin = SeekOrigin.Begin;
					if( offset > handle.Stream.Length )
						offset = 0;
					break;
				case 1:
					seekOrigin = SeekOrigin.Current;
					Debug.Assert( handle.Stream.Position + offset <= handle.Stream.Length );
					break;
				case 2:
					seekOrigin = SeekOrigin.End;
					Debug.Assert( handle.Stream.Length + offset <= handle.Stream.Length );
					break;
			}

			return handle.Stream.Seek( offset, seekOrigin );
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x71B19E77, "sceIoLseekAsync" )]
		// SDK location: /user/pspiofilemgr.h:181
		// SDK declaration: int sceIoLseekAsync(SceUID fd, SceOff offset, int whence);
		public int sceIoLseekAsync( int fd, long offset, int whence )
		{
			return Module.NotImplementedReturn;
		}

		[DontTrace]
		[Stateless]
		[BiosFunction( 0x68963324, "sceIoLseek32" )]
		// SDK location: /user/pspiofilemgr.h:198
		// SDK declaration: int sceIoLseek32(SceUID fd, int offset, int whence);
		public int sceIoLseek32( int fd, int offset, int whence )
		{
			KFile handle = _kernel.GetHandle<KFile>( fd );
			if( handle == null )
			{
				Debug.WriteLine( string.Format( "sceIoLseek32: kernel file handle not found: {0}", fd ) );
				return -1;
			}

			SeekOrigin seekOrigin;
			switch( whence )
			{
				default:
				case 0:
					seekOrigin = SeekOrigin.Begin;
					if( offset > handle.Stream.Length )
						offset = 0;
					break;
				case 1:
					seekOrigin = SeekOrigin.Current;
					Debug.Assert( handle.Stream.Position + offset <= handle.Stream.Length );
					break;
				case 2:
					seekOrigin = SeekOrigin.End;
					Debug.Assert( handle.Stream.Length + offset <= handle.Stream.Length );
					break;
			}

			return ( int )handle.Stream.Seek( offset, seekOrigin );
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1B385D8F, "sceIoLseek32Async" )]
		// SDK location: /user/pspiofilemgr.h:210
		// SDK declaration: int sceIoLseek32Async(SceUID fd, int offset, int whence);
		public int sceIoLseek32Async( int fd, int offset, int whence )
		{
			return Module.NotImplementedReturn;
		}
	}
}
