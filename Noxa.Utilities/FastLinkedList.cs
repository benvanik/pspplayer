// ----------------------------------------------------------------------------
// Shared Utility Library
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace Noxa
{
	public class LinkedListEntry<T>
	{
		public LinkedListEntry<T> Next;
		public LinkedListEntry<T> Previous;
		public T Value;
	}

	public class FastLinkedList<T>
	{
		private LinkedListEntry<T> _head;
		private LinkedListEntry<T> _tail;
		private int _count;

		public FastLinkedList()
		{
		}

		public T Head
		{
			get
			{
				if( _head == null )
					return default( T );
				return _head.Value;
			}
		}

		public T Tail
		{
			get
			{
				if( _tail == null )
					return default( T );
				return _tail.Value;
			}
		}

		public LinkedListEntry<T> HeadEntry
		{
			get
			{
				return _head;
			}
		}

		public LinkedListEntry<T> TailEntry
		{
			get
			{
				return _tail;
			}
		}

		public int Count
		{
			get
			{
				return _count;
			}
		}

		public LinkedListEntry<T> Enqueue( T value )
		{
			LinkedListEntry<T> entry = new LinkedListEntry<T>();
			entry.Value = value;
			entry.Previous = _tail;
			_tail = entry;
			if( _head == null )
				_head = entry;
			_count++;
			return entry;
		}

		public LinkedListEntry<T> InsertAtHead( T value )
		{
			LinkedListEntry<T> entry = new LinkedListEntry<T>();
			entry.Value = value;
			entry.Next = _head;
			_head = entry;
			if( _tail == null )
				_tail = entry;
			_count++;
			return entry;
		}

		public LinkedListEntry<T> InsertBefore( T value, LinkedListEntry<T> proceeding )
		{
			LinkedListEntry<T> entry = new LinkedListEntry<T>();
			entry.Value = value;
			entry.Next = proceeding;
			entry.Previous = proceeding.Previous;
			proceeding.Previous = entry;
			if( entry.Previous == null )
				_head = entry;
			_count++;
			return entry;
		}

		public LinkedListEntry<T> InsertAfter( T value, LinkedListEntry<T> preceeding )
		{
			LinkedListEntry<T> entry = new LinkedListEntry<T>();
			entry.Value = value;
			entry.Next = preceeding.Next;
			entry.Previous = preceeding;
			preceeding.Next = entry;
			if( entry.Next == null )
				_tail = entry;
			_count++;
			return entry;
		}

		public T Dequeue()
		{
			LinkedListEntry<T> entry = _head;
			if( entry == null )
				return default( T );
			_head = entry.Next;
			if( entry.Next != null )
				entry.Next.Previous = null;
			else
				_tail = null;
			_count--;
			return entry.Value;
		}

		public void Remove( T value )
		{
			LinkedListEntry<T> entry = this.Find( value );
			if( entry != null )
				this.Remove( entry );
		}

		public void Remove( LinkedListEntry<T> entry )
		{
			if( entry.Previous == null )
				_head = entry.Next;
			else
				entry.Previous.Next = entry.Next;
			if( entry.Next == null )
				_tail = entry.Previous;
			else
				entry.Next.Previous = entry.Previous;
			_count--;
		}

		public void Clear()
		{
			_head = null;
			_tail = null;
			_count = 0;
		}

		public LinkedListEntry<T> Find( T value )
		{
			LinkedListEntry<T> entry = _head;
			while( entry != null )
			{
				if( entry.Value.Equals( value ) )
					return entry;
				entry = entry.Next;
			}
			return null;
		}
	}
}
