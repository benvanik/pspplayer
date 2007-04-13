// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

//**************************************************************************
//		PSPGU Tutorial: 'Lesson1' - main.cpp
//**************************************************************************

#include "main.h"

PSP_MODULE_INFO( "sample3", 0, 1, 1 );
PSP_MAIN_THREAD_ATTR(THREAD_ATTR_USER);

void *dList;		// display List, used by sceGUStart
void *fbp0;			// frame buffer

int fps = 0;		// for calculating the frames per second
char fpsDisplay[100];
u32 tickResolution;
u64 fpsTickNow;
u64 fpsTickLast;

Vertex __attribute__((aligned(16))) triangle[3] = 
{
	{ GU_COLOR( 1.0f, 1.0f, 1.0f, 0.0f ), 0.0f, 1.0f, 0.0f },
	{ GU_COLOR( 1.0f, 1.0f, 1.0f, 0.0f ), 1.0f,-1.0f, 0.0f },
	{ GU_COLOR( 1.0f, 1.0f, 1.0f, 0.0f ),-1.0f,-1.0f, 0.0f }
};

Vertex __attribute__((aligned(16))) quad[4] = 
{
	{ GU_COLOR( 1.0f, 1.0f, 1.0f, 0.0f ),-1.0f, 1.0f, 0.0f },
	{ GU_COLOR( 1.0f, 1.0f, 1.0f, 0.0f ), 1.0f, 1.0f, 0.0f },
	{ GU_COLOR( 1.0f, 1.0f, 1.0f, 0.0f ),-1.0f,-1.0f, 0.0f },
	{ GU_COLOR( 1.0f, 1.0f, 1.0f, 0.0f ), 1.0f,-1.0f, 0.0f }

};

int main(int argc, char **argv)
{
	pspDebugScreenInit();
	SetupCallbacks();

	sceRtcGetCurrentTick( &fpsTickLast );
	tickResolution = sceRtcGetTickResolution();

	dList = memalign( 16, 640 );
	fbp0  = 0;

	InitGU();
	SetupProjection();

	while( 1 )
	{
		DrawScene();
		FPS();
		//sceDisplayWaitVblankStart();
		fbp0 = sceGuSwapBuffers();

	}

	sceGuTerm();			// Terminating the Graphics System

	// Free Memory
	free( dList );
	free( fbp0 );

	sceKernelExitGame();	// Quits Application
	return 0;
}

void FPS( void )
{
	fps++;
	sceRtcGetCurrentTick( &fpsTickNow );
	
	if( ((fpsTickNow - fpsTickLast)/((float)tickResolution)) >= 1.0f )
	{
		fpsTickLast = fpsTickNow;
		sprintf( fpsDisplay, "FPS: %d", fps );
		fps = 0;
	}
	pspDebugScreenSetOffset( (int)fbp0 );
	pspDebugScreenSetXY( 0, 0 );
	pspDebugScreenPrintf( fpsDisplay );

}

void InitGU( void )
{
	// Init GU
	sceGuInit();
	sceGuStart( GU_DIRECT, dList );

	// Set Buffers
	sceGuDrawBuffer( GU_PSM_8888, fbp0, BUF_WIDTH );
	sceGuDispBuffer( SCR_WIDTH, SCR_HEIGHT, (void*)0x88000, BUF_WIDTH);
	sceGuDepthBuffer( (void*)0x110000, BUF_WIDTH);

	sceGuOffset( 2048 - (SCR_WIDTH/2), 2048 - (SCR_HEIGHT/2));
	sceGuViewport( 2048, 2048, SCR_WIDTH, SCR_HEIGHT);
	sceGuDepthRange( 65535, 0);

	// Set Render States
	sceGuScissor( 0, 0, SCR_WIDTH, SCR_HEIGHT);
	sceGuEnable( GU_SCISSOR_TEST );
	sceGuDepthFunc( GU_GEQUAL );
	sceGuEnable( GU_DEPTH_TEST );
	sceGuFrontFace( GU_CW );
	sceGuShadeModel( GU_SMOOTH );
	sceGuEnable( GU_CULL_FACE );
	sceGuEnable( GU_CLIP_PLANES );

	sceGuFinish();
	sceGuSync(0,0);

	sceDisplayWaitVblankStart();
	sceGuDisplay(GU_TRUE);
	// finish
}

void SetupProjection( void )
{
	// setup matrices for the triangle
	sceGumMatrixMode(GU_PROJECTION);
	sceGumLoadIdentity();
	sceGumPerspective( 75.0f, 16.0f/9.0f, 0.5f, 1000.0f);

	sceGumMatrixMode(GU_VIEW);
	sceGumLoadIdentity();

	sceGuClearColor( GU_COLOR( 0.0f, 0.0f, 0.0f, 1.0f ) ); // 0xff554433 == cool bg color
	sceGuClearDepth(0);
	
}

void DrawScene( void )
{
	sceGuStart( GU_DIRECT, dList );
		
	// clear screen
	sceGuClear(GU_COLOR_BUFFER_BIT|GU_DEPTH_BUFFER_BIT);
	sceGumMatrixMode(GU_MODEL);
	sceGumLoadIdentity();
	
	{	// Move 1.5 units left and 6 units back
		ScePspFVector3 move = { -1.5f, 0.0f, -3.0f };
		sceGumTranslate( &move );
	}

	// Draw Triangle
	sceGumDrawArray( GU_TRIANGLES, GU_COLOR_8888|GU_VERTEX_32BITF|GU_TRANSFORM_3D,
					3, 0, triangle );

	{	// Move 3 units to the right
		ScePspFVector3 move = { 3.0f, 0.0f, 0.0f };
		sceGumTranslate( &move );
	}

	// Draw Quad
	sceGumDrawArray( GU_TRIANGLE_STRIP, GU_COLOR_8888|GU_VERTEX_32BITF|GU_TRANSFORM_3D,
					4, 0, quad );

	sceGuFinish();
	sceGuSync(0,0);	
}
