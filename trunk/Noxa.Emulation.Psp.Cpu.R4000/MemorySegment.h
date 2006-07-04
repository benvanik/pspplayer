#pragma once

using namespace System;
using namespace System::IO;
using namespace Noxa::Emulation::Psp;

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace Cpu {
				
				ref class MemorySegment : IMemorySegment
				{
				protected:
					MemoryType				_type;
					String^					_name;
					int						_baseAddress;
					int						_length;
					unsigned char*			_memory;

				protected:
					!MemorySegment(void);
					void Clear();

					void Allocate( int size );

				public:
					MemorySegment( Cpu::MemoryType type, String^ name, int baseAddress, int length );
					~MemorySegment(void);

					property Cpu::MemoryType MemoryType
					{
						virtual Cpu::MemoryType get()
						{
							return _type;
						}
					}

					property String^ Name
					{
						virtual String^ get()
						{
							return _name;
						}
					}

					property int BaseAddress
					{
						virtual int get()
						{
							return _baseAddress;
						}
					}

					property int Length
					{
						virtual int get()
						{
							return _length;
						}
					}

					virtual event MemoryChangeDelegate^ MemoryChanged;

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
				};
			}
		}
	}
}
