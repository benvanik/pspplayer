// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "StdAfx.h"
#include "R4000Statistics.h"

using namespace System::Diagnostics;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Cpu;
using namespace Noxa::Emulation::Psp::Debugging::Statistics;

#ifdef STATISTICS

uint _instructionsExecuted;
uint _executionLoops;
uint _codeBlocksExecuted;
uint _codeBlocksGenerated;

uint _codeCacheHits;
uint _codeCacheMisses;

uint _jumpBlockInlineCount;
uint _jumpBlockThunkCount;
uint _jumpBlockLookupCount;
uint _codeBlockRetCount;

uint _jumpBlockThunkCalls;
uint _jumpBlockThunkBuilds;
uint _jumpBlockThunkHits;
uint _jumpBlockInlineHits;
uint _jumpBlockInlineMisses;

uint _managedMemoryReadCount;
uint _managedMemoryWriteCount;

uint _managedSyscallCount;
uint _nativeSyscallCount;
uint _cpuSyscallCount;
uint _unimplementedSyscallCount;

#endif

R4000Statistics::R4000Statistics() : CounterSource( "CPU" )
{
	InstructionsExecuted = gcnew Counter( "Instructions Executed", "The number of instructions executed." );
	ExecutionLoops = gcnew Counter( "Execution Loops", "The number of master execution loops performed." );
	CodeBlocksExecuted = gcnew Counter( "Blocks Executed", "The number of code blocks executed." );
	CodeBlocksGenerated = gcnew Counter( "Blocks Generated", "The number of code blocks generated." );

	CodeCacheHits = gcnew Counter( "Code Cache Hits", "Number of lookups that resulted in a hit." );
	CodeCacheMisses = gcnew Counter( "Code Cache Misses", "Number of lookups that resulted in a miss." );
	CodeCacheBlockCount = gcnew Counter( "Code Cache Count", "The number of code blocks contained within the cache." );
	
	CodeBlockLength = gcnew Counter( "Block Length", "The number of instructions per code block." );

	JumpBlockInlineCount = gcnew Counter( "Jump Block Inline Count", "Number of inline jumps." );
	JumpBlockThunkCount = gcnew Counter( "Jump Block Thunk Count", "Number of jump block thunks." );
	JumpBlockLookupCount = gcnew Counter( "Jump Block Lookup Count", "Number of jump lookup bounces." );
	CodeBlockRetCount = gcnew Counter( "Code Block Ret Count", "Number of rets." );

	JumpBlockThunkCalls = gcnew Counter( "Jump Block Thunk Calls", "__missingBlockThunkM calls." );
	JumpBlockThunkBuilds = gcnew Counter( "Jump Block Thunk Builds", "Number of builds from __missingBlockThunkM." );
	JumpBlockThunkHits = gcnew Counter( "Jump Block Thunk Hits", "Number of hits in __missingBlockThunkM (no build needed)." );
	JumpBlockInlineHits = gcnew Counter( "Jump Block Inline Hits", "" );
	JumpBlockInlineMisses = gcnew Counter( "Jump Block Inline Misses", "" );

	ManagedMemoryReadCount = gcnew Counter( "Managed Memory Reads", "The number of times the managed memory system handled a read." );
	ManagedMemoryWriteCount = gcnew Counter( "Managed Memory Writes", "The number of times the managed memory system handled a write." );

	ManagedSyscallCount = gcnew Counter( "Managed Syscalls", "The number of managed syscalls executed." );
	NativeSyscallCount = gcnew Counter( "Native Syscalls", "The number of native syscalls executed." );
	CpuSyscallCount = gcnew Counter( "Inline Syscalls", "The number of CPU inlined syscalls executed." );
	UnimplementedSyscallCount = gcnew Counter( "Unimplemented Syscalls", "The number of times an unimplemented syscall was called." );

	CodeSizeRatio = gcnew Counter( "Code Size Ratio", "The ratio of MIPS code size to x86 code size.", CounterMode::ArithmeticMean );

	GenerationTime = gcnew Counter( "Generation Time", "The time to generate blocks, in seconds.", CounterMode::ArithmeticMean );
	
	this->RegisterCounter( this->InstructionsExecuted );
	this->RegisterCounter( this->ExecutionLoops );
	this->RegisterCounter( this->CodeBlocksExecuted );
	this->RegisterCounter( this->CodeBlocksGenerated );

	this->RegisterCounter( this->CodeCacheHits );
	this->RegisterCounter( this->CodeCacheMisses );
	this->RegisterCounter( this->CodeCacheBlockCount );
	
	this->RegisterCounter( this->CodeBlockLength );
	
	this->RegisterCounter( this->JumpBlockInlineCount );
	this->RegisterCounter( this->JumpBlockThunkCount );
	this->RegisterCounter( this->JumpBlockLookupCount );
	this->RegisterCounter( this->CodeBlockRetCount );

	this->RegisterCounter( this->JumpBlockThunkCalls );
	this->RegisterCounter( this->JumpBlockThunkBuilds );
	this->RegisterCounter( this->JumpBlockThunkHits );
	this->RegisterCounter( this->JumpBlockInlineHits );
	this->RegisterCounter( this->JumpBlockInlineMisses );

	this->RegisterCounter( this->ManagedMemoryReadCount );
	this->RegisterCounter( this->ManagedMemoryWriteCount );

	this->RegisterCounter( this->ManagedSyscallCount );
	this->RegisterCounter( this->NativeSyscallCount );
	this->RegisterCounter( this->CpuSyscallCount );
	this->RegisterCounter( this->UnimplementedSyscallCount );

	this->RegisterCounter( this->CodeSizeRatio );

	this->RegisterCounter( this->GenerationTime );
}

void R4000Statistics::Sample()
{
#ifdef STATISTICS
	InstructionsExecuted->Update( _instructionsExecuted );
	ExecutionLoops->Update( _executionLoops );
	CodeBlocksExecuted->Update( _codeBlocksExecuted );
	CodeBlocksGenerated->Update( _codeBlocksGenerated );

	CodeCacheHits->Update( _codeCacheHits );
	CodeCacheMisses->Update( _codeCacheMisses );
	//CodeCacheBlockCount->Update( _

	JumpBlockInlineCount->Update( _jumpBlockInlineCount );
	JumpBlockThunkCount->Update( _jumpBlockThunkCount );
	JumpBlockLookupCount->Update( _jumpBlockLookupCount );
	CodeBlockRetCount->Update( _codeBlockRetCount );

	JumpBlockThunkCalls->Update( _jumpBlockThunkCalls );
	JumpBlockThunkBuilds->Update( _jumpBlockThunkBuilds );
	JumpBlockThunkHits->Update( _jumpBlockThunkHits );
	JumpBlockInlineHits->Update( _jumpBlockInlineHits );
	JumpBlockInlineMisses->Update( _jumpBlockInlineMisses );

	ManagedMemoryReadCount->Update( _managedMemoryReadCount );
	ManagedMemoryWriteCount->Update( _managedMemoryWriteCount );

	ManagedSyscallCount->Update( _managedSyscallCount );
	NativeSyscallCount->Update( _nativeSyscallCount );
	CpuSyscallCount->Update( _cpuSyscallCount );
	UnimplementedSyscallCount->Update( _unimplementedSyscallCount );

#endif
}
