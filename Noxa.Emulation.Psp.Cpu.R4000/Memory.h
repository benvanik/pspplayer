#pragma once

using namespace System;
using namespace System::Collections::Generic;
using namespace System::IO;
using namespace Noxa::Emulation::Psp;

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace Cpu {

				ref class MemorySegment;
				
				ref class Memory : IMemory
				{
				protected:
					List<IMemorySegment^>^	_segs;

				protected:
					!Memory(void);
					void Clear();

				public:
					Memory(void);
					~Memory(void);

					virtual IMemorySegment^ DefineSegment( MemoryType type, String^ name, int baseAddress, int length );
					virtual void RegisterSegment( IMemorySegment^ segment );
					virtual IMemorySegment^ FindSegment( String^ name );
					virtual IMemorySegment^ FindSegment( int baseAddress );

					virtual int ReadWord( int address );
					virtual array<unsigned char>^ ReadBytes( int address, int count );
					virtual int ReadStream( int address, Stream^ destination, int count );
					virtual void WriteWord( int address, int width, int value );
					virtual void WriteBytes( int address, array<unsigned char>^ bytes );
					virtual void WriteBytes( int address, array<unsigned char>^ bytes, int index, int count );
					virtual void WriteStream( int address, Stream^ source, int count );

					virtual void Load( Stream^ stream );
					virtual void Load( String^ fileName );
					virtual void Save( Stream^ stream );
					virtual void Save( String^ fileName );

					void DumpMainMemory( String^ fileName );
				};
			}
		}
	}
}
