// ----------------------------------------------------------------------------
// Shared Utility Library
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;

namespace Noxa
{
	public class CircularList<T>
	{
		protected T[] _list;
		protected int _index;
		protected int _length;

		public CircularList( int capacity )
		{
			_list = new T[ capacity ];
		}

		public int Count
		{
			get
			{
				return _length;
			}
		}

		public void Clear()
		{
			_index = 0;
			_length = 0;
		}

		public int Add( T item )
		{
			int target = _index + _length;
			if( _length + 1 > _list.Length )
			{
				_index++;
				_index %= _list.Length;
			}
			else
			{
				_length++;
			}
			target %= _list.Length;
			_list[ target ] = item;
			return target;
		}

		public T Dequeue()
		{
			if( _length == 0 )
				return default( T );
			_length--;

			T item = _list[ _index ];

			_index++;
			_index %= _list.Length;

			return item;
		}

		public T Peek()
		{
			if( _length == 0 )
				return default( T );
			return _list[ _index ];
		}

		public T PeekAhead( int offset )
		{
			if( _length == 0 )
				return default( T );
			int index = ( _index + offset ) % _list.Length;
			return _list[ index ];
		}
	}
}
