using System;
using System.Collections.Generic;
using System.Text;

namespace Noxa
{
	public class ReadOnlyDictionary<TKey,TValue> :IDictionary<TKey,TValue>
	{
		protected Dictionary<TKey, TValue> _internal;

		public ReadOnlyDictionary( Dictionary<TKey, TValue> source )
		{
			_internal = new Dictionary<TKey, TValue>( source );
		}

		#region IDictionary<TKey,TValue> Members

		public void Add( TKey key, TValue value )
		{
		}

		public bool ContainsKey( TKey key )
		{
			return _internal.ContainsKey( key );
		}

		public ICollection<TKey> Keys
		{
			get
			{
				return _internal.Keys;
			}
		}

		public bool Remove( TKey key )
		{
			return false;
		}

		public bool TryGetValue( TKey key, out TValue value )
		{
			return _internal.TryGetValue( key, out value );
		}

		public ICollection<TValue> Values
		{
			get
			{
				return _internal.Values;
			}
		}

		public TValue this[ TKey key ]
		{
			get
			{
				return _internal[ key ];
			}
			set
			{
			}
		}

		#endregion

		#region ICollection<KeyValuePair<TKey,TValue>> Members

		public void Add( KeyValuePair<TKey, TValue> item )
		{
		}

		public void Clear()
		{
		}

		public bool Contains( KeyValuePair<TKey, TValue> item )
		{
			return ( ( ICollection<KeyValuePair<TKey, TValue>> )_internal ).Contains( item );
		}

		public void CopyTo( KeyValuePair<TKey, TValue>[] array, int arrayIndex )
		{
			( ( ICollection<KeyValuePair<TKey, TValue>> )_internal ).CopyTo( array, arrayIndex );
		}

		public int Count
		{
			get
			{
				return _internal.Count;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return true;
			}
		}

		public bool Remove( KeyValuePair<TKey, TValue> item )
		{
			return false;
		}

		#endregion

		#region IEnumerable<KeyValuePair<TKey,TValue>> Members

		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			return _internal.GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return _internal.GetEnumerator();
		}

		#endregion
	}
}
