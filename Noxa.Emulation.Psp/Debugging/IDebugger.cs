// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Noxa.Emulation.Psp.Debugging
{
	public enum DebugDataType
	{
		Objdump,
		Elf,
	}

	public interface IDebugger
	{
		IEmulationHost Host
		{
			get;
		}

		bool IsAttached
		{
			get;
		}

		IDebugControl Control
		{
			get;
		}

		IDebugInspector Inspector
		{
			get;
		}

		IProgramDebugData DebugData
		{
			get;
		}

		bool LoadDebugData( DebugDataType dataType, Stream stream );

		void Show();
		void Hide();
	}
}
