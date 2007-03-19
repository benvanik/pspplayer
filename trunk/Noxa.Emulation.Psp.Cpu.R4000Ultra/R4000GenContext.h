// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

#include <string>

using namespace System;
using namespace System::Collections::Generic;
using namespace System::Diagnostics;
using namespace Noxa::Emulation::Psp;

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace Cpu {

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
						Label = ( char* )malloc( 20 );
						sprintf_s( Label, 20, "l%Xlm", address );
					}

					~LabelMarker()
					{
						SAFEFREE( Label );
					}
				};

				ref class R4000GenContext
				{
				public:
					R4000GenContext( R4000Generator* generator, byte* mainMemory, byte* frameBuffer )
					{
						Generator = generator;

						MainMemory = mainMemory;
						FrameBuffer = frameBuffer;

						BranchLabels = gcnew Dictionary<int, LabelMarker^>();
					}

					R4000Generator*		Generator;

					byte*				MainMemory;
					byte*				FrameBuffer;

					int					StartAddress;
					int					EndAddress;

					bool				UpdatePC;
					bool				UseSyscalls;
					bool				LastSyscallStateless;

					Dictionary<int, LabelMarker^>^ BranchLabels;
					int					LastBranchTarget;

					int					JumpTarget;
					int					JumpRegister;

					bool				InDelay;
					LabelMarker^		BranchTarget;

					void*				CtxPointer;

					void Reset( int startAddress )
					{
						StartAddress = startAddress;
						EndAddress = startAddress;

						UpdatePC = false;
						UseSyscalls = false;

						BranchLabels->Clear();
						LastBranchTarget = 0;
						InDelay = false;
						BranchTarget = nullptr;
						JumpTarget = 0;
						JumpRegister = 0;
					}

					bool IsBranchLocal( int address )
					{
						return( ( address >= StartAddress ) &&
							( address <= EndAddress ) );
					}

					void DefineBranchTarget( int address )
					{
						if( BranchLabels->ContainsKey( address ) == false )
						{
							//Debug::WriteLine( String::Format( "Defining branch target {0:X8}", address ) );
							LabelMarker^ lm = gcnew LabelMarker( address );
							BranchLabels->Add( address, lm );
						}
						if( LastBranchTarget < address )
							LastBranchTarget = address;
					}
				};

			}
		}
	}
}
