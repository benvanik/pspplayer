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
			namespace Video {

				ref class OglStatistics : public IVideoStatistics
				{
				public:
					OglStatistics(){}

					float FPS;								// Frames per second

					void GatherStats();

					property int FramesPerSecond
					{
						virtual int get()
						{
							return ( int )FPS;
						}
					}

				};

			}
		}
	}
}
