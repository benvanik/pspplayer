using System;
using System.Collections.Generic;
using System.Text;

namespace Noxa.Emulation.Psp.IO.Media.Iso
{
	class MediaFolder : IMediaFolder
	{
		public IMediaDevice Device
		{
			get
			{
				return null;
			}
		}

		public IMediaItem[] Items
		{
			get
			{
				throw new Exception( "The method or operation is not implemented." );
			}
		}

		public IMediaItem this[ string name ]
		{
			get
			{
				throw new Exception( "The method or operation is not implemented." );
			}
		}

		public IMediaItem Find( string path )
		{
			throw new NotImplementedException();
		}

		public IMediaFolder FindFolder( string path )
		{
			throw new Exception( "The method or operation is not implemented." );
		}

		public IMediaFile FindFile( string path )
		{
			throw new Exception( "The method or operation is not implemented." );
		}

		public IMediaItem CreateSymbolicLink( string name, MediaItemType type )
		{
			throw new Exception( "The method or operation is not implemented." );
		}

		public IMediaFolder CreateFolder( string name )
		{
			throw new Exception( "The method or operation is not implemented." );
		}

		public IMediaFile CreateFile( string name )
		{
			throw new Exception( "The method or operation is not implemented." );
		}

		public bool MoveItem( string name, IMediaFolder destination )
		{
			throw new Exception( "The method or operation is not implemented." );
		}

		public bool CopyItem( string name, IMediaFolder destination )
		{
			throw new Exception( "The method or operation is not implemented." );
		}

		public void DeleteItem( string name )
		{
			throw new Exception( "The method or operation is not implemented." );
		}

		public string Name
		{
			get
			{
				throw new Exception( "The method or operation is not implemented." );
			}
			set
			{
				throw new Exception( "The method or operation is not implemented." );
			}
		}

		public IMediaFolder Parent
		{
			get
			{
				throw new Exception( "The method or operation is not implemented." );
			}
		}

		public string AbsolutePath
		{
			get
			{
				throw new Exception( "The method or operation is not implemented." );
			}
		}

		public MediaItemAttributes Attributes
		{
			get
			{
				throw new Exception( "The method or operation is not implemented." );
			}
			set
			{
				throw new Exception( "The method or operation is not implemented." );
			}
		}

		public DateTime CreationTime
		{
			get
			{
				throw new Exception( "The method or operation is not implemented." );
			}
			set
			{
				throw new Exception( "The method or operation is not implemented." );
			}
		}

		public DateTime ModificationTime
		{
			get
			{
				throw new Exception( "The method or operation is not implemented." );
			}
			set
			{
				throw new Exception( "The method or operation is not implemented." );
			}
		}

		public DateTime AccessTime
		{
			get
			{
				throw new Exception( "The method or operation is not implemented." );
			}
			set
			{
				throw new Exception( "The method or operation is not implemented." );
			}
		}

		public bool IsSymbolicLink
		{
			get
			{
				throw new Exception( "The method or operation is not implemented." );
			}
		}

		public bool MoveTo( IMediaFolder destination )
		{
			throw new Exception( "The method or operation is not implemented." );
		}

		public bool CopyTo( IMediaFolder destination )
		{
			throw new Exception( "The method or operation is not implemented." );
		}

		public void Delete()
		{
			throw new Exception( "The method or operation is not implemented." );
		}
		
		public IEnumerator<IMediaItem> GetEnumerator()
		{
			throw new Exception( "The method or operation is not implemented." );
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			throw new Exception( "The method or operation is not implemented." );
		}
	}
}
