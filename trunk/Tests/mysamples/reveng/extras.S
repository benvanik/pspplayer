	.set noreorder

#include "pspimport.s"

IMPORT_START "SysMemUserForUser",0x40000000
IMPORT_FUNC	"SysMemUserForUser",0x7591C7DB,sceKernelSetCompiledSdkVersion
IMPORT_FUNC	"SysMemUserForUser",0xF77D77CB,sceKernelSetCompilerVersion
