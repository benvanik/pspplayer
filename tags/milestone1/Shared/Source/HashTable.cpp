// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

// Original license from the file follows. The code
// has been heavily modified!

/* An expandable hash tables datatype.  
Copyright (C) 1999, 2000, 2001 Free Software Foundation, Inc.
Contributed by Vladimir Makarov (vmakarov@cygnus.com).

This file is part of the libiberty library.
Libiberty is free software; you can redistribute it and/or
modify it under the terms of the GNU Library General Public
License as published by the Free Software Foundation; either
version 2 of the License, or (at your option) any later version.

Libiberty is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Library General Public License for more details.

You should have received a copy of the GNU Library General Public
License along with libiberty; see the file COPYING.LIB.  If
not, write to the Free Software Foundation, Inc., 59 Temple Place - Suite 330,
Boston, MA 02111-1307, USA.  */

/* This package implements basic hash table functionality.  It is possible
to search for an entry, create an entry and destroy an entry.

Elements in the table are generic pointers.

The size of the table is not fixed; if the occupancy of the table
grows too high the hash table will be expanded.

The abstract data implementation is based on generalized Algorithm D
from Knuth's book "The art of computer programming".  Hash table is
expanded by creation of new hash table and transferring elements from
the old table to the new table. */

#include "StdAfx.h"

#include "HashTable.h"

#pragma warning( disable: 4949 )
#pragma unmanaged

#include <string.h>
#include <stdio.h>
#include <malloc.h>
#include <assert.h>

using namespace Noxa::Emulation::Psp;

#define EMPTY_ENTRY		((PTR) 0)		// Value for slots that are empty
#define DELETED_ENTRY	((PTR) 1)		// Value for slots that contained an entry that has been deleted

static uint higher_prime_number(uint);
static int htab_expand(HashTable);
static PTR *find_empty_slot_for_expand(HashTable, hashval_t);

/* The following function returns a nearest prime number which is
greater than N, and near a power of two. */

static uint higher_prime_number( uint n )
{
	/* These are primes that are near, but slightly smaller than, a
	power of two.  */
	static uint primes[] = {
		(uint) 2,
		(uint) 7,
		(uint) 13,
		(uint) 31,
		(uint) 61,
		(uint) 127,
		(uint) 251,
		(uint) 509,
		(uint) 1021,
		(uint) 2039,
		(uint) 4093,
		(uint) 8191,
		(uint) 16381,
		(uint) 32749,
		(uint) 65521,
		(uint) 131071,
		(uint) 262139,
		(uint) 524287,
		(uint) 1048573,
		(uint) 2097143,
		(uint) 4194301,
		(uint) 8388593,
		(uint) 16777213,
		(uint) 33554393,
		(uint) 67108859,
		(uint) 134217689,
		(uint) 268435399,
		(uint) 536870909,
		(uint) 1073741789,
		(uint) 2147483647,
		/* 4294967291L */
		((uint) 2147483647) + ((uint) 2147483644),
	};

	uint* low = &primes[0];
	uint* high = &primes[sizeof(primes) / sizeof(primes[0])];

	while( low != high )
	{
		uint* mid = low + ( high - low ) / 2;
		if( n > *mid )
			low = mid + 1;
		else
			high = mid;
	}

	/* If we've run out of primes, abort.  */
	assert( n <= *low );
	if( n > *low )
		fprintf( stderr, "Cannot find prime bigger than %lu\r\n", n );

	return *low;
}

/* Returns a hash code for P.  */
static hashval_t hash_pointer( const PTR p )
{
	return ( hashval_t )( (long)p >> 3 );
}

/* Returns non-zero if P1 and P2 are equal.  */
static int eq_pointer( const PTR p1, const PTR p2 )
{
	return p1 == p2;
}

/* This function creates table with length slightly longer than given
source length.  The created hash table is initiated as empty (all the
hash table entries are EMPTY_ENTRY).  The function returns the created
hash table.  Memory allocation may fail; it may return NULL.  */
HashTable Noxa::Emulation::Psp::HTCreate( size_t capacity, HTHashDelegate hashFunction, HTCompareDelegate compareFunction, HTDeleteDelegate deleteFunction )
{
	capacity = higher_prime_number( capacity );
	HashTable result = ( HashTable )calloc( 1, sizeof( struct HashTable_t ) );
	if( result == NULL )
		return NULL;

	result->Entries = ( PTR* )calloc( capacity, sizeof( PTR ) );
	if( result->Entries == NULL )
	{
		free( result );
		return NULL;
	}

	result->Capacity		= capacity;
	result->HashFunction	= hashFunction;
	result->CompareFunction	= compareFunction;
	result->DeleteFunction	= deleteFunction;
	return result;
}

HashTable Noxa::Emulation::Psp::HTCreateDefault( size_t capacity )
{
	return HTCreate( capacity, hash_pointer, eq_pointer, NULL );
}

/* This function frees all memory allocated for given hash table.
Naturally the hash table must already exist. */
void Noxa::Emulation::Psp::HTDelete( HashTable htab )
{
	if( htab->DeleteFunction != NULL )
	{
		for( int i = htab->Capacity - 1; i >= 0; i-- )
		{
			if( ( htab->Entries[i] != EMPTY_ENTRY ) &&
				( htab->Entries[i] != DELETED_ENTRY ) )
				(*htab->DeleteFunction)( htab->Entries[i] );
		}
	}

	free( htab->Entries );
	free( htab );
}

/* This function clears all entries in the given hash table.  */
void Noxa::Emulation::Psp::HTClear( HashTable htab )
{
	if( htab->DeleteFunction != NULL )
	{
		for( int i = htab->Capacity - 1; i >= 0; i-- )
		{
			if( ( htab->Entries[i] != EMPTY_ENTRY ) &&
				( htab->Entries[i] != DELETED_ENTRY ) )
				(*htab->DeleteFunction)( htab->Entries[i] );
		}
	}

	htab->n_elements = 0;
	htab->n_deleted = 0;
	memset( htab->Entries, 0, htab->Capacity * sizeof( PTR ) );
}

/* Similar to htab_find_slot, but without several unwanted side effects:
- Does not call htab->CompareFunction when it finds an existing entry.
- Does not change the count of elements/searches/collisions in the
hash table.
This function also assumes there are no deleted entries in the table.
HASH is the hash value for the element to be inserted.  */
static PTR* find_empty_slot_for_expand( HashTable htab, hashval_t hash )
{
	size_t size = htab->Capacity;
	hashval_t hash2 = 1 + hash % (size - 2);
	uint index = hash % size;

	while( true )
	{
		PTR* slot = htab->Entries + index;

		assert( *slot != DELETED_ENTRY );
		if( *slot == EMPTY_ENTRY )
			return slot;
		else if( *slot == DELETED_ENTRY )
		{
			fprintf( stderr, "Slot contains deleted entry - unable to expand when deleted entries are present\r\n" );
		}

		index += hash2;
		if( index >= size )
			index -= size;
	}
}

/* The following function changes size of memory allocated for the
entries and repeatedly inserts the table elements.  The occupancy
of the table after the call will be about 50%.  Naturally the hash
table must already exist.  Remember also that the place of the
table entries is changed.  If memory allocation failures are allowed,
this function will return zero, indicating that the table could not be
expanded.  If all goes well, it will return a non-zero value.  */
static int htab_expand( HashTable htab )
{
	PTR* oentries = htab->Entries;
	PTR* olimit = oentries + htab->Capacity;

	htab->Capacity = higher_prime_number( htab->Capacity * 2 );

	PTR* nentries = ( PTR* )calloc( htab->Capacity, sizeof( PTR* ) );
	if( nentries == NULL )
		return 0;
	htab->Entries = nentries;

	htab->n_elements -= htab->n_deleted;
	htab->n_deleted = 0;

	PTR* p = oentries;
	do
	{
		PTR x = *p;

		if( ( x != EMPTY_ENTRY ) &&
			( x != DELETED_ENTRY ) )
		{
			PTR* q = find_empty_slot_for_expand( htab, (*htab->HashFunction)( x ) );

			*q = x;
		}

		p++;
	}
	while( p < olimit );

	free( oentries );
	return 1;
}

/* This function searches for a hash table entry equal to the given
element.  It cannot be used to insert or delete an element.  */
PTR htab_find_with_hash( HashTable htab, const PTR key, hashval_t hash )
{
#ifdef _DEBUG
	htab->SearchCount++;
#endif

	size_t size = htab->Capacity;
	uint index = hash % size;

	PTR entry = htab->Entries[index];
	if( ( entry == EMPTY_ENTRY ) ||
		( ( entry != DELETED_ENTRY ) &&
		  ( (*htab->CompareFunction)( *( (PTR*)entry ), key ) ) ) )
		return entry;

	hashval_t hash2 = 1 + hash % ( size - 2 );

	while( true )
	{
#ifdef _DEBUG
		htab->CollisionCount++;
#endif
		index += hash2;
		if( index >= size )
			index -= size;

		entry = htab->Entries[index];
		if( ( entry == EMPTY_ENTRY ) ||
			( ( entry != DELETED_ENTRY ) &&
			  ( (*htab->CompareFunction)( *( (PTR*)entry ), key ) ) ) )
			return entry;
	}
}

/* Like htab_find_slot_with_hash, but compute the hash value from the element.  */
PTR Noxa::Emulation::Psp::HTFind( HashTable htab, const PTR key )
{
	return htab_find_with_hash( htab, key, (*htab->HashFunction)( key ) );
}

/* This function searches for a hash table slot containing an entry
equal to the given element.  To delete an entry, call this with
INSERT = 0, then call htab_clear_slot on the slot returned (possibly
after doing some checks).  To insert an entry, call this with
INSERT = 1, then write the value you want into the returned slot.
When inserting an entry, NULL may be returned if memory allocation
fails.  */
PTR* htab_find_slot_with_hash( HashTable htab, const PTR key, hashval_t hash, bool insert )
{
	if( ( insert == true ) &&
		( htab->Capacity * 3 <= htab->n_elements * 4 ) &&
		( htab_expand( htab ) == 0 ) )
		return NULL;

	size_t size = htab->Capacity;
	hashval_t hash2 = 1 + hash % ( size - 2 );
	uint index = hash % size;

#ifdef _DEBUG
	htab->SearchCount++;
#endif

	PTR* first_deleted_slot = NULL;

	while( true )
	{
		PTR entry = htab->Entries[index];
		if( entry == EMPTY_ENTRY )
		{
			if( insert == false )
				return NULL;

			htab->n_elements++;

			if( first_deleted_slot != NULL )
			{
				*first_deleted_slot = EMPTY_ENTRY;
				return first_deleted_slot;
			}

			return &htab->Entries[index];
		}

		if( entry == DELETED_ENTRY )
		{
			if( first_deleted_slot == NULL )
				first_deleted_slot = &htab->Entries[index];
		}
		else if( (*htab->CompareFunction)( *( (PTR*)entry ), key ) )
			return &htab->Entries[index];

#ifdef _DEBUG
		htab->CollisionCount++;
#endif

		index += hash2;
		if (index >= size)
			index -= size;
	}
}

/* Like htab_find_slot_with_hash, but compute the hash value from the element.  */
PTR* Noxa::Emulation::Psp::HTFindSlot( HashTable htab, const PTR key, bool insert )
{
	return htab_find_slot_with_hash( htab, key, (*htab->HashFunction)( key ), insert );
}

void Noxa::Emulation::Psp::HTAdd( HashTable htab, const PTR key, PTR value )
{
	PTR* slot = HTFindSlot( htab, key, true );
	*slot = value;
}

/* This function deletes an element with the given value from hash
table.  If there is no matching element in the hash table, this
function does nothing.  */
void Noxa::Emulation::Psp::HTRemove( HashTable htab, PTR key )
{
	PTR* slot = HTFindSlot( htab, key, false );
	if( *slot == EMPTY_ENTRY )
		return;

	if( htab->DeleteFunction != NULL )
		(*htab->DeleteFunction)( *slot );

	*slot = DELETED_ENTRY;
	htab->n_deleted++;
}

/* This function clears a specified slot in a hash table.  It is
useful when you've already done the lookup and don't want to do it
again.  */
void htab_clear_slot( HashTable htab, PTR* slot )
{
	if( ( slot < htab->Entries ) ||
		( slot >= htab->Entries + htab->Capacity ) ||
		( *slot == EMPTY_ENTRY ) ||
		( *slot == DELETED_ENTRY ) )
	{
		assert( false );
		fprintf( stderr, "Unable to clear slot that is empty/deleted (or invalid slot pointer)\r\n" );
	}

	if( htab->DeleteFunction != NULL )
		(*htab->DeleteFunction)( *slot );

	*slot = DELETED_ENTRY;
	htab->n_deleted++;
}

/* This function scans over the entire hash table calling
CALLBACK for each live entry.  If CALLBACK returns false,
the iteration stops.  INFO is passed as CALLBACK's second
argument.  */
void Noxa::Emulation::Psp::HTEnumerate( HashTable htab, HTEnumDelegate callback, PTR info )
{
	PTR* slot = htab->Entries;
	PTR* limit = slot + htab->Capacity;

	do
	{
		PTR x = *slot;

		if( ( x != EMPTY_ENTRY ) &&
			( x != DELETED_ENTRY ) )
		{
			if( !(*callback)( slot, info ) )
				break;
		}
	}
	while( ++slot < limit );
}

/* Return the current size of given hash table. */
size_t Noxa::Emulation::Psp::HTGetCapacity( HashTable htab )
{
	return htab->Capacity;
}

/* Return the current number of elements in given hash table. */
size_t Noxa::Emulation::Psp::HTGetCount( HashTable htab )
{
	return htab->n_elements - htab->n_deleted;
}

/* Return the fraction of fixed collisions during all work with given hash table. */
float Noxa::Emulation::Psp::HTGetCollisionRate( HashTable htab )
{
#ifdef _DEBUG
	if( htab->SearchCount == 0 )
		return 0.0f;

	return (float)htab->CollisionCount / (float)htab->SearchCount;
#else
	return 0.0f;
#endif
}

#pragma managed
