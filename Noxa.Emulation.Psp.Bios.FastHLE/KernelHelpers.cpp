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
using namespace Noxa::Emulation::Psp::Cpu;

__inline static String^ ReadString( IMemory^ memory, int address )
{
	StringBuilder^ sb = gcnew StringBuilder();
	while( true )
	{
		byte c = ( byte )( memory->ReadWord( address ) & 0xFF );
		if( c == 0 )
			break;
		sb->Append( ( char )c );
		address++;
	}
	return sb->ToString();
}

__inline static int WriteString( IMemory^ memory, const int address, String^ value )
{
	array<byte>^ bytes = Encoding::ASCII->GetBytes( value );
	memory->WriteBytes( address, bytes );
	memory->WriteWord( address + bytes->Length, 1, 0 );
	return bytes->Length + 1;
}

__inline static String^ ReadString( byte* memory, const int address )
{
	byte* ptr = memory + ( address - MainMemoryBase );
	StringBuilder^ sb = gcnew StringBuilder();
	while( true )
	{
		byte c = *ptr;
		if( c == 0 )
			break;
		sb->Append( ( char )c );
		ptr++;
	}
	return sb->ToString();
}

__inline static int WriteString( byte* memory, const int address, String^ value )
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

__inline static DateTime ReadTime( IMemory^ memory, const int address )
{
	ushort year = ( ushort )( memory->ReadWord( address ) & 0xFFFF );
	ushort month = ( ushort )( memory->ReadWord( address + 2 ) & 0xFFFF );
	ushort day = ( ushort )( memory->ReadWord( address + 4 ) & 0xFFFF );
	ushort hour = ( ushort )( memory->ReadWord( address + 6 ) & 0xFFFF );
	ushort minute = ( ushort )( memory->ReadWord( address + 8 ) & 0xFFFF );
	ushort second = ( ushort )( memory->ReadWord( address + 10 ) & 0xFFFF );
	uint microsecond = ( uint )( memory->ReadWord( address + 12 ) );

	// 1000 microseconds per millisecond?
	return DateTime( year, month, day, hour, minute, second, ( int )( microsecond / 1000 ) );
}

__inline static int WriteTime( IMemory^ memory, const int address, DateTime time )
{
	memory->WriteWord( address, 2, time.Year );
	memory->WriteWord( address + 2, 2, time.Month );
	memory->WriteWord( address + 4, 2, time.Day );
	memory->WriteWord( address + 6, 2, time.Hour );
	memory->WriteWord( address + 8, 2, time.Minute );
	memory->WriteWord( address + 10, 2, time.Second );
	memory->WriteWord( address + 12, 4, time.Millisecond * 1000 );

	return 16;
}

__inline static DateTime ReadTime( byte* memory, const int address )
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

__inline static int WriteTime( byte* memory, const int address, DateTime time )
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
