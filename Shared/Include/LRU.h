// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

#include <assert.h>
#include <malloc.h>
#include <hash_map>
#include "LL.h"

#ifndef NULL
#define NULL 0
#endif

namespace Noxa {
	namespace Emulation {
		namespace Psp {

			template<typename T>
			class LRU
			{
			private:
				// I'm lame and actually keep two lookups - one for key -> entry and one for entry -> key
				stdext::hash_map<uint, LLEntry<T>*>	_lookup;
				stdext::hash_map<LLEntry<T>*, uint> _keyLookup;
				LL<T>				_list;
				int					_count;
				int					_maxCount;

			public:
				LRU( int maxCount );
				~LRU();

				void Add( uint key, T value );
				void Remove( uint key );
				T Find( uint key );

				int GetCount(){ return _count; }
				int GetMaxCount(){ return _maxCount; }

				void Clear();
			};

			template<typename T>
			LRU<T>::LRU( int maxCount )
			{
				_count = 0;
				_maxCount = maxCount;
			}

			template<typename T>
			LRU<T>::~LRU()
			{
				this->Clear();
				_count = 0;
				_maxCount = 0;
			}

			template<typename T>
			void LRU<T>::Add( uint key, T value )
			{
				if( _count + 1 > _maxCount )
				{
					// Evict the last entry in the list
					LLEntry<T>* dead = _list.GetTail();

					// Need to remove from hash!
					uint oldKey = _keyLookup[ dead ];
					_lookup.erase( oldKey );
					_keyLookup.erase( dead );

					//_list.Remove( dead );
					_count--;
				}

				_count++;
				_list.InsertAtHead( value );
				LLEntry<T>* entry = _list.GetHead();
				_lookup[ key ] = entry;
				_keyLookup[ entry ] = key;
			}

			template<typename T>
			void LRU<T>::Remove( uint key )
			{
				LLEntry<T>* entry = _lookup[ key ];
				if( entry == NULL )
					return;

				_lookup.erase( key );
				_keyLookup.erase( entry );
				
				_count--;
				//_list.Remove( entry );
			}

			template<typename T>
			T LRU<T>::Find( uint key )
			{
				LLEntry<T>* entry = _lookup[ key ];
				if( entry == NULL )
					return NULL;

				_list.MoveToHead( entry );

				return entry->Value;
			}

			template<typename T>
			void LRU<T>::Clear()
			{
				_lookup.clear();
				_keyLookup.clear();
				_list.Clear();
				_count = 0;
			}

		}
	}
}
