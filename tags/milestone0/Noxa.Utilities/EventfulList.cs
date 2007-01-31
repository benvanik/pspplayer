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
	public class EventfulListEventArgs<T> : EventArgs
	{
		private T _item;
		private int _index;

		public EventfulListEventArgs( T item )
		{
			_item = item;
			_index = -1;
		}

		public EventfulListEventArgs( T item, int index )
		{
			_item = item;
			_index = index;
		}

		public T Item
		{
			get
			{
				return _item;
			}
		}

		public int Index
		{
			get
			{
				return _index;
			}
		}
	}

	/// <summary>
	/// Generic list that fires events on most actions and allows some more control over the collection.
	/// </summary>
	/// <typeparam name="T">Type of item being stored.</typeparam>
	/// <remarks>
	/// Written by ben -at- vanik.net (http://www.noxa.org).
	/// A similar class is present in the WinFX SDK; perhaps I'll rip out theirs.
	/// </remarks>
	public class EventfulList<T> : System.Collections.ObjectModel.Collection<T>
	{
		public event EventHandler ClearingItems;
		public event EventHandler ClearedItems;

		public event EventHandler<EventfulListEventArgs<T>> RemovingItem;
		public event EventHandler<EventfulListEventArgs<T>> RemovedItem;

		public event EventHandler<EventfulListEventArgs<T>> InsertingItem;
		public event EventHandler<EventfulListEventArgs<T>> InsertedItem;

		public event EventHandler<EventfulListEventArgs<T>> ChangingItem;
		public event EventHandler<EventfulListEventArgs<T>> ChangedItem;

		protected bool _initialized = false;
		protected bool _suppressEvents = false;
		protected bool _allowChanging = true;
		protected bool _allowRemoval = true;

		public bool IsInitialized
		{
			get
			{
				return _initialized;
			}
			set
			{
				_initialized = value;
			}
		}

		public bool SuppressEvents
		{
			get
			{
				return _suppressEvents;
			}
			set
			{
				_suppressEvents = value;
			}
		}

		public bool AllowChanging
		{
			get
			{
				return _allowChanging;
			}
			set
			{
				_allowChanging = false;
			}
		}

		public bool AllowRemoval
		{
			get
			{
				return _allowRemoval;
			}
			set
			{
				_allowRemoval = false;
			}
		}

		protected override void ClearItems()
		{
			if( _allowRemoval == false )
				return;

			EventHandler handler;

			if( _suppressEvents == false )
			{
				handler = this.ClearingItems;
				if( handler != null )
					handler( this, EventArgs.Empty );
			}

			base.ClearItems();

			if( _suppressEvents == false )
			{
				handler = this.ClearedItems;
				if( handler != null )
					handler( this, EventArgs.Empty );
			}
		}

		protected override void InsertItem( int index, T item )
		{
			EventHandler<EventfulListEventArgs<T>> handler;

			if( _suppressEvents == false )
			{
				handler = this.InsertingItem;
				if( handler != null )
					handler( this, new EventfulListEventArgs<T>( item, index ) );
			}
			
			base.InsertItem( index, item );

			if( _suppressEvents == false )
			{
				handler = this.InsertedItem;
				if( handler != null )
					handler( this, new EventfulListEventArgs<T>( item, index ) );
			}
		}

		protected override void RemoveItem( int index )
		{
			if( _allowRemoval == false )
				return;

			EventHandler<EventfulListEventArgs<T>> handler;
			T item = default( T );

			if( _suppressEvents == false )
			{
				// Here so that when suppressed speed is not impacted
				item = base[ index ];

				handler = this.RemovingItem;
				if( handler != null )
					handler( this, new EventfulListEventArgs<T>( item, index ) );
			}

			base.RemoveItem( index );

			if( _suppressEvents == false )
			{
				handler = this.RemovedItem;
				if( handler != null )
					handler( this, new EventfulListEventArgs<T>( item, index ) );
			}
		}

		protected override void SetItem( int index, T item )
		{
			if( _allowChanging == false )
				return;

			EventHandler<EventfulListEventArgs<T>> handler;

			if( _suppressEvents == false )
			{
				handler = this.ChangingItem;
				if( handler != null )
					handler( this, new EventfulListEventArgs<T>( item, index ) );
			}

			base.SetItem( index, item );

			if( _suppressEvents == false )
			{
				handler = this.ChangedItem;
				if( handler != null )
					handler( this, new EventfulListEventArgs<T>( item, index ) );
			}
		}
	}
}
