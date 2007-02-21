// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "StdAfx.h"
#include <string>
#include "OglDriver.h"
#include "VideoApi.h"

using namespace System::Diagnostics;
using namespace System::Threading;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Video;
using namespace Noxa::Emulation::Psp::Video::Native;

extern uint _vcount;

// NativeInterface
VdlRef* GetNextListBatch();
void FreeList( VideoDisplayList* list );

void WorkerThreadThunk( Object^ object );

void OglDriver::StartThread()
{
	_shutdown = false;
	_threadSync = gcnew AutoResetEvent( true );

	_thread = gcnew Thread( gcnew ParameterizedThreadStart( &WorkerThreadThunk ) );
	_thread->Name = "Video worker";
	_thread->Priority = ThreadPriority::Normal;
	_thread->IsBackground = true;
	_thread->Start( this );
}

void OglDriver::StopThread()
{
	if( ( _shutdown == false ) &&
		( _thread != nullptr ) )
	{
		_shutdown = true;

		if( _thread->Join( 500 ) == false )
			_thread->Abort();
		_thread = nullptr;
	}
}

void OglDriver::WorkerThread()
{
	try
	{
		while( _shutdown == false )
		{
			// Try to get a batch of lists to draw (as a frame)
			VdlRef* batch = GetNextListBatch();
			if( batch != NULL )
			{
				VdlRef* ref = batch;
				while( ref != NULL )
				{
					//Debug::WriteLine( "Would process list" );

					// Move to next while freeing old - since we are
					// done with it we also need to free the list
					FreeList( ref->List );
					VdlRef* next = ref->Next;
					SAFEFREE( ref );
					ref = next;
				}

				_vcount++;
			}

			//Thread::Sleep( 100 );
		}
	}
	catch( ThreadAbortException^ )
	{
		Debug::WriteLine( "OglDriver: aborting worker thread via ThreadAbortException" );
	}
	catch( ThreadInterruptedException^ )
	{
		Debug::WriteLine( "OglDriver: aborting worker thread via ThreadInterruptedException" );
	}
}

void WorkerThreadThunk( Object^ object )
{
	OglDriver^ driver = ( OglDriver^ )object;
	driver->WorkerThread();
}
