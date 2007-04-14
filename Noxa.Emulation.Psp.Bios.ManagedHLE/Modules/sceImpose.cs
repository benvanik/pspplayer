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
	class sceImpose : Module
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

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x24FD7BCF, "sceRegOpenRegistry" )]
		// manual add - ptrs to storage for values
		int sceImposeGetLanguageMode( int language, int swapButtons ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x36AA6E91, "sceRegOpenRegistry" )]
		// manual add
		int sceImposeSetLanguageMode( int language, int swapButtons ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x381BD9E7, "sceRegOpenRegistry" )]
		// manual add
		int sceImposeHomeButton(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x72189C48, "sceRegOpenRegistry" )]
		// manual add
		int sceImposeSetUMDPopup( int enable ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8C943191, "sceRegOpenRegistry" )]
		// manual add
		int sceImposeGetBatteryIconStatus( int status0, int status1 ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE0887BC8, "sceRegOpenRegistry" )]
		// manual add
		int sceImposeGetUMDPopup(){ return Module.NotImplementedReturn; }

	}
}
