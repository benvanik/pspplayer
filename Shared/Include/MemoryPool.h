// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

#include <string>

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
				int			_free;
				int			_used;
				
				ObjectPointer*	_freeList;
				ObjectPointer*	_usedList;

			public:
				MemoryPool()
				{
					_free = _used = 0;
					_freeList = NULL;
					_usedList = NULL;
				}

				~MemoryPool()
				{
					this->Clear();
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

							return p->Target;
						}
						q = p;
						p = p->Next;
					}

					// If here, no block was found
					p = this->Allocate( size );
					if( p == NULL )
					{
						// Couldn't allocate - too large?
						return NULL;
					}

					p->Next = _usedList;
					_usedList = p;

					_used++;

					return p->Target;
				}

				void Release( void* ptr )
				{
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

							return;
						}
						q = p;
						p = p->Next;
					}

					// Not found in used list!
				}

				void Clear()
				{
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
