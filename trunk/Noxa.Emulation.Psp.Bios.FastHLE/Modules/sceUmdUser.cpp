// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "Stdafx.h"
#define WIN32_LEAN_AND_MEAN
#include <Windows.h>

#include "sceUmdUser.h"
#include "Kernel.h"

using namespace System;
using namespace System::Diagnostics;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Bios;
using namespace Noxa::Emulation::Psp::Bios::Modules;
using namespace Noxa::Emulation::Psp::Media;

enum UmdStatus
{
	UmdInit			= 0x00,
	UmdMediaOut		= 0x01,
	UmdMediaIn		= 0x02,
	UmdMediaChange	= 0x04,
	UmdNotReady		= 0x08,
	UmdReady		= 0x10,
	UmdReadable		= 0x20,
};

enum UmdMode
{
	UmdPowerOn		= 0x01,
	UmdPowerCurrent	= 0x02,
};

// int sceUmdCheckMedium(); (/umd/pspumd.h:42)
int sceUmdUser::sceUmdCheckMedium()
{
	IUmdDevice^ umd = _kernel->_emu->Umd;
	if( umd == nullptr )
		return 0;
	else
	{
		if( umd->State == MediaState::Present )
			return 1;
		else
			return 0;
	}
}

// int sceUmdActivate(int unit, const char *drive); (/umd/pspumd.h:66)
int sceUmdUser::sceUmdActivate( IMemory^ memory, int unit, int drive )
{
	Debug::WriteLine( String::Format( "sceUmdActivate: activating unit {0} / drive {1}", unit, KernelHelpers::ReadString( memory, drive ) ) );

	return 0;
}

// manual add
int sceUmdUser::sceUmdDeactivate( IMemory^ memory, int unit, int drive )
{
	Debug::WriteLine( String::Format( "sceUmdDeactivate: deactivating unit {0} / drive {1}", unit, KernelHelpers::ReadString( memory, drive ) ) );

	return 0;
}

// manual add
int sceUmdUser::sceUmdGetDriveStat()
{
	//return ( UmdInit | UmdMediaIn | UmdReady | UmdReadable );
	return ( UmdMediaIn | UmdReadable );
}

// manual add
int sceUmdUser::sceUmdGetErrorStat()
{
	// Unknown if this is right or not!
	return 0;
}

// int sceUmdWaitDriveStat(int stat); (/umd/pspumd.h:75)
int sceUmdUser::sceUmdWaitDriveStat( int stat )
{
	return 0;
}

// manual add - params not right?
int sceUmdUser::sceUmdWaitDriveStatWithTimer( int stat )
{
	return 0;
}

// manual add
int sceUmdUser::sceUmdWaitDriveStatCB( int stat )
{
	return 0;
}

// int sceUmdRegisterUMDCallBack(int cbid); (/umd/pspumd.h:89)
int sceUmdUser::sceUmdRegisterUMDCallBack( int cbid )
{
	KernelCallback^ cb = ( KernelCallback^ )_kernel->FindHandle( cbid );
	if( cb == nullptr )
		return -1;

	// Don't support more than one callback
	Debug::Assert( _kernel->Callbacks->ContainsKey( KernelCallbackType::Umd ) == false );

	_kernel->Callbacks->Add( KernelCallbackType::Umd, cb );

	return 0;
}

// manual add
int sceUmdUser::sceUmdUnRegisterUMDCallBack( int cbid )
{
	_kernel->Callbacks->Remove( KernelCallbackType::Umd );

	return 0;
}

// manual add
int sceUmdUser::sceUmdGetDiscInfo( IMemory^ memory, int discInfo )
{
	IUmdDevice^ umd = _kernel->_emu->Umd;
	if( umd == nullptr )
		return -1;

	memory->WriteWord( discInfo, 4, ( int )umd->Capacity );
	memory->WriteWord( discInfo + 4, 4, ( int )umd->DiscType );

	return 0;
}
