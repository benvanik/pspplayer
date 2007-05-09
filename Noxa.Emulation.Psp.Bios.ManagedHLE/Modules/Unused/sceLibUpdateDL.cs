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
	class sceLibUpdateDL : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceLibUpdateDL";
			}
		}

		#endregion

		#region State Management

		public sceLibUpdateDL( Kernel kernel )
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
		[BiosFunction( 0xFC1AB540, "sceUpdateDownloadInit" )]
		int sceUpdateDownloadInit(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF6690A9A, "sceUpdateDownloadInitEx" )]
		int sceUpdateDownloadInitEx(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x88FF3935, "sceLibUpdateDL_88FF3935" )]
		int sceLibUpdateDL_88FF3935(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB5DB018D, "sceLibUpdateDL_B5DB018D" )]
		int sceLibUpdateDL_B5DB018D(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF7E66CB4, "sceLibUpdateDL_F7E66CB4" )]
		int sceLibUpdateDL_F7E66CB4(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC1AF1076, "sceLibUpdateDL_C1AF1076" )]
		int sceLibUpdateDL_C1AF1076(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD6A09757, "sceUpdateDownloadEnd" )]
		int sceUpdateDownloadEnd(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFD675E8D, "sceLibUpdateDL_FD675E8D" )]
		int sceLibUpdateDL_FD675E8D(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFA9AA797, "sceUpdateDownloadReadData" )]
		int sceUpdateDownloadReadData(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4F49C9C1, "sceUpdateDownloadAbort" )]
		int sceUpdateDownloadAbort(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - 7376DCE1 */
