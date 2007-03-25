// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "stdafx.h"

#define WIN32_LEAN_AND_MEAN
#include <Windows.h>

#include "Noxa.Emulation.Psp.Input.SimpleInput.h"

using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Input;

IList<ComponentIssue^>^ SimpleInput::Test( ComponentParameters^ parameters )
{
	List<ComponentIssue^>^ issues = gcnew List<ComponentIssue^>();

	// Test for DX/XInput

	return issues;
}