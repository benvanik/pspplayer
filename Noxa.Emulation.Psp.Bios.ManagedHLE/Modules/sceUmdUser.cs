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
using Noxa.Emulation.Psp.Media;

namespace Noxa.Emulation.Psp.Bios.ManagedHLE.Modules
{
	class sceUmdUser : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceUmdUser";
			}
		}

		#endregion

		#region State Management

		public sceUmdUser( Kernel kernel )
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

		enum UmdStatus
		{
			Init = 0x00,
			MediaOut = 0x01,
			MediaIn = 0x02,
			MediaChange = 0x04,
			NotReady = 0x08,
			Ready = 0x10,
			Readable = 0x20,
		}

		enum UmdMode
		{
			PowerOn = 0x01,
			PowerCurrent = 0x02,
		}

		[DontTrace]
		[Stateless]
		[BiosFunction( 0x46EBB729, "sceUmdCheckMedium" )]
		// SDK location: /umd/pspumd.h:42
		// SDK declaration: int sceUmdCheckMedium();
		public int sceUmdCheckMedium()
		{
			IUmdDevice umd = _kernel.Emulator.Umd;
			if( umd == null )
				return 0;
			else
				return ( umd.State == MediaState.Present ) ? 1 : 0;
		}

		[Stateless]
		[BiosFunction( 0xC6183D47, "sceUmdActivate" )]
		// SDK location: /umd/pspumd.h:66
		// SDK declaration: int sceUmdActivate(int unit, const char *drive);
		public int sceUmdActivate( int unit, int drive )
		{
			Log.WriteLine( Verbosity.Verbose, Feature.Bios, "sceUmdActivate: activating unit {0} / drive {1}", unit, _kernel.ReadString( ( uint )drive ) );
			return 0;
		}

		[Stateless]
		[BiosFunction( 0xE83742BA, "sceUmdDeactivate" )]
		// manual add
		public int sceUmdDeactivate( int unit, int drive )
		{
			Log.WriteLine( Verbosity.Verbose, Feature.Bios, "sceUmdDeactivate: deactivating unit {0} / drive {1}", unit, _kernel.ReadString( ( uint )drive ) );
			return 0;
		}

		[Stateless]
		[BiosFunction( 0x6B4A146C, "sceUmdGetDriveStat" )]
		// manual add
		public int sceUmdGetDriveStat()
		{
			//return ( UmdInit | UmdMediaIn | UmdReady | UmdReadable );
			return ( int )( UmdStatus.MediaIn | UmdStatus.Readable );
		}

		[Stateless]
		[BiosFunction( 0x20628E6F, "sceUmdGetErrorStat" )]
		// manual add
		public int sceUmdGetErrorStat()
		{
			// Is this right?
			return 0;
		}

		[BiosFunction( 0x8EF08FCE, "sceUmdWaitDriveStat" )]
		// SDK location: /umd/pspumd.h:75
		// SDK declaration: int sceUmdWaitDriveStat(int stat);
		public int sceUmdWaitDriveStat( int stat )
		{
			// Just hope we never get here
			Log.WriteLine( Verbosity.Verbose, Feature.Bios, "sceUmdWaitDriveStat: waiting on status {0}", stat );
			return 0;
		}

		[BiosFunction( 0x56202973, "sceUmdWaitDriveStatWithTimer" )]
		// manual add - assuming timer = timeout
		public int sceUmdWaitDriveStatWithTimer( int stat, int timeout )
		{
			// Just hope we never get here
			Log.WriteLine( Verbosity.Verbose, Feature.Bios, "sceUmdWaitDriveStatWithTimer: waiting on status {0}", stat );
			return 0;
		}

		[BiosFunction( 0x4A9E5E29, "sceUmdWaitDriveStatCB" )]
		// manual add
		public int sceUmdWaitDriveStatCB( int stat )
		{
			// Just hope we never get here
			Log.WriteLine( Verbosity.Verbose, Feature.Bios, "sceUmdWaitDriveStatCB: waiting on status {0}", stat );
			return 0;
		}

		[Stateless]
		[BiosFunction( 0xAEE7404D, "sceUmdRegisterUMDCallBack" )]
		// SDK location: /umd/pspumd.h:89
		// SDK declaration: int sceUmdRegisterUMDCallBack(int cbid);
		public int sceUmdRegisterUMDCallBack( int cbid )
		{
			KCallback cb = _kernel.GetHandle<KCallback>( cbid );
			if( cb == null )
				return -1;

			_kernel.Callbacks[ Kernel.CallbackTypes.Umd ].Enqueue( cb );

			return 0;
		}

		[Stateless]
		[BiosFunction( 0xBD2BDE07, "sceUmdUnRegisterUMDCallBack" )]
		// manual add
		public int sceUmdUnRegisterUMDCallBack( int cbid )
		{
			KCallback cb = _kernel.GetHandle<KCallback>( cbid );
			if( cb == null )
				return -1;

			_kernel.Callbacks[ Kernel.CallbackTypes.Umd ].Remove( cb );

			return 0;
		}

		[Stateless]
		[BiosFunction( 0x340B7686, "sceUmdGetDiscInfo" )]
		// manual add
		public int sceUmdGetDiscInfo( int discInfo )
		{
			IUmdDevice umd = _kernel.Emulator.Umd;
			if( umd == null )
				return -1;

			unsafe
			{
				uint* pinfo = ( uint* )_memorySystem.Translate( ( uint )discInfo );
				*pinfo = ( uint )umd.Capacity;
				*( pinfo + 1 ) = ( uint )umd.DiscType;
			}

			return 0;
		}
	}
}

/* GenerateStubsV2: auto-generated - 5E619990 */
