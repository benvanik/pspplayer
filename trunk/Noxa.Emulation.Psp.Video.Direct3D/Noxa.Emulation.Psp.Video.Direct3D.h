// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

#include "D3dDriver.h"

using namespace System;
using namespace System::Collections::Generic;
using namespace Noxa::Emulation::Psp;

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace Video {

				public ref class Direct3DVideo : IComponent
				{
				public:
					property ComponentType Type
					{
						virtual ComponentType get()
						{
							return ComponentType::Video;
						}
					}

					property String^ Name
					{
						virtual String^ get()
						{
							return "Direct3D Video";
						}
					}

					property System::Version^ Version
					{
						virtual System::Version^ get()
						{
							return gcnew System::Version( 1, 0 );
						}
					}

					property String^ Author
					{
						virtual String^ get()
						{
							return "Ben Vanik (ben.vanik@gmail.com)";
						}
					}

					property String^ Website
					{
						virtual String^ get()
						{
							return "http://www.noxa.org";
						}
					}

					property String^ RssFeed
					{
						virtual String^ get()
						{
							return "http://www.noxa.org/rss";
						}
					}

					property ComponentBuild Build
					{
						virtual ComponentBuild get()
						{
#ifdef _DEBUG
							return ComponentBuild::Debug;
#else
							return ComponentBuild::Release;
#endif
						}
					}

					property bool IsTestable
					{
						virtual bool get()
						{
							return true;
						}
					}

					virtual IList<ComponentIssue^>^ Test( ComponentParameters^ parameters );

					virtual String^ ToString() override
					{
						return this->Name;
					}

					property bool IsConfigurable
					{
						virtual bool get()
						{
							return false;
						}
					}

					virtual IComponentConfiguration^ CreateConfiguration( ComponentParameters^ parameters )
					{
						return nullptr;
					}

					virtual IComponentInstance^ CreateInstance( IEmulationInstance^ emulator, ComponentParameters^ parameters )
					{
						return gcnew D3dDriver( emulator, parameters );
					}
				};

			}
		}
	}
}
