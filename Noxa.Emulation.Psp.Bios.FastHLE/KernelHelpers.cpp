// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "Stdafx.h"
#include "KernelHelpers.h"
#include <memory>

using namespace System;
using namespace System::Collections::Generic;
using namespace System::Diagnostics;
using namespace System::Text;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Bios;
using namespace Noxa::Emulation::Psp::Cpu;

String^ KernelHelpers::ReadString( IMemory^ memory, int address )
{
	return ReadString( ( byte* )memory->MainMemoryPointer, address );
}

int KernelHelpers::WriteString( IMemory^ memory, const int address, String^ value )
{
	return WriteString( ( byte* )memory->MainMemoryPointer, address, value );
}

String^ KernelHelpers::ReadString( byte* memory, const int address )
{
	byte* ptr = memory + ( address - MainMemoryBase );
	return gcnew String( ( char* )ptr );
}

int KernelHelpers::WriteString( byte* memory, const int address, String^ value )
{
	array<byte>^ bytes = Encoding::ASCII->GetBytes( value );
	pin_ptr<byte> ptr = &bytes[ 0 ];
	int length = bytes->Length;
	byte* target = memory + ( address - MainMemoryBase );
	memcpy( target, ptr, length );
	*( target + length ) = 0;
	return length + 1;
}

//unsigned short	year 
//unsigned short	month 
//unsigned short	day 
//unsigned short	hour 
//unsigned short	minute 
//unsigned short	second 
//unsigned int		microsecond

DateTime KernelHelpers::ReadTime( IMemory^ memory, const int address )
{
	return ReadTime( ( byte* )memory->MainMemoryPointer, address );
}

int KernelHelpers::WriteTime( IMemory^ memory, const int address, DateTime time )
{
	return WriteTime( ( byte* )memory->MainMemoryPointer, address, time );
}

DateTime KernelHelpers::ReadTime( byte* memory, const int address )
{
	ushort* ptr = ( ushort* )( memory + ( address - MainMemoryBase ) );
	ushort year = *ptr;
	ushort month = *( ptr + 1 );
	ushort day = *( ptr + 2 );
	ushort hour = *( ptr + 3 );
	ushort minute = *( ptr + 4 );
	ushort second = *( ptr + 5 );
	uint microsecond = *( ( uint *)( ptr + 6 ) );

	// 1000 microseconds per millisecond?
	return DateTime( year, month, day, hour, minute, second, ( int )( microsecond / 1000 ) );
}

int KernelHelpers::WriteTime( byte* memory, const int address, DateTime time )
{
	ushort* ptr = ( ushort* )( memory + ( address - MainMemoryBase ) );
	*ptr = ( ushort )time.Year;
	*( ptr + 1 ) = ( ushort )time.Month;
	*( ptr + 2 ) = ( ushort )time.Day;
	*( ptr + 3 ) = ( ushort )time.Hour;
	*( ptr + 4 ) = ( ushort )time.Minute;
	*( ptr + 5 ) = ( ushort )time.Second;
	*( ( uint* )( ptr + 6 ) ) = ( ushort )time.Millisecond * 1000;

	return 16;
}
