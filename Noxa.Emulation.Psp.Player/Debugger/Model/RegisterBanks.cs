// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

using Noxa.Emulation.Psp.Debugging.Hooks;

namespace Noxa.Emulation.Psp.Player.Debugger.Model
{
	class RegisterBank
	{
		public readonly string Name;
		public readonly RegisterSet Set;
		public readonly Register[] Registers;

		public bool UseAltNames = false;

		public RegisterBank( string name, RegisterSet set, Register[] registers )
		{
			this.Name = name;
			this.Set = set;
			this.Registers = registers;
			foreach( Register register in registers )
				register.Bank = this;
		}
	}

	enum RegisterFormat
	{
		Integer,
		Single,
	}

	class Register
	{
		public RegisterBank Bank;
		public readonly int Ordinal;
		public readonly string Name;
		public readonly string AltName;
		public readonly RegisterFormat Format;
		public readonly bool ReadOnly;

		public Register( int ordinal, string name, string altName, RegisterFormat format )
		{
			this.Ordinal = ordinal;
			this.Name = name;
			this.AltName = altName;
			this.Format = format;
		}

		public Register( int ordinal, string name, string altName, RegisterFormat format, bool readOnly )
			: this( ordinal, name, altName, format )
		{
			this.ReadOnly = readOnly;
		}

		public override string ToString()
		{
			if( this.Bank.UseAltNames == true )
				return this.AltName;
			else
				return this.Name;
		}
	}

	static class RegisterBanks
	{
		public readonly static RegisterBank General;
		public readonly static RegisterBank Fpu;
		public readonly static RegisterBank Vfpu;

		static RegisterBanks()
		{
			General = new RegisterBank( "General", RegisterSet.Gpr, new Register[]{
				new Register( 0, "$0", "$0", RegisterFormat.Integer, true ),
				new Register( 1, "$at", "$1", RegisterFormat.Integer ),
				new Register( 2, "$v0", "$2", RegisterFormat.Integer ),
				new Register( 3, "$v1", "$3", RegisterFormat.Integer ),
				new Register( 4, "$a0", "$4", RegisterFormat.Integer ),
				new Register( 5, "$a1", "$5", RegisterFormat.Integer ),
				new Register( 6, "$a2", "$6", RegisterFormat.Integer ),
				new Register( 7, "$a3", "$7", RegisterFormat.Integer ),
				new Register( 8, "$t0", "$8", RegisterFormat.Integer ),
				new Register( 9, "$t1", "$9", RegisterFormat.Integer ),
				new Register( 10, "$t2", "$10", RegisterFormat.Integer ),
				new Register( 11, "$t3", "$11", RegisterFormat.Integer ),
				new Register( 12, "$t4", "$12", RegisterFormat.Integer ),
				new Register( 13, "$t5", "$13", RegisterFormat.Integer ),
				new Register( 14, "$t6", "$14", RegisterFormat.Integer ),
				new Register( 15, "$t7", "$15", RegisterFormat.Integer ),
				new Register( 16, "$s0", "$16", RegisterFormat.Integer ),
				new Register( 17, "$s1", "$17", RegisterFormat.Integer ),
				new Register( 18, "$s2", "$18", RegisterFormat.Integer ),
				new Register( 19, "$s3", "$19", RegisterFormat.Integer ),
				new Register( 20, "$s4", "$20", RegisterFormat.Integer ),
				new Register( 21, "$s5", "$21", RegisterFormat.Integer ),
				new Register( 22, "$s6", "$22", RegisterFormat.Integer ),
				new Register( 23, "$s7", "$23", RegisterFormat.Integer ),
				new Register( 24, "$t8", "$24", RegisterFormat.Integer ),
				new Register( 25, "$t9", "$25", RegisterFormat.Integer ),
				new Register( 26, "$k0", "$26", RegisterFormat.Integer ),
				new Register( 27, "$k1", "$27", RegisterFormat.Integer ),
				new Register( 28, "$gp", "$28", RegisterFormat.Integer ),
				new Register( 29, "$sp", "$29", RegisterFormat.Integer ),
				new Register( 30, "$fp", "$30", RegisterFormat.Integer ),
				new Register( 31, "$ra", "$31", RegisterFormat.Integer ),
			} );

			Register[] fpu = new Register[ 32 ];
			for( int n = 0; n < fpu.Length; n++ )
				fpu[ n ] = new Register( n, string.Format( "f{0}", n ), string.Format( "$f{0}", n ), RegisterFormat.Single );
			Fpu = new RegisterBank( "FPU", RegisterSet.Fpu, fpu );

			Register[] vfpu = new Register[ 128 ];
			for( int n = 0; n < vfpu.Length; n++ )
				vfpu[ n ] = new Register( n, string.Format( "v{0:000}", n ), string.Format( "$v{0}", n ), RegisterFormat.Single );
			Vfpu = new RegisterBank( "VFPU", RegisterSet.Vfpu, vfpu );
		}
	}
}
