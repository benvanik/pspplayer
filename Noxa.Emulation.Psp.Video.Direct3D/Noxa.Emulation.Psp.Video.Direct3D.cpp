// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

// This is the main DLL file.

#include "stdafx.h"

#include "Noxa.Emulation.Psp.Video.Direct3D.h"

using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Video;

IList<ComponentIssue^>^ Direct3DVideo::Test( ComponentParameters^ parameters )
{
	List<ComponentIssue^>^ issues = gcnew List<ComponentIssue^>();

	return issues;
}