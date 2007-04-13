// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "Stdafx.h"
#include "Loader.h"
#include "Kernel.h"
#include "KernelHelpers.h"
#include "KPartition.h"
#include "KThread.h"
#include "SDK/elftypes.h"
#include "SDK/prxtypes.h"

using namespace System::Diagnostics;

using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Bios;
using namespace Noxa::Emulation::Psp::Media;

void kmodule_thread_end( Kernel* kernel, KThread* thread, int arg );

Loader::Loader( NativeBios^ bios )
{
	Debug::Assert( bios != nullptr );
	_bios = bios;
}

Loader::~Loader()
{
}

Elf32_Shdr* FindSection( byte* buffer, String^ name )
{
	Elf32_Ehdr* header = ( Elf32_Ehdr* )buffer;

	Elf32_Shdr* strhdr = ( Elf32_Shdr* )( buffer + header->e_shoff + ( header->e_shentsize * header->e_shstrndx ) );
	byte* pstrtab = buffer + strhdr->sh_offset;

	for( int n = 0; n < header->e_shnum; n++ )
	{
		Elf32_Shdr* shdr = ( Elf32_Shdr* )( buffer + header->e_shoff + ( header->e_shentsize * n ) );
		char* pname = ( char* )( pstrtab + shdr->sh_name );
		String^ sname = gcnew String( pname );
		if( sname == name )
			return shdr;
	}

	return NULL;
}

byte* GetStrTab( byte* buffer )
{
	Elf32_Shdr* strtab = FindSection( buffer, ".strtab" );
	Debug::Assert( strtab != NULL );
	if( strtab == NULL )
		return NULL;

	byte* pstrtab = buffer + strtab->sh_offset;

	return pstrtab;
}

String^ GetName( byte* pstrtab, int index )
{
	char* pname = ( char* )( pstrtab + index );
	return gcnew String( pname );
}

ref struct HiReloc
{
	uint	Value;
	uint*	CodePointer;
};

LoadResults^ Loader::LoadModule( ModuleType type, Stream^ moduleStream, LoadParameters^ parameters )
{
	Vector<Elf32_Shdr*> allocSections( 128 );
	Vector<Elf32_Shdr*> relocSections( 128 );

	Kernel* kernel = _bios->_kernel;
	Debug::Assert( kernel != NULL );
	MemorySystem* memory = kernel->Memory;
	Debug::Assert( memory != NULL );
	ICpu^ cpu = _bios->Emulator->Cpu;

	LoadResults^ results = gcnew LoadResults();
	results->Successful = false;
	results->Imports = gcnew List<StubImport^>();
	results->Exports = gcnew List<StubExport^>();

	Debug::Assert( moduleStream != nullptr );
	Debug::Assert( moduleStream->CanRead == true );
	if( ( moduleStream == nullptr ) ||
		( moduleStream->CanRead == false ) )
		return results;

	// Load the entire module in to memory - nasty, but it works
	int length = ( int )moduleStream->Length;
	array<byte>^ mbuffer = gcnew array<byte>( length );
	moduleStream->Read( mbuffer, 0, length );
	byte* buffer = ( byte* )malloc( length );
	Debug::Assert( buffer != NULL );
	pin_ptr<byte> bp = &mbuffer[ 0 ];
	memcpy( buffer, bp, length );
	mbuffer = nullptr;

	// Get header
	Elf32_Ehdr* header = ( Elf32_Ehdr* )buffer;
	Debug::Assert( header->e_magic == 0x464C457F );
	if( header->e_magic != 0x464C457F )
		goto err;
	Debug::Assert( header->e_machine == ELF_MACHINE_MIPS );
	if( header->e_machine != ELF_MACHINE_MIPS )
		goto err;

	switch( type )
	{
	case ModuleType::Boot:
		/*Debug::Assert( header->e_type == ELF_EXEC_TYPE );
		if( header->e_type != ELF_EXEC_TYPE )
			goto err;*/
		break;
	case ModuleType::Prx:
		/*Debug::Assert( header->e_type == ELF_PRX_TYPE );
		if( header->e_type != ELF_PRX_TYPE )
			goto err;*/
		break;
	}

	results->EntryAddress = header->e_entry;
	bool needsRelocation = (
		( results->EntryAddress < 0x08000000 ) ||
		( header->e_type == ELF_PRX_TYPE ) );

	// p-hdrs
	for( int n = 0; n < header->e_phnum; n++ )
	{
		Elf32_Phdr* phdr = ( Elf32_Phdr* )( buffer + header->e_phoff + ( header->e_phentsize * n ) );
	}

	// s-hdrs
	uint lowAddress = 0;
	uint extents = 0;
	for( int n = 0; n < header->e_shnum; n++ )
	{
		Elf32_Shdr* shdr = ( Elf32_Shdr* )( buffer + header->e_shoff + ( header->e_shentsize * n ) );

		if( ( shdr->sh_flags & SHF_ALLOC ) == SHF_ALLOC )
		{
			allocSections.Add( shdr );
			uint upperBound = shdr->sh_addr + shdr->sh_size;
			if( upperBound > extents )
				extents = upperBound;
		}
		if( ( shdr->sh_type == SHT_REL ) ||
			( shdr->sh_type == SHT_PRXRELOC ) )
			relocSections.Add( shdr );
	}

	// Module info
	Elf32_Shdr* moduleInfoShdr = FindSection( buffer, PSP_MODULE_INFO_NAME );
	Debug::Assert( moduleInfoShdr != NULL );
	PspModuleInfo* moduleInfo = ( PspModuleInfo* )( buffer + moduleInfoShdr->sh_offset );
	results->GlobalPointer = moduleInfo->gp;
	results->Name = gcnew String( moduleInfo->name );

	uint baseAddress = 0;
	KMemoryBlock* moduleBlock = NULL;
	if( needsRelocation == true )
	{
		if( type == ModuleType::Boot )
			baseAddress = 0x08900000;
		else
		{
			// Find the next block in RAM
			moduleBlock = kernel->Partitions[ 2 ]->Allocate( KAllocLow, 0, extents );
			baseAddress = moduleBlock->Address;
		}

		results->EntryAddress += baseAddress;
		results->LowerBounds = baseAddress;
		results->UpperBounds = baseAddress + extents;
	}
	else
	{
		Debug::Assert( type == ModuleType::Boot );
		results->LowerBounds = 0x08900000;
		results->UpperBounds = extents;
	}

	// Allocate space taken by module
	if( type == ModuleType::Boot )
		moduleBlock = kernel->Partitions[ 2 ]->Allocate( KAllocSpecific, results->LowerBounds, results->UpperBounds - results->LowerBounds );
	else
	{
		// Should be done above
		Debug::Assert( moduleBlock != NULL );
	}
	Debug::Assert( moduleBlock != NULL );

	// Allocate sections in memory
	void* state = NULL;
	Elf32_Shdr** pshdr;
	while( ( pshdr = allocSections.Enumerate( &state ) ) != NULL )
	{
		Elf32_Shdr* shdr = *pshdr;
		uint address = baseAddress + shdr->sh_addr;
		byte* pdest = memory->Translate( address );
		
		switch( shdr->sh_type )
		{
		case SHT_NOBITS:
			// Write zeros?
			memset( pdest, 0, shdr->sh_size );
			break;
		default:
		case SHT_PROGBITS:
			{
				byte* psrc = buffer + shdr->sh_offset;
				memcpy( pdest, psrc, shdr->sh_size );
			}
			break;
		}
	}

	// Perform relocations
	if( needsRelocation == true )
	{
		List<HiReloc^>^ hiRelocs = gcnew List<HiReloc^>();

		// Find symbol table
		Elf32_Shdr* symtabShdr = FindSection( buffer, ".symtab" );
		byte* symtab = NULL;
		if( symtabShdr != NULL )
			symtab = buffer + symtabShdr->sh_offset;

		// Gather relocations
		for( int n = 0; n < header->e_shnum; n++ )
		{
			Elf32_Shdr* shdr = ( Elf32_Shdr* )( buffer + header->e_shoff + ( header->e_shentsize * n ) );
			if( ( shdr->sh_type != SHT_REL ) &&
				( shdr->sh_type != SHT_PRXRELOC ) )
				continue;

			hiRelocs->Clear();

			Elf32_Shdr* targetHdr = ( Elf32_Shdr* )( buffer + header->e_shoff + ( header->e_shentsize * shdr->sh_info ) );

			// If the target is not allocated, do not perform the relocation
			if( ( targetHdr->sh_flags & SHF_ALLOC ) == 0 )
				continue;

			int count = shdr->sh_size / sizeof( Elf32_Rel );
			for( int m = 0; m < count; m++ )
			{
				Elf32_Rel* reloc = ( Elf32_Rel* )( buffer + shdr->sh_offset + ( sizeof( Elf32_Rel ) * m ) );
				uint rtype = ELF32_R_TYPE( reloc->r_info );
				uint symbolIndex = ELF32_R_SYM( reloc->r_info );
				uint offset = reloc->r_offset;

				if( rtype == R_MIPS_NONE )
					continue;

				uint base = baseAddress;
				offset += baseAddress;

				if( shdr->sh_type == SHT_REL )
				{
					// Elf style - use symbol table
					Elf32_Sym* sym = NULL;
					if( symbolIndex != 0 )
					{
						assert( symtab != NULL );
						sym = ( Elf32_Sym* )( symtab + ( sizeof( Elf32_Sym ) * symbolIndex ) );
						base += sym->st_value;
					}
				}
				else if( shdr->sh_type == SHT_PRXRELOC )
				{
					// PRX style - crazy!
					int offsetHeaderN = symbolIndex & 0xFF;
					int valueHeaderN = ( symbolIndex >> 8 ) & 0xFF;
					assert( offsetHeaderN < header->e_phnum );
					assert( valueHeaderN < header->e_phnum );
					Elf32_Phdr* offsetHeader = ( Elf32_Phdr* )( buffer + header->e_phoff + ( header->e_phentsize * offsetHeaderN ) );
					Elf32_Phdr* valueHeader = ( Elf32_Phdr* )( buffer + header->e_phoff + ( header->e_phentsize * valueHeaderN ) );
					
					base += valueHeader->p_vaddr;
					offset += offsetHeader->p_vaddr;
				}

				uint* pcode = ( uint* )memory->Translate( offset );
				assert( pcode != NULL );

				switch( rtype )
				{
				case R_MIPS_HI16:
					{
						HiReloc^ hiReloc = gcnew HiReloc();
						hiReloc->Value = base;
						hiReloc->CodePointer = ( uint* )pcode;
						hiRelocs->Add( hiReloc );
					}
					break;
				case R_MIPS_LO16:
					{
						uint code = *pcode;
						uint vallo = ( ( code & 0x0000FFFF ) ^ 0x00008000 ) - 0x00008000;
						while( hiRelocs->Count > 0 )
						{
							HiReloc^ hiReloc = hiRelocs[ hiRelocs->Count - 1 ];
							hiRelocs->RemoveAt( hiRelocs->Count - 1 );

							assert( hiReloc->Value == base );

							uint value2 = *hiReloc->CodePointer;
							uint temp = ( ( value2 & 0x0000FFFF ) << 16 ) + vallo;
							temp += base;
							temp = ( ( temp >> 16 ) + ( ( ( temp & 0x00008000 ) != 0 ) ? ( uint )1 : ( uint )0 ) ) & 0x0000FFFF;
							value2 = ( uint )( ( value2 & ~0x0000FFFF ) | temp );

							//Debug.WriteLine( string.Format( "   Updating memory at 0x{0:X8} to {1:X8} (from previous HI16)", hiReloc.Address, value2 ) );
							*hiReloc->CodePointer = value2;
						}
						*pcode = ( uint )( ( code & ~0x0000FFFF ) | ( ( base + vallo ) & 0x0000FFFF ) );
					}
					break;
				case R_MIPS_26:
					{
						uint code = *pcode;
						uint addr = ( code & 0x03FFFFFF ) << 2;
						addr += base;
						*pcode = ( code & ~0x03FFFFFF ) | ( addr >> 2 );
					}
					break;
				case R_MIPS_32:
					*pcode += base;
					break;
				}
			}

			assert( hiRelocs->Count == 0 );
			// Finish off hi relocs?
		}
	}

	// Get exports
	uint pexports = moduleInfo->exports + ( ( needsRelocation == true ) ? baseAddress : 0 );
	int exportCount = ( moduleInfo->exp_end - moduleInfo->exports ) / sizeof( PspModuleExport );
	for( int n = 0; n < exportCount; n++ )
	{
		PspModuleExport* ex = ( PspModuleExport* )memory->Translate( pexports + ( sizeof( PspModuleExport ) * n ) );
		String^ name = nullptr;
		if( ex->name != 0x0 )
			name = KernelHelpers::ReadString( memory, ( const int )ex->name );
		
		uint* pnid = ( uint* )memory->Translate( ex->exports );
		uint* pvalue = ( uint* )memory->Translate( ex->exports + ( ( ex->func_count + ex->var_count ) * 4 ) );

		for( int m = 0; m < ( ex->func_count + ex->var_count ); m++ )
		{
			uint nid = *( pnid + m );
			uint value = *( pvalue + m );

			StubExport^ stubExport = gcnew StubExport();
			if( m < ex->func_count )
				stubExport->Type = StubType::Function;
			else
				stubExport->Type = StubType::Variable;
			stubExport->ModuleName = name;
			stubExport->NID = nid;
			stubExport->Address = value;
			results->Exports->Add( stubExport );
		}
	}

	int nidGoodCount = 0;
	int nidNotFoundCount = 0;
	int nidNotImplementedCount = 0;
	int moduleNotFoundCount = 0;
	List<String^>^ missingModules = gcnew List<String^>();
	
	// Get imports
	uint pimports = moduleInfo->imports + ( ( needsRelocation == true ) ? baseAddress : 0 );
	int importCount = ( moduleInfo->imp_end - moduleInfo->imports ) / sizeof( PspModuleImport );
	for( int n = 0; n < importCount; n++ )
	{
		PspModuleImport* im = ( PspModuleImport* )memory->Translate( pimports + ( sizeof( PspModuleImport ) * n ) );
		String^ name = nullptr;
		if( im->name != 0x0 )
			name = KernelHelpers::ReadString( memory, ( const int )im->name );
		Debug::Assert( name != nullptr );

		BiosModule^ module = _bios->FindModule( name );
		if( module == nullptr )
			missingModules->Add( name );
		
		uint* pnid = ( uint* )memory->Translate( im->nids );
		uint* pfuncs = ( uint* )memory->Translate( im->funcs );
		
		// Functions
		for( int m = 0; m < ( im->func_count + im->var_count ); m++ )
		{
			uint nid = *( pnid + m );
			uint* pcode = ( pfuncs + ( m * 2 ) );

			BiosFunction^ function = _bios->FindFunction( nid );

			StubImport^ stubImport = gcnew StubImport();
			if( module == nullptr )
			{
				stubImport->Result = StubReferenceResult::ModuleNotFound;
				moduleNotFoundCount++;
			}
			else if( function == nullptr )
			{
				Debug::WriteLine( String::Format( "LoadModule: {0} 0x{1:X8} not found (NID not present)", name, nid ) );
				stubImport->Result = StubReferenceResult::NidNotFound;
				nidNotFoundCount++;
			}
			else if( function->IsImplemented == false )
			{
				Debug::WriteLine( String::Format( "LoadModule: 0x{1:X8} found and patched at 0x{2:X8} -> {0}::{3} (NI)", name, nid, ( uint )pcode, function->Name ) );
				stubImport->Result = StubReferenceResult::NidNotImplemented;
				nidNotImplementedCount++;
			}
			else
			{
				//Debug::WriteLine( String::Format( "LoadModule: 0x{1:X8} found and patched at 0x{2:X8} -> {0}::{3}", name, nid, ( uint )pcode, function->Name ) );
				stubImport->Result = StubReferenceResult::Success;
				nidGoodCount++;
			}
			if( m < im->func_count )
				stubImport->Type = StubType::Function;
			else
				stubImport->Type = StubType::Variable;
			stubImport->ModuleName = name;
			stubImport->Function = function;
			stubImport->NID = nid;
			stubImport->Address = ( uint )pcode;
			results->Imports->Add( stubImport );

			// Perform fixup, if possible
			if( function != nullptr )
			{
				uint syscall = cpu->RegisterSyscall( nid );
				*( pcode + 1 ) = ( uint )( ( syscall << 6 ) | 0xC );
			}
		}
	}

	// Print stats
#ifdef _DEBUG
	Debug::WriteLine( String::Format( "LoadModule: {0}/{1} ({2}%) NIDs good; {3} not implemented, {4} not found, {5} in missing modules = {6} total", nidGoodCount, results->Imports->Count - moduleNotFoundCount, ( nidGoodCount / ( float )( results->Imports->Count - moduleNotFoundCount ) ) * 100.0f, nidNotImplementedCount, nidNotFoundCount, moduleNotFoundCount, results->Imports->Count ) );
	if( missingModules->Count > 0 )
	{
		Debug::WriteLine( String::Format( "LoadModule: {0} missing modules (contain {1} NIDs ({2}% of total)):", missingModules->Count, moduleNotFoundCount, ( moduleNotFoundCount / ( float )results->Imports->Count ) * 100.0f ) );
		for each( String^ moduleName in missingModules )
			Debug::WriteLine( String::Format( "\t\t{0}", moduleName ) );
	}
#endif

	results->Successful = true;

	if( results->Successful == true )
	{
		// Allocate room for args
		KMemoryBlock* argsBlock = kernel->Partitions[ 2 ]->Allocate( KAllocHigh, 0, 0xFF ); // 256b of args - enough?

		// Set arguments - we put these right below user space, and right above the stack
		uint args = 0;
		uint argp = argsBlock->Address;
		String^ arg0 = parameters->Path->AbsolutePath->Replace( '\\', '/' ) + "/";
		args += arg0->Length + 1;
		KernelHelpers::WriteString( memory, ( const int )argp, arg0 );
		uint envp = argp;// + args;
		// write envp??

		// What we do here simulates what the modulemgr does - it creates a user mode thread that
		// runs module_start (_start, etc) and then exits when complete.
		// NOTE: If we are a PRX, the entry address will correspond to module_start, so we don't need to do anything!

		// Create a thread
		KThread* thread = new KThread( kernel, kernel->Partitions[ 2 ], "kmodule_thread", results->EntryAddress, 1, KThreadUser, 0x4000 );
		thread->GlobalPointer = results->GlobalPointer;
		
		int specialId = thread->AddSpecialHandler( &kmodule_thread_end, 0 );
		thread->SetSpecialHandler( specialId );

		thread->Start( args, argp );

		// Schedule so that our thread runs
		kernel->Schedule();
	}

err:
	SAFEFREE( buffer );

	return results;
}

void kmodule_thread_end( Kernel* kernel, KThread* thread, int arg )
{
	// Delete thread and reschedule
	thread->Exit( 0 );
	kernel->Schedule();
}
