// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "Stdafx.h"
#include "KernelHelpers.h"
#include "KFile.h"
#include <malloc.h>
#include <string.h>

using namespace System;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Bios;

#define STDBUFFERSIZE	2048

KFile::KFile( KDevice* device, IMediaItem^ item )
{
	Device = device;
	Item = item;

	bool readOnly = ( item->Attributes & MediaItemAttributes::ReadOnly ) == MediaItemAttributes::ReadOnly;
	CanWrite = !readOnly;
}

KFile::KFile( KDevice* device, IMediaItem^ item, Stream^ stream, bool readOnly )
{
	Device = device;
	Item = item;
	BoundStream = stream;
	IsOpen = true;
	CanWrite = !readOnly;
	CanSeek = true;
}

KFile::KFile( int specialFileHandle )
{
	IsStd = true;
	IsOpen = true;
	CanWrite = !( specialFileHandle == HSTDIN );
	CanSeek = false;
}

KFile::~KFile()
{
}

KStdFile::KStdFile( int specialFileHandle )
	: KFile( specialFileHandle == HSTDIN )
{
	StdType = specialFileHandle;
	UID = specialFileHandle;

	_buffer = ( byte* )malloc( STDBUFFERSIZE );
	_bufferIndex = 0;
}

KStdFile::~KStdFile()
{
	SAFEFREE( _buffer );
	_bufferIndex = 0;
}

void KStdFile::Write( const byte* buffer, int count )
{
	// Copy to buffer
	assert( _bufferIndex + count < STDBUFFERSIZE );
	if( _bufferIndex + count >= STDBUFFERSIZE )
		return;
	memcpy( _buffer + _bufferIndex, buffer, count );
	_bufferIndex += count;

	// Add trailing 0 - we assume input doesn't have it
	_buffer[ _bufferIndex ] = 0;
	_bufferIndex++;

	// If the buffer ends in \n, flush
	if( _buffer[ _bufferIndex - 2 ] == '\n' )
	{
		String^ temp = gcnew String( ( char* )_buffer, 0, _bufferIndex );
		Debug::WriteLine( String::Format( "{0}: {1}", ( StdType == HSTDERR ? "stderr" : "stdout" ), temp ) );
		_bufferIndex = 0;
	}
}

void KStdFile::Write( const char* string )
{
	int length = strlen( string );
	this->Write( ( const byte* )string, length );
}

void KStdFile::Write( String^ string )
{
	char* ptr = KernelHelpers::ToNativeString( string );
	if( ptr != NULL )
		this->Write( ( const byte* )ptr, string->Length );
	SAFEFREE( ptr );
}
