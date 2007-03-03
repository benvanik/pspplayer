// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "Stdafx.h"
#include "Kernel.h"
#include "KernelDevice.h"
#include "KernelFileHandle.h"

using namespace System;
using namespace System::Collections::Generic;
using namespace System::Diagnostics;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Bios;

void Kernel::AddHandle( KernelHandle^ handle )
{
	Debug::Assert( handle != nullptr );
	_handles->Add( handle->ID, handle );
}

void Kernel::RemoveHandle( KernelHandle^ handle )
{
	Debug::Assert( handle != nullptr );
	if( _handles->ContainsKey( handle->ID ) == true )
		_handles->Remove( handle->ID );
}

__inline KernelHandle^ Kernel::FindHandle( int id )
{
	KernelHandle^ ret;
	if( _handles->TryGetValue( id, ret ) == true )
		return ret;
	else
		return nullptr;
}

__inline KernelDevice^ Kernel::FindDevice( String^ path )
{
	int colon = path->IndexOf( ':' );
	if( colon < 0 )
	{
		IMediaDevice^ device = CurrentPath->Device;
		for( int n = 0; n < _devices->Count; n++ )
		{
			KernelFileDevice^ fileDevice = ( KernelFileDevice^ )_devices[ n ];
			if( fileDevice != nullptr )
			{
				if( fileDevice->MediaDevice == device )
					return fileDevice;
			}
			else
			{
				KernelBlockDevice^ blockDevice = ( KernelBlockDevice^ )_devices[ n ];
				// TODO: block device lookup?
				Debug::Assert( false, "Block devices are not really supported" );
			}
		}
		return nullptr;
	}
	else
	{
		String^ dev = path->Substring( 0, colon )->ToLowerInvariant();
		KernelDevice^ ret;
		if( _deviceMap->TryGetValue( dev, ret ) == true )
			return ret;
		else
			return nullptr;
	}
}

void Kernel::CreateStdio()
{
	StdIn = gcnew KernelFileHandle( this->AllocateID() );
	StdIn->HandleType = KernelHandleType::Stdio;
	StdIn->CanWrite = false;
	StdIn->CanSeek = false;
	StdIn->IsOpen = true;
	StdIn->Stream = gcnew MemoryStream();
	this->AddHandle( StdIn );

	StdOut = gcnew KernelFileHandle( this->AllocateID() );
	StdOut->HandleType = KernelHandleType::Stdio;
	StdOut->CanWrite = true;
	StdOut->CanSeek = false;
	StdOut->IsOpen = true;
	StdOut->Stream = gcnew MemoryStream();
	this->AddHandle( StdOut );

	StdErr = gcnew KernelFileHandle( this->AllocateID() );
	StdErr->HandleType = KernelHandleType::Stdio;
	StdErr->CanWrite = true;
	StdErr->CanSeek = false;
	StdErr->IsOpen = true;
	StdErr->Stream = gcnew MemoryStream();
	this->AddHandle( StdErr );
}
