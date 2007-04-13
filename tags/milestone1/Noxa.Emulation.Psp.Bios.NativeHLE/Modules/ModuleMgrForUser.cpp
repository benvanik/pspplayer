// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "Stdafx.h"
#include "ModuleMgrForUser.h"
#include "Kernel.h"
#include "Loader.h"
#include "KModule.h"

using namespace System;
using namespace System::Diagnostics;
using namespace System::IO;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Bios;
using namespace Noxa::Emulation::Psp::Bios::Modules;

// int sceKernelSelfStopUnloadModule(int unknown, SceSize argsize, void *argp); (/user/pspmodulemgr.h:152)
int ModuleMgrForUser::sceKernelSelfStopUnloadModule( int status, int argsize, int argp )
{
	return this->sceKernelStopUnloadSelfModule( argsize, argp, status, 0 );
}

// int sceKernelStopUnloadSelfModule(SceSize argsize, void *argp, int *status, SceKernelSMOption *option); (/user/pspmodulemgr.h:164)
int ModuleMgrForUser::sceKernelStopUnloadSelfModule( int argsize, int argp, int status, int option )
{
	_kernel->StopGame( status );

	return 0;
}

// SceUID sceKernelLoadModuleByID(SceUID fid, int flags, SceKernelLMOption *option); (/user/pspmodulemgr.h:91)
int ModuleMgrForUser::sceKernelLoadModuleByID( IMemory^ memory, int fid, int flags, int option )
{
	KFile* handle = ( KFile* )_kernel->Handles->Lookup( fid );
	if( handle == NULL )
		return -1;

	assert( false );

	return 0;
}

// SceUID sceKernelLoadModule(const char *path, int flags, SceKernelLMOption *option); (/user/pspmodulemgr.h:68)
int ModuleMgrForUser::sceKernelLoadModule( IMemory^ memory, int path, int flags, int option )
{
	String^ modulePath = KernelHelpers::ReadString( memory, ( const int )path );
	IMediaFile^ file = ( IMediaFile^ )KernelHelpers::FindPath( _kernel, modulePath );
	if( file == nullptr )
	{
		Debug::WriteLine( String::Format( "sceKernelLoadModule: module not found: {0}", modulePath ) );
		return -1;
	}

	Debug::WriteLine( String::Format( "sceKernelLoadModule: loading module {0}", modulePath ) );

	Stream^ stream = nullptr;

	// Check to see if it's one we have a decoded version of
	String^ kernelLocation = Path::GetDirectoryName( System::Reflection::Assembly::GetExecutingAssembly()->Location );
	String^ prxLocation = Path::Combine( kernelLocation, "PRX" );
	String^ lookasidePrx = Path::Combine( prxLocation, file->Name );
	if( File::Exists( lookasidePrx ) == true )
	{
		// Load ours instead
		Debug::WriteLine( String::Format( "sceKernelLoadModule: lookaside prx found at {0}", lookasidePrx ) );
		stream = File::OpenRead( lookasidePrx );
	}
	else
	{
		// Load the given file
		stream = file->OpenRead();
	}

	Debug::Assert( stream != nullptr );
	if( stream == nullptr )
	{
		Debug::WriteLine( "sceKernelLoadModule: unable to load module" );
		return -1;
	}

	// Quick check to make sure it isn't encrypted before sending off to loader
	array<byte>^ magicBytes = gcnew array<byte>( 4 );
	stream->Read( magicBytes, 0, 4 );
	stream->Seek( 0, SeekOrigin::Begin );
	// 0x7E, 0x50, 0x53, 0x50 = ~PSP
	bool encrypted = (
		( magicBytes[ 0 ] == 0x7E ) &&
		( magicBytes[ 1 ] == 0x50 ) &&
		( magicBytes[ 2 ] == 0x53 ) &&
		( magicBytes[ 3 ] == 0x50 ) );
	//Debug::Assert( encrypted == false );
	if( encrypted == true )
	{
		Debug::WriteLine( "sceKernelLoadModule: module is encrypted - unable to load" );

		// We spoof the caller in to thinking we worked right... by just returning 0 ^_^
		KModule* kmod = new KModule( _kernel, nullptr );
		_kernel->Handles->Add( kmod );
		return kmod->UID;
	}

	NativeBios^ bios = _kernel->Bios;

	// Load!
	LoadParameters^ params = gcnew LoadParameters();
	params->Path = file->Parent;
	LoadResults^ results = bios->_loader->LoadModule( ModuleType::Prx, stream, params );
	Debug::Assert( results->Successful == true );
	if( results->Successful == false )
	{
		Debug::WriteLine( "sceKernelLoadModule: loader failed on LoadModule" );
		return -1;
	}

	// Create a local representation of the module
	BiosModule^ module = gcnew BiosModule( results->Name, results->Exports->ToArray() );
	bios->_metaModuleList->Add( module );
	bios->_metaModules->Add( module->Name, module );

	KModule* kmod = new KModule( _kernel, module );
	_kernel->Handles->Add( kmod );

	return kmod->UID;
}

// int sceKernelStartModule(SceUID modid, SceSize argsize, void *argp, int *status, SceKernelSMOption *option); (/user/pspmodulemgr.h:119)
int ModuleMgrForUser::sceKernelStartModule( IMemory^ memory, int modid, int argsize, int argp, int status, int option )
{
	KModule* kmod = ( KModule* )_kernel->Handles->Lookup( modid );
	assert( kmod != NULL );
	if( kmod == NULL )
		return -1;

	if( kmod->ModuleStart != NULL )
	{
		// Run module start?
	}

	return 0;
}
