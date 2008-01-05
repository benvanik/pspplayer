// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace Noxa.Emulation.Psp.Media.FileSystem
{
	class MediaFile : IMediaFile
	{
		protected IMediaDevice _device;
		protected MediaFolder _parent;
		protected FileInfo _info;

		internal MediaFile( IMediaDevice device, MediaFolder parent, FileInfo info )
		{
			Debug.Assert( device != null );
			Debug.Assert( parent != null );
			Debug.Assert( info != null );

			_device = device;
			_parent = parent;
			_info = info;

			// Add to parent
			if( _parent != null )
				_parent.AddItemInternal( this );

			this.Refresh();
		}

		public IMediaDevice Device
		{
			get
			{
				return _device;
			}
		}

		public long LogicalBlockNumber
		{
			get
			{
				return 0;
			}
		}

		public long Length
		{
			get
			{
				this.Refresh();
				return _info.Length;
			}
		}

		public Stream OpenRead()
		{
			return _info.OpenRead();
		}

		public Stream Open( MediaFileMode mode, MediaFileAccess access )
		{
			if( _device.IsReadOnly == true )
				return null;

			FileMode fsMode = FileMode.OpenOrCreate;
			switch( mode )
			{
				case MediaFileMode.Normal:
					fsMode = FileMode.OpenOrCreate;
					break;
				case MediaFileMode.Append:
					fsMode = FileMode.Open;
					break;
				case MediaFileMode.Truncate:
					fsMode = FileMode.Truncate;
					break;
			}
			FileAccess fsAccess = FileAccess.Read;
			switch( access )
			{
				case MediaFileAccess.Read:
					fsAccess = FileAccess.Read;
					break;
				case MediaFileAccess.Write:
					fsAccess = FileAccess.Write;
					break;
				case MediaFileAccess.ReadWrite:
					fsAccess = FileAccess.ReadWrite;
					break;
			}

			Stream stream = _info.Open( fsMode, fsAccess, FileShare.ReadWrite );

			// Hack to overcome the Win32 limitation of not being able to
			// open a file for read/write append
			if( mode == MediaFileMode.Append )
				stream.Seek( 0, SeekOrigin.End );

			return stream;
		}

		public string Name
		{
			get
			{
				this.Refresh();
				return _info.Name;
			}
			set
			{
				if( _device.IsReadOnly == true )
					return;
				_parent.RenameItemInternal( this, value );
				_info.MoveTo( value );
			}
		}

		public IMediaFolder Parent
		{
			get
			{
				return _parent;
			}
		}

		public string AbsolutePath
		{
			get
			{
				this.Refresh();
				return Path.Combine( _parent.AbsolutePath, _info.Name );
			}
		}

		public MediaItemAttributes Attributes
		{
			get
			{
				this.Refresh();
				MediaItemAttributes ret = MediaItemAttributes.Normal;
				FileAttributes attr = _info.Attributes;
				if( ( attr & FileAttributes.ReadOnly ) == FileAttributes.ReadOnly )
					ret |= MediaItemAttributes.ReadOnly;
				if( ( attr & FileAttributes.Hidden ) == FileAttributes.Hidden )
					ret |= MediaItemAttributes.Hidden;
				return ret;
			}
			set
			{
				if( _device.IsReadOnly == true )
					return;
				this.Refresh();
				FileAttributes attr = _info.Attributes;
				attr &= ~( FileAttributes.ReadOnly | FileAttributes.Hidden );
				if( ( value & MediaItemAttributes.Hidden ) == MediaItemAttributes.Hidden )
					attr |= FileAttributes.Hidden;
				if( ( value & MediaItemAttributes.ReadOnly ) == MediaItemAttributes.ReadOnly )
					attr |= FileAttributes.ReadOnly;
				_info.Attributes = attr;
			}
		}

		public DateTime CreationTime
		{
			get
			{
				this.Refresh();
				return _info.CreationTime;
			}
			set
			{
				if( _device.IsReadOnly == true )
					return;
				_info.CreationTime = value;
			}
		}

		public DateTime ModificationTime
		{
			get
			{
				this.Refresh();
				return _info.LastWriteTime;
			}
			set
			{
				if( _device.IsReadOnly == true )
					return;
				_info.LastWriteTime = value;
			}
		}

		public DateTime AccessTime
		{
			get
			{
				this.Refresh();
				return _info.LastAccessTime;
			}
			set
			{
				if( _device.IsReadOnly == true )
					return;
				_info.LastAccessTime = value;
			}
		}

		public bool IsSymbolicLink
		{
			get
			{
				// TODO: Support symbolic links - maybe?
				return false;
			}
		}

		public bool MoveTo( IMediaFolder destination )
		{
			try
			{
				if( _device.IsReadOnly == true )
					return false;
				if( destination.Device.IsReadOnly == true )
					return false;

				this.Refresh();

				_parent.RemoveItemInternal( this );
				_info.MoveTo( Path.Combine( destination.AbsolutePath, _info.Name ) );

				MediaFolder folder = destination as MediaFolder;
				Debug.Assert( folder != null, "Do not support inter-device moves yet" );
				folder.AddItemInternal( this );
				_parent = folder;
			}
			catch
			{
				return false;
			}
			return true;
		}

		public bool CopyTo( IMediaFolder destination )
		{
			if( destination.Device.IsReadOnly == true )
				return false;

			// TODO: File copy
			throw new NotImplementedException();
		}

		public void Delete()
		{
			if( _device.IsReadOnly == true )
				return;

			_parent.RemoveItemInternal( this );
			try
			{
				_info.Delete();
			}
			catch { }
		}

		internal void Refresh()
		{
			// TODO: Only refresh info when required
			_info.Refresh();
		}
	}
}
