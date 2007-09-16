TARGET = hooker
OBJS = main.o hooking.o hookasm.o

BUILD_PRX=1
PSP_FW_VERSION=150

# Use the kernel's small inbuilt libc
#USE_KERNEL_LIBC = 1
# Use only kernel libraries
#USE_KERNEL_LIBS = 1

INCDIR = 
#CFLAGS = -O2 -G0 -Wall -fno-builtin-printf -DDEBUG
CFLAGS = -Os -G0 -Wall -fno-builtin-printf
CXXFLAGS = $(CFLAGS) -fno-exceptions -fno-rtti
ASFLAGS = $(CFLAGS)

LIBDIR =

LIBS =

PSPSDK=$(shell psp-config --pspsdk-path)
include $(PSPSDK)/lib/build.mak
