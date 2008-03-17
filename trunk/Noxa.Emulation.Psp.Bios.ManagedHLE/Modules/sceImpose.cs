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
	unsafe class sceImpose : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceImpose";
			}
		}

		#endregion

		#region State Management

		public sceImpose( Kernel kernel )
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

		[Stateless]
		[BiosFunction( 0x24FD7BCF, "sceImposeGetLanguageMode" )]
		// manual add - ptrs to storage for values
		public int sceImposeGetLanguageMode( int language, int swapButtons )
		{
			// Values should eventually be loaded from the registry
			// /CONFIG/SYSTEM/XMB/language and /CONFIG/SYSTEM/XMB/button_assign
			if( language != 0 )
			{
				int* planguage = ( int* )_memorySystem.Translate( ( uint )language );
				*planguage = ( int )Language.English;
			}
			if( swapButtons != 0 )
			{
				int* pswapButtons = ( int* )_memorySystem.Translate( ( uint )swapButtons );
				*pswapButtons = 0;
			}
			return 0;
		}

		[Stateless]
		[BiosFunction( 0x36AA6E91, "sceImposeSetLanguageMode" )]
		// manual add
		public int sceImposeSetLanguageMode( int language, int swapButtons )
		{
			return 0;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x381BD9E7, "sceImposeHomeButton" )]
		// manual add
		public int sceImposeHomeButton() { return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x72189C48, "sceImposeSetUMDPopup" )]
		// manual add
		public int sceImposeSetUMDPopup( int enable ) { return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8C943191, "sceImposeGetBatteryIconStatus" )]
		// manual add
		public int sceImposeGetBatteryIconStatus( int status0, int status1 ) { return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE0887BC8, "sceImposeGetUMDPopup" )]
		// manual add
		public int sceImposeGetUMDPopup() { return Module.NotImplementedReturn; }
	}
}

/* GenerateStubsV2: auto-generated - 1E486ACC */
