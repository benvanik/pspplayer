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
	class sceParseUri : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceParseUri";
			}
		}

		#endregion

		#region State Management

		public sceParseUri( Kernel kernel )
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
		[BiosFunction( 0x49E950EC, "sceUriEscape" )]
		public int sceUriEscape(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x062BB07E, "sceUriUnescape" )]
		public int sceUriUnescape(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x568518C9, "sceUriParse" )]
		public int sceUriParse(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7EE318AF, "sceUriBuild" )]
		public int sceUriBuild(){ return Module.NotImplementedReturn; }
	}
}

/* GenerateStubsV2: auto-generated - 7ADAA780 */
