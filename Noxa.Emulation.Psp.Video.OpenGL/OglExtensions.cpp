// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "StdAfx.h"
#define WIN32_LEAN_AND_MEAN
#include <Windows.h>
#include <assert.h>
#include <string>
#include <cmath>
#pragma unmanaged
#include <gl/gl.h>
#include <gl/glu.h>
#include <gl/glext.h>
#include <gl/wglext.h>
#pragma managed
#include "OglExtensions.h"

using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Video;

#pragma unmanaged

PFNGLBLENDEQUATIONPROC Noxa::Emulation::Psp::Video::glBlendEquation = NULL;
PFNWGLSWAPINTERVALEXTPROC Noxa::Emulation::Psp::Video::wglSwapIntervalEXT = NULL;

bool Noxa::Emulation::Psp::Video::SetupExtensions()
{
	glBlendEquation = (PFNGLBLENDEQUATIONPROC)wglGetProcAddress( "glBlendEquationEXT" );
	assert( glBlendEquation != NULL );
	if( glBlendEquation == NULL )
		return false;

	wglSwapIntervalEXT = (PFNWGLSWAPINTERVALEXTPROC)wglGetProcAddress( "wglSwapIntervalEXT" );
	assert( wglSwapIntervalEXT != NULL );
	if( wglSwapIntervalEXT == NULL )
		return false;

	return true;
}

#pragma managed
