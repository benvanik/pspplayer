using System;
using System.Collections.Generic;
using System.Text;
using Noxa.Emulation.Psp.Cpu;
using System.Diagnostics;

namespace Noxa.Emulation.Psp.Bios.GenericHle.Modules
{
	class GeUser : IModule
	{
		#region IModule Members

		protected HleInstance _hle;
		protected Kernel _kernel;

		public GeUser( HleInstance hle )
		{
			Debug.Assert( hle != null );

			_hle = hle;
			_kernel = hle.Kernel as Kernel;
		}

		public string Name
		{
			get
			{
				return "sceGe_user";
			}
		}

		#endregion

		[BiosStub( 0x1f6752ad, "sceGeEdramGetSize", true, 0 )]
		public int sceGeEdramGetSize( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// unsigned int
			return 0x001fffff;
		}

		[BiosStub( 0xe47e40e4, "sceGeEdramGetAddr", true, 0 )]
		public int sceGeEdramGetAddr( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// void *
			return 0x04000000;
		}

		[BiosStub( 0xb77905ea, "sceGeEdramSetAddrTranslation", false, 0 )]
		[BiosStubIncomplete]
		public int sceGeEdramSetAddrTranslation( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xdc93cfef, "sceGeGetCmd", true, 1 )]
		[BiosStubIncomplete]
		public int sceGeGetCmd( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = int cmd
			
			// unsigned int
			return 0;
		}

		[BiosStub( 0x57c8945b, "sceGeGetMtx", true, 2 )]
		[BiosStubIncomplete]
		public int sceGeGetMtx( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = int type
			// a1 = void *matrix
			
			// int
			return 0;
		}

		[BiosStub( 0x438a385a, "sceGeSaveContext", false, 0 )]
		[BiosStubIncomplete]
		public int sceGeSaveContext( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x0bf608fb, "sceGeRestoreContext", true, 1 )]
		[BiosStubIncomplete]
		public int sceGeRestoreContext( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = const PspGeContext *context
			
			// int
			return 0;
		}

		[BiosStub( 0xab49e76a, "sceGeListEnQueue", true, 4 )]
		[BiosStubIncomplete]
		public int sceGeListEnQueue( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = const void *list
			// a1 = void *stall
			// a2 = int cbid
			// a3 = void *arg
			
			// int
			return 0;
		}

		[BiosStub( 0x1c0d95a6, "sceGeListEnQueueHead", true, 4 )]
		[BiosStubIncomplete]
		public int sceGeListEnQueueHead( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = const void *list
			// a1 = void *stall
			// a2 = int cbid
			// a3 = void *arg
			
			// int
			return 0;
		}

		[BiosStub( 0x5fb86ab0, "sceGeListDeQueue", true, 1 )]
		[BiosStubIncomplete]
		public int sceGeListDeQueue( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = int qid
			
			// int
			return 0;
		}

		[BiosStub( 0xe0d68148, "sceGeListUpdateStallAddr", true, 2 )]
		[BiosStubIncomplete]
		public int sceGeListUpdateStallAddr( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = int qid
			// a1 = void *stall
			
			// int
			return 0;
		}

		[BiosStub( 0x03444eb4, "sceGeListSync", true, 2 )]
		[BiosStubIncomplete]
		public int sceGeListSync( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = int qid
			// a1 = int syncType
			
			// int
			return 0;
		}

		[BiosStub( 0xb287bd61, "sceGeDrawSync", true, 1 )]
		[BiosStubIncomplete]
		public int sceGeDrawSync( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = int syncType
			
			// int
			return 0;
		}

		[BiosStub( 0xb448ec0d, "sceGeBreak", false, 0 )]
		[BiosStubIncomplete]
		public int sceGeBreak( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0x4c06e472, "sceGeContinue", false, 0 )]
		[BiosStubIncomplete]
		public int sceGeContinue( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}

		[BiosStub( 0xa4fc06a4, "sceGeSetCallback", true, 1 )]
		[BiosStubIncomplete]
		public int sceGeSetCallback( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = PspGeCallbackData *cb
			
			// int
			return 0;
		}

		[BiosStub( 0x05db22ce, "sceGeUnsetCallback", true, 1 )]
		[BiosStubIncomplete]
		public int sceGeUnsetCallback( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = int cbid
			
			// int
			return 0;
		}

		[BiosStub( 0xe66cb92e, "", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown1( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}
	}
}
