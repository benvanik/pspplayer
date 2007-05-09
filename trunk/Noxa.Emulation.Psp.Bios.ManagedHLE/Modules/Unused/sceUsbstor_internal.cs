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
	class sceUsbstor_internal : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceUsbstor_internal";
			}
		}

		#endregion

		#region State Management

		public sceUsbstor_internal( Kernel kernel )
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
		[BiosFunction( 0xDC1A5833, "sceUsbstorSendData" )]
		int sceUsbstorSendData(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFC4D0080, "sceUsbstorRecvData" )]
		int sceUsbstorRecvData(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x489E6BF1, "sceUsbstor_internal_489E6BF1" )]
		int sceUsbstor_internal_489E6BF1(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2CAFEFF1, "sceUsbstor_internal_2CAFEFF1" )]
		int sceUsbstor_internal_2CAFEFF1(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x50F99EAD, "sceUsbstorGetString" )]
		int sceUsbstorGetString(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - 72314EEC */
