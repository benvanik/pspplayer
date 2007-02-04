// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

using namespace System;
using namespace System::Threading;
using namespace Noxa::Emulation::Psp;

#if MULTITHREADED
#define LOCK	Monitor::Enter( _syncRoot )
#define UNLOCK	UNLOCK;
#else
#define LOCK
#define UNLOCK
#endif

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace Cpu {

				//delegate int DynamicCodeDelegate( Core core0, Memory memory, int[] generalRegisters, BiosFunction[] syscallList, int branchTarget );

				ref class CodeBlock
				{
				public:
					int			Address;
					int			Size;
					int			InstructionCount;
					void*		Pointer;
					bool		EndsOnSyscall;
					
					int			ExecutionCount;
				};

				ref class R4000Cache
				{
				protected:
					array<array<array<CodeBlock^>^>^>^	_lookup;
					Object^			_syncRoot;

				public:
					R4000Cache()
					{
						_syncRoot = gcnew Object();
						this->Clear();
					}

				public:

					void Add( CodeBlock^ block )
					{
						uint addr = ( ( uint )block->Address ) >> 2;

						uint b0 = addr >> 20;
						uint b1 = ( addr >> 10 ) & 0x3FF;
						uint b2 = addr & 0x3FF;

						LOCK;
						{
							array<array<CodeBlock^>^>^ block0 = _lookup[ b0 ];
							if( block0 == nullptr )
							{
								block0 = gcnew array<array<CodeBlock^>^>( 1024 );
								_lookup[ b0 ] = block0;
							}

							array<CodeBlock^>^ block1 = block0[ b1 ];
							if( block1 == nullptr )
							{
								block1 = gcnew array<CodeBlock^>( 1024 );
								block0[ b1 ] = block1;
							}

							block1[ b2 ] = block;
						}
						UNLOCK;
					}

					CodeBlock^ Find( int address )
					{
						uint addr = ( ( uint )address ) >> 2;

						uint b0 = addr >> 20;
						uint b1 = ( addr >> 10 ) & 0x3FF;
						uint b2 = addr & 0x3FF;

						LOCK;
						{
							array<array<CodeBlock^>^>^ block0 = _lookup[ b0 ];
							if( block0 == nullptr )
							{
								UNLOCK;
								return nullptr;
							}

							array<CodeBlock^>^ block1 = block0[ b1 ];
							if( block1 == nullptr )
							{
								UNLOCK;
								return nullptr;
							}

							CodeBlock^ block = block1[ b2 ];

							UNLOCK;
							return block;
						}
					}

					void Invalidate( int address )
					{
						uint addr = ( ( uint )address ) >> 2;

						uint b0 = addr >> 20;
						uint b1 = ( addr >> 10 ) & 0x3FF;

						LOCK;
						{
							array<array<CodeBlock^>^>^ block0 = _lookup[ b0 ];
							if( block0 == nullptr )
							{
								UNLOCK;
								return;
							}

							array<CodeBlock^>^ block1 = block0[ b1 ];
							if( block1 == nullptr )
							{
								UNLOCK;
								return;
							}

							for( int n = 0; n < block1->Length; n++ )
							{
								CodeBlock^ block = block1[ n ];
								if( block == nullptr )
									continue;
								
								// If we have gone beyond the starting address, then break out
								int upper = block->Address + block->Size;
								if( upper > address )
									break;

								// If the block contains the address, invalidate
								if( ( address >= block->Address ) &&
									( address <= upper ) )
									block1[ n ] = nullptr;
							}
						}
						UNLOCK;
					}

					void Clear()
					{
						LOCK;
						{
							_lookup = gcnew array<array<array<CodeBlock^>^>^>( 1024 );
						}
						UNLOCK;
					}

				};

			}
		}
	}
}
