// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

#include "R4000Ctx.h"

using namespace System;
using namespace Noxa::Emulation::Psp;

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace Cpu {

				ref class R4000Cp0
				{
				internal:
					array<uint>^		Registers;
					array<uint>^		Control;

				protected:
					int					_coreId;
					bool				_conditionBit;

				protected:

					ref class R4000Cp0Context
					{
					public:
						array<uint>^ GeneralRegisters;
						array<uint>^ ControlRegisters;
						bool ConditionBit;
					};

				public:

					R4000Cp0( R4000Ctx* ctx, int coreId )
					{
						_coreId = coreId;
						Registers = gcnew array<uint>( 32 );
						Control = gcnew array<uint>( 32 );
					}

					property Object^ Context
					{
						virtual Object^ get()
						{
							R4000Cp0Context^ context = gcnew R4000Cp0Context();
							context->GeneralRegisters = ( array<uint>^ )Registers->Clone();
							context->ControlRegisters = ( array<uint>^ )Control->Clone();
							context->ConditionBit = _conditionBit;
							return context;
						}
						virtual void set( Object^ value )
						{
							R4000Cp0Context^ context = ( R4000Cp0Context^ )value;
							Registers = ( array<uint>^ )context->GeneralRegisters->Clone();
							Control = ( array<uint>^ )context->ControlRegisters->Clone();
							_conditionBit = context->ConditionBit;
						}
					}

					virtual void Clear()
					{
						for( int n = 0; n < Registers->Length; n++ )
							Registers[ n ] = 0;
						for( int n = 0; n < Control->Length; n++ )
							Control[ n ] = 0;

						Registers[ 15 ] = 0; // revision id
						Registers[ 16 ] = 0; // configuration
						Registers[ 22 ] = _coreId;
						// 21: SC-code, SC-code << 2 ????
						// 24 ??
					}

					property int default[ int ]
					{
						virtual int get( int index )
						{
							return Registers[ index ];
						}
						virtual void set( int index, int value )
						{
							switch( index )
							{
							case 9:
							case 11:
							case 12:
							case 13:
							case 14:
							case 25:
							case 28:
							case 29:
							case 30:
								// Can write, but check perms
								break;
							}
						}
					}

					virtual int GetControlRegister( int reg )
					{
						return Control[ reg ];
					}

					virtual void SetControlRegister( int reg, int value )
					{
						Control[ reg ] = value;
					}

					property bool ConditionBit
					{
						virtual bool get()
						{
							return _conditionBit;
						}
						virtual void set( bool value )
						{
							_conditionBit = value;
						}
					}

					void ThrowException( int pc, int cause )
					{
					}
				};

			}
		}
	}
}
