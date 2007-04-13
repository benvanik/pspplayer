// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

//#define WIN32_LEAN_AND_MEAN
//#include <windows.h>
#include <assert.h>
#include <malloc.h>

#ifndef NULL
#define NULL 0
#endif

// Removes checks - make sure things don't assert!
#ifdef _DEBUG
#define CHECKEDVECTOR
#endif

namespace Noxa {
	namespace Emulation {
		namespace Psp {

			template<typename T>
			struct VectorEntry
			{
				bool			Valid;
				VectorEntry<T>*	Next;
				T				Value;
			};

			template<typename T>
			class Vector
			{
			private:
				VectorEntry<T>*	_list;
				int				_capacity;
				int				_used;
				int				_index;
				int				_firstFree;

			public:
				Vector( int initialCapacity );
				~Vector();

				T* Add( int* newIndex = 0 );
				int Add( T value );
				T* Get( int index );
				T GetValue( int index );
				void Remove( T value );
				T Remove( int index );
				void Clear();

				int IndexOf( T value );
				T* Enumerate( void** state );

				__inline int GetCapacity(){ return _capacity; }
				__inline int GetCount(){ return _used; }

			private:
				bool Expand( int newLength );
				void Compress();
				VectorEntry<T>* FindPrevious( int index );
				VectorEntry<T>* FindNext( int index );
				void FixupList( int index );
			};

			template<typename T>
			Vector<T>::Vector( int initialCapacity )
			{
				_list = NULL;

				if( initialCapacity < 10 )
					initialCapacity = 10;
				_capacity = -1;

				bool ok = this->Expand( initialCapacity );
				assert( ok == true );
			}

			template<typename T>
			Vector<T>::~Vector()
			{
				SAFEFREE( _list );
				_capacity = -1;
				_index = 0;
				_used = 0;
				_firstFree = -1;
			}

			template<typename T>
			T* Vector<T>::Add( int* newIndex )
			{
				assert( _capacity > 0 );
			#ifdef CHECKEDVECTOR
				if( _capacity <= 0 )
					return NULL;
			#endif

				// If we have a free index defined, try to use it
				int index = _firstFree;
				if( index >= 0 )
				{
					assert( _list[ index ].Valid == false );
			#ifdef CHECKEDVECTOR
					if( _list[ index ].Valid == false )
						index = -1;
			#endif
					_firstFree = -1;

					// Fixup list
					this->FixupList( index );

					_used++;
					if( newIndex != NULL )
						*newIndex = index;
					_list[ index ].Valid = true;
					return &_list[ index ].Value;
				}

				// Look in the next free slot
				if( _index < 0 )
				{
					// Hit the end of the list on the last add
					if( _used < _capacity )
					{
						// Have some free space within us - compress
						this->Compress();
						assert( _index > 0 );
						assert( _index < _capacity );
					}
					else
					{
						// We ran out of room - grow
						bool ok = this->Expand( _capacity * 2 );
						assert( ok == true );
						if( ok == false )
							return NULL;
					}
				}
				assert( _list[ _index ].Valid == false );

				index = _index;
				_index++;
				if( _index >= _capacity )
					_index = -1;

				// Fixup list
				this->FixupList( index );

				_used++;
				if( newIndex != NULL )
					*newIndex = index;
				_list[ index ].Valid = true;
				return &_list[ index ].Value;
			}

			template<typename T>
			int Vector<T>::Add( T value )
			{
				int newIndex;
				T* ptr = this->Add( &newIndex );
				assert( ptr != NULL );
				if( ptr == NULL )
					return -1;
				*ptr = value;
				return newIndex;
			}

			template<typename T>
			T* Vector<T>::Get( int index )
			{
				assert( _capacity > 0 );
			#ifdef CHECKEDVECTOR
				if( _capacity <= 0 )
					return NULL;
			#endif

				assert( index >= 0 );
				assert( index < _capacity );
			#ifdef CHECKEDVECTOR
				if( ( index < 0 ) ||
					( index >= _capacity ) )
					return NULL;
			#endif

				if( _list[ index ].Valid == false )
					return NULL;

				return &_list[ index ].Value;
			}

			template<typename T>
			T Vector<T>::GetValue( int index )
			{
				T* pointer = this->Get( index );
				if( pointer == NULL )
					return NULL;
				return *pointer;
			}

			template<typename T>
			void Vector<T>::Remove( T value )
			{
				int index = this->IndexOf( value );
				if( index >= 0 )
					this->Remove( index );
			}

			template<typename T>
			T Vector<T>::Remove( int index )
			{
				assert( _capacity > 0 );
			#ifdef CHECKEDVECTOR
				if( _capacity <= 0 )
					return NULL;
			#endif

				assert( index >= 0 );
				assert( index < _capacity );
			#ifdef CHECKEDVECTOR
				if( ( index < 0 ) ||
					( index >= _capacity ) )
					return NULL;
			#endif

				if( _list[ index ].Valid == false )
					return NULL;

				// Find the previous entry
				VectorEntry<T>* previous = this->FindPrevious( index );

				VectorEntry<T>* entry = &_list[ index ];
				if( previous == entry )
					previous = NULL;
				entry->Valid = false;
				entry->Next = NULL;

				// Hook previous to next
				if( previous != NULL )
					previous->Next = entry->Next;

				if( index < _firstFree )
					_firstFree = index;

				if( _index = index + 1 )
					_index--;

				_used--;

				return entry->Value;
			}

			template<typename T>
			void Vector<T>::Clear()
			{
				assert( _capacity > 0 );
			#ifdef CHECKEDVECTOR
				if( _capacity <= 0 )
					return;
			#endif

				memset( _list, 0, sizeof( VectorEntry<T> ) * _capacity );

				_index = 0;
				_used = 0;
				_firstFree = -1;
			}

			template<typename T>
			int Vector<T>::IndexOf( T value )
			{
				if( _used == 0 )
					return -1;

				VectorEntry<T>* e = this->FindNext( -1 );
				assert( e != NULL );
				while( e != NULL )
				{
					if( e->Value == value )
					{
						// .... sigh
						return 0;
					}
					e = e->Next;
				}

				return -1;
			}

			template<typename T>
			T* Vector<T>::Enumerate( void** state )
			{
				assert( state != NULL );
			#ifdef CHECKEDVECTOR
				if( state == NULL )
					return NULL;
			#endif

				if( _used == 0 )
					return NULL;

				if( *state == NULL )
				{
					// First iteration
					VectorEntry<T>* entry = this->FindNext( -1 );
					assert( entry != NULL );
					*state = ( void* )entry;
					return &entry->Value;
				}
				else
				{
					VectorEntry<T>* previous = ( VectorEntry<T>* )( *state );
					VectorEntry<T>* entry = previous->Next;
					if( entry == NULL )
					{
						*state = NULL;
						return NULL;
					}
					else
					{
						*state = entry;
						return &entry->Value;
					}
				}
			}

			template<typename T>
			bool Vector<T>::Expand( int newLength )
			{
				assert( newLength > 0 );
				if( _capacity <= 0 )
				{
					// Startup case - easy
					_list = ( VectorEntry<T>* )calloc( 1, sizeof( VectorEntry<T> ) * newLength );
					assert( _list != NULL );
					_capacity = newLength;

					_index = 0;
					_used = 0;
					_firstFree = -1;

					return ( _list != NULL );
				}
				else
				{
					// Expand list

					// NOT IMPLEMENTED
					assert( false );

					return false;
				}
			}

			template<typename T>
			void Vector<T>::Compress()
			{
				// Uh oh - compression is totally wrong - it fucks up all the indicies.... need to do something else
				assert( false );
				return;

				// Walk the linked list and place in that order
				VectorEntry<T>* entry = this->FindNext( -1 );
				if( entry == NULL )
					return;
				VectorEntry<T>* previous = NULL;

				_index = 0;
				while( entry->Next != NULL )
				{
					if( &_list[ _index ] != entry )
					{
						memcpy( &_list[ _index ], entry, sizeof( VectorEntry<T> ) );
						entry->Valid = false;
						entry = &_list[ _index ];
						if( previous != NULL )
							previous->Next = entry;
					}

					_index++;
					previous = entry;
					entry = entry->Next;
				}
			}

			template<typename T>
			VectorEntry<T>* Vector<T>::FindPrevious( int index )
			{
				for( int n = index - 1; index >= 0; index-- )
				{
					if( _list[ n ].Valid == true )
						return &_list[ n ];
				}
				return NULL;
			}

			template<typename T>
			VectorEntry<T>* Vector<T>::FindNext( int index )
			{
				for( int n = index + 1; index < _index; index++ )
				{
					if( _list[ n ].Valid == true )
						return &_list[ n ];
				}
				return NULL;
			}

			template<typename T>
			void Vector<T>::FixupList( int index )
			{
				VectorEntry<T>* previous = this->FindPrevious( index );
				VectorEntry<T>* next = this->FindNext( index );
				if( previous != NULL )
					previous->Next = &_list[ index ];
				_list[ index ].Next = next;
			}

		}
	}
}
