// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "StdAfx.h"
#define WIN32_LEAN_AND_MEAN
#include <Windows.h>
#include <assert.h>
#pragma unmanaged
#include <gl/gl.h>
#include <gl/glu.h>
#include <gl/glext.h>
#include <gl/wglext.h>
#pragma managed

#include <string>
#include "OglDriver.h"
#include "VideoApi.h"
#include "DisplayList.h"
#include "OglContext.h"
#include "OglExtensions.h"

using namespace System::Diagnostics;
using namespace System::Threading;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Video;
using namespace Noxa::Emulation::Psp::Video::Native;

bool _shutdown;

// Statistics
extern uint _vcount;
extern uint _displayListsProcessed;
extern uint _processedFrames;
extern uint _abortedLists;

extern HANDLE _hSyncEvent;
extern HANDLE _hListSyncEvent;
extern HANDLE _hWorkWaitingEvent;
extern int _workWaiting;
extern bool _vsyncWaiting;

// NativeInterface
DisplayList* GetNextDisplayList();

// Processing
void ProcessList( OglContext* context, DisplayList* list );

void WorkerThreadThunk( Object^ object );

void OglDriver::StartThread()
{
	_shutdown = false;
	_threadSync = gcnew AutoResetEvent( true );

	_context = ( OglContext* )malloc( sizeof( OglContext ) );
	memset( _context, 0, sizeof( OglContext ) );
	_context->TextureFilterMin = GL_LINEAR;
	_context->TextureFilterMag = GL_LINEAR;
	_context->TextureWrapS = GL_REPEAT;
	_context->TextureWrapT = GL_REPEAT;
	_context->TextureCache = new LRU<TextureEntry*>( 1500 );
	_context->TextureOffset[ 0 ] = 0.0f;
	_context->TextureOffset[ 1 ] = 0.0f;
	_context->TextureScale[ 0 ] = 1.0f;
	_context->TextureScale[ 1 ] = 1.0f;

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

		_thread->Interrupt();
		if( _thread->Join( 1000 ) == false )
			_thread->Abort();
		while( _thread->IsAlive == true )
			Thread::Sleep( 10 );
		_thread = nullptr;

		SAFEFREE( _context );
	}
}

#pragma unmanaged
void NativeWorker( HDC hDC, OglContext* context )
{
	long long startTime;
	long long endTime;
	long long freq;

	// We take ticks/sec to ticks/frame (16.6667 ms)
	QueryPerformanceFrequency( ( LARGE_INTEGER* )&freq );
	freq /= ( 1000 * 17 );

	while( _shutdown == false )
	{
		int listsDone = 0;
		if( ( _workWaiting > 0 ) ||
			( WaitForSingleObject( _hWorkWaitingEvent, 100 ) == WAIT_OBJECT_0 ) )
		{
			//QueryPerformanceCounter( ( LARGE_INTEGER* )&startTime );

			// Work to do!
			while( ( _shutdown == false ) &&
				( _workWaiting > 0 ) )
			{
				DisplayList* list = GetNextDisplayList();
				if( list != NULL )
				{
					// Keep working on this list until we are done with it
					do
					{
						// Wait until not stalled
						while( ( list->Stalled == true ) &&
							( _shutdown == false ) )
						{
							if( WaitForSingleObject( _hWorkWaitingEvent, 16 ) != WAIT_OBJECT_0 )
							{
								// Timeout - abort list!
								/*list->Drawn = true;
								list->Done = true;
								PulseEvent( _hListSyncEvent );
								_abortedLists++;
								goto listAbort;*/
							}
						}

						// Process
						ProcessList( context, list );

					} while( list->Done == false );
listAbort:
#ifdef STATISTICS
					_displayListsProcessed++;
#endif

					//QueryPerformanceCounter( ( LARGE_INTEGER* )&endTime );
					//if( ( ( endTime - startTime ) / freq ) > 0 )
					//	break;
				}
				else
					break;

				listsDone++;

				if( _vsyncWaiting == true )
					break;
			}
		}

		if( listsDone > 0 )
		{
			_processedFrames++;

			glBindTexture( GL_TEXTURE_2D, 0 );

			// THIS IS WRONG
			_vcount++;
			SwapBuffers( hDC );
			PulseEvent( _hSyncEvent );
		}
		else
			Sleep( 0 );
		_vsyncWaiting = false;
	}
}
#pragma managed

void OglDriver::SetupOpenGL()
{
	HDC hDC = GetDC( ( HWND )_handle );
	_hDC = hDC;
	Debug::Assert( _hDC != NULL );
	if( _hDC == NULL )
		return;

	PIXELFORMATDESCRIPTOR pfd;
	ZeroMemory( &pfd, sizeof( pfd ) );
	pfd.nSize = sizeof( pfd );
	pfd.nVersion = 1;
	pfd.dwFlags = PFD_DRAW_TO_WINDOW | PFD_SUPPORT_OPENGL | PFD_TYPE_RGBA
#ifdef VSYNC
		| PFD_DOUBLEBUFFER
#endif
		;
	pfd.iPixelType = PFD_TYPE_RGBA;
	pfd.cColorBits = 24;
	pfd.cDepthBits = 32;
	pfd.iLayerType = PFD_MAIN_PLANE;
	int iFormat = ChoosePixelFormat( hDC, &pfd );
	SetPixelFormat( hDC, iFormat, &pfd );

	HGLRC hRC = wglCreateContext( hDC );
	_hRC = hRC;
	Debug::Assert( _hRC != NULL );
	if( _hRC == NULL )
	{
		ReleaseDC( ( HWND )_handle, hDC );
		_hDC = NULL;
		return;
	}
	wglMakeCurrent( hDC, hRC );
	
	glShadeModel( GL_SMOOTH );
	glClearColor( 0.0f, 0.0f, 0.0f, 1.0f );
	glClearDepth( 0.0f );
	glEnable( GL_DEPTH_TEST );
	glDepthFunc( GL_LEQUAL );
	glDepthRange( 1.0f, 0.0f );
	glHint( GL_PERSPECTIVE_CORRECTION_HINT, GL_NICEST );

	SetupExtensions();

//#ifndef VSYNC
	wglSwapIntervalEXT( 0 );
//#endif

	_context->ClutTable = calloc( 1, CLUTSIZE );
}

void OglDriver::DestroyOpenGL()
{
	wglMakeCurrent( NULL, NULL );
	if( _hRC != NULL )
	    wglDeleteContext( ( HGLRC )_hRC );
	if( ( _handle != NULL ) &&
		( _hDC != NULL ) )
		ReleaseDC( ( HWND )_handle, ( HDC )_hDC );

	SAFEFREE( _context->ClutTable );
}

void OglDriver::Resize( int width, int height )
{
	glViewport( 0, height, width, height );
}

void OglDriver::WorkerThread()
{
	this->SetupOpenGL();

	// Setup the context
	_context->Memory = ( NativeMemorySystem* )_emu->Cpu->Memory->NativeMemorySystem.ToPointer();
	Debug::Assert( _context->Memory != NULL );
	if( _context->Memory == NULL )
		return;

	_startTime = DateTime::Now;

	try
	{
		NativeWorker( ( HDC )_hDC, _context );
	}
	catch( ThreadAbortException^ )
	{
		Debug::WriteLine( "OglDriver: aborting worker thread via ThreadAbortException" );
	}
	catch( ThreadInterruptedException^ )
	{
		Debug::WriteLine( "OglDriver: aborting worker thread via ThreadInterruptedException" );
	}
	finally
	{
		this->DestroyOpenGL();
	}
}

void WorkerThreadThunk( Object^ object )
{
	OglDriver^ driver = ( OglDriver^ )object;
	driver->WorkerThread();
}
