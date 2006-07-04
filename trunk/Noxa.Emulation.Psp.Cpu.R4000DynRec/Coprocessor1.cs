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
		public double[] Registers;

		#region Internal context

		private class Cp1Context
		{
			public double[] GeneralRegisters;
			public uint ControlRegister;
		}

		#endregion

		public Coprocessor1()
		{
			Registers = new double[ 32 ];
		}

		public object Context
		{
			get
			{
				Cp1Context context = new Cp1Context();
				context.ControlRegister = ControlRegister;
				context.GeneralRegisters = ( double[] )Registers.Clone();
				return context;
			}
			set
			{
				Cp1Context context = ( Cp1Context )value;
				ControlRegister = context.ControlRegister;
				Registers = ( double[] )context.GeneralRegisters.Clone();
			}
		}

		public virtual void Clear()
		{
			for( int n = 0; n < Registers.Length; n++ )
				Registers[ n ] = 0.0;

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
			uint value = ( uint )flags;
			ControlRegister &= 0xFFFC0FFF;
			ControlRegister |= value << 12;
		}

		public void SetEnableBits( FpuFlags flags)
		{
			uint value = ( uint )flags;
			value &= 0x1F;
			ControlRegister &= 0xFFFFF07F;
			ControlRegister |= value << 7;
		}

		public void SetFlagBits( FpuFlags flags )
		{
			uint value = ( uint )flags;
			value &= 0x1F;
			ControlRegister &= 0xFFFFFF83;
			ControlRegister |= value << 2;
		}

		public FpuRoundingMode RoundingMode
		{
			get
			{
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
