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
	class sceGe_driver : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceGe_driver";
			}
		}

		#endregion

		#region State Management

		public sceGe_driver( Kernel kernel )
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
		[BiosFunction( 0x71FCD1D6, "sceGeInit" )]
		int sceGeInit(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9F2C2948, "sceGeEnd" )]
		int sceGeEnd(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8F185DF7, "sceGeEdramInit" )]
		int sceGeEdramInit(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1F6752AD, "sceGeEdramGetSize" )]
		int sceGeEdramGetSize(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE47E40E4, "sceGeEdramGetAddr" )]
		int sceGeEdramGetAddr(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB77905EA, "sceGeEdramSetAddrTranslation" )]
		int sceGeEdramSetAddrTranslation(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB415364D, "sceGeGetReg" )]
		int sceGeGetReg(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDC93CFEF, "sceGeGetCmd" )]
		int sceGeGetCmd(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x57C8945B, "sceGeGetMtx" )]
		int sceGeGetMtx(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x438A385A, "sceGeSaveContext" )]
		int sceGeSaveContext(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0BF608FB, "sceGeRestoreContext" )]
		int sceGeRestoreContext(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xAB49E76A, "sceGeListEnQueue" )]
		int sceGeListEnQueue(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1C0D95A6, "sceGeListEnQueueHead" )]
		int sceGeListEnQueueHead(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5FB86AB0, "sceGeListDeQueue" )]
		int sceGeListDeQueue(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE0D68148, "sceGeListUpdateStallAddr" )]
		int sceGeListUpdateStallAddr(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x03444EB4, "sceGeListSync" )]
		int sceGeListSync(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB287BD61, "sceGeDrawSync" )]
		int sceGeDrawSync(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB448EC0D, "sceGeBreak" )]
		int sceGeBreak(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4C06E472, "sceGeContinue" )]
		int sceGeContinue(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA4FC06A4, "sceGeSetCallback" )]
		int sceGeSetCallback(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x05DB22CE, "sceGeUnsetCallback" )]
		int sceGeUnsetCallback(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9DA4A75F, "sceGe_driver_9DA4A75F" )]
		int sceGe_driver_9DA4A75F(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x114E1745, "sceGe_driver_114E1745" )]
		int sceGe_driver_114E1745(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - 521117F2 */
