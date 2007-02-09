// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

#include "R4000Ctx.h"

using namespace System;
using namespace System::Diagnostics;
using namespace Noxa::Emulation::Psp;

#define Cp1RegisterCount 32
#define Cp1RegistersSize ( Cp1RegisterCount * sizeof( float ) )

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace Cpu {
				
				[Flags]
				enum class R4000Cp1Flags
				{
					Default = 0x00,
					InexactOperation = 0x01,
					Underflow = 0x02,
					Overflow = 0x04,
					DivisionByZero = 0x08,
					InvalidOperation = 0x10,
					UnimplementedOperation = 0x20
				};

				enum class R4000Cp1RoundingMode
				{
					RoundToNearest = 0x00,
					RoundToZero = 0x01,
					RoundToPositiveInfinity = 0x02,
					RoundToNegativeInfinity = 0x03
				};

				ref class R4000Cp1
				{
				internal:
					int			_control;
					int*		_conditionBit;
					float*		_registers;

				protected:

					ref class R4000Cp1Context
					{
					public:
						array<float>^ GeneralRegisters;
						int ControlRegister;
						int ConditionBit;
					};

				public:

					R4000Cp1( R4000Ctx* ctx )
					{
						_conditionBit = &ctx->Cp1ConditionBit;
						_registers = ctx->Cp1Registers;

						this->Clear();
					}

					property int Control
					{
						virtual int get()
						{
							return _control;
						}
						virtual void set( int value )
						{
							Debug::Assert( false );
							//_control = value;
						}
					}

					property int Implementation;
					
					property array<float>^ Registers
					{
						virtual array<float>^ get()
						{
							array<float>^ ret = gcnew array<float>( Cp1RegisterCount );
							pin_ptr<float> ptr = &ret[ 0 ];
							memcpy( ptr, _registers, Cp1RegistersSize );
							return ret;
						}
					}

					property Object^ Context
					{
						virtual Object^ get()
						{
							R4000Cp1Context^ context = gcnew R4000Cp1Context();
							context->ControlRegister = Control;
							context->ConditionBit = *_conditionBit;
							context->GeneralRegisters = Registers;
							return context;
						}
						virtual void set( Object^ value )
						{
							R4000Cp1Context^ context = ( R4000Cp1Context^ )value;
							Control = context->ControlRegister;
							*_conditionBit = context->ConditionBit;
							for( int n = 0; n < Cp1RegisterCount; n++ )
								_registers[ n ] = context->GeneralRegisters[ n ];
						}
					}

					virtual void Clear()
					{
						for( int n = 0; n < Cp1RegisterCount; n++ )
							_registers[ n ] = 0.0f;

						Implementation = ( ( 0x05 ) << 8 ) | 0x10; // 10 = 0001.0000
						
						//this->FlushBit = false;
						*_conditionBit = 0;
						//this->RoundingMode = R4000Cp1RoundingMode::RoundToNearest;
					}

					// pg 188

					property bool FlushBit
					{
						bool get()
						{
							return ( ( Control & 0x01000000 ) >> 24 ) == 1 ? true : false;
						}
						void set( bool value )
						{
							int value1 = ( int )value;
							Control &= 0xFEFFFFFF;
							Control |= value1 << 24;
						}
					}

					property bool ConditionBit
					{
						virtual bool get()
						{
							//return ( ( Control & 0x00800000 ) >> 23 ) == 1 ? true : false;
							return ( *_conditionBit == 1 ) ? true : false;
						}
						virtual void set( bool value )
						{
							/*int value1 = ( int )value;
							Control &= 0xFF7FFFFF;
							Control |= value1 << 23;*/
							*_conditionBit = value;
						}
					}

					void SetCauseBits( R4000Cp1Flags flags )
					{
						int value = ( int )flags;
						Control &= 0xFFFC0FFF;
						Control |= value << 12;
					}

					void SetEnableBits( R4000Cp1Flags flags )
					{
						int value = ( int )flags;
						value &= 0x1F;
						Control &= 0xFFFFF07F;
						Control |= value << 7;
					}

					void SetFlagBits( R4000Cp1Flags flags )
					{
						int value = ( int )flags;
						value &= 0x1F;
						Control &= 0xFFFFFF83;
						Control |= value << 2;
					}

					property R4000Cp1RoundingMode RoundingMode
					{
						R4000Cp1RoundingMode get()
						{
							return ( R4000Cp1RoundingMode )( Control & 0x00000003 );
						}
						void set( R4000Cp1RoundingMode rm )
						{
							int value = ( int )rm;
							Control &= 0xFFFFFFFC;
							Control |= value;
						}
					}
/* All of this is legacy
					bool Process( int instruction )
					{
						// Should be on core
						// BC1F 665
						// CFC1 675
						// DMFC1 682
						// LDC1 688
						// MTC1 694
						// SDC1 701
						// SWC1 705

						// r-type
						//int cop1 = ( instruction >> 26 ) & 0x3F;
						//System::Diagnostics::Debug::Assert( cop1 == 0x11 );
						int fmt = ( instruction >> 21 ) & 0x1F;
						// 0 = S = single binary fp
						// 1 = D = double binary fp
						// 4 = W = single 32 binary fixed point
						// 5 = L = longword 64 binary fixed point
						int ft = ( instruction >> 16 ) & 0x1F; // source 2
						int fs = ( instruction >> 11 ) & 0x1F; // source 1
						int fd = ( instruction >> 6 ) & 0x1F; // dest
						int func = instruction & 0x3F;

						// Fetch mangle on sizing
						double fsd;
						double ftd;
						if( ( fmt == 0 ) || ( fmt == 4 ) )
						{
							fsd = Registers[ fs ];
							ftd = Registers[ ft ];
						}
						else
						{
							__int64 s0 = BitConverter::DoubleToInt64Bits( Registers[ fs + 1 ] );
							__int64 s1 = BitConverter::DoubleToInt64Bits( Registers[ fs ] );
							fsd = BitConverter::Int64BitsToDouble( ( s0 << 32 ) | s1 );
							__int64 t0 = BitConverter::DoubleToInt64Bits( Registers[ ft + 1 ] );
							__int64 t1 = BitConverter::DoubleToInt64Bits( Registers[ ft ] );
							ftd = BitConverter::Int64BitsToDouble( ( t0 << 32 ) | t1 );
						}
						
						double fdd = 0.0;
						switch( func )
						{
						case 0:
							// add
							fdd = fsd + ftd;
							break;
						case 1:
							// sub
							fdd = fsd - ftd;
							break;
						case 2:
							// mul
							fdd = fsd * ftd;
							break;
						case 3:
							// div
							fdd = fsd / ftd;
							break;
						case 4:
							// sqrt
							fdd = Math::Sqrt( fsd );
							break;
						case 5:
							// abs
							fdd = Math::Abs( fsd );
							break;
						case 6:
							// mov
							fdd = fsd;
							break;
						case 7:
							// neg
							fdd = -fsd;
							break;
						case 8:
							// round.l
							fdd = ConvertFormat( Math::Round( fsd, MidpointRounding::ToEven ), fmt, 5 );
							fmt = 5;
							break;
						case 9:
							// trunc.l
							fdd = ConvertFormat( Math::Truncate( fsd ), fmt, 5 );
							fmt = 5;
							break;
						case 10:
							// ceil.l
							fdd = ConvertFormat( Math::Ceiling( fsd ), fmt, 5 );
							fmt = 5;
							break;
						case 11:
							// floor.l
							fdd = ConvertFormat( Math::Floor( fsd ), fmt, 5 );
							fmt = 5;
							break;
						case 12:
							// round.w
							fdd = ConvertFormat( Math::Round( fsd, MidpointRounding::ToEven ), fmt, 4 );
							fmt = 4;
							break;
						case 13:
							// trunc.w
							fdd = ConvertFormat( Math::Truncate( fsd ), fmt, 4 );
							fmt = 4;
							break;
						case 14:
							// ceil.w
							fdd = ConvertFormat( Math::Ceiling( fsd ), fmt, 4 );
							fmt = 4;
							break;
						case 15:
							// floor.w
							fdd = ConvertFormat( Math::Floor( fsd ), fmt, 4 );
							fmt = 4;
							break;
						case 32:
							// cvt.s
							fdd = ConvertFormat( fsd, fmt, 0 );
							fmt = 0;
							break;
						case 33:
							// cvt.d
							fdd = ConvertFormat( fsd, fmt, 1 );
							fmt = 1;
							break;
						case 36:
							// cvt.w
							fdd = ConvertFormat( fsd, fmt, 4 );
							fmt = 4;
							break;
						case 37:
							// cvt.l
							fdd = ConvertFormat( fsd, fmt, 5 );
							fmt = 5;
							break;
						}
						if( ( func >= 48 ) &&
							( func <= 63 ) )
						{
							// compare
							int fc = ( instruction >> 4 ) & 0x03;
							int cond = instruction & 0x0F;

							bool less;
							bool equal;
							bool unordered;
							if( double::IsNaN( fsd ) || double::IsNaN( ftd ) )
							{
								less = false;
								equal = false;
								unordered = true;
								//if( cond bit 3 set ) signal invalidop
							}
							else
							{
								less = fsd < ftd;
								equal = fsd == ftd;
								unordered = false;
							}
							bool lessBit = ( ( cond >> 2 ) & 0x1 ) == 1 ? true : false;
							bool equalBit = ( ( cond >> 1 ) & 0x1 ) == 1 ? true : false;
							bool unorderedBit = ( cond & 0x1 ) == 1 ? true : false;
							bool condition = ( lessBit && less ) || ( equalBit && equal ) || ( unordered && unorderedBit );
							ConditionBit = condition;
							// COC[1] = condition
							// TODO: figure out what COC is
						}
						else
						{
							if( ( fmt == 0 ) || ( fmt == 4 ) )
							{
								Registers[ fd ] = fdd;
							}
							else
							{
								__int64 d = BitConverter::DoubleToInt64Bits( fdd );
								double d0 = BitConverter::Int64BitsToDouble( ( ( unsigned __int64 )d ) >> 32 );
								double d1 = BitConverter::Int64BitsToDouble( d & 0xFFFFFFFF );
								Registers[ fd + 1 ] = d0;
								Registers[ fd ] = d1;
							}
						}

						// TODO: Verify something was actually done
						return true;
					}

				protected:
					double ConvertFormat( double value, int original, int target )
					{
						// 0 = S = single binary fp
						// 1 = D = double binary fp
						// 4 = W = single 32 binary fixed point
						// 5 = L = longword 64 binary fixed point

						if( original == target )
							return value;

						else if( target == 0 )
							return ( float )value;
						else if( target == 1 )
							return ( double )value;
						else if( target == 4 )
							return ( double )( ( int )value );
						else if( target == 5 )
							return ( double )( ( __int64 )value );

						return value;
					}*/
				};

			}
		}
	}
}
