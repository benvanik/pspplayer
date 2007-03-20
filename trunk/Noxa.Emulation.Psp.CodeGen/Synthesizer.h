// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

#include "Label.h"
#include "InstructionSet.h"
#include "Operand.h"

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace CodeGen {

				class Synthesizer
				{
				public:

					Synthesizer();

					void Reset();

					void EncodeOperand1( const Operand& op1 );
					void EncodeOperand2( const Operand& op2 );
					void EncodeOperand3( const Operand& op3 );
					void EncodeImmediate( int i );
					void EncodeInstruction( const Instruction* instruction );

					int GetLength();
					int Commit( byte* buffer ) const;

				public:
					bool RelativeReference() const { return relative; }
					bool AbsoluteReference() const { return !relative; }
					bool HasDisplacement() const { return format.D1 || format.D2 || format.D3 || format.D4; }
					bool HasImmediate() const { return format.I1 || format.I2 || format.I3 || format.I4; }
					bool IsRipRelative() const { return modRM.mod == 0 && modRM.r_m == 5; }

					int GetImmediate() const;
					void SetImmediate( int imm );
					Label* GetReference();
					void SetReference( Label* reference );
					void SetJumpOffset( int offset );
					void SetCallOffset( int offset );
					void SetTarget( Reference* target );
					void SetDisplacement( int displacement );
					void AddDisplacement( int displacement );

				private:

					Operand::Type	_op1Type;
					Operand::Type	_op2Type;

					int				_op1Reg;
					int				_op2Reg;
					int				_baseReg;
					int				_indexReg;

					int				_scale;

					Label*			_reference;
					Reference*		_target;

					void EncodeBase( const Operand& base );
					void EncodeIndex( const Operand& index );

					void SetScale( int scale );

					void EncodeRexByte( const Instruction* instruction );
					void EncodeModField();
					void EncodeRMField( const Instruction* instruction );
					void EncodeRegField( const Instruction* instruction );
					void EncodeSibByte( const Instruction* instruction );

					void AddPrefix( byte p );

					bool relative;

					struct
					{
						bool P1 : 1;
						bool P2 : 1;
						bool P3 : 1;
						bool P4 : 1;
						bool REX : 1;
						bool O2 : 1;
						bool O1 : 1;
						bool modRM : 1;
						bool SIB : 1;
						bool D1 : 1;
						bool D2 : 1;
						bool D3 : 1;
						bool D4 : 1;
						bool I1 : 1;
						bool I2 : 1;
						bool I3 : 1;
						bool I4 : 1;
					} format;

					byte P1;   // Prefixes
					byte P2;
					byte P3;
					byte P4;
					struct
					{
						union
						{
							struct
							{
								byte B : 1;
								byte X : 1;
								byte R : 1;
								byte W : 1;
								byte prefix : 4;
							};

							byte b;
						};
					} REX;
					byte O1;   // Opcode
					byte O2;
					struct
					{
						union
						{
							struct
							{
								byte r_m : 3;
								byte reg : 3;
								byte mod : 2;
							};

							byte b;
						};
					} modRM;
					struct
					{
						union
						{
							struct
							{
								byte base : 3;
								byte index : 3;
								byte scale : 2;
							};

							byte b;
						};
					} SIB;
					union
					{
						int64 displacement;

						struct
						{
							byte D1;
							byte D2;
							byte D3;
							byte D4;
						};
					};
					union
					{
						int immediate;

						struct
						{
							byte I1;
							byte I2;
							byte I3;
							byte I4;
						};
					};

					static int Align( byte* buffer, int alignment, bool write );

				};

			}
		}
	}
}
