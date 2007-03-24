// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "Stdafx.h"
#include "IoFileMgrForUser.h"
#include "Kernel.h"
#include "KernelFileHandle.h"

using namespace System;
using namespace System::Collections::Generic;
using namespace System::Diagnostics;
using namespace System::IO;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Bios;
using namespace Noxa::Emulation::Psp::Bios::Modules;

// SceUID sceIoOpen(const char *file, int flags, SceMode mode); (/user/pspiofilemgr.h:63)
int IoFileMgrForUser::sceIoOpen( IMemory^ memory, int fileName, int flags, int mode )
{
	String^ path = KernelHelpers::ReadString( memory, fileName );
	IMediaFile^ file = ( IMediaFile^ )KernelHelpers::FindPath( _kernel, path );
	if( file == nullptr )
	{
		// Create if needed
		if( ( flags & 0x0200 ) != 0 )
		{
			String^ parentPath = path->Substring( 0, path->LastIndexOf( '/' ) );
			String^ newName = path->Substring( path->LastIndexOf( '/' ) + 1 );
			IMediaFolder^ parent = ( IMediaFolder^ )KernelHelpers::FindPath( _kernel, parentPath );
			if( parent == nullptr )
			{
				Debug::WriteLine( String::Format( "sceIoOpen: could not find parent to create file '{0}' in on open", path ) );
				return -1;
			}
			file = parent->CreateFile( newName );
		}
		else
		{
			Debug::WriteLine( String::Format( "sceIoOpen: could not find path '{0}'", path ) );
			return -1;
		}
	}
	/*
	 *	#define PSP_O_RDONLY	0x0001
		#define PSP_O_WRONLY	0x0002
		#define PSP_O_RDWR		(PSP_O_RDONLY | PSP_O_WRONLY)
		#define PSP_O_NBLOCK	0x0004
		#define PSP_O_DIROPEN	0x0008	// Internal use for dopen
		#define PSP_O_APPEND	0x0100
		#define PSP_O_CREAT		0x0200
		#define PSP_O_TRUNC		0x0400
		#define	PSP_O_EXCL		0x0800
		#define PSP_O_NOWAIT	0x8000*/
	MediaFileMode fileMode = MediaFileMode::Normal;
	if( ( flags & 0x0100 ) == 0x0100 )
		fileMode = MediaFileMode::Append;
	if( ( flags & 0x0400 ) == 0x0400 )
		fileMode = MediaFileMode::Truncate;
	MediaFileAccess fileAccess = MediaFileAccess::ReadWrite;
	if( ( flags & 0x0001 ) == 0x0001 )
		fileAccess = MediaFileAccess::Read;
	if( ( flags & 0x0002 ) == 0x0002 )
		fileAccess = MediaFileAccess::Write;
	if( ( flags & 0x0003 ) == 0x0003 )
		fileAccess = MediaFileAccess::ReadWrite;

	if( ( flags & 0x0800 ) != 0 )
	{
		// Exclusive O_EXCL
		//int x = 1;
	}
	if( ( flags & 0x8000 ) != 0 )
	{
		// Non-blocking O_NOWAIT
		//int x = 1;
	}
	if( ( flags & 0x0004 ) != 0 )
	{
		// ? O_NBLOCK
		//int x = 1;
	}

	Stream^ stream = file->Open( fileMode, fileAccess );
	if( stream == nullptr )
	{
		Debug::WriteLine( String::Format( "sceIoOpen: could not open stream on file '{0}' for mode {1} access {2}", path, fileMode, fileAccess ) );
		return -1;
	}

	KernelFileHandle^ handle = gcnew KernelFileHandle( _kernel->AllocateID() );
	handle->Device = ( KernelFileDevice^ )_kernel->FindDevice( path );
	handle->CanWrite = !handle->Device->ReadOnly;
	handle->CanSeek = handle->Device->IsSeekable;
	handle->IsOpen = true;
	handle->MediaItem = file;
	handle->Stream = stream;
	_kernel->AddHandle( handle );

#ifdef VERBOSEIO
	Debug::WriteLine( String::Format( "sceIoOpen: opened file {0} with ID {1}", path, handle->ID ) );
#endif

	return handle->ID;
}

// SceUID sceIoOpenAsync(const char *file, int flags, SceMode mode); (/user/pspiofilemgr.h:73)
int IoFileMgrForUser::sceIoOpenAsync( IMemory^ memory, int file, int flags, int mode ){ return NISTUBRETURN; }

// int sceIoClose(SceUID fd); (/user/pspiofilemgr.h:85)
int IoFileMgrForUser::sceIoClose( int fd )
{
	KernelFileHandle^ handle = ( KernelFileHandle^ )_kernel->FindHandle( fd );
	if( handle == nullptr )
	{
		Debug::WriteLine( String::Format( "sceIoClose: kernel file handle {0} not found", fd ) );
		return -1;
	}

	if( handle->Stream != nullptr )
		handle->Stream->Close();
	handle->IsOpen = false;
	_kernel->RemoveHandle( handle );

	return 0;
}

// int sceIoCloseAsync(SceUID fd); (/user/pspiofilemgr.h:93)
int IoFileMgrForUser::sceIoCloseAsync( int fd ){ return NISTUBRETURN; }

// int sceIoRead(SceUID fd, void *data, SceSize size); (/user/pspiofilemgr.h:109)
int IoFileMgrForUser::sceIoRead( IMemory^ memory, int fd, int data, int size )
{
	KernelFileHandle^ handle = ( KernelFileHandle^ )_kernel->FindHandle( fd );
	if( handle == nullptr )
	{
		Debug::WriteLine( String::Format( "sceIoRead: kernel file handle {0} not found", fd ) );
		return -1;
	}

	if( fd == 0 )
	{
		// stdin
	}

	int length = MIN2( size, ( int )( handle->Stream->Length - handle->Stream->Position ) );

	memory->WriteStream( data, handle->Stream, length );

	return length;
}

// int sceIoReadAsync(SceUID fd, void *data, SceSize size); (/user/pspiofilemgr.h:125)
int IoFileMgrForUser::sceIoReadAsync( IMemory^ memory, int fd, int data, int size ){ return NISTUBRETURN; }

// int sceIoWrite(SceUID fd, const void *data, SceSize size); (/user/pspiofilemgr.h:141)
int IoFileMgrForUser::sceIoWrite( IMemory^ memory, int fd, int data, int size )
{
	// a0 = SceUID fd
	// a1 = const void *data
	// a2 = SceSize size

	KernelFileHandle^ handle;
	if( fd == 1 )
		handle = _kernel->StdOut;
	else
		handle = ( KernelFileHandle^ )_kernel->FindHandle( fd );
	if( handle == nullptr )
	{
		Debug::WriteLine( String::Format( "sceIoWrite: kernel file handle {0} not found", fd ) );
		return -1;
	}

	memory->ReadStream( data, handle->Stream, size );

	if( fd == 1 )
	{
		// stdout
		array<byte>^ buffer = memory->ReadBytes( data, size );
		String^ str = System::Text::Encoding::ASCII->GetString( buffer, 0, buffer->Length )->TrimEnd();
		Debug::WriteLine( String::Format( "stdout: {0}", str ) );
	}
	else if( fd == 2 )
	{
		// stderr
		array<byte>^ buffer = memory->ReadBytes( data, size );
		String^ str = System::Text::Encoding::ASCII->GetString( buffer, 0, buffer->Length )->TrimEnd();
		Debug::WriteLine( String::Format( "stderr: {0}", str ) );
	}

	return 0;
}

// int sceIoWriteAsync(SceUID fd, const void *data, SceSize size); (/user/pspiofilemgr.h:152)
int IoFileMgrForUser::sceIoWriteAsync( IMemory^ memory, int fd, int data, int size ){ return NISTUBRETURN; }

// SceOff sceIoLseek(SceUID fd, SceOff offset, int whence); (/user/pspiofilemgr.h:169)
int64 IoFileMgrForUser::sceIoLseek( int fd, int64 offset, int whence )
{
	KernelFileHandle^ handle = ( KernelFileHandle^ )_kernel->FindHandle( fd );
	if( handle == nullptr )
	{
		Debug::WriteLine( String::Format( "sceIoLseek: kernel file handle {0} not found", fd ) );
		return -1;
	}

	SeekOrigin seekOrigin = SeekOrigin::Current;
	switch( whence )
	{
		case 0:
			seekOrigin = SeekOrigin::Begin;
			//Debug.Assert( offset < handle.Stream.Length );
			if( offset > handle->Stream->Length )
			{
				offset = 0;
				//return ( int )handle.Stream.Position;
			}
			break;
		case 1:
			seekOrigin = SeekOrigin::Current;
			Debug::Assert( handle->Stream->Position + offset <= handle->Stream->Length );
			break;
		case 2:
			seekOrigin = SeekOrigin::End;
			Debug::Assert( handle->Stream->Length + offset <= handle->Stream->Length );
			break;
	}

	int64 newPosition = handle->Stream->Seek( offset, seekOrigin );
	return newPosition;
}

// int sceIoLseekAsync(SceUID fd, SceOff offset, int whence); (/user/pspiofilemgr.h:181)
int IoFileMgrForUser::sceIoLseekAsync( int fd, int64 offset, int whence ){ return NISTUBRETURN; }

// int sceIoLseek32(SceUID fd, int offset, int whence); (/user/pspiofilemgr.h:198)
int IoFileMgrForUser::sceIoLseek32( int fd, int offset, int whence )
{
	KernelFileHandle^ handle = ( KernelFileHandle^ )_kernel->FindHandle( fd );
	if( handle == nullptr )
	{
		Debug::WriteLine( String::Format( "sceIoLseek: kernel file handle {0} not found", fd ) );
		return -1;
	}

	SeekOrigin seekOrigin = SeekOrigin::Current;
	switch( whence )
	{
		case 0:
			seekOrigin = SeekOrigin::Begin;
			//Debug.Assert( offset < handle.Stream.Length );
			if( offset > handle->Stream->Length )
			{
				offset = 0;
				//return ( int )handle.Stream.Position;
			}
			break;
		case 1:
			seekOrigin = SeekOrigin::Current;
			Debug::Assert( handle->Stream->Position + offset < handle->Stream->Length );
			break;
		case 2:
			seekOrigin = SeekOrigin::End;
			Debug::Assert( handle->Stream->Length + offset < handle->Stream->Length );
			break;
	}

	int64 newPosition = handle->Stream->Seek( offset, seekOrigin );
	return ( int )newPosition;
}

// int sceIoLseek32Async(SceUID fd, int offset, int whence); (/user/pspiofilemgr.h:210)
int IoFileMgrForUser::sceIoLseek32Async( int fd, int offset, int whence ){ return NISTUBRETURN; }
