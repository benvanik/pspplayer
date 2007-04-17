// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

using Noxa.Utilities;
using Noxa.Emulation.Psp;
using Noxa.Emulation.Psp.Bios;
using Noxa.Emulation.Psp.Cpu;

namespace Noxa.Emulation.Psp.Bios.ManagedHLE.Modules
{
	class sceUmdUser : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceUmdUser";
			}
		}

		#endregion

		#region State Management

		public sceUmdUser( Kernel kernel )
			: base( kernel )
		{
		}

		public override void Start()
		{
		}

		public override void Stop()
		{
		}

		#endregion

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x46EBB729, "sceUmdCheckMedium" )]
		// SDK location: /umd/pspumd.h:42
		// SDK declaration: int sceUmdCheckMedium(int a);
		public int sceUmdCheckMedium( int a ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC6183D47, "sceUmdActivate" )]
		// SDK location: /umd/pspumd.h:66
		// SDK declaration: int sceUmdActivate(int unit, const char *drive);
		public int sceUmdActivate( int unit, int drive ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE83742BA, "sceUmdDeactivate" )]
		// manual add
		public int sceUmdDeactivate( int unit, int drive )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6B4A146C, "sceUmdGetDriveStat" )]
		// manual add
		public int sceUmdGetDriveStat()
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x20628E6F, "sceUmdGetErrorStat" )]
		// manual add
		public int sceUmdGetErrorStat()
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8EF08FCE, "sceUmdWaitDriveStat" )]
		// SDK location: /umd/pspumd.h:75
		// SDK declaration: int sceUmdWaitDriveStat(int stat);
		public int sceUmdWaitDriveStat( int stat ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x56202973, "sceUmdWaitDriveStatWithTimer" )]
		// manual add - params not right?
		public int sceUmdWaitDriveStatWithTimer( int stat )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4A9E5E29, "sceUmdWaitDriveStatCB" )]
		// manual add
		public int sceUmdWaitDriveStatCB( int stat )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xAEE7404D, "sceUmdRegisterUMDCallBack" )]
		// SDK location: /umd/pspumd.h:89
		// SDK declaration: int sceUmdRegisterUMDCallBack(int cbid);
		public int sceUmdRegisterUMDCallBack( int cbid ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBD2BDE07, "sceUmdUnRegisterUMDCallBack" )]
		// manual add
		public int sceUmdUnRegisterUMDCallBack( int cbid )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x340B7686, "sceUmdGetDiscInfo" )]
		// manual add
		public int sceUmdGetDiscInfo( int discInfo )
		{
			return Module.NotImplementedReturn;
		}

	}
}

/* GenerateStubsV2: auto-generated - D87462A2 */
