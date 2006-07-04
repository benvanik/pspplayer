using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Noxa.Emulation.Psp.IO.Media.Iso
{
	class MediaFile : IMediaFile
	{
		public long Length
		{
			get
			{
				throw new Exception( "The method or operation is not implemented." );
			}
		}

		public IMediaDevice Device
		{
			get
			{
				return null;
			}
		}

		public Stream OpenRead()
		{
			throw new Exception( "The method or operation is not implemented." );
		}

		public Stream Open( MediaFileMode mode, MediaFileAccess access )
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
	}
}
