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
	class Coprocessor0
	{
		public int[] Registers;
		public int[] Control;
		public int CoreId;
		public bool ConditionBit;

		#region Internal context

		private class Cp0Context
		{
			public int[] GeneralRegisters;
			public int[] ControlRegisters;
			public bool ConditionBit;
		}

		#endregion

		public Coprocessor0( int coreId )
		{
			CoreId = coreId;
			Registers = new int[ 32 ];
			Control = new int[ 32 ];
		}

		public object Context
		{
			get
			{
				Cp0Context context = new Cp0Context();
				context.GeneralRegisters = ( int[] )Registers.Clone();
				context.ControlRegisters = ( int[] )Control.Clone();
				context.ConditionBit = ConditionBit;
				return context;
			}
			set
			{
				Cp0Context context = ( Cp0Context )value;
				Registers = ( int[] )context.GeneralRegisters.Clone();
				Control = ( int[] )context.ControlRegisters.Clone();
				ConditionBit = context.ConditionBit;
			}
		}

		public void Clear()
		{
			for( int n = 0; n < Registers.Length; n++ )
				Registers[ n ] = 0;
			for( int n = 0; n < Control.Length; n++ )
				Control[ n ] = 0;

			Registers[ 15 ] = 0; // revision id
			Registers[ 16 ] = 0; // configuration
			Registers[ 22 ] = CoreId;
			// 21: SC-code, SC-code << 2 ????
			// 24 ??
		}

		public int this[ int index ]
		{
			get
			{
				return Registers[ index ];
			}
			set
			{
				switch( index )
				{
				case 9:
				case 11:
				case 12:
				case 13:
				case 14:
				case 25:
				case 28:
				case 29:
				case 30:
					// Can write, but check perms
					break;
				}
			}
		}

		public int GetControlRegister( int reg )
		{
			return Control[ reg ];
		}

		public void SetControlRegister( int reg, int value )
		{
			Control[ reg ] = value;
		}

		public void ThrowException( int pc, int cause )
		{
		}
	}
}
