// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

// This is the main DLL file.

#include "stdafx.h"

#define WIN32_LEAN_AND_MEAN
#include <Windows.h>
#pragma unmanaged
#include <gl/gl.h>
#include <gl/glu.h>
#include <gl/glext.h>
#include <gl/wglext.h>
#pragma managed

#include "Noxa.Emulation.Psp.Video.OpenGL.h"

using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Video;

IList<ComponentIssue^>^ OpenGLVideo::Test( ComponentParameters^ parameters )
{
	List<ComponentIssue^>^ issues = gcnew List<ComponentIssue^>();

	// Test for OGL extensions used:
	// - GL_EXT_packed_pixels
	// - GL_EXT_blend_subtract
	// - GL_EXT_blend_minmax

	return issues;
}