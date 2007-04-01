// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

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
				T* Get( int index );
				T GetValue( int index );
				T Remove( int index );
				void Clear();

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

		}
	}
}
