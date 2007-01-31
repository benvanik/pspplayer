// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

using namespace System;
using namespace Noxa::Emulation::Psp;

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace Cpu {

				ref class R4000Cpu;
				ref class R4000Cp0;
				ref class R4000Cp1;
				ref class R4000Cp2;
				ref class R4000Coprocessor;
				ref class Memory;
				ref class CodeCache;

				[Flags]
				enum class CoreAttributes
				{
					Default = 0x000,
					HasCp2 = 0x001
				};
				
				ref class R4000Core : ICpuCore
				{
				protected:
					R4000Cpu^					_cpu;
					Memory^						_memory;
					String^						_name;
					int							_coreId;
					R4000Cp0^					_cp0;
					R4000Cp1^					_cp1;
					R4000Cp2^					_cp2;
					array<R4000Coprocessor^>^	_cops;

				protected:
					
					ref class R4000CoreContext
					{
					public:
						int ProgramCounter;
						array<int>^ GeneralRegisters;
						int Hi;
						int Lo;
						bool LL;
						bool InDelaySlot;
						int DelayPc;
						bool DelayNop;
						int InterruptState;

						Object^ Cp0;
						Object^ Cp1;
					};

				public:
					R4000Core( R4000Cpu^ cpu, String^ name, int coreId, CoreAttributes attributes );

					property R4000Cpu^ Cpu
					{
						virtual R4000Cpu^ get()
						{
							return _cpu;
						}
					}

					property int CoreID
					{
						virtual int get()
						{
							return _coreId;
						}
					}

					property R4000Cp0^ Cp0
					{
						virtual R4000Cp0^ get()
						{
							return _cp0;
						}
					}

					property R4000Cp1^ Cp1
					{
						virtual R4000Cp1^ get()
						{
							return _cp1;
						}
					}

					property R4000Cp2^ Cp2
					{
						virtual R4000Cp2^ get()
						{
							return _cp2;
						}
					}

					property String^ Name
					{
						virtual String^ get()
						{
							return _name;
						}
					}

					property int ProgramCounter
					{
						virtual int get()
						{
							return Pc;
						}
						virtual void set( int value )
						{
							Pc = value;
							InDelaySlot = false;
							DelayPc = 0;
							DelayNop = false;
						}
					}

					property array<int>^ GeneralRegisters
					{
						virtual array<int>^ get()
						{
							return Registers;
						}
					}

					// TODO: Make state get/set faster
					property Object^ Context
					{
						virtual Object^ get();
						virtual void set( Object^ value );
					}

					property int Pc;
					property array<int>^ Registers;
					property int Hi;
					property int Lo;
					property bool LL;
					
					property bool InDelaySlot;
					property int DelayPc;
					property bool DelayNop;

					property int InterruptState;

					void Clear();

					bool Process( int instruction );
					int TranslateAddress( int address );
				};
			
			}
		}
	}
}
