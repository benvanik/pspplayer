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
	class sceGe_user : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceGe_user";
			}
		}

		#endregion

		#region State Management

		public sceGe_user( Kernel kernel )
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

		// Most of these functions are really not implemented, but are inlined by
		// the native interface and are never called here. I marked them as implemented
		// so that I wouldn't see the debug spew.

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB77905EA, "sceGeEdramSetAddrTranslation" )]
		// manual add
		int sceGeEdramSetAddrTranslation( int translation )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1F6752AD, "sceGeEdramGetSize" )]
		// SDK location: /ge/pspge.h:47
		// SDK declaration: unsigned int sceGeEdramGetSize();
		int sceGeEdramGetSize(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE47E40E4, "sceGeEdramGetAddr" )]
		// SDK location: /ge/pspge.h:54
		// SDK declaration: void * sceGeEdramGetAddr();
		int sceGeEdramGetAddr(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDC93CFEF, "sceGeGetCmd" )]
		// SDK location: /ge/pspge.h:63
		// SDK declaration: unsigned int sceGeGetCmd(int cmd);
		int sceGeGetCmd( int cmd ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x57C8945B, "sceGeGetMtx" )]
		// SDK location: /ge/pspge.h:93
		// SDK declaration: int sceGeGetMtx(int type, void *matrix);
		int sceGeGetMtx( int type, int matrix ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x438A385A, "sceGeSaveContext" )]
		// SDK location: /ge/pspge.h:102
		// SDK declaration: int sceGeSaveContext(PspGeContext *context);
		int sceGeSaveContext( int context ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0BF608FB, "sceGeRestoreContext" )]
		// SDK location: /ge/pspge.h:111
		// SDK declaration: int sceGeRestoreContext(const PspGeContext *context);
		int sceGeRestoreContext( int context ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xAB49E76A, "sceGeListEnQueue" )]
		// SDK location: /ge/pspge.h:124
		// SDK declaration: int sceGeListEnQueue(const void *list, void *stall, int cbid, void *arg);
		int sceGeListEnQueue( int list, int stall, int cbid, int arg ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1C0D95A6, "sceGeListEnQueueHead" )]
		// SDK location: /ge/pspge.h:137
		// SDK declaration: int sceGeListEnQueueHead(const void *list, void *stall, int cbid, void *arg);
		int sceGeListEnQueueHead( int list, int stall, int cbid, int arg ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5FB86AB0, "sceGeListDeQueue" )]
		// SDK location: /ge/pspge.h:146
		// SDK declaration: int sceGeListDeQueue(int qid);
		int sceGeListDeQueue( int qid ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE0D68148, "sceGeListUpdateStallAddr" )]
		// SDK location: /ge/pspge.h:156
		// SDK declaration: int sceGeListUpdateStallAddr(int qid, void *stall);
		int sceGeListUpdateStallAddr( int qid, int stall ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x03444EB4, "sceGeListSync" )]
		// SDK location: /ge/pspge.h:176
		// SDK declaration: int sceGeListSync(int qid, int syncType);
		int sceGeListSync( int qid, int syncType ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB287BD61, "sceGeDrawSync" )]
		// SDK location: /ge/pspge.h:185
		// SDK declaration: int sceGeDrawSync(int syncType);
		int sceGeDrawSync( int syncType ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA4FC06A4, "sceGeSetCallback" )]
		// SDK location: /ge/pspge.h:193
		// SDK declaration: int sceGeSetCallback(PspGeCallbackData *cb);
		int sceGeSetCallback( int cb ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x05DB22CE, "sceGeUnsetCallback" )]
		// SDK location: /ge/pspge.h:201
		// SDK declaration: int sceGeUnsetCallback(int cbid);
		int sceGeUnsetCallback( int cbid ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB448EC0D, "sceGeBreak" )]
		// manual add
		int sceGeBreak( int mode, int unknown )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4C06E472, "sceGeContinue" )]
		// manual add
		int sceGeContinue()
		{
			return Module.NotImplementedReturn;
		}

	}
}

/* GenerateStubsV2: auto-generated - 4D5DD3D5 */
