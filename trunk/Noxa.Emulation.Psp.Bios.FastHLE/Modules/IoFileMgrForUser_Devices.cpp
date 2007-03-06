// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "Stdafx.h"
#include "IoFileMgrForUser.h"
#include "Kernel.h"
#include "KernelDevice.h"

using namespace System;
using namespace System::Collections::Generic;
using namespace System::Diagnostics;
using namespace System::IO;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Bios;
using namespace Noxa::Emulation::Psp::Bios::Modules;
using namespace Noxa::Emulation::Psp::Media;

// int sceIoIoctl(SceUID fd, unsigned int cmd, void *indata, int inlen, void *outdata, int outlen); (/user/pspiofilemgr.h:368)
int IoFileMgrForUser::sceIoIoctl( IMemory^ memory, int fd, int cmd, int indata, int inlen, int outdata, int outlen ){ return NISTUBRETURN; }

// int sceIoIoctlAsync(SceUID fd, unsigned int cmd, void *indata, int inlen, void *outdata, int outlen); (/user/pspiofilemgr.h:381)
int IoFileMgrForUser::sceIoIoctlAsync( IMemory^ memory, int fd, int cmd, int indata, int inlen, int outdata, int outlen ){ return NISTUBRETURN; }

// int sceIoDevctl(const char *dev, unsigned int cmd, void *indata, int inlen, void *outdata, int outlen); (/user/pspiofilemgr.h:306)
int IoFileMgrForUser::sceIoDevctl( IMemory^ memory, int dev, int cmd, int indata, int inlen, int outdata, int outlen ){ return NISTUBRETURN; }

// int sceIoGetDevType(SceUID fd); (/user/pspiofilemgr.h:448)
int IoFileMgrForUser::sceIoGetDevType( int fd ){ return NISTUBRETURN; }

// int sceIoAssign(const char *dev1, const char *dev2, const char *dev3, int mode, void* unk1, long unk2); (/user/pspiofilemgr.h:325)
int IoFileMgrForUser::sceIoAssign( IMemory^ memory, int dev1, int dev2, int dev3, int mode, int unk1, int unk2 ){ return NISTUBRETURN; }

// int sceIoUnassign(const char *dev); (/user/pspiofilemgr.h:334)
int IoFileMgrForUser::sceIoUnassign( IMemory^ memory, int dev ){ return NISTUBRETURN; }
