// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Noxa.Emulation.Psp.Cpu;
using System.Diagnostics;

namespace Noxa.Emulation.Psp.Bios.GenericHle.Modules
{
	class Power : IModule
	{
		#region IModule Members
		
		protected HleInstance _hle;
		protected Kernel _kernel;

		public Power( HleInstance hle )
		{
			Debug.Assert( hle != null );

			_hle = hle;
			_kernel = hle.Kernel as Kernel;
		}

		public string Name
		{
			get
			{
				return "scePower";
			}
		}

		#endregion

		[BiosStub( 0x2b51fe2f, "", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown1( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x442bfbac, "", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown2( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xefd3c963, "scePowerTick", true, 1 )]
		public int scePowerTick( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = int unknown

			// Generates a power tick to prevent the psp from going into standby

			// int
			return 0;
		}

		[BiosStub( 0xedc13fe5, "scePowerGetIdleTimer", true, 1 )]
		[BiosStubIncomplete]
		public int scePowerGetIdleTimer( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = void

			// int
			return 0;
		}

		[BiosStub( 0x7f30b3b1, "scePowerIdleTimerEnable", true, 1 )]
		[BiosStubIncomplete]
		public int scePowerIdleTimerEnable( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = int unknown

			// int
			return 0;
		}

		[BiosStub( 0x972ce941, "scePowerIdleTimerDisable", true, 1 )]
		[BiosStubIncomplete]
		public int scePowerIdleTimerDisable( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = int unknown

			// int
			return 0;
		}

		[BiosStub( 0x27f3292c, "scePowerBatteryUpdateInfo", false, 0 )]
		[BiosStubIncomplete]
		public int scePowerBatteryUpdateInfo( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xe8e4e204, "", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown3( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xb999184c, "scePowerGetLowBatteryCapacity", false, 0 )]
		[BiosStubIncomplete]
		public int scePowerGetLowBatteryCapacity( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x87440f5e, "scePowerIsPowerOnline", true, 0 )]
		public int scePowerIsPowerOnline( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// External power always on

			// int
			return 1;
		}

		[BiosStub( 0x0afd0d8b, "scePowerIsBatteryExist", true, 0 )]
		public int scePowerIsBatteryExist( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// Never a battery

			// int
			return 0;
		}

		[BiosStub( 0x1e490401, "scePowerIsBatteryCharging", true, 0 )]
		public int scePowerIsBatteryCharging( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// No battery, no charge

			// int
			return 0;
		}

		[BiosStub( 0xb4432bc8, "scePowerGetBatteryChargingStatus", true, 0 )]
		public int scePowerGetBatteryChargingStatus( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// No battery, no status

			// int
			return 0;
		}

		[BiosStub( 0xd3075926, "scePowerIsLowBattery", true, 0 )]
		public int scePowerIsLowBattery( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// No battery

			// int
			return 0;
		}

		[BiosStub( 0x78a1a796, "", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown4( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x94f5a53f, "scePowerGetBatteryRemainCapacity", true, 0 )]
		public int scePowerGetBatteryRemainCapacity( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// No battery

			// int
			return 0;
		}

		[BiosStub( 0xfd18a0ff, "", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown5( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x2085d15d, "scePowerGetBatteryLifePercent", true, 0 )]
		public int scePowerGetBatteryLifePercent( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// No battery

			// int
			return 0;
		}

		[BiosStub( 0x8efb3fa2, "scePowerGetBatteryLifeTime", true, 0 )]
		public int scePowerGetBatteryLifeTime( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// No battery

			// int
			return 0;
		}

		[BiosStub( 0x28e12023, "scePowerGetBatteryTemp", true, 0 )]
		public int scePowerGetBatteryTemp( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// No battery

			// int
			return 0;
		}

		[BiosStub( 0x862ae1a6, "scePowerGetBatteryElec", true, 0 )]
		public int scePowerGetBatteryElec( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// No battery

			// int
			return 0;
		}

		[BiosStub( 0x483ce86b, "scePowerGetBatteryVolt", true, 0 )]
		public int scePowerGetBatteryVolt( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// No battery

			// int
			return 0;
		}

		[BiosStub( 0x23436a4a, "", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown6( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x0cd21b1f, "", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown7( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x165ce085, "", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown8( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xd6d016ef, "scePowerLock", true, 1 )]
		[BiosStubIncomplete]
		public int scePowerLock( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = int unknown

			// Power off will not take effect if locked (only after unlocked)

			// int
			return 0;
		}

		[BiosStub( 0x23c31ffe, "", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown9( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xfa97a599, "", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown10( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xca3d34c1, "scePowerUnlock", true, 1 )]
		[BiosStubIncomplete]
		public int scePowerUnlock( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = int unknown

			// End lock, turn off if a power off was attempted

			// int
			return 0;
		}

		[BiosStub( 0xb3edd801, "", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown11( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xdb62c9cf, "scePowerCancelRequest", false, 0 )]
		[BiosStubIncomplete]
		public int scePowerCancelRequest( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x7fa406dd, "scePowerIsRequest", false, 0 )]
		[BiosStubIncomplete]
		public int scePowerIsRequest( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x2b7c7cf4, "scePowerRequestStandby", false, 0 )]
		[BiosStubIncomplete]
		public int scePowerRequestStandby( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xac32c9cc, "scePowerRequestSuspend", false, 0 )]
		[BiosStubIncomplete]
		public int scePowerRequestSuspend( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x2875994b, "", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown12( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x3951af53, "scePowerEncodeUBattery", false, 0 )]
		[BiosStubIncomplete]
		public int scePowerEncodeUBattery( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x0074ef9b, "scePowerGetResumeCount", false, 0 )]
		[BiosStubIncomplete]
		public int scePowerGetResumeCount( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x04b7766e, "scePowerRegisterCallback", true, 2 )]
		[BiosStubIncomplete]
		public int scePowerRegisterCallback( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = int slot
			// a1 = SceUID cbid

			// int
			return 0;
		}

		[BiosStub( 0xdfa8baf8, "scePowerUnregisterCallback", false, 0 )]
		[BiosStubIncomplete]
		public int scePowerUnregisterCallback( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x0442d852, "", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown13( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xdb9d28dd, "scePowerUnregitserCallback", false, 0 )]
		[BiosStubIncomplete]
		public int scePowerUnregitserCallback( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x843fbf43, "scePowerSetCpuClockFrequency", true, 1 )]
		[BiosStubIncomplete]
		public int scePowerSetCpuClockFrequency( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = int cpufreq

			// int
			return 0;
		}

		[BiosStub( 0xb8d7b3fb, "scePowerSetBusClockFrequency", true, 1 )]
		[BiosStubIncomplete]
		public int scePowerSetBusClockFrequency( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = int busfreq

			// int
			return 0;
		}

		[BiosStub( 0xfee03a2f, "scePowerGetCpuClockFrequency", true, 0 )]
		[BiosStubIncomplete]
		public int scePowerGetCpuClockFrequency( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// int
			return 0;
		}

		[BiosStub( 0x478fe6f5, "scePowerGetBusClockFrequency", true, 0 )]
		[BiosStubIncomplete]
		public int scePowerGetBusClockFrequency( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// int
			return 0;
		}

		[BiosStub( 0xfdb5bfe9, "scePowerGetCpuClockFrequencyInt", true, 0 )]
		[BiosStubIncomplete]
		public int scePowerGetCpuClockFrequencyInt( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// int
			return 0;
		}

		[BiosStub( 0xbd681969, "scePowerGetBusClockFrequencyInt", true, 0 )]
		[BiosStubIncomplete]
		public int scePowerGetBusClockFrequencyInt( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// int
			return 0;
		}

		[BiosStub( 0xb1a52c83, "scePowerGetCpuClockFrequencyFloat", true, 0 )]
		[BiosStubIncomplete]
		public int scePowerGetCpuClockFrequencyFloat( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// float
			return 0;
		}

		[BiosStub( 0x9badb3eb, "scePowerGetBusClockFrequencyFloat", true, 0 )]
		[BiosStubIncomplete]
		public int scePowerGetBusClockFrequencyFloat( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// float
			return 0;
		}

		[BiosStub( 0x737486f2, "scePowerSetClockFrequency", true, 3 )]
		[BiosStubIncomplete]
		public int scePowerSetClockFrequency( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = int cpufreq
			// a1 = int ramfreq
			// a2 = int busfreq

			// int
			return 0;
		}

		[BiosStub( 0x34f9c463, "", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown14( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xea382a27, "", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown15( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}
	}
}
