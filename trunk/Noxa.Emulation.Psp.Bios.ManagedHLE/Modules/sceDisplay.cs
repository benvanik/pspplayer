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
	class sceDisplay : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceDisplay";
			}
		}

		#endregion

		#region State Management

		public sceDisplay( Kernel kernel )
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
		[BiosFunction( 0xDBA6C4C4, "sceDisplayGetFramePerSec" )]
		// manual add - is this int or float return?
		int sceDisplayGetFramePerSec()
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0E20F177, "sceDisplaySetMode" )]
		// SDK location: /display/pspdisplay.h:53
		// SDK declaration: int sceDisplaySetMode(int mode, int width, int height);
		int sceDisplaySetMode( int mode, int width, int height ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDEA197D4, "sceDisplayGetMode" )]
		// SDK location: /display/pspdisplay.h:64
		// SDK declaration: int sceDisplayGetMode(int *pmode, int *pwidth, int *pheight);
		int sceDisplayGetMode( int pmode, int pwidth, int pheight ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x289D82FE, "sceDisplaySetFrameBuf" )]
		// SDK location: /display/pspdisplay.h:74
		// SDK declaration: void sceDisplaySetFrameBuf(void *topaddr, int bufferwidth, int pixelformat, int sync);
		void sceDisplaySetFrameBuf( int topaddr, int bufferwidth, int pixelformat, int sync ){}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xEEDA2E54, "sceDisplayGetFrameBuf" )]
		// SDK location: /display/pspdisplay.h:84
		// SDK declaration: int sceDisplayGetFrameBuf(void **topaddr, int *bufferwidth, int *pixelformat, int *unk1);
		int sceDisplayGetFrameBuf( int topaddr, int bufferwidth, int pixelformat, int unk1 ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9C6EAAD7, "sceDisplayGetVcount" )]
		// SDK location: /display/pspdisplay.h:89
		// SDK declaration: unsigned int sceDisplayGetVcount();
		int sceDisplayGetVcount(){ return Module.NotImplementedReturn; }

		[Stateless]
		[BiosFunction( 0x773DD3A3, "sceDisplayGetCurrentHcount" )]
		// manual add
		int sceDisplayGetCurrentHcount()
		{
			return 0;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x210EAB3A, "sceDisplayGetAccumulatedHcount" )]
		// manual add
		int sceDisplayGetAccumulatedHcount()
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x36CDFADE, "sceDisplayWaitVblank" )]
		// SDK location: /display/pspdisplay.h:104
		// SDK declaration: int sceDisplayWaitVblank();
		int sceDisplayWaitVblank(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8EB9EC49, "sceDisplayWaitVblankCB" )]
		// SDK location: /display/pspdisplay.h:109
		// SDK declaration: int sceDisplayWaitVblankCB();
		int sceDisplayWaitVblankCB(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x984C27E7, "sceDisplayWaitVblankStart" )]
		// SDK location: /display/pspdisplay.h:94
		// SDK declaration: int sceDisplayWaitVblankStart();
		int sceDisplayWaitVblankStart(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x46F186C3, "sceDisplayWaitVblankStartCB" )]
		// SDK location: /display/pspdisplay.h:99
		// SDK declaration: int sceDisplayWaitVblankStartCB();
		int sceDisplayWaitVblankStartCB(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - B169BC61 */
