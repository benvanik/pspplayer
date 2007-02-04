// SoftwireTest.cpp : main project file.

#include "stdafx.h"
#include "CodeGenerator.hpp"

using namespace System;

typedef unsigned char byte;

int printf( const char *str )
{
	Console::WriteLine( gcnew String( str ) );
	return 0;
}

void fixup( void* method, int length, int pattern, int newValue )
{
	byte* ptr = ( byte* )method;
	for( int n = 0; n < length; n++ )
	{
		if( *ptr == 0x68 )
		{
			ptr++;
			int* iptr = ( int* )ptr;
			if( *iptr == pattern )
			{
				*iptr = newValue;
			}
			ptr += 4;
		}
		else
			ptr++;
	}
}

int main(array<System::String ^> ^args)
{
	Console::WriteLine(L"Testing run-time intrinsics.");
	Console::WriteLine(L"Press any key to start assembling.");
	Console::ReadKey();

	SoftWire::CodeGenerator x86( false );
	x86.setEchoFile( "test.txt" );

	static char string[] = "All working!";
	static char string2[] = "All working2!";

	x86.annotate( "random leading junk" );
	x86.mov(x86.eax, 0);
	x86.add(x86.eax, 1);
	x86.jmp(x86.eax);
	
	x86.annotate( "call to printf" );
	x86.push((unsigned int)string);
	x86.call((int)printf);
	x86.add(x86.esp, (byte)4);

	x86.annotate( "random tailing junk" );
	x86.mov(x86.eax, 0);
	x86.add(x86.eax, 2);

	x86.annotate( "ret" );
	x86.ret();

	void (*emulator)() = (void(*)())x86.callable();
	x86.acquire();

	const char *listing = x86.getListing();
	Console::WriteLine( "orig code:\n" + gcnew String( listing ) );

	fixup( emulator, 64, (int)string, (int)string2 );

	Console::WriteLine(L"output: ");
	emulator();
	Console::WriteLine();

	return 0;
}
