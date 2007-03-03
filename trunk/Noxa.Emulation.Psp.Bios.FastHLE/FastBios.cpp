// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "Stdafx.h"
#include "FastBios.h"
#include "Kernel.h"
#include "Module.h"

using namespace System;
using namespace System::Collections::Generic;
using namespace System::Diagnostics;
//using namespace System::Reflection;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Bios;

FastBios::FastBios( IEmulationInstance^ emulator, ComponentParameters^ parameters )
{
	Debug::Assert( emulator != nullptr );
	Debug::Assert( parameters != nullptr );

	_emulator = emulator;
	_parameters = parameters;

	_moduleList = gcnew List<Module^>();
	_modules = gcnew Dictionary<String^, Module^>();
	_functionList = gcnew List<BiosFunction^>();
	_functions = gcnew Dictionary<uint, BiosFunction^>();

	_kernel = gcnew Bios::Kernel( this );

	this->GatherFunctions();
}

void FastBios::Cleanup()
{
	for( int n = 0; n < _moduleList->Count; n++ )
	{
		_moduleList[ n ]->Stop();
		_moduleList[ n ]->Clear();
	}
}

void FastBios::ClearModules()
{
	for( int n = 0; n < _moduleList->Count; n++ )
		_moduleList[ n ]->Clear();
}

void FastBios::StartModules()
{
	for( int n = 0; n < _moduleList->Count; n++ )
		_moduleList[ n ]->Start();
}

void FastBios::StopModules()
{
	for( int n = 0; n < _moduleList->Count; n++ )
		_moduleList[ n ]->Stop();
}

void FastBios::GatherFunctions()
{
	int moduleCount = 0;
	int functionCount = 0;
	int implementedCount = 0;

	for each( Type^ type in Reflection::Assembly::GetCallingAssembly()->GetTypes() )
	{
		if( type->GetInterface( "Noxa.Emulation.Psp.Bios.IModule" ) == nullptr )
			continue;
		if( type->IsAbstract == true )
			continue;

		Module^ module = ( Module^ )Activator::CreateInstance( type, this->_kernel );
		Debug::Assert( module != nullptr );
		if( module == nullptr )
			continue;

		_moduleList->Add( module );
		_modules->Add( module->Name, module );

		moduleCount++;

		for each( Reflection::MethodInfo^ mi in type->GetMethods() )
		{
			array<Object^>^ attrs = mi->GetCustomAttributes( BiosFunctionAttribute::typeid, false );
			if( attrs->Length == 0 )
				continue;
			BiosFunctionAttribute^ attr = ( BiosFunctionAttribute^ )attrs[ 0 ];

			bool isImplemented = ( mi->GetCustomAttributes( NotImplementedAttribute::typeid, false )->Length == 0 );
			bool isStateless = ( mi->GetCustomAttributes( StatelessAttribute::typeid, false )->Length > 0 );

			functionCount++;
			if( isImplemented == true )
				implementedCount++;

			BiosFunction^ function = gcnew BiosFunction( module,
				attr->NID, attr->Name,
				isImplemented, isStateless,
				mi );
			this->RegisterFunction( function );

			if( isImplemented == true )
			{
				void* nativePointer = module->QueryNativePointer( attr->NID );
				if( nativePointer != 0x0 )
					function->NativeMethod = IntPtr( nativePointer );
			}
		}
	}

	Debug::WriteLine( String::Format( "FastBios: found {0} functions in {1} modules. {2} ({3}%) implemented", functionCount, moduleCount, implementedCount, ( implementedCount / functionCount ) ) );
}

void FastBios::RegisterFunction( BiosFunction^ function )
{
	Debug::Assert( function != nullptr );
	if( _functions->ContainsKey( function->NID ) == true )
	{
		Debug::WriteLine( String::Format( "FastBios::RegisterFunction: NID 0x{0:X8} already registered", function->NID ) );
		return;
	}
	_functions->Add( function->NID, function );
	_functionList->Add( function );
}

void FastBios::UnregisterFunction( uint nid )
{
	throw gcnew NotImplementedException();
}
