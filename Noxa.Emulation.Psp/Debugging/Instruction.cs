using System;
using System.Collections.Generic;
using System.Text;

namespace Noxa.Emulation.Psp.Debugging
{
	public class Instruction
	{
		public int Address;
		public int Code;
		public string Opcode;
		public string Operands;

		public Instruction( int address, int code )
			: this( address, code, null, null )
		{
		}

		public Instruction( int address, int code, string opcode, string operands )
		{
			this.Address = address;
			this.Code = code;
			this.Opcode = opcode;
			this.Operands = operands;
		}

		public override string ToString()
		{
			return string.Format( "[{0:X8}] {1:X8}    {2} {3}", Address, Code, Opcode, Operands );
		}
	}
}
