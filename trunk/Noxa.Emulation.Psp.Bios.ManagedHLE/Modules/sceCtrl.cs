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
	class sceCtrl : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceCtrl";
			}
		}

		#endregion

		#region State Management

		public sceCtrl( Kernel kernel )
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
		[BiosFunction( 0x6A2774F3, "sceCtrlSetSamplingCycle" )]
		// SDK location: /ctrl/pspctrl.h:119
		// SDK declaration: int sceCtrlSetSamplingCycle(int cycle);
		public int sceCtrlSetSamplingCycle( int cycle ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x02BAAD91, "sceCtrlGetSamplingCycle" )]
		// SDK location: /ctrl/pspctrl.h:128
		// SDK declaration: int sceCtrlGetSamplingCycle(int *pcycle);
		public int sceCtrlGetSamplingCycle( int pcycle ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1F4011E6, "sceCtrlSetSamplingMode" )]
		// SDK location: /ctrl/pspctrl.h:137
		// SDK declaration: int sceCtrlSetSamplingMode(int mode);
		public int sceCtrlSetSamplingMode( int mode ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDA6B76A1, "sceCtrlGetSamplingMode" )]
		// SDK location: /ctrl/pspctrl.h:146
		// SDK declaration: int sceCtrlGetSamplingMode(int *pmode);
		public int sceCtrlGetSamplingMode( int pmode ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA7144800, "sceCtrlSetIdleCancelThreshold" )]
		// manual add
		public int sceCtrlSetIdleCancelThreshold()
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3A622550, "sceCtrlPeekBufferPositive" )]
		// SDK location: /ctrl/pspctrl.h:148
		// SDK declaration: int sceCtrlPeekBufferPositive(SceCtrlData *pad_data, int count);
		public int sceCtrlPeekBufferPositive( int pad_data, int count ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC152080A, "sceCtrlPeekBufferNegative" )]
		// SDK location: /ctrl/pspctrl.h:150
		// SDK declaration: int sceCtrlPeekBufferNegative(SceCtrlData *pad_data, int count);
		public int sceCtrlPeekBufferNegative( int pad_data, int count ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1F803938, "sceCtrlReadBufferPositive" )]
		// SDK location: /ctrl/pspctrl.h:168
		// SDK declaration: int sceCtrlReadBufferPositive(SceCtrlData *pad_data, int count);
		public int sceCtrlReadBufferPositive( int pad_data, int count ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x60B81F86, "sceCtrlReadBufferNegative" )]
		// SDK location: /ctrl/pspctrl.h:170
		// SDK declaration: int sceCtrlReadBufferNegative(SceCtrlData *pad_data, int count);
		public int sceCtrlReadBufferNegative( int pad_data, int count ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB1D0E5CD, "sceCtrlPeekLatch" )]
		// SDK location: /ctrl/pspctrl.h:172
		// SDK declaration: int sceCtrlPeekLatch(SceCtrlLatch *latch_data);
		public int sceCtrlPeekLatch( int latch_data ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0B588501, "sceCtrlReadLatch" )]
		// SDK location: /ctrl/pspctrl.h:174
		// SDK declaration: int sceCtrlReadLatch(SceCtrlLatch *latch_data);
		public int sceCtrlReadLatch( int latch_data ){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - D9D618DF */
