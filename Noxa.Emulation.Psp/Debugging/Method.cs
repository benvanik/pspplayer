using System;
using System.Collections.Generic;
using System.Text;

namespace Noxa.Emulation.Psp.Debugging
{
	public abstract class Method
	{
		protected int _entryAddress;
		protected int _length;
		protected string _name;

		protected Dictionary<int, Instruction> _instructions;

		protected Method( int entryAddress, string name )
		{
			_entryAddress = entryAddress;
			_name = name;

			_instructions = new Dictionary<int, Instruction>();
		}

		public int EntryAddress
		{
			get
			{
				return _entryAddress;
			}
		}

		public string Name
		{
			get
			{
				return _name;
			}
		}

		public virtual Dictionary<int, Instruction> Instructions
		{
			get
			{
				return _instructions;
			}
		}

		public override string ToString()
		{
			//return string.Format( "[{0:X8}] {1} ({2} instructions)", _entryAddress, _name, _instructions.Count );
			return string.Format( "{0} (0x{1:X8})", _name, _entryAddress );
		}
	}
}
