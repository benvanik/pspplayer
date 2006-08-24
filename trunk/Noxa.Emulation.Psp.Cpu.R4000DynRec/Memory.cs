// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace Noxa.Emulation.Psp.Cpu
{
	unsafe class Memory : IMemory
	{
		// This is my uber simple memory manager
		// The C++/CLI implementation is much more general, however it's also slower
		// It's better for a debugger, where as this should be better for speed purposes

		//_memory->DefineSegment( MemoryType::PhysicalMemory, "Main Memory", 0x08000000, 0x01FFFFFF );
		//_memory->DefineSegment( MemoryType::PhysicalMemory, "Hardware Vectors", 0x1FC00000, 0x000FFFFF );
		//_memory->DefineSegment( MemoryType::PhysicalMemory, "Scratchpad", 0x00010000, 0x00003FFF );
		//_memory->DefineSegment( MemoryType::PhysicalMemory, "Frame Buffer", 0x04000000, 0x001FFFFF );
		//_memory->DefineSegment( MemoryType::HardwareMapped, "Hardware IO 1", 0x1C000000, 0x03BFFFFF );
		//_memory->DefineSegment( MemoryType::HardwareMapped, "Hardware IO 2", 0x1FD00000, 0x002FFFFF );

		internal const int MainMemoryBaseAddress = 0x08000000;
		internal byte[] _mainMemory;
		private byte[] _scratchPad;
		private byte[] _frameBufferBytes;
		private IMemorySegment _frameBuffer;

		public Memory()
		{
			_mainMemory = new byte[ 0x01FFFFFF ];
			_scratchPad = new byte[ 0x00003FFF ];
			_frameBufferBytes = new byte[ 0x001FFFFF ];
		}

		public IMemorySegment DefineSegment( MemoryType type, string name, int baseAddress, int length )
		{
			throw new NotSupportedException();
		}

		public void RegisterSegment( IMemorySegment segment )
		{
			if( segment.BaseAddress == 0x04000000 )
			{
				_frameBufferBytes = null;
				_frameBuffer = segment;
			}
			else
			{
				Debug.Assert( false, "Cannot override other segments" );
			}
		}

		public IMemorySegment FindSegment( string name )
		{
			throw new NotSupportedException();
		}

		public IMemorySegment FindSegment( int baseAddress )
		{
			throw new NotSupportedException();
		}

		public int ReadWord( int address )
		{
			//Debug.WriteLine( string.Format( "RW @ 0x{0:X8}", address ) );
			if( ( address >= 0x08000000 ) && ( address < 0x9FFFFFF ) )
			{
				address -= 0x08000000;
				fixed( byte* basePointer = _mainMemory )
				{
					int* ptr = ( int* )( basePointer + address );
					return *ptr;
				}
			}
			else if( ( address >= 0x00010000 ) && ( address < 0x00003FFF ) )
			{
				address -= 0x00010000;
				fixed( byte* basePointer = _scratchPad )
				{
					int* ptr = ( int* )( basePointer + address );
					return *ptr;
				}
			}
			else if( ( address >= 0x04000000 ) && ( address < 0x001FFFFF ) )
			{
				if( _frameBuffer != null )
					return _frameBuffer.ReadWord( address );
				else
				{
					address -= 0x04000000;
					fixed( byte* basePointer = _frameBufferBytes )
					{
						int* ptr = ( int* )( basePointer + address );
						return *ptr;
					}
				}
			}
			else
			{
				Debugger.Break();
				return 0;
			}
		}

		public byte[] ReadBytes( int address, int count )
		{
			if( ( address >= 0x08000000 ) && ( address < 0x9FFFFFF ) )
			{
				address -= 0x08000000;
				byte[] buffer = new byte[ count ];
				Array.Copy( _mainMemory, address, buffer, 0, count );
				return buffer;
			}
			else if( ( address >= 0x00010000 ) && ( address < 0x00003FFF ) )
				throw new NotImplementedException();
			else if( ( address >= 0x04000000 ) && ( address < 0x001FFFFF ) )
				throw new NotImplementedException();
			else
				return null;
		}

		public int ReadStream( int address, System.IO.Stream destination, int count )
		{
			if( ( address >= 0x08000000 ) && ( address < 0x9FFFFFF ) )
			{
				address -= 0x08000000;
				//long pos = destination.Position;
				destination.Write( _mainMemory, address, count );
				//destination.Position = pos;
				return count;
			}
			else if( ( address >= 0x00010000 ) && ( address < 0x00003FFF ) )
				throw new NotImplementedException();
			else if( ( address >= 0x04000000 ) && ( address < 0x001FFFFF ) )
				throw new NotImplementedException();
			else
				return 0;
		}

		public void WriteWord( int address, int width, int value )
		{
			Debug.Assert( address > 0x20 );

			if( ( address >= 0x08000000 ) && ( address < 0x9FFFFFF ) )
			{
				address -= 0x08000000;
				fixed( byte *basePointer = _mainMemory )
				{
					byte *ptr = basePointer + address;
					if( width == 4 )
					{
						int* p = ( int* )ptr;
						*p = value;
					}
					else if( width == 1 )
					{
						*ptr = ( byte )value;
					}
					else if( width == 2 )
					{
						short* p = ( short* )ptr;
						*p = ( short )value;
					}
					else
						Debug.Assert( false, "Unsupported width" );
				}
			}
			else if( ( address >= 0x00010000 ) && ( address < 0x00013FFF ) )
			{
				address -= 0x00010000;
				fixed( byte* basePointer = _scratchPad )
				{
					byte* ptr = basePointer + address;
					if( width == 4 )
					{
						int* p = ( int* )ptr;
						*p = value;
					}
					else if( width == 1 )
					{
						*ptr = ( byte )value;
					}
					else if( width == 2 )
					{
						short* p = ( short* )ptr;
						*p = ( short )value;
					}
					else
						Debug.Assert( false, "Unsupported width" );
				}
			}
			else if( ( address >= 0x04000000 ) && ( address < 0x041FFFFF ) )
			{
				if( _frameBuffer != null )
					_frameBuffer.WriteWord( address, width, value );
				else
				{
					address -= 0x04000000;
					fixed( byte* basePointer = _frameBufferBytes )
					{
						byte* ptr = basePointer + address;
						if( width == 4 )
						{
							int* p = ( int* )ptr;
							*p = value;
						}
						else if( width == 1 )
						{
							*ptr = ( byte )value;
						}
						else if( width == 2 )
						{
							short* p = ( short* )ptr;
							*p = ( short )value;
						}
						else
							Debug.Assert( false, "Unsupported width" );
					}
				}
			}
			else
			{
				Debugger.Break();
			}

			//if( this->MemoryChanged != nullptr )
			//this->MemoryChanged( this, address, width, value );
		}

		public void WriteBytes( int address, byte[] bytes )
		{
			if( ( address >= 0x08000000 ) && ( address < 0x9FFFFFF ) )
			{
				address -= 0x08000000;
				Array.Copy( bytes, 0, _mainMemory, address, bytes.Length );
			}
			else if( ( address >= 0x00010000 ) && ( address < 0x00003FFF ) )
			{
				address -= 0x00010000;
				Array.Copy( bytes, 0, _scratchPad, address, bytes.Length );
			}
			else if( ( address >= 0x04000000 ) && ( address < 0x001FFFFF ) )
			{
				if( _frameBuffer != null )
					_frameBuffer.WriteBytes( address, bytes );
				else
				{
					address -= 0x04000000;
					Array.Copy( bytes, 0, _frameBufferBytes, address, bytes.Length );
				}
			}
			else
				Debugger.Break();
		}

		public void WriteBytes( int address, byte[] bytes, int index, int count )
		{
			throw new NotImplementedException();
		}

		public void WriteStream( int address, System.IO.Stream source, int count )
		{
			if( ( address >= 0x08000000 ) && ( address < 0x9FFFFFF ) )
			{
				address -= 0x08000000;
				//long pos = source.Position;
				source.Read( _mainMemory, address, count );
				//source.Position = pos;
			}
			else if( ( address >= 0x00010000 ) && ( address < 0x00003FFF ) )
				throw new NotImplementedException();
			else if( ( address >= 0x04000000 ) && ( address < 0x001FFFFF ) )
				throw new NotImplementedException();
			else
				Debugger.Break();
		}

		public void Load( System.IO.Stream stream )
		{
			throw new NotImplementedException();
		}

		public void Load( string fileName )
		{
			throw new NotImplementedException();
		}

		public void Save( System.IO.Stream stream )
		{
			throw new NotImplementedException();
		}

		public void Save( string fileName )
		{
			throw new NotImplementedException();
		}

		public uint GetMemoryHash( int address, int count, uint prime )
		{
			if( ( address >= 0x08000000 ) && ( address < 0x9FFFFFF ) )
			{
				address -= 0x08000000;

				uint hash;
				int n;
				for( hash = ( uint )count, n = 0; n < count; ++n )
					hash = hash + _mainMemory[ address + n ];
				return hash % prime;
			}
			else if( ( address >= 0x00010000 ) && ( address < 0x00003FFF ) )
				throw new NotImplementedException();
			else if( ( address >= 0x04000000 ) && ( address < 0x001FFFFF ) )
				throw new NotImplementedException();
			else
				throw new NotSupportedException();
		}

		public void DumpMainMemory( string fileName )
		{
			using( FileStream file = File.OpenWrite( fileName ) )
			using( BinaryWriter writer = new BinaryWriter( file ) )
			{
				writer.Write( _mainMemory );
			}
		}
	}
}
