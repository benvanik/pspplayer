// SoftwireTest.cpp : main project file.

#include "stdafx.h"
#include "CodeGenerator.hpp"

using namespace System;

int printf( const char *str )
{
	Console::WriteLine( gcnew String( str ) );
	return 0;
}

int main(array<System::String ^> ^args)
{
	Console::WriteLine(L"Testing run-time intrinsics.");
	Console::WriteLine(L"Press any key to start assembling.");
	Console::ReadKey();

	SoftWire::Assembler x86;
	x86.setEchoFile( "c:\\test.txt" );

	static char string[] = "All working!";

	x86.push((int)string);
	x86.call((int)printf);
	x86.add(x86.esp, 4);
	x86.ret();

	void (*emulator)() = (void(*)())x86.callable();

	const char *listing = x86.getListing();
	Console::WriteLine( gcnew String( listing ) );

	Console::WriteLine(L"output: ");
	emulator();
	Console::WriteLine();
	return 0;
}
