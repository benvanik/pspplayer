// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Noxa.Emulation.Psp;
using Noxa.Emulation.Psp.Bios;
using Noxa.Emulation.Psp.Cpu;
using Noxa.Utilities;

namespace Noxa.Emulation.Psp.Bios.ManagedHLE.Modules
{
	class LoadExecForUser : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "LoadExecForUser";
			}
		}

		#endregion

		#region State Management

		public LoadExecForUser( Kernel kernel )
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

		[StructLayout( LayoutKind.Sequential )]
		private struct SceKernelLoadExecParam
		{
			public uint Size;
			public uint ArgCount;
			public uint ArgPointer;
			public uint Key;
		}

		[BiosFunction( 0xBD2F1094, "sceKernelLoadExec" )]
		// SDK location: /user/psploadexec.h:80
		// SDK declaration: int sceKernelLoadExec(const char *file, struct SceKernelLoadExecParam *param);
		public unsafe int sceKernelLoadExec( int file, int param )
		{
			// Restart the emu loading the prx in file?
			string prxName = _kernel.ReadString( ( uint )file );
			SceKernelLoadExecParam* loadParam = ( SceKernelLoadExecParam* )_kernel.MemorySystem.Translate( ( uint )param );

			Log.WriteLine( Verbosity.Critical, Feature.Bios, "sceKernelLoadExec: attempt to reset psp to prx {0}; THIS IS NOT IMPLEMENTED", prxName );

			return Module.NotImplementedReturn;
		}

		[BiosFunction( 0x05572A5F, "sceKernelExitGame" )]
		// SDK location: /user/psploadexec.h:57
		// SDK declaration: void sceKernelExitGame();
		public void sceKernelExitGame()
		{
			this.sceKernelExitGameWithStatus( 0 );
		}

		[BiosFunction( 0x2AC9954B, "sceKernelExitGameWithStatus" )]
		// manual add
		public void sceKernelExitGameWithStatus( int status )
		{
			Log.WriteLine( Verbosity.Normal, Feature.Bios, "sceKernelExitGameWithStatus: exiting with status code {0}", status );

			// Call exit callbacks
			FastLinkedList<KCallback> cbll = _kernel.Callbacks[ Kernel.CallbackTypes.Exit ];
			if( cbll.Count > 0 )
			{
				LinkedListEntry<KCallback> cbs = cbll.HeadEntry;
				while( cbs != null )
				{
					_kernel.NotifyCallback( cbs.Value, ( uint )status );
					cbs = cbs.Next;
				}
			}
			else
			{
				// Do the callbacks kill the game for us?
				_kernel.StopGame( status );
			}
		}

		[Stateless]
		[BiosFunction( 0x4AC57943, "sceKernelRegisterExitCallback" )]
		// SDK location: /user/psploadexec.h:49
		// SDK declaration: int sceKernelRegisterExitCallback(int cbid);
		public int sceKernelRegisterExitCallback( int cbid )
		{
			KCallback cb = _kernel.GetHandle<KCallback>( cbid );
			if( cb == null )
			{
				Log.WriteLine( Verbosity.Normal, Feature.Bios, "sceKernelRegisterExitCallback: callback ID {0} not found", cbid );
				return -1;
			}

			_kernel.Callbacks[ Kernel.CallbackTypes.Exit ].Enqueue( cb );

			return 0;
		}
	}
}

/* GenerateStubsV2: auto-generated - 641B8CC7 */
