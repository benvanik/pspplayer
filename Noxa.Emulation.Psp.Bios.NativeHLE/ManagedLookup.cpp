// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "Stdafx.h"
#include "ManagedLookup.h"

using namespace System::Collections;
using namespace Noxa::Emulation::Psp::Bios;

ref class MLC
{
public:
	static ArrayList^		Lookup;
};

void SetupManagedLookup()
{
	MLC::Lookup = gcnew ArrayList( 2048 );
}

void ClearManagedLookup()
{
	MLC::Lookup = nullptr;
}

generic<typename T>
int AddObject( T object )
{
	return MLC::Lookup->Add( object );
}

generic<typename T>
T LookupObject( int tag )
{
	return ( T )MLC::Lookup[ tag ];
}

generic<typename T>
T RemoveObject( int tag )
{
	T ret = ( T )MLC::Lookup[ tag ];
	MLC::Lookup->Remove( tag );
	return ret;
}
