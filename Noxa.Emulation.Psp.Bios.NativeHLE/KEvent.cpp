// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "Stdafx.h"
#include "KernelHelpers.h"
#include "Kernel.h"
#include "KEvent.h"
#include "KThread.h"
#include <malloc.h>
#include <string.h>

using namespace System;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Bios;

KEvent::KEvent( char* name, uint attributes, uint initialValue )
{
	assert( name != NULL );

	Name = _strdup( name );
	Attributes = attributes;
	InitialValue = initialValue;
	Value = initialValue;

	WaitingThreads = new LL<KThread*>();
}

KEvent::~KEvent()
{
	SAFEFREE( Name );

	assert( WaitingThreads->GetCount() == 0 );
	SAFEDELETE( WaitingThreads );
}

bool KEvent::Matches( uint userValue, uint waitType )
{
	if( ( waitType & KThreadWaitAnd ) == KThreadWaitAnd )
	{
		// &
		return ( Value & userValue ) > 0;
	}
	else
	{
		// |
		assert( ( waitType & KThreadWaitOr ) == KThreadWaitOr );
		return ( Value | userValue ) > 0;
	}
}

bool KEvent::Signal( Kernel* kernel )
{
	bool needsSwitch = false;

	LLEntry<KThread*>* e = WaitingThreads->GetHead();
	while( e != NULL )
	{
		LLEntry<KThread*>* next = e->Next;

		KThread* thread = e->Value;
		assert( thread != NULL );
		assert( thread->WaitingOn == KThreadWaitEvent );
		assert( thread->WaitEvent == this );

		bool matches = this->Matches( thread->WaitArgument, thread->WaitEventMode );
		if( matches == true )
		{
			// Wake thread
			thread->Wake();

			// Finish wait
			if( thread->WaitAddress != 0x0 )
			{
				int* poutBits = ( int* )kernel->Memory->Translate( thread->WaitAddress );
				*poutBits = this->Value;
			}

			if( ( thread->WaitEventMode & KThreadWaitClearAll ) != 0 )
				this->Value = 0;
			else if( ( thread->WaitEventMode & KThreadWaitClearPattern ) != 0 )
				this->Value = this->Value & ~thread->WaitArgument;

			WaitingThreads->Remove( e );

			needsSwitch = true;
		}

		e = next;
	}

	return needsSwitch;
}
