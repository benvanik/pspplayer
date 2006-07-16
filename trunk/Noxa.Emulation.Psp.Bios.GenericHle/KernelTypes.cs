using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Noxa.Emulation.Psp.IO.Media;

namespace Noxa.Emulation.Psp.Bios.GenericHle
{
	class KernelStatus
	{
		public int Status;
		public KernelSysClock IdleClocks;
		public uint LeaveIdleCount;
		public uint ThreadSwitchCount;
		public uint VfpuSwitchCount;
	}

	enum KernelHandleType
	{
		Thread,
		Stdio,
		File,
		MemoryBlock,
		Callback,
		Semaphore,
		GuCallback,
		Event
	}

	class KernelHandle
	{
		public KernelHandleType HandleType;
		public int Uid;
	}

	#region Timing

	class KernelSysClock
	{
		public uint Low;
		public uint Hi;
	}

	#endregion

	#region Threading

	enum KernelThreadState
	{
		Running = 1,
		Ready = 2,
		Waiting = 4,
		Suspended = 8,
		Stopped = 16,
		Killed = 32
	}

	[Flags]
	enum KernelThreadAttributes : uint
	{
		/// <summary>
		/// Enable VFPU access for the thread.
		/// </summary>
		Vfpu = 0x00004000,
		/// <summary>
		/// Start the thread in user mode (done automatically if the thread creating it is in user mode).
		/// </summary>
		User = 0x80000000,
		/// <summary>
		/// Thread is part of the USB/WLAN API.
		/// </summary>
		UsbWlan = 0xa0000000,
		/// <summary>
		/// Thread is part of the VSH API.
		/// </summary>
		Vsh = 0xc0000000,
		/// <summary>
		/// Allow using scratchpad memory for a thread, NOT USABLE ON V1.0.
		/// </summary>
		ScratchSram = 0x00008000,
		/// <summary>
		/// Disables filling the stack with 0xFF on creation.
		/// </summary>
		NoFillStack = 0x00100000,
		/// <summary>
		/// Clear the stack when the thread is deleted.
		/// </summary>
		ClearStack = 0x00200000,
	}

	class KernelThreadContext
	{
		public object CoreState;
		public int ProgramCounter;
	}

	enum KernelThreadWait
	{
		ThreadEnd,
		Delay,
		Sleep,
		Event,
	}

	class KernelThread : KernelHandle
	{
		public string Name;
		public int EntryAddress;
		public int Priority;				// Lower = better
		public int InitialPriority;
		public uint StackSize;
		public KernelThreadAttributes Attributes;
		public int ArgumentsLength;
		public int ArgumentsPointer;
		public KernelThreadState State;
		public int ExitCode;
		public KernelMemoryBlock StackBlock;
		public KernelSysClock RunClocks;
		public uint WakeupCount;
		public uint ReleaseCount;
		public uint InterruptPreemptionCount;
		public uint ThreadPreemptionCount;
		public bool CanHandleCallbacks;
		public List<KernelCallback> Callbacks;
		
		public KernelThreadWait WaitType;
		public int WaitId;
		public int WaitTimeout;
		public KernelEvent WaitEvent;

		// Helps with wait states that may need output
		public int OutAddress;

		// Thread options?

		public KernelThreadContext Context;

		public KernelThread( int uid, string name, int entryAddress, int priority, uint stackSize, int attributes )
		{
			Uid = uid;
			Name = name;
			EntryAddress = entryAddress;
			Priority = priority;
			InitialPriority = priority;
			StackSize = stackSize;
			Attributes = ( KernelThreadAttributes )attributes;
			
			RunClocks = new KernelSysClock();

			Callbacks = new List<KernelCallback>();

			State = KernelThreadState.Stopped;
			Context = null;
		}

		public void Start( KernelPartition partition, int argumentsLength, int argumentsPointer )
		{
			StackBlock = partition.Allocate( KernelAllocationType.High, 0, StackSize );

			State = KernelThreadState.Running;
			ArgumentsLength = argumentsLength;
			ArgumentsPointer = argumentsPointer;
		}

		public void Exit( int code )
		{
			State = KernelThreadState.Killed;
			ExitCode = code;
			StackBlock.Partition.Free( StackBlock );
		}

		public void Wait( KernelEvent ev, int bitMask, int outAddress )
		{
			State = KernelThreadState.Waiting;
			WaitType = KernelThreadWait.Event;
			WaitEvent = ev;
			WaitId = bitMask;
			OutAddress = outAddress;
		}
	}

	#endregion

	#region Synchronization

	class KernelSemaphore : KernelHandle
	{
	}

	#endregion

	#region Files/block devices

	abstract class KernelDevice
	{
		public string Name;
		public string[] Paths;
		public bool IsSeekable;
		public bool ReadOnly;

		public KernelDevice( string name, string[] paths, bool isSeekable, bool readOnly )
		{
			Name = name;
			Paths = paths;
			IsSeekable = isSeekable;
			ReadOnly = readOnly;
		}
	}

	class KernelBlockDevice : KernelDevice
	{
		public const int BlockSizeVariable = 0;

		public int BlockSize;

		public KernelBlockDevice( string name, string[] paths, bool isSeekable, bool readOnly, int blockSize )
			: base( name, paths, isSeekable, readOnly )
		{
			BlockSize = blockSize;
		}
	}

	class KernelFileDevice : KernelDevice
	{
		public IMediaDevice MediaDevice;
		public IMediaFolder MediaRoot;

		public KernelFileDevice( string name, string[] paths, bool isSeekable, bool readOnly, IMediaDevice mediaDevice, IMediaFolder mediaRoot )
			: base( name, paths, isSeekable, readOnly )
		{
			MediaDevice = mediaDevice;
			MediaRoot = mediaRoot;
		}
	}

	class KernelFileHandle : KernelHandle
	{
		public KernelFileDevice Device;
		public IMediaItem MediaItem;
		public int FolderOffset;
		public Stream Stream;
		public bool IsOpen;
		public bool CanWrite;
		public bool CanSeek;
		public KernelCallback Callback;
	}

	#endregion

	#region Memory

	class KernelMemoryBlock : KernelHandle
	{
		public string Name;
		public KernelPartition Partition;
		public uint Size;
		public uint Address;
		public bool IsFree;
	}

	enum KernelAllocationType
	{
		Low,
		High,
		SpecificAddress
	}

	class KernelPartition
	{
		public Kernel Kernel;
		public int Id;
		public uint BaseAddress;
		public uint Size;
		public uint Top;
		public uint Bottom;
		public uint FreeSize;

		public List<KernelMemoryBlock> Blocks;
		public List<KernelMemoryBlock> FreeList;

		public KernelPartition( Kernel kernel, int id, uint baseAddress, uint size )
		{
			Kernel = kernel;
			Id = id;
			BaseAddress = baseAddress;
			Size = size;
			Top = baseAddress + ( uint )size;
			Bottom = baseAddress;
			FreeSize = Size;

			Blocks = new List<KernelMemoryBlock>( 1024 );
			FreeList = new List<KernelMemoryBlock>( 1024 );

			KernelMemoryBlock emptyBlock = new KernelMemoryBlock();
			emptyBlock.HandleType = KernelHandleType.MemoryBlock;
			emptyBlock.Uid = -1;
			emptyBlock.IsFree = true;
			emptyBlock.Address = baseAddress;
			emptyBlock.Size = size;
			emptyBlock.Partition = this;
			Blocks.Add( emptyBlock );

			FreeList.Add( emptyBlock );
		}

		public KernelMemoryBlock Allocate( KernelAllocationType type, uint address, uint size )
		{
			KernelMemoryBlock newBlock = null;

			if( type == KernelAllocationType.SpecificAddress )
			{
				for( int n = 0; n < Blocks.Count; n++ )
				{
					KernelMemoryBlock block = Blocks[ n ];

					if( block.IsFree == false )
						continue;
					if( ( address >= block.Address ) &&
						( address < block.Address + block.Size ) )
					{
						// Split
						newBlock = this.SplitBlock( block, address, size );
						if( block.Size <= 0 )
						{
							// Free space dead
							Blocks.Remove( block );
							FreeList.Remove( block );
						}
						break;
					}
				}
			}
			else
			{
				if( type == KernelAllocationType.Low )
				{
					for( int n = 0; n < Blocks.Count; n++ )
					{
						KernelMemoryBlock block = Blocks[ n ];

						if( block.IsFree == false )
							continue;
						newBlock = this.SplitBlock( block, block.Address, size );
						if( block.Size <= 0 )
						{
							// Free space dead
							Blocks.Remove( block );
							FreeList.Remove( block );
						}
						break;
					}
				}
				else
				{
					for( int n = Blocks.Count - 1; n >= 0; n-- )
					{
						KernelMemoryBlock block = Blocks[ n ];

						if( block.IsFree == false )
							continue;
						newBlock = this.SplitBlock( block, ( uint )( ( block.Address + block.Size ) - size ), size );
						if( block.Size <= 0 )
						{
							// Free space dead
							Blocks.Remove( block );
							FreeList.Remove( block );
						}
						break;
					}
				}
			}

			FreeSize -= size;
			return newBlock;
		}

		private KernelMemoryBlock SplitBlock( KernelMemoryBlock block, uint address, uint size )
		{
			KernelMemoryBlock newBlock = new KernelMemoryBlock();
			newBlock.Uid = block.Partition.Kernel.AllocateUid();
			newBlock.IsFree = false;
			newBlock.HandleType = KernelHandleType.MemoryBlock;
			newBlock.Partition = block.Partition;
			newBlock.Address = address;
			newBlock.Size = size;
			
			if( address == block.Address )
			{
				// Bottom up - put right before free and shift free up
				block.Address = address + size;
				block.Size -= size;

				Blocks.Insert( Blocks.IndexOf( block ), newBlock );
			}
			else if( address == ( block.Address + block.Size ) - size )
			{
				// Top down - put right after free and shift free down
				block.Size -= size;

				Blocks.Insert( Blocks.IndexOf( block ) + 1, newBlock );
			}
			else
			{
				// Middle - need to split and add new + subfree
				uint originalSize = block.Size;
				block.Size = newBlock.Address - block.Address;

				KernelMemoryBlock freeBlock = new KernelMemoryBlock();
				freeBlock.HandleType = KernelHandleType.MemoryBlock;
				freeBlock.Uid = -1;
				freeBlock.IsFree = true;
				freeBlock.Partition = block.Partition;
				freeBlock.Address = newBlock.Address + newBlock.Size;
				freeBlock.Size = originalSize - block.Size - newBlock.Size;

				int index = Blocks.IndexOf( block );
				Blocks.Insert( index + 1, newBlock );

				if( freeBlock.Size > 0 )
				{
					Blocks.Insert( index + 2, newBlock );
					FreeList.Add( freeBlock );

					FreeList.Sort( new Comparison<KernelMemoryBlock>( this.BlockCompare ) );
				}
			}

			return newBlock;
		}

		public void Free( KernelMemoryBlock block )
		{
			int index = Blocks.IndexOf( block );
			block.IsFree = true;
			block.Uid = -1;

			FreeSize += block.Size;
			FreeList.Add( block );

			// Coalesce
			if( index > 0 )
			{
				KernelMemoryBlock target = Blocks[ index - 1 ];
				if( target.IsFree == true )
				{
					Blocks.RemoveAt( index );
					FreeList.Remove( block );
					index = index - 1;

					target.Size += block.Size;
				}
			}
			if( index < Blocks.Count - 1 )
			{
				KernelMemoryBlock target = Blocks[ index + 1 ];
				if( target.IsFree == true )
				{
					Blocks.RemoveAt( index + 1 );
					FreeList.Remove( target );

					Blocks[ index ].Size += target.Size;
				}
			}

			FreeList.Sort( new Comparison<KernelMemoryBlock>( this.BlockCompare ) );
		}

		private int BlockCompare( KernelMemoryBlock a, KernelMemoryBlock b )
		{
			if( a.Address < b.Address )
				return -1;
			else if( a.Address == b.Address )
				return 0;
			else
				return 1;
		}
	}

	#endregion

	#region Callbacks

	enum KernelCallbackName
	{
		AsyncIo,
		Exit
	}

	//typedef int (*SceKernelCallbackFunction)(int count, int arg, void *common);
	class KernelCallback : KernelHandle
	{
		public string Name;
		public KernelThread Thread;
		public int FunctionAddress;
		public int CommonAddress;
		public int NotifyCount;
		public int NotifyArguments;
	}

	#endregion

	#region Events

	class KernelEvent : KernelHandle
	{
		public string Name;
		public int BitMask;
	}

	#endregion
}
