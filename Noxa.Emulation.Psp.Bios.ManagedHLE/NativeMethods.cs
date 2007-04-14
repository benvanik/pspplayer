// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Security;
using System.Text;
using System.Runtime.InteropServices;
using ComTypes = System.Runtime.InteropServices.ComTypes;

namespace Noxa.Emulation.Psp.Bios.ManagedHLE
{
	[SuppressUnmanagedCodeSecurity]
	static class NativeMethods
	{
		[DllImport( "kernel32.dll" )]
		public static extern void GetSystemTimeAsFileTime( out ComTypes.FILETIME lpSystemTimeAsFileTime );

		[DllImport( "kernel32.dll" )]
		public static extern bool QueryPerformanceCounter( out long lpPerformanceCount );

		[DllImport( "kernel32.dll", SetLastError = true )]
		public static extern bool QueryPerformanceFrequency( out long lpFrequency );
	}
}
