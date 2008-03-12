using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Noxa.Emulation.Psp.Bios.ManagedHLE
{
	class KMessagePipe : KHandle
	{
		public Kernel Kernel;

		public string Name;

		public Stream Stream;
		public KPartition Partition;

		public FastLinkedList<KThread> WaitingThreads;

		public KMessagePipe( Kernel kernel, KPartition partition, string name, int size )
		{
			Kernel = kernel;
			Partition = partition;
			Name = name;
			Stream = new System.IO.MemoryStream(size);
			WaitingThreads = new FastLinkedList<KThread>();
		}

		public void Signal()
		{
			// Try to wake threads
			bool wokeThreads = false;

			LinkedListEntry<KThread> entry = WaitingThreads.HeadEntry;
			while( entry != null )
			{
				int needed = ( int )entry.Value.WaitArgument;
				if( this.Stream.Length >= needed )
				{
					this.Stream.Seek( 0, SeekOrigin.Begin );
					this.Kernel.Memory.WriteStream( ( int )entry.Value.WaitAddress, this.Stream, ( int )entry.Value.WaitArgument );
					this.Stream.SetLength( 0 );

					if (entry.Value.WaitAddressResult != 0)
					{
						unsafe
						{
							uint* poutSize = (uint*)Kernel.MemorySystem.Translate(entry.Value.WaitAddressResult);
							*poutSize = entry.Value.WaitArgument;
						}
					}

					entry.Value.Wake( 0 );
					wokeThreads = true;
				}
				else
					break;
			}

			if( wokeThreads == true )
				Kernel.Schedule();
		}

		public int MaybeWait( int message, int size, int unk1, int outSize, int timeout, bool allowCallbacks )
		{
			// Buffer has enough data, return instantly.
			if( size <= this.Stream.Length )
			{
				Log.WriteLine( Verbosity.Normal, Feature.Bios, "MaybeWait -> {0} ? {1}", size, this.Stream.Length );
				this.Stream.Seek( 0, SeekOrigin.Begin );
				this.Kernel.Memory.WriteStream( message, this.Stream, size );
				this.Stream.SetLength( 0 );

				if (outSize != 0)
				{
					unsafe
					{
						uint* poutSize = (uint * )Kernel.MemorySystem.Translate((uint)outSize);
						*poutSize = (uint)size;
					}
				}

				return 0;
			}

			Log.WriteLine( Verbosity.Normal, Feature.Bios, "MaybeWaitSleep -> {0} ? {1}", size, this.Stream.Length );
			this.Wait( message, size, unk1, outSize, timeout, allowCallbacks );
			return 0;

		}

		public void Wait( int message, int size, int unk1, int outSize, int timeout, bool allowCallbacks )
		{
			this.Kernel.ActiveThread.Wait( this, message, size, outSize, timeout, allowCallbacks );
		}
	}
}
