// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace Noxa.Emulation.Psp.RemoteDebugger.Model
{
	abstract class TableEntry
	{
		public static TableEntry Invalid
		{
			get
			{
				return null;
			}
		}
	}

	class InstructionEntry : TableEntry
	{
		public readonly string Name;
		public readonly InstructionFormatter Formatter;
		public readonly uint Flags;

		public InstructionEntry( string name, InstructionFormatter formatter, uint flags )
		{
			this.Name = name;
			this.Formatter = formatter;
			this.Flags = flags;
		}
	}

	class TableReference : TableEntry
	{
		public readonly InstructionTables.InstructionEncoding Reference;

		public TableReference( InstructionTables.InstructionEncoding reference )
		{
			this.Reference = reference;
		}
	}
}
