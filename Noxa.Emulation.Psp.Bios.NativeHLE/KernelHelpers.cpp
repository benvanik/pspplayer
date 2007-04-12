// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "Stdafx.h"
#define WIN32_LEAN_AND_MEAN
#include <windows.h>
#include "Kernel.h"
#include "KernelHelpers.h"
#include "KDevice.h"
#include <malloc.h>
#include <string.h>

using namespace System;
using namespace System::Diagnostics;
using namespace Noxa::Emulation::Psp::Bios;
using namespace Noxa::Emulation::Psp::Bios::KernelHelpers;

char* KernelHelpers::ToNativeString( String^ string )
{
	if( ( string == nullptr ) ||
		( string->Length == 0 ) )
		return 0;

	int length = string->Length;
	char* dest = ( char* )malloc( length + 1 );
	for( int n = 0; n < length; n++ )
		dest[ n ] = ( byte )string[ n ];
	dest[ length ] = 0;

	return dest;
}

IMediaItem^ KernelHelpers::FindPath( Kernel* kernel, String^ path )
{
	if( path->IndexOf( ':' ) >= 0 )
	{
		const char* npath = ToNativeString( path );
		IMediaDevice^ device = kernel->FindMediaDevice( npath );
		SAFEFREE( npath );
		if( device == nullptr )
		{
			// Perhaps a block device?
			Debug::WriteLine( String::Format( "KernelHelpers::FindPath: unable to find device for path {0}", path ) );
			return nullptr;
		}

		path = path->Substring( path->IndexOf( ':' ) + 1 );
		if( device->State == MediaState::Present )
		{
			return device->Root->Find( path );
		}
		else
		{
			Debug::WriteLine( String::Format( "KernelHelpers::FindPath: unable to find root for path {0}", path ) );
			return nullptr;
		}
	}
	else
	{
		IMediaFolder^ root = kernel->CurrentPath;
		Debug::Assert( root != nullptr );

		return root->Find( path );
	}
}

String^ KernelHelpers::ReadString( IMemory^ memory, const int address )
{
	byte* ptr = ( ( MemorySystem* )memory->MemorySystemInstance )->Translate( address );
	if( ptr == NULL )
		return nullptr;
	else
		return gcnew String( ( const char* )ptr );
}

String^ KernelHelpers::ReadString( MemorySystem* memory, const int address )
{
	byte* ptr = memory->Translate( address );
	if( ptr == NULL )
		return nullptr;
	else
		return gcnew String( ( const char* )ptr );
}

int KernelHelpers::WriteString( IMemory^ memory, const int address, String^ value )
{
	char* nativeValue = ToNativeString( value );
	WriteString( ( ( MemorySystem* )memory->MemorySystemInstance ), address, nativeValue );
	SAFEFREE( nativeValue );
	return value->Length + 1;
}

int KernelHelpers::WriteString( MemorySystem* memory, const int address, String^ value )
{
	char* nativeValue = ToNativeString( value );
	WriteString( memory, address, nativeValue );
	SAFEFREE( nativeValue );
	return value->Length + 1;
}

#pragma unmanaged
int KernelHelpers::ReadString( MemorySystem* memory, const int address, byte* buffer, const int bufferSize )
{
	assert( memory != NULL );
	assert( buffer != NULL );
	assert( bufferSize > 0 );
	if( buffer == NULL )
		return 0;

	byte* ptr = memory->Translate( address );
	byte* end = ( byte* )memchr( ptr, 0, bufferSize );
	assert( end != NULL );
	if( end == NULL )
		return 0;
	int length = ( int )( end - ptr ) + 1;
	memcpy( buffer, ptr, length );
	return length;
}
#pragma managed

#pragma unmanaged
void KernelHelpers::WriteString( MemorySystem* memory, const int address, const char* buffer )
{
	assert( memory != NULL );
	assert( buffer != NULL );
	if( buffer == NULL )
		return;

	byte* ptr = memory->Translate( address );
	strcpy_s( ( char* )ptr, 0xFFFFFFFF, buffer );
}
#pragma managed

//unsigned short	year 
//unsigned short	month 
//unsigned short	day 
//unsigned short	hour 
//unsigned short	minute 
//unsigned short	second 
//unsigned int		microsecond

DateTime KernelHelpers::ReadTime( IMemory^ memory, const int address )
{
	int64 ft = ReadTime( ( ( MemorySystem* )memory->MemorySystemInstance ), address );
	return DateTime::FromFileTimeUtc( ft );
}

int KernelHelpers::WriteTime( IMemory^ memory, const int address, DateTime time )
{
	int64 ft = time.ToFileTimeUtc();
	return WriteTime( ( ( MemorySystem* )memory->MemorySystemInstance ), address, ft );
}

#define nano100SecInWeek		( int64 )10000000 * 60 * 60 * 24 * 7;
#define nano100SecInDay			( int64 )10000000 * 60 * 60 * 24;
#define nano100SecInHour		( int64 )10000000 * 60 * 60;
#define nano100SecInMin			( int64 )10000000 * 60;
#define nano100SecInSec			( int64 )10000000;
#define nano100SecInMs			( int64 )10000000 / 1000;

#pragma unmanaged
int64 KernelHelpers::ReadTime( MemorySystem* memory, const int address )
{
	ushort* ptr = ( ushort* )memory->Translate( address );

	SYSTEMTIME st;

	st.wYear			= *ptr;
	st.wMonth			= *( ptr + 1 );
	st.wDay				= *( ptr + 2 );
	st.wHour			= *( ptr + 3 );
	st.wMinute			= *( ptr + 4 );
	st.wSecond			= *( ptr + 5 );
	st.wMilliseconds	= *( ( uint* )( ptr + 6 ) ) / 1000;
	// TODO: proper us in FILETIME?

	::FILETIME ft;
	SystemTimeToFileTime( &st, &ft );

	return *( int64* )&ft;
}
#pragma managed

#pragma unmanaged
int KernelHelpers::WriteTime( MemorySystem* memory, const int address, int64 time )
{
	ushort* ptr = ( ushort* )memory->Translate( address );

	SYSTEMTIME st;
	FileTimeToSystemTime( ( ::FILETIME* )&time, &st );

	*ptr = ( ushort )st.wYear;
	*( ptr + 1 ) = ( ushort )st.wMonth;
	*( ptr + 2 ) = ( ushort )st.wDay;
	*( ptr + 3 ) = ( ushort )st.wHour;
	*( ptr + 4 ) = ( ushort )st.wMinute;
	*( ptr + 5 ) = ( ushort )st.wSecond;
	*( ( uint* )( ptr + 6 ) ) = ( ushort )st.wMilliseconds * 1000;
	// TODO: use milliseconds from FILETIME?

	return 16;
}
#pragma managed
