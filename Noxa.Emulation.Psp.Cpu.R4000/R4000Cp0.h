#pragma once

#include "R4000Coprocessor.h"

using namespace System;
using namespace Noxa::Emulation::Psp;

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace Cpu {

				ref class R4000Cp0 : R4000Coprocessor
				{
				protected:
					array<int>^			_registers;
					array<int>^			_control;
					int					_coreId;
					bool				_conditionBit;

				protected:

					ref class R4000Cp0Context
					{
					public:
						array<int>^ GeneralRegisters;
						array<int>^ ControlRegisters;
						bool ConditionBit;
					};

				public:

					R4000Cp0( R4000Core^ core )
						: R4000Coprocessor( core )
					{
						_coreId = core->CoreID;
						_registers = gcnew array<int>( 32 );
						_control = gcnew array<int>( 32 );
					}

					property Object^ Context
					{
						virtual Object^ get() override
						{
							R4000Cp0Context^ context = gcnew R4000Cp0Context();
							context->GeneralRegisters = ( array<int>^ )_registers->Clone();
							context->ControlRegisters = ( array<int>^ )_control->Clone();
							context->ConditionBit = _conditionBit;
							return context;
						}
						virtual void set( Object^ value ) override
						{
							R4000Cp0Context^ context = ( R4000Cp0Context^ )value;
							_registers = ( array<int>^ )context->GeneralRegisters->Clone();
							_control = ( array<int>^ )context->ControlRegisters->Clone();
							_conditionBit = context->ConditionBit;
						}
					}

					virtual void Clear() override
					{
						for( int n = 0; n < _registers->Length; n++ )
							_registers[ n ] = 0;
						for( int n = 0; n < _control->Length; n++ )
							_control[ n ] = 0;

						_registers[ 15 ] = 0; // revision id
						_registers[ 16 ] = 0; // configuration
						_registers[ 22 ] = _coreId;
						// 21: SC-code, SC-code << 2 ????
						// 24 ??
					}

					property int default[ int ]
					{
						virtual int get( int index ) override
						{
							return _registers[ index ];
						}
						virtual void set( int index, int value ) override
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

					virtual int GetControlRegister( int reg ) override
					{
						return _control[ reg ];
					}

					virtual void SetControlRegister( int reg, int value ) override
					{
						_control[ reg ] = value;
					}

					property bool ConditionBit
					{
						virtual bool get() override
						{
							return _conditionBit;
						}
						virtual void set( bool value ) override
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
