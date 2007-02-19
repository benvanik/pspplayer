TARGET = sample4
OBJS = main.o CTimer.o

INCDIR = 
CFLAGS = -O2 -G0 -Wall
CXXFLAGS = $(CFLAGS) -fno-exceptions -fno-rtti
ASFLAGS = $(CFLAGS)

LIBDIR =
LIBS = -lpspgum -lpspgu -lpsprtc -lstdc++ -lm
LDFLAGS =

EXTRA_TARGETS = EBOOT.PBP
PSP_EBOOT_TITLE = Sample 4 - NeHe L2

KXDIR			= __SCE__$(TARGET)
KXDUMMY			= %__SCE__$(TARGET)

PSPSDK=$(shell psp-config --pspsdk-path)
include $(PSPSDK)/lib/build.mak

run: kxploit
	cp -r $(KXDIR) /cygdrive/h/PSP/GAME/
	cp -r $(KXDUMMY) /cygdrive/h/PSP/GAME/
	
./CTimer.o: ./CTimer.cpp 
	$(CXX) $(CXXFLAGS) -c ./CTimer.cpp -o ./CTimer.o

./main.o: ./main.c
	$(CXX) $(CXXFLAGS) -c ./main.c -o ./main.o
