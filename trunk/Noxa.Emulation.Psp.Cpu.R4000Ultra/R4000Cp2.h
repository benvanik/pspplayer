// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

using namespace System;
using namespace Noxa::Emulation::Psp;

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace Cpu {

				ref class R4000Cp2
				{
				protected:

				public:

					R4000Cp2()
					{
					}

					property Object^ Context
					{
						virtual Object^ get()
						{
							return nullptr;
						}
						virtual void set( Object^ value )
						{
						}
					}

					virtual void Clear()
					{
					}

					property bool ConditionBit
					{
						virtual bool get()
						{
							return false;
						}
						virtual void set( bool value )
						{
						}
					}
				};

			}
		}
	}
}
