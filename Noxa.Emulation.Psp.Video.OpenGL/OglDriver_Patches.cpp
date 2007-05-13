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

#include "OglDriver.h"
#include "VideoApi.h"
#include "OglContext.h"
#include "OglTextures.h"
#include "OglExtensions.h"

using namespace System::Diagnostics;
using namespace System::Threading;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Video;
using namespace Noxa::Emulation::Psp::Video::Native;

void DrawBezier( OglContext* context, int vertexType, int vertexSize, byte* iptr, byte* ptr, int ucount, int vcount );
//void DrawSpline( OglContext* context, int vertexType, int vertexCount, int vertexSize, byte* ptr );

#pragma unmanaged

void DrawBezier( OglContext* context, int vertexType, int vertexSize, byte* iptr, byte* ptr, int ucount, int vcount )
{
}

#pragma managed
