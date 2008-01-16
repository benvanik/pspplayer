// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2008 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace Noxa.Emulation.Psp.HookerPC
{
	delegate void CallEventHandler( uint threadId, uint nid, uint callingAddress, string[] parameterValues );
	delegate void ReturnEventHandler( uint threadId, uint nid, uint callingAddress, string returnValue );

	unsafe class HookerClient : IDisposable
	{
		private const int HostPort = 10004;
		private Thread _thread;
		private TcpClient _client;

		public event CallEventHandler CallHit;
		public event ReturnEventHandler ReturnHit;

		public void Connect()
		{
			this.Disconnect();
			_client = new TcpClient( new IPEndPoint( IPAddress.Loopback, HostPort ) );

			_thread = new Thread( WorkerThread );
			_thread.Name = "Worker Thread";
			_thread.IsBackground = true;
			_thread.Start();
		}

		public void Disconnect()
		{
			this.Dispose();
		}

		public void Dispose()
		{
			if( _client != null )
			{
				_thread.Abort();
				_thread = null;
				if( _client.Connected == true )
					_client.Close();
				_client = null;
			}
		}

		private void WorkerThread()
		{
			try
			{
				NetworkStream stream = _client.GetStream();
				BinaryReader reader = new BinaryReader( stream );
				byte[] buffer = new byte[ 512 ];
				while( _client.Connected == true )
				{
					if( _client.Available > 0 )
					{
						if( _client.Client.Poll( 1000000, SelectMode.SelectRead ) == true )
						{
							while( _client.Available > 0 )
							{
								int mode = reader.ReadByte();
								switch( mode )
								{
									case 0:
										this.HandleCall( reader, mode, buffer );
										break;
									case 1:
										this.HandleReturn( reader, mode, buffer );
										break;
								}
							}
						}
					}
				}
			}
			catch( ThreadAbortException )
			{
				return;
			}
		}

		// 49b
		[StructLayout( LayoutKind.Sequential )]
		struct CallPacket
		{
			// 0?
			//public byte Mode;
			public uint ThreadID;
			public uint NID;
			public uint CallingAddress;
			// [4 bits per entry] param 6, param 5, param 4, param 3, param 2, param 1, param 0, param count
			public uint ParameterFormats;
			// If long (format 0x2 or 0x4), two values are used
			public fixed uint ParameterValues[ 8 ];
		}

		// 22b
		[StructLayout( LayoutKind.Sequential )]
		struct ReturnPacket
		{
			// 1?
			//public byte Mode;
			public uint ThreadID;
			public uint NID;
			public uint CallingAddress;
			public byte ReturnFormat;
			public uint ReturnLow;
			public uint ReturnHigh;
		}

		// 4 bits
		enum ParameterType
		{
			Int16 = 0x00,
			Int32 = 0x01,
			Int64 = 0x02,
			Int32Hex = 0x03,
			Int64Hex = 0x04,
			Int32Oct = 0x05,
			Single = 0x06,
			String = 0x07,
			Void = 0x08,
		}

		private void HandleCall( BinaryReader reader, int mode, byte[] buffer )
		{
			reader.Read( buffer, 0, sizeof( CallPacket ) );
			fixed( byte* ptr = &buffer[ 0 ] )
			{
				CallPacket* p = ( CallPacket* )ptr;

				// # of parameters (excluding return)
				uint parameterFormat = p->ParameterFormats;
				uint count = parameterFormat & 0xF;
				int valueIndex = 0;
				for( uint n = 0; n < count; n++ )
				{
					parameterFormat = parameterFormat >> 4;
					switch( ( ( ParameterType )( parameterFormat & 0xF ) ) )
					{
						case ParameterType.Void:
							break;
						case ParameterType.Int32:
						case ParameterType.Int32Hex:
						case ParameterType.Int32Oct:
							uint value32 = p->ParameterValues[ valueIndex ];
							valueIndex++;
							break;
						case ParameterType.Int64:
						case ParameterType.Int64Hex:
							ulong value64 = p->ParameterValues[ valueIndex ] | ( ( ( ulong )p->ParameterValues[ valueIndex + 1 ] ) << 32 );
							valueIndex += 2;
							break;
					}
				}
			}
		}

		private void HandleReturn( BinaryReader reader, int mode, byte[] buffer )
		{
			reader.Read( buffer, 0, sizeof( ReturnPacket ) );
			fixed( byte* ptr = &buffer[ 0 ] )
			{
				ReturnPacket* p = ( ReturnPacket* )ptr;

				ParameterType returnFormat = ( ParameterType )p->ReturnFormat;
				switch( returnFormat )
				{
					case ParameterType.Int64:
					case ParameterType.Int64Hex:
						ulong result64 = p->ReturnLow | ( ( ( ulong )p->ReturnHigh ) << 32 );
						break;
					case ParameterType.Void:
						break;
					default:
						uint result = p->ReturnLow;
						break;
				}
			}
		}
	}
}
