using System;
using System.Collections.Generic;
using System.Text;
using Noxa.Emulation.Psp.Cpu;
using System.Diagnostics;
using Noxa.Emulation.Psp.Video;

namespace Noxa.Emulation.Psp.Bios.GenericHle.Modules
{
	class GeUser : IModule
	{
		#region IModule Members

		protected HleInstance _hle;
		protected Kernel _kernel;
		protected IVideoDriver _video;

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

		public class GeCallbackData : KernelHandle
		{
			public int SignalFunction;
			public int SignalArgument;
			public int FinishFunction;
			public int FinishArgument;
		}

		protected List<GeCallbackData> _callbacks = new List<GeCallbackData>();

		// Display lists are crazy and support stalling
		// From what I can guess, it allows the GE to read memory 0..N while the CPU is
		// still writing N+1...M
		// Since we are awesome and don't do that, we need to keep a local cache as we
		// construct things and make sure we stall at the right places
		protected List<DisplayList> _outstandingLists = new List<DisplayList>( 20 );

		protected class DisplayListData
		{
			public List<VideoPacket> Packets;
			public int StallAddress;
		}

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

		protected void ReadPackets( IMemory memory, int pointer, List<VideoPacket> list, int stallAddress )
		{
			if( stallAddress == 0x0 )
				stallAddress = int.MaxValue;

			while( pointer < stallAddress )
			{
				int code = memory.ReadWord( pointer );
				VideoPacket packet = new VideoPacket( code );
				list.Add( packet );

				pointer += 4;

				// Stop conditions totally guessed from sceGuFinish
				if( ( packet.Command == VideoCommand.END ) ||
					( packet.Command == VideoCommand.FINISH ) ||
					( packet.Command == VideoCommand.Unknown0x11 ) )
					break;
			}
		}

		[BiosStub( 0xab49e76a, "sceGeListEnQueue", true, 4 )]
		public int sceGeListEnQueue( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = const void *list
			// a1 = void *stall
			// a2 = int cbid
			// a3 = void *arg

			if( _video == null )
				_video = _hle.Emulator.Video;

			DisplayList list = new DisplayList();
			list.CallbackId = a2;
			list.Argument = a3;

			if( a1 == 0x0 )
			{
				list.Ready = true;

				// Read all now
				List<VideoPacket> packets = new List<VideoPacket>( 256 );
				ReadPackets( memory, a0, packets, 0 );
				list.Packets = packets.ToArray();
			}
			else
			{
				DisplayListData dld = new DisplayListData();
				dld.StallAddress = a1;
				dld.Packets = new List<VideoPacket>( 256 );
				list.KernelData = dld;
				list.Ready = false;

				// Rest will follow
				ReadPackets( memory, a0, dld.Packets, a1 );

				_outstandingLists.Add( list );
			}

			bool success = _video.Enqueue( list, false );
			
			// int - ID of the queue
			if( success == true )
				return list.ID;
			else
				return -1;
		}

		[BiosStub( 0x1c0d95a6, "sceGeListEnQueueHead", true, 4 )]
		public int sceGeListEnQueueHead( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = const void *list
			// a1 = void *stall
			// a2 = int cbid
			// a3 = void *arg

			if( _video == null )
				_video = _hle.Emulator.Video;

			DisplayList list = new DisplayList();
			list.CallbackId = a2;
			list.Argument = a3;

			if( a1 == 0x0 )
			{
				list.Ready = true;

				// Read all now
				List<VideoPacket> packets = new List<VideoPacket>( 256 );
				ReadPackets( memory, a0, packets, 0 );
				list.Packets = packets.ToArray();
			}
			else
			{
				DisplayListData dld = new DisplayListData();
				dld.StallAddress = a1;
				dld.Packets = new List<VideoPacket>( 256 );
				list.KernelData = dld;
				list.Ready = false;

				// Rest will follow
				ReadPackets( memory, a0, dld.Packets, a1 );

				_outstandingLists.Add( list );
			}

			bool success = _video.Enqueue( list, false );

			// int - ID of the queue
			if( success == true )
				return list.ID;
			else
				return -1;
		}

		[BiosStub( 0x5fb86ab0, "sceGeListDeQueue", true, 1 )]
		public int sceGeListDeQueue( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = int qid

			if( _video == null )
				_video = _hle.Emulator.Video;

			_video.Abort( a0 );
			
			// int
			return 0;
		}

		[BiosStub( 0xe0d68148, "sceGeListUpdateStallAddr", true, 2 )]
		public int sceGeListUpdateStallAddr( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = int qid
			// a1 = void *stall

			if( _video == null )
				_video = _hle.Emulator.Video;

			DisplayList list = _video.FindDisplayList( a0 );
			Debug.Assert( list != null );
			if( list == null )
				return -1;

			if( list.Ready == false )
			{
				DisplayListData dld = list.KernelData as DisplayListData;
				int oldStallAddress = dld.StallAddress;
				dld.StallAddress = a1;
				ReadPackets( memory, oldStallAddress, dld.Packets, dld.StallAddress );
			}

			// int
			return 0;
		}

		[BiosStub( 0x03444eb4, "sceGeListSync", true, 2 )]
		public int sceGeListSync( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = int qid
			// a1 = int syncType

			if( _video == null )
				_video = _hle.Emulator.Video;

			DisplayList list = _video.FindDisplayList( a0 );
			Debug.Assert( list != null );
			if( list == null )
				return -1;

			if( list.Ready == false )
			{
				DisplayListData dld = list.KernelData as DisplayListData;
				int oldStallAddress = dld.StallAddress;
				ReadPackets( memory, oldStallAddress, dld.Packets, 0x0 );
				lock( list )
				{
					list.Packets = dld.Packets.ToArray();
					list.Ready = true;
				}

				_outstandingLists.Remove( list );
				list.KernelData = null;

				_video.Sync( list );
			}
			
			// int
			return 0;
		}

		[BiosStub( 0xb287bd61, "sceGeDrawSync", true, 1 )]
		public int sceGeDrawSync( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = int syncType

			if( _video == null )
				_video = _hle.Emulator.Video;

			// Don't know how to handle the others - check sceGuSync.c in sdk
			Debug.Assert( a0 == 0 );

			_video.Sync();

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
		public int sceGeSetCallback( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = PspGeCallbackData *cb

			GeCallbackData cb = new GeCallbackData();
			cb.HandleType = KernelHandleType.GuCallback;
			cb.Uid = _kernel.AllocateUid();
			cb.SignalFunction = memory.ReadWord( a0 );
			cb.SignalArgument = memory.ReadWord( a0 + 4 );
			cb.FinishFunction = memory.ReadWord( a0 + 8 );
			cb.FinishArgument = memory.ReadWord( a0 + 16 );
			_kernel.AddHandle( cb );

			_callbacks.Add( cb );
			
			// int - the callback ID
			return cb.Uid;
		}

		[BiosStub( 0x05db22ce, "sceGeUnsetCallback", true, 1 )]
		public int sceGeUnsetCallback( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			// a0 = int cbid

			GeCallbackData cb = _kernel.FindHandle( a0 ) as GeCallbackData;
			if( cb == null )
				return -1;

			_callbacks.Remove( cb );
			
			// int
			return 0;
		}

		[BiosStub( 0xe66cb92e, "GeUser_0xE66CB92E", false, 0 )]
		[BiosStubIncomplete]
		public int Unknown1( IMemory memory, int a0, int a1, int a2, int a3, int sp )
		{
			return 0;
		}
	}
}
