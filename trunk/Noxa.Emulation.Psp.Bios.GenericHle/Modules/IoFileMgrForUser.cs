using System;
using System.Collections.Generic;
using System.Text;
using Noxa.Emulation.Psp.Cpu;
using System.Diagnostics;
using Noxa.Emulation.Psp.IO.Media;
using System.IO;

namespace Noxa.Emulation.Psp.Bios.GenericHle.Modules
{
	class IoFileMgrForUser : IModule
	{
		#region IModule Members

		protected HleInstance _hle;
		protected Kernel _kernel;

		public IoFileMgrForUser( HleInstance hle )
		{
			Debug.Assert( hle != null );

			_hle = hle;
			_kernel = hle.Kernel as Kernel;
		}

		public string Name
		{
			get
			{
				return "IoFileMgrForUser";
			}
		}

		#endregion

		public IMediaItem FindPath( string path )
		{
			if( path.IndexOf( ':' ) >= 0 )
			{
				KernelFileDevice device = _kernel.FindDevice( path ) as KernelFileDevice;
				if( device == null )
				{
					// Perhaps a block device?
					return null;
				}

				path = path.Substring( path.IndexOf( ':' ) + 1 );
				if( ( device.MediaDevice.State == MediaState.Present ) &&
					( device.MediaRoot != null ) )
				{
					return device.MediaRoot.Find( path );
				}
				else
					return null;
			}
			else
			{
				IMediaFolder root = _kernel.CurrentPath;
				Debug.Assert( root != null );

				return root.Find( path );
			}
		}

		[BiosStub( 0x3251ea56, "sceIoPollAsync", true, 2 )]
		[BiosStubIncomplete]
		public int sceIoPollAsync( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceUID fd
			// a1 = long long *res

			KernelFileHandle handle = _kernel.FindHandle( a0 ) as KernelFileHandle;
			if( handle == null )
			{
				Debug.WriteLine( string.Format( "sceIoPollAsync: kernel file handle {0} not found", a0 ) );
				return -1;
			}

			// int
			return 0;
		}

		[BiosStub( 0xe23eec33, "sceIoWaitAsync", true, 2 )]
		[BiosStubIncomplete]
		public int sceIoWaitAsync( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceUID fd
			// a1 = long long *res

			KernelFileHandle handle = _kernel.FindHandle( a0 ) as KernelFileHandle;
			if( handle == null )
			{
				Debug.WriteLine( string.Format( "sceIoWaitAsync: kernel file handle {0} not found", a0 ) );
				return -1;
			}

			// int
			return 0;
		}

		[BiosStub( 0x35dbd746, "sceIoWaitAsyncCB", true, 2 )]
		[BiosStubIncomplete]
		public int sceIoWaitAsyncCB( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceUID fd
			// a1 = long long *res

			KernelFileHandle handle = _kernel.FindHandle( a0 ) as KernelFileHandle;
			if( handle == null )
			{
				Debug.WriteLine( string.Format( "sceIoWaitAsyncCB: kernel file handle {0} not found", a0 ) );
				return -1;
			}

			// int
			return 0;
		}

		[BiosStub( 0xcb05f8d6, "sceIoGetAsyncStat", true, 3 )]
		[BiosStubIncomplete]
		public int sceIoGetAsyncStat( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceUID fd
			// a1 = int poll
			// a2 = long long *res

			KernelFileHandle handle = _kernel.FindHandle( a0 ) as KernelFileHandle;
			if( handle == null )
			{
				Debug.WriteLine( string.Format( "sceIoGetAsyncStat: kernel file handle {0} not found", a0 ) );
				return -1;
			}

			// int
			return 0;
		}

		[BiosStub( 0xb293727f, "sceIoChangeAsyncPriority", true, 2 )]
		[BiosStubIncomplete]
		public int sceIoChangeAsyncPriority( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceUID fd
			// a1 = int pri

			KernelFileHandle handle = _kernel.FindHandle( a0 ) as KernelFileHandle;
			if( handle == null )
			{
				Debug.WriteLine( string.Format( "sceIoChangeAsyncPriority: kernel file handle {0} not found", a0 ) );
				return -1;
			}

			// int
			return 0;
		}

		[BiosStub( 0xa12a0514, "sceIoSetAsyncCallback", true, 3 )]
		[BiosStubIncomplete]
		public int sceIoSetAsyncCallback( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceUID fd
			// a1 = SceUID cb
			// a2 = void *argp

			KernelFileHandle handle = _kernel.FindHandle( a0 ) as KernelFileHandle;
			if( handle == null )
			{
				Debug.WriteLine( string.Format( "sceIoSetAsyncCallback: kernel file handle {0} not found", a0 ) );
				return -1;
			}

			// int
			return 0;
		}

		[BiosStub( 0x810c4bc3, "sceIoClose", true, 1 )]
		public int sceIoClose( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceUID fd

			KernelFileHandle handle = _kernel.FindHandle( a0 ) as KernelFileHandle;
			if( handle == null )
			{
				Debug.WriteLine( string.Format( "sceIoClose: kernel file handle {0} not found", a0 ) );
				return -1;
			}

			handle.Stream.Close();
			handle.IsOpen = false;
			_kernel.RemoveHandle( handle );

			// int
			return 0;
		}

		[BiosStub( 0xff5940b6, "sceIoCloseAsync", true, 1 )]
		[BiosStubIncomplete]
		public int sceIoCloseAsync( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceUID fd

			KernelFileHandle handle = _kernel.FindHandle( a0 ) as KernelFileHandle;
			if( handle == null )
			{
				Debug.WriteLine( string.Format( "sceIoCloseAsync: kernel file handle {0} not found", a0 ) );
				return -1;
			}

			// int
			return 0;
		}

		[BiosStub( 0x109f50bc, "sceIoOpen", true, 3 )]
		public int sceIoOpen( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = const char *file
			// a1 = int flags
			// a2 = SceMode mode

			string path = Kernel.ReadString( memory, a0 );
			IMediaFile file = this.FindPath( path ) as IMediaFile;
			if( file == null )
			{
				// Create if needed
				if( ( a1 & 0x0200 ) != 0 )
				{
					string parentPath = path.Substring( 0, path.LastIndexOf( '/' ) );
					string newName = path.Substring( path.LastIndexOf( '/' ) + 1 );
					IMediaFolder parent = this.FindPath( parentPath ) as IMediaFolder;
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
			if( ( a1 & 0x0100 ) != 0 )
				fileMode = MediaFileMode.Append;
			if( ( a1 & 0x0400 ) != 0 )
				fileMode = MediaFileMode.Truncate;
			MediaFileAccess fileAccess = MediaFileAccess.ReadWrite;
			if( ( a1 & 0x0001 ) != 0 )
				fileAccess = MediaFileAccess.Read;
			if( ( a1 & 0x0002 ) != 0 )
				fileAccess = MediaFileAccess.Write;
			if( ( a1 & 0x0003 ) != 0 )
				fileAccess = MediaFileAccess.ReadWrite;

			if( ( a1 & 0x0800 ) != 0 )
			{
				// Exclusive O_EXCL
				int x = 1;
			}
			if( ( a1 & 0x8000 ) != 0 )
			{
				// Non-blocking O_NOWAIT
				int x = 1;
			}
			if( ( a1 & 0x0004 ) != 0 )
			{
				// ? O_NBLOCK
				int x = 1;
			}

			Stream stream = file.Open( fileMode, fileAccess );
			if( stream == null )
			{
				Debug.WriteLine( string.Format( "sceIoOpen: could not open stream on file '{0}' for mode {1} access {2}", path, fileMode, fileAccess ) );
				return -1;
			}

			KernelFileHandle handle = new KernelFileHandle();
			handle.HandleType = KernelHandleType.File;
			handle.Uid = _kernel.AllocateUid();
			handle.Device = _kernel.FindDevice( path ) as KernelFileDevice;
			handle.CanWrite = !handle.Device.ReadOnly;
			handle.CanSeek = handle.Device.IsSeekable;
			handle.IsOpen = true;
			handle.MediaItem = file;
			handle.Stream = stream;
			_kernel.AddHandle( handle );

			// SceUID
			return handle.Uid;
		}

		[BiosStub( 0x89aa9906, "sceIoOpenAsync", true, 3 )]
		[BiosStubIncomplete]
		public int sceIoOpenAsync( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = const char *file
			// a1 = int flags
			// a2 = SceMode mode

			string path = Kernel.ReadString( memory, a0 );
			IMediaFile file = this.FindPath( path ) as IMediaFile;
			if( file == null )
			{
				Debug.WriteLine( string.Format( "sceIoOpenAsync: could not find path '{0}'", path ) );
				return -1;
			}

			// SceUID
			return 0;
		}

		[BiosStub( 0x6a638d83, "sceIoRead", true, 3 )]
		public int sceIoRead( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceUID fd
			// a1 = void *data
			// a2 = SceSize size

			KernelFileHandle handle = _kernel.FindHandle( a0 ) as KernelFileHandle;
			if( handle == null )
			{
				Debug.WriteLine( string.Format( "sceIoRead: kernel file handle {0} not found", a0 ) );
				return -1;
			}

			memory.ReadStream( a1, handle.Stream, a2 );

			// int
			return 0;
		}

		[BiosStub( 0xa0b5a7c2, "sceIoReadAsync", true, 3 )]
		[BiosStubIncomplete]
		public int sceIoReadAsync( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceUID fd
			// a1 = void *data
			// a2 = SceSize size

			KernelFileHandle handle = _kernel.FindHandle( a0 ) as KernelFileHandle;
			if( handle == null )
			{
				Debug.WriteLine( string.Format( "sceIoReadAsync: kernel file handle {0} not found", a0 ) );
				return -1;
			}

			// int
			return 0;
		}

		[BiosStub( 0x42ec03ac, "sceIoWrite", true, 3 )]
		public int sceIoWrite( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceUID fd
			// a1 = const void *data
			// a2 = SceSize size

			KernelFileHandle handle = _kernel.FindHandle( a0 ) as KernelFileHandle;
			if( handle == null )
			{
				Debug.WriteLine( string.Format( "sceIoWrite: kernel file handle {0} not found", a0 ) );
				return -1;
			}

			memory.WriteStream( a1, handle.Stream, a2 );

			// int
			return 0;
		}

		[BiosStub( 0x0facab19, "sceIoWriteAsync", true, 3 )]
		[BiosStubIncomplete]
		public int sceIoWriteAsync( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceUID fd
			// a1 = const void *data
			// a2 = SceSize size

			KernelFileHandle handle = _kernel.FindHandle( a0 ) as KernelFileHandle;
			if( handle == null )
			{
				Debug.WriteLine( string.Format( "sceIoWriteAsync: kernel file handle {0} not found", a0 ) );
				return -1;
			}

			// int
			return 0;
		}

		[BiosStub( 0x27eb27b8, "sceIoLseek", true, 3 )]
		public int sceIoLseek( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceUID fd
			// a1 + a2 = SceOff offset (64)
			// a3 = int whence

			KernelFileHandle handle = _kernel.FindHandle( a0 ) as KernelFileHandle;
			if( handle == null )
			{
				Debug.WriteLine( string.Format( "sceIoLseek: kernel file handle {0} not found", a0 ) );
				return -1;
			}

			System.IO.SeekOrigin seekOrigin = SeekOrigin.Current;
			switch( a3 )
			{
				case 1:
					seekOrigin = System.IO.SeekOrigin.Current;
					break;
				case 2:
					seekOrigin = System.IO.SeekOrigin.End;
					break;
				case 0:
					seekOrigin = System.IO.SeekOrigin.Begin;
					break;
			}

			// make sure a1 has the value
			long newPosition = handle.Stream.Seek( a1, seekOrigin );

			// SceOff (64)
			return ( int )newPosition;
		}

		[BiosStub( 0x71b19e77, "sceIoLseekAsync", true, 3 )]
		[BiosStubIncomplete]
		public int sceIoLseekAsync( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceUID fd
			// a1 = SceOff offset
			// a2 = int whence

			KernelFileHandle handle = _kernel.FindHandle( a0 ) as KernelFileHandle;
			if( handle == null )
			{
				Debug.WriteLine( string.Format( "sceIoLseekAsync: kernel file handle {0} not found", a0 ) );
				return -1;
			}

			// int
			return 0;
		}

		[BiosStub( 0x68963324, "sceIoLseek32", true, 3 )]
		public int sceIoLseek32( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceUID fd
			// a1 = int offset
			// a2 = int whence

			KernelFileHandle handle = _kernel.FindHandle( a0 ) as KernelFileHandle;
			if( handle == null )
			{
				Debug.WriteLine( string.Format( "sceIoLseek32: kernel file handle {0} not found", a0 ) );
				return -1;
			}

			System.IO.SeekOrigin seekOrigin = SeekOrigin.Current;
			switch( a2 )
			{
				case 1:
					seekOrigin = System.IO.SeekOrigin.Current;
					break;
				case 2:
					seekOrigin = System.IO.SeekOrigin.End;
					break;
				case 0:
					seekOrigin = System.IO.SeekOrigin.Begin;
					break;
			}

			long newPosition = handle.Stream.Seek( a1, seekOrigin );

			// int
			return ( int )newPosition;
		}

		[BiosStub( 0x1b385d8f, "sceIoLseek32Async", true, 3 )]
		[BiosStubIncomplete]
		public int sceIoLseek32Async( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceUID fd
			// a1 = int offset
			// a2 = int whence

			KernelFileHandle handle = _kernel.FindHandle( a0 ) as KernelFileHandle;
			if( handle == null )
			{
				Debug.WriteLine( string.Format( "sceIoLseek32Async: kernel file handle {0} not found", a0 ) );
				return -1;
			}

			// int
			return 0;
		}

		[BiosStub( 0x63632449, "sceIoIoctl", true, 6 )]
		[BiosStubIncomplete]
		public int sceIoIoctl( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceUID fd
			// a1 = unsigned int cmd
			// a2 = void *indata
			// a3 = int inlen
			// sp[0] = void *outdata
			// sp[4] = int outlen
			int a4 = memory.ReadWord( sp + 0 );
			int a5 = memory.ReadWord( sp + 4 );

			KernelFileHandle handle = _kernel.FindHandle( a0 ) as KernelFileHandle;
			if( handle == null )
			{
				Debug.WriteLine( string.Format( "sceIoIoctl: kernel file handle {0} not found", a0 ) );
				return -1;
			}

			// int
			return 0;
		}

		[BiosStub( 0xe95a012b, "sceIoIoctlAsync", true, 6 )]
		[BiosStubIncomplete]
		public int sceIoIoctlAsync( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceUID fd
			// a1 = unsigned int cmd
			// a2 = void *indata
			// a3 = int inlen
			// sp[0] = void *outdata
			// sp[4] = int outlen
			int a4 = memory.ReadWord( sp + 0 );
			int a5 = memory.ReadWord( sp + 4 );

			KernelFileHandle handle = _kernel.FindHandle( a0 ) as KernelFileHandle;
			if( handle == null )
			{
				Debug.WriteLine( string.Format( "sceIoIoctlAsync: kernel file handle {0} not found", a0 ) );
				return -1;
			}

			// int
			return 0;
		}

		[BiosStub( 0xb29ddf9c, "sceIoDopen", true, 1 )]
		public int sceIoDopen( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = const char *dirname

			string path = Kernel.ReadString( memory, a0 );
			IMediaFolder folder = this.FindPath( path ) as IMediaFolder;
			if( folder == null )
			{
				Debug.WriteLine( string.Format( "sceIoDopen: could not find path '{0}'", path ) );
				return -1;
			}

			KernelFileHandle handle = new KernelFileHandle();
			handle.HandleType = KernelHandleType.File;
			handle.Uid = _kernel.AllocateUid();
			handle.Device = _kernel.FindDevice( path ) as KernelFileDevice;
			handle.CanWrite = !handle.Device.ReadOnly;
			handle.CanSeek = handle.Device.IsSeekable;
			handle.IsOpen = true;
			handle.MediaItem = folder;
			handle.FolderOffset = -2;
			_kernel.AddHandle( handle );

			// SceUID
			return handle.Uid;
		}

		[BiosStub( 0xe3eb004c, "sceIoDread", true, 2 )]
		public int sceIoDread( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceUID fd
			// a1 = SceIoDirent *dir

			KernelFileHandle handle = _kernel.FindHandle( a0 ) as KernelFileHandle;
			if( handle == null )
			{
				Debug.WriteLine( string.Format( "sceIoDread: kernel file handle {0} not found", a0 ) );
				return -1;
			}

			int offset = handle.FolderOffset;
			handle.FolderOffset++;

			IMediaFolder folder = handle.MediaItem as IMediaFolder;
			if( offset == folder.Items.Length )
				return 0;

			// (d_stat)
			//	SceMode			st_mode 
			//	unsigned int	st_attr 
			//	SceOff			st_size  (64)
			//	ScePspDateTime	st_ctime 
			//	ScePspDateTime	st_atime 
			//	ScePspDateTime	st_mtime 
			//	unsigned int	st_private [6]
			//char				d_name [256] 
			//void*				d_private 
			//int				dummy

			if( ( offset == -2 ) ||
				( offset == -1 ) )
			{
				int address = a1;
				memory.WriteWord( address, 4, 0777 );
				address += 4;

				uint attributes = 0x0010;
				memory.WriteWord( address, 4, ( int )attributes );
				address += 4;

				memory.WriteWord( address, 4, 0 );
				memory.WriteWord( address + 4, 4, 0 );
				address += 8;

				address += Kernel.WriteTime( memory, address, DateTime.Now );
				address += Kernel.WriteTime( memory, address, DateTime.Now );
				address += Kernel.WriteTime( memory, address, DateTime.Now );

				address += 6 * 4; // no private stat data - blank here?

				string name = ( offset == -2 ) ? "." : "..";
				int nameLength = Kernel.WriteString( memory, address, name );
				address += 256; // Maybe blank here?

				memory.WriteWord( address, 4, 0 );
				address += 4; // no private dir data

				memory.WriteWord( address, 4, 0 );
				address += 4;
			}
			else
			{
				IMediaItem child = folder.Items[ offset ];
				IMediaFolder childFolder = child as IMediaFolder;
				IMediaFile childFile = child as IMediaFile;

				int address = a1;
				memory.WriteWord( address, 4, 0777 );
				address += 4;

				uint attributes = 0;
				//if( ( child.Attributes & MediaItemAttributes.Hidden ) == MediaItemAttributes.Hidden )
				//	attributes |= 0;
				//if( ( child.Attributes & MediaItemAttributes.ReadOnly ) == MediaItemAttributes.ReadOnly )
				//	attributes |= 0;
				if( childFile != null )
					attributes |= 0x0020;
				if( childFolder != null )
					attributes |= 0x0010;
				if( child.IsSymbolicLink == true )
					attributes |= 0x0008;
				memory.WriteWord( address, 4, ( int )attributes );
				address += 4;

				if( childFile != null )
					memory.WriteWord( address, 4, ( int )childFile.Length );
				else
					memory.WriteWord( address, 4, 0 );
				memory.WriteWord( address + 4, 4, 0 );
				address += 8;

				address += Kernel.WriteTime( memory, address, child.CreationTime );
				address += Kernel.WriteTime( memory, address, child.AccessTime );
				address += Kernel.WriteTime( memory, address, child.ModificationTime );

				address += 6 * 4; // no private stat data - blank here?

				int nameLength = Kernel.WriteString( memory, address, child.Name );
				address += 256; // Maybe blank here?

				memory.WriteWord( address, 4, 0 );
				address += 4; // no private dir data

				memory.WriteWord( address, 4, 0 );
				address += 4;
			}

			// int - 0 to stop, 1 to keep going
			return 1;
		}

		[BiosStub( 0xeb092469, "sceIoDclose", true, 1 )]
		public int sceIoDclose( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceUID fd

			KernelFileHandle handle = _kernel.FindHandle( a0 ) as KernelFileHandle;
			if( handle == null )
			{
				Debug.WriteLine( string.Format( "sceIoDclose: kernel file handle {0} not found", a0 ) );
				return -1;
			}

			handle.IsOpen = false;
			_kernel.RemoveHandle( handle );

			// int
			return 0;
		}

		[BiosStub( 0xf27a9c51, "sceIoRemove", true, 1 )]
		public int sceIoRemove( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = const char *file

			string path = Kernel.ReadString( memory, a0 );
			IMediaFile file = this.FindPath( path ) as IMediaFile;
			if( file == null )
			{
				Debug.WriteLine( string.Format( "sceIoRemove: could not find path '{0}'", path ) );
				return -1;
			}

			file.Delete();

			// int
			return 0;
		}

		[BiosStub( 0x06a70004, "sceIoMkdir", true, 2 )]
		public int sceIoMkdir( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = const char *
			// a1 = SceMode mode

			// will be absolute for full path or relative
			string path = Kernel.ReadString( memory, a0 );

			if( path[ 0 ] == '/' )
			{
				if( path[ path.Length - 1 ] == '/' )
					path = path.Substring( 0, path.Length - 1 );
				string parentPath = path.Substring( 0, path.LastIndexOf( '/' ) );
				string newName = path.Substring( path.LastIndexOf( '/' ) + 1 );

				IMediaFolder folder = this.FindPath( parentPath ) as IMediaFolder;
				if( folder == null )
				{
					Debug.WriteLine( string.Format( "sceIoMkdir: could not find path '{0}'", path ) );
					return -1;
				}

				// TODO: something with mode
				IMediaFolder newFolder = folder.CreateFolder( newName );
			}

			// int
			return 0;
		}

		[BiosStub( 0x1117c65f, "sceIoRmdir", true, 1 )]
		public int sceIoRmdir( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = const char *path

			string path = Kernel.ReadString( memory, a0 );
			IMediaFolder folder = this.FindPath( path ) as IMediaFolder;
			if( folder == null )
			{
				Debug.WriteLine( string.Format( "sceIoRmdir: could not find path '{0}'", path ) );
				return -1;
			}

			folder.Delete();

			// int
			return 0;
		}

		[BiosStub( 0x55f4717d, "sceIoChdir", true, 1 )]
		public int sceIoChdir( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = const char *path

			string path = Kernel.ReadString( memory, a0 );
			IMediaFolder folder = this.FindPath( path ) as IMediaFolder;
			if( folder == null )
			{
				Debug.WriteLine( string.Format( "sceIoChdir: could not find path '{0}'", path ) );
				return -1;
			}

			_kernel.CurrentPath = folder;

			// int
			return 0;
		}

		[BiosStub( 0xab96437f, "sceIoSync", true, 2 )]
		[BiosStubIncomplete]
		public int sceIoSync( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = const char *device
			// a1 = unsigned int unk

			// int
			return 0;
		}

		[BiosStub( 0xace946e8, "sceIoGetstat", true, 2 )]
		[BiosStubIncomplete]
		public int sceIoGetstat( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = const char *file
			// a1 = SceIoStat *stat

			// COULD BE FOLDER
			string path = Kernel.ReadString( memory, a0 );
			IMediaFile file = this.FindPath( path ) as IMediaFile;
			if( file == null )
			{
				Debug.WriteLine( string.Format( "sceIoGetstat: could not find path '{0}'", path ) );
				return -1;
			}

			// int
			return 0;
		}

		[BiosStub( 0xb8a740f4, "sceIoChstat", true, 3 )]
		[BiosStubIncomplete]
		public int sceIoChstat( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = const char *file
			// a1 = SceIoStat *stat
			// a2 = int bits

			// COULD BE FOLDER
			string path = Kernel.ReadString( memory, a0 );
			IMediaFile file = this.FindPath( path ) as IMediaFile;
			if( file == null )
			{
				Debug.WriteLine( string.Format( "sceIoChstat: could not find path '{0}'", path ) );
				return -1;
			}

			// int
			return 0;
		}

		[BiosStub( 0x779103a0, "sceIoRename", true, 2 )]
		[BiosStubIncomplete]
		public int sceIoRename( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = const char *oldname
			// a1 = const char *newname

			// COULD BE FOLDER
			string path = Kernel.ReadString( memory, a0 );
			IMediaFile file = this.FindPath( path ) as IMediaFile;
			if( file == null )
			{
				Debug.WriteLine( string.Format( "sceIoRename: could not find path '{0}'", path ) );
				return -1;
			}

			// int
			return 0;
		}

		[BiosStub( 0x54f5fb11, "sceIoDevctl", true, 6 )]
		[BiosStubIncomplete]
		public int sceIoDevctl( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = const char *dev
			// a1 = unsigned int cmd
			// a2 = void *indata
			// a3 = int inlen
			// sp[0] = void *outdata
			// sp[4] = int outlen
			int a4 = memory.ReadWord( sp + 0 );
			int a5 = memory.ReadWord( sp + 4 );

			// int
			return 0;
		}

		[BiosStub( 0x08bd7374, "sceIoGetDevType", true, 1 )]
		[BiosStubIncomplete]
		public int sceIoGetDevType( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceUID fd

			KernelFileHandle handle = _kernel.FindHandle( a0 ) as KernelFileHandle;
			if( handle == null )
			{
				Debug.WriteLine( string.Format( "sceIoGetDevType: kernel file handle {0} not found", a0 ) );
				return -1;
			}

			// int
			return 0;
		}

		[BiosStub( 0xb2a628c1, "sceIoAssign", true, 6 )]
		[BiosStubIncomplete]
		public int sceIoAssign( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = const char *dev2
			// a1 = const char *dev3
			// a2 = const char *mode
			// a3 = int unk1
			// sp[0] = void *unk1
			// sp[4] = long unk2
			int a4 = memory.ReadWord( sp + 0 );
			int a5 = memory.ReadWord( sp + 4 );

			// int
			return 0;
		}

		[BiosStub( 0x6d08a871, "sceIoUnassign", true, 1 )]
		[BiosStubIncomplete]
		public int sceIoUnassign( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = const char *dev

			// int
			return 0;
		}

		[BiosStub( 0xe8bc6571, "sceIoCancel", true, 1 )]
		[BiosStubIncomplete]
		public int sceIoCancel( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = SceUID fd

			KernelFileHandle handle = _kernel.FindHandle( a0 ) as KernelFileHandle;
			if( handle == null )
			{
				Debug.WriteLine( string.Format( "sceIoCancel: kernel file handle {0} not found", a0 ) );
				return -1;
			}

			// int
			return 0;
		}
	}
}
