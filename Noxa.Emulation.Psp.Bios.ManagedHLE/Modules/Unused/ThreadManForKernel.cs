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
	class ThreadManForKernel : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "ThreadManForKernel";
			}
		}

		#endregion

		#region State Management

		public ThreadManForKernel( Kernel kernel )
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
		[BiosFunction( 0x0C106E53, "sceKernelRegisterThreadEventHandler" )]
		int sceKernelRegisterThreadEventHandler(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x72F3C145, "sceKernelReleaseThreadEventHandler" )]
		int sceKernelReleaseThreadEventHandler(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x369EEB6B, "sceKernelReferThreadEventHandlerStatus" )]
		int sceKernelReferThreadEventHandlerStatus(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE81CAF8F, "sceKernelCreateCallback" )]
		int sceKernelCreateCallback(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xEDBA5844, "sceKernelDeleteCallback" )]
		int sceKernelDeleteCallback(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC11BA8C4, "sceKernelNotifyCallback" )]
		int sceKernelNotifyCallback(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBA4051D6, "sceKernelCancelCallback" )]
		int sceKernelCancelCallback(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2A3D44FF, "sceKernelGetCallbackCount" )]
		int sceKernelGetCallbackCount(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x349D6D6C, "sceKernelCheckCallback" )]
		int sceKernelCheckCallback(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x730ED8BC, "sceKernelReferCallbackStatus" )]
		int sceKernelReferCallbackStatus(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9ACE131E, "sceKernelSleepThread" )]
		int sceKernelSleepThread(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x82826F70, "sceKernelSleepThreadCB" )]
		int sceKernelSleepThreadCB(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD59EAD2F, "sceKernelWakeupThread" )]
		int sceKernelWakeupThread(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFCCFAD26, "sceKernelCancelWakeupThread" )]
		int sceKernelCancelWakeupThread(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9944F31F, "sceKernelSuspendThread" )]
		int sceKernelSuspendThread(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x75156E8F, "sceKernelResumeThread" )]
		int sceKernelResumeThread(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8FD9F70C, "sceKernelSuspendAllUserThreads" )]
		int sceKernelSuspendAllUserThreads(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x278C0DF5, "sceKernelWaitThreadEnd" )]
		int sceKernelWaitThreadEnd(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x840E8133, "sceKernelWaitThreadEndCB" )]
		int sceKernelWaitThreadEndCB(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCEADEB47, "sceKernelDelayThread" )]
		int sceKernelDelayThread(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x68DA9E36, "sceKernelDelayThreadCB" )]
		int sceKernelDelayThreadCB(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBD123D9E, "sceKernelDelaySysClockThread" )]
		int sceKernelDelaySysClockThread(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1181E963, "sceKernelDelaySysClockThreadCB" )]
		int sceKernelDelaySysClockThreadCB(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD6DA4BA1, "sceKernelCreateSema" )]
		int sceKernelCreateSema(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x28B6489C, "sceKernelDeleteSema" )]
		int sceKernelDeleteSema(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3F53E640, "sceKernelSignalSema" )]
		int sceKernelSignalSema(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4E3A1105, "sceKernelWaitSema" )]
		int sceKernelWaitSema(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6D212BAC, "sceKernelWaitSemaCB" )]
		int sceKernelWaitSemaCB(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x58B1F937, "sceKernelPollSema" )]
		int sceKernelPollSema(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8FFDF9A2, "sceKernelCancelSema" )]
		int sceKernelCancelSema(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBC6FEBC5, "sceKernelReferSemaStatus" )]
		int sceKernelReferSemaStatus(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x55C20A00, "sceKernelCreateEventFlag" )]
		int sceKernelCreateEventFlag(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xEF9E4C70, "sceKernelDeleteEventFlag" )]
		int sceKernelDeleteEventFlag(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1FB15A32, "sceKernelSetEventFlag" )]
		int sceKernelSetEventFlag(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x812346E4, "sceKernelClearEventFlag" )]
		int sceKernelClearEventFlag(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x402FCF22, "sceKernelWaitEventFlag" )]
		int sceKernelWaitEventFlag(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x328C546A, "sceKernelWaitEventFlagCB" )]
		int sceKernelWaitEventFlagCB(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x30FD48F0, "sceKernelPollEventFlag" )]
		int sceKernelPollEventFlag(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCD203292, "sceKernelCancelEventFlag" )]
		int sceKernelCancelEventFlag(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA66B0120, "sceKernelReferEventFlagStatus" )]
		int sceKernelReferEventFlagStatus(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x8125221D, "sceKernelCreateMbx" )]
		int sceKernelCreateMbx(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x86255ADA, "sceKernelDeleteMbx" )]
		int sceKernelDeleteMbx(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE9B3061E, "sceKernelSendMbx" )]
		int sceKernelSendMbx(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x18260574, "sceKernelReceiveMbx" )]
		int sceKernelReceiveMbx(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF3986382, "sceKernelReceiveMbxCB" )]
		int sceKernelReceiveMbxCB(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0D81716A, "sceKernelPollMbx" )]
		int sceKernelPollMbx(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x87D4DD36, "sceKernelCancelReceiveMbx" )]
		int sceKernelCancelReceiveMbx(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA8E8C846, "sceKernelReferMbxStatus" )]
		int sceKernelReferMbxStatus(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7C0DC2A0, "sceKernelCreateMsgPipe" )]
		int sceKernelCreateMsgPipe(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF0B7DA1C, "sceKernelDeleteMsgPipe" )]
		int sceKernelDeleteMsgPipe(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x876DBFAD, "sceKernelSendMsgPipe" )]
		int sceKernelSendMsgPipe(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7C41F2C2, "sceKernelSendMsgPipeCB" )]
		int sceKernelSendMsgPipeCB(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x884C9F90, "sceKernelTrySendMsgPipe" )]
		int sceKernelTrySendMsgPipe(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x74829B76, "sceKernelReceiveMsgPipe" )]
		int sceKernelReceiveMsgPipe(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFBFA697D, "sceKernelReceiveMsgPipeCB" )]
		int sceKernelReceiveMsgPipeCB(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDF52098F, "sceKernelTryReceiveMsgPipe" )]
		int sceKernelTryReceiveMsgPipe(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x349B864D, "sceKernelCancelMsgPipe" )]
		int sceKernelCancelMsgPipe(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x33BE4024, "sceKernelReferMsgPipeStatus" )]
		int sceKernelReferMsgPipeStatus(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x56C039B5, "sceKernelCreateVpl" )]
		int sceKernelCreateVpl(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x89B3D48C, "sceKernelDeleteVpl" )]
		int sceKernelDeleteVpl(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBED27435, "sceKernelAllocateVpl" )]
		int sceKernelAllocateVpl(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xEC0A693F, "sceKernelAllocateVplCB" )]
		int sceKernelAllocateVplCB(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xAF36D708, "sceKernelTryAllocateVpl" )]
		int sceKernelTryAllocateVpl(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB736E9FF, "sceKernelFreeVpl" )]
		int sceKernelFreeVpl(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x1D371B8A, "sceKernelCancelVpl" )]
		int sceKernelCancelVpl(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x39810265, "sceKernelReferVplStatus" )]
		int sceKernelReferVplStatus(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC07BB470, "sceKernelCreateFpl" )]
		int sceKernelCreateFpl(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xED1410E0, "sceKernelDeleteFpl" )]
		int sceKernelDeleteFpl(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD979E9BF, "sceKernelAllocateFpl" )]
		int sceKernelAllocateFpl(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE7282CB6, "sceKernelAllocateFplCB" )]
		int sceKernelAllocateFplCB(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x623AE665, "sceKernelTryAllocateFpl" )]
		int sceKernelTryAllocateFpl(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF6414A71, "sceKernelFreeFpl" )]
		int sceKernelFreeFpl(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA8AA591F, "sceKernelCancelFpl" )]
		int sceKernelCancelFpl(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD8199E4C, "sceKernelReferFplStatus" )]
		int sceKernelReferFplStatus(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x110DEC9A, "sceKernelUSec2SysClock" )]
		int sceKernelUSec2SysClock(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC8CD158C, "sceKernelUSec2SysClockWide" )]
		int sceKernelUSec2SysClockWide(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBA6B92E2, "sceKernelSysClock2USec" )]
		int sceKernelSysClock2USec(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE1619D7C, "sceKernelSysClock2USecWide" )]
		int sceKernelSysClock2USecWide(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDB738F35, "sceKernelGetSystemTime" )]
		int sceKernelGetSystemTime(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x82BC5777, "sceKernelGetSystemTimeWide" )]
		int sceKernelGetSystemTimeWide(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x369ED59D, "sceKernelGetSystemTimeLow" )]
		int sceKernelGetSystemTimeLow(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6652B8CA, "sceKernelSetAlarm" )]
		int sceKernelSetAlarm(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB2C25152, "sceKernelSetSysClockAlarm" )]
		int sceKernelSetSysClockAlarm(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7E65B999, "sceKernelCancelAlarm" )]
		int sceKernelCancelAlarm(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDAA3F564, "sceKernelReferAlarmStatus" )]
		int sceKernelReferAlarmStatus(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x20FFF560, "sceKernelCreateVTimer" )]
		int sceKernelCreateVTimer(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x328F9E52, "sceKernelDeleteVTimer" )]
		int sceKernelDeleteVTimer(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB3A59970, "sceKernelGetVTimerBase" )]
		int sceKernelGetVTimerBase(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB7C18B77, "sceKernelGetVTimerBaseWide" )]
		int sceKernelGetVTimerBaseWide(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x034A921F, "sceKernelGetVTimerTime" )]
		int sceKernelGetVTimerTime(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC0B3FFD2, "sceKernelGetVTimerTimeWide" )]
		int sceKernelGetVTimerTimeWide(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x542AD630, "sceKernelSetVTimerTime" )]
		int sceKernelSetVTimerTime(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFB6425C3, "sceKernelSetVTimerTimeWide" )]
		int sceKernelSetVTimerTimeWide(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC68D9437, "sceKernelStartVTimer" )]
		int sceKernelStartVTimer(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD0AEEE87, "sceKernelStopVTimer" )]
		int sceKernelStopVTimer(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD8B299AE, "sceKernelSetVTimerHandler" )]
		int sceKernelSetVTimerHandler(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x53B00E9A, "sceKernelSetVTimerHandlerWide" )]
		int sceKernelSetVTimerHandlerWide(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD2D615EF, "sceKernelCancelVTimerHandler" )]
		int sceKernelCancelVTimerHandler(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x5F32BEAA, "sceKernelReferVTimerStatus" )]
		int sceKernelReferVTimerStatus(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x04E72261, "sceKernelAllocateKTLS" )]
		int sceKernelAllocateKTLS(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD198B811, "sceKernelFreeKTLS" )]
		int sceKernelFreeKTLS(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3AD875C3, "sceKernelGetThreadKTLS" )]
		int sceKernelGetThreadKTLS(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xA249EAAE, "sceKernelGetKTLS" )]
		int sceKernelGetKTLS(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xB50F4E46, "ThreadManForKernel_B50F4E46" )]
		int ThreadManForKernel_B50F4E46(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x446D8DE6, "sceKernelCreateThread" )]
		int sceKernelCreateThread(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9FA03CD3, "sceKernelDeleteThread" )]
		int sceKernelDeleteThread(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF475845D, "sceKernelStartThread" )]
		int sceKernelStartThread(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xAA73C935, "sceKernelExitThread" )]
		int sceKernelExitThread(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x809CE29B, "sceKernelExitDeleteThread" )]
		int sceKernelExitDeleteThread(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x616403BA, "sceKernelTerminateThread" )]
		int sceKernelTerminateThread(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x383F7BCC, "sceKernelTerminateDeleteThread" )]
		int sceKernelTerminateDeleteThread(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3AD58B8C, "sceKernelSuspendDispatchThread" )]
		int sceKernelSuspendDispatchThread(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x27E22EC2, "sceKernelResumeDispatchThread" )]
		int sceKernelResumeDispatchThread(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xEA748E31, "sceKernelChangeCurrentThreadAttr" )]
		int sceKernelChangeCurrentThreadAttr(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x71BC9871, "sceKernelChangeThreadPriority" )]
		int sceKernelChangeThreadPriority(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x912354A7, "sceKernelRotateThreadReadyQueue" )]
		int sceKernelRotateThreadReadyQueue(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2C34E053, "sceKernelReleaseWaitThread" )]
		int sceKernelReleaseWaitThread(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x293B45B8, "sceKernelGetThreadId" )]
		int sceKernelGetThreadId(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x94AA61EE, "sceKernelGetThreadCurrentPriority" )]
		int sceKernelGetThreadCurrentPriority(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3B183E26, "sceKernelGetThreadExitStatus" )]
		int sceKernelGetThreadExitStatus(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF6427665, "sceKernelGetUserLevel" )]
		int sceKernelGetUserLevel(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x85A2A5BF, "sceKernelIsUserModeThread" )]
		int sceKernelIsUserModeThread(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDD55A192, "sceKernelGetSyscallRA" )]
		int sceKernelGetSyscallRA(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xBC31C1B9, "sceKernelExtendKernelStack" )]
		int sceKernelExtendKernelStack(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4FE44D5E, "sceKernelCheckThreadKernelStack" )]
		int sceKernelCheckThreadKernelStack(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD13BDE95, "sceKernelCheckThreadStack" )]
		int sceKernelCheckThreadStack(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x52089CA1, "sceKernelGetThreadStackFreeSize" )]
		int sceKernelGetThreadStackFreeSize(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xD890B370, "sceKernelGetThreadKernelStackFreeSize" )]
		int sceKernelGetThreadKernelStackFreeSize(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x17C1684E, "sceKernelReferThreadStatus" )]
		int sceKernelReferThreadStatus(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFFC36A14, "sceKernelReferThreadRunStatus" )]
		int sceKernelReferThreadRunStatus(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2D69D086, "ThreadManForKernel_2D69D086" )]
		int ThreadManForKernel_2D69D086(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xFCB5EB49, "sceKernelGetSystemStatusFlag" )]
		int sceKernelGetSystemStatusFlag(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x627E6F3A, "sceKernelReferSystemStatus" )]
		int sceKernelReferSystemStatus(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x94416130, "sceKernelGetThreadmanIdList" )]
		int sceKernelGetThreadmanIdList(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x57CF62DD, "sceKernelGetThreadmanIdType" )]
		int sceKernelGetThreadmanIdType(){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - BCEEA0C5 */
