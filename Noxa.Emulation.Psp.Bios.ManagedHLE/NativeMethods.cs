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
		[StructLayout( LayoutKind.Sequential )]
		public struct SystemTime
		{
			public ushort Year;
			public ushort Month;
			public ushort DayOfWeek;
			public ushort Day;
			public ushort Hour;
			public ushort Minute;
			public ushort Second;
			public ushort Milliseconds;
		}

		[StructLayout( LayoutKind.Sequential, CharSet = CharSet.Unicode )]
		public struct TimeZoneInformation
		{
			public int bias;
			[MarshalAs( UnmanagedType.ByValTStr, SizeConst = 32 )]
			public string standardName;
			public SystemTime standardDate;
			public int standardBias;
			[MarshalAs( UnmanagedType.ByValTStr, SizeConst = 32 )]
			public string daylightName;
			public SystemTime daylightDate;
			public int daylightBias;
		}

		[DllImport( "kernel32.dll" )]
		public static extern uint GetTimeZoneInformation( out TimeZoneInformation lpTimeZoneInformation );

		[DllImport( "kernel32.dll" )]
		public static extern void GetSystemTimeAsFileTime( out ComTypes.FILETIME lpSystemTimeAsFileTime );

		[DllImport( "kernel32.dll" )]
		public static extern void GetLocalTime( out SystemTime lpSystemTime );

		[DllImport( "kernel32.dll" )]
		public static extern void GetSystemTime( out SystemTime lpSystemTime );

		[DllImport( "kernel32.dll" )]
		public static extern bool QueryPerformanceCounter( out long lpPerformanceCount );

		[DllImport( "kernel32.dll", SetLastError = true )]
		public static extern bool QueryPerformanceFrequency( out long lpFrequency );
	}
}
