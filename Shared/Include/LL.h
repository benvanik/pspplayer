// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

#include <assert.h>
#include <malloc.h>

#ifndef NULL
#define NULL 0
#endif

namespace Noxa {
	namespace Emulation {
		namespace Psp {

			template<typename T>
			struct LLEntry
			{
				LLEntry<T>*		Next;
				LLEntry<T>*		Previous;
				T				Value;
			};

			template<typename T>
			class LL
			{
			private:
				LLEntry<T>*		_head;
				LLEntry<T>*		_tail;
				int				_count;

			public:
				LL();
				~LL();

				void InsertAtHead( T value );
				void Enqueue( T value );
				T Dequeue();
				T Pop();
				
				void InsertBefore( T value, LLEntry<T>* proceeding );
				void InsertAfter( T value, LLEntry<T>* preceeding );

				T PeekHead();
				T PeekTail();
				LLEntry<T>* Find( T value );
				LLEntry<T>* FindReverse( T value );

				void Remove( T value );
				void Remove( LLEntry<T>* entry );

				int GetCount(){ return _count; }
				LLEntry<T>* GetHead(){ return _head; }
				LLEntry<T>* GetTail(){ return _tail; }

				void Clear();
			};

			template<typename T>
			LL<T>::LL()
			{
				_head = NULL;
				_tail = NULL;
				_count = 0;
			}

			template<typename T>
			LL<T>::~LL()
			{
				this->Clear();
			}

			template<typename T>
			void LL<T>::InsertAtHead( T value )
			{
				LLEntry<T>* entry = ( LLEntry<T>* )malloc( sizeof( LLEntry<T> ) );
				entry->Next = _head;
				entry->Previous = NULL;
				entry->Value = value;
				_head = entry;
				if( _tail == NULL )
					_tail = entry;
				_count++;
			}

			template<typename T>
			void LL<T>::Enqueue( T value )
			{
				LLEntry<T>* entry = ( LLEntry<T>* )malloc( sizeof( LLEntry<T> ) );
				entry->Next = NULL;
				entry->Previous = _tail;
				entry->Value = value;
				_tail = entry;
				if( _head == NULL )
					_head = entry;
				_count++;
			}

			template<typename T>
			T LL<T>::Dequeue()
			{
				if( _head == NULL )
					return NULL;
				LLEntry<T>* entry = _head;
				_head = entry->Next;
				if( _head == NULL )
					_tail = NULL;
				T value = entry->Value;
				free( entry );
				_count--;
				return value;
			}

			template<typename T>
			T LL<T>::Pop()
			{
				if( _tail == NULL )
					return NULL;
				LLEntry<T>* entry = _tail;
				_tail = entry->Previous;
				if( _tail == NULL )
					_head = NULL;
				T value = entry->Value;
				free( entry );
				_count--;
				return value;
			}

			template<typename T>
			void LL<T>::InsertBefore( T value, LLEntry<T>* proceeding )
			{
				assert( proceeding != NULL );
				if( proceeding == NULL )
				{
					this->Enqueue( value );
					return;
				}

				LLEntry<T>* entry = ( LLEntry<T>* )malloc( sizeof( LLEntry<T> ) );
				entry->Previous = proceeding->Previous;
				entry->Next = proceeding;
				entry->Value = value;

				proceeding->Previous = entry;
				if( entry->Previous != NULL )
					entry->Previous->Next = entry;
				else
					_head = entry;

				_count++;
			}

			template<typename T>
			void LL<T>::InsertAfter( T value, LLEntry<T>* preceeding )
			{
				assert( preceeding != NULL );
				if( preceeding == NULL )
				{
					this->Enqueue( value );
					return;
				}

				LLEntry<T>* entry = ( LLEntry<T>* )malloc( sizeof( LLEntry<T> ) );
				entry->Previous = preceeding;
				entry->Next = preceeding->Next;
				entry->Value = value;

				preceeding->Next = entry;
				if( entry->Next != NULL )
					entry->Next->Previous = entry;
				else
					_tail = entry;

				_count++;
			}

			template<typename T>
			T LL<T>::PeekHead()
			{
				if( _head != NULL )
					return _head->Value;
				else
					return NULL;
			}

			template<typename T>
			T LL<T>::PeekTail()
			{
				if( _tail != NULL )
					return _tail->Value;
				else
					return NULL;
			}

			template<typename T>
			LLEntry<T>* LL<T>::Find( T value )
			{
				LLEntry<T>* e = _head;
				while( e != NULL )
				{
					if( e->Value == value )
						return e;
					e = e->Next;
				}
				return NULL;
			}

			template<typename T>
			LLEntry<T>* LL<T>::FindReverse( T value )
			{
				LLEntry<T>* e = _tail;
				while( e != NULL )
				{
					if( e->Value == value )
						return e;
					e = e->Previous;
				}
				return NULL;
			}

			template<typename T>
			void LL<T>::Remove( T value )
			{
				LLEntry<T>* entry = this->Find( value );
				if( entry == NULL )
					return;
				this->Remove( entry );
			}

			template<typename T>
			void LL<T>::Remove( LLEntry<T>* entry )
			{
				if( entry->Next != NULL )
					entry->Next->Previous = entry->Previous;
				else
					_tail = entry->Previous;
				if( entry->Previous != NULL )
					entry->Previous->Next = entry->Next;
				else
					_head = entry->Next;

				free( entry );

				_count--;
			}

			template<typename T>
			void LL<T>::Clear()
			{
				LLEntry<T>* entry = _head;
				while( entry != NULL )
				{
					LLEntry<T>* next = entry->Next;
					free( entry );
					entry = next;
				}
				_head = NULL;
				_tail = NULL;
				_count = 0;
			}

		}
	}
}
