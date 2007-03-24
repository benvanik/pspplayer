// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace Noxa.Emulation.Psp.Debugging.DebugModel
{
	public abstract class Method
	{
		protected int _entryAddress;
		protected int _length;
		protected string _name;

		protected Dictionary<int, Instruction> _instructions;

		protected Method( int entryAddress, int length, string name )
		{
			_entryAddress = entryAddress;
			_length = length;
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

		public int Length
		{
			get
			{
				return _length;
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
