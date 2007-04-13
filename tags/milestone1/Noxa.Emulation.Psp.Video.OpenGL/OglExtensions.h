// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

#define WIN32_LEAN_AND_MEAN
#include <Windows.h>
#pragma unmanaged
#include <gl/gl.h>
#include <gl/glu.h>
#include <gl/glext.h>
#include <gl/wglext.h>
#pragma managed

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace Video {

				bool SetupExtensions();

				extern PFNGLBLENDEQUATIONPROC		glBlendEquation;

			}
		}
	}
}
