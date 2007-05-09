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
	class sceNetAdhocMatching : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceNetAdhocMatching";
			}
		}

		#endregion

		#region State Management

		public sceNetAdhocMatching( Kernel kernel )
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
		[BiosFunction( 0x2A2A1E07, "sceNetAdhocMatchingInit" )]
		public int sceNetAdhocMatchingInit(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7945ECDA, "sceNetAdhocMatchingTerm" )]
		public int sceNetAdhocMatchingTerm(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCA5EDA6F, "sceNetAdhocMatchingCreate" )]
		public int sceNetAdhocMatchingCreate(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x93EF3843, "sceNetAdhocMatchingStart" )]
		public int sceNetAdhocMatchingStart(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x32B156B3, "sceNetAdhocMatchingStop" )]
		public int sceNetAdhocMatchingStop(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF16EAF4F, "sceNetAdhocMatchingDelete" )]
		public int sceNetAdhocMatchingDelete(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5E3D4B79, "sceNetAdhocMatchingSelectTarget" )]
		public int sceNetAdhocMatchingSelectTarget(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xEA3C6108, "sceNetAdhocMatchingCancelTarget" )]
		public int sceNetAdhocMatchingCancelTarget(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB58E61B7, "sceNetAdhocMatchingSetHelloOpt" )]
		public int sceNetAdhocMatchingSetHelloOpt(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB5D96C2A, "sceNetAdhocMatchingGetHelloOpt" )]
		public int sceNetAdhocMatchingGetHelloOpt(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC58BCD9E, "sceNetAdhocMatchingGetMembers" )]
		public int sceNetAdhocMatchingGetMembers(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x40F8F435, "sceNetAdhocMatchingGetPoolMaxAlloc" )]
		public int sceNetAdhocMatchingGetPoolMaxAlloc(){ return Module.NotImplementedReturn; }
	}
}

/* GenerateStubsV2: auto-generated - C7106C71 */
