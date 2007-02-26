// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "StdAfx.h"
#include "R4000Memory.h"
#include <string>
//#include "MemorySegment.h"

using namespace System::Diagnostics;
using namespace System::IO;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Cpu;

#define MainMemoryBase			0x08000000
#define MainMemorySize			0x01FFFFFF
#define MainMemoryBound			( MainMemoryBase + MainMemorySize )
#define ScratchPadBase			0x00010000
#define ScratchPadSize			0x00003FFF
#define ScratchPadBound			( ScratchPadBase + ScratchPadSize )
#define FrameBufferBase			0x04000000
#define FrameBufferSize			0x001FFFFF
#define FrameBufferBound		( FrameBufferBase + FrameBufferSize )

R4000Memory::R4000Memory()
{
	MainMemory = ( byte* )malloc( MainMemorySize );
	_scratchPad = ( byte* )malloc( ScratchPadSize );
	_frameBufferBytes = ( byte* )malloc( FrameBufferSize );

	memset( MainMemory, 0x0, MainMemorySize );
}

R4000Memory::~R4000Memory()
{
	this->Clear();
}

R4000Memory::!R4000Memory()
{
	this->Clear();
}

void R4000Memory::Clear()
{
	SAFEFREE( MainMemory );
	SAFEFREE( _scratchPad );
	SAFEFREE( _frameBufferBytes );
}

IMemorySegment^ R4000Memory::DefineSegment( MemoryType type, String^ name, int baseAddress, int length )
{
	throw gcnew NotSupportedException();
}

void R4000Memory::RegisterSegment( IMemorySegment^ segment )
{
	if( segment->BaseAddress == FrameBufferBase )
	{
		SAFEFREE( _frameBufferBytes );
		_frameBuffer = segment;
	}
	else
	{
		Debug::Assert( false, "Cannot override any segmenet but the framebuffer." );
	}
}

IMemorySegment^ R4000Memory::FindSegment( String^ name )
{
	throw gcnew NotSupportedException();
}

IMemorySegment^ R4000Memory::FindSegment( int baseAddress )
{
	throw gcnew NotSupportedException();
}

int R4000Memory::ReadWord( int address )
{
	//Debug::WriteLine( String::Format( "RW @ 0x{0:X8}", address ) );
	if( ( address >= MainMemoryBase ) && ( address < MainMemoryBound ) )
	{
		int* ptr = ( int* )( MainMemory + ( address - MainMemoryBase ) );
		return *ptr;
	}
	else if( ( address >= ScratchPadBase ) && ( address < ScratchPadBound ) )
	{
		int* ptr = ( int* )( _scratchPad + ( address - ScratchPadBase ) );
		return *ptr;
	}
	else if( ( address >= FrameBufferBase ) && ( address < FrameBufferBound ) )
	{
		if( _frameBuffer != nullptr )
			return _frameBuffer->ReadWord( address );
		else
		{
			int* ptr = ( int* )( _frameBufferBytes + ( address - FrameBufferBase ) );
			return *ptr;
		}
	}
	else
	{
		Debugger::Break();
		return 0;
	}
}

array<byte>^ R4000Memory::ReadBytes( int address, int count )
{
	if( ( address >= MainMemoryBase ) && ( address < MainMemoryBound ) )
	{
		array<byte>^ buffer = gcnew array<byte>( count );
		pin_ptr<byte> ptr = &buffer[ 0 ];
		memcpy( ptr, MainMemory + ( address - MainMemoryBase ), count );
		return buffer;
	}
	else if( ( address >= ScratchPadBase ) && ( address < ScratchPadBound ) )
		throw gcnew NotImplementedException();
	else if( ( address >= FrameBufferBase ) && ( address < FrameBufferBound ) )
		throw gcnew NotImplementedException();
	else
		return nullptr;
}

int R4000Memory::ReadStream( int address, Stream^ destination, int count )
{
	if( ( address >= MainMemoryBase ) && ( address < MainMemoryBound ) )
	{
		//long pos = destination.Position;
		array<byte>^ buffer = gcnew array<byte>( count );
		destination->Write( buffer, 0, count );
		pin_ptr<byte> ptr = &buffer[ 0 ];
		memcpy( ptr, MainMemory + ( address - MainMemoryBase ), count );
		//destination.Position = pos;
		return count;
	}
	else if( ( address >= ScratchPadBase ) && ( address < ScratchPadBound ) )
		throw gcnew NotImplementedException();
	else if( ( address >= FrameBufferBase ) && ( address < FrameBufferBound ) )
		throw gcnew NotImplementedException();
	else
		return 0;
}

void R4000Memory::WriteWord( int address, int width, int value )
{
	if( ( address >= MainMemoryBase ) && ( address < MainMemoryBound ) )
	{
		byte* ptr = MainMemory + ( address - MainMemoryBase );
		switch( width )
		{
		case 4:
			{
				int* p = ( int* )ptr;
				*p = value;
			}
			break;
		case 1:
			*ptr = ( byte )value;
			break;
		case 2:
			{
				short* p = ( short* )ptr;
				*p = ( short )value;
			}
			break;
		default:
			Debug::Assert( false, "Unsupported width." );
			break;
		}
	}
	else if( ( address >= ScratchPadBase ) && ( address < ScratchPadBound ) )
	{
		byte* ptr = _scratchPad + ( address - ScratchPadBase );
		switch( width )
		{
		case 4:
			{
				int* p = ( int* )ptr;
				*p = value;
			}
			break;
		case 1:
			*ptr = ( byte )value;
			break;
		case 2:
			{
				short* p = ( short* )ptr;
				*p = ( short )value;
			}
			break;
		default:
			Debug::Assert( false, "Unsupported width." );
			break;
		}
	}
	else if( ( address >= FrameBufferBase ) && ( address < FrameBufferBound ) )
	{
		if( _frameBuffer != nullptr )
			_frameBuffer->WriteWord( address, width, value );
		else
		{
			byte* ptr = _frameBufferBytes + ( address - FrameBufferBase );
			switch( width )
			{
			case 4:
				{
					int* p = ( int* )ptr;
					*p = value;
				}
				break;
			case 1:
				*ptr = ( byte )value;
				break;
			case 2:
				{
					short* p = ( short* )ptr;
					*p = ( short )value;
				}
				break;
			default:
				Debug::Assert( false, "Unsupported width." );
				break;
			}
		}
	}
	else
	{
		Debugger::Break();
	}

	//if( this->MemoryChanged != nullptr )
	//this->MemoryChanged( this, address, width, value );
}

void R4000Memory::WriteBytes( int address, array<byte>^ bytes )
{
	if( ( address >= MainMemoryBase ) && ( address < MainMemoryBound ) )
	{
		pin_ptr<byte> ptr = &bytes[ 0 ];
		memcpy( MainMemory + ( address - MainMemoryBase ), ptr, bytes->Length );
	}
	else if( ( address >= ScratchPadBase ) && ( address < ScratchPadBound ) )
	{
		pin_ptr<byte> ptr = &bytes[ 0 ];
		memcpy( _scratchPad + ( address - ScratchPadBase ), ptr, bytes->Length );
	}
	else if( ( address >= FrameBufferBase ) && ( address < FrameBufferBound ) )
	{
		if( _frameBuffer != nullptr )
			_frameBuffer->WriteBytes( address, bytes );
		else
		{
			pin_ptr<byte> ptr = &bytes[ 0 ];
			memcpy( _frameBufferBytes + ( address - FrameBufferBase ), ptr, bytes->Length );
		}
	}
	else
		Debugger::Break();
}

void R4000Memory::WriteBytes( int address, array<byte>^ bytes, int index, int count )
{
	throw gcnew NotSupportedException();
}

void R4000Memory::WriteStream( int address, Stream^ source, int count )
{
	if( count == 0 )
		return;
	if( ( address >= MainMemoryBase ) && ( address < MainMemoryBound ) )
	{
		//long pos = source.Position;
		array<byte>^ buffer = gcnew array<byte>( count );
		source->Read( buffer, 0, count );
		pin_ptr<byte> ptr = &buffer[ 0 ];
		memcpy( MainMemory + ( address - MainMemoryBase ), ptr, count );
		//source.Position = pos;
	}
	else if( ( address >= ScratchPadBase ) && ( address < ScratchPadBound ) )
		throw gcnew NotImplementedException();
	else if( ( address >= FrameBufferBase ) && ( address < FrameBufferBound ) )
		throw gcnew NotImplementedException();
	else
		Debugger::Break();
}

void R4000Memory::Load( Stream^ stream )
{
	throw gcnew NotImplementedException();
}

void R4000Memory::Load( String^ fileName )
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

void R4000Memory::Save( Stream^ stream )
{
	throw gcnew NotImplementedException();
}

void R4000Memory::Save( String^ fileName )
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

uint R4000Memory::GetMemoryHash( int address, int count, uint prime )
{
	if( ( address >= MainMemoryBase ) && ( address < MainMemoryBound ) )
	{
		uint hash = ( uint )count;
		byte *ptr = MainMemory + ( address - MainMemoryBase );
		for( int n = 0; n < count; n++ )
		{
			hash = hash + *ptr;
			ptr++;
		}
		return hash % prime;
	}
	else if( ( address >= ScratchPadBase ) && ( address < ScratchPadBound ) )
		throw gcnew NotImplementedException();
	else if( ( address >= FrameBufferBase ) && ( address < FrameBufferBound ) )
		throw gcnew NotImplementedException();
	else
		throw gcnew NotSupportedException();
}

void R4000Memory::DumpMainMemory( String^ fileName )
{
	if( MainMemory == NULL )
		return;

	FileStream^ stream = File::OpenWrite( fileName );
	BinaryWriter^ writer = gcnew BinaryWriter( stream );
	array<byte>^ bytes = gcnew array<byte>( MainMemorySize );
	pin_ptr<byte> ptr = &bytes[ 0 ];
	memcpy( ptr, MainMemory, MainMemorySize );
	writer->Write( bytes );
	writer->Close();
}
