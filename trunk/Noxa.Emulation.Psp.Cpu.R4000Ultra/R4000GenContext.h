// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

#include "R4000BlockBuilder.h"

using namespace System;
using namespace System::Collections::Generic;
using namespace Noxa::Emulation::Psp;

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace Cpu {

				ref class R4000Memory;
				class R4000Generator;

				enum class GenerationResult
				{
					Success,
					Invalid,
					Syscall,

					Branch,
					BranchAndNullifyDelay,

					Jump
				};

				ref class LabelMarker
				{
				public:

					char*	Label;
					bool	Found;
					int		Address;

					LabelMarker( int address )
					{
						Address = address;
					}
				};

				ref class R4000GenContext
				{
				internal:
					R4000BlockBuilder^	_builder;

				protected:
					R4000Generator*		_gen;

				public:
					R4000GenContext( R4000BlockBuilder^ builder, R4000Generator* generator )
					{
						_builder = builder;
						_gen = generator;
					}

					property R4000Memory^ Memory
					{
						virtual R4000Memory^ get()
						{
							return _builder->_memory;
						}
					}

					property R4000Generator* Generator
					{
						virtual R4000Generator* get()
						{
							return _gen;
						}
					}

					int StartAddress;
					int EndAddress;

					bool UpdatePC;

					Dictionary<int, LabelMarker^>^ BranchLabels;
					int LastBranchTarget;

					bool InDelay;
					LabelMarker^ BranchTarget;

					void Reset( int startAddress )
					{
						StartAddress = startAddress;
						EndAddress = startAddress;

						UpdatePC = false;

						BranchLabels->Clear();
						LastBranchTarget = 0;
						InDelay = false;
						BranchTarget = nullptr;
					}

					bool IsBranchLocal( int address )
					{
						return( ( address >= StartAddress ) &&
							( address <= EndAddress ) );
					}

					void DefineBranchTarget( int address )
					{
					}
				};

			}
		}
	}
}
