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
	public static class ProgramDebugData
	{
		public static IProgramDebugData Load( DebugDataType dataType, Stream stream )
		{
			switch( dataType )
			{
				case DebugDataType.Elf:
					ElfDebugData edd = new ElfDebugData( stream );
					return edd;
				case DebugDataType.Symbols:
					SimpleElfDebugData sdd = new SimpleElfDebugData( stream );
					return sdd;
				case DebugDataType.Objdump:
					ObjdumpDebugData odd = new ObjdumpDebugData( stream );
					return odd;
				default:
					throw new NotImplementedException();
			}
		}
	}
}
