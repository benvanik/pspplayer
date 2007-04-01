// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "Stdafx.h"
#include "NativeBios.h"

using namespace System;
using namespace System::Collections::Generic;
using namespace System::Diagnostics;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Bios;

NativeBios::NativeBios( IEmulationInstance^ emulator, ComponentParameters^ parameters )
{
	Debug::Assert( emulator != nullptr );
	Debug::Assert( parameters != nullptr );

	_emulator = emulator;
	_parameters = parameters;

	_metaModuleList = gcnew List<BiosModule^>();
	_metaModules = gcnew Dictionary<String^, BiosModule^>();
	_moduleList = gcnew List<Module^>();
	_functionList = gcnew List<BiosFunction^>();
	_functions = gcnew Dictionary<uint, BiosFunction^>();

	Loader = gcnew Loader( this );
	Kernel = new Kernel();

	this->GatherFunctions();
}

NativeBios::~NativeBios()
{
	SAFEDELETE( Kernel );
}

void NativeBios::Cleanup()
{
	for( int n = 0; n < _moduleList->Count; n++ )
	{
		_moduleList[ n ]->Stop();
		_moduleList[ n ]->Clear();
	}
}

BiosModule^ NativeBios::FindModule( String^ name )
{
	if( _metaModules->ContainsKey( name ) == true )
		return ( BiosModule^ )_metaModules[ name ];
	return nullptr;
}

BiosFunction^ NativeBios::FindFunction( uint nid )
{
	if( _functions->ContainsKey( nid ) == true )
		return _functions[ nid ];
	return nullptr;
}

void NativeBios::ClearModules()
{
	for( int n = 0; n < _moduleList->Count; n++ )
		_moduleList[ n ]->Clear();
}

void NativeBios::StartModules()
{
	for( int n = 0; n < _moduleList->Count; n++ )
		_moduleList[ n ]->Start();
}

void NativeBios::StopModules()
{
	for( int n = 0; n < _moduleList->Count; n++ )
		_moduleList[ n ]->Stop();
}

void NativeBios::GatherFunctions()
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

		Module^ module = ( Module^ )Activator::CreateInstance( type, _kernel );
		Debug::Assert( module != nullptr );
		if( module == nullptr )
			continue;

		_moduleList->Add( module );
		
		BiosModule^ metaModule = gcnew BiosModule( module->Name );
		_metaModuleList->Add( metaModule );
		_metaModules->Add( metaModule->Name, metaModule );

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

			IntPtr nativePointer = IntPtr::Zero;
			if( isImplemented == true )
			{
				implementedCount++;

				void* nativePtr = module->QueryNativePointer( attr->NID );
				if( nativePtr != 0x0 )
					nativePointer = IntPtr( nativePtr );
			}

			BiosFunction^ function = gcnew BiosFunction(
				metaModule,
				attr->NID, attr->Name,
				isImplemented, isStateless,
				mi, nativePointer );
			this->RegisterFunction( function );
		}
	}

	Debug::WriteLine( String::Format( "NativeBios: found {0} functions in {1} modules. {2} ({3}%) implemented", functionCount, moduleCount, implementedCount, ( implementedCount / ( float )functionCount ) * 100.0f ) );
}

void NativeBios::RegisterFunction( BiosFunction^ function )
{
	Debug::Assert( function != nullptr );
	if( _functions->ContainsKey( function->NID ) == true )
	{
		Debug::WriteLine( String::Format( "NativeBios::RegisterFunction: NID 0x{0:X8} already registered", function->NID ) );
		return;
	}
	_functions->Add( function->NID, function );
	_functionList->Add( function );
}
