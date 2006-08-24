// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace Noxa.Emulation.Psp.Cpu
{
	[Flags]
	enum FpuFlags
	{
		Default = 0x00,
		InexactOperation = 0x01,
		Underflow = 0x02,
		Overflow = 0x04,
		DivisionByZero = 0x08,
		InvalidOperation = 0x10,
		UnimplementedOperation = 0x20
	};

	enum FpuRoundingMode
	{
		RoundToNearest = 0x00,
		RoundToZero = 0x01,
		RoundToPositiveInfinity = 0x02,
		RoundToNegativeInfinity = 0x03
	};

	class Coprocessor1
	{
		public uint ControlRegister;
		public uint Implementation;
		public float[] Registers;

		#region Internal context

		private class Cp1Context
		{
			public float[] GeneralRegisters;
			public uint ControlRegister;
		}

		#endregion

		public Coprocessor1()
		{
			Registers = new float[ 32 ];
		}

		public object Context
		{
			get
			{
				Cp1Context context = new Cp1Context();
				context.ControlRegister = ControlRegister;
				context.GeneralRegisters = ( float[] )Registers.Clone();
				return context;
			}
			set
			{
				Cp1Context context = ( Cp1Context )value;
				ControlRegister = context.ControlRegister;
				Registers = ( float[] )context.GeneralRegisters.Clone();
			}
		}

		public virtual void Clear()
		{
			for( int n = 0; n < Registers.Length; n++ )
				Registers[ n ] = 0.0f;

			Implementation = ( ( 0x05 ) << 8 ) | 0x10; // 10 = 0001.0000
			
			FlushBit = false;
			ConditionBit = false;
			RoundingMode = FpuRoundingMode.RoundToNearest;
		}

		// pg 188

		public bool FlushBit
		{
			get
			{
				// TODO: optimize - no reason to have a control register!
				return ( ( ControlRegister & 0x01000000 ) >> 24 ) == 1 ? true : false;
			}
			set
			{
				uint value1 = ( value == true ) ? 1u : 0u;
				ControlRegister &= 0xFEFFFFFF;
				ControlRegister |= value1 << 24;
			}
		}

		public bool ConditionBit
		{
			get
			{
				// TODO: optimize - no reason to have a control register!
				return ( ( ControlRegister & 0x00800000 ) >> 23 ) == 1 ? true : false;
			}
			set
			{
				uint value1 = ( value == true ) ? 1u : 0u;
				ControlRegister &= 0xFF7FFFFF;
				ControlRegister |= value1 << 23;
			}
		}

		public void SetCauseBits( FpuFlags flags )
		{
			// TODO: optimize - no reason to have a control register!
			uint value = ( uint )flags;
			ControlRegister &= 0xFFFC0FFF;
			ControlRegister |= value << 12;
		}

		public void SetEnableBits( FpuFlags flags)
		{
			// TODO: optimize - no reason to have a control register!
			uint value = ( uint )flags;
			value &= 0x1F;
			ControlRegister &= 0xFFFFF07F;
			ControlRegister |= value << 7;
		}

		public void SetFlagBits( FpuFlags flags )
		{
			// TODO: optimize - no reason to have a control register!
			uint value = ( uint )flags;
			value &= 0x1F;
			ControlRegister &= 0xFFFFFF83;
			ControlRegister |= value << 2;
		}

		public FpuRoundingMode RoundingMode
		{
			get
			{
				// TODO: optimize - no reason to have a control register!
				return ( FpuRoundingMode )( ControlRegister & 0x00000003 );
			}
			set
			{
				uint rm = ( uint )value;
				ControlRegister &= 0xFFFFFFFC;
				ControlRegister |= rm;
			}
		}
	}
}
