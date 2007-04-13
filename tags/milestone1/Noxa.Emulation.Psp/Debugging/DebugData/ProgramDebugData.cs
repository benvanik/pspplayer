// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Noxa.Emulation.Psp.Debugging.DebugData
{
	/// <summary>
	/// Utility class for loading program debug databases.
	/// </summary>
	public static class ProgramDebugData
	{
		/// <summary>
		/// Create a program debug database from the given executable stream.
		/// </summary>
		/// <param name="dataType">The type of data to load from the stream.</param>
		/// <param name="stream">A stream containing the executable bytes.</param>
		/// <returns>A new <see cref="IProgramDebugData"/> instance of the given <paramref name="dataType"/>.</returns>
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
				default:
					throw new NotImplementedException();
			}
		}
	}
}
