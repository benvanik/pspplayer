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
	class ThreadManForUser : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "ThreadManForUser";
			}
		}

		#endregion

		#region State Management

		public ThreadManForUser( Kernel kernel )
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
		[BiosFunction( 0x6E9EA350, "_sceKernelReturnFromCallback" )]
		// SDK location: /user/pspthreadman.h:1453
		// SDK declaration: void _sceKernelReturnFromCallback();
		public void _sceKernelReturnFromCallback(){}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0C106E53, "sceKernelRegisterThreadEventHandler" )]
		// SDK location: /user/pspthreadman.h:1729
		// SDK declaration: SceUID sceKernelRegisterThreadEventHandler(const char *name, SceUID threadID, int mask, SceKernelThreadEventHandler handler, void *common);
		public int sceKernelRegisterThreadEventHandler( int name, int threadID, int mask, int handler, int common ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x72F3C145, "sceKernelReleaseThreadEventHandler" )]
		// SDK location: /user/pspthreadman.h:1738
		// SDK declaration: int sceKernelReleaseThreadEventHandler(SceUID uid);
		public int sceKernelReleaseThreadEventHandler( int uid ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x369EEB6B, "sceKernelReferThreadEventHandlerStatus" )]
		// SDK location: /user/pspthreadman.h:1748
		// SDK declaration: int sceKernelReferThreadEventHandlerStatus(SceUID uid, struct SceKernelThreadEventHandlerInfo *info);
		public int sceKernelReferThreadEventHandlerStatus( int uid, int info ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE81CAF8F, "sceKernelCreateCallback" )]
		// SDK location: /user/pspthreadman.h:985
		// SDK declaration: int sceKernelCreateCallback(const char *name, SceKernelCallbackFunction func, void *arg);
		public int sceKernelCreateCallback( int name, int func, int arg ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xEDBA5844, "sceKernelDeleteCallback" )]
		// SDK location: /user/pspthreadman.h:1005
		// SDK declaration: int sceKernelDeleteCallback(SceUID cb);
		public int sceKernelDeleteCallback( int cb ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC11BA8C4, "sceKernelNotifyCallback" )]
		// SDK location: /user/pspthreadman.h:1015
		// SDK declaration: int sceKernelNotifyCallback(SceUID cb, int arg2);
		public int sceKernelNotifyCallback( int cb, int arg2 ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBA4051D6, "sceKernelCancelCallback" )]
		// SDK location: /user/pspthreadman.h:1024
		// SDK declaration: int sceKernelCancelCallback(SceUID cb);
		public int sceKernelCancelCallback( int cb ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2A3D44FF, "sceKernelGetCallbackCount" )]
		// SDK location: /user/pspthreadman.h:1033
		// SDK declaration: int sceKernelGetCallbackCount(SceUID cb);
		public int sceKernelGetCallbackCount( int cb ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x349D6D6C, "sceKernelCheckCallback" )]
		// SDK location: /user/pspthreadman.h:1040
		// SDK declaration: int sceKernelCheckCallback();
		public int sceKernelCheckCallback(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x730ED8BC, "sceKernelReferCallbackStatus" )]
		// SDK location: /user/pspthreadman.h:996
		// SDK declaration: int sceKernelReferCallbackStatus(SceUID cb, SceKernelCallbackInfo *status);
		public int sceKernelReferCallbackStatus( int cb, int status ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9ACE131E, "sceKernelSleepThread" )]
		// SDK location: /user/pspthreadman.h:244
		// SDK declaration: int sceKernelSleepThread();
		public int sceKernelSleepThread(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x82826F70, "sceKernelSleepThreadCB" )]
		// SDK location: /user/pspthreadman.h:255
		// SDK declaration: int sceKernelSleepThreadCB();
		public int sceKernelSleepThreadCB(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD59EAD2F, "sceKernelWakeupThread" )]
		// SDK location: /user/pspthreadman.h:264
		// SDK declaration: int sceKernelWakeupThread(SceUID thid);
		public int sceKernelWakeupThread( int thid ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFCCFAD26, "sceKernelCancelWakeupThread" )]
		// SDK location: /user/pspthreadman.h:273
		// SDK declaration: int sceKernelCancelWakeupThread(SceUID thid);
		public int sceKernelCancelWakeupThread( int thid ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9944F31F, "sceKernelSuspendThread" )]
		// SDK location: /user/pspthreadman.h:282
		// SDK declaration: int sceKernelSuspendThread(SceUID thid);
		public int sceKernelSuspendThread( int thid ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x75156E8F, "sceKernelResumeThread" )]
		// SDK location: /user/pspthreadman.h:291
		// SDK declaration: int sceKernelResumeThread(SceUID thid);
		public int sceKernelResumeThread( int thid ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x278C0DF5, "sceKernelWaitThreadEnd" )]
		// SDK location: /user/pspthreadman.h:301
		// SDK declaration: int sceKernelWaitThreadEnd(SceUID thid, SceUInt *timeout);
		public int sceKernelWaitThreadEnd( int thid, int timeout ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x840E8133, "sceKernelWaitThreadEndCB" )]
		// SDK location: /user/pspthreadman.h:311
		// SDK declaration: int sceKernelWaitThreadEndCB(SceUID thid, SceUInt *timeout);
		public int sceKernelWaitThreadEndCB( int thid, int timeout ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCEADEB47, "sceKernelDelayThread" )]
		// SDK location: /user/pspthreadman.h:323
		// SDK declaration: int sceKernelDelayThread(SceUInt delay);
		public int sceKernelDelayThread( int delay ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x68DA9E36, "sceKernelDelayThreadCB" )]
		// SDK location: /user/pspthreadman.h:335
		// SDK declaration: int sceKernelDelayThreadCB(SceUInt delay);
		public int sceKernelDelayThreadCB( int delay ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBD123D9E, "sceKernelDelaySysClockThread" )]
		// SDK location: /user/pspthreadman.h:344
		// SDK declaration: int sceKernelDelaySysClockThread(SceKernelSysClock *delay);
		public int sceKernelDelaySysClockThread( int delay ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1181E963, "sceKernelDelaySysClockThreadCB" )]
		// SDK location: /user/pspthreadman.h:354
		// SDK declaration: int sceKernelDelaySysClockThreadCB(SceKernelSysClock *delay);
		public int sceKernelDelaySysClockThreadCB( int delay ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD6DA4BA1, "sceKernelCreateSema" )]
		// SDK location: /user/pspthreadman.h:515
		// SDK declaration: SceUID sceKernelCreateSema(const char *name, SceUInt attr, int initVal, int maxVal, SceKernelSemaOptParam *option);
		public int sceKernelCreateSema( int name, int attr, int initVal, int maxVal, int option ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x28B6489C, "sceKernelDeleteSema" )]
		// SDK location: /user/pspthreadman.h:523
		// SDK declaration: int sceKernelDeleteSema(SceUID semaid);
		public int sceKernelDeleteSema( int semaid ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3F53E640, "sceKernelSignalSema" )]
		// SDK location: /user/pspthreadman.h:539
		// SDK declaration: int sceKernelSignalSema(SceUID semaid, int signal);
		public int sceKernelSignalSema( int semaid, int signal ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4E3A1105, "sceKernelWaitSema" )]
		// SDK location: /user/pspthreadman.h:555
		// SDK declaration: int sceKernelWaitSema(SceUID semaid, int signal, SceUInt *timeout);
		public int sceKernelWaitSema( int semaid, int signal, int timeout ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6D212BAC, "sceKernelWaitSemaCB" )]
		// SDK location: /user/pspthreadman.h:571
		// SDK declaration: int sceKernelWaitSemaCB(SceUID semaid, int signal, SceUInt *timeout);
		public int sceKernelWaitSemaCB( int semaid, int signal, int timeout ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x58B1F937, "sceKernelPollSema" )]
		// SDK location: /user/pspthreadman.h:581
		// SDK declaration: int sceKernelPollSema(SceUID semaid, int signal);
		public int sceKernelPollSema( int semaid, int signal ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBC6FEBC5, "sceKernelReferSemaStatus" )]
		// SDK location: /user/pspthreadman.h:591
		// SDK declaration: int sceKernelReferSemaStatus(SceUID semaid, SceKernelSemaInfo *info);
		public int sceKernelReferSemaStatus( int semaid, int info ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x55C20A00, "sceKernelCreateEventFlag" )]
		// SDK location: /user/pspthreadman.h:645
		// SDK declaration: SceUID sceKernelCreateEventFlag(const char *name, int attr, int bits, SceKernelEventFlagOptParam *opt);
		public int sceKernelCreateEventFlag( int name, int attr, int bits, int opt ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xEF9E4C70, "sceKernelDeleteEventFlag" )]
		// SDK location: /user/pspthreadman.h:709
		// SDK declaration: int sceKernelDeleteEventFlag(int evid);
		public int sceKernelDeleteEventFlag( int evid ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1FB15A32, "sceKernelSetEventFlag" )]
		// SDK location: /user/pspthreadman.h:655
		// SDK declaration: int sceKernelSetEventFlag(SceUID evid, u32 bits);
		public int sceKernelSetEventFlag( int evid, int bits ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x812346E4, "sceKernelClearEventFlag" )]
		// SDK location: /user/pspthreadman.h:665
		// SDK declaration: int sceKernelClearEventFlag(SceUID evid, u32 bits);
		public int sceKernelClearEventFlag( int evid, int bits ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x402FCF22, "sceKernelWaitEventFlag" )]
		// SDK location: /user/pspthreadman.h:688
		// SDK declaration: int sceKernelWaitEventFlag(int evid, u32 bits, u32 wait, u32 *outBits, SceUInt *timeout);
		public int sceKernelWaitEventFlag( int evid, int bits, int wait, int outBits, int timeout ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x328C546A, "sceKernelWaitEventFlagCB" )]
		// SDK location: /user/pspthreadman.h:700
		// SDK declaration: int sceKernelWaitEventFlagCB(int evid, u32 bits, u32 wait, u32 *outBits, SceUInt *timeout);
		public int sceKernelWaitEventFlagCB( int evid, int bits, int wait, int outBits, int timeout ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x30FD48F0, "sceKernelPollEventFlag" )]
		// SDK location: /user/pspthreadman.h:676
		// SDK declaration: int sceKernelPollEventFlag(int evid, u32 bits, u32 wait, u32 *outBits);
		public int sceKernelPollEventFlag( int evid, int bits, int wait, int outBits ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA66B0120, "sceKernelReferEventFlagStatus" )]
		// SDK location: /user/pspthreadman.h:719
		// SDK declaration: int sceKernelReferEventFlagStatus(SceUID event, SceKernelEventFlagInfo *status);
		public int sceKernelReferEventFlagStatus( int evid, int status ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8125221D, "sceKernelCreateMbx" )]
		// SDK location: /user/pspthreadman.h:774
		// SDK declaration: SceUID sceKernelCreateMbx(const char *name, SceUInt attr, SceKernelMbxOptParam *option);
		public int sceKernelCreateMbx( int name, int attr, int option ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x86255ADA, "sceKernelDeleteMbx" )]
		// SDK location: /user/pspthreadman.h:782
		// SDK declaration: int sceKernelDeleteMbx(SceUID mbxid);
		public int sceKernelDeleteMbx( int mbxid ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE9B3061E, "sceKernelSendMbx" )]
		// SDK location: /user/pspthreadman.h:806
		// SDK declaration: int sceKernelSendMbx(SceUID mbxid, void *message);
		public int sceKernelSendMbx( int mbxid, int message ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x18260574, "sceKernelReceiveMbx" )]
		// SDK location: /user/pspthreadman.h:824
		// SDK declaration: int sceKernelReceiveMbx(SceUID mbxid, void **pmessage, SceUInt *timeout);
		public int sceKernelReceiveMbx( int mbxid, int pmessage, int timeout ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF3986382, "sceKernelReceiveMbxCB" )]
		// SDK location: /user/pspthreadman.h:842
		// SDK declaration: int sceKernelReceiveMbxCB(SceUID mbxid, void **pmessage, SceUInt *timeout);
		public int sceKernelReceiveMbxCB( int mbxid, int pmessage, int timeout ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0D81716A, "sceKernelPollMbx" )]
		// SDK location: /user/pspthreadman.h:859
		// SDK declaration: int sceKernelPollMbx(SceUID mbxid, void **pmessage);
		public int sceKernelPollMbx( int mbxid, int pmessage ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x87D4DD36, "sceKernelCancelReceiveMbx" )]
		// SDK location: /user/pspthreadman.h:876
		// SDK declaration: int sceKernelCancelReceiveMbx(SceUID mbxid, int *pnum);
		public int sceKernelCancelReceiveMbx( int mbxid, int pnum ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA8E8C846, "sceKernelReferMbxStatus" )]
		// SDK location: /user/pspthreadman.h:886
		// SDK declaration: int sceKernelReferMbxStatus(SceUID mbxid, SceKernelMbxInfo *info);
		public int sceKernelReferMbxStatus( int mbxid, int info ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7C0DC2A0, "sceKernelCreateMsgPipe" )]
		// SDK location: /user/pspthreadman.h:1114
		// SDK declaration: SceUID sceKernelCreateMsgPipe(const char *name, int part, int attr, void *unk1, void *opt);
		public int sceKernelCreateMsgPipe( int name, int part, int attr, int unk1, int opt ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF0B7DA1C, "sceKernelDeleteMsgPipe" )]
		// SDK location: /user/pspthreadman.h:1123
		// SDK declaration: int sceKernelDeleteMsgPipe(SceUID uid);
		public int sceKernelDeleteMsgPipe( int uid ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x876DBFAD, "sceKernelSendMsgPipe" )]
		// SDK location: /user/pspthreadman.h:1137
		// SDK declaration: int sceKernelSendMsgPipe(SceUID uid, void *message, unsigned int size, int unk1, void *unk2, unsigned int *timeout);
		public int sceKernelSendMsgPipe( int uid, int message, int size, int unk1, int unk2, int timeout ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7C41F2C2, "sceKernelSendMsgPipeCB" )]
		// SDK location: /user/pspthreadman.h:1151
		// SDK declaration: int sceKernelSendMsgPipeCB(SceUID uid, void *message, unsigned int size, int unk1, void *unk2, unsigned int *timeout);
		public int sceKernelSendMsgPipeCB( int uid, int message, int size, int unk1, int unk2, int timeout ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x884C9F90, "sceKernelTrySendMsgPipe" )]
		// SDK location: /user/pspthreadman.h:1164
		// SDK declaration: int sceKernelTrySendMsgPipe(SceUID uid, void *message, unsigned int size, int unk1, void *unk2);
		public int sceKernelTrySendMsgPipe( int uid, int message, int size, int unk1, int unk2 ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x74829B76, "sceKernelReceiveMsgPipe" )]
		// SDK location: /user/pspthreadman.h:1178
		// SDK declaration: int sceKernelReceiveMsgPipe(SceUID uid, void *message, unsigned int size, int unk1, void *unk2, unsigned int *timeout);
		public int sceKernelReceiveMsgPipe( int uid, int message, int size, int unk1, int unk2, int timeout ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFBFA697D, "sceKernelReceiveMsgPipeCB" )]
		// SDK location: /user/pspthreadman.h:1192
		// SDK declaration: int sceKernelReceiveMsgPipeCB(SceUID uid, void *message, unsigned int size, int unk1, void *unk2, unsigned int *timeout);
		public int sceKernelReceiveMsgPipeCB( int uid, int message, int size, int unk1, int unk2, int timeout ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDF52098F, "sceKernelTryReceiveMsgPipe" )]
		// SDK location: /user/pspthreadman.h:1205
		// SDK declaration: int sceKernelTryReceiveMsgPipe(SceUID uid, void *message, unsigned int size, int unk1, void *unk2);
		public int sceKernelTryReceiveMsgPipe( int uid, int message, int size, int unk1, int unk2 ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x349B864D, "sceKernelCancelMsgPipe" )]
		// SDK location: /user/pspthreadman.h:1216
		// SDK declaration: int sceKernelCancelMsgPipe(SceUID uid, int *psend, int *precv);
		public int sceKernelCancelMsgPipe( int uid, int psend, int precv ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x33BE4024, "sceKernelReferMsgPipeStatus" )]
		// SDK location: /user/pspthreadman.h:1237
		// SDK declaration: int sceKernelReferMsgPipeStatus(SceUID uid, SceKernelMppInfo *info);
		public int sceKernelReferMsgPipeStatus( int uid, int info ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x56C039B5, "sceKernelCreateVpl" )]
		// SDK location: /user/pspthreadman.h:1256
		// SDK declaration: SceUID sceKernelCreateVpl(const char *name, int part, int attr, unsigned int size, struct SceKernelVplOptParam *opt);
		public int sceKernelCreateVpl( int name, int part, int attr, int size, int opt ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x89B3D48C, "sceKernelDeleteVpl" )]
		// SDK location: /user/pspthreadman.h:1265
		// SDK declaration: int sceKernelDeleteVpl(SceUID uid);
		public int sceKernelDeleteVpl( int uid ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBED27435, "sceKernelAllocateVpl" )]
		// SDK location: /user/pspthreadman.h:1277
		// SDK declaration: int sceKernelAllocateVpl(SceUID uid, unsigned int size, void **data, unsigned int *timeout);
		public int sceKernelAllocateVpl( int uid, int size, int data, int timeout ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xEC0A693F, "sceKernelAllocateVplCB" )]
		// SDK location: /user/pspthreadman.h:1289
		// SDK declaration: int sceKernelAllocateVplCB(SceUID uid, unsigned int size, void **data, unsigned int *timeout);
		public int sceKernelAllocateVplCB( int uid, int size, int data, int timeout ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xAF36D708, "sceKernelTryAllocateVpl" )]
		// SDK location: /user/pspthreadman.h:1300
		// SDK declaration: int sceKernelTryAllocateVpl(SceUID uid, unsigned int size, void **data);
		public int sceKernelTryAllocateVpl( int uid, int size, int data ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB736E9FF, "sceKernelFreeVpl" )]
		// SDK location: /user/pspthreadman.h:1310
		// SDK declaration: int sceKernelFreeVpl(SceUID uid, void *data);
		public int sceKernelFreeVpl( int uid, int data ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1D371B8A, "sceKernelCancelVpl" )]
		// SDK location: /user/pspthreadman.h:1320
		// SDK declaration: int sceKernelCancelVpl(SceUID uid, int *pnum);
		public int sceKernelCancelVpl( int uid, int pnum ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x39810265, "sceKernelReferVplStatus" )]
		// SDK location: /user/pspthreadman.h:1340
		// SDK declaration: int sceKernelReferVplStatus(SceUID uid, SceKernelVplInfo *info);
		public int sceKernelReferVplStatus( int uid, int info ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC07BB470, "sceKernelCreateFpl" )]
		// SDK location: /user/pspthreadman.h:1360
		// SDK declaration: int sceKernelCreateFpl(const char *name, int part, int attr, unsigned int size, unsigned int blocks, struct SceKernelFplOptParam *opt);
		public int sceKernelCreateFpl( int name, int part, int attr, int size, int blocks, int opt ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xED1410E0, "sceKernelDeleteFpl" )]
		// SDK location: /user/pspthreadman.h:1369
		// SDK declaration: int sceKernelDeleteFpl(SceUID uid);
		public int sceKernelDeleteFpl( int uid ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD979E9BF, "sceKernelAllocateFpl" )]
		// SDK location: /user/pspthreadman.h:1380
		// SDK declaration: int sceKernelAllocateFpl(SceUID uid, void **data, unsigned int *timeout);
		public int sceKernelAllocateFpl( int uid, int data, int timeout ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE7282CB6, "sceKernelAllocateFplCB" )]
		// SDK location: /user/pspthreadman.h:1391
		// SDK declaration: int sceKernelAllocateFplCB(SceUID uid, void **data, unsigned int *timeout);
		public int sceKernelAllocateFplCB( int uid, int data, int timeout ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x623AE665, "sceKernelTryAllocateFpl" )]
		// SDK location: /user/pspthreadman.h:1401
		// SDK declaration: int sceKernelTryAllocateFpl(SceUID uid, void **data);
		public int sceKernelTryAllocateFpl( int uid, int data ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF6414A71, "sceKernelFreeFpl" )]
		// SDK location: /user/pspthreadman.h:1411
		// SDK declaration: int sceKernelFreeFpl(SceUID uid, void *data);
		public int sceKernelFreeFpl( int uid, int data ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA8AA591F, "sceKernelCancelFpl" )]
		// SDK location: /user/pspthreadman.h:1421
		// SDK declaration: int sceKernelCancelFpl(SceUID uid, int *pnum);
		public int sceKernelCancelFpl( int uid, int pnum ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD8199E4C, "sceKernelReferFplStatus" )]
		// SDK location: /user/pspthreadman.h:1442
		// SDK declaration: int sceKernelReferFplStatus(SceUID uid, SceKernelFplInfo *info);
		public int sceKernelReferFplStatus( int uid, int info ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0E927AED, "_sceKernelReturnFromTimerHandler" )]
		// SDK location: /user/pspthreadman.h:1447
		// SDK declaration: void _sceKernelReturnFromTimerHandler();
		public void _sceKernelReturnFromTimerHandler(){}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x110DEC9A, "sceKernelUSec2SysClock" )]
		// SDK location: /user/pspthreadman.h:1463
		// SDK declaration: int sceKernelUSec2SysClock(unsigned int usec, SceKernelSysClock *clock);
		public int sceKernelUSec2SysClock( int usec, int clock ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC8CD158C, "sceKernelUSec2SysClockWide" )]
		// SDK location: /user/pspthreadman.h:1472
		// SDK declaration: SceInt64 sceKernelUSec2SysClockWide(unsigned int usec);
		public long sceKernelUSec2SysClockWide( int usec ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBA6B92E2, "sceKernelSysClock2USec" )]
		// SDK location: /user/pspthreadman.h:1483
		// SDK declaration: int sceKernelSysClock2USec(SceKernelSysClock *clock, unsigned int *low, unsigned int *high);
		public int sceKernelSysClock2USec( int clock, int low, int high ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE1619D7C, "sceKernelSysClock2USecWide" )]
		// SDK location: /user/pspthreadman.h:1494
		// SDK declaration: int sceKernelSysClock2USecWide(SceInt64 clock, unsigned *low, unsigned int *high);
		public int sceKernelSysClock2USecWide( long clock, int low, int high ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDB738F35, "sceKernelGetSystemTime" )]
		// SDK location: /user/pspthreadman.h:1503
		// SDK declaration: int sceKernelGetSystemTime(SceKernelSysClock *time);
		public int sceKernelGetSystemTime( int time ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x82BC5777, "sceKernelGetSystemTimeWide" )]
		// SDK location: /user/pspthreadman.h:1510
		// SDK declaration: SceInt64 sceKernelGetSystemTimeWide();
		public long sceKernelGetSystemTimeWide(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x369ED59D, "sceKernelGetSystemTimeLow" )]
		// SDK location: /user/pspthreadman.h:1517
		// SDK declaration: unsigned int sceKernelGetSystemTimeLow();
		public int sceKernelGetSystemTimeLow(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6652B8CA, "sceKernelSetAlarm" )]
		// SDK location: /user/pspthreadman.h:915
		// SDK declaration: SceUID sceKernelSetAlarm(SceUInt clock, SceKernelAlarmHandler handler, void *common);
		public int sceKernelSetAlarm( int clock, int handler, int common ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB2C25152, "sceKernelSetSysClockAlarm" )]
		// SDK location: /user/pspthreadman.h:926
		// SDK declaration: SceUID sceKernelSetSysClockAlarm(SceKernelSysClock *clock, SceKernelAlarmHandler handler, void *common);
		public int sceKernelSetSysClockAlarm( int clock, int handler, int common ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7E65B999, "sceKernelCancelAlarm" )]
		// SDK location: /user/pspthreadman.h:935
		// SDK declaration: int sceKernelCancelAlarm(SceUID alarmid);
		public int sceKernelCancelAlarm( int alarmid ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDAA3F564, "sceKernelReferAlarmStatus" )]
		// SDK location: /user/pspthreadman.h:945
		// SDK declaration: int sceKernelReferAlarmStatus(SceUID alarmid, SceKernelAlarmInfo *info);
		public int sceKernelReferAlarmStatus( int alarmid, int info ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x20FFF560, "sceKernelCreateVTimer" )]
		// SDK location: /user/pspthreadman.h:1531
		// SDK declaration: SceUID sceKernelCreateVTimer(const char *name, struct SceKernelVTimerOptParam *opt);
		public int sceKernelCreateVTimer( int name, int opt ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x328F9E52, "sceKernelDeleteVTimer" )]
		// SDK location: /user/pspthreadman.h:1540
		// SDK declaration: int sceKernelDeleteVTimer(SceUID uid);
		public int sceKernelDeleteVTimer( int uid ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB3A59970, "sceKernelGetVTimerBase" )]
		// SDK location: /user/pspthreadman.h:1550
		// SDK declaration: int sceKernelGetVTimerBase(SceUID uid, SceKernelSysClock *base);
		public int sceKernelGetVTimerBase( int uid, int clockBase ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB7C18B77, "sceKernelGetVTimerBaseWide" )]
		// SDK location: /user/pspthreadman.h:1559
		// SDK declaration: SceInt64 sceKernelGetVTimerBaseWide(SceUID uid);
		public long sceKernelGetVTimerBaseWide( int uid ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x034A921F, "sceKernelGetVTimerTime" )]
		// SDK location: /user/pspthreadman.h:1569
		// SDK declaration: int sceKernelGetVTimerTime(SceUID uid, SceKernelSysClock *time);
		public int sceKernelGetVTimerTime( int uid, int time ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC0B3FFD2, "sceKernelGetVTimerTimeWide" )]
		// SDK location: /user/pspthreadman.h:1578
		// SDK declaration: SceInt64 sceKernelGetVTimerTimeWide(SceUID uid);
		public long sceKernelGetVTimerTimeWide( int uid ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x542AD630, "sceKernelSetVTimerTime" )]
		// SDK location: /user/pspthreadman.h:1588
		// SDK declaration: int sceKernelSetVTimerTime(SceUID uid, SceKernelSysClock *time);
		public int sceKernelSetVTimerTime( int uid, int time ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFB6425C3, "sceKernelSetVTimerTimeWide" )]
		// SDK location: /user/pspthreadman.h:1598
		// SDK declaration: SceInt64 sceKernelSetVTimerTimeWide(SceUID uid, SceInt64 time);
		public long sceKernelSetVTimerTimeWide( int uid, long time ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC68D9437, "sceKernelStartVTimer" )]
		// SDK location: /user/pspthreadman.h:1607
		// SDK declaration: int sceKernelStartVTimer(SceUID uid);
		public int sceKernelStartVTimer( int uid ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD0AEEE87, "sceKernelStopVTimer" )]
		// SDK location: /user/pspthreadman.h:1616
		// SDK declaration: int sceKernelStopVTimer(SceUID uid);
		public int sceKernelStopVTimer( int uid ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD8B299AE, "sceKernelSetVTimerHandler" )]
		// SDK location: /user/pspthreadman.h:1631
		// SDK declaration: int sceKernelSetVTimerHandler(SceUID uid, SceKernelSysClock *time, SceKernelVTimerHandler handler, void *common);
		public int sceKernelSetVTimerHandler( int uid, int time, int handler, int common ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x53B00E9A, "sceKernelSetVTimerHandlerWide" )]
		// SDK location: /user/pspthreadman.h:1643
		// SDK declaration: int sceKernelSetVTimerHandlerWide(SceUID uid, SceInt64 time, SceKernelVTimerHandlerWide handler, void *common);
		public int sceKernelSetVTimerHandlerWide( int uid, long time, int handler, int common ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD2D615EF, "sceKernelCancelVTimerHandler" )]
		// SDK location: /user/pspthreadman.h:1652
		// SDK declaration: int sceKernelCancelVTimerHandler(SceUID uid);
		public int sceKernelCancelVTimerHandler( int uid ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5F32BEAA, "sceKernelReferVTimerStatus" )]
		// SDK location: /user/pspthreadman.h:1673
		// SDK declaration: int sceKernelReferVTimerStatus(SceUID uid, SceKernelVTimerInfo *info);
		public int sceKernelReferVTimerStatus( int uid, int info ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x446D8DE6, "sceKernelCreateThread" )]
		// SDK location: /user/pspthreadman.h:169
		// SDK declaration: SceUID sceKernelCreateThread(const char *name, SceKernelThreadEntry entry, int initPriority, int stackSize, SceUInt attr, SceKernelThreadOptParam *option);
		public int sceKernelCreateThread( int name, int entry, int initPriority, int stackSize, int attr, int option ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9FA03CD3, "sceKernelDeleteThread" )]
		// SDK location: /user/pspthreadman.h:179
		// SDK declaration: int sceKernelDeleteThread(SceUID thid);
		public int sceKernelDeleteThread( int thid ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF475845D, "sceKernelStartThread" )]
		// SDK location: /user/pspthreadman.h:188
		// SDK declaration: int sceKernelStartThread(SceUID thid, SceSize arglen, void *argp);
		public int sceKernelStartThread( int thid, int arglen, int argp ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x532A522E, "_sceKernelExitThread" )]
		// SDK location: /user/pspthreadman.h:1679
		// SDK declaration: void _sceKernelExitThread();
		public void _sceKernelExitThread(){}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xAA73C935, "sceKernelExitThread" )]
		// SDK location: /user/pspthreadman.h:195
		// SDK declaration: int sceKernelExitThread(int status);
		public int sceKernelExitThread( int status ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x809CE29B, "sceKernelExitDeleteThread" )]
		// SDK location: /user/pspthreadman.h:202
		// SDK declaration: int sceKernelExitDeleteThread(int status);
		public int sceKernelExitDeleteThread( int status ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x616403BA, "sceKernelTerminateThread" )]
		// SDK location: /user/pspthreadman.h:211
		// SDK declaration: int sceKernelTerminateThread(SceUID thid);
		public int sceKernelTerminateThread( int thid ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x383F7BCC, "sceKernelTerminateDeleteThread" )]
		// SDK location: /user/pspthreadman.h:220
		// SDK declaration: int sceKernelTerminateDeleteThread(SceUID thid);
		public int sceKernelTerminateDeleteThread( int thid ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3AD58B8C, "sceKernelSuspendDispatchThread" )]
		// SDK location: /user/pspthreadman.h:227
		// SDK declaration: int sceKernelSuspendDispatchThread();
		public int sceKernelSuspendDispatchThread(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x27E22EC2, "sceKernelResumeDispatchThread" )]
		// SDK location: /user/pspthreadman.h:237
		// SDK declaration: int sceKernelResumeDispatchThread(int state);
		public int sceKernelResumeDispatchThread( int state ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xEA748E31, "sceKernelChangeCurrentThreadAttr" )]
		// SDK location: /user/pspthreadman.h:364
		// SDK declaration: int sceKernelChangeCurrentThreadAttr(int unknown, SceUInt attr);
		public int sceKernelChangeCurrentThreadAttr( int unknown, int attr ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x71BC9871, "sceKernelChangeThreadPriority" )]
		// SDK location: /user/pspthreadman.h:381
		// SDK declaration: int sceKernelChangeThreadPriority(SceUID thid, int priority);
		public int sceKernelChangeThreadPriority( int thid, int priority ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x912354A7, "sceKernelRotateThreadReadyQueue" )]
		// SDK location: /user/pspthreadman.h:390
		// SDK declaration: int sceKernelRotateThreadReadyQueue(int priority);
		public int sceKernelRotateThreadReadyQueue( int priority ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2C34E053, "sceKernelReleaseWaitThread" )]
		// SDK location: /user/pspthreadman.h:399
		// SDK declaration: int sceKernelReleaseWaitThread(SceUID thid);
		public int sceKernelReleaseWaitThread( int thid ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x293B45B8, "sceKernelGetThreadId" )]
		// SDK location: /user/pspthreadman.h:406
		// SDK declaration: int sceKernelGetThreadId();
		public int sceKernelGetThreadId(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x94AA61EE, "sceKernelGetThreadCurrentPriority" )]
		// SDK location: /user/pspthreadman.h:413
		// SDK declaration: int sceKernelGetThreadCurrentPriority();
		public int sceKernelGetThreadCurrentPriority(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3B183E26, "sceKernelGetThreadExitStatus" )]
		// SDK location: /user/pspthreadman.h:422
		// SDK declaration: int sceKernelGetThreadExitStatus(SceUID thid);
		public int sceKernelGetThreadExitStatus( int thid ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD13BDE95, "sceKernelCheckThreadStack" )]
		// SDK location: /user/pspthreadman.h:429
		// SDK declaration: int sceKernelCheckThreadStack();
		public int sceKernelCheckThreadStack(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x52089CA1, "sceKernelGetThreadStackFreeSize" )]
		// SDK location: /user/pspthreadman.h:439
		// SDK declaration: int sceKernelGetThreadStackFreeSize(SceUID thid);
		public int sceKernelGetThreadStackFreeSize( int thid ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x17C1684E, "sceKernelReferThreadStatus" )]
		// SDK location: /user/pspthreadman.h:458
		// SDK declaration: int sceKernelReferThreadStatus(SceUID thid, SceKernelThreadInfo *info);
		public int sceKernelReferThreadStatus( int thid, int info ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFFC36A14, "sceKernelReferThreadRunStatus" )]
		// SDK location: /user/pspthreadman.h:468
		// SDK declaration: int sceKernelReferThreadRunStatus(SceUID thid, SceKernelThreadRunStatus *status);
		public int sceKernelReferThreadRunStatus( int thid, int status ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x627E6F3A, "sceKernelReferSystemStatus" )]
		// SDK location: /user/pspthreadman.h:1100
		// SDK declaration: int sceKernelReferSystemStatus(SceKernelSystemStatus *status);
		public int sceKernelReferSystemStatus( int status ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x94416130, "sceKernelGetThreadmanIdList" )]
		// SDK location: /user/pspthreadman.h:1075
		// SDK declaration: int sceKernelGetThreadmanIdList(enum SceKernelIdListType type, SceUID *readbuf, int readbufsize, int *idcount);
		public int sceKernelGetThreadmanIdList( int type, int readbuf, int readbufsize, int idcount ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x57CF62DD, "sceKernelGetThreadmanIdType" )]
		// SDK location: /user/pspthreadman.h:1688
		// SDK declaration: enum SceKernelIdListType sceKernelGetThreadmanIdType(SceUID uid);
		public int sceKernelGetThreadmanIdType( int uid ){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - D8829FE8 */
