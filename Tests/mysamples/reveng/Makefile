TARGET = reveng
EXTRAS_OBJS = SysMemUserForUser_0000.o SysMemUserForUser_0008.o SysMemUserForUser_0009.o
OBJS = main.o $(EXTRAS_OBJS)

INCDIR = 
CFLAGS = -O2 -G0 -Wall
CXXFLAGS = $(CFLAGS) -fno-exceptions -fno-rtti
ASFLAGS = $(CFLAGS)

LIBDIR =
LDFLAGS =

EXTRA_TARGETS = EBOOT.PBP
PSP_EBOOT_TITLE = Reverse Engineer

$(EXTRAS_OBJS): extras.S
	@$(CC) $(CFLAGS) -c $< -o $@

PSPSDK=$(shell psp-config --pspsdk-path)
include $(PSPSDK)/lib/build.mak
