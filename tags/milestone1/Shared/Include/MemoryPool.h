// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

#define WIN32_LEAN_AND_MEAN
#include <Windows.h>
#include <string>

#define LOCK EnterCriticalSection( &_cs )
#define UNLOCK LeaveCriticalSection( &_cs )

namespace Noxa {
	namespace Emulation {
		namespace Psp {

			class MemoryPool
			{
			protected:
				typedef struct ObjectPointer_t
				{
					void*				Target;
					int					Size;
					ObjectPointer_t*	Next;
				} ObjectPointer;

			protected:
				int					_free;
				int					_used;
				CRITICAL_SECTION	_cs;
				
				ObjectPointer*	_freeList;
				ObjectPointer*	_usedList;

			public:
				MemoryPool()
				{
					_free = _used = 0;
					_freeList = NULL;
					_usedList = NULL;

					InitializeCriticalSection( &_cs );
				}

				~MemoryPool()
				{
					this->Clear();

					DeleteCriticalSection( &_cs );
				}

				int GetFreeCount()
				{
					return _free;
				}

				int GetUsedCount()
				{
					return _used;
				}

				int GetCount()
				{
					return _used + _free;
				}

				void* Request( int size )
				{
					if( size == 0 )
						return NULL;

					LOCK;

					// Try to find a free item
					ObjectPointer* p = _freeList;
					ObjectPointer* q = NULL;
					while( p != NULL )
					{
						if( p->Size >= size )
						{
							// Remove from existing list
							if( q != NULL )
								q->Next = p->Next;
							else
								_freeList = p->Next;

							// Add to used list
							p->Next = _usedList;
							_usedList = p;

							_used++;
							_free--;

							void* target = p->Target;
							UNLOCK;
							return target;
						}
						q = p;
						p = p->Next;
					}

					// If here, no block was found
					p = this->Allocate( size );
					if( p == NULL )
					{
						// Couldn't allocate - too large?
						UNLOCK;
						return NULL;
					}

					p->Next = _usedList;
					_usedList = p;

					_used++;

					void* target = p->Target;
					UNLOCK;
					return target;
				}

				void Release( void* ptr )
				{
					LOCK;

					ObjectPointer* p = _usedList;
					ObjectPointer* q = NULL;
					while( p != NULL )
					{
						if( p->Target == ptr )
						{
							// Remove from used list
							if( q != NULL )
								q->Next = p->Next;
							else
								_usedList = p->Next;

							// Add to free list
							p->Next = _freeList;
							_freeList = p;

							_free++;
							_used--;

							UNLOCK;
							return;
						}
						q = p;
						p = p->Next;
					}

					// Not found in used list!
					UNLOCK;
				}

				void Clear()
				{
					LOCK;

					this->FreeList( _freeList );
					_freeList = NULL;

					if( _used > 0 )
					{
						// Whoa! Trying to Clear() with things used!
					}
					this->FreeList( _usedList );
					_usedList = NULL;

					_free = 0;
					_used = 0;

					UNLOCK;
				}

			protected:
				ObjectPointer* Allocate( int size )
				{
					void* ptr = malloc( size );
					if( ptr == NULL )
						return NULL;

					ObjectPointer* p = ( ObjectPointer* )malloc( sizeof( ObjectPointer ) );
					p->Size = size;
					p->Target = ptr;
					p->Next = NULL;

					return p;
				}

				void FreeList( ObjectPointer* list )
				{
					ObjectPointer* p = list;
					while( p != NULL )
					{
						SAFEFREE( p->Target );
						p->Size = 0;

						ObjectPointer* q = p;
						p = p->Next;
						q->Next = NULL;
						SAFEFREE( q );
					}
				}
			};
			
		}
	}
}
