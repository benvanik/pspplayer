// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2008 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace Be.Windows.Forms
{
	public unsafe class MappedByteProvider : IByteProvider
	{
		private byte* _buffer;
		private uint _startAddress;
		private uint _length;

		private bool _hasChanges;

		public MappedByteProvider( byte* buffer, uint startAddress, uint length )
		{
			_buffer = buffer;
			_startAddress = startAddress;
			_length = length;
		}

		#region IByteProvider Members

		public byte ReadByte( long index )
		{
			return *( _buffer + index );
		}

		public void WriteByte( long index, byte value )
		{
			*( _buffer + index ) = value;
			this.OnChanged( EventArgs.Empty );
		}

		public void InsertBytes( long index, byte[] bs )
		{
			throw new NotImplementedException();
		}

		public void DeleteBytes( long index, long length )
		{
			throw new NotImplementedException();
		}

		public long Length { get { return _length; } }

		public long Offset { get { return _startAddress; } }

		public event EventHandler LengthChanged;
		public event EventHandler Changed;

		private void OnChanged( EventArgs e )
		{
			_hasChanges = true;
			if( Changed != null )
				Changed( this, e );
		}

		private void OnLengthChanged( EventArgs e )
		{
			if( LengthChanged != null )
				LengthChanged( this, e );
		}

		public bool HasChanges()
		{
			return _hasChanges;
		}

		public void ApplyChanges()
		{
			_hasChanges = false;
		}

		public bool SupportsWriteByte()
		{
			return true;
		}

		public bool SupportsInsertBytes()
		{
			return false;
		}

		public bool SupportsDeleteBytes()
		{
			return false;
		}

		#endregion
	}
}
