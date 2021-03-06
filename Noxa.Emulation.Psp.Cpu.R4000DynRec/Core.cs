// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Noxa.Emulation.Psp.Cpu
{
	[Flags]
	enum CoreAttributes
	{
		Default = 0x000,
		HasCp2 = 0x001
	}

	class Core : ICpuCore
	{
		protected int _coreId;
		protected string _coreName;

		public Core( Cpu cpu, int coreId, string coreName, CoreAttributes attributes )
		{
			Debug.Assert( cpu != null );
			Debug.Assert( coreId >= 0 );
			Debug.Assert( coreName != null );

			Cpu = cpu;
			_coreId = coreId;
			_coreName = coreName;

			Cp0 = new Coprocessor0( _coreId );
			Cp1 = new Coprocessor1();

			bool hasCp2 = ( attributes & CoreAttributes.HasCp2 ) == CoreAttributes.HasCp2;
			if( hasCp2 )
				Cp2 = new Coprocessor2();
			
			Registers = new int[ 32 ];

			this.Clear();
		}

		internal Cpu Cpu;

		internal Coprocessor0 Cp0;
		internal Coprocessor1 Cp1;
		internal Coprocessor2 Cp2;

		internal int Pc;
		internal int[] Registers;
		internal int Hi;
		internal int Lo;
		internal bool LL;
		internal bool InDelaySlot;
		internal int DelayPc;
		internal bool DelayNop;
		internal int InterruptState;

		// Used for tracking instruction counts inside of blocks
		internal int BlockCounter;

		public string Name
		{
			get
			{
				return _coreName;
			}
		}

		public int ProgramCounter
		{
			get
			{
				return Pc;
			}
			set
			{
				Pc = value;
				InDelaySlot = false;
				DelayPc = 0;
				DelayNop = false;
			}
		}

		public int[] GeneralRegisters
		{
			get
			{
				return Registers;
			}
		}

		public void SetGeneralRegister( int register, int value )
		{
			Registers[ register ] = value;
		}

		#region Internal context

		private class CoreContext
		{
			public int ProgramCounter;
			public int[] GeneralRegisters;
			public int Hi;
			public int Lo;
			public bool LL;
			public bool InDelaySlot;
			public int DelayPc;
			public bool DelayNop;
			public int InterruptState;

			public Object Cp0;
			public Object Cp1;
		}

		#endregion

		public object Context
		{
			get
			{
				CoreContext context = new CoreContext();
				
				context.ProgramCounter = Pc;
				context.GeneralRegisters = ( int[] )Registers.Clone();
				context.Hi = Hi;
				context.Lo = Lo;
				context.LL = LL;
				context.InDelaySlot = InDelaySlot;
				context.DelayPc = DelayPc;
				context.DelayNop = DelayNop;
				context.InterruptState = InterruptState;

				context.Cp0 = Cp0.Context;
				context.Cp1 = Cp1.Context;

				return context;
			}
			set
			{
				CoreContext context = ( CoreContext )value;

				Pc = context.ProgramCounter;
				Registers = ( int[] )context.GeneralRegisters.Clone();
				Hi = context.Hi;
				Lo = context.Lo;
				LL = context.LL;
				InDelaySlot = context.InDelaySlot;
				DelayPc = context.DelayPc;
				DelayNop = context.DelayNop;
				InterruptState = context.InterruptState;

				Cp0.Context = context.Cp0;
				Cp1.Context = context.Cp1;

				// HACK: clear delay slot
				if( InDelaySlot == true )
				{
					InDelaySlot = false;
					Pc = DelayPc - 4;
					DelayPc = 0;
					DelayNop = false;
				}
			}
		}

		public CoreState State
		{
			get
			{
				CoreState state = new CoreState();

				state.ProgramCounter = this.Pc;
				state.GeneralRegisters = new int[ this.Registers.Length ];
				Array.Copy( this.Registers, state.GeneralRegisters, this.Registers.Length );
				state.Hi = this.Hi;
				state.Lo = this.Lo;
				state.LL = this.LL;

				state.Cp0ControlRegisters = Cp0.Control;
				state.Cp0Registers = new int[ Cp0.Registers.Length ];
				Array.Copy( Cp0.Registers, state.Cp0Registers, Cp0.Registers.Length );
				state.Cp0ConditionBit = Cp0.ConditionBit;

				state.FpuControlRegister = Cp1.ControlRegister;
				state.FpuRegisters = new float[ Cp1.Registers.Length ];
				Array.Copy( Cp1.Registers, state.FpuRegisters, Cp1.Registers.Length );

				return state;
			}
		}

		public void Clear()
		{
			Pc = 0;
			for( int n = 0; n < Registers.Length; n++ )
				Registers[ n ] = 0;
			Registers[ 29 ] = 0x087FFFFF;
			Hi = 0;
			Lo = 0;
			LL = false;
			InDelaySlot = false;
			DelayPc = 0;
			DelayNop = false;
			InterruptState = 0;

			Cp0.Clear();
			Cp1.Clear();
		}

		public bool Process( int instruction )
		{
			return false;
		}

		public int TranslateAddress( int address )
		{
			//if( ( address & 0x80000000 ) != 0 )
			//{
			//    // Kernel.... hack!
			//    address = address & 0x7FFFFFFF;
			//}
			//if( ( address & 0x40000000 ) != 0 )
			//{
			//    // Kernel.... hack!
			//    address = address & 0x3FFFFFFF;
			//}
			return address & 0x3FFFFFFF;
		}
	}
}
