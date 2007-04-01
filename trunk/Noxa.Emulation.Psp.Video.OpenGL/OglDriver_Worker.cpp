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

// NativeInterface
VdlRef* GetNextListBatch();
void FreeList( VideoDisplayList* list );

// Processing
void ProcessList( OglContext* context, VideoDisplayList* list );

void WorkerThreadThunk( Object^ object );

void OglDriver::StartThread()
{
	_shutdown = false;
	_threadSync = gcnew AutoResetEvent( true );

	_context = ( OglContext* )malloc( sizeof( OglContext ) );
	memset( _context, 0, sizeof( OglContext ) );

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
	float theta = 0.0f;
	while( _shutdown == false )
	{
		// Try to get a batch of lists to draw (as a frame)
		VdlRef* batch = GetNextListBatch();
		if( batch != NULL )
		{
			VdlRef* ref = batch;
			while( ref != NULL )
			{
				// I've actually seen this happen!
				assert( ref->List->Ready == true );

				// Actually process the list
				ProcessList( context, ref->List );

#ifdef STATISTICS
				_displayListsProcessed++;
#endif

				// Move to next while freeing old - since we are
				// done with it we also need to free the list
				FreeList( ref->List );
				VdlRef* next = ref->Next;
				SAFEFREE( ref );
				ref = next;
			}

			//glClear( GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT );
			
			/*glMatrixMode( GL_MODELVIEW );
			glPushMatrix();
			glRotatef( theta, 0.0f, 0.0f, 1.0f );
			glBegin( GL_TRIANGLES );
			glColor3f( 1.0f, 0.0f, 0.0f ); glVertex2f( 0.0f, 1.0f );
			glColor3f( 0.0f, 1.0f, 0.0f ); glVertex2f( 0.87f, -0.5f );
			glColor3f( 0.0f, 0.0f, 1.0f ); glVertex2f( -0.87f, -0.5f );
			glEnd();
			glPopMatrix();*/

			glFlush();
			
			theta += 0.3f;

			_vcount++;

			SwapBuffers( hDC );
		}
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
	pfd.cDepthBits = 16;
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
	glClearColor( 0.0f, 0.0f, 0.0f, 0.5f );
	glClearDepth( 0.0f );
	glEnable( GL_DEPTH_TEST );
	glDepthFunc( GL_LEQUAL );
	glDepthRange( 1.0f, 0.0f );
	glHint( GL_PERSPECTIVE_CORRECTION_HINT, GL_NICEST );

	SetupExtensions();
}

void OglDriver::DestroyOpenGL()
{
	wglMakeCurrent( NULL, NULL );
	if( _hRC != NULL )
	    wglDeleteContext( ( HGLRC )_hRC );
	if( ( _handle != NULL ) &&
		( _hDC != NULL ) )
		ReleaseDC( ( HWND )_handle, ( HDC )_hDC );
}

void OglDriver::WorkerThread()
{
	this->SetupOpenGL();

	// Setup the context
	bool supportInternalMemory = ( _emu->Cpu->Memory->MainMemoryPointer != NULL );
	Debug::Assert( supportInternalMemory == true );
	_context->MainMemoryPointer = ( byte* )_emu->Cpu->Memory->MainMemoryPointer;
	_context->VideoMemoryPointer = ( byte* )_emu->Cpu->Memory->VideoMemoryPointer;

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
