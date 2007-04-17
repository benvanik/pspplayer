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
	class sceHprm : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceHprm";
			}
		}

		#endregion

		#region State Management

		public sceHprm( Kernel kernel )
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
		[BiosFunction( 0x208DB1BD, "sceHprmIsRemoteExist" )]
		// SDK location: /hprm/psphprm.h:78
		// SDK declaration: int sceHprmIsRemoteExist();
		public int sceHprmIsRemoteExist(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7E69EDA4, "sceHprmIsHeadphoneExist" )]
		// SDK location: /hprm/psphprm.h:71
		// SDK declaration: int sceHprmIsHeadphoneExist();
		public int sceHprmIsHeadphoneExist(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x219C58F1, "sceHprmIsMicrophoneExist" )]
		// SDK location: /hprm/psphprm.h:85
		// SDK declaration: int sceHprmIsMicrophoneExist();
		public int sceHprmIsMicrophoneExist(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1910B327, "sceHprmPeekCurrentKey" )]
		// SDK location: /hprm/psphprm.h:46
		// SDK declaration: int sceHprmPeekCurrentKey(u32 *key);
		public int sceHprmPeekCurrentKey( int key ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2BCEC83E, "sceHprmPeekLatch" )]
		// SDK location: /hprm/psphprm.h:55
		// SDK declaration: int sceHprmPeekLatch(u32 *latch);
		public int sceHprmPeekLatch( int latch ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x40D2F9F0, "sceHprmReadLatch" )]
		// SDK location: /hprm/psphprm.h:64
		// SDK declaration: int sceHprmReadLatch(u32 *latch);
		public int sceHprmReadLatch( int latch ){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - CFD8EAF0 */
