PSPSDK=$(shell psp-config --pspsdk-path)
PSPDIR=$(shell psp-config --psp-prefix)
TARGET_LIB = ../../libmp4ff.a
OBJS = 	mp4ff.o mp4atom.o mp4meta.o mp4sample.o mp4util.o \
	mp4tagupdate.o
INCDIR = ../include
CFLAGS = -O2 -G0 -fomit-frame-pointer -fPIC -DUSE_TAGGING=1
CXXFLAGS = $(CFLAGS) -fno-rtti
ASFLAGS = $(CFLAGS)

include $(PSPSDK)/lib/build.mak

clean:
	rm -f *.o
	rm -f ../../libmp4ff.a