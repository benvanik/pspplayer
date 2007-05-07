// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

//**************************************************************************
//		PSPGU Tutorial: 'Lesson4' - main.cpp
//**************************************************************************

#include "main.h"

PSP_MODULE_INFO( "sample6", 0, 1, 1 );
PSP_MAIN_THREAD_ATTR(THREAD_ATTR_USER);

void *dList;		// display List, used by sceGUStart
void *fbp0;			// frame buffer

int fps = 0;		// for calculating the frames per second
char *fpsDisplay;
u32 tickResolution;
u64 fpsTickNow;
u64 fpsTickLast;

float rtri = 0.0f;
float rquad= 0.0f;
float dt;

Vertex __attribute__((aligned(16))) pyramid[3*4] = 
{	//          RED   GREEN BLUE  ALPHA
	{ GU_COLOR( 1.0f, 0.0f, 0.0f, 0.0f ), 0.0f, 1.0f, 0.0f },	// Front
	{ GU_COLOR( 0.0f, 0.0f, 1.0f, 0.0f ), 1.0f,-1.0f, 1.0f },
	{ GU_COLOR( 0.0f, 1.0f, 0.0f, 0.0f ),-1.0f,-1.0f, 1.0f },

	{ GU_COLOR( 1.0f, 0.0f, 0.0f, 0.0f ), 0.0f, 1.0f, 0.0f },	// Right
	{ GU_COLOR( 0.0f, 1.0f, 0.0f, 0.0f ), 1.0f,-1.0f,-1.0f },
	{ GU_COLOR( 0.0f, 0.0f, 1.0f, 0.0f ), 1.0f,-1.0f, 1.0f },

	{ GU_COLOR( 1.0f, 0.0f, 0.0f, 0.0f ), 0.0f, 1.0f, 0.0f },	// Back
	{ GU_COLOR( 0.0f, 0.0f, 1.0f, 0.0f ),-1.0f,-1.0f,-1.0f },
	{ GU_COLOR( 0.0f, 1.0f, 0.0f, 0.0f ), 1.0f,-1.0f,-1.0f },

	{ GU_COLOR( 1.0f, 0.0f, 0.0f, 0.0f ), 0.0f, 1.0f, 0.0f },	// Left
	{ GU_COLOR( 0.0f, 1.0f, 0.0f, 0.0f ),-1.0f,-1.0f, 1.0f },
	{ GU_COLOR( 0.0f, 0.0f, 1.0f, 0.0f ),-1.0f,-1.0f,-1.0f }
};

Vertex __attribute__((aligned(16))) cube[3*12] = 
{
	{ GU_COLOR( 0.0f, 1.0f, 0.0f, 0.0f ),-1.0f, 1.0f,-1.0f },	// Top
	{ GU_COLOR( 0.0f, 1.0f, 0.0f, 0.0f ), 1.0f, 1.0f,-1.0f },
	{ GU_COLOR( 0.0f, 1.0f, 0.0f, 0.0f ),-1.0f, 1.0f, 1.0f },

	{ GU_COLOR( 0.0f, 1.0f, 0.0f, 0.0f ),-1.0f, 1.0f, 1.0f },
	{ GU_COLOR( 0.0f, 1.0f, 0.0f, 0.0f ), 1.0f, 1.0f,-1.0f },
	{ GU_COLOR( 0.0f, 1.0f, 0.0f, 0.0f ), 1.0f, 1.0f, 1.0f },

	{ GU_COLOR( 1.0f, 0.5f, 0.0f, 0.0f ),-1.0f,-1.0f, 1.0f },	// Bottom
	{ GU_COLOR( 1.0f, 0.5f, 0.0f, 0.0f ), 1.0f,-1.0f, 1.0f },
	{ GU_COLOR( 1.0f, 0.5f, 0.0f, 0.0f ),-1.0f,-1.0f,-1.0f },

	{ GU_COLOR( 1.0f, 0.5f, 0.0f, 0.0f ),-1.0f,-1.0f,-1.0f },
	{ GU_COLOR( 1.0f, 0.5f, 0.0f, 0.0f ), 1.0f,-1.0f, 1.0f },
	{ GU_COLOR( 1.0f, 0.5f, 0.0f, 0.0f ), 1.0f,-1.0f,-1.0f },

	{ GU_COLOR( 1.0f, 0.0f, 0.0f, 0.0f ),-1.0f, 1.0f, 1.0f },	// Front
	{ GU_COLOR( 1.0f, 0.0f, 0.0f, 0.0f ), 1.0f, 1.0f, 1.0f },
	{ GU_COLOR( 1.0f, 0.0f, 0.0f, 0.0f ),-1.0f,-1.0f, 1.0f },

	{ GU_COLOR( 1.0f, 0.0f, 0.0f, 0.0f ),-1.0f,-1.0f, 1.0f },
	{ GU_COLOR( 1.0f, 0.0f, 0.0f, 0.0f ), 1.0f, 1.0f, 1.0f },
	{ GU_COLOR( 1.0f, 0.0f, 0.0f, 0.0f ), 1.0f,-1.0f, 1.0f },

	{ GU_COLOR( 1.0f, 1.0f, 0.0f, 0.0f ),-1.0f,-1.0f,-1.0f },	// Back
	{ GU_COLOR( 1.0f, 1.0f, 1.0f, 0.0f ), 1.0f,-1.0f,-1.0f },
	{ GU_COLOR( 1.0f, 1.0f, 1.0f, 0.0f ),-1.0f, 1.0f,-1.0f },

	{ GU_COLOR( 1.0f, 1.0f, 1.0f, 0.0f ),-1.0f, 1.0f,-1.0f },
	{ GU_COLOR( 1.0f, 1.0f, 1.0f, 0.0f ), 1.0f,-1.0f,-1.0f },
	{ GU_COLOR( 1.0f, 1.0f, 1.0f, 0.0f ), 1.0f, 1.0f,-1.0f },

	{ GU_COLOR( 0.0f, 0.0f, 1.0f, 0.0f ),-1.0f, 1.0f,-1.0f },	// Left
	{ GU_COLOR( 0.0f, 0.0f, 1.0f, 0.0f ),-1.0f, 1.0f, 1.0f },
	{ GU_COLOR( 0.0f, 0.0f, 1.0f, 0.0f ),-1.0f,-1.0f,-1.0f },

	{ GU_COLOR( 0.0f, 0.0f, 1.0f, 0.0f ),-1.0f,-1.0f,-1.0f },
	{ GU_COLOR( 0.0f, 0.0f, 1.0f, 0.0f ),-1.0f, 1.0f, 1.0f },
	{ GU_COLOR( 0.0f, 0.0f, 1.0f, 0.0f ),-1.0f,-1.0f, 1.0f },

	{ GU_COLOR( 1.0f, 0.0f, 1.0f, 0.0f ), 1.0f, 1.0f, 1.0f },	// Right
	{ GU_COLOR( 1.0f, 0.0f, 1.0f, 0.0f ), 1.0f, 1.0f,-1.0f },
	{ GU_COLOR( 1.0f, 0.0f, 1.0f, 0.0f ), 1.0f,-1.0f, 1.0f },

	{ GU_COLOR( 1.0f, 0.0f, 1.0f, 0.0f ), 1.0f,-1.0f, 1.0f },
	{ GU_COLOR( 1.0f, 0.0f, 1.0f, 0.0f ), 1.0f, 1.0f,-1.0f },
	{ GU_COLOR( 1.0f, 0.0f, 1.0f, 0.0f ), 1.0f,-1.0f,-1.0f }

};

int main(int argc, char **argv)
{
	SetupCallbacks();
	pspDebugScreenInit();

	sceRtcGetCurrentTick( &fpsTickLast );
	tickResolution = sceRtcGetTickResolution();

	dList = malloc( 1024 );
	fbp0  = 0;
	fpsDisplay = (char*) memalign(16, 20*sizeof(char));		// (NEW)
	fpsDisplay = "FPS: calculating";				        // (NEW)

	sceKernelDcacheWritebackAll();
	InitGU();
	SetupProjection();
	CTimer timer;
	
	while( 1 )
	{
		dt = timer.GetDeltaTime( );
		DrawScene();
		FPS();

		sceDisplayWaitVblankStart();
		fbp0 = sceGuSwapBuffers();

	}

	sceGuTerm();			// Terminating the Graphics System

	// Free Memory
	free( dList );
	free( fpsDisplay );

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
		sprintf( fpsDisplay, "FPS: %i", fps );
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
	sceGuEnable( GU_CULL_FACE );					
	sceGuShadeModel( GU_SMOOTH );
	sceGuEnable( GU_CLIP_PLANES );

	sceGuFinish();
	sceGuSync(0,0);

	sceDisplayWaitVblankStart();
	sceGuDisplay(GU_TRUE);
	// finish
}

void SetupProjection( void )
{
	sceGuStart( GU_DIRECT, dList );

	// setup matrices for the triangle
	sceGumMatrixMode(GU_PROJECTION);
	sceGumLoadIdentity();
	sceGumPerspective( 75.0f, 16.0f/9.0f, 0.5f, 1000.0f);

	sceGumMatrixMode(GU_VIEW);
	sceGumLoadIdentity();

	sceGuClearColor( GU_COLOR( 0.0f, 0.0f, 0.0f, 1.0f ) );	// 0xff554433 == cool bg color
	sceGuClearDepth(0);
	
	sceGuFinish( );
	
}

void DrawScene( void )
{
	sceGuStart( GU_DIRECT, dList );							// Starts the display list
		
	// clear screen
	sceGuClear(GU_COLOR_BUFFER_BIT|GU_DEPTH_BUFFER_BIT);	// Clears the Color and Depth Buffer
	sceGumMatrixMode(GU_MODEL);								// Selects the Model Matrix
	sceGumLoadIdentity();									// And Reset it
	
	{	// Move 1.5 units left and 3 units back
		ScePspFVector3 move = { -1.5f, 0.0f, -3.0f };
		
		sceGumTranslate( &move );
		sceGumRotateY( rtri );								// Rotate the triangle on the Y-axis
	}

	// Draw Pyramid
	sceGumDrawArray( GU_TRIANGLES, GU_COLOR_8888|GU_VERTEX_32BITF|GU_TRANSFORM_3D,
					3*4, 0, pyramid );						// Draw the Pyramid (NEW)

	sceGumLoadIdentity();		// Reset the Matrix
	{	// Move 1.5 units left and 4 units back
		ScePspFVector3 move = { 1.5f, 0.0f, -4.0f };
		ScePspFVector3 rot  = { rquad, rquad, rquad };		// Define rotation structure (NEW)

		sceGumTranslate( &move );							// Move the Object
		sceGumRotateXYZ( &rot );							// Rotate the Object (NEW)
	}

	// Draw Cube
	sceGumDrawArray( GU_TRIANGLES, GU_COLOR_8888|GU_VERTEX_32BITF|GU_TRANSFORM_3D,
					3*12, 0, cube );						// Draw the Cube (NEW)

	rtri += (1.0f * dt);
	rquad-= (1.0f * dt);

	sceGuFinish();
	sceGuSync(0,0);	

}
