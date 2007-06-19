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
		[BiosFunction( 0xB29DDF9C, "sceIoDopen" )]
		// SDK location: /user/pspiofilemgr.h:267
		// SDK declaration: SceUID sceIoDopen(const char *dirname);
		public int sceIoDopen( int dirname )
		{
			string path = _kernel.ReadString( ( uint )dirname );
			IMediaFolder folder = ( IMediaFolder )_kernel.FindPath( path );
			if( folder == null )
			{
				Log.WriteLine( Verbosity.Normal, Feature.Bios, "sceIoDopen: could not find path {0}", path );
				return -1;
			}

			KDevice dev = _kernel.FindDevice( folder.Device );

			KFile handle = new KFile( _kernel, dev, folder );
			handle.FolderOffset = -2;
			_kernel.AddHandle( handle );

			Log.WriteLine( Verbosity.Verbose, Feature.Bios, "sceIoDopen: opened {0} with ID {1}", path, handle.UID );

			return ( int )handle.UID;
		}

		[Stateless]
		[BiosFunction( 0xE3EB004C, "sceIoDread" )]
		// SDK location: /user/pspiofilemgr.h:280
		// SDK declaration: int sceIoDread(SceUID fd, SceIoDirent *dir);
		public int sceIoDread( int fd, int dir )
		{
			KFile handle = _kernel.GetHandle<KFile>( fd );
			if( handle == null )
			{
				Log.WriteLine( Verbosity.Normal, Feature.Bios, "sceIoDread: kernel dir handle {0} not found", fd );
				return -1;
			}

			int offset = handle.FolderOffset;
			handle.FolderOffset++;

			IMediaFolder folder = ( IMediaFolder )handle.Item;
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
				int address = dir;
				_memory.WriteWord( address, 4, 0777 | 0x10 );
				address += 4;

				uint attributes = 0x0010;
				_memory.WriteWord( address, 4, ( int )attributes );
				address += 4;

				_memory.WriteWord( address, 4, 0 );
				_memory.WriteWord( address + 4, 4, 0 );
				address += 8;

				address += _kernel.WriteTime( ( uint )address, DateTime.Now );
				address += _kernel.WriteTime( ( uint )address, DateTime.Now );
				address += _kernel.WriteTime( ( uint )address, DateTime.Now );

				address += 6 * 4; // no private stat data - blank here?

				String name = ( offset == -2 ) ? "." : "..";
				int nameLength = _kernel.WriteString( ( uint )address, name );
				address += 256; // Maybe blank here?

				_memory.WriteWord( address, 4, 0 );
				address += 4; // no private dir data

				_memory.WriteWord( address, 4, 0 );
				address += 4;
			}
			else
			{
				IMediaItem child = folder.Items[ offset ];
				IMediaFolder childFolder = null;
				IMediaFile childFile = null;
				if( child.GetType().GetInterface( "IMediaFolder", false ) != null )
					childFolder = ( IMediaFolder )child;
				else
					childFile = ( IMediaFile )child;

				int address = dir;
				int mode = 0777 | ( ( childFolder != null ) ? 0x10 : 0x20 );
				_memory.WriteWord( address, 4, mode );
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
				_memory.WriteWord( address, 4, ( int )attributes );
				address += 4;

				if( childFile != null )
					_memory.WriteWord( address, 4, ( int )childFile.Length );
				else
					_memory.WriteWord( address, 4, 0 );
				_memory.WriteWord( address + 4, 4, 0 );
				address += 8;

				address += _kernel.WriteTime( ( uint )address, child.CreationTime );
				address += _kernel.WriteTime( ( uint )address, child.AccessTime );
				address += _kernel.WriteTime( ( uint )address, child.ModificationTime );

				// private[ 0 ] = start sector on disk
				if( childFile != null )
					_memory.WriteWord( address, 4, ( int )( uint )childFile.LogicalBlockNumber );
				else
					_memory.WriteWord( address, 4, 0 );
				address += 5 * 4; // no private stat data - blank here?

				int nameLength = _kernel.WriteString( ( uint )address, child.Name );
				address += 256; // Maybe blank here?

				_memory.WriteWord( address, 4, 0 );
				address += 4; // no private dir data

				_memory.WriteWord( address, 4, 0 );
				address += 4;
			}

			// 0 to stop, 1 to keep going
			return 1;
		}

		[Stateless]
		[BiosFunction( 0xEB092469, "sceIoDclose" )]
		// SDK location: /user/pspiofilemgr.h:288
		// SDK declaration: int sceIoDclose(SceUID fd);
		public int sceIoDclose( int fd )
		{
			KFile handle = _kernel.GetHandle<KFile>( fd );
			if( handle == null )
			{
				Log.WriteLine( Verbosity.Normal, Feature.Bios, "sceIoDclose: kernel dir handle {0} not found", fd );
				return -1;
			}

			handle.IsOpen = false;
			_kernel.Handles.Remove( handle.UID );

			return 0;
		}

		[Stateless]
		[BiosFunction( 0xF27A9C51, "sceIoRemove" )]
		// SDK location: /user/pspiofilemgr.h:218
		// SDK declaration: int sceIoRemove(const char *file);
		public int sceIoRemove( int file )
		{
			string path = _kernel.ReadString( ( uint )file );
			IMediaItem item = _kernel.FindPath( path );
			if( item == null )
			{
				Log.WriteLine( Verbosity.Normal, Feature.Bios, "sceIoRemove: could not find path {0}", path );
				return -1;
			}

			item.Delete();

			return 0;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x06A70004, "sceIoMkdir" )]
		// SDK location: /user/pspiofilemgr.h:227
		// SDK declaration: int sceIoMkdir(const char *dir, SceMode mode);
		public int sceIoMkdir( int dir, int mode )
		{
			return Module.NotImplementedReturn;
		}

		[Stateless]
		[BiosFunction( 0x1117C65F, "sceIoRmdir" )]
		// SDK location: /user/pspiofilemgr.h:235
		// SDK declaration: int sceIoRmdir(const char *path);
		public int sceIoRmdir( int pathName )
		{
			string path = _kernel.ReadString( ( uint )pathName );
			IMediaItem item = _kernel.FindPath( path );
			if( item == null )
			{
				Log.WriteLine( Verbosity.Normal, Feature.Bios, "sceIoRmdir: could not find path {0}", path );
				return -1;
			}

			item.Delete();

			return 0;
		}

		[Stateless]
		[BiosFunction( 0x55F4717D, "sceIoChdir" )]
		// SDK location: /user/pspiofilemgr.h:243
		// SDK declaration: int sceIoChdir(const char *path);
		public int sceIoChdir( int pathName )
		{
			string path = _kernel.ReadString( ( uint )pathName );
			IMediaFolder folder = ( IMediaFolder )_kernel.FindPath( path );
			if( folder == null )
			{
				Log.WriteLine( Verbosity.Normal, Feature.Bios, "sceIoChdir: could not find path {0}", path );
				return -1;
			}

			_kernel.CurrentPath = folder;

			return 0;
		}

		[DontTrace]
		[Stateless]
		[BiosFunction( 0xACE946E8, "sceIoGetstat" )]
		// SDK location: /user/pspiofilemgr.h:344
		// SDK declaration: int sceIoGetstat(const char *file, SceIoStat *stat);
		public int sceIoGetstat( int file, int stat )
		{
			string path = _kernel.ReadString( ( uint )file );
			IMediaItem item = _kernel.FindPath( path );
			if( item == null )
			{
				Log.WriteLine( Verbosity.Normal, Feature.Bios, "sceIoGetstat: could not find path {0}", path );
				return -1;
			}

			int mode = 0;
			int attr = 0;

			if( item.IsSymbolicLink == true )
			{
				mode |= 0x4000;
				attr |= 0x0008;
			}

			//if( ( item.Attributes & MediaItemAttributes.Hidden ) == MediaItemAttributes.Hidden )
			mode |= 0x0100 | 0x0020 | 0x004; // read user / group /others
			attr |= 0x0004; // read

			if( ( item.Attributes & MediaItemAttributes.ReadOnly ) == MediaItemAttributes.ReadOnly )
			{
				mode |= 0x0080 | 0x0010 | 0x0002; // write user / group / others
				attr |= 0x0002; // write
			}

			mode |= 0x0040 | 0x0008 | 0x0001; // exec user / group / others
			attr |= 0x0001; // execute

			uint size = 0;
			if( item is IMediaFile )
			{
				mode |= 0x2000; // file
				attr |= 0x0020; // file
				size = ( uint )( ( IMediaFile )item ).Length;
			}
			else if( item is IMediaFolder )
			{
				mode |= 0x1000; // dir
				attr |= 0x0010; // directory
			}

			int address = stat;
			_memory.WriteWord( address, 4, mode );
			address += 4;
			_memory.WriteWord( address, 4, attr );
			address += 4;
			// Is this the right order for 64 bit?
			_memory.WriteWord( address, 4, ( int )size );
			_memory.WriteWord( address + 4, 4, 0 );
			address += 8;
			address += _kernel.WriteTime( ( uint )address, item.CreationTime );
			address += _kernel.WriteTime( ( uint )address, item.AccessTime );
			address += _kernel.WriteTime( ( uint )address, item.ModificationTime );
			
			// + 6 words of garbage

			return 0;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB8A740F4, "sceIoChstat" )]
		// SDK location: /user/pspiofilemgr.h:355
		// SDK declaration: int sceIoChstat(const char *file, SceIoStat *stat, int bits);
		public int sceIoChstat( int file, int stat, int bits )
		{
			return Module.NotImplementedReturn;
		}

		[Stateless]
		[BiosFunction( 0x779103A0, "sceIoRename" )]
		// SDK location: /user/pspiofilemgr.h:252
		// SDK declaration: int sceIoRename(const char *oldname, const char *newname);
		public int sceIoRename( int oldname, int newname )
		{
			string path = _kernel.ReadString( ( uint )oldname );
			IMediaItem item = _kernel.FindPath( path );
			if( item == null )
			{
				Log.WriteLine( Verbosity.Normal, Feature.Bios, "sceIoRename: could not find path {0}", path );
				return -1;
			}

			string newPath = _kernel.ReadString( ( uint )newname );
			item.Name = Path.GetFileName( newPath );

			return 0;
		}
	}
}
