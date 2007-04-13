// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

//**************************************************************************
//		PSPGU Tutorial: 'Lesson5' - main.cpp
//**************************************************************************

#include "main.h"

PSP_MODULE_INFO( "sample7", 0, 1, 1 );
PSP_MAIN_THREAD_ATTR(THREAD_ATTR_USER);

bool exiting = false;
int fps = 0;			// for calculating the frames per second
char *fpsDisplay;
void *dList;			// display List, used by sceGUStart
void *fbp0;				// frame buffer

u32 tickResolution;
u64 fpsTickNow;
u64 fpsTickLast;

float xrot = 0.0f;		// X Rotation
float yrot = 0.0f;		// Y Rotation
float zrot = 0.0f;		// Z Rotation

float dt;				// For time based animation

unsigned int color = GU_COLOR( 1.0f, 1.0f, 1.0f, 1.0f );

CTGATexture texture;

Vertex __attribute__((aligned(16))) quad[3*12] = 
{
	{ 0, 1, color,-1.0f, 1.0f,-1.0f },	// Top
	{ 1, 1, color, 1.0f, 1.0f,-1.0f },
	{ 0, 0, color,-1.0f, 1.0f, 1.0f },

	{ 0, 0, color,-1.0f, 1.0f, 1.0f },
	{ 1, 1, color, 1.0f, 1.0f,-1.0f },
	{ 1, 0, color, 1.0f, 1.0f, 1.0f },

	{ 0, 1, color,-1.0f,-1.0f, 1.0f },	// Bottom
	{ 1, 1, color, 1.0f,-1.0f, 1.0f },
	{ 0, 0, color,-1.0f,-1.0f,-1.0f },

	{ 0, 0, color,-1.0f,-1.0f,-1.0f },
	{ 1, 1, color, 1.0f,-1.0f, 1.0f },
	{ 1, 0, color, 1.0f,-1.0f,-1.0f },

	{ 0, 1, color,-1.0f, 1.0f, 1.0f },	// Front
	{ 1, 1, color, 1.0f, 1.0f, 1.0f },
	{ 0, 0, color,-1.0f,-1.0f, 1.0f },

	{ 0, 0, color,-1.0f,-1.0f, 1.0f },
	{ 1, 1, color, 1.0f, 1.0f, 1.0f },
	{ 1, 0, color, 1.0f,-1.0f, 1.0f },

	{ 0, 1, color,-1.0f,-1.0f,-1.0f },	// Back
	{ 1, 1, color, 1.0f,-1.0f,-1.0f },
	{ 0, 0, color,-1.0f, 1.0f,-1.0f },

	{ 0, 0, color,-1.0f, 1.0f,-1.0f },
	{ 1, 1, color, 1.0f,-1.0f,-1.0f },
	{ 1, 0, color, 1.0f, 1.0f,-1.0f },

	{ 0, 1, color,-1.0f, 1.0f,-1.0f },	// Left
	{ 1, 1, color,-1.0f, 1.0f, 1.0f },
	{ 0, 0, color,-1.0f,-1.0f,-1.0f },

	{ 0, 0, color,-1.0f,-1.0f,-1.0f },
	{ 1, 1, color,-1.0f, 1.0f, 1.0f },
	{ 1, 0, color,-1.0f,-1.0f, 1.0f },

	{ 0, 1, color, 1.0f, 1.0f, 1.0f },	// Right
	{ 1, 1, color, 1.0f, 1.0f,-1.0f },
	{ 0, 0, color, 1.0f,-1.0f, 1.0f },

	{ 0, 0, color, 1.0f,-1.0f, 1.0f },
	{ 1, 1, color, 1.0f, 1.0f,-1.0f },
	{ 1, 0, color, 1.0f,-1.0f,-1.0f }

};

int main(int argc, char **argv)
{
	fbp0  = 0;
	dList = malloc( 1024 );
	fpsDisplay = (char*) malloc( 20*sizeof(char));
	fpsDisplay = "FPS: calculating";

	sceRtcGetCurrentTick( &fpsTickLast );
	tickResolution = sceRtcGetTickResolution();

	CTimer timer;

	SetupCallbacks();
	pspDebugScreenInit();
	sceKernelDcacheWritebackAll();

	if( !texture.LoadTGA( "Data/NeHe.tga" ) ) {
		sceKernelExitGame();
	}
	texture.Swizzle();

	InitGU();
	SetupProjection();

	while( !exiting )
	{
		dt = timer.GetDeltaTime( );
		DrawScene();
		FPS();
		//sceDisplayWaitVblankStart();
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
		sprintf( fpsDisplay, "FPS: %i", (int)fps );
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
	sceGuEnable(GU_TEXTURE_2D);					//Enable Texture2D (NEW)

	// setup texture (NEW)
	// 32-bit image, if we swizzled the texture will return true, otherwise false (NEW)
	sceGuTexMode( GU_PSM_8888, 0, 0, texture.Swizzled() );	
	sceGuTexFunc( GU_TFX_DECAL, GU_TCC_RGB );	// Apply image as a decal (NEW)
	sceGuTexFilter( GU_LINEAR, GU_LINEAR );		// Linear filtering (Good Quality) (NEW)
	sceGuTexScale( 1.0f, 1.0f );                // No scaling
	sceGuTexOffset( 0.0f, 0.0f );
	//sceGuAmbientColor( 0xffffffff );
	//sceGuTexEnvColor( 0xffffff );

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

	sceGuClearColor( GU_COLOR( 0.0f, 0.0f, 0.0f, 1.0f ) );	// 0xff554433 == cool bg color
	sceGuClearDepth(0);
	
}

void DrawScene( void )
{
	sceGuStart( GU_DIRECT, dList );							// Starts the display list
		
	// clear screen
	sceGuClear(GU_COLOR_BUFFER_BIT|GU_DEPTH_BUFFER_BIT);	// Clears the Color and Depth Buffer
	sceGumMatrixMode(GU_MODEL);								// Selects the Model Matrix
	sceGumLoadIdentity();									// And Reset it
	
	{	// Move 1.5 units left and 3 units back
		ScePspFVector3 move = { 0.0f, 0.0f, -3.0f };
		ScePspFVector3 rot  = { xrot, yrot, zrot };		// Define rotation structure (NEW)

		sceGumTranslate( &move );							// Move the Object
		sceGumRotateXYZ( &rot );							// Rotate the Object (NEW)
	}

	sceGuTexImage( 0, texture.Width(), texture.Height(), texture.Width(), texture.Image() );

	// Draw Quad
	sceGumDrawArray( GU_TRIANGLES, GU_TEXTURE_32BITF|GU_COLOR_8888|GU_VERTEX_32BITF|GU_TRANSFORM_3D,
					3*12, 0, quad );						// Draw the Cube (NEW)

	xrot+= (1.0f * dt);
	yrot+= (1.0f * dt);
	zrot+= (1.0f * dt);

	sceGuFinish();
	sceGuSync(0,0);	

}

//END OF FILE
