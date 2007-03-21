// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "stdafx.h"
#include "CodeGenerator.h"

using namespace System;
using namespace System::Diagnostics;
using namespace Noxa::Emulation::Psp::CodeGen;

void __x()
{
	Debug::WriteLine( "Hello!" );
}

#pragma unmanaged
void __y()
{
	__x();
}
void Bounce( FunctionPointer p )
{
	p();
}
#pragma managed

int main(array<System::String ^> ^args)
{
    CodeGenerator* g = new CodeGenerator( 1024 * 10, 1024 * 1024 * 4 );

	Label* l1 = g->DefineLabel();
	Label* l2 = g->DefineLabel();

	int a = 12345;

	//g->int3();
	g->push( g->eax );
	g->MarkLabel( l1 );

	g->push( g->ebx );
	g->mov( g->ebx, g->dword_ptr[ &a ] );
	g->add( g->ebx, 1 );
	g->mov( g->dword_ptr[ &a ], g->ebx );
	g->pop( g->ebx );

	//g->jmp( l2 );

	g->push( g->eax );
	//g->mov( g->eax, ( int )__x );
	g->call( ( int )__y );
	g->pop( g->eax );

	g->add( g->eax, 1 );
	g->cmp( g->eax, 5 );
	g->jne( l1 );
	
	g->MarkLabel( l2 );
	g->pop( g->eax );
	g->ret();

	FunctionPointer p = g->GenerateCode();
	g->Reset();

	Bounce( p );

	delete g;

    return 0;
}
