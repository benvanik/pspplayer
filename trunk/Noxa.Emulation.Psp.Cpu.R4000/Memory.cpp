#include "StdAfx.h"
#include "Memory.h"

#include <string>
#include "MemorySegment.h"

using namespace System::Diagnostics;
using namespace System::IO;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Cpu;

Memory::Memory(void)
{
	_segs = gcnew List<IMemorySegment^>();
}

Memory::~Memory(void)
{
	this->Clear();
}

Memory::!Memory(void)
{
	this->Clear();
}

void Memory::Clear()
{
	_segs->Clear();
}

IMemorySegment^ Memory::DefineSegment( MemoryType type, String^ name, int baseAddress, int length )
{
	MemorySegment^ seg = gcnew MemorySegment( type, name, baseAddress, length );
	_segs->Add( seg );
	return seg;
}

void Memory::RegisterSegment( IMemorySegment^ segment )
{
	for( int n = 0; n < _segs->Count; n++ )
	{
		if( _segs[ n ]->BaseAddress == segment->BaseAddress )
		{
			_segs[ n ] = segment;
			return;
		}
	}
	_segs->Add( segment );
}

IMemorySegment^ Memory::FindSegment( String^ name )
{
	for( int n = 0; n < _segs->Count; n++ )
	{
		if( _segs[ n ]->Name == name )
			return _segs[ n ];
	}
	return nullptr;
}

IMemorySegment^ Memory::FindSegment( int baseAddress )
{
	for( int n = 0; n < _segs->Count; n++ )
	{
		if( _segs[ n ]->BaseAddress == baseAddress )
			return _segs[ n ];
	}
	return nullptr;
}

int Memory::ReadWord( int address )
{
	for( int n = 0; n < _segs->Count; n++ )
	{
		IMemorySegment^ seg = _segs[ n ];
		if( ( seg->BaseAddress <= address ) &&
			( seg->BaseAddress + seg->Length > address ) )
		{
			return seg->ReadWord( address );
		}
	}
	Debug::WriteLine( String::Format( "Memory::ReadWord to undefined seg 0x{0:X8}", (int^)address ) );
	return 0;
}

array<unsigned char>^ Memory::ReadBytes( int address, int count )
{
	for( int n = 0; n < _segs->Count; n++ )
	{
		IMemorySegment^ seg = _segs[ n ];
		if( ( seg->BaseAddress <= address ) &&
			( seg->BaseAddress + seg->Length > address ) )
		{
			// TODO: Ensure along segment bounds
			return seg->ReadBytes( address, count );
		}
	}
	Debug::WriteLine( String::Format( "Memory::ReadBytes to undefined seg 0x{0:X8}", (int^)address ) );
	return nullptr;
}

int Memory::ReadStream( int address, Stream^ destination, int count )
{
	for( int n = 0; n < _segs->Count; n++ )
	{
		IMemorySegment^ seg = _segs[ n ];
		if( ( seg->BaseAddress <= address ) &&
			( seg->BaseAddress + seg->Length > address ) )
		{
			// TODO: ensure along segment bounds
			return seg->ReadStream( address, destination, count );
		}
	}
	Debug::WriteLine( String::Format( "Memory::ReadStream to undefined seg 0x{0:X8}", (int^)address ) );
	return 0;
}

void Memory::WriteWord( int address, int width, int value )
{
	for( int n = 0; n < _segs->Count; n++ )
	{
		IMemorySegment^ seg = _segs[ n ];
		if( ( seg->BaseAddress <= address ) &&
			( seg->BaseAddress + seg->Length > address ) )
		{
			seg->WriteWord( address, width, value );
			return;
		}
	}
	Debug::WriteLine( String::Format( "Memory::WriteWord to undefined seg 0x{0:X8}", (int^)address ) );
}

void Memory::WriteBytes( int address, array<unsigned char>^ bytes )
{
	this->WriteBytes( address, bytes, 0, bytes->Length );
}

void Memory::WriteBytes( int address, array<unsigned char>^ bytes, int index, int count )
{
	for( int n = 0; n < _segs->Count; n++ )
	{
		IMemorySegment^ seg = _segs[ n ];
		if( ( seg->BaseAddress <= address ) &&
			( seg->BaseAddress + seg->Length > address ) )
		{
			seg->WriteBytes( address, bytes, index, count );
			return;
		}
	}
	Debug::WriteLine( String::Format( "Memory::WriteBytes to undefined seg 0x{0:X8}", (int^)address ) );
}

void Memory::WriteStream( int address, Stream^ source, int count )
{
	for( int n = 0; n < _segs->Count; n++ )
	{
		IMemorySegment^ seg = _segs[ n ];
		if( ( seg->BaseAddress <= address ) &&
			( seg->BaseAddress + seg->Length > address ) )
		{
			seg->WriteStream( address, source, count );
			return;
		}
	}
	Debug::WriteLine( String::Format( "Memory::WriteStream to undefined seg 0x{0:X8}", (int^)address ) );
}

void Memory::Load( Stream^ stream )
{
}

void Memory::Load( String^ fileName )
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

void Memory::Save( Stream^ stream )
{
}

void Memory::Save( String^ fileName )
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
