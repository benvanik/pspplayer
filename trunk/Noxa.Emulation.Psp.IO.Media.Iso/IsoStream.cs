using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace Noxa.Emulation.Psp.IO.Media.Iso
{
	class IsoStream : Stream
	{
		protected Stream _innerStream;
		protected long _basePosition;
		protected long _length;

		internal IsoStream( Stream innerStream, long basePosition, long length )
		{
			Debug.Assert( innerStream != null );
			_innerStream = innerStream;
			_basePosition = basePosition;
			_length = length;
		}

		public override bool CanRead
		{
			get
			{
				return true;
			}
		}

		public override bool CanSeek
		{
			get
			{
				return true;
			}
		}

		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		public override void Flush()
		{
		}

		public override long Length
		{
			get
			{
				return _length;
			}
		}

		public override long Position
		{
			get
			{
				return _innerStream.Position - _basePosition;
			}
			set
			{
				this.Seek( value, SeekOrigin.Begin );
			}
		}

		public override int Read( byte[] buffer, int offset, int count )
		{
			long final = _innerStream.Position + count;
			long end = _basePosition + _length;
			if( final > end )
				count = ( int )( end - _innerStream.Position );
			return _innerStream.Read( buffer, offset, count );
		}

		public override long Seek( long offset, SeekOrigin origin )
		{
			switch( origin )
			{
				case SeekOrigin.Current:
					return _innerStream.Seek( offset, SeekOrigin.Current ) - _basePosition;
				case SeekOrigin.Begin:
					return _innerStream.Seek( _basePosition + offset, SeekOrigin.Begin ) - _basePosition;
				case SeekOrigin.End:
					long end = _basePosition + _length;
					long target = end + offset;
					return _innerStream.Seek( target, SeekOrigin.Begin ) - _basePosition;
			}

			return 0;
		}

		public override void SetLength( long value )
		{
		}

		public override void Write( byte[] buffer, int offset, int count )
		{
		}
	}
}
