#pragma once

using namespace System;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Utilities;

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace Video {

				ref class Direct3DDriver : IVideoDriver
				{
				protected:
					IEmulationInstance^			_emu;
					ComponentParameters^		_params;
					DisplayProperties^			_props;

					PerformanceTimer^			_timer;

				public:
					Direct3DDriver( IEmulationInstance^ emulator, ComponentParameters^ parameters );

					property ComponentParameters^ Parameters
					{
						virtual ComponentParameters^ get()
						{
							return _params;
						}
					}

					property IEmulationInstance^ Emulator
					{
						virtual IEmulationInstance^ get()
						{
							return _emu;
						}
					}

					property DisplayProperties^ Properties
					{
						virtual DisplayProperties^ get()
						{
							return _props;
						}
					}

					virtual void Cleanup();

					virtual void Suspend();

					virtual bool Resume();
				};

			}
		}
	}
}
