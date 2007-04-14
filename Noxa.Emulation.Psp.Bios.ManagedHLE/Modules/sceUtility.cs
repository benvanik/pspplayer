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
	class sceUtility : Module
	{
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

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x50C4CD57, "sceUtilitySavedataInitStart" )]
		// SDK location: /utility/psputility_savedata.h:97
		// SDK declaration: int sceUtilitySavedataInitStart(SceUtilitySavedataParam * params);
		int sceUtilitySavedataInitStart( int saveParams ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9790B33C, "sceUtilitySavedataShutdownStart" )]
		// SDK location: /utility/psputility_savedata.h:117
		// SDK declaration: int sceUtilitySavedataShutdownStart();
		int sceUtilitySavedataShutdownStart(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD4B95FFB, "sceUtilitySavedataUpdate" )]
		// SDK location: /utility/psputility_savedata.h:124
		// SDK declaration: void sceUtilitySavedataUpdate(int unknown);
		void sceUtilitySavedataUpdate( int unknown ){}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8874DBE0, "sceUtilitySavedataGetStatus" )]
		// SDK location: /utility/psputility_savedata.h:107
		// SDK declaration: int sceUtilitySavedataGetStatus();
		int sceUtilitySavedataGetStatus(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2AD8E239, "sceUtilityMsgDialogInitStart" )]
		// SDK location: /utility/psputility_msgdialog.h:50
		// SDK declaration: int sceUtilityMsgDialogInitStart(SceUtilityMsgDialogParams *params);
		int sceUtilityMsgDialogInitStart( int msgParams ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x67AF3428, "sceUtilityMsgDialogShutdownStart" )]
		// SDK location: /utility/psputility_msgdialog.h:57
		// SDK declaration: void sceUtilityMsgDialogShutdownStart();
		void sceUtilityMsgDialogShutdownStart(){}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4928BD96, "sceUtilityMsgDialogAbort" )]
		// manual add
		void sceUtilityMsgDialogAbort()
		{
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x95FC253B, "sceUtilityMsgDialogUpdate" )]
		// SDK location: /utility/psputility_msgdialog.h:73
		// SDK declaration: void sceUtilityMsgDialogUpdate(int n);
		void sceUtilityMsgDialogUpdate( int n ){}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9A1C91D7, "sceUtilityMsgDialogGetStatus" )]
		// SDK location: /utility/psputility_msgdialog.h:66
		// SDK declaration: int sceUtilityMsgDialogGetStatus();
		int sceUtilityMsgDialogGetStatus(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF6269B82, "sceUtilityOskInitStart" )]
		// SDK location: /utility/psputility_osk.h:86
		// SDK declaration: int sceUtilityOskInitStart(SceUtilityOskParams* params);
		int sceUtilityOskInitStart( int oskParams ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3DFAEBA9, "sceUtilityOskShutdownStart" )]
		// SDK location: /utility/psputility_osk.h:92
		// SDK declaration: int sceUtilityOskShutdownStart();
		int sceUtilityOskShutdownStart(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4B85C861, "sceUtilityOskUpdate" )]
		// SDK location: /utility/psputility_osk.h:99
		// SDK declaration: int sceUtilityOskUpdate(int n);
		int sceUtilityOskUpdate( int n ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF3F76017, "sceUtilityOskGetStatus" )]
		// SDK location: /utility/psputility_osk.h:106
		// SDK declaration: int sceUtilityOskGetStatus();
		int sceUtilityOskGetStatus(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x45C18506, "sceUtilitySetSystemParamInt" )]
		// SDK location: /utility/psputility_sysparam.h:104
		// SDK declaration: int sceUtilitySetSystemParamInt(int id, int value);
		int sceUtilitySetSystemParamInt( int id, int value ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x41E30674, "sceUtilitySetSystemParamString" )]
		// SDK location: /utility/psputility_sysparam.h:113
		// SDK declaration: int sceUtilitySetSystemParamString(int id, const char *str);
		int sceUtilitySetSystemParamString( int id, int str ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA5DA2406, "sceUtilityGetSystemParamInt" )]
		// SDK location: /utility/psputility_sysparam.h:122
		// SDK declaration: int sceUtilityGetSystemParamInt( int id, int *value );
		int sceUtilityGetSystemParamInt( int id, int value ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x34B78343, "sceUtilityGetSystemParamString" )]
		// SDK location: /utility/psputility_sysparam.h:132
		// SDK declaration: int sceUtilityGetSystemParamString(int id, char *str, int len);
		int sceUtilityGetSystemParamString( int id, int str, int len ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5EEE6548, "sceUtilityCheckNetParam" )]
		// SDK location: /utility/psputility_netparam.h:60
		// SDK declaration: int sceUtilityCheckNetParam(int id);
		int sceUtilityCheckNetParam( int id ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x434D4B3A, "sceUtilityGetNetParam" )]
		// SDK location: /utility/psputility_netparam.h:71
		// SDK declaration: int sceUtilityGetNetParam(int conf, int param, netData *data);
		int sceUtilityGetNetParam( int conf, int param, int data ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x072DEBF2, "sceUtilityCreateNetParam" )]
		// SDK location: /utility/psputility_netparam.h:81
		// SDK declaration: int sceUtilityCreateNetParam(int conf);
		int sceUtilityCreateNetParam( int conf ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9CE50172, "sceUtilityDeleteNetParam" )]
		// SDK location: /utility/psputility_netparam.h:111
		// SDK declaration: int sceUtilityDeleteNetParam(int conf);
		int sceUtilityDeleteNetParam( int conf ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFB0C4840, "sceUtilityCopyNetParam" )]
		// SDK location: /utility/psputility_netparam.h:102
		// SDK declaration: int sceUtilityCopyNetParam(int src, int dest);
		int sceUtilityCopyNetParam( int src, int dest ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFC4516F3, "sceUtilitySetNetParam" )]
		// SDK location: /utility/psputility_netparam.h:92
		// SDK declaration: int sceUtilitySetNetParam(int param, const void *val);
		int sceUtilitySetNetParam( int param, int val ){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - 697E771E */
