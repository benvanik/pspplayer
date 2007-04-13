TARGET = sample8
OBJS = main.o CTimer.o TGALoader.o 

INCDIR = 
CFLAGS = -g -O2 -G0 -Wall
CXXFLAGS = $(CFLAGS) -fno-exceptions -fno-rtti
ASFLAGS = $(CFLAGS)

LIBDIR =
LIBS = -lpspgum -lpspgu -lpsprtc -lstdc++ -lm
LDFLAGS =

EXTRA_TARGETS = EBOOT.PBP
PSP_EBOOT_TITLE = Sample 8 - NeHe L6

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

./TGALoader.o: ./TGALoader.cpp 
	$(CXX) $(CXXFLAGS) -c ./TGALoader.cpp -o ./TGALoader.o
	