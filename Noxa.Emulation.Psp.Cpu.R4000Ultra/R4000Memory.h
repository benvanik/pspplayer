// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

using namespace System;
using namespace System::Collections::Generic;
using namespace System::IO;
using namespace Noxa::Emulation::Psp;

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace Cpu {

				ref class R4000MemorySegment;
				
				ref class R4000Memory : IMemory
				{
				public:
					byte*			ScratchPad;
					byte*			MainMemory;
					byte*			VideoMemory;

					MemorySystem*	SystemInstance;

				protected:
					IMemorySegment^	_frameBuffer;

				protected:
					!R4000Memory();

				internal:
					void Clear();

				public:
					R4000Memory();
					~R4000Memory();

					virtual IMemorySegment^ DefineSegment( MemoryType type, String^ name, int baseAddress, int length );
					virtual void RegisterSegment( IMemorySegment^ segment );
					virtual IMemorySegment^ FindSegment( String^ name );
					virtual IMemorySegment^ FindSegment( int baseAddress );

					property void* MainMemoryPointer
					{
						virtual void* get()
						{
							return MainMemory;
						}
					}

					property void* VideoMemoryPointer
					{
						virtual void* get()
						{
							return VideoMemory;
						}
					}

					property void* MemorySystemInstance
					{
						virtual void* get()
						{
							return SystemInstance;
						}
					}

					virtual int ReadWord( int address );
					virtual int64 ReadDoubleWord( int address );
					virtual array<unsigned char>^ ReadBytes( int address, int count );
					virtual int ReadStream( int address, Stream^ destination, int count );
					virtual void WriteWord( int address, int width, int value );
					virtual void WriteDoubleWord( int address, int64 value );
					virtual void WriteBytes( int address, array<unsigned char>^ bytes );
					virtual void WriteBytes( int address, array<unsigned char>^ bytes, int index, int count );
					virtual void WriteStream( int address, Stream^ source, int count );

					virtual unsigned int GetMemoryHash( int address, int count, unsigned int prime );

					void DumpMainMemory( String^ fileName );
				};
			}
		}
	}
}
