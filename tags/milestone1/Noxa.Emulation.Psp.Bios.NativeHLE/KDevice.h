// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

#include "NoxaShared.h"
#include "KHandle.h"
#include "KernelHelpers.h"

using namespace System;
using namespace System::Diagnostics;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Bios;

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace Bios {

				class KDevice : public KHandle
				{
				public:
					char*			Name;
					char**			Aliases;
					int				AliasCount;

					bool			ReadOnly;
					
					bool			IsBlockDevice;
					int				BlockSize;

					gcref<IMediaDevice^>	Device;

				public:
					KDevice( char* name, bool readOnly, IMediaDevice^ device, array<String^>^ aliases )
					{
						Name = _strdup( name );
						ReadOnly = readOnly;
						IsBlockDevice = false;
						Device = device;

						AliasCount = aliases->Length;
						Aliases = new char*[ AliasCount ];
						for( int n = 0; n < AliasCount; n++ )
							Aliases[ n ] = KernelHelpers::ToNativeString( aliases[ n ] );
					}

					KDevice( char* name, bool readOnly, bool isBlockDevice, int blockSize )
					{
						Name = _strdup( name );
						ReadOnly = readOnly;
						IsBlockDevice = isBlockDevice;
						BlockSize = blockSize;

						for( int n = 0; n < AliasCount; n++ )
							SAFEFREE( Aliases[ n ] );
						SAFEDELETEA( Aliases );
					}

					~KDevice()
					{
						SAFEFREE( Name );
					}
				};

			}
		}
	}
}
