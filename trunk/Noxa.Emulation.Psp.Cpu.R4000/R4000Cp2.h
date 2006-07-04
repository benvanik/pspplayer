#pragma once

#include "R4000Coprocessor.h"

using namespace System;
using namespace Noxa::Emulation::Psp;

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace Cpu {

				ref class R4000Cp2 : R4000Coprocessor
				{
				protected:

				public:

					R4000Cp2( R4000Core^ core )
						: R4000Coprocessor( core )
					{
					}

					property Object^ Context
					{
						virtual Object^ get() override
						{
							return nullptr;
						}
						virtual void set( Object^ value ) override
						{
						}
					}

					virtual void Clear() override
					{
					}

					property bool ConditionBit
					{
						virtual bool get() override
						{
							return false;
						}
						virtual void set( bool value ) override
						{
						}
					}
				};

			}
		}
	}
}
