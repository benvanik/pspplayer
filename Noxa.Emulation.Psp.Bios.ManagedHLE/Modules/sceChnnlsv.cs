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
	class sceChnnlsv : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceChnnlsv";
			}
		}

		#endregion

		#region State Management

		public sceChnnlsv( Kernel kernel )
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
		[BiosFunction( 0xE7833020, "sceChnnlsv_E7833020" )]
		// SDK location: /vsh/pspchnnlsv.h:53
		// SDK declaration: int sceChnnlsv_E7833020(pspChnnlsvContext1 *ctx, int mode);
		public int sceChnnlsv_E7833020( int ctx, int mode ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF21A1FCA, "sceChnnlsv_F21A1FCA" )]
		// SDK location: /vsh/pspchnnlsv.h:63
		// SDK declaration: int sceChnnlsv_F21A1FCA(pspChnnlsvContext1 *ctx, unsigned char *data, int len);
		public int sceChnnlsv_F21A1FCA( int ctx, int data, int len ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC4C494F8, "sceChnnlsv_C4C494F8" )]
		// SDK location: /vsh/pspchnnlsv.h:73
		// SDK declaration: int sceChnnlsv_C4C494F8(pspChnnlsvContext1 *ctx, unsigned char *hash, unsigned char *cryptkey);
		public int sceChnnlsv_C4C494F8( int ctx, int hash, int cryptkey ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xABFDFC8B, "sceChnnlsv_ABFDFC8B" )]
		// SDK location: /vsh/pspchnnlsv.h:86
		// SDK declaration: int sceChnnlsv_ABFDFC8B(pspChnnlsvContext2 *ctx, int mode1, int mode2, unsigned char *hashkey, unsigned char *cipherkey);
		public int sceChnnlsv_ABFDFC8B( int ctx, int mode1, int mode2, int hashkey, int cipherkey ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x850A7FA1, "sceChnnlsv_850A7FA1" )]
		// SDK location: /vsh/pspchnnlsv.h:97
		// SDK declaration: int sceChnnlsv_850A7FA1(pspChnnlsvContext2 *ctx, unsigned char *data, int len);
		public int sceChnnlsv_850A7FA1( int ctx, int data, int len ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x21BE78B4, "sceChnnlsv_21BE78B4" )]
		// SDK location: /vsh/pspchnnlsv.h:105
		// SDK declaration: int sceChnnlsv_21BE78B4(pspChnnlsvContext2 *ctx);
		public int sceChnnlsv_21BE78B4( int ctx ){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - C412570C */
