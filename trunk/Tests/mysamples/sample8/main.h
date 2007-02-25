//**************************************************************************
//		PSPGU Tutorial: 'Lesson6' - main.h
//**************************************************************************
// Includes
#include <malloc.h>
#include <pspkernel.h>
#include <pspdisplay.h>
#include <pspdebug.h>
#include <stdio.h>

#include <pspgu.h>
#include <pspgum.h>
#include <pspctrl.h>
#include "CTimer.h"
#include "TGALoader.h"

// Defines
#define BUF_WIDTH (512)
#define SCR_WIDTH (480)
#define SCR_HEIGHT (272)
#define GU_VERTEX_TEX_COLOR_TRANSFORM ( GU_TEXTURE_32BITF|GU_COLOR_8888|GU_VERTEX_32BITF|GU_TRANSFORM_3D )

typedef struct {
	float u, v;
	unsigned int color;
	float nx, ny, nz;
	float x, y, z;
} Vertex;

//**************************************************************************
// time related functions:
void FPS( void );				// Display Frames Per Second 

//Graphics related functions:
void InitGU( void );			// Initialize the Graphics Subsystem
void SetupProjection( void );	// Setups the Projection Matrix
void DrawScene( void );			// Render Geometry

int main(int argc, char **argv);



//**************************************************************************

// Exit callback
int exit_callback(int arg1, int arg2, void *common) {
          sceKernelExitGame();
          return 0;
}

// Callback thread 
int CallbackThread(SceSize args, void *argp) {
          int cbid;

          cbid = sceKernelCreateCallback("Exit Callback", exit_callback, NULL);
          sceKernelRegisterExitCallback(cbid);

          sceKernelSleepThreadCB();

          return 0;
}

// Sets up the callback thread and returns its thread id
int SetupCallbacks(void) {
          int thid = 0;

          thid = sceKernelCreateThread("update_thread", CallbackThread, 0x11, 0xFA0, 0, 0);
          if(thid >= 0) {
                    sceKernelStartThread(thid, 0, 0);
          }

          return thid;
}
