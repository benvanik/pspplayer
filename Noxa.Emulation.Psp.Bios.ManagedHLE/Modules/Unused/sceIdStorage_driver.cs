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
	class sceIdStorage_driver : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceIdStorage_driver";
			}
		}

		#endregion

		#region State Management

		public sceIdStorage_driver( Kernel kernel )
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
		[BiosFunction( 0xAB129D20, "sceIdStorageInit" )]
		int sceIdStorageInit(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2CE0BE69, "sceIdStorageEnd" )]
		int sceIdStorageEnd(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF77565B6, "sceIdStorageSuspend" )]
		int sceIdStorageSuspend(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFE51173D, "sceIdStorageResume" )]
		int sceIdStorageResume(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xEB830733, "sceIdStorageGetLeafSize" )]
		int sceIdStorageGetLeafSize(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFEFA40C2, "sceIdStorageIsFormatted" )]
		int sceIdStorageIsFormatted(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2D633688, "sceIdStorageIsReadOnly" )]
		int sceIdStorageIsReadOnly(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB9069BAD, "sceIdStorageIsDirty" )]
		int sceIdStorageIsDirty(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x958089DB, "sceIdStorageFormat" )]
		int sceIdStorageFormat(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF4BCB3EE, "sceIdStorageUnformat" )]
		int sceIdStorageUnformat(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xEB00C509, "sceIdStorageReadLeaf" )]
		int sceIdStorageReadLeaf(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1FA4D135, "sceIdStorageWriteLeaf" )]
		int sceIdStorageWriteLeaf(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x08A471A6, "sceIdStorageCreateLeaf" )]
		int sceIdStorageCreateLeaf(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2C97AB36, "sceIdStorageDeleteLeaf" )]
		int sceIdStorageDeleteLeaf(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x99ACCB71, "sceIdStorage_driver_99ACCB71" )]
		int sceIdStorage_driver_99ACCB71(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x37833CB8, "sceIdStorage_driver_37833CB8" )]
		int sceIdStorage_driver_37833CB8(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x31E08AFB, "sceIdStorageEnumId" )]
		int sceIdStorageEnumId(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6FE062D1, "sceIdStorageLookup" )]
		int sceIdStorageLookup(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x683AAC10, "sceIdStorageUpdate" )]
		int sceIdStorageUpdate(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3AD32523, "sceIdStorageFlush" )]
		int sceIdStorageFlush(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - 55CC285F */
