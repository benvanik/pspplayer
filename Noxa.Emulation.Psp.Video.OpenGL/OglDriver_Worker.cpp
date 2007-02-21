// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "StdAfx.h"
#include <Windows.h>
#pragma unmanaged
#include <gl/gl.h>
#include <gl/glu.h>
#pragma managed

#include <string>
#include "OglDriver.h"
#include "VideoApi.h"

using namespace System::Diagnostics;
using namespace System::Threading;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Video;
using namespace Noxa::Emulation::Psp::Video::Native;

// Statistics
extern uint _vcount;
extern uint _displayListsProcessed;

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

#pragma unmanaged
float theta = 0.0f;
void NativeWorker( HDC hDC )
{
	// Try to get a batch of lists to draw (as a frame)
	VdlRef* batch = GetNextListBatch();
	if( batch != NULL )
	{
		VdlRef* ref = batch;
		while( ref != NULL )
		{
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

		glClear( GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT );
		
		glPushMatrix();
		glRotatef( theta, 0.0f, 0.0f, 1.0f );
		glBegin( GL_TRIANGLES );
		glColor3f( 1.0f, 0.0f, 0.0f ); glVertex2f( 0.0f, 1.0f );
		glColor3f( 0.0f, 1.0f, 0.0f ); glVertex2f( 0.87f, -0.5f );
		glColor3f( 0.0f, 0.0f, 1.0f ); glVertex2f( -0.87f, -0.5f );
		glEnd();
		glPopMatrix();
		
		theta += 1.0f;

		_vcount++;

		SwapBuffers( hDC );
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
	glClearDepth( 1.0f );
	glEnable( GL_DEPTH_TEST );
	glDepthFunc( GL_LEQUAL );
	glHint( GL_PERSPECTIVE_CORRECTION_HINT, GL_NICEST );
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

	try
	{
		while( _shutdown == false )
		{
			NativeWorker( ( HDC )_hDC );

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
