// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "Stdafx.h"
#include "IoFileMgrForUser.h"
#include "Kernel.h"
#include "KernelDevice.h"

using namespace System;
using namespace System::Collections::Generic;
using namespace System::Diagnostics;
using namespace System::IO;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Bios;
using namespace Noxa::Emulation::Psp::Bios::Modules;
using namespace Noxa::Emulation::Psp::Media;

IMediaItem^ IoFileMgrForUser::FindPath( String^ path )
{
	if( path->IndexOf( ':' ) >= 0 )
	{
		KernelFileDevice^ device = ( KernelFileDevice^ )_kernel->FindDevice( path );
		if( device == nullptr )
		{
			// Perhaps a block device?
			Debug::WriteLine( String::Format( "IoFileMgrForUser::FindPath: unable to find device for path {0}", path ) );
			return nullptr;
		}

		path = path->Substring( path->IndexOf( ':' ) + 1 );
		if( ( device->MediaDevice->State == MediaState::Present ) &&
			( device->MediaRoot != nullptr ) )
		{
			return device->MediaRoot->Find( path );
		}
		else
		{
			Debug::WriteLine( String::Format( "IoFileMgrForUser::FindPath: unable to find root for path {0}", path ) );
			return nullptr;
		}
	}
	else
	{
		IMediaFolder^ root = _kernel->CurrentPath;
		Debug::Assert( root != nullptr );

		return root->Find( path );
	}
}

// int sceIoPollAsync(SceUID fd, SceInt64 *res); (/user/pspiofilemgr.h:419)
int IoFileMgrForUser::sceIoPollAsync( IMemory^ memory, int fd, int64 res ){ return NISTUBRETURN; }

// int sceIoWaitAsync(SceUID fd, SceInt64 *res); (/user/pspiofilemgr.h:399)
int IoFileMgrForUser::sceIoWaitAsync( IMemory^ memory, int fd, int64 res ){ return NISTUBRETURN; }

// int sceIoWaitAsyncCB(SceUID fd, SceInt64 *res); (/user/pspiofilemgr.h:409)
int IoFileMgrForUser::sceIoWaitAsyncCB( IMemory^ memory, int fd, int64 res ){ return NISTUBRETURN; }

// int sceIoGetAsyncStat(SceUID fd, int poll, SceInt64 *res); (/user/pspiofilemgr.h:430)
int IoFileMgrForUser::sceIoGetAsyncStat( IMemory^ memory, int fd, int poll, int64 res ){ return NISTUBRETURN; }

// int sceIoChangeAsyncPriority(SceUID fd, int pri); (/user/pspiofilemgr.h:458)
int IoFileMgrForUser::sceIoChangeAsyncPriority( int fd, int pri ){ return NISTUBRETURN; }

// int sceIoSetAsyncCallback(SceUID fd, SceUID cb, void *argp); (/user/pspiofilemgr.h:469)
int IoFileMgrForUser::sceIoSetAsyncCallback( IMemory^ memory, int fd, int cb, int argp ){ return NISTUBRETURN; }

// int sceIoCancel(SceUID fd); (/user/pspiofilemgr.h:439)
int IoFileMgrForUser::sceIoCancel( int fd ){ return NISTUBRETURN; }
