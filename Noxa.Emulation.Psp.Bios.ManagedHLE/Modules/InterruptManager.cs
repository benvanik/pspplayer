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
	class InterruptManager : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "InterruptManager";
			}
		}

		#endregion

		#region State Management

		public InterruptManager( Kernel kernel )
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

		public enum PspInterrupts
		{
			PSP_GPIO_INT = 4,
			PSP_ATA_INT = 5,
			PSP_UMD_INT = 6,
			PSP_MSCM0_INT = 7,
			PSP_WLAN_INT = 8,
			PSP_AUDIO_INT = 10,
			PSP_I2C_INT = 12,
			PSP_SIRCS_INT = 14,
			PSP_SYSTIMER0_INT = 15,
			PSP_SYSTIMER1_INT = 16,
			PSP_SYSTIMER2_INT = 17,
			PSP_SYSTIMER3_INT = 18,
			PSP_THREAD0_INT = 19,
			PSP_NAND_INT = 20,
			PSP_DMACPLUS_INT = 21,
			PSP_DMA0_INT = 22,
			PSP_DMA1_INT = 23,
			PSP_MEMLMD_INT = 24,
			PSP_GE_INT = 25,
			PSP_VBLANK_INT = 30,
			PSP_MECODEC_INT = 31,
			PSP_HPREMOTE_INT = 36,
			PSP_MSCM1_INT = 60,
			PSP_MSCM2_INT = 61,
			PSP_THREAD1_INT = 65,
			PSP_INTERRUPT_INT = 66,
		}

		/*enum PspSubInterrupts
		{
			PSP_GPIO_SUBINT = PSP_GPIO_INT,
			PSP_ATA_SUBINT = PSP_ATA_INT,
			PSP_UMD_SUBINT = PSP_UMD_INT,
			PSP_DMACPLUS_SUBINT = PSP_DMACPLUS_INT,
			PSP_GE_SUBINT = PSP_GE_INT,
			PSP_DISPLAY_SUBINT = PSP_VBLANK_INT,
		}*/

		[Stateless]
		[BiosFunction( 0xCA04A2B9, "sceKernelRegisterSubIntrHandler" )]
		// SDK location: /user/pspintrman.h:119
		// SDK declaration: int sceKernelRegisterSubIntrHandler(int intno, int no, void *handler, void *arg);
		public int sceKernelRegisterSubIntrHandler( int intno, int no, int address, int arg )
		{
			KIntHandler handler = new KIntHandler( _kernel, intno, no, ( uint )address, ( uint )arg );

			Debug.Assert( _kernel.Interrupts[ intno ][ no ] == null );
			_kernel.Interrupts[ intno ][ no ] = handler;

			Log.WriteLine( Verbosity.Verbose, Feature.Bios, "sceKernelRegisterSubIntrHandler: registered handler for interrupt {0} (slot {1}), calling code at {2:X8}", intno, no, address );

			// Handlers are not enabled by default

			return 0;
		}

		[Stateless]
		[BiosFunction( 0xD61E6961, "sceKernelReleaseSubIntrHandler" )]
		// SDK location: /user/pspintrman.h:129
		// SDK declaration: int sceKernelReleaseSubIntrHandler(int intno, int no);
		public int sceKernelReleaseSubIntrHandler( int intno, int no )
		{
			KIntHandler handler = _kernel.Interrupts[ intno ][ no ];
			Debug.Assert( handler != null );
			if( handler == null )
				return -1;

			handler.Enabled = false;

			_kernel.Interrupts[ intno ][ no ] = null;

			return 0;
		}

		[Stateless]
		[BiosFunction( 0xFB8E22EC, "sceKernelEnableSubIntr" )]
		// SDK location: /user/pspintrman.h:139
		// SDK declaration: int sceKernelEnableSubIntr(int intno, int no);
		public int sceKernelEnableSubIntr( int intno, int no )
		{
			KIntHandler handler = _kernel.Interrupts[ intno ][ no ];
			Debug.Assert( handler != null );
			if( handler == null )
				return -1;

			handler.Enabled = true;

			return 0;
		}

		[Stateless]
		[BiosFunction( 0x8A389411, "sceKernelDisableSubIntr" )]
		// SDK location: /user/pspintrman.h:149
		// SDK declaration: int sceKernelDisableSubIntr(int intno, int no);
		public int sceKernelDisableSubIntr( int intno, int no )
		{
			KIntHandler handler = _kernel.Interrupts[ intno ][ no ];
			Debug.Assert( handler != null );
			if( handler == null )
				return -1;

			handler.Enabled = false;

			return 0;
		}

		[Stateless]
		[BiosFunction( 0xD2E8363F, "QueryIntrHandlerInfo" )]
		// SDK location: /user/pspintrman.h:170
		// SDK declaration: int QueryIntrHandlerInfo(SceUID intr_code, SceUID sub_intr_code, PspIntrHandlerOptionParam *data);
		public int QueryIntrHandlerInfo( int intr_code, int sub_intr_code, int data )
		{
			KIntHandler handler = _kernel.Interrupts[ intr_code ][ sub_intr_code ];
			Debug.Assert( handler != null );
			if( handler == null )
				return -1;

			unsafe
			{
				byte* p = _memorySystem.Translate( ( uint )data );

				*( ( uint* )p ) = 0x38; // size
				*( ( uint* )( p + 4 ) ) = handler.Address;
				*( ( uint* )( p + 8 ) ) = handler.Argument; // common
				*( ( uint* )( p + 0xC ) ) = 0; // gp?
				*( ( ushort* )( p + 0x10 ) ) = ( ushort )handler.InterruptNumber;
				*( ( ushort* )( p + 0x12 ) ) = ( ushort )handler.Slot;
				*( ( ushort* )( p + 0x14 ) ) = 0;
				*( ( ushort* )( p + 0x16 ) ) = handler.Enabled ? ( ushort )1 : ( ushort )0;
				*( ( uint* )( p + 0x18 ) ) = handler.CallCount;
				*( ( uint* )( p + 0x1C ) ) = 0; // ?
				*( ( uint* )( p + 0x20 ) ) = 0; // total clock lo
				*( ( uint* )( p + 0x24 ) ) = 0; // total clock hi
				*( ( uint* )( p + 0x28 ) ) = 0; // min clock lo
				*( ( uint* )( p + 0x2C ) ) = 0; // min clock hi
				*( ( uint* )( p + 0x30 ) ) = 0; // max clock lo
				*( ( uint* )( p + 0x34 ) ) = 0; // max clock hi
			}

			Log.WriteLine( Verbosity.Verbose, Feature.Bios, "QueryIntrHandlerInfo: called, but a lot of it isn't implemented - make sure nothing is used!" );

			return 0;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFC4374B8, "sceKernelIsSubInterruptOccurred" )]
		public int sceKernelIsSubInterruptOccurred()
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xEEE43F47, "sceKernelRegisterUserSpaceIntrStack" )]
		public int sceKernelRegisterUserSpaceIntrStack()
		{
			return Module.NotImplementedReturn;
		}
	}
}

/* GenerateStubsV2: auto-generated - 31D5B3BA */
