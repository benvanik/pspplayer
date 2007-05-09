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
	class sceDmacplus_driver : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceDmacplus_driver";
			}
		}

		#endregion

		#region State Management

		public sceDmacplus_driver( Kernel kernel )
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
		[BiosFunction( 0xDF32F547, "sceDmacplusInit" )]
		int sceDmacplusInit(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE6219C61, "sceDmacplusEnd" )]
		int sceDmacplusEnd(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC748491D, "sceDmacplusQuerySuspend" )]
		int sceDmacplusQuerySuspend(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x853C5EC6, "sceDmacplusSuspend" )]
		int sceDmacplusSuspend(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD49A5922, "sceDmacplusResume" )]
		int sceDmacplusResume(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA7B05225, "sceDmacplusEnableIntr" )]
		int sceDmacplusEnableIntr(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBF939272, "sceDmacplusDisableIntr" )]
		int sceDmacplusDisableIntr(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6AA3D654, "sceDmacplusQueryIntr" )]
		int sceDmacplusQueryIntr(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0DA570C1, "sceDmacplusAcquireIntr" )]
		int sceDmacplusAcquireIntr(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFE3F3228, "sceDmacplusLcdcInit" )]
		int sceDmacplusLcdcInit(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCDB039A7, "sceDmacplusLcdcEnd" )]
		int sceDmacplusLcdcEnd(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC3236D69, "sceDmacplusLcdcQuerySuspend" )]
		int sceDmacplusLcdcQuerySuspend(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC8336549, "sceDmacplusLcdcSuspend" )]
		int sceDmacplusLcdcSuspend(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x42BEB349, "sceDmacplusLcdcResume" )]
		int sceDmacplusLcdcResume(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x29B50A82, "sceDmacplus_driver_29B50A82" )]
		int sceDmacplus_driver_29B50A82(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x58C380BB, "sceDmacplus_driver_58C380BB" )]
		int sceDmacplus_driver_58C380BB(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8AE579CD, "sceDmacplusLcdcSetFormat" )]
		int sceDmacplusLcdcSetFormat(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x241601AE, "sceDmacplusLcdcGetFormat" )]
		int sceDmacplusLcdcGetFormat(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF8C7C180, "sceDmacplusLcdcEnable" )]
		int sceDmacplusLcdcEnable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x576522BC, "sceDmacplusLcdcDisable" )]
		int sceDmacplusLcdcDisable(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBEC10779, "sceDmacplusAvcInit" )]
		int sceDmacplusAvcInit(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7AAE5443, "sceDmacplusAvcEnd" )]
		int sceDmacplusAvcEnd(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1E7483D0, "sceDmacplusAvcQuerySuspend" )]
		int sceDmacplusAvcQuerySuspend(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x168836CF, "sceDmacplusAvcSuspend" )]
		int sceDmacplusAvcSuspend(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5E32C8AF, "sceDmacplusAvcResume" )]
		int sceDmacplusAvcResume(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB2EFA681, "sceDmacplus_driver_B2EFA681" )]
		int sceDmacplus_driver_B2EFA681(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1D73DDA8, "sceDmacplus_driver_1D73DDA8" )]
		int sceDmacplus_driver_1D73DDA8(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x74323807, "sceDmacplus_driver_74323807" )]
		int sceDmacplus_driver_74323807(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3A98EE05, "sceDmacplusAvcSync" )]
		int sceDmacplusAvcSync(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFD8B15BC, "sceDmacplusSc2MeInit" )]
		int sceDmacplusSc2MeInit(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF7DEC189, "sceDmacplusSc2MeEnd" )]
		int sceDmacplusSc2MeEnd(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD0464B29, "sceDmacplusSc2MeQuerySuspend" )]
		int sceDmacplusSc2MeQuerySuspend(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFEE3392C, "sceDmacplusSc2MeSuspend" )]
		int sceDmacplusSc2MeSuspend(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC5B75E09, "sceDmacplusSc2MeResume" )]
		int sceDmacplusSc2MeResume(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4B980588, "sceDmacplusSc2MeNormal16" )]
		int sceDmacplusSc2MeNormal16(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3438DA0B, "sceDmacplusSc2MeLLI" )]
		int sceDmacplusSc2MeLLI(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x58DE4914, "sceDmacplusSc2MeSync" )]
		int sceDmacplusSc2MeSync(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1F5A80D0, "sceDmacplusMe2ScInit" )]
		int sceDmacplusMe2ScInit(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD1E1567E, "sceDmacplusMe2ScEnd" )]
		int sceDmacplusMe2ScEnd(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9393B40D, "sceDmacplusMe2ScQuerySuspend" )]
		int sceDmacplusMe2ScQuerySuspend(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6D4F8BD7, "sceDmacplusMe2ScSuspend" )]
		int sceDmacplusMe2ScSuspend(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x78A55D62, "sceDmacplusMe2ScResume" )]
		int sceDmacplusMe2ScResume(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBE693828, "sceDmacplusMe2ScNormal16" )]
		int sceDmacplusMe2ScNormal16(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2D5940FF, "sceDmacplusMe2ScLLI" )]
		int sceDmacplusMe2ScLLI(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xAB49D2CB, "sceDmacplusMe2ScSync" )]
		int sceDmacplusMe2ScSync(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB6592A56, "sceDmacplusSc128Init" )]
		int sceDmacplusSc128Init(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4F1B8D26, "sceDmacplusSc128End" )]
		int sceDmacplusSc128End(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x410ED0DD, "sceDmacplusSc128QuerySuspend" )]
		int sceDmacplusSc128QuerySuspend(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3DB353DB, "sceDmacplusSc128Suspend" )]
		int sceDmacplusSc128Suspend(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x97DCBBBF, "sceDmacplusSc128Resume" )]
		int sceDmacplusSc128Resume(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC7D33466, "sceDmacplusSc128Memcpy" )]
		int sceDmacplusSc128Memcpy(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x28558DBF, "sceDmacplusSc128TryMemcpy" )]
		int sceDmacplusSc128TryMemcpy(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFD183BCF, "sceDmacplusSc128LLI" )]
		int sceDmacplusSc128LLI(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - 7F24300A */
