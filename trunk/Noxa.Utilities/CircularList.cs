// ----------------------------------------------------------------------------
// Shared Utility Library
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Noxa
{
	/// <summary>
	/// A circular list.
	/// </summary>
	/// <typeparam name="T">The list element type.</typeparam>
	public class CircularList<T> : IEnumerable<T>
	{
		private bool _allowOverwriting;
		private T[] _list;
		private int _index;
		private int _length;

		/// <summary>
		/// Initializes a new CircularList with the given parameters.
		/// </summary>
		/// <param name="capacity">The maximum capacity of the list.</param>
		/// <param name="allowOverwriting"><c>true</c> to allow overwriting of list elements.</param>
		public CircularList( int capacity, bool allowOverwriting )
		{
			_allowOverwriting = allowOverwriting;
			_list = new T[ capacity ];
		}

		/// <summary>
		/// Get the current count of items in the list.
		/// </summary>
		public int Count
		{
			get
			{
				return _length;
			}
		}

		/// <summary>
		/// Remove all items from the list.
		/// </summary>
		public void Clear()
		{
			_index = 0;
			_length = 0;
		}

		/// <summary>
		/// Get an enumerator over the list.
		/// </summary>
		/// <returns>An enumerator over the items in the list.</returns>
		public IEnumerator<T> GetEnumerator()
		{
			int startIndex = _index - _length;
			if( startIndex < 0 )
				startIndex += _list.Length;
			for( int n = startIndex; n < startIndex + _length; n++ )
			{
				yield return _list[ n % _list.Length ];
			}
		}

		/// <summary>
		/// Not implemented.
		/// </summary>
		/// <returns>An exception!</returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Add a new item to the list.
		/// </summary>
		/// <param name="item">The item to add.</param>
		/// <returns>The index of the new item.</returns>
		public int Enqueue( T item )
		{
			if( _length + 1 > _list.Length )
			{
				if( _allowOverwriting == false )
					throw new InvalidOperationException( "List is full." );
			}
			int target = _index % _list.Length;
			_index = ( _index + 1 ) % _list.Length;
			_length = Math.Min( _length + 1, _list.Length );
			_list[ target ] = item;
			return target;
		}

		/// <summary>
		/// Remove an item from the head of the list.
		/// </summary>
		/// <returns></returns>
		public T Dequeue()
		{
			if( _length == 0 )
				return default( T );

			int startIndex = ( _index - _length );
			if( startIndex < 0 )
				startIndex += _list.Length;
			T item = _list[ startIndex ];
			_list[ startIndex ] = default( T );

			_length--;

			return item;
		}

		/// <summary>
		/// Remove an item from the tail of the list.
		/// </summary>
		/// <returns>The item removed.</returns>
		public T Pop()
		{
			if( _length == 0 )
				return default( T );

			_index = ( _index - 1 ) % _list.Length;
			_length--;

			T item = _list[ _index ];
			_list[ _index ] = default( T );

			return item;
		}

		/// <summary>
		/// Return the item at the head of the list.
		/// </summary>
		/// <returns>The item at the head of the list.</returns>
		public T PeekHead()
		{
			if( _length == 0 )
				return default( T );
			int startIndex = ( _index - _length );
			if( startIndex < 0 )
				startIndex += _list.Length;
			return _list[ startIndex ];
		}

		/// <summary>
		/// Return the item at the tail of the list.
		/// </summary>
		/// <returns>The item at the tail of the list.</returns>
		public T PeekTail()
		{
			if( _length == 0 )
				return default( T );
			return _list[ ( _index - 1 ) % _list.Length ];
		}

		/// <summary>
		/// Return the item at a specific offset in the list.
		/// </summary>
		/// <param name="offset">The offset, from the head, to look at.</param>
		/// <returns>The item at the given offset in the list.</returns>
		public T PeekAhead( int offset )
		{
			if( _length == 0 )
				return default( T );
			int startIndex = ( _index - _length );
			if( startIndex < 0 )
				startIndex += _list.Length;
			startIndex = ( startIndex + offset ) % _list.Length;
			return _list[ startIndex ];
		}
	}
}
