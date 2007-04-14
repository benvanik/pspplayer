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
	class sceReg : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceReg";
			}
		}

		#endregion

		#region State Management

		public sceReg( Kernel kernel )
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
		[BiosFunction( 0x92E41280, "sceRegOpenRegistry" )]
		// SDK location: /registry/pspreg.h:68
		// SDK declaration: int sceRegOpenRegistry(struct RegParam *reg, int mode, REGHANDLE *h);
		int sceRegOpenRegistry( int reg, int mode, int h ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFA8A5739, "sceRegCloseRegistry" )]
		// SDK location: /registry/pspreg.h:86
		// SDK declaration: int sceRegCloseRegistry(REGHANDLE h);
		int sceRegCloseRegistry( int h ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDEDA92BF, "sceRegRemoveRegistry" )]
		// SDK location: /registry/pspreg.h:229
		// SDK declaration: int sceRegRemoveRegistry(struct RegParam *reg);
		int sceRegRemoveRegistry( int reg ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1D8A762E, "sceRegOpenCategory" )]
		// SDK location: /registry/pspreg.h:98
		// SDK declaration: int sceRegOpenCategory(REGHANDLE h, const char *name, int mode, REGHANDLE *hd);
		int sceRegOpenCategory( int h, int name, int mode, int hd ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0CAE832B, "sceRegCloseCategory" )]
		// SDK location: /registry/pspreg.h:117
		// SDK declaration: int sceRegCloseCategory(REGHANDLE hd);
		int sceRegCloseCategory( int hd ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x39461B4D, "sceRegFlushRegistry" )]
		// SDK location: /registry/pspreg.h:77
		// SDK declaration: int sceRegFlushRegistry(REGHANDLE h);
		int sceRegFlushRegistry( int h ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0D69BF40, "sceRegFlushCategory" )]
		// SDK location: /registry/pspreg.h:126
		// SDK declaration: int sceRegFlushCategory(REGHANDLE hd);
		int sceRegFlushCategory( int hd ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x57641A81, "sceRegCreateKey" )]
		// SDK location: /registry/pspreg.h:220
		// SDK declaration: int sceRegCreateKey(REGHANDLE hd, const char *name, int type, SceSize size);
		int sceRegCreateKey( int hd, int name, int type, int size ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x17768E14, "sceRegSetKeyValue" )]
		// SDK location: /registry/pspreg.h:187
		// SDK declaration: int sceRegSetKeyValue(REGHANDLE hd, const char *name, const void *buf, SceSize size);
		int sceRegSetKeyValue( int hd, int name, int buf, int size ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD4475AA8, "sceRegGetKeyInfo" )]
		// SDK location: /registry/pspreg.h:139
		// SDK declaration: int sceRegGetKeyInfo(REGHANDLE hd, const char *name, REGHANDLE *hk, unsigned int *type, SceSize *size);
		int sceRegGetKeyInfo( int hd, int name, int hk, int type, int size ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x28A8E98A, "sceRegGetKeyValue" )]
		// SDK location: /registry/pspreg.h:163
		// SDK declaration: int sceRegGetKeyValue(REGHANDLE hd, REGHANDLE hk, void *buf, SceSize size);
		int sceRegGetKeyValue( int hd, int hk, int buf, int size ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2C0DB9DD, "sceRegGetKeysNum" )]
		// SDK location: /registry/pspreg.h:197
		// SDK declaration: int sceRegGetKeysNum(REGHANDLE hd, int *num);
		int sceRegGetKeysNum( int hd, int num ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2D211135, "sceRegGetKeys" )]
		// SDK location: /registry/pspreg.h:208
		// SDK declaration: int sceRegGetKeys(REGHANDLE hd, char *buf, int num);
		int sceRegGetKeys( int hd, int buf, int num ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4CA16893, "sceRegRemoveCategory" )]
		// SDK location: /registry/pspreg.h:108
		// SDK declaration: int sceRegRemoveCategory(REGHANDLE h, const char *name);
		int sceRegRemoveCategory( int h, int name ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC5768D02, "sceRegGetKeyInfoByName" )]
		// SDK location: /registry/pspreg.h:151
		// SDK declaration: int sceRegGetKeyInfoByName(REGHANDLE hd, const char *name, unsigned int *type, SceSize *size);
		int sceRegGetKeyInfoByName( int hd, int name, int type, int size ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x30BE0259, "sceRegGetKeyValueByName" )]
		// SDK location: /registry/pspreg.h:175
		// SDK declaration: int sceRegGetKeyValueByName(REGHANDLE hd, const char *name, void *buf, SceSize size);
		int sceRegGetKeyValueByName( int hd, int name, int buf, int size ){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - F5B7AB2E */
