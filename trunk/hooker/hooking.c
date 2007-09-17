// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2007 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include <pspkernel.h>
#include <pspdebug.h>
#include <pspsysmem_kernel.h>
#include <psputilsforkernel.h>
#include <pspmoduleexport.h>
#include <psploadcore.h>
#include <pspthreadman_kernel.h>
#include <pspsdk.h>
#include <stdio.h>
#include <string.h>
#include <stdlib.h>

typedef unsigned int uint;

#define MINIMUMENTRYCAPACITY	1000
#define MAXPARAMETERCOUNT		6

#define MAKEKERNEL( p ) ( 0x80000000 | ( uint )p )
#define DELIMITER ";"

#define TYPE_INT16	'h'
#define TYPE_INT32	'i'
#define TYPE_INT64	'l'
#define TYPE_HEX32	'x'
#define TYPE_HEX64	'X'
#define TYPE_OCT32	'o'
#define TYPE_SINGLE	'f'
#define TYPE_STRING	's'
#define TYPE_VOID	'v'

// hookasm.s
extern int SetK1( int v );
extern void _HookEntry();

typedef struct HookEntry_t
{
	int				Enabled;
	char			LibraryName[ 32 ];
	uint			NID;
	char			FunctionName[ 32 ];
	char			ParameterFormat[ MAXPARAMETERCOUNT ];
	char			ReturnFormat;
	
	uint*			SyscallAddress;
	void*			RealDelegate;
	void*			HookDelegate;

	uint			Thunk[ 2 ];
} HookEntry;

HookEntry*	_entries;
int			_entryCount;
int			_entryCapacity;

uint FindNID( char* moduleName, uint nid )
{
	SceUID ids[ 150 ];
	int count;
	memset( ids, 0, 150 * sizeof( SceUID ) );
	sceKernelGetModuleIdList( ids, 150 * sizeof( SceUID ), &count );
	
	SceModule *module = NULL;
	int n;
	for( n = 0; n < count; n++ )
	{
		SceModule* myModule = sceKernelFindModuleByUID( ids[ n ] );
		if( strcmp( myModule->modname, moduleName ) == 0 )
		{
			module = myModule;
			break;
		}
	}

	if( module != NULL )
	{
		void* entTable = module->ent_top;
		int entLength = module->ent_size;

		n = 0;
		while( n < entLength )
		{
			struct SceLibraryEntryTable* entry = ( struct SceLibraryEntryTable* )( entTable + n );
			int total = entry->stubcount + entry->vstubcount;
			uint* vars = entry->entrytable;
			if( entry->stubcount > 0 )
			{
				int count;
				for( count = 0; count < entry->stubcount; count++ )
				{
					if( vars[ count ] == nid )
						return vars[ count + total ];
				}
			}
			n += ( entry->len * 4 );
		}
	}

	return 0;
}

typedef struct SyscallHeader_t
{
	void *unk;
	unsigned int basenum;
	unsigned int topnum;
	unsigned int size;
} SyscallHeader;

void* FindSyscallAddress( uint address )
{
	void **ptr;
	asm( "cfc0 %0, $12\n" : "=r"( ptr ) );
	if( !ptr )
	{
		printf( "FindSyscallAddress: bad ptr\n" );
		return NULL;
	}

	SyscallHeader* head = ( SyscallHeader* )*ptr;
	uint* syscalls = ( uint* )( *ptr + 0x10 );
	int size = ( head->size - 0x10 ) / sizeof( uint );

	int n;
	for( n = 0; n < size; n++ )
	{
		if( syscalls[ n ] == address )
			return &syscalls[ n ];
	}
	printf( "FindSyscallAddress: not found\n" );
	return NULL;
}

void BuildThunk( uint* thunk, void* target, int id )
{
	uint t = ( MAKEKERNEL( target ) & 0x3FFFFFFF ) >> 2;
	thunk[ 0 ] = ( 0x00000002 << 26 ) | t;
	thunk[ 1 ] = 0x34020000 | id;
}

int _fileBufferOffset = 0;
int _fileBufferCount = 0;
char _fileBuffer[ 512 ];
int ReadLine( SceUID fd, char* buffer )
{
	char* p = buffer;
	while( 1 )
	{
		if( ( _fileBufferCount <= 0 ) ||
			( _fileBufferOffset >= 512 ) )
		{
			int bytesRead = sceIoRead( fd, _fileBuffer, 512 );
			if( bytesRead <= 0 )
				return -1;
			_fileBufferOffset = 0;
			_fileBufferCount = bytesRead;
		}
		else
		{
			char next = _fileBuffer[ _fileBufferOffset++ ];
			_fileBufferCount--;
			if( next == '\n' )
			{
				*p = '\0';
				return 1;
			}
			else
			{
				*p = next;
				p++;
			}
		}
	}
}

int LoadHooks( const char* hooksFile )
{
	SceUID fplId = sceKernelCreateFpl( "hooker_entries", 4, 0, sizeof( HookEntry ) * MINIMUMENTRYCAPACITY, 1, NULL );
	void* entries = ( void* )_entries;
	if( sceKernelAllocateFpl( fplId, &entries, 0 ) < 0 )
	{
		printf( "LoadHooks: unable to allocate _entries, aborting\n" );
		return -1;
	}
	_entries = ( HookEntry* )MAKEKERNEL( entries );
	memset( _entries, 0, sizeof( HookEntry ) * MINIMUMENTRYCAPACITY );
	_entryCount = 0;
	_entryCapacity = MINIMUMENTRYCAPACITY;

	printf( "LoadHooks: loading from %s\n", hooksFile );
	SceUID hooksFd = sceIoOpen( hooksFile, PSP_O_RDONLY, 0777 );
	if( hooksFd <= 0 )
	{
		printf( "LoadHooks: unable to open %s, aborting\n", hooksFile );
		return -1;
	}

	int intc;

	int failed = 0;
	char lineBuffer[ 512 ];
	while( 1 )
	{
		if( ReadLine( hooksFd, lineBuffer ) <= 0 )
			break;

		if( _entryCount + 1 >= _entryCapacity )
		{
			printf( "LoadHooks: maximum capacity of %d reached - ignoring the rest of the hooks\n", _entryCapacity );
			break;
		}

		char* enabledString		= strtok( lineBuffer, DELIMITER );
		char* libraryName		= strtok( NULL, DELIMITER );
		char* nidString			= strtok( NULL, DELIMITER );
		char* functionName		= strtok( NULL, DELIMITER );
		char* parameterFormat	= strtok( NULL, DELIMITER );
		char* returnFormat		= strtok( NULL, DELIMITER );

		int shouldEnable = ( enabledString[ 0 ] == '1' );
		if( shouldEnable == 1 )
			printf( "LoadHooks: hooking %s::%s[%s]\n", libraryName, functionName, nidString );

		HookEntry* entry = &_entries[ _entryCount++ ];
		entry->Enabled = 0;
		strcpy( entry->LibraryName, libraryName );
		entry->NID = strtoul( nidString, NULL, 16 );
		strcpy( entry->FunctionName, functionName );
		strcpy( entry->ParameterFormat, parameterFormat );
		entry->ReturnFormat = returnFormat[ 0 ];

		if( shouldEnable == 0 )
			continue;

		void* exportAddress = ( void* )FindNID( libraryName, entry->NID );
		if( exportAddress == NULL )
		{
			failed++;
			printf( "LoadHooks: unable to find NID %s::%s[0x%08X]\n", entry->LibraryName, entry->FunctionName, entry->NID );
			continue;
		}
		else if( ( uint )exportAddress < 0x80000000 )
		{
			printf( "LoadHooks: export address 0x%08X is not kernel mode; ignoring %s::%s[0x%08X]\n", ( uint )exportAddress, entry->LibraryName, entry->FunctionName, entry->NID );
			continue;
		}
		entry->RealDelegate = exportAddress;

		uint* syscallAddress = ( uint* )FindSyscallAddress( ( uint )( uint* )exportAddress );
		if( syscallAddress == NULL )
		{
			failed++;
			printf( "LoadHooks: unable to find syscall address for %s::%s[0x%08X]\n", entry->LibraryName, entry->FunctionName, entry->NID );
			continue;
		}
		entry->SyscallAddress = syscallAddress;

		// This routine actually makes the call
		BuildThunk( entry->Thunk, ( void* )_HookEntry, _entryCount - 1 );
		intc = pspSdkDisableInterrupts();
		sceKernelDcacheWritebackInvalidateRange( ( uint* )MAKEKERNEL( entry->Thunk ), sizeof( uint ) * 2 );
		sceKernelIcacheInvalidateRange( ( uint* )MAKEKERNEL( entry->Thunk ), sizeof( uint ) * 2 );
		pspSdkEnableInterrupts( intc );

		//printf( "LoadHooks: %s::%s[0x%08X] real address=0x%08X, syscall address=0x%08X\n", entry->LibraryName, entry->FunctionName, entry->NID, exportAddress, syscallAddress );
		//printf( "LoadHooks: %s::%s[0x%08X] thunk addr=0x%08X\n", entry->LibraryName, entry->FunctionName, entry->NID, ( uint )entry->Thunk );

		entry->Enabled = shouldEnable;
	}

	sceIoClose( hooksFd );

	printf( "LoadHooks: read %d entries from %s; %d failed\n", _entryCount, hooksFile, failed );

	// HACK: never fail
	failed = 0;
	if( failed > 0 )
	{
		return -1;
	}
	else
	{
		// Perform all rewrites here, where we know we are good
		intc = pspSdkDisableInterrupts();
		int n;
		for( n = 0; n < _entryCount; n++ )
		{
			HookEntry* entry = &_entries[ n ];
			if( entry->Enabled == 1 )
			{
				*entry->SyscallAddress = MAKEKERNEL( entry->Thunk );
				sceKernelDcacheWritebackInvalidateRange( entry->SyscallAddress, sizeof( void* ) );
				sceKernelIcacheInvalidateRange( entry->SyscallAddress, sizeof( void* ) );
			}
		}
		pspSdkEnableInterrupts( intc );
		return 1;
	}
}

int Format( char type, char* buffer, uint* value )
{
	if( value == NULL )
		return sprintf( buffer, "[invalid ptr]" );
	switch( type )
	{
	case TYPE_INT16:
		return sprintf( buffer, "%d", *( short* )value );
	default:
	case TYPE_INT32:
		return sprintf( buffer, "%d", *( int* )value );
	case TYPE_INT64:
		{
			unsigned long long int64 = ( ( unsigned long long )value[ 1 ] ) << 32 | value[ 0 ];
			return sprintf( buffer, "%lld", int64 );
		}
	case TYPE_HEX32:
		return sprintf( buffer, "0x%08X", *( uint* )value );
	case TYPE_HEX64:
		return sprintf( buffer, "0x%08X%08X", *( uint* )value[ 1 ], *( uint* )value[ 0 ] );
	case TYPE_OCT32:
		return sprintf( buffer, "0%o", *( int* )value );
	case TYPE_SINGLE:
		return sprintf( buffer, "%g", *( float* )value );
	case TYPE_STRING:
		if( ( char* )value[ 0 ] == NULL )
			return sprintf( buffer, "[invalid ptr]" );
		else
			return sprintf( buffer, "%s", ( char* )value[ 0 ] );
	case TYPE_VOID:
		return sprintf( buffer, "void" );
	}
}

void* _OnHookHit( int id, uint* args )
{
	HookEntry* entry = NULL;
	int intc = pspSdkDisableInterrupts();
	if( ( id >= 0 ) && ( id < _entryCount ) )
		entry = &_entries[ id ];
	pspSdkEnableInterrupts( intc );

	int k1 = SetK1( 0 );

	if( ( entry != NULL ) &&
		( entry->Enabled == 1 ) )
	{
		char buffer[ 256 ];
		int position = sprintf( buffer, "~0x%08X -> %s[0x%08X](", sceKernelGetThreadId(), entry->FunctionName, entry->NID );
		int n;
		int m;
		for( n = 0, m = 0; n < MAXPARAMETERCOUNT; n++ )
		{
			if( ( entry->ParameterFormat[ n ] == 0 ) ||
				( entry->ParameterFormat[ n ] == 'v' ) )
				break;
			if( n != 0 )
			{
				*( buffer + position ) = ',';
				position++;
			}
			position += Format( entry->ParameterFormat[ n ], buffer + position, args + m );

			switch( entry->ParameterFormat[ n ] )
			{
			default:
				m++;
				break;
			case TYPE_INT64:
			case TYPE_HEX64:
				m += 2;
				break;
			}
		}
		position += sprintf( buffer + position, ") from 0x%08X\n", sceKernelGetSyscallRA() );
		printf( buffer );
	}

	SetK1( k1 );

	if( entry != NULL )
		return entry->RealDelegate;
	else
		return NULL;
}

void _OnHookReturn( int id, uint* returnValue )
{
	HookEntry* entry = NULL;
	int intc = pspSdkDisableInterrupts();
	if( ( id >= 0 ) && ( id < _entryCount ) )
		entry = &_entries[ id ];
	pspSdkEnableInterrupts( intc );

	int k1 = SetK1( 0 );

	if( ( entry != NULL ) &&
		( entry->Enabled == 1 ) )
	{
		char buffer[ 128 ];
		int position = sprintf( buffer, "~0x%08X <- %s[0x%08X] = ", sceKernelGetThreadId(), entry->FunctionName, entry->NID );
		position += Format( entry->ReturnFormat, buffer + position, returnValue );
		position += sprintf( buffer + position, "\n"  );
		printf( buffer );
	}

	SetK1( k1 );
}
