using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.IO;
using System.Diagnostics;

namespace Noxa.Emulation.Psp.IO.Media.Iso
{
	class MediaFolder : IMediaFolder
	{
		protected UmdDevice _device;
		protected MediaFolder _parent;
		protected string _name;
		protected long _totalSize;
		protected MediaItemAttributes _attributes;
		protected DateTime _timestamp;
		
		protected List<IMediaItem> _items;
		protected IMediaItem[] _itemCache;
		protected Dictionary<string, IMediaItem> _itemLookup;

		internal MediaFolder( UmdDevice device, MediaFolder parent, string name, MediaItemAttributes attributes, DateTime timestamp )
		{
			Debug.Assert( device != null );

			_device = device;
			_parent = parent;

			_name = name;
			_attributes = attributes;
			_timestamp = timestamp;

			_items = new List<IMediaItem>();
			_itemLookup = new Dictionary<string, IMediaItem>();

			// Add to parent
			if( _parent != null )
				_parent.AddItemInternal( this );
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
		}

		internal void AddItemInternal( IMediaItem item )
		{
			_items.Add( item );
			_itemLookup.Add( item.Name, item );
			_itemCache = null;
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

			_totalSize = sum;
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
			return null;
		}

		public IMediaFolder CreateFolder( string name )
		{
			return null;
		}

		public IMediaFile CreateFile( string name )
		{
			return null;
		}

		public bool MoveItem( string name, IMediaFolder destination )
		{
			return false;
		}

		public bool CopyItem( string name, IMediaFolder destination )
		{
			IMediaItem item = this[ name ];
			if( item == null )
				return false;
			return item.CopyTo( destination );
		}

		public void DeleteItem( string name )
		{
		}

		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
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
				// Possible we are the root
				if( _parent == null )
					//return Path.Combine( _device.HostPath, _info.Name );
					return "";
				else
					return Path.Combine( _parent.AbsolutePath, _name );
			}
		}

		public MediaItemAttributes Attributes
		{
			get
			{
				return _attributes;
			}
			set
			{
			}
		}

		public DateTime CreationTime
		{
			get
			{
				return _timestamp;
			}
			set
			{
			}
		}

		public DateTime ModificationTime
		{
			get
			{
				return _timestamp;
			}
			set
			{
			}
		}

		public DateTime AccessTime
		{
			get
			{
				return DateTime.Now;
			}
			set
			{
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
			return false;
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
