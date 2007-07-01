// ----------------------------------------------------------------------------
// Shared Utility Library
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Noxa
{
	public class RangedLinkedList<T>
	{
		private Range _bounds;
		private RLLMeta<T>[] _metas;
		private RLLEntry<T> _head;

		private const int MainSplit = 0xFFF;

		public RangedLinkedList( Range bounds )
		{
			_bounds = bounds;

			int maxMetas = Math.Min( bounds.Size, MainSplit );
			int offset = bounds.Offset;
			int size = bounds.Size / maxMetas;
			_metas = new RLLMeta<T>[ maxMetas ];
			for( int n = 0; n < maxMetas; n++ )
			{
				_metas[ n ] = new RLLMeta<T>( new Range( offset, size ) );
				offset += size;
			}
		}

		public void Add( Range range, T value )
		{
			RLLEntry<T> e = new RLLEntry<T>( range, value );
			if( _head != null )
			{
				if( _head.Range.Offset > range.Offset )
				{
					e.Next = _head;
					_head.Previous = e;
					_head = e;
				}
			}
			else
				_head = e;

			int metaIndex = ( range.Offset / _metas.Length );
			int metaRange = Math.Max( 1, e.Range.Size / _metas[ 0 ].Range.Size );
			for( int n = metaIndex; n < metaIndex + metaRange; n++ )
			{
				RLLMeta<T> meta = _metas[ n ];
				if( meta.Range.Extents < range.Offset )
					continue;
				if( meta.Range.Offset > range.Extents )
					break;
				if( meta.Target != null )
				{
					if( meta.Target.Range.Offset > range.Offset )
						meta.Target = e;
				}
				else
					meta.Target = e;
			}

			if( _head != e )
			{
				RLLEntry<T> ee = _head;
				while( ee != null )
				{
					if( ee.Range.Offset > range.Offset )
					{
						e.Next = ee;
						e.Previous = ee.Previous;
						ee.Previous = e;
						if( e.Previous != null )
							e.Previous.Next = e;
						break;
					}
					if( ee.Next == null )
					{
						e.Previous = ee;
						ee.Next = e;
						break;
					}
					ee = ee.Next;
				}
			}
		}

		public void Remove( int index )
		{
			RLLEntry<T> e = this.Find( index );
			if( e == null )
				return;

			if( e.Previous != null )
				e.Previous.Next = e.Next;
			else
				_head = e.Next;
			if( e.Next != null )
				e.Next.Previous = e.Previous;

			int metaIndex = ( index / _metas.Length );
			int metaRange = e.Range.Size / _metas[ 0 ].Range.Size;
			for( int n = metaIndex - metaRange; n < metaIndex + metaRange; n++ )
			{
				if( _metas[ n ].Target == e )
				{
					if( ( e.Next != null ) &&
						( _metas[ n ].Range.Contains( e.Next.Range.Offset ) == true ) )
						_metas[ n ].Target = e.Next;
					else
						_metas[ n ].Target = null;
				}
			}
		}

		public bool Contains( int index )
		{
			RLLEntry<T> e = this.Find( index );
			return ( e != null );
		}

		public T this[ int index ]
		{
			get
			{
				RLLEntry<T> e = this.Find( index );
				if( e == null )
					return default( T );
				else
					return e.Value;
			}
		}

		public void Clear()
		{
			_head = null;
			for( int n = 0; n < _metas.Length; n++ )
				_metas[ n ].Target = null;
		}

		private RLLEntry<T> Find( int index )
		{
			RLLMeta<T> meta = this.FindMeta( index );
			RLLEntry<T> e = meta.Target;
			while( e != null )
			{
				if( e.Range.Extents < index )
					continue;
				if( e.Range.Offset > index )
					break;
				Debug.Assert( e.Range.Contains( index ) == true );
				return e;
			}
			return null;
		}

		private RLLMeta<T> FindMeta( int index )
		{
			Debug.Assert( _bounds.Contains( index ) == true );
			int metaIndex = ( index / _metas.Length );
			Debug.Assert( _metas[ metaIndex ].Range.Contains( index ) == true );
			return _metas[ metaIndex ];
		}

		private class RLLMeta<U>
		{
			public readonly Range Range;
			public RLLEntry<U> Target;
			public RLLMeta( Range range )
			{
				this.Range = range;
			}
		}

		private class RLLEntry<U>
		{
			public readonly Range Range;
			public readonly U Value;
			public RLLEntry<U> Previous;
			public RLLEntry<U> Next;
			public RLLEntry( Range range, U value )
			{
				this.Range = range;
				this.Value = value;
			}
		}
	}
}
