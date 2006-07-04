#pragma once

using namespace System;
using namespace Noxa::Emulation::Psp;

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace Cpu {

				ref class R4000Core;

				ref class R4000Coprocessor abstract
				{
				protected:
					R4000Core^			_core;

				protected:

					R4000Coprocessor( R4000Core^ core )
					{
						_core = core;
					}

				public:

					property R4000Core^ Core
					{
						R4000Core^ get()
						{
							return _core;
						}
					}

					property Object^ Context
					{
						virtual Object^ get() abstract;
						virtual void set( Object^ value ) abstract;
					}

					virtual void Clear() abstract;

					property bool ConditionBit
					{
						virtual bool get() abstract;
						virtual void set( bool value ) abstract;
					}

					property int default[ int ]
					{
						virtual int get( int core )
						{
							return 0;
						}
						virtual void set( int core, int value )
						{
						}
					}

					virtual int GetControlRegister( int reg )
					{
						return 0;
					}

					virtual void SetControlRegister( int reg, int value )
					{
					}
				};

			}
		}
	}
}
