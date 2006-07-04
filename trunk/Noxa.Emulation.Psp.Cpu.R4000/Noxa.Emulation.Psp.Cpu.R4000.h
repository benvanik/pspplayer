// Noxa.Emulation.Psp.Cpu.R4000.h

#pragma once

#include "R4000Cpu.h"

using namespace System;
using namespace Noxa::Emulation::Psp;

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace Cpu {

				public ref class GenericCpu : IComponent
				{
				public:
					property ComponentType Type
					{
						virtual ComponentType get()
						{
							return ComponentType::Cpu;
						}
					}

					property String^ Name
					{
						virtual String^ get()
						{
							return "Interpreted CPU";
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
						return gcnew R4000Cpu( emulator, parameters );
					}
				};

			}
		}
	}
}
