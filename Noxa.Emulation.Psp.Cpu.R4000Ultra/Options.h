// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

// When enabled, code dealing with certain critical areas that may later
// be accessed from multiple threads will be gaurded with locks.
//#define MULTITHREADED