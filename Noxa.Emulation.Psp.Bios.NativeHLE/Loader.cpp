// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "Stdafx.h"
#include "Loader.h"

using namespace System::Diagnostics;

using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Bios;
using namespace Noxa::Emulation::Psp::Media;

Loader::Loader( NativeBios^ bios )
{
	Debug::Assert( bios != nullptr );
	_bios = bios;
}

Loader::~Loader()
{
}

LoadResults^ Loader::LoadModule( ModuleType type, Stream^ moduleStream, LoadParameters^ parameters )
{
	return nullptr;
}

/*
		public bool LoadBoot( GameInformation game, IEmulationInstance instance, out uint lowerBounds, out uint upperBounds, out uint entryAddress )
		{
			lowerBounds = 0;
			upperBounds = 0;
			entryAddress = 0;

			if( bootStream == null )
			{
				// Whoa!
				Debug.WriteLine( "LoadBoot: game had no boot.bin/eboot.bin, aborting" );
				return false;
			}

			ElfFile elf;
			// TODO: enable exception handling to gracefully handle bad elfs
			//try
			//{
				elf = new ElfFile( bootStream );
			//}
			//catch
			//{
			//	Debug.WriteLine( "LoadBoot: elf load failed, possibly encrypted, aborting" );
			//	return false;
			//}

			uint baseAddress = 0x08900000;

			ElfLoadResult result = elf.Load( bootStream, instance, baseAddress );
			if( result.Stubs.Count > 0 )
			{
		 * // report
			}

			// Setup the CPU
			//instance.Bios.Game = game;
			instance.Bios.BootStream = bootStream;
			instance.Cpu.SetupGame( game, bootStream );
			if( instance.Host.Debugger != null )
				instance.Host.Debugger.SetupGame( game, bootStream );

			// Set arguments - we put these right below user space, and right above the stack
			int args = 0;
			int argp = 0x087FFFFF;
			string arg0 = game.Folder.AbsolutePath.Replace( '\\', '/' ) + '/';
			args += arg0.Length + 1;
			argp -= args;
			instance.Cpu.Memory.WriteBytes( argp, Encoding.ASCII.GetBytes( arg0 ) );
			int envp = argp;
			// write envp??

			// dword align the stack (just truncate, we don't care if we waste a byte or two)
			int sp = envp & unchecked( ( int )0xFFFFFFF0 );

			// TODO: Move this elsewhere?
			instance.Cpu[ 0 ].ProgramCounter = ( int )elf.EntryAddress;
			instance.Cpu[ 0 ].SetGeneralRegister( 4, args );
			instance.Cpu[ 0 ].SetGeneralRegister( 5, argp );
			instance.Cpu[ 0 ].SetGeneralRegister( 6, envp );
			instance.Cpu[ 0 ].SetGeneralRegister( 26, 0x09FBFF00 ); //0x08380000;
			instance.Cpu[ 0 ].SetGeneralRegister( 28, ( int )elf.GlobalPointer );
			instance.Cpu[ 0 ].SetGeneralRegister( 29, sp ); // start below argv & envp data - not 0x087FFFFF
			//instance.Cpu[ 0 ].SetGeneralRegister( 31, ( int )elf.EntryAddress );

			entryAddress = elf.EntryAddress;
			lowerBounds = baseAddress;
			upperBounds = elf.UpperAddress;

			return true;
		}
		*/