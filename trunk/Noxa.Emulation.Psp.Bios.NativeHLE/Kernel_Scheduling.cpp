// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "Stdafx.h"
#define WIN32_LEAN_AND_MEAN
#include <windows.h>
#include "Kernel.h"
#include "NativeBios.h"
#include "HandleTable.h"
#include "KThread.h"
#include "KEvent.h"
#include "KCallback.h"
#include "Module.h"

using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Bios;
using namespace Noxa::Emulation::Psp::Cpu::Native;

bool Kernel::Schedule()
{
	while( this->SchedulableThreads->GetCount() == 0 )
	{
		// No threads to run? Check for delayed threads
		void* state = NULL;
		KThread** pthread;
		KThread* earliest = NULL;
		while( ( pthread = Threads->Enumerate( &state ) ) != NULL )
		{
			KThread* thread = *pthread;
			if( thread->State == KThreadWaiting )
			{
				if( thread->WaitingOn == KThreadWaitDelay )
				{
					if( ( earliest == NULL ) ||
						( thread->WaitTimestamp + thread->WaitTimeout ) > ( earliest->WaitTimestamp + thread->WaitTimeout ) )
						earliest = thread;
				}
			}
		}

		if( earliest != NULL )
		{
			// Wait on it
			while( earliest->State == KThreadWaiting )
				Sleep( 1 );
		}
		else
		{
			Debug::WriteLine( "Kernel::Schedule: ran out of threads to run - exiting sim" );
			return false;
		}
	}

	// Find the next thread to run - this is easy
	KThread* nextThread = this->SchedulableThreads->PeekHead();
	assert( nextThread != NULL );

	// If it didn't change, don't do anything
	if( nextThread == ActiveThread )
		return false;

	// Switch
	Cpu->SwitchContext( nextThread->Context->ContextID );

	ActiveThread = nextThread;

	return true;
}

void Kernel::Execute()
{
	if( ActiveThread != NULL )
	{
		// Execute active thread
		int breakFlags;
		int instructionCount = Cpu->Execute( &breakFlags );
		if( breakFlags != 0 )
		{
			// Schedule request
			this->Schedule();
		}

		//Debug::WriteLine( String::Format( "Kernel: execute ended after {0} instructions w/ flags {1}", instructionCount, breakFlags ) );
	}
	else
	{
		// Load game and run until it creates its first thread

		IEmulationInstance^ emu = Emu;
		Debug::Assert( emu != nullptr );
		NativeBios^ bios = Bios;
		Debug::Assert( bios != nullptr );
		ILoader^ loader = bios->Loader;
		Debug::Assert( loader != nullptr );

		// Clear everything
		emu->LightReset();

		// Get bootstream
		Debug::Assert( bios->BootStream == nullptr );
		GameLoader^ gameLoader = gcnew GameLoader();
		bios->BootStream = gameLoader->FindBootStream( bios->Game );
		Debug::Assert( bios->BootStream != nullptr );

		LoadParameters^ params = gcnew LoadParameters();
		params->Path = bios->Game->Folder;
		LoadResults^ results = loader->LoadModule( ModuleType::Boot, bios->BootStream, params );
		
		Debug::Assert( results->Successful == true );
		if( results->Successful == false )
		{
			Debug::WriteLine( "Kernel: load failed!" );
			bios->Game = nullptr;
			return;
		}

		this->CurrentPath = bios->Game->Folder;
		emu->Cpu->SetupGame( bios->Game, bios->BootStream );

		// Start modules
		for( int n = 0; n < bios->_moduleList->Count; n++ )
			bios->_moduleList[ n ]->Start();

		//// Run until we have a thread
		//int preThreadCount = 0;
		//while( ActiveThread == NULL )
		//{
		//	int breakFlags;
		//	preThreadCount += Cpu->Execute( &breakFlags );
		//	assert( breakFlags == 0 );
		//}

		//Debug::WriteLine( String::Format( "Kernel: first thread created, executed {0} instructions", preThreadCount ) );
	}
}

extern Kernel* _currentKernel;
void IssueCallbackComplete()
{
	// Let the kernel know where we are
	assert( _currentKernel != NULL );
	_currentKernel->CallbackComplete();
}

int Kernel::IssueCallback( int callbackId, int argument )
{
	KCallback* cb = ( KCallback* )Handles->Lookup( callbackId );
	if( cb == NULL )
		return -1;

	// Not sure how to handle a CB to an active thread!
	assert( cb->Thread != ActiveThread );

	_oldThread = ActiveThread;
	ActiveThread = cb->Thread;

	Cpu->MarshalCallback( cb->Thread->Context->ContextID, cb->Address, argument, &IssueCallbackComplete );

	// The CPU, when complete with the callback, will set the return value (hopefully)
	// It will then call IssueCallbackComplete so we can switch the thread back in ourselves

	return 0;
}

void Kernel::CallbackComplete()
{
	assert( _oldThread != NULL );

	ActiveThread = _oldThread;
	_oldThread = NULL;
}
