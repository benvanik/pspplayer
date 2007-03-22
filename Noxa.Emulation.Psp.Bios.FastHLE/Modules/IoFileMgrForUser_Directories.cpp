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
using namespace Noxa::Emulation::Psp::Media;

// SceUID sceIoDopen(const char *dirname); (/user/pspiofilemgr.h:267)
int IoFileMgrForUser::sceIoDopen( IMemory^ memory, int dirname )
{
	String^ path = KernelHelpers::ReadString( memory, dirname );
	IMediaFolder^ folder = ( IMediaFolder^ )this->FindPath( path );
	if( folder == nullptr )
	{
		Debug::WriteLine( String::Format( "sceIoDopen: could not find path {0}", path ) );
		return -1;
	}

	KernelFileHandle^ handle = gcnew KernelFileHandle( _kernel->AllocateID() );
	handle->Device = ( KernelFileDevice^ )_kernel->FindDevice( path );
	handle->CanWrite = !handle->Device->ReadOnly;
	handle->CanSeek = handle->Device->IsSeekable;
	handle->IsOpen = true;
	handle->MediaItem = folder;
	handle->FolderOffset = -2;
	_kernel->AddHandle( handle );

#ifdef VERBOSEIO
	Debug::WriteLine( String::Format( "sceIoDopen: opened {0} with ID {1}", path, handle->ID ) );
#endif

	return handle->ID;
}

// int sceIoDread(SceUID fd, SceIoDirent *dir); (/user/pspiofilemgr.h:280)
int IoFileMgrForUser::sceIoDread( IMemory^ memory, int fd, int dir )
{
	KernelFileHandle^ handle = ( KernelFileHandle^ )_kernel->FindHandle( fd );
	if( handle == nullptr )
	{
		Debug::WriteLine( String::Format( "sceIoDread: kernel file handle {0} not found", fd ) );
		return -1;
	}

	int offset = handle->FolderOffset;
	handle->FolderOffset++;

	IMediaFolder^ folder = ( IMediaFolder^ )handle->MediaItem;
	if( offset == folder->Items->Length )
		return 0;

	// (d_stat)
	//	SceMode			st_mode 
	//	unsigned int	st_attr 
	//	SceOff			st_size  (64)
	//	ScePspDateTime	st_ctime 
	//	ScePspDateTime	st_atime 
	//	ScePspDateTime	st_mtime 
	//	unsigned int	st_private [6]
	//char				d_name [256] 
	//void*				d_private 
	//int				dummy

	if( ( offset == -2 ) ||
		( offset == -1 ) )
	{
		int address = dir;
		memory->WriteWord( address, 4, 0777 );
		address += 4;

		uint attributes = 0x0010;
		memory->WriteWord( address, 4, ( int )attributes );
		address += 4;

		memory->WriteWord( address, 4, 0 );
		memory->WriteWord( address + 4, 4, 0 );
		address += 8;

		address += KernelHelpers::WriteTime( memory, address, DateTime::Now );
		address += KernelHelpers::WriteTime( memory, address, DateTime::Now );
		address += KernelHelpers::WriteTime( memory, address, DateTime::Now );

		address += 6 * 4; // no private stat data - blank here?

		String^ name = ( offset == -2 ) ? "." : "..";
		int nameLength = KernelHelpers::WriteString( memory, address, name );
		address += 256; // Maybe blank here?

		memory->WriteWord( address, 4, 0 );
		address += 4; // no private dir data

		memory->WriteWord( address, 4, 0 );
		address += 4;
	}
	else
	{
		IMediaItem^ child = folder->Items[ offset ];
		IMediaFolder^ childFolder = nullptr;
		IMediaFile^ childFile = nullptr;
		if( child->GetType()->GetInterface( "IMediaFolder", false ) != nullptr )
			childFolder = ( IMediaFolder^ )child;
		else
			childFile = ( IMediaFile^ )child;

		int address = dir;
		memory->WriteWord( address, 4, 0777 );
		address += 4;

		uint attributes = 0;
		//if( ( child.Attributes & MediaItemAttributes.Hidden ) == MediaItemAttributes.Hidden )
		//	attributes |= 0;
		//if( ( child.Attributes & MediaItemAttributes.ReadOnly ) == MediaItemAttributes.ReadOnly )
		//	attributes |= 0;
		if( childFile != nullptr )
			attributes |= 0x0020;
		if( childFolder != nullptr )
			attributes |= 0x0010;
		if( child->IsSymbolicLink == true )
			attributes |= 0x0008;
		memory->WriteWord( address, 4, ( int )attributes );
		address += 4;

		if( childFile != nullptr )
			memory->WriteWord( address, 4, ( int )childFile->Length );
		else
			memory->WriteWord( address, 4, 0 );
		memory->WriteWord( address + 4, 4, 0 );
		address += 8;

		address += KernelHelpers::WriteTime( memory, address, child->CreationTime );
		address += KernelHelpers::WriteTime( memory, address, child->AccessTime );
		address += KernelHelpers::WriteTime( memory, address, child->ModificationTime );

		address += 6 * 4; // no private stat data - blank here?

		int nameLength = KernelHelpers::WriteString( memory, address, child->Name );
		address += 256; // Maybe blank here?

		memory->WriteWord( address, 4, 0 );
		address += 4; // no private dir data

		memory->WriteWord( address, 4, 0 );
		address += 4;
	}

	// 0 to stop, 1 to keep going
	return 1;
	}

// int sceIoDclose(SceUID fd); (/user/pspiofilemgr.h:288)
int IoFileMgrForUser::sceIoDclose( int fd )
{
	KernelFileHandle^ handle = ( KernelFileHandle^ )_kernel->FindHandle( fd );
	if( handle == nullptr )
	{
		Debug::WriteLine( String::Format( "sceIoDclose: kernel file handle {0} not found", fd ) );
		return -1;
	}

	handle->IsOpen = false;
	_kernel->RemoveHandle( handle );

	return 0;
}

// int sceIoRemove(const char *file); (/user/pspiofilemgr.h:218)
int IoFileMgrForUser::sceIoRemove( IMemory^ memory, int fileName )
{
	String^ path = KernelHelpers::ReadString( memory, fileName );
	IMediaFile^ file = ( IMediaFile^ )this->FindPath( path );
	if( file == nullptr )
	{
		Debug::WriteLine( String::Format( "sceIoRemove: could not find path '{0}'", path ) );
		return -1;
	}

	file->Delete();

	return 0;
}

// int sceIoMkdir(const char *dir, SceMode mode); (/user/pspiofilemgr.h:227)
int IoFileMgrForUser::sceIoMkdir( IMemory^ memory, int dir, int mode )
{
	// will be absolute for full path or relative
	String^ path = KernelHelpers::ReadString( memory, dir );

	// Ensure the path does not exist
	if( this->FindPath( path ) != nullptr )
		return 0;

	if( path[ path->Length - 1 ] == '/' )
		path = path->Substring( 0, path->Length - 1 );
	String^ parentPath = path->Substring( 0, path->LastIndexOf( '/' ) );
	String^ newName = path->Substring( path->LastIndexOf( '/' ) + 1 );

	IMediaFolder^ folder = ( IMediaFolder^ )this->FindPath( parentPath );
	if( folder == nullptr )
	{
		Debug::WriteLine( String::Format( "sceIoMkdir: could not find path '{0}'", path ) );
		return -1;
	}
	
	// TODO: something with mode
	IMediaFolder^ newFolder = folder->CreateFolder( newName );

	return 0;
}

// int sceIoRmdir(const char *path); (/user/pspiofilemgr.h:235)
int IoFileMgrForUser::sceIoRmdir( IMemory^ memory, int pathName )
{
	String^ path = KernelHelpers::ReadString( memory, pathName );
	IMediaFolder^ folder = ( IMediaFolder^ )this->FindPath( path );
	if( folder == nullptr )
	{
		Debug::WriteLine( String::Format( "sceIoRmdir: could not find path '{0}'", path ) );
		return -1;
	}

	folder->Delete();

	return 0;
}

// int sceIoChdir(const char *path); (/user/pspiofilemgr.h:243)
int IoFileMgrForUser::sceIoChdir( IMemory^ memory, int pathName )
{
	String^ path = KernelHelpers::ReadString( memory, pathName );
	IMediaFolder^ folder = ( IMediaFolder^ )this->FindPath( path );
	if( folder == nullptr )
	{
		Debug::WriteLine( String::Format( "sceIoChdir: could not find path '{0}'", path ) );
		return -1;
	}

	_kernel->CurrentPath = folder;

	return 0;
}

// int sceIoSync(const char *device, unsigned int unk); (/user/pspiofilemgr.h:389)
int IoFileMgrForUser::sceIoSync( IMemory^ memory, int device, int unk ){ return NISTUBRETURN; }

// int sceIoGetstat(const char *file, SceIoStat *stat); (/user/pspiofilemgr.h:344)
int IoFileMgrForUser::sceIoGetstat( IMemory^ memory, int fileName, int stat )
{
	int address = stat;

	String^ path = KernelHelpers::ReadString( memory, fileName );
	IMediaItem^ item = this->FindPath( path );
	if( item == nullptr )
	{
		Debug::WriteLine( String::Format( "sceIoGetstat: could not find path '{0}'", path ) );
		return -1;
	}

	int mode = 0;
	int attr = 0;

	if( item->IsSymbolicLink == true )
	{
		mode |= 0x4000;
		attr |= 0x0008;
	}

	//if( ( item.Attributes & MediaItemAttributes.Hidden ) == MediaItemAttributes.Hidden )
	mode |= 0x0100 | 0x0020 | 0x004; // read user / group /others
	attr |= 0x0004; // read

	if( ( item->Attributes & MediaItemAttributes::ReadOnly ) == MediaItemAttributes::ReadOnly )
	{
		mode |= 0x0080 | 0x0010 | 0x0002; // write user / group / others
		attr |= 0x0002; // write
	}

	mode |= 0x0040 | 0x0008 | 0x0001; // exec user / group / others
	attr |= 0x0001; // execute

	uint size = 0;
	if( item->GetType() == IMediaFile::typeid )
	{
		IMediaFile^ file = ( IMediaFile^ )file;
		mode |= 0x2000; // file
		attr |= 0x0020; // file
		size = ( uint )file->Length;
	}
	else if( item->GetType() == IMediaFolder::typeid )
	{
		mode |= 0x1000; // dir
		attr |= 0x0010; // directory
	}

	memory->WriteWord( address, 4, mode );
	address += 4;
	memory->WriteWord( address, 4, attr );
	address += 4;
	// Is this the right order for 64 bit?
	memory->WriteWord( address, 4, ( int )size );
	memory->WriteWord( address + 0, 4, 0 );
	address += 8;
	address += KernelHelpers::WriteTime( memory, address, item->CreationTime );
	address += KernelHelpers::WriteTime( memory, address, item->AccessTime );
	address += KernelHelpers::WriteTime( memory, address, item->ModificationTime );
	
	// + 6 words of garbage

	return 0;
}

// int sceIoChstat(const char *file, SceIoStat *stat, int bits); (/user/pspiofilemgr.h:355)
int IoFileMgrForUser::sceIoChstat( IMemory^ memory, int file, int stat, int bits ){ return NISTUBRETURN; }

// int sceIoRename(const char *oldname, const char *newname); (/user/pspiofilemgr.h:252)
int IoFileMgrForUser::sceIoRename( IMemory^ memory, int oldname, int newname ){ return NISTUBRETURN; }
