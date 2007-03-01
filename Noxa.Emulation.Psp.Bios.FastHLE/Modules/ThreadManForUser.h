// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

#include "NoxaShared.h"
#include "ModulesShared.h"
#include "Module.h"

using namespace System;
using namespace System::Diagnostics;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Bios;

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace Bios {
				namespace Modules {

					ref class ThreadManForUser : public Module
					{
					public:
						ThreadManForUser( Kernel^ kernel ) : Module( kernel ) {}
						~ThreadManForUser(){}

						property String^ Name { virtual String^ get() override { return "ThreadManForUser"; } }

						//virtual void Start() override;
						//virtual void Stop() override;
						//virtual void Clear() override;

					internal:
						//virtual void* QueryNativePointer( uint nid ) override;

					public: // ------ Implemented calls ------

					public: // ------ Stubbed calls ------

						[NotImplemented]
						[BiosFunction( 0x6E9EA350, "_sceKernelReturnFromCallback" )] [Stateless]
						// /user/pspthreadman.h:1453: void _sceKernelReturnFromCallback();
						void _sceKernelReturnFromCallback(){}

						[NotImplemented]
						[BiosFunction( 0x0C106E53, "sceKernelRegisterThreadEventHandler" )] [Stateless]
						// /user/pspthreadman.h:1729: SceUID sceKernelRegisterThreadEventHandler(const char *name, SceUID threadID, int mask, SceKernelThreadEventHandler handler, void *common);
						int sceKernelRegisterThreadEventHandler( int name, int threadID, int mask, int handler, int common ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x72F3C145, "sceKernelReleaseThreadEventHandler" )] [Stateless]
						// /user/pspthreadman.h:1738: int sceKernelReleaseThreadEventHandler(SceUID uid);
						int sceKernelReleaseThreadEventHandler( int uid ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x369EEB6B, "sceKernelReferThreadEventHandlerStatus" )] [Stateless]
						// /user/pspthreadman.h:1748: int sceKernelReferThreadEventHandlerStatus(SceUID uid, struct SceKernelThreadEventHandlerInfo *info);
						int sceKernelReferThreadEventHandlerStatus( int uid, int info ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xE81CAF8F, "sceKernelCreateCallback" )] [Stateless]
						// /user/pspthreadman.h:985: int sceKernelCreateCallback(const char *name, SceKernelCallbackFunction func, void *arg);
						int sceKernelCreateCallback( int name, int func, int arg ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xEDBA5844, "sceKernelDeleteCallback" )] [Stateless]
						// /user/pspthreadman.h:1005: int sceKernelDeleteCallback(SceUID cb);
						int sceKernelDeleteCallback( int cb ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xC11BA8C4, "sceKernelNotifyCallback" )] [Stateless]
						// /user/pspthreadman.h:1015: int sceKernelNotifyCallback(SceUID cb, int arg2);
						int sceKernelNotifyCallback( int cb, int arg2 ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xBA4051D6, "sceKernelCancelCallback" )] [Stateless]
						// /user/pspthreadman.h:1024: int sceKernelCancelCallback(SceUID cb);
						int sceKernelCancelCallback( int cb ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x2A3D44FF, "sceKernelGetCallbackCount" )] [Stateless]
						// /user/pspthreadman.h:1033: int sceKernelGetCallbackCount(SceUID cb);
						int sceKernelGetCallbackCount( int cb ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x349D6D6C, "sceKernelCheckCallback" )] [Stateless]
						// /user/pspthreadman.h:1040: int sceKernelCheckCallback();
						int sceKernelCheckCallback(){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x730ED8BC, "sceKernelReferCallbackStatus" )] [Stateless]
						// /user/pspthreadman.h:996: int sceKernelReferCallbackStatus(SceUID cb, SceKernelCallbackInfo *status);
						int sceKernelReferCallbackStatus( int cb, int status ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x9ACE131E, "sceKernelSleepThread" )] [Stateless]
						// /user/pspthreadman.h:244: int sceKernelSleepThread();
						int sceKernelSleepThread(){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x82826F70, "sceKernelSleepThreadCB" )] [Stateless]
						// /user/pspthreadman.h:255: int sceKernelSleepThreadCB();
						int sceKernelSleepThreadCB(){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xD59EAD2F, "sceKernelWakeupThread" )] [Stateless]
						// /user/pspthreadman.h:264: int sceKernelWakeupThread(SceUID thid);
						int sceKernelWakeupThread( int thid ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xFCCFAD26, "sceKernelCancelWakeupThread" )] [Stateless]
						// /user/pspthreadman.h:273: int sceKernelCancelWakeupThread(SceUID thid);
						int sceKernelCancelWakeupThread( int thid ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x9944F31F, "sceKernelSuspendThread" )] [Stateless]
						// /user/pspthreadman.h:282: int sceKernelSuspendThread(SceUID thid);
						int sceKernelSuspendThread( int thid ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x75156E8F, "sceKernelResumeThread" )] [Stateless]
						// /user/pspthreadman.h:291: int sceKernelResumeThread(SceUID thid);
						int sceKernelResumeThread( int thid ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x278C0DF5, "sceKernelWaitThreadEnd" )] [Stateless]
						// /user/pspthreadman.h:301: int sceKernelWaitThreadEnd(SceUID thid, SceUInt *timeout);
						int sceKernelWaitThreadEnd( int thid, int timeout ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x840E8133, "sceKernelWaitThreadEndCB" )] [Stateless]
						// /user/pspthreadman.h:311: int sceKernelWaitThreadEndCB(SceUID thid, SceUInt *timeout);
						int sceKernelWaitThreadEndCB( int thid, int timeout ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xCEADEB47, "sceKernelDelayThread" )] [Stateless]
						// /user/pspthreadman.h:323: int sceKernelDelayThread(SceUInt delay);
						int sceKernelDelayThread( int delay ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x68DA9E36, "sceKernelDelayThreadCB" )] [Stateless]
						// /user/pspthreadman.h:335: int sceKernelDelayThreadCB(SceUInt delay);
						int sceKernelDelayThreadCB( int delay ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xBD123D9E, "sceKernelDelaySysClockThread" )] [Stateless]
						// /user/pspthreadman.h:344: int sceKernelDelaySysClockThread(SceKernelSysClock *delay);
						int sceKernelDelaySysClockThread( int delay ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x1181E963, "sceKernelDelaySysClockThreadCB" )] [Stateless]
						// /user/pspthreadman.h:354: int sceKernelDelaySysClockThreadCB(SceKernelSysClock *delay);
						int sceKernelDelaySysClockThreadCB( int delay ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xD6DA4BA1, "sceKernelCreateSema" )] [Stateless]
						// /user/pspthreadman.h:515: SceUID sceKernelCreateSema(const char *name, SceUInt attr, int initVal, int maxVal, SceKernelSemaOptParam *option);
						int sceKernelCreateSema( int name, int attr, int initVal, int maxVal, int option ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x28B6489C, "sceKernelDeleteSema" )] [Stateless]
						// /user/pspthreadman.h:523: int sceKernelDeleteSema(SceUID semaid);
						int sceKernelDeleteSema( int semaid ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x3F53E640, "sceKernelSignalSema" )] [Stateless]
						// /user/pspthreadman.h:539: int sceKernelSignalSema(SceUID semaid, int signal);
						int sceKernelSignalSema( int semaid, int signal ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x4E3A1105, "sceKernelWaitSema" )] [Stateless]
						// /user/pspthreadman.h:555: int sceKernelWaitSema(SceUID semaid, int signal, SceUInt *timeout);
						int sceKernelWaitSema( int semaid, int signal, int timeout ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x6D212BAC, "sceKernelWaitSemaCB" )] [Stateless]
						// /user/pspthreadman.h:571: int sceKernelWaitSemaCB(SceUID semaid, int signal, SceUInt *timeout);
						int sceKernelWaitSemaCB( int semaid, int signal, int timeout ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x58B1F937, "sceKernelPollSema" )] [Stateless]
						// /user/pspthreadman.h:581: int sceKernelPollSema(SceUID semaid, int signal);
						int sceKernelPollSema( int semaid, int signal ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xBC6FEBC5, "sceKernelReferSemaStatus" )] [Stateless]
						// /user/pspthreadman.h:591: int sceKernelReferSemaStatus(SceUID semaid, SceKernelSemaInfo *info);
						int sceKernelReferSemaStatus( int semaid, int info ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x55C20A00, "sceKernelCreateEventFlag" )] [Stateless]
						// /user/pspthreadman.h:645: SceUID sceKernelCreateEventFlag(const char *name, int attr, int bits, SceKernelEventFlagOptParam *opt);
						int sceKernelCreateEventFlag( int name, int attr, int bits, int opt ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xEF9E4C70, "sceKernelDeleteEventFlag" )] [Stateless]
						// /user/pspthreadman.h:709: int sceKernelDeleteEventFlag(int evid);
						int sceKernelDeleteEventFlag( int evid ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x1FB15A32, "sceKernelSetEventFlag" )] [Stateless]
						// /user/pspthreadman.h:655: int sceKernelSetEventFlag(SceUID evid, u32 bits);
						int sceKernelSetEventFlag( int evid, int bits ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x812346E4, "sceKernelClearEventFlag" )] [Stateless]
						// /user/pspthreadman.h:665: int sceKernelClearEventFlag(SceUID evid, u32 bits);
						int sceKernelClearEventFlag( int evid, int bits ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x402FCF22, "sceKernelWaitEventFlag" )] [Stateless]
						// /user/pspthreadman.h:688: int sceKernelWaitEventFlag(int evid, u32 bits, u32 wait, u32 *outBits, SceUInt *timeout);
						int sceKernelWaitEventFlag( int evid, int bits, int wait, int outBits, int timeout ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x328C546A, "sceKernelWaitEventFlagCB" )] [Stateless]
						// /user/pspthreadman.h:700: int sceKernelWaitEventFlagCB(int evid, u32 bits, u32 wait, u32 *outBits, SceUInt *timeout);
						int sceKernelWaitEventFlagCB( int evid, int bits, int wait, int outBits, int timeout ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x30FD48F0, "sceKernelPollEventFlag" )] [Stateless]
						// /user/pspthreadman.h:676: int sceKernelPollEventFlag(int evid, u32 bits, u32 wait, u32 *outBits);
						int sceKernelPollEventFlag( int evid, int bits, int wait, int outBits ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xA66B0120, "sceKernelReferEventFlagStatus" )] [Stateless]
						// /user/pspthreadman.h:719: int sceKernelReferEventFlagStatus(SceUID event, SceKernelEventFlagInfo *status);
						int sceKernelReferEventFlagStatus( int event, int status ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x8125221D, "sceKernelCreateMbx" )] [Stateless]
						// /user/pspthreadman.h:774: SceUID sceKernelCreateMbx(const char *name, SceUInt attr, SceKernelMbxOptParam *option);
						int sceKernelCreateMbx( int name, int attr, int option ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x86255ADA, "sceKernelDeleteMbx" )] [Stateless]
						// /user/pspthreadman.h:782: int sceKernelDeleteMbx(SceUID mbxid);
						int sceKernelDeleteMbx( int mbxid ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xE9B3061E, "sceKernelSendMbx" )] [Stateless]
						// /user/pspthreadman.h:806: int sceKernelSendMbx(SceUID mbxid, void *message);
						int sceKernelSendMbx( int mbxid, int message ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x18260574, "sceKernelReceiveMbx" )] [Stateless]
						// /user/pspthreadman.h:824: int sceKernelReceiveMbx(SceUID mbxid, void **pmessage, SceUInt *timeout);
						int sceKernelReceiveMbx( int mbxid, int pmessage, int timeout ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xF3986382, "sceKernelReceiveMbxCB" )] [Stateless]
						// /user/pspthreadman.h:842: int sceKernelReceiveMbxCB(SceUID mbxid, void **pmessage, SceUInt *timeout);
						int sceKernelReceiveMbxCB( int mbxid, int pmessage, int timeout ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x0D81716A, "sceKernelPollMbx" )] [Stateless]
						// /user/pspthreadman.h:859: int sceKernelPollMbx(SceUID mbxid, void **pmessage);
						int sceKernelPollMbx( int mbxid, int pmessage ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x87D4DD36, "sceKernelCancelReceiveMbx" )] [Stateless]
						// /user/pspthreadman.h:876: int sceKernelCancelReceiveMbx(SceUID mbxid, int *pnum);
						int sceKernelCancelReceiveMbx( int mbxid, int pnum ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xA8E8C846, "sceKernelReferMbxStatus" )] [Stateless]
						// /user/pspthreadman.h:886: int sceKernelReferMbxStatus(SceUID mbxid, SceKernelMbxInfo *info);
						int sceKernelReferMbxStatus( int mbxid, int info ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x7C0DC2A0, "sceKernelCreateMsgPipe" )] [Stateless]
						// /user/pspthreadman.h:1114: SceUID sceKernelCreateMsgPipe(const char *name, int part, int attr, void *unk1, void *opt);
						int sceKernelCreateMsgPipe( int name, int part, int attr, int unk1, int opt ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xF0B7DA1C, "sceKernelDeleteMsgPipe" )] [Stateless]
						// /user/pspthreadman.h:1123: int sceKernelDeleteMsgPipe(SceUID uid);
						int sceKernelDeleteMsgPipe( int uid ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x876DBFAD, "sceKernelSendMsgPipe" )] [Stateless]
						// /user/pspthreadman.h:1137: int sceKernelSendMsgPipe(SceUID uid, void *message, unsigned int size, int unk1, void *unk2, unsigned int *timeout);
						int sceKernelSendMsgPipe( int uid, int message, int size, int unk1, int unk2, int timeout ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x7C41F2C2, "sceKernelSendMsgPipeCB" )] [Stateless]
						// /user/pspthreadman.h:1151: int sceKernelSendMsgPipeCB(SceUID uid, void *message, unsigned int size, int unk1, void *unk2, unsigned int *timeout);
						int sceKernelSendMsgPipeCB( int uid, int message, int size, int unk1, int unk2, int timeout ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x884C9F90, "sceKernelTrySendMsgPipe" )] [Stateless]
						// /user/pspthreadman.h:1164: int sceKernelTrySendMsgPipe(SceUID uid, void *message, unsigned int size, int unk1, void *unk2);
						int sceKernelTrySendMsgPipe( int uid, int message, int size, int unk1, int unk2 ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x74829B76, "sceKernelReceiveMsgPipe" )] [Stateless]
						// /user/pspthreadman.h:1178: int sceKernelReceiveMsgPipe(SceUID uid, void *message, unsigned int size, int unk1, void *unk2, unsigned int *timeout);
						int sceKernelReceiveMsgPipe( int uid, int message, int size, int unk1, int unk2, int timeout ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xFBFA697D, "sceKernelReceiveMsgPipeCB" )] [Stateless]
						// /user/pspthreadman.h:1192: int sceKernelReceiveMsgPipeCB(SceUID uid, void *message, unsigned int size, int unk1, void *unk2, unsigned int *timeout);
						int sceKernelReceiveMsgPipeCB( int uid, int message, int size, int unk1, int unk2, int timeout ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xDF52098F, "sceKernelTryReceiveMsgPipe" )] [Stateless]
						// /user/pspthreadman.h:1205: int sceKernelTryReceiveMsgPipe(SceUID uid, void *message, unsigned int size, int unk1, void *unk2);
						int sceKernelTryReceiveMsgPipe( int uid, int message, int size, int unk1, int unk2 ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x349B864D, "sceKernelCancelMsgPipe" )] [Stateless]
						// /user/pspthreadman.h:1216: int sceKernelCancelMsgPipe(SceUID uid, int *psend, int *precv);
						int sceKernelCancelMsgPipe( int uid, int psend, int precv ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x33BE4024, "sceKernelReferMsgPipeStatus" )] [Stateless]
						// /user/pspthreadman.h:1237: int sceKernelReferMsgPipeStatus(SceUID uid, SceKernelMppInfo *info);
						int sceKernelReferMsgPipeStatus( int uid, int info ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x56C039B5, "sceKernelCreateVpl" )] [Stateless]
						// /user/pspthreadman.h:1256: SceUID sceKernelCreateVpl(const char *name, int part, int attr, unsigned int size, struct SceKernelVplOptParam *opt);
						int sceKernelCreateVpl( int name, int part, int attr, int size, int opt ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x89B3D48C, "sceKernelDeleteVpl" )] [Stateless]
						// /user/pspthreadman.h:1265: int sceKernelDeleteVpl(SceUID uid);
						int sceKernelDeleteVpl( int uid ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xBED27435, "sceKernelAllocateVpl" )] [Stateless]
						// /user/pspthreadman.h:1277: int sceKernelAllocateVpl(SceUID uid, unsigned int size, void **data, unsigned int *timeout);
						int sceKernelAllocateVpl( int uid, int size, int data, int timeout ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xEC0A693F, "sceKernelAllocateVplCB" )] [Stateless]
						// /user/pspthreadman.h:1289: int sceKernelAllocateVplCB(SceUID uid, unsigned int size, void **data, unsigned int *timeout);
						int sceKernelAllocateVplCB( int uid, int size, int data, int timeout ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xAF36D708, "sceKernelTryAllocateVpl" )] [Stateless]
						// /user/pspthreadman.h:1300: int sceKernelTryAllocateVpl(SceUID uid, unsigned int size, void **data);
						int sceKernelTryAllocateVpl( int uid, int size, int data ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xB736E9FF, "sceKernelFreeVpl" )] [Stateless]
						// /user/pspthreadman.h:1310: int sceKernelFreeVpl(SceUID uid, void *data);
						int sceKernelFreeVpl( int uid, int data ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x1D371B8A, "sceKernelCancelVpl" )] [Stateless]
						// /user/pspthreadman.h:1320: int sceKernelCancelVpl(SceUID uid, int *pnum);
						int sceKernelCancelVpl( int uid, int pnum ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x39810265, "sceKernelReferVplStatus" )] [Stateless]
						// /user/pspthreadman.h:1340: int sceKernelReferVplStatus(SceUID uid, SceKernelVplInfo *info);
						int sceKernelReferVplStatus( int uid, int info ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xC07BB470, "sceKernelCreateFpl" )] [Stateless]
						// /user/pspthreadman.h:1360: int sceKernelCreateFpl(const char *name, int part, int attr, unsigned int size, unsigned int blocks, struct SceKernelFplOptParam *opt);
						int sceKernelCreateFpl( int name, int part, int attr, int size, int blocks, int opt ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xED1410E0, "sceKernelDeleteFpl" )] [Stateless]
						// /user/pspthreadman.h:1369: int sceKernelDeleteFpl(SceUID uid);
						int sceKernelDeleteFpl( int uid ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xD979E9BF, "sceKernelAllocateFpl" )] [Stateless]
						// /user/pspthreadman.h:1380: int sceKernelAllocateFpl(SceUID uid, void **data, unsigned int *timeout);
						int sceKernelAllocateFpl( int uid, int data, int timeout ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xE7282CB6, "sceKernelAllocateFplCB" )] [Stateless]
						// /user/pspthreadman.h:1391: int sceKernelAllocateFplCB(SceUID uid, void **data, unsigned int *timeout);
						int sceKernelAllocateFplCB( int uid, int data, int timeout ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x623AE665, "sceKernelTryAllocateFpl" )] [Stateless]
						// /user/pspthreadman.h:1401: int sceKernelTryAllocateFpl(SceUID uid, void **data);
						int sceKernelTryAllocateFpl( int uid, int data ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xF6414A71, "sceKernelFreeFpl" )] [Stateless]
						// /user/pspthreadman.h:1411: int sceKernelFreeFpl(SceUID uid, void *data);
						int sceKernelFreeFpl( int uid, int data ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xA8AA591F, "sceKernelCancelFpl" )] [Stateless]
						// /user/pspthreadman.h:1421: int sceKernelCancelFpl(SceUID uid, int *pnum);
						int sceKernelCancelFpl( int uid, int pnum ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xD8199E4C, "sceKernelReferFplStatus" )] [Stateless]
						// /user/pspthreadman.h:1442: int sceKernelReferFplStatus(SceUID uid, SceKernelFplInfo *info);
						int sceKernelReferFplStatus( int uid, int info ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x0E927AED, "_sceKernelReturnFromTimerHandler" )] [Stateless]
						// /user/pspthreadman.h:1447: void _sceKernelReturnFromTimerHandler();
						void _sceKernelReturnFromTimerHandler(){}

						[NotImplemented]
						[BiosFunction( 0x110DEC9A, "sceKernelUSec2SysClock" )] [Stateless]
						// /user/pspthreadman.h:1463: int sceKernelUSec2SysClock(unsigned int usec, SceKernelSysClock *clock);
						int sceKernelUSec2SysClock( int usec, int clock ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xC8CD158C, "sceKernelUSec2SysClockWide" )] [Stateless]
						// /user/pspthreadman.h:1472: SceInt64 sceKernelUSec2SysClockWide(unsigned int usec);
						int64 sceKernelUSec2SysClockWide( int usec ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xBA6B92E2, "sceKernelSysClock2USec" )] [Stateless]
						// /user/pspthreadman.h:1483: int sceKernelSysClock2USec(SceKernelSysClock *clock, unsigned int *low, unsigned int *high);
						int sceKernelSysClock2USec( int clock, int low, int high ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xE1619D7C, "sceKernelSysClock2USecWide" )] [Stateless]
						// /user/pspthreadman.h:1494: int sceKernelSysClock2USecWide(SceInt64 clock, unsigned *low, unsigned int *high);
						int sceKernelSysClock2USecWide( int64 clock, int low, int high ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xDB738F35, "sceKernelGetSystemTime" )] [Stateless]
						// /user/pspthreadman.h:1503: int sceKernelGetSystemTime(SceKernelSysClock *time);
						int sceKernelGetSystemTime( int time ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x82BC5777, "sceKernelGetSystemTimeWide" )] [Stateless]
						// /user/pspthreadman.h:1510: SceInt64 sceKernelGetSystemTimeWide();
						int64 sceKernelGetSystemTimeWide(){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x369ED59D, "sceKernelGetSystemTimeLow" )] [Stateless]
						// /user/pspthreadman.h:1517: unsigned int sceKernelGetSystemTimeLow();
						int sceKernelGetSystemTimeLow(){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x6652B8CA, "sceKernelSetAlarm" )] [Stateless]
						// /user/pspthreadman.h:915: SceUID sceKernelSetAlarm(SceUInt clock, SceKernelAlarmHandler handler, void *common);
						int sceKernelSetAlarm( int clock, int handler, int common ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xB2C25152, "sceKernelSetSysClockAlarm" )] [Stateless]
						// /user/pspthreadman.h:926: SceUID sceKernelSetSysClockAlarm(SceKernelSysClock *clock, SceKernelAlarmHandler handler, void *common);
						int sceKernelSetSysClockAlarm( int clock, int handler, int common ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x7E65B999, "sceKernelCancelAlarm" )] [Stateless]
						// /user/pspthreadman.h:935: int sceKernelCancelAlarm(SceUID alarmid);
						int sceKernelCancelAlarm( int alarmid ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xDAA3F564, "sceKernelReferAlarmStatus" )] [Stateless]
						// /user/pspthreadman.h:945: int sceKernelReferAlarmStatus(SceUID alarmid, SceKernelAlarmInfo *info);
						int sceKernelReferAlarmStatus( int alarmid, int info ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x20FFF560, "sceKernelCreateVTimer" )] [Stateless]
						// /user/pspthreadman.h:1531: SceUID sceKernelCreateVTimer(const char *name, struct SceKernelVTimerOptParam *opt);
						int sceKernelCreateVTimer( int name, int opt ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x328F9E52, "sceKernelDeleteVTimer" )] [Stateless]
						// /user/pspthreadman.h:1540: int sceKernelDeleteVTimer(SceUID uid);
						int sceKernelDeleteVTimer( int uid ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xB3A59970, "sceKernelGetVTimerBase" )] [Stateless]
						// /user/pspthreadman.h:1550: int sceKernelGetVTimerBase(SceUID uid, SceKernelSysClock *base);
						int sceKernelGetVTimerBase( int uid, int base ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xB7C18B77, "sceKernelGetVTimerBaseWide" )] [Stateless]
						// /user/pspthreadman.h:1559: SceInt64 sceKernelGetVTimerBaseWide(SceUID uid);
						int64 sceKernelGetVTimerBaseWide( int uid ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x034A921F, "sceKernelGetVTimerTime" )] [Stateless]
						// /user/pspthreadman.h:1569: int sceKernelGetVTimerTime(SceUID uid, SceKernelSysClock *time);
						int sceKernelGetVTimerTime( int uid, int time ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xC0B3FFD2, "sceKernelGetVTimerTimeWide" )] [Stateless]
						// /user/pspthreadman.h:1578: SceInt64 sceKernelGetVTimerTimeWide(SceUID uid);
						int64 sceKernelGetVTimerTimeWide( int uid ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x542AD630, "sceKernelSetVTimerTime" )] [Stateless]
						// /user/pspthreadman.h:1588: int sceKernelSetVTimerTime(SceUID uid, SceKernelSysClock *time);
						int sceKernelSetVTimerTime( int uid, int time ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xFB6425C3, "sceKernelSetVTimerTimeWide" )] [Stateless]
						// /user/pspthreadman.h:1598: SceInt64 sceKernelSetVTimerTimeWide(SceUID uid, SceInt64 time);
						int64 sceKernelSetVTimerTimeWide( int uid, int64 time ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xC68D9437, "sceKernelStartVTimer" )] [Stateless]
						// /user/pspthreadman.h:1607: int sceKernelStartVTimer(SceUID uid);
						int sceKernelStartVTimer( int uid ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xD0AEEE87, "sceKernelStopVTimer" )] [Stateless]
						// /user/pspthreadman.h:1616: int sceKernelStopVTimer(SceUID uid);
						int sceKernelStopVTimer( int uid ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xD8B299AE, "sceKernelSetVTimerHandler" )] [Stateless]
						// /user/pspthreadman.h:1631: int sceKernelSetVTimerHandler(SceUID uid, SceKernelSysClock *time, SceKernelVTimerHandler handler, void *common);
						int sceKernelSetVTimerHandler( int uid, int time, int handler, int common ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x53B00E9A, "sceKernelSetVTimerHandlerWide" )] [Stateless]
						// /user/pspthreadman.h:1643: int sceKernelSetVTimerHandlerWide(SceUID uid, SceInt64 time, SceKernelVTimerHandlerWide handler, void *common);
						int sceKernelSetVTimerHandlerWide( int uid, int64 time, int handler, int common ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xD2D615EF, "sceKernelCancelVTimerHandler" )] [Stateless]
						// /user/pspthreadman.h:1652: int sceKernelCancelVTimerHandler(SceUID uid);
						int sceKernelCancelVTimerHandler( int uid ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x5F32BEAA, "sceKernelReferVTimerStatus" )] [Stateless]
						// /user/pspthreadman.h:1673: int sceKernelReferVTimerStatus(SceUID uid, SceKernelVTimerInfo *info);
						int sceKernelReferVTimerStatus( int uid, int info ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x446D8DE6, "sceKernelCreateThread" )] [Stateless]
						// /user/pspthreadman.h:169: SceUID sceKernelCreateThread(const char *name, SceKernelThreadEntry entry, int initPriority, int stackSize, SceUInt attr, SceKernelThreadOptParam *option);
						int sceKernelCreateThread( int name, int entry, int initPriority, int stackSize, int attr, int option ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x9FA03CD3, "sceKernelDeleteThread" )] [Stateless]
						// /user/pspthreadman.h:179: int sceKernelDeleteThread(SceUID thid);
						int sceKernelDeleteThread( int thid ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xF475845D, "sceKernelStartThread" )] [Stateless]
						// /user/pspthreadman.h:188: int sceKernelStartThread(SceUID thid, SceSize arglen, void *argp);
						int sceKernelStartThread( int thid, int arglen, int argp ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x532A522E, "_sceKernelExitThread" )] [Stateless]
						// /user/pspthreadman.h:1679: void _sceKernelExitThread();
						void _sceKernelExitThread(){}

						[NotImplemented]
						[BiosFunction( 0xAA73C935, "sceKernelExitThread" )] [Stateless]
						// /user/pspthreadman.h:195: int sceKernelExitThread(int status);
						int sceKernelExitThread( int status ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x809CE29B, "sceKernelExitDeleteThread" )] [Stateless]
						// /user/pspthreadman.h:202: int sceKernelExitDeleteThread(int status);
						int sceKernelExitDeleteThread( int status ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x616403BA, "sceKernelTerminateThread" )] [Stateless]
						// /user/pspthreadman.h:211: int sceKernelTerminateThread(SceUID thid);
						int sceKernelTerminateThread( int thid ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x383F7BCC, "sceKernelTerminateDeleteThread" )] [Stateless]
						// /user/pspthreadman.h:220: int sceKernelTerminateDeleteThread(SceUID thid);
						int sceKernelTerminateDeleteThread( int thid ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x3AD58B8C, "sceKernelSuspendDispatchThread" )] [Stateless]
						// /user/pspthreadman.h:227: int sceKernelSuspendDispatchThread();
						int sceKernelSuspendDispatchThread(){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x27E22EC2, "sceKernelResumeDispatchThread" )] [Stateless]
						// /user/pspthreadman.h:237: int sceKernelResumeDispatchThread(int state);
						int sceKernelResumeDispatchThread( int state ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xEA748E31, "sceKernelChangeCurrentThreadAttr" )] [Stateless]
						// /user/pspthreadman.h:364: int sceKernelChangeCurrentThreadAttr(int unknown, SceUInt attr);
						int sceKernelChangeCurrentThreadAttr( int unknown, int attr ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x71BC9871, "sceKernelChangeThreadPriority" )] [Stateless]
						// /user/pspthreadman.h:381: int sceKernelChangeThreadPriority(SceUID thid, int priority);
						int sceKernelChangeThreadPriority( int thid, int priority ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x912354A7, "sceKernelRotateThreadReadyQueue" )] [Stateless]
						// /user/pspthreadman.h:390: int sceKernelRotateThreadReadyQueue(int priority);
						int sceKernelRotateThreadReadyQueue( int priority ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x2C34E053, "sceKernelReleaseWaitThread" )] [Stateless]
						// /user/pspthreadman.h:399: int sceKernelReleaseWaitThread(SceUID thid);
						int sceKernelReleaseWaitThread( int thid ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x293B45B8, "sceKernelGetThreadId" )] [Stateless]
						// /user/pspthreadman.h:406: int sceKernelGetThreadId();
						int sceKernelGetThreadId(){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x94AA61EE, "sceKernelGetThreadCurrentPriority" )] [Stateless]
						// /user/pspthreadman.h:413: int sceKernelGetThreadCurrentPriority();
						int sceKernelGetThreadCurrentPriority(){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x3B183E26, "sceKernelGetThreadExitStatus" )] [Stateless]
						// /user/pspthreadman.h:422: int sceKernelGetThreadExitStatus(SceUID thid);
						int sceKernelGetThreadExitStatus( int thid ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xD13BDE95, "sceKernelCheckThreadStack" )] [Stateless]
						// /user/pspthreadman.h:429: int sceKernelCheckThreadStack();
						int sceKernelCheckThreadStack(){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x52089CA1, "sceKernelGetThreadStackFreeSize" )] [Stateless]
						// /user/pspthreadman.h:439: int sceKernelGetThreadStackFreeSize(SceUID thid);
						int sceKernelGetThreadStackFreeSize( int thid ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x17C1684E, "sceKernelReferThreadStatus" )] [Stateless]
						// /user/pspthreadman.h:458: int sceKernelReferThreadStatus(SceUID thid, SceKernelThreadInfo *info);
						int sceKernelReferThreadStatus( int thid, int info ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xFFC36A14, "sceKernelReferThreadRunStatus" )] [Stateless]
						// /user/pspthreadman.h:468: int sceKernelReferThreadRunStatus(SceUID thid, SceKernelThreadRunStatus *status);
						int sceKernelReferThreadRunStatus( int thid, int status ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x627E6F3A, "sceKernelReferSystemStatus" )] [Stateless]
						// /user/pspthreadman.h:1100: int sceKernelReferSystemStatus(SceKernelSystemStatus *status);
						int sceKernelReferSystemStatus( int status ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x94416130, "sceKernelGetThreadmanIdList" )] [Stateless]
						// /user/pspthreadman.h:1075: int sceKernelGetThreadmanIdList(enum SceKernelIdListType type, SceUID *readbuf, int readbufsize, int *idcount);
						int sceKernelGetThreadmanIdList( int type, int readbuf, int readbufsize, int idcount ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x57CF62DD, "sceKernelGetThreadmanIdType" )] [Stateless]
						// /user/pspthreadman.h:1688: enum SceKernelIdListType sceKernelGetThreadmanIdType(SceUID uid);
						int sceKernelGetThreadmanIdType( int uid ){ return NISTUBRETURN; }

					};
				
				}
			}
		}
	}
}

/* GenerateStubsV2: auto-generated - B603D278 */
