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

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBD2F1094, "sceKernelLoadExec" )]
		// SDK location: /user/psploadexec.h:80
		// SDK declaration: int sceKernelLoadExec(const char *file, struct SceKernelLoadExecParam *param);
		public int sceKernelLoadExec( int file, int param ){ return Module.NotImplementedReturn; }

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
			Debug.WriteLine( string.Format( "sceKernelExitGameWithStatus: exiting with status code {0}", status ) );

			// Call exit callbacks
			FastLinkedList<KCallback> cbll = _kernel.Callbacks[ Kernel.CallbackTypes.Exit ];
			if( cbll.Count > 0 )
			{
				LinkedListEntry<KCallback> cbs = cbll.HeadEntry;
				while( cbs != null )
				{
					_kernel.IssueCallback( cbs.Value, ( uint )status );
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
				Debug.WriteLine( string.Format( "sceKernelRegisterExitCallback: callback ID {0} not found", cbid ) );
				return -1;
			}

			_kernel.Callbacks[ Kernel.CallbackTypes.Exit ].Enqueue( cb );

			return 0;
		}

	}
}

/* GenerateStubsV2: auto-generated - 7EE3B66B */
