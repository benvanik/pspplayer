// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "StdAfx.h"
#include "MemorySegment.h"
#include <string>

using namespace System::Diagnostics;
using namespace System::IO;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Cpu;

MemorySegment::MemorySegment( Cpu::MemoryType type, String^ name, int baseAddress, int length )
{
	_type = type;
	_name = name;
	_baseAddress = baseAddress;
	_length = 0;
	_memory = NULL;
	this->Allocate( length );
}

MemorySegment::~MemorySegment(void)
{
	this->Clear();
}

MemorySegment::!MemorySegment(void)
{
	this->Clear();
}

void MemorySegment::Allocate( int size )
{
	this->Clear();

	_length = size;
	if( _length == 0 )
		_memory = NULL;
	else
		_memory = ( unsigned char * )calloc( _length, 1 );
}

void MemorySegment::Clear()
{
	if( ( _length != 0 ) &&
		( _memory != NULL ) )
	{
		_length = 0;
		free( _memory );
	}
}

int MemorySegment::ReadWord( int address )
{
	// Address won't be word aligned, and we index into bytes
	if( address < _baseAddress )
		return 0;
	unsigned char* base = _memory + address - _baseAddress;
	if( base + 4 > _memory + _length )
		return 0;

	int result = *(int*)base;
	//Debug::WriteLine( String::Format( "MemorySegment: ReadWord( 0x{0:X8} ) = 0x{1:X8}", address, result ) );

	return result;
}

array<unsigned char>^ MemorySegment::ReadBytes( int address, int count )
{
	if( address < _baseAddress )
		return nullptr;
	unsigned char* base = _memory + address - _baseAddress;
	if( base + count > _memory + _length )
		return nullptr;

	array<unsigned char>^ buffer = gcnew array<unsigned char>( count );
	for( int n = 0; n < count; n++ )
	{
		buffer[ n ] = *base;
		base++;
	}

	return buffer;
}

int MemorySegment::ReadStream( int address, Stream^ destination, int count )
{
	if( address < _baseAddress )
		return 0;
	unsigned char* base = _memory + address - _baseAddress;
	if( base + count > _memory + _length )
		return 0;

	for( int n = 0; n < count; n++ )
	{
		// This could stand to be optimized :)
		destination->WriteByte( *base );
		base++;
	}

	return count;
}

void MemorySegment::WriteWord( int address, int width, int value )
{
	if( address < _baseAddress )
		return;
	unsigned char* base = _memory + address - _baseAddress;
	if( base + width > _memory + _length )
		return;
	if( width == 1 )
		*base = (unsigned char)value;
	else if( width == 2 )
		*(unsigned short*)base = (unsigned short)value;
	else
		*(unsigned int*)base = (unsigned int)value;

	//if( this->MemoryChanged != nullptr )
	this->MemoryChanged( this, address, width, value );

	//Debug::WriteLine( String::Format( "MemorySegment: WriteWord( 0x{0:X8}, 0x{1:X8} ) width={2}", address, value, width ) );
}

void MemorySegment::WriteBytes( int address, array<unsigned char>^ bytes )
{
	this->WriteBytes( address, bytes, 0, bytes->Length );
}

void MemorySegment::WriteBytes( int address, array<unsigned char>^ bytes, int index, int count )
{
	if( address < _baseAddress )
		return;
	unsigned char* base = _memory + address - _baseAddress;
	if( base + count > _memory + _length )
		return;
	for( int n = index; n < index + count; n++ )
	{
		*base = bytes[ n ];
		base++;
	}
}

void MemorySegment::WriteStream( int address, Stream^ source, int count )
{
	if( address < _baseAddress )
		return;
	unsigned char* base = _memory + address - _baseAddress;
	if( base + count > _memory + _length )
		return;
	for( int n = 0; n < count; n++ )
	{
		*base = source->ReadByte();
		base++;
	}
}


void MemorySegment::Load( Stream^ stream )
{
}

void MemorySegment::Load( String^ fileName )
{
	Stream^ stream = nullptr;
	try
	{
		stream = File::OpenRead( fileName );
		this->Load( stream );
	}
	finally
	{
		stream->Close();
	}
}

void MemorySegment::Save( Stream^ stream )
{
}

void MemorySegment::Save( String^ fileName )
{
	Stream^ stream = nullptr;
	try
	{
		stream = File::OpenWrite( fileName );
		this->Save( stream );
	}
	finally
	{
		stream->Close();
	}
}

unsigned int MemorySegment::GetMemoryHash( int address, int count, unsigned int prime )
{
	throw gcnew System::NotImplementedException();
}
