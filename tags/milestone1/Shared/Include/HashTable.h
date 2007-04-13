// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			
			typedef uint hashval_t;			// Hash code type
			typedef void* PTR;				// Entry pointer type

			// Calculate hash of a table entry.
			typedef hashval_t (*HTHashDelegate)( const PTR );

			// Compare a table entry with a possible entry.  The entry already in
			// the table always comes first, so the second element can be of a
			// different type (but in this case htab_find and htab_find_slot
			// cannot be used; instead the variants that accept a hash value
			// must be used).
			typedef int (*HTCompareDelegate)( const PTR, const PTR );

			// Cleanup function called whenever a live element is removed from
			// the hash table.
			typedef void (*HTDeleteDelegate)( PTR );
			  
			// Function called by HTEnumDelegateerse for each live element.  The first
			// arg is the slot of the element (which can be passed to htab_clear_slot
			// if desired), the second arg is the auxiliary pointer handed to
			// HTEnumDelegateerse.  Return 1 to continue scan, 0 to stop.  */
			typedef int (*HTEnumDelegate)( void **, void * );

			typedef struct HashTable_t
			{
				HTHashDelegate		HashFunction;		// Element key hashing function
				HTCompareDelegate	CompareFunction;	// Comparison function
				HTDeleteDelegate	DeleteFunction;		// Element value cleanup function

				PTR*				Entries;			// Table
				size_t				Capacity;			// # of total entry slots
				size_t				n_elements;			// # of elements (live and deleted) in the table
				size_t				n_deleted;			// # of deleted items in the table

			#ifdef _DEBUG
				uint				SearchCount;		// # of find slot calls
				uint				CollisionCount;		// # of collisions
			#endif
			} *HashTable;

			HashTable HTCreate( size_t capacity, HTHashDelegate hashFunction, HTCompareDelegate compareFunction, HTDeleteDelegate deleteFunction );
			HashTable HTCreateDefault( size_t capacity );
			void HTDelete( HashTable htab );
			void HTClear( HashTable htab );
			
			PTR HTFind( HashTable htab, const PTR key );
			PTR* HTFindSlot( HashTable htab, const PTR key, bool insert );
			void HTAdd( HashTable htab, const PTR key, PTR value );
			void HTRemove( HashTable htab, PTR key );
			
			void HTEnumerate( HashTable htab, HTEnumDelegate callback, PTR info );

			size_t HTGetCapacity( HashTable htab );
			size_t HTGetCount( HashTable htab );
			float HTGetCollisionRate( HashTable htab );

		}
	}
}
