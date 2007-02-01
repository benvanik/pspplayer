// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

#include <string>
#include "R4000Cp0.h"
#include "R4000Cp1.h"
#include "R4000Cp2.h"

using namespace System;
using namespace Noxa::Emulation::Psp;

#define RegisterCount 32
#define RegistersSize ( RegisterCount * sizeof( int ) )

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace Cpu {

				ref class R4000Cpu;

				ref class R4000Core : ICpuCore
				{
				internal:
					R4000Cpu^			Cpu;
					R4000Cp0^			Cp0;
					R4000Cp1^			Cp1;
					R4000Cp2^			Cp2;

					int			PC;
					int*		Registers;
					int			HI;
					int			LO;
					bool		LL;

					bool		InDelaySlot;
					int			DelayPC;
					bool		DelayNop;
					int			InterruptState;

					// Used for tracking instruction counts inside of blocks
					int			BlockCounter;

				public:
					R4000Core( R4000Cpu^ cpu )
					{
						Cpu = cpu;

						Cp0 = gcnew R4000Cp0( 0 );
						Cp1 = gcnew R4000Cp1();
						Cp2 = gcnew R4000Cp2();

						Registers = ( int * )malloc( RegistersSize );

						this->Clear();
					}

				protected:
					~R4000Core()
					{
						SAFEFREE( Registers );
					}

				public:

					void Clear()
					{
						PC = 0;
						if( Registers != NULL )
						{
							for( int n = 0; n < RegisterCount; n++ )
								Registers[ n ] = 0;
							Registers[ 29 ] = 0x087FFFFF;
						}
						HI = 0;
						LO = 0;
						LL = false;
						InDelaySlot = false;
						DelayPC = 0;
						DelayNop = false;
						InterruptState = 0;

						BlockCounter = 0;

						Cp0->Clear();
						Cp1->Clear();
						Cp2->Clear();
					}

					property String^ Name
					{
						virtual String^ get()
						{
							return "Allegrex";
						}
					}

					property int ProgramCounter
					{
						virtual int get()
						{
							return PC;
						}
						virtual void set( int value )
						{
							PC = value;
							InDelaySlot = false;
							DelayPC = 0;
							DelayNop = false;
						}
					}

					property array<int>^ GeneralRegisters
					{
						virtual array<int>^ get()
						{
							if( Registers != NULL )
							{
								array<int>^ ret = gcnew array<int>( RegisterCount );
								pin_ptr<array<int>^> ptr = &ret;
								memcpy( ptr, Registers, RegistersSize );
								return ret;
							}
							else
								return gcnew array<int>( 32 );
						}
					}

					virtual void SetGeneralRegister( int reg, int value )
					{
						Registers[ reg ] = value;
					}

				protected:
					ref class R4000CoreContext
					{
					public:
						int PC;
						int* Registers;
						int HI;
						int LO;
						bool LL;
						bool InDelaySlot;
						int DelayPC;
						bool DelayNop;
						int InterruptState;

						Object^ Cp0;
						Object^ Cp1;
					};

				public:

					property Object^ Context
					{
						virtual Object^ get()
						{
							R4000CoreContext^ context = gcnew R4000CoreContext();
	
							context->PC = PC;
							context->Registers = ( int* )malloc( RegistersSize );
							for( int n = 0; n < RegisterCount; n++ )
								context->Registers[ n ] = Registers[ n ];
							context->HI = HI;
							context->LO = LO;
							context->LL = LL;
							context->InDelaySlot = InDelaySlot;
							context->DelayPC = DelayPC;
							context->DelayNop = DelayNop;
							context->InterruptState = InterruptState;

							context->Cp0 = Cp0->Context;
							context->Cp1 = Cp1->Context;

							return context;
						}
						virtual void set( Object^ value )
						{
							R4000CoreContext^ context = ( R4000CoreContext^ )value;

							PC = context->PC;
							for( int n = 0; n < RegisterCount; n++ )
								Registers[ n ] = context->Registers[ n ];
							HI = context->HI;
							LO = context->LO;
							LL = context->LL;
							InDelaySlot = context->InDelaySlot;
							DelayPC = context->DelayPC;
							DelayNop = context->DelayNop;
							InterruptState = context->InterruptState;

							Cp0->Context = context->Cp0;
							Cp1->Context = context->Cp1;

							// HACK: clear delay slot
							if( InDelaySlot == true )
							{
								InDelaySlot = false;
								PC = DelayPC - 4;
								DelayPC = 0;
								DelayNop = false;
							}
						}
					}

					property CoreState^ State
					{
						virtual CoreState^ get()
						{
							CoreState^ state = gcnew CoreState();

							state->ProgramCounter = PC;
							state->GeneralRegisters = this->GeneralRegisters;
							state->Hi = HI;
							state->Lo = LO;
							state->LL = LL;

							state->Cp0ControlRegisters = Cp0->Control;
							state->Cp0Registers = Cp0->Registers;
							state->Cp0ConditionBit = Cp0->ConditionBit;

							state->FpuControlRegister = Cp1->Control;
							state->FpuRegisters = ( array<float>^ )Cp1->Registers;

							return state;
						}
					}
				};

			}
		}
	}
}
