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
using System.Collections;

namespace Noxa.Emulation.Psp.Media.FileSystem
{
	class MediaFolder : IMediaFolder
	{
		protected IMediaDevice _device;
		protected MediaFolder _parent;
		protected DirectoryInfo _info;

		// This is a bad idea, as we could easily get out of date
		protected List<IMediaItem> _items;
		protected IMediaItem[] _itemCache;
		protected Dictionary<string, IMediaItem> _itemLookup;

		internal MediaFolder( IMediaDevice device, MediaFolder parent, DirectoryInfo info )
		{
			Debug.Assert( device != null );
			Debug.Assert( info != null );

			_device = device;
			_parent = parent;
			_info = info;

			_items = new List<IMediaItem>();
			_itemLookup = new Dictionary<string, IMediaItem>();

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

		internal MediaFolder ParentFolder
		{
			get
			{
				return _parent;
			}
			set
			{
				_parent = value;
			}
		}

		internal void AddItemInternal( IMediaItem item )
		{
			_items.Add( item );
			_itemLookup.Add( item.Name, item );
			_itemCache = null;
		}

		internal void RenameItemInternal( IMediaItem item, string newName )
		{
			if( ( _items.Contains( item ) == false ) ||
				( _itemLookup.ContainsKey( item.Name ) == false ) )
				Debug.Assert( false, "Consistancy issue during rename - original not found" );
			else
				_itemLookup.Remove( item.Name );
			_itemLookup.Add( newName, item );
			_itemCache = null;
		}

		internal void RemoveItemInternal( IMediaItem item )
		{
			if( ( _items.Contains( item ) == false ) ||
				( _itemLookup.ContainsKey( item.Name ) == false ) )
				Debug.Assert( false, "Consistancy issue during remove - original not found" );
			else
			{
				_itemLookup.Remove( item.Name );
				_items.Remove( item );
			}
			_itemCache = null;
		}

		internal long Cache()
		{
			long sum = 0;

			foreach( DirectoryInfo info in _info.GetDirectories() )
			{
				MediaFolder folder = new MediaFolder( _device, this, info );
				sum += folder.Cache();
			}

			foreach( FileInfo info in _info.GetFiles() )
			{
				MediaFile file = new MediaFile( _device, this, info );
				sum += file.Length;
			}

			return sum;
		}

		internal void Drop()
		{
			_items.Clear();
			_itemCache = null;
			_itemLookup.Clear();
		}

		internal long CalculateSize()
		{
			long sum = 0;

			foreach( IMediaItem item in _items )
			{
				MediaFolder folder = item as MediaFolder;
				if( folder != null )
					sum += folder.CalculateSize();
				else
				{
					MediaFile file = item as MediaFile;
					sum += file.Length;
				}
			}

			return sum;
		}

		public IMediaItem[] Items
		{
			get
			{
				// Cache item list so we aren't copying on every call
				if( _itemCache == null )
					_itemCache = _items.ToArray();
				return _itemCache;
			}
		}

		public IMediaItem this[ string name ]
		{
			get
			{
				if( _itemLookup.ContainsKey( name ) == true )
					return _itemLookup[ name ];
				else
					return null;
			}
		}

		public IMediaItem Find( string path )
		{
			if( path.Length == 0 )
				return this;

			int slashIndex = path.IndexOfAny( new char[] { '/', '\\' } );
			if( slashIndex < 0 )
			{
				if( path == "." )
					return this;
				else if( path == ".." )
					return _parent;
				else
					return this[ path ];
			}

			string localPath = path.Substring( 0, slashIndex );
			string subPath = path.Substring( slashIndex + 1 );

			if( localPath.Length == 0 )
				return this.Find( subPath );

			if( localPath == "." )
				return this.Find( subPath );
			if( localPath == ".." )
				return _parent.Find( subPath );

			IMediaFolder local = this[ localPath ] as IMediaFolder;
			if( local == null )
				return null;
			if( subPath.Length == 0 )
				return local;
			return local.Find( subPath );
		}

		public IMediaFolder FindFolder( string path )
		{
			return this.Find( path ) as IMediaFolder;
		}

		public IMediaFile FindFile( string path )
		{
			return this.Find( path ) as IMediaFile;
		}

		public IMediaItem CreateSymbolicLink( string name, MediaItemType type )
		{
			if( _device.IsReadOnly == true )
				return null;

			// TODO: Support creation of symbolic links (may be hard on native fs)
			throw new NotImplementedException();
		}

		public IMediaFolder CreateFolder( string name )
		{
			if( _device.IsReadOnly == true )
				return null;

			Debug.Assert( name != null );
			Debug.Assert( name.Length > 0 );
			// TODO: Assert name is valid directory style via regex, here we just do / check
			Debug.Assert( name.IndexOfAny( new char[] { '/', '\\' } ) < 0 );
			
			DirectoryInfo info = Directory.CreateDirectory( Path.Combine( _info.FullName, name ) );
			
			MediaFolder folder = new MediaFolder( _device, this, info );
			return folder;
		}

		public IMediaFile CreateFile( string name )
		{
			if( _device.IsReadOnly == true )
				return null;

			Debug.Assert( name != null );
			Debug.Assert( name.Length > 0 );
			// TODO: Assert name is valid file style via regex, here we just do / check
			Debug.Assert( name.IndexOfAny( new char[] { '/', '\\' } ) < 0 );
			string path = Path.Combine( _info.FullName, name );

			FileInfo info = new FileInfo( path );
			// This should create a 0 length file
			info.Create().Dispose();
			
			MediaFile file = new MediaFile(_device, this, info );
			return file;
		}

		public bool MoveItem( string name, IMediaFolder destination )
		{
			if( _device.IsReadOnly == true )
				return false;
			if( destination.Device.IsReadOnly == true )
				return false;

			IMediaItem item = this[ name ];
			if( item == null )
				return false;
			return item.MoveTo( destination );
		}

		public bool CopyItem( string name, IMediaFolder destination )
		{
			if( destination.Device.IsReadOnly == true )
				return false;

			IMediaItem item = this[ name ];
			if( item == null )
				return false;
			return item.CopyTo( destination );
		}

		public void DeleteItem( string name )
		{
			if( _device.IsReadOnly == true )
				return;

			IMediaItem item = this[ name ];
			if( item == null )
				return;
			item.Delete();
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

				// Possible we are the root
				if( _parent == null )
					//return Path.Combine( _device.HostPath, _info.Name );
					return "";
				else
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

			// TODO: Folder copy
			throw new NotImplementedException();
		}

		public void Delete()
		{
			if( _device.IsReadOnly == true )
				return;

			_parent.RemoveItemInternal( this );
			_info.Delete( true );
		}

		internal void Refresh()
		{
			// TODO: Only refresh info when required
			_info.Refresh();
		}

		public IEnumerator<IMediaItem> GetEnumerator()
		{
			return _items.GetEnumerator();
		}

		IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return _items.GetEnumerator();
		}
	}
}
