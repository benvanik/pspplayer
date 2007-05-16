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
	unsafe class sceUtility : Module
	{
		#region Common

		private enum UtilityStatus
		{
			None = 0,
			Init = 1,
			Running = 2,
			Finished = 3,
			Closed = 4,
		}

		#endregion

		#region Properties

		public override string Name
		{
			get
			{
				return "sceUtility";
			}
		}

		#endregion

		#region State Management

		public sceUtility( Kernel kernel )
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

		#region Game Sharing

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC492F751, "sceUtilityGameSharingInitStart" )]
		public int sceUtilityGameSharingInitStart()
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xEFC6F80F, "sceUtilityGameSharingShutdownStart" )]
		public int sceUtilityGameSharingShutdownStart()
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7853182D, "sceUtilityGameSharingUpdate" )]
		public int sceUtilityGameSharingUpdate()
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x946963F3, "sceUtilityGameSharingGetStatus" )]
		public int sceUtilityGameSharingGetStatus()
		{
			return Module.NotImplementedReturn;
		}

		#endregion

		#region Netplay

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3AD50AE7, "sceNetplayDialogInitStart" )]
		public int sceNetplayDialogInitStart()
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBC6B6296, "sceNetplayDialogShutdownStart" )]
		public int sceNetplayDialogShutdownStart()
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x417BED54, "sceNetplayDialogUpdate" )]
		public int sceNetplayDialogUpdate()
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB6CEE597, "sceNetplayDialogGetStatus" )]
		public int sceNetplayDialogGetStatus()
		{
			return Module.NotImplementedReturn;
		}

		#endregion

		#region Netconf

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4DB1E739, "sceUtilityNetconfInitStart" )]
		public int sceUtilityNetconfInitStart()
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF88155F6, "sceUtilityNetconfShutdownStart" )]
		public int sceUtilityNetconfShutdownStart()
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x91E70E35, "sceUtilityNetconfUpdate" )]
		public int sceUtilityNetconfUpdate()
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6332AA39, "sceUtilityNetconfGetStatus" )]
		public int sceUtilityNetconfGetStatus()
		{
			return Module.NotImplementedReturn;
		}

		#endregion

		#region Savedata

		private const uint
			LoadNoMemStick = 0x80110301,
			LoadAccessError = 0x80110305,
			LoadDataBroken = 0x80110306,
			LoadNoData = 0x80110307,
			LoadBadParams = 0x80110308;

		private const uint
			SaveNoMemStick = 0x80110381,
			SaveNoSpace = 0x80110383,
			SaveMemStickProtected = 0x80110384,
			SaveAccessError = 0x80110385,
			SaveBadParams = 0x80110388,
			SaveNoUMD = 0x80110389,
			SaveWrongUMD = 0x8011038a;

		// REMEMBER - char in C# is 2 bytes! have to use bytes!
		private struct PspUtilitySavedataSFOParam
		{
			public fixed sbyte title[ 0x80 ];
			public fixed sbyte savedataTitle[ 0x80 ];
			public fixed sbyte detail[ 0x400 ];
			public byte parentalLevel;
			public fixed byte unknown[ 3 ];
		}

		private struct PspUtilitySavedataFileData
		{
			public void* buf;
			public uint bufSize;
			public uint size;	// ??? - why are there two sizes?
			public int unknown;
		}

		// Structure to hold the parameters for the ::sceUtilitySavedataInitStart function.
		private struct SceUtilitySavedataParam
		{
			// Size of the structure
			public uint size;

			public int language;

			public int buttonSwap;

			public fixed int unknown[ 4 ];
			public int result;
			public fixed int unknown2[ 4 ];

			// mode: 0 to load, 1 to save
			public int mode;
			public int bind;

			// unknown13 use 0x10
			public int overwriteMode;

			// gameName: name used from the game for saves, equal for all saves
			public fixed sbyte gameName[ 16 ];
			// saveName: name of the particular save, normally a number
			public fixed sbyte saveName[ 24 ];
			// fileName: name of the data file of the game for example DATA.BIN
			public fixed sbyte fileName[ 16 ];

			// pointer to a buffer that will contain data file unencrypted data
			public void* dataBuf;

			// size of allocated space to dataBuf
			public uint dataBufSize;
			public uint dataSize;

			public PspUtilitySavedataSFOParam sfoParam;

			public PspUtilitySavedataFileData icon0FileData;
			public PspUtilitySavedataFileData icon1FileData;
			public PspUtilitySavedataFileData pic1FileData;
			public PspUtilitySavedataFileData snd0FileData;

			fixed byte unknown17[ 4 ];
		}

		private UtilityStatus _saveStatus;

		[Stateless]
		[BiosFunction( 0x50C4CD57, "sceUtilitySavedataInitStart" )]
		// SDK location: /utility/psputility_savedata.h:97
		// SDK declaration: int sceUtilitySavedataInitStart(SceUtilitySavedataParam * params);
		public int sceUtilitySavedataInitStart( int saveParams )
		{
			SceUtilitySavedataParam* p = ( SceUtilitySavedataParam* )_memorySystem.Translate( ( uint )saveParams );

			string fileName = new string( p->fileName );
			string gameName = new string( p->gameName );
			string saveName = new string( p->saveName );

			// these will of course only be filled in on save
			string title = new string( p->sfoParam.title );
			string saveDataTitle = new string( p->sfoParam.savedataTitle );
			string detail = new string( p->sfoParam.detail );

			//A pointer to the data to be saved is in p->dataBuf, 
			//with length p->dataBufSize or p->dataSize

			//Data for the save icon0,1,pic1,snd0 is in the corresponding members of p

			Log.WriteLine( Verbosity.Critical, Feature.Bios, "sceUtilitySavedataInitStart: {0} {1} {2} {3}",
				p->mode == 0 ? "Load" : "Save", fileName, gameName, saveName );

			//For now, we just fake it all by saying that there is no data when a game wants to 
			//load, and that there is no memstick when a game wants to save.
			_saveStatus = UtilityStatus.Running;

			int mode = p->mode;
			if( mode == 0 )
			{
				//Load a file
				return unchecked( ( int )LoadNoData );
			}
			else
			{
				//Save a file
				return unchecked( ( int )SaveNoMemStick );
			}
		}

		[Stateless]
		[BiosFunction( 0x9790B33C, "sceUtilitySavedataShutdownStart" )]
		// SDK location: /utility/psputility_savedata.h:117
		// SDK declaration: int sceUtilitySavedataShutdownStart();
		public int sceUtilitySavedataShutdownStart()
		{
			_saveStatus = UtilityStatus.Closed;
			return 0;
		}

		[Stateless]
		[BiosFunction( 0xD4B95FFB, "sceUtilitySavedataUpdate" )]
		// SDK location: /utility/psputility_savedata.h:124
		// SDK declaration: int sceUtilitySavedataUpdate(int unknown);
		public int sceUtilitySavedataUpdate( int unknown )
		{
			switch( _saveStatus )
			{
				case UtilityStatus.Init:
					_saveStatus = UtilityStatus.Running;
					break;
				case UtilityStatus.Running:
					_saveStatus = UtilityStatus.Finished;
					break;
				case UtilityStatus.Finished:
					//_saveStatus = MessageStatus.Closed;
					break;
				case UtilityStatus.Closed:
					_saveStatus = UtilityStatus.None;
					break;
				case UtilityStatus.None:
					break;
			}
			return 0;
		}

		[Stateless]
		[BiosFunction( 0x8874DBE0, "sceUtilitySavedataGetStatus" )]
		// SDK location: /utility/psputility_savedata.h:107
		// SDK declaration: int sceUtilitySavedataGetStatus();
		public int sceUtilitySavedataGetStatus()
		{
			//this.sceUtilitySavedataUpdate( 2 );

			return ( int )_saveStatus;
		}

		#endregion

		#region MsgDialog

		// We fake this pretty hardcore ^_^

		private enum MessageLanguage
		{
			// ?? same as PBP I imagine
			Japanese = 0,
			English = 1,
			French = 2,
			Spanish = 3,
			German = 4,
			Italian = 5,
			Dutch = 6,
			Portuguese = 7,
			Korean = 8,
		}

		private enum MessageFlags
		{
			Error = 0x000,
			Info = 0x001,
			YesNo = 0x010,
			DefaultNo = 0x100,
		}

		private enum MessageType
		{
			Internal = 0,
			Provided = 1,
		}

		private enum MessageButton
		{
			Unknown = 0,
			Yes = 1,
			No = 2,
			Cancel = 3,
		}

		private unsafe class MsgDialog
		{
			public MessageLanguage Language;
			public bool SwapButtons;

			public MessageFlags Flags;

			public MessageType Type;
			public uint MessageID;
			public string Message;

			public UtilityStatus Status;
			public uint* ReturnValue;
			public uint* ReturnButton;
		}

		private MsgDialog _currentDialog;

		[Stateless]
		[BiosFunction( 0x2AD8E239, "sceUtilityMsgDialogInitStart" )]
		// SDK location: /utility/psputility_msgdialog.h:50
		// SDK declaration: int sceUtilityMsgDialogInitStart(SceUtilityMsgDialogParams *params);
		public unsafe int sceUtilityMsgDialogInitStart( int msgParams )
		{
			uint* p = ( uint* )_memorySystem.Translate( ( uint )msgParams );

			// Size... we don't care
			p++;

			MsgDialog dialog = new MsgDialog();
			dialog.Language = ( MessageLanguage )( *( p++ ) );
			dialog.SwapButtons = ( *( p++ ) == 1 );

			// unknowns
			p += 4;

			// We save the address so we can write to it later
			dialog.ReturnValue = p;
			p++;

			// unknowns
			p += 4;
			p++;

			dialog.Type = ( MessageType )( *p++ );

			if( dialog.Type == MessageType.Internal )
			{
				dialog.MessageID = *( p++ );
			}
			else
			{
				p++;
				dialog.Message = _kernel.ReadString( ( byte* )p, Encoding.UTF8 ).Trim();
			}
			p += ( 512 / 4 );

			dialog.Flags = ( MessageFlags )( *p++ );

			// Save address for later
			dialog.ReturnButton = p;
			p++;

			dialog.Status = UtilityStatus.Running;

			_currentDialog = dialog;

			Log.WriteLine( Verbosity.Normal, Feature.Bios, "MsgDialog: {0}", dialog.Message );

			// Fake the result!
			// Not sure exactly how right this is
			if( ( dialog.Flags & MessageFlags.YesNo ) == MessageFlags.YesNo )
			{
				if( ( dialog.Flags & MessageFlags.DefaultNo ) == MessageFlags.DefaultNo )
				{
					*dialog.ReturnValue = unchecked( ( uint )-1 );
					*dialog.ReturnButton = ( uint )MessageButton.No;
				}
				else
				{
					*dialog.ReturnValue = 0;
					*dialog.ReturnButton = ( uint )MessageButton.Yes;
				}
			}
			else
			{
				*dialog.ReturnValue = 0;
				*dialog.ReturnButton = ( uint )MessageButton.Yes;
			}

			return 0;
		}

		[Stateless]
		[BiosFunction( 0x67AF3428, "sceUtilityMsgDialogShutdownStart" )]
		// SDK location: /utility/psputility_msgdialog.h:57
		// SDK declaration: int sceUtilityMsgDialogShutdownStart();
		public int sceUtilityMsgDialogShutdownStart()
		{
			_currentDialog.Status = UtilityStatus.Closed;
			return 0;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4928BD96, "sceUtilityMsgDialogAbort" )]
		// manual add
		public void sceUtilityMsgDialogAbort()
		{
		}

		[Stateless]
		[BiosFunction( 0x95FC253B, "sceUtilityMsgDialogUpdate" )]
		// SDK location: /utility/psputility_msgdialog.h:73
		// SDK declaration: int sceUtilityMsgDialogUpdate(int n);
		public int sceUtilityMsgDialogUpdate( int n )
		{
			// Hacky state machine
			Debug.Assert( _currentDialog != null );
			switch( _currentDialog.Status )
			{
				case UtilityStatus.Init:
					_currentDialog.Status = UtilityStatus.Running;
					break;
				case UtilityStatus.Running:
					_currentDialog.Status = UtilityStatus.Finished;
					break;
				case UtilityStatus.Finished:
					//_currentDialog.Status = MessageStatus.Closed;
					break;
				case UtilityStatus.Closed:
					_currentDialog.Status = UtilityStatus.None;
					break;
			}
			return 0;
		}

		[Stateless]
		[BiosFunction( 0x9A1C91D7, "sceUtilityMsgDialogGetStatus" )]
		// SDK location: /utility/psputility_msgdialog.h:66
		// SDK declaration: int sceUtilityMsgDialogGetStatus();
		public int sceUtilityMsgDialogGetStatus()
		{
			if( _currentDialog == null )
				return -1;

			if( _currentDialog.Status == UtilityStatus.None )
				return -1;

			// ToE rarely calls update
			this.sceUtilityMsgDialogUpdate( 2 );

			return ( int )_currentDialog.Status;
		}

		#endregion

		#region Osk

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF6269B82, "sceUtilityOskInitStart" )]
		// SDK location: /utility/psputility_osk.h:86
		// SDK declaration: int sceUtilityOskInitStart(SceUtilityOskParams* params);
		public int sceUtilityOskInitStart( int oskParams )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3DFAEBA9, "sceUtilityOskShutdownStart" )]
		// SDK location: /utility/psputility_osk.h:92
		// SDK declaration: int sceUtilityOskShutdownStart();
		public int sceUtilityOskShutdownStart()
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4B85C861, "sceUtilityOskUpdate" )]
		// SDK location: /utility/psputility_osk.h:99
		// SDK declaration: int sceUtilityOskUpdate(int n);
		public int sceUtilityOskUpdate( int n )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF3F76017, "sceUtilityOskGetStatus" )]
		// SDK location: /utility/psputility_osk.h:106
		// SDK declaration: int sceUtilityOskGetStatus();
		public int sceUtilityOskGetStatus()
		{
			return Module.NotImplementedReturn;
		}

		#endregion

		#region SystemParam

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x45C18506, "sceUtilitySetSystemParamInt" )]
		// SDK location: /utility/psputility_sysparam.h:104
		// SDK declaration: int sceUtilitySetSystemParamInt(int id, int value);
		public int sceUtilitySetSystemParamInt( int id, int value )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x41E30674, "sceUtilitySetSystemParamString" )]
		// SDK location: /utility/psputility_sysparam.h:113
		// SDK declaration: int sceUtilitySetSystemParamString(int id, const char *str);
		public int sceUtilitySetSystemParamString( int id, int str )
		{
			return Module.NotImplementedReturn;
		}

		[Stateless]
		[BiosFunction( 0xA5DA2406, "sceUtilityGetSystemParamInt" )]
		// SDK location: /utility/psputility_sysparam.h:122
		// SDK declaration: int sceUtilityGetSystemParamInt( int id, int *value );
		public int sceUtilityGetSystemParamInt( int id, int valueAddr )
		{
			uint* ptr = ( uint* )_memorySystem.Translate( ( uint )valueAddr );
			switch( id )
			{
				case 7:
					*ptr = 1; //english
					break;
				default:
					*ptr = 0;
					break;
			};
			return 0;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x34B78343, "sceUtilityGetSystemParamString" )]
		// SDK location: /utility/psputility_sysparam.h:132
		// SDK declaration: int sceUtilityGetSystemParamString(int id, char *str, int len);
		public int sceUtilityGetSystemParamString( int id, int str, int len )
		{
			return Module.NotImplementedReturn;
		}

		#endregion

		#region NetParam

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5EEE6548, "sceUtilityCheckNetParam" )]
		// SDK location: /utility/psputility_netparam.h:60
		// SDK declaration: int sceUtilityCheckNetParam(int id);
		public int sceUtilityCheckNetParam( int id )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x434D4B3A, "sceUtilityGetNetParam" )]
		// SDK location: /utility/psputility_netparam.h:71
		// SDK declaration: int sceUtilityGetNetParam(int conf, int param, netData *data);
		public int sceUtilityGetNetParam( int conf, int param, int data )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x072DEBF2, "sceUtilityCreateNetParam" )]
		// SDK location: /utility/psputility_netparam.h:81
		// SDK declaration: int sceUtilityCreateNetParam(int conf);
		public int sceUtilityCreateNetParam( int conf )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9CE50172, "sceUtilityDeleteNetParam" )]
		// SDK location: /utility/psputility_netparam.h:111
		// SDK declaration: int sceUtilityDeleteNetParam(int conf);
		public int sceUtilityDeleteNetParam( int conf )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFB0C4840, "sceUtilityCopyNetParam" )]
		// SDK location: /utility/psputility_netparam.h:102
		// SDK declaration: int sceUtilityCopyNetParam(int src, int dest);
		public int sceUtilityCopyNetParam( int src, int dest )
		{
			return Module.NotImplementedReturn;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFC4516F3, "sceUtilitySetNetParam" )]
		// SDK location: /utility/psputility_netparam.h:92
		// SDK declaration: int sceUtilitySetNetParam(int param, const void *val);
		public int sceUtilitySetNetParam( int param, int val )
		{
			return Module.NotImplementedReturn;
		}

		#endregion
	}
}

/* GenerateStubsV2: auto-generated - A0F86C34 */
