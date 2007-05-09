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
	class vsh : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "vsh";
			}
		}

		#endregion

		#region State Management

		public vsh( Kernel kernel )
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
		[BiosFunction( 0x0B8DE38E, "vsh_0B8DE38E" )]
		int vsh_0B8DE38E(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0E20CEEC, "vsh_0E20CEEC" )]
		int vsh_0E20CEEC(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x61FE226A, "vsh_61FE226A" )]
		int vsh_61FE226A(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0F2F6718, "vsh_0F2F6718" )]
		int vsh_0F2F6718(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5E43E468, "vsh_5E43E468" )]
		int vsh_5E43E468(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD0A3A365, "vsh_D0A3A365" )]
		int vsh_D0A3A365(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7ADF6A69, "vsh_7ADF6A69" )]
		int vsh_7ADF6A69(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1A62E6EA, "vsh_1A62E6EA" )]
		int vsh_1A62E6EA(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5452A47B, "vsh_5452A47B" )]
		int vsh_5452A47B(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x81BBD0D1, "vsh_81BBD0D1" )]
		int vsh_81BBD0D1(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6286E89D, "vsh_6286E89D" )]
		int vsh_6286E89D(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x34DA1B67, "vsh_34DA1B67" )]
		int vsh_34DA1B67(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF1A72B42, "vsh_F1A72B42" )]
		int vsh_F1A72B42(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7DA81064, "vsh_7DA81064" )]
		int vsh_7DA81064(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFDEE4109, "vsh_FDEE4109" )]
		int vsh_FDEE4109(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA3C7DF11, "vsh_A3C7DF11" )]
		int vsh_A3C7DF11(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB55C095C, "vsh_B55C095C" )]
		int vsh_B55C095C(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x80FB409B, "vsh_80FB409B" )]
		int vsh_80FB409B(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x595F8E64, "vsh_595F8E64" )]
		int vsh_595F8E64(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4A619A1D, "vsh_4A619A1D" )]
		int vsh_4A619A1D(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3E00F3B4, "vsh_3E00F3B4" )]
		int vsh_3E00F3B4(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2695A5B8, "vsh_2695A5B8" )]
		int vsh_2695A5B8(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x182745AF, "vvvGetVersion" )]
		int vvvGetVersion(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x58925420, "vsh_58925420" )]
		int vsh_58925420(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x78A67F41, "vsh_78A67F41" )]
		int vsh_78A67F41(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xAA8B9556, "vsh_AA8B9556" )]
		int vsh_AA8B9556(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xEF3DAF7D, "vsh_EF3DAF7D" )]
		int vsh_EF3DAF7D(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC0CCA5B1, "vsh_C0CCA5B1" )]
		int vsh_C0CCA5B1(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB55DCA45, "vsh_B55DCA45" )]
		int vsh_B55DCA45(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x741C9BE1, "vsh_741C9BE1" )]
		int vsh_741C9BE1(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2B8B7775, "vsh_2B8B7775" )]
		int vsh_2B8B7775(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - 31F0FAB6 */
