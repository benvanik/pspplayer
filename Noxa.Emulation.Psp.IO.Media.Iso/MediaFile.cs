using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace Noxa.Emulation.Psp.IO.Media.Iso
{
	class MediaFile : IMediaFile
	{
		/// <summary>
		/// Files under this size will be cached in their own memory streams.
		/// </summary>
		public const int MaximumCachableStreamSize = 4 * 1024 * 1024;

		protected UmdDevice _device;
		protected MediaFolder _parent;
		protected string _name;
		protected MediaItemAttributes _attributes;
		protected DateTime _timestamp;
		protected long _position;
		protected long _length;

		internal MediaFile( UmdDevice device, MediaFolder parent, string name, MediaItemAttributes attributes, DateTime timestamp, long position, long length )
		{
			Debug.Assert( device != null );
			Debug.Assert( parent != null );
			Debug.Assert( name != null );
			Debug.Assert( name.Length > 0 );

			_device = device;
			_parent = parent;

			_name = name;
			_attributes = attributes;
			_timestamp = timestamp;
			_position = position;
			_length = length;

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

		public long Length
		{
			get
			{
				return _length;
			}
		}

		public Stream OpenRead()
		{
			return this.Open( MediaFileMode.Normal, MediaFileAccess.Read );
		}

		public Stream Open( MediaFileMode mode, MediaFileAccess access )
		{
			Debug.Assert( mode == MediaFileMode.Normal );
			if( mode != MediaFileMode.Normal )
				return null;

			Debug.Assert( access == MediaFileAccess.Read );
			if( access != MediaFileAccess.Read )
				return null;

			Stream stream = _device.OpenStream();
			stream.Seek( _position, SeekOrigin.Begin );

			// If the file is under 2mb, load it in to memory
			if( _length < MaximumCachableStreamSize )
			{
				byte[] buffer = new byte[ _length ];
				stream.Read( buffer, 0, buffer.Length );

				MemoryStream memoryStream = new MemoryStream( buffer, false );

				stream.Dispose();
				stream = null;

				return memoryStream;
			}

			return stream;
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

			// TODO: File copy
			throw new NotImplementedException();
		}

		public void Delete()
		{
		}
	}
}
