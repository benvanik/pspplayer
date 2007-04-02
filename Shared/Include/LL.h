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
				
				void InsertBefore( T value, LLEntry<T>* proceeding );
				void InsertAfter( T value, LLEntry<T>* preceeding );

				T PeekHead();
				T PeekTail();
				LLEntry<T>* Find( T value );
				LLEntry<T>* FindReverse( T value );

				void Remove( T value );
				void Remove( LLEntry<T>* entry );

				void Clear();
			};

		}
	}
}
