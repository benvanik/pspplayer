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
	//I have seen this module with several different names :(

	unsafe class sceSas : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceSas";
			}
		}

		#endregion

		#region State Management

		public sceSas(Kernel kernel)
			: base(kernel)
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
		[BiosFunction(0xa3589d81, "__sceSasCore")]
		public int __sceSasCore() { return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction(0x68a46b95, "__sceSasGetEndFlag")]
		public int __sceSasGetEndFlag() { return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction(0x440ca7d8, "__sceSasSetVolume")]
		public int __sceSasSetVolume() { return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction(0xad84d37f, "__sceSasSetPitch")]
		public int __sceSasSetPitch() { return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction(0x99944089, "__sceSasSetVoice")]
		public int __sceSasSetVoice() { return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction(0xb7660a23, "__sceSasSetNoise")]
		public int __sceSasSetNoise() { return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction(0x019b25eb, "__sceSasSetADSR")]
		public int __sceSasSetADSR() { return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction(0x9ec3676a, "__sceSasSetADSRmode")]
		public int __sceSasSetADSRmode() { return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction(0x5f9529f6, "__sceSasSetSL")]
		public int __sceSasSetSL() { return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction(0x74ae582a, "__sceSasGetEnvelopeHeight")]
		public int __sceSasGetEnvelopeHeight() { return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction(0xcbcd4f79, "__sceSasSetSimpleADSR")]
		public int __sceSasSetSimpleADSR() { return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction(0x42778a9f, "__sceSasInit")]
		public int __sceSasInit() { return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction(0xa0cf2fa4, "__sceSasSetKeyOff")]
		public int __sceSasSetKeyOff() { return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction(0x76f01aca, "__sceSasSetKeyOn")]
		public int __sceSasSetKeyOn() { return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction(0xf983b186, "sceSasCore_f983b186")]
		public int sceSasCore_f983b186() { return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction(0xd5a229c9, "__sceSasRevEVOL")]
		public int __sceSasRevEVOL() { return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction(0x33d4ab37, "__sceSasRevType")]
		public int __sceSasRevType() { return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction(0x267a6dd2, "__sceSasRevParam")]
		public int __sceSasRevParam() { return Module.NotImplementedReturn; }
	
		[NotImplemented]
		[Stateless]
		[BiosFunction(0x2c8e6ab3, "__sceSasGetPauseFlag")]
		public int __sceSasGetPauseFlag() { return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction(0x50a14dfc, "sceSasCore_50a14dfc")]
		public int sceSasCore_50a14dfc() { return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction(0x787d04d5, "__sceSasSetPause")]
		public int __sceSasSetPause() { return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction(0xa232cbe6, "sceSasCore_a232cbe6")]
		public int sceSasCore_a232cbe6() { return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction(0xd5ebbbcd, "sceSasCore_d5ebbbcd")]
		public int sceSasCore_d5ebbbcd() { return Module.NotImplementedReturn; }
	}
}

