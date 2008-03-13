// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2008 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace Noxa.Emulation.Psp.Video.ManagedGL
{
	enum DisplayListState
	{
		Ready = 0,
		Drawing,
		Stalled,
		Done,
	}

	unsafe class DisplayList
	{
		public int ID;

		public void* Pointer;
		public uint StartAddress;
		public uint StallAddress;

		public DisplayListState State;

		public int CallbackID; // -1 if none
		public uint ContextAddress; // If >0, a pointer to the stored context to restore

		public uint Base;
		public uint[] Stack = new uint[ 32 ];
		public int StackIndex;
	}
}
