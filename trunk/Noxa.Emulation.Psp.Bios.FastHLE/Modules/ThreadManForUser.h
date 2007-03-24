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

					public ref class ThreadManForUser : public Module
					{
					public:
						ThreadManForUser( Kernel^ kernel ) : Module( kernel ) {}
						~ThreadManForUser(){}

					public:
						property String^ Name { virtual String^ get() override { return "ThreadManForUser"; } }

						//virtual void Start() override;
						//virtual void Stop() override;
						//virtual void Clear() override;

					internal:
						//virtual void* QueryNativePointer( uint nid ) override;

					public: // So many they are all inline... good luck!

						// - Callbacks ------------------------------------------------------------------------------------------

						[NotImplemented]
						[BiosFunction( 0x6E9EA350, "_sceKernelReturnFromCallback" )] [Stateless]
						// void _sceKernelReturnFromCallback(); (/user/pspthreadman.h:1453)
						void _sceKernelReturnFromCallback(){}

						[BiosFunction( 0xE81CAF8F, "sceKernelCreateCallback" )] [Stateless]
						// int sceKernelCreateCallback(const char *name, SceKernelCallbackFunction func, void *arg); (/user/pspthreadman.h:985)
						int sceKernelCreateCallback( IMemory^ memory, int name, int func, int arg );

						[BiosFunction( 0xEDBA5844, "sceKernelDeleteCallback" )] [Stateless]
						// int sceKernelDeleteCallback(SceUID cb); (/user/pspthreadman.h:1005)
						int sceKernelDeleteCallback( int cb );

						[NotImplemented]
						[BiosFunction( 0xC11BA8C4, "sceKernelNotifyCallback" )] [Stateless]
						// int sceKernelNotifyCallback(SceUID cb, int arg2); (/user/pspthreadman.h:1015)
						int sceKernelNotifyCallback( int cb, int arg2 );

						[BiosFunction( 0xBA4051D6, "sceKernelCancelCallback" )] [Stateless]
						// int sceKernelCancelCallback(SceUID cb); (/user/pspthreadman.h:1024)
						int sceKernelCancelCallback( int cb );

						[BiosFunction( 0x2A3D44FF, "sceKernelGetCallbackCount" )] [Stateless]
						// int sceKernelGetCallbackCount(SceUID cb); (/user/pspthreadman.h:1033)
						int sceKernelGetCallbackCount( int cb );

						[NotImplemented]
						[BiosFunction( 0x349D6D6C, "sceKernelCheckCallback" )] [Stateless]
						// int sceKernelCheckCallback(); (/user/pspthreadman.h:1040)
						int sceKernelCheckCallback();

						[BiosFunction( 0x730ED8BC, "sceKernelReferCallbackStatus" )] [Stateless]
						// int sceKernelReferCallbackStatus(SceUID cb, SceKernelCallbackInfo *status); (/user/pspthreadman.h:996)
						int sceKernelReferCallbackStatus( IMemory^ memory, int cb, int status );

						// - Threads --------------------------------------------------------------------------------------------

						[NotImplemented]
						[BiosFunction( 0x0C106E53, "sceKernelRegisterThreadEventHandler" )] [Stateless]
						// SceUID sceKernelRegisterThreadEventHandler(const char *name, SceUID threadID, int mask, SceKernelThreadEventHandler handler, void *common); (/user/pspthreadman.h:1729)
						int sceKernelRegisterThreadEventHandler( IMemory^ memory, int name, int threadID, int mask, int handler, int common );

						[NotImplemented]
						[BiosFunction( 0x72F3C145, "sceKernelReleaseThreadEventHandler" )] [Stateless]
						// int sceKernelReleaseThreadEventHandler(SceUID uid); (/user/pspthreadman.h:1738)
						int sceKernelReleaseThreadEventHandler( int uid );

						[NotImplemented]
						[BiosFunction( 0x369EEB6B, "sceKernelReferThreadEventHandlerStatus" )] [Stateless]
						// int sceKernelReferThreadEventHandlerStatus(SceUID uid, struct SceKernelThreadEventHandlerInfo *info); (/user/pspthreadman.h:1748)
						int sceKernelReferThreadEventHandlerStatus( IMemory^ memory, int uid, int info );

						[BiosFunction( 0x446D8DE6, "sceKernelCreateThread" )] [Stateless]
						// SceUID sceKernelCreateThread(const char *name, SceKernelThreadEntry entry, int initPriority, int stackSize, SceUInt attr, SceKernelThreadOptParam *option); (/user/pspthreadman.h:169)
						int sceKernelCreateThread( IMemory^ memory, int name, int entry, int initPriority, int stackSize, int attr, int option );

						[BiosFunction( 0x9FA03CD3, "sceKernelDeleteThread" )]
						// int sceKernelDeleteThread(SceUID thid); (/user/pspthreadman.h:179)
						int sceKernelDeleteThread( int thid );

						[BiosFunction( 0xF475845D, "sceKernelStartThread" )]
						// int sceKernelStartThread(SceUID thid, SceSize arglen, void *argp); (/user/pspthreadman.h:188)
						int sceKernelStartThread( int thid, int arglen, int argp );

						[NotImplemented]
						[BiosFunction( 0x532A522E, "_sceKernelExitThread" )] [Stateless]
						// void _sceKernelExitThread(); (/user/pspthreadman.h:1679)
						void _sceKernelExitThread();

						[BiosFunction( 0xAA73C935, "sceKernelExitThread" )]
						// int sceKernelExitThread(int status); (/user/pspthreadman.h:195)
						int sceKernelExitThread( int status );

						[BiosFunction( 0x809CE29B, "sceKernelExitDeleteThread" )]
						// int sceKernelExitDeleteThread(int status); (/user/pspthreadman.h:202)
						int sceKernelExitDeleteThread( int status );

						[BiosFunction( 0x616403BA, "sceKernelTerminateThread" )]
						// int sceKernelTerminateThread(SceUID thid); (/user/pspthreadman.h:211)
						int sceKernelTerminateThread( int thid );

						[BiosFunction( 0x383F7BCC, "sceKernelTerminateDeleteThread" )]
						// int sceKernelTerminateDeleteThread(SceUID thid); (/user/pspthreadman.h:220)
						int sceKernelTerminateDeleteThread( int thid );

						[NotImplemented]
						[BiosFunction( 0x3AD58B8C, "sceKernelSuspendDispatchThread" )] [Stateless]
						// int sceKernelSuspendDispatchThread(); (/user/pspthreadman.h:227)
						int sceKernelSuspendDispatchThread();

						[NotImplemented]
						[BiosFunction( 0x27E22EC2, "sceKernelResumeDispatchThread" )] [Stateless]
						// int sceKernelResumeDispatchThread(int state); (/user/pspthreadman.h:237)
						int sceKernelResumeDispatchThread( int state );

						[BiosFunction( 0xEA748E31, "sceKernelChangeCurrentThreadAttr" )] [Stateless]
						// int sceKernelChangeCurrentThreadAttr(int unknown, SceUInt attr); (/user/pspthreadman.h:364)
						int sceKernelChangeCurrentThreadAttr( int unknown, int attr );

						[BiosFunction( 0x71BC9871, "sceKernelChangeThreadPriority" )] [Stateless]
						// int sceKernelChangeThreadPriority(SceUID thid, int priority); (/user/pspthreadman.h:381)
						int sceKernelChangeThreadPriority( int thid, int priority );

						[NotImplemented]
						[BiosFunction( 0x912354A7, "sceKernelRotateThreadReadyQueue" )] [Stateless]
						// int sceKernelRotateThreadReadyQueue(int priority); (/user/pspthreadman.h:390)
						int sceKernelRotateThreadReadyQueue( int priority );

						[BiosFunction( 0x2C34E053, "sceKernelReleaseWaitThread" )] [Stateless]
						// int sceKernelReleaseWaitThread(SceUID thid); (/user/pspthreadman.h:399)
						int sceKernelReleaseWaitThread( int thid );

						[BiosFunction( 0x293B45B8, "sceKernelGetThreadId" )] [Stateless]
						// int sceKernelGetThreadId(); (/user/pspthreadman.h:406)
						int sceKernelGetThreadId();

						[BiosFunction( 0x94AA61EE, "sceKernelGetThreadCurrentPriority" )] [Stateless]
						// int sceKernelGetThreadCurrentPriority(); (/user/pspthreadman.h:413)
						int sceKernelGetThreadCurrentPriority();

						[BiosFunction( 0x3B183E26, "sceKernelGetThreadExitStatus" )] [Stateless]
						// int sceKernelGetThreadExitStatus(SceUID thid); (/user/pspthreadman.h:422)
						int sceKernelGetThreadExitStatus( int thid );

						[NotImplemented]
						[BiosFunction( 0xD13BDE95, "sceKernelCheckThreadStack" )] [Stateless]
						// int sceKernelCheckThreadStack(); (/user/pspthreadman.h:429)
						int sceKernelCheckThreadStack();

						[NotImplemented]
						[BiosFunction( 0x52089CA1, "sceKernelGetThreadStackFreeSize" )] [Stateless]
						// int sceKernelGetThreadStackFreeSize(SceUID thid); (/user/pspthreadman.h:439)
						int sceKernelGetThreadStackFreeSize( int thid );

						[BiosFunction( 0x17C1684E, "sceKernelReferThreadStatus" )] [Stateless]
						// int sceKernelReferThreadStatus(SceUID thid, SceKernelThreadInfo *info); (/user/pspthreadman.h:458)
						int sceKernelReferThreadStatus( IMemory^ memory, int thid, int info );

						[BiosFunction( 0xFFC36A14, "sceKernelReferThreadRunStatus" )] [Stateless]
						// int sceKernelReferThreadRunStatus(SceUID thid, SceKernelThreadRunStatus *status); (/user/pspthreadman.h:468)
						int sceKernelReferThreadRunStatus( IMemory^ memory, int thid, int status );

						[BiosFunction( 0x627E6F3A, "sceKernelReferSystemStatus" )] [Stateless]
						// int sceKernelReferSystemStatus(SceKernelSystemStatus *status); (/user/pspthreadman.h:1100)
						int sceKernelReferSystemStatus( IMemory^ memory, int status );

						[NotImplemented]
						[BiosFunction( 0x94416130, "sceKernelGetThreadmanIdList" )] [Stateless]
						// int sceKernelGetThreadmanIdList(enum SceKernelIdListType type, SceUID *readbuf, int readbufsize, int *idcount); (/user/pspthreadman.h:1075)
						int sceKernelGetThreadmanIdList( IMemory^ memory, int type, int readbuf, int readbufsize, int idcount );

						[NotImplemented]
						[BiosFunction( 0x57CF62DD, "sceKernelGetThreadmanIdType" )] [Stateless]
						// enum SceKernelIdListType sceKernelGetThreadmanIdType(SceUID uid); (/user/pspthreadman.h:1688)
						int sceKernelGetThreadmanIdType( int uid );

						// - Thread Control -------------------------------------------------------------------------------------

						[BiosFunction( 0x9ACE131E, "sceKernelSleepThread" )]
						// int sceKernelSleepThread(); (/user/pspthreadman.h:244)
						int sceKernelSleepThread();

						[BiosFunction( 0x82826F70, "sceKernelSleepThreadCB" )]
						// int sceKernelSleepThreadCB(); (/user/pspthreadman.h:255)
						int sceKernelSleepThreadCB();

						[BiosFunction( 0xD59EAD2F, "sceKernelWakeupThread" )]
						// int sceKernelWakeupThread(SceUID thid); (/user/pspthreadman.h:264)
						int sceKernelWakeupThread( int thid );

						[NotImplemented]
						[BiosFunction( 0xFCCFAD26, "sceKernelCancelWakeupThread" )] [Stateless]
						// int sceKernelCancelWakeupThread(SceUID thid); (/user/pspthreadman.h:273)
						int sceKernelCancelWakeupThread( int thid );

						[BiosFunction( 0x9944F31F, "sceKernelSuspendThread" )]
						// int sceKernelSuspendThread(SceUID thid); (/user/pspthreadman.h:282)
						int sceKernelSuspendThread( int thid );

						[BiosFunction( 0x75156E8F, "sceKernelResumeThread" )]
						// int sceKernelResumeThread(SceUID thid); (/user/pspthreadman.h:291)
						int sceKernelResumeThread( int thid );

						[BiosFunction( 0x278C0DF5, "sceKernelWaitThreadEnd" )]
						// int sceKernelWaitThreadEnd(SceUID thid, SceUInt *timeout); (/user/pspthreadman.h:301)
						int sceKernelWaitThreadEnd( IMemory^ memory, int thid, int timeout );

						[BiosFunction( 0x840E8133, "sceKernelWaitThreadEndCB" )]
						// int sceKernelWaitThreadEndCB(SceUID thid, SceUInt *timeout); (/user/pspthreadman.h:311)
						int sceKernelWaitThreadEndCB( IMemory^ memory, int thid, int timeout );

						[BiosFunction( 0xCEADEB47, "sceKernelDelayThread" )]
						// int sceKernelDelayThread(SceUInt delay); (/user/pspthreadman.h:323)
						int sceKernelDelayThread( int delay );

						[BiosFunction( 0x68DA9E36, "sceKernelDelayThreadCB" )]
						// int sceKernelDelayThreadCB(SceUInt delay); (/user/pspthreadman.h:335)
						int sceKernelDelayThreadCB( int delay );

						[NotImplemented]
						[BiosFunction( 0xBD123D9E, "sceKernelDelaySysClockThread" )]
						// int sceKernelDelaySysClockThread(SceKernelSysClock *delay); (/user/pspthreadman.h:344)
						int sceKernelDelaySysClockThread( IMemory^ memory, int delay );

						[NotImplemented]
						[BiosFunction( 0x1181E963, "sceKernelDelaySysClockThreadCB" )]
						// int sceKernelDelaySysClockThreadCB(SceKernelSysClock *delay); (/user/pspthreadman.h:354)
						int sceKernelDelaySysClockThreadCB( IMemory^ memory, int delay );

						// - Semaphores -----------------------------------------------------------------------------------------

						[NotImplemented]
						[BiosFunction( 0xD6DA4BA1, "sceKernelCreateSema" )] [Stateless]
						// SceUID sceKernelCreateSema(const char *name, SceUInt attr, int initVal, int maxVal, SceKernelSemaOptParam *option); (/user/pspthreadman.h:515)
						int sceKernelCreateSema( int name, int attr, int initVal, int maxVal, int option ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x28B6489C, "sceKernelDeleteSema" )] [Stateless]
						// int sceKernelDeleteSema(SceUID semaid); (/user/pspthreadman.h:523)
						int sceKernelDeleteSema( int semaid ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x3F53E640, "sceKernelSignalSema" )] [Stateless]
						// int sceKernelSignalSema(SceUID semaid, int signal); (/user/pspthreadman.h:539)
						int sceKernelSignalSema( int semaid, int signal ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x4E3A1105, "sceKernelWaitSema" )] [Stateless]
						// int sceKernelWaitSema(SceUID semaid, int signal, SceUInt *timeout); (/user/pspthreadman.h:555)
						int sceKernelWaitSema( int semaid, int signal, int timeout ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x6D212BAC, "sceKernelWaitSemaCB" )] [Stateless]
						// int sceKernelWaitSemaCB(SceUID semaid, int signal, SceUInt *timeout); (/user/pspthreadman.h:571)
						int sceKernelWaitSemaCB( int semaid, int signal, int timeout ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x58B1F937, "sceKernelPollSema" )] [Stateless]
						// int sceKernelPollSema(SceUID semaid, int signal); (/user/pspthreadman.h:581)
						int sceKernelPollSema( int semaid, int signal ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xBC6FEBC5, "sceKernelReferSemaStatus" )] [Stateless]
						// int sceKernelReferSemaStatus(SceUID semaid, SceKernelSemaInfo *info); (/user/pspthreadman.h:591)
						int sceKernelReferSemaStatus( int semaid, int info ){ return NISTUBRETURN; }

						// - Events ---------------------------------------------------------------------------------------------

						[BiosFunction( 0x55C20A00, "sceKernelCreateEventFlag" )] [Stateless]
						// SceUID sceKernelCreateEventFlag(const char *name, int attr, int bits, SceKernelEventFlagOptParam *opt); (/user/pspthreadman.h:645)
						int sceKernelCreateEventFlag( IMemory^ memory, int name, int attr, int bits, int opt );

						[BiosFunction( 0xEF9E4C70, "sceKernelDeleteEventFlag" )] [Stateless]
						// int sceKernelDeleteEventFlag(int evid); (/user/pspthreadman.h:709)
						int sceKernelDeleteEventFlag( int evid );

						[BiosFunction( 0x1FB15A32, "sceKernelSetEventFlag" )]
						// int sceKernelSetEventFlag(SceUID evid, u32 bits); (/user/pspthreadman.h:655)
						int sceKernelSetEventFlag( int evid, int bits );

						[BiosFunction( 0x812346E4, "sceKernelClearEventFlag" )]
						// int sceKernelClearEventFlag(SceUID evid, u32 bits); (/user/pspthreadman.h:665)
						int sceKernelClearEventFlag( int evid, int bits );

						[BiosFunction( 0x402FCF22, "sceKernelWaitEventFlag" )]
						// int sceKernelWaitEventFlag(int evid, u32 bits, u32 wait, u32 *outBits, SceUInt *timeout); (/user/pspthreadman.h:688)
						int sceKernelWaitEventFlag( IMemory^ memory, int evid, int bits, int wait, int outBits, int timeout );

						[BiosFunction( 0x328C546A, "sceKernelWaitEventFlagCB" )]
						// int sceKernelWaitEventFlagCB(int evid, u32 bits, u32 wait, u32 *outBits, SceUInt *timeout); (/user/pspthreadman.h:700)
						int sceKernelWaitEventFlagCB( IMemory^ memory, int evid, int bits, int wait, int outBits, int timeout );

						[BiosFunction( 0x30FD48F0, "sceKernelPollEventFlag" )] [Stateless]
						// int sceKernelPollEventFlag(int evid, u32 bits, u32 wait, u32 *outBits); (/user/pspthreadman.h:676)
						int sceKernelPollEventFlag( IMemory^ memory, int evid, int bits, int wait, int outBits );

						[BiosFunction( 0xA66B0120, "sceKernelReferEventFlagStatus" )] [Stateless]
						// int sceKernelReferEventFlagStatus(SceUID event, SceKernelEventFlagInfo *status); (/user/pspthreadman.h:719)
						int sceKernelReferEventFlagStatus( IMemory^ memory, int evid, int status );

						// - Mailboxes? -----------------------------------------------------------------------------------------

						[NotImplemented]
						[BiosFunction( 0x8125221D, "sceKernelCreateMbx" )] [Stateless]
						// SceUID sceKernelCreateMbx(const char *name, SceUInt attr, SceKernelMbxOptParam *option); (/user/pspthreadman.h:774)
						int sceKernelCreateMbx( int name, int attr, int option ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x86255ADA, "sceKernelDeleteMbx" )] [Stateless]
						// int sceKernelDeleteMbx(SceUID mbxid); (/user/pspthreadman.h:782)
						int sceKernelDeleteMbx( int mbxid ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xE9B3061E, "sceKernelSendMbx" )] [Stateless]
						// int sceKernelSendMbx(SceUID mbxid, void *message); (/user/pspthreadman.h:806)
						int sceKernelSendMbx( int mbxid, int message ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x18260574, "sceKernelReceiveMbx" )] [Stateless]
						// int sceKernelReceiveMbx(SceUID mbxid, void **pmessage, SceUInt *timeout); (/user/pspthreadman.h:824)
						int sceKernelReceiveMbx( int mbxid, int pmessage, int timeout ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xF3986382, "sceKernelReceiveMbxCB" )] [Stateless]
						// int sceKernelReceiveMbxCB(SceUID mbxid, void **pmessage, SceUInt *timeout); (/user/pspthreadman.h:842)
						int sceKernelReceiveMbxCB( int mbxid, int pmessage, int timeout ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x0D81716A, "sceKernelPollMbx" )] [Stateless]
						// int sceKernelPollMbx(SceUID mbxid, void **pmessage); (/user/pspthreadman.h:859)
						int sceKernelPollMbx( int mbxid, int pmessage ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x87D4DD36, "sceKernelCancelReceiveMbx" )] [Stateless]
						// int sceKernelCancelReceiveMbx(SceUID mbxid, int *pnum); (/user/pspthreadman.h:876)
						int sceKernelCancelReceiveMbx( int mbxid, int pnum ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xA8E8C846, "sceKernelReferMbxStatus" )] [Stateless]
						// int sceKernelReferMbxStatus(SceUID mbxid, SceKernelMbxInfo *info); (/user/pspthreadman.h:886)
						int sceKernelReferMbxStatus( int mbxid, int info ){ return NISTUBRETURN; }

						// - MsgPipes -------------------------------------------------------------------------------------------

						[NotImplemented]
						[BiosFunction( 0x7C0DC2A0, "sceKernelCreateMsgPipe" )] [Stateless]
						// SceUID sceKernelCreateMsgPipe(const char *name, int part, int attr, void *unk1, void *opt); (/user/pspthreadman.h:1114)
						int sceKernelCreateMsgPipe( int name, int part, int attr, int unk1, int opt ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xF0B7DA1C, "sceKernelDeleteMsgPipe" )] [Stateless]
						// int sceKernelDeleteMsgPipe(SceUID uid); (/user/pspthreadman.h:1123)
						int sceKernelDeleteMsgPipe( int uid ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x876DBFAD, "sceKernelSendMsgPipe" )] [Stateless]
						// int sceKernelSendMsgPipe(SceUID uid, void *message, unsigned int size, int unk1, void *unk2, unsigned int *timeout); (/user/pspthreadman.h:1137)
						int sceKernelSendMsgPipe( int uid, int message, int size, int unk1, int unk2, int timeout ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x7C41F2C2, "sceKernelSendMsgPipeCB" )] [Stateless]
						// int sceKernelSendMsgPipeCB(SceUID uid, void *message, unsigned int size, int unk1, void *unk2, unsigned int *timeout); (/user/pspthreadman.h:1151)
						int sceKernelSendMsgPipeCB( int uid, int message, int size, int unk1, int unk2, int timeout ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x884C9F90, "sceKernelTrySendMsgPipe" )] [Stateless]
						// int sceKernelTrySendMsgPipe(SceUID uid, void *message, unsigned int size, int unk1, void *unk2); (/user/pspthreadman.h:1164)
						int sceKernelTrySendMsgPipe( int uid, int message, int size, int unk1, int unk2 ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x74829B76, "sceKernelReceiveMsgPipe" )] [Stateless]
						// int sceKernelReceiveMsgPipe(SceUID uid, void *message, unsigned int size, int unk1, void *unk2, unsigned int *timeout); (/user/pspthreadman.h:1178)
						int sceKernelReceiveMsgPipe( int uid, int message, int size, int unk1, int unk2, int timeout ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xFBFA697D, "sceKernelReceiveMsgPipeCB" )] [Stateless]
						// int sceKernelReceiveMsgPipeCB(SceUID uid, void *message, unsigned int size, int unk1, void *unk2, unsigned int *timeout); (/user/pspthreadman.h:1192)
						int sceKernelReceiveMsgPipeCB( int uid, int message, int size, int unk1, int unk2, int timeout ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xDF52098F, "sceKernelTryReceiveMsgPipe" )] [Stateless]
						// int sceKernelTryReceiveMsgPipe(SceUID uid, void *message, unsigned int size, int unk1, void *unk2); (/user/pspthreadman.h:1205)
						int sceKernelTryReceiveMsgPipe( int uid, int message, int size, int unk1, int unk2 ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x349B864D, "sceKernelCancelMsgPipe" )] [Stateless]
						// int sceKernelCancelMsgPipe(SceUID uid, int *psend, int *precv); (/user/pspthreadman.h:1216)
						int sceKernelCancelMsgPipe( int uid, int psend, int precv ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x33BE4024, "sceKernelReferMsgPipeStatus" )] [Stateless]
						// int sceKernelReferMsgPipeStatus(SceUID uid, SceKernelMppInfo *info); (/user/pspthreadman.h:1237)
						int sceKernelReferMsgPipeStatus( int uid, int info ){ return NISTUBRETURN; }

						// - VPL? -----------------------------------------------------------------------------------------------

						[NotImplemented]
						[BiosFunction( 0x56C039B5, "sceKernelCreateVpl" )] [Stateless]
						// SceUID sceKernelCreateVpl(const char *name, int part, int attr, unsigned int size, struct SceKernelVplOptParam *opt); (/user/pspthreadman.h:1256)
						int sceKernelCreateVpl( int name, int part, int attr, int size, int opt ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x89B3D48C, "sceKernelDeleteVpl" )] [Stateless]
						// int sceKernelDeleteVpl(SceUID uid); (/user/pspthreadman.h:1265)
						int sceKernelDeleteVpl( int uid ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xBED27435, "sceKernelAllocateVpl" )] [Stateless]
						// int sceKernelAllocateVpl(SceUID uid, unsigned int size, void **data, unsigned int *timeout); (/user/pspthreadman.h:1277)
						int sceKernelAllocateVpl( int uid, int size, int data, int timeout ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xEC0A693F, "sceKernelAllocateVplCB" )] [Stateless]
						// int sceKernelAllocateVplCB(SceUID uid, unsigned int size, void **data, unsigned int *timeout); (/user/pspthreadman.h:1289)
						int sceKernelAllocateVplCB( int uid, int size, int data, int timeout ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xAF36D708, "sceKernelTryAllocateVpl" )] [Stateless]
						// int sceKernelTryAllocateVpl(SceUID uid, unsigned int size, void **data); (/user/pspthreadman.h:1300)
						int sceKernelTryAllocateVpl( int uid, int size, int data ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xB736E9FF, "sceKernelFreeVpl" )] [Stateless]
						// int sceKernelFreeVpl(SceUID uid, void *data); (/user/pspthreadman.h:1310)
						int sceKernelFreeVpl( int uid, int data ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x1D371B8A, "sceKernelCancelVpl" )] [Stateless]
						// int sceKernelCancelVpl(SceUID uid, int *pnum); (/user/pspthreadman.h:1320)
						int sceKernelCancelVpl( int uid, int pnum ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x39810265, "sceKernelReferVplStatus" )] [Stateless]
						// int sceKernelReferVplStatus(SceUID uid, SceKernelVplInfo *info); (/user/pspthreadman.h:1340)
						int sceKernelReferVplStatus( int uid, int info ){ return NISTUBRETURN; }

						// - FPL? -----------------------------------------------------------------------------------------------

						[NotImplemented]
						[BiosFunction( 0xC07BB470, "sceKernelCreateFpl" )] [Stateless]
						// int sceKernelCreateFpl(const char *name, int part, int attr, unsigned int size, unsigned int blocks, struct SceKernelFplOptParam *opt); (/user/pspthreadman.h:1360)
						int sceKernelCreateFpl( int name, int part, int attr, int size, int blocks, int opt ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xED1410E0, "sceKernelDeleteFpl" )] [Stateless]
						// int sceKernelDeleteFpl(SceUID uid); (/user/pspthreadman.h:1369)
						int sceKernelDeleteFpl( int uid ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xD979E9BF, "sceKernelAllocateFpl" )] [Stateless]
						// int sceKernelAllocateFpl(SceUID uid, void **data, unsigned int *timeout); (/user/pspthreadman.h:1380)
						int sceKernelAllocateFpl( int uid, int data, int timeout ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xE7282CB6, "sceKernelAllocateFplCB" )] [Stateless]
						// int sceKernelAllocateFplCB(SceUID uid, void **data, unsigned int *timeout); (/user/pspthreadman.h:1391)
						int sceKernelAllocateFplCB( int uid, int data, int timeout ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x623AE665, "sceKernelTryAllocateFpl" )] [Stateless]
						// int sceKernelTryAllocateFpl(SceUID uid, void **data); (/user/pspthreadman.h:1401)
						int sceKernelTryAllocateFpl( int uid, int data ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xF6414A71, "sceKernelFreeFpl" )] [Stateless]
						// int sceKernelFreeFpl(SceUID uid, void *data); (/user/pspthreadman.h:1411)
						int sceKernelFreeFpl( int uid, int data ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xA8AA591F, "sceKernelCancelFpl" )] [Stateless]
						// int sceKernelCancelFpl(SceUID uid, int *pnum); (/user/pspthreadman.h:1421)
						int sceKernelCancelFpl( int uid, int pnum ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xD8199E4C, "sceKernelReferFplStatus" )] [Stateless]
						// int sceKernelReferFplStatus(SceUID uid, SceKernelFplInfo *info); (/user/pspthreadman.h:1442)
						int sceKernelReferFplStatus( int uid, int info ){ return NISTUBRETURN; }

						// - Time -----------------------------------------------------------------------------------------------

						[NotImplemented]
						[BiosFunction( 0x0E927AED, "_sceKernelReturnFromTimerHandler" )] [Stateless]
						// void _sceKernelReturnFromTimerHandler(); (/user/pspthreadman.h:1447)
						void _sceKernelReturnFromTimerHandler();

						[BiosFunction( 0x110DEC9A, "sceKernelUSec2SysClock" )] [Stateless]
						// int sceKernelUSec2SysClock(unsigned int usec, SceKernelSysClock *clock); (/user/pspthreadman.h:1463)
						int sceKernelUSec2SysClock( IMemory^ memory, int usec, int clock );

						[BiosFunction( 0xC8CD158C, "sceKernelUSec2SysClockWide" )] [Stateless]
						// SceInt64 sceKernelUSec2SysClockWide(unsigned int usec); (/user/pspthreadman.h:1472)
						int64 sceKernelUSec2SysClockWide( int usec );

						[BiosFunction( 0xBA6B92E2, "sceKernelSysClock2USec" )] [Stateless]
						// int sceKernelSysClock2USec(SceKernelSysClock *clock, unsigned int *low, unsigned int *high); (/user/pspthreadman.h:1483)
						int sceKernelSysClock2USec( IMemory^ memory, int clock, int sec, int usec );

						[BiosFunction( 0xE1619D7C, "sceKernelSysClock2USecWide" )] [Stateless]
						// int sceKernelSysClock2USecWide(SceInt64 clock, unsigned *low, unsigned int *high); (/user/pspthreadman.h:1494)
						int sceKernelSysClock2USecWide( IMemory^ memory, int64 clock, int sec, int usec );

						[BiosFunction( 0xDB738F35, "sceKernelGetSystemTime" )] [Stateless]
						// int sceKernelGetSystemTime(SceKernelSysClock *time); (/user/pspthreadman.h:1503)
						int sceKernelGetSystemTime( IMemory^ memory, int time );

						[BiosFunction( 0x82BC5777, "sceKernelGetSystemTimeWide" )] [Stateless]
						// SceInt64 sceKernelGetSystemTimeWide(); (/user/pspthreadman.h:1510)
						int64 sceKernelGetSystemTimeWide();

						[BiosFunction( 0x369ED59D, "sceKernelGetSystemTimeLow" )] [Stateless]
						// unsigned int sceKernelGetSystemTimeLow(); (/user/pspthreadman.h:1517)
						int sceKernelGetSystemTimeLow();

						// - Alarms ---------------------------------------------------------------------------------------------

						[NotImplemented]
						[BiosFunction( 0x6652B8CA, "sceKernelSetAlarm" )] [Stateless]
						// SceUID sceKernelSetAlarm(SceUInt clock, SceKernelAlarmHandler handler, void *common); (/user/pspthreadman.h:915)
						int sceKernelSetAlarm( int clock, int handler, int common ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xB2C25152, "sceKernelSetSysClockAlarm" )] [Stateless]
						// SceUID sceKernelSetSysClockAlarm(SceKernelSysClock *clock, SceKernelAlarmHandler handler, void *common); (/user/pspthreadman.h:926)
						int sceKernelSetSysClockAlarm( int clock, int handler, int common ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x7E65B999, "sceKernelCancelAlarm" )] [Stateless]
						// int sceKernelCancelAlarm(SceUID alarmid); (/user/pspthreadman.h:935)
						int sceKernelCancelAlarm( int alarmid ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xDAA3F564, "sceKernelReferAlarmStatus" )] [Stateless]
						// int sceKernelReferAlarmStatus(SceUID alarmid, SceKernelAlarmInfo *info); (/user/pspthreadman.h:945)
						int sceKernelReferAlarmStatus( int alarmid, int info ){ return NISTUBRETURN; }

						// - Timers ---------------------------------------------------------------------------------------------

						[NotImplemented]
						[BiosFunction( 0x20FFF560, "sceKernelCreateVTimer" )] [Stateless]
						// SceUID sceKernelCreateVTimer(const char *name, struct SceKernelVTimerOptParam *opt); (/user/pspthreadman.h:1531)
						int sceKernelCreateVTimer( int name, int opt ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x328F9E52, "sceKernelDeleteVTimer" )] [Stateless]
						// int sceKernelDeleteVTimer(SceUID uid); (/user/pspthreadman.h:1540)
						int sceKernelDeleteVTimer( int uid ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xB3A59970, "sceKernelGetVTimerBase" )] [Stateless]
						// int sceKernelGetVTimerBase(SceUID uid, SceKernelSysClock *base); (/user/pspthreadman.h:1550)
						int sceKernelGetVTimerBase( int uid, int base ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xB7C18B77, "sceKernelGetVTimerBaseWide" )] [Stateless]
						// SceInt64 sceKernelGetVTimerBaseWide(SceUID uid); (/user/pspthreadman.h:1559)
						int64 sceKernelGetVTimerBaseWide( int uid ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x034A921F, "sceKernelGetVTimerTime" )] [Stateless]
						// int sceKernelGetVTimerTime(SceUID uid, SceKernelSysClock *time); (/user/pspthreadman.h:1569)
						int sceKernelGetVTimerTime( int uid, int time ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xC0B3FFD2, "sceKernelGetVTimerTimeWide" )] [Stateless]
						// SceInt64 sceKernelGetVTimerTimeWide(SceUID uid); (/user/pspthreadman.h:1578)
						int64 sceKernelGetVTimerTimeWide( int uid ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x542AD630, "sceKernelSetVTimerTime" )] [Stateless]
						// int sceKernelSetVTimerTime(SceUID uid, SceKernelSysClock *time); (/user/pspthreadman.h:1588)
						int sceKernelSetVTimerTime( int uid, int time ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xFB6425C3, "sceKernelSetVTimerTimeWide" )] [Stateless]
						// SceInt64 sceKernelSetVTimerTimeWide(SceUID uid, SceInt64 time); (/user/pspthreadman.h:1598)
						int64 sceKernelSetVTimerTimeWide( int uid, int64 time ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xC68D9437, "sceKernelStartVTimer" )] [Stateless]
						// int sceKernelStartVTimer(SceUID uid); (/user/pspthreadman.h:1607)
						int sceKernelStartVTimer( int uid ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xD0AEEE87, "sceKernelStopVTimer" )] [Stateless]
						// int sceKernelStopVTimer(SceUID uid); (/user/pspthreadman.h:1616)
						int sceKernelStopVTimer( int uid ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xD8B299AE, "sceKernelSetVTimerHandler" )] [Stateless]
						// int sceKernelSetVTimerHandler(SceUID uid, SceKernelSysClock *time, SceKernelVTimerHandler handler, void *common); (/user/pspthreadman.h:1631)
						int sceKernelSetVTimerHandler( int uid, int time, int handler, int common ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x53B00E9A, "sceKernelSetVTimerHandlerWide" )] [Stateless]
						// int sceKernelSetVTimerHandlerWide(SceUID uid, SceInt64 time, SceKernelVTimerHandlerWide handler, void *common); (/user/pspthreadman.h:1643)
						int sceKernelSetVTimerHandlerWide( int uid, int64 time, int handler, int common ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0xD2D615EF, "sceKernelCancelVTimerHandler" )] [Stateless]
						// int sceKernelCancelVTimerHandler(SceUID uid); (/user/pspthreadman.h:1652)
						int sceKernelCancelVTimerHandler( int uid ){ return NISTUBRETURN; }

						[NotImplemented]
						[BiosFunction( 0x5F32BEAA, "sceKernelReferVTimerStatus" )] [Stateless]
						// int sceKernelReferVTimerStatus(SceUID uid, SceKernelVTimerInfo *info); (/user/pspthreadman.h:1673)
						int sceKernelReferVTimerStatus( int uid, int info ){ return NISTUBRETURN; }

						// ------------------------------------------------------------------------------------------------------

					};
				
				}
			}
		}
	}
}

/* GenerateStubsV2: auto-generated - C9509829 */
