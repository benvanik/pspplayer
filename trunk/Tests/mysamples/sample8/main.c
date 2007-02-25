// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

//**************************************************************************
//		PSPGU Tutorial: 'Lesson6' - main.cpp
//**************************************************************************

#include "main.h"

PSP_MODULE_INFO( "sample8", 0, 1, 1 );
PSP_MAIN_THREAD_ATTR(THREAD_ATTR_USER);

int fps = 0;			// for calculating the frames per second
char *fpsDisplay;
void *dList;			// display List, used by sceGUStart
void *fbp0;				// frame buffer

u32 tickResolution;
u64 fpsTickNow;
u64 fpsTickLast;

bool exiting = false;
bool light = false;		// Lighting ON/OFF (NEW)
bool texFilter = false; // Texture Filter, NEAREST / LINEAR (NEW)

float xrot = 0.0f;		// X Rotation (NEW)
float yrot = 0.0f;		// Y Rotation (NEW)
float xspeed = 0.0f;	// X Speed (NEW)
float yspeed = 0.0f;	// Y Speed (NEW)
float z = -3.0f;        // Z Position (NEW)

float dt;				// For time based animation

unsigned int color = GU_COLOR( 1.0f, 1.0f, 1.0f, 1.0f );

CTGATexture texture;

Vertex __attribute__((aligned(16))) quad[3*12] = 
{
	{ 0, 1, color, 0.0f, 1.0f, 0.0f,-1.0f, 1.0f,-1.0f },	// Top
	{ 1, 1, color, 0.0f, 1.0f, 0.0f, 1.0f, 1.0f,-1.0f },
	{ 0, 0, color, 0.0f, 1.0f, 0.0f,-1.0f, 1.0f, 1.0f },

	{ 0, 0, color, 0.0f, 1.0f, 0.0f,-1.0f, 1.0f, 1.0f },
	{ 1, 1, color, 0.0f, 1.0f, 0.0f, 1.0f, 1.0f,-1.0f },
	{ 1, 0, color, 0.0f, 1.0f, 0.0f, 1.0f, 1.0f, 1.0f },

	{ 0, 1, color, 0.0f,-1.0f, 0.0f,-1.0f,-1.0f, 1.0f },	// Bottom
	{ 1, 1, color, 0.0f,-1.0f, 0.0f, 1.0f,-1.0f, 1.0f },
	{ 0, 0, color, 0.0f,-1.0f, 0.0f,-1.0f,-1.0f,-1.0f },

	{ 0, 0, color, 0.0f,-1.0f, 0.0f,-1.0f,-1.0f,-1.0f },
	{ 1, 1, color, 0.0f,-1.0f, 0.0f, 1.0f,-1.0f, 1.0f },
	{ 1, 0, color, 0.0f,-1.0f, 0.0f, 1.0f,-1.0f,-1.0f },

	{ 0, 1, color, 0.0f, 0.0f, 1.0f,-1.0f, 1.0f, 1.0f },	// Front
	{ 1, 1, color, 0.0f, 0.0f, 1.0f, 1.0f, 1.0f, 1.0f },
	{ 0, 0, color, 0.0f, 0.0f, 1.0f,-1.0f,-1.0f, 1.0f },

	{ 0, 0, color, 0.0f, 0.0f, 1.0f,-1.0f,-1.0f, 1.0f },
	{ 1, 1, color, 0.0f, 0.0f, 1.0f, 1.0f, 1.0f, 1.0f },
	{ 1, 0, color, 0.0f, 0.0f, 1.0f, 1.0f,-1.0f, 1.0f },

	{ 0, 1, color, 0.0f, 0.0f,-1.0f,-1.0f,-1.0f,-1.0f },	// Back
	{ 1, 1, color, 0.0f, 0.0f,-1.0f, 1.0f,-1.0f,-1.0f },
	{ 0, 0, color, 0.0f, 0.0f,-1.0f,-1.0f, 1.0f,-1.0f },

	{ 0, 0, color, 0.0f, 0.0f,-1.0f,-1.0f, 1.0f,-1.0f },
	{ 1, 1, color, 0.0f, 0.0f,-1.0f, 1.0f,-1.0f,-1.0f },
	{ 1, 0, color, 0.0f, 0.0f,-1.0f, 1.0f, 1.0f,-1.0f },

	{ 0, 1, color,-1.0f, 0.0f, 0.0f,-1.0f, 1.0f,-1.0f },	// Left
	{ 1, 1, color,-1.0f, 0.0f, 0.0f,-1.0f, 1.0f, 1.0f },
	{ 0, 0, color,-1.0f, 0.0f, 0.0f,-1.0f,-1.0f,-1.0f },

	{ 0, 0, color,-1.0f, 0.0f, 0.0f,-1.0f,-1.0f,-1.0f },
	{ 1, 1, color,-1.0f, 0.0f, 0.0f,-1.0f, 1.0f, 1.0f },
	{ 1, 0, color,-1.0f, 0.0f, 0.0f,-1.0f,-1.0f, 1.0f },

	{ 0, 1, color, 1.0f, 0.0f, 0.0f, 1.0f, 1.0f, 1.0f },	// Right
	{ 1, 1, color, 1.0f, 0.0f, 0.0f, 1.0f, 1.0f,-1.0f },
	{ 0, 0, color, 1.0f, 0.0f, 0.0f, 1.0f,-1.0f, 1.0f },

	{ 0, 0, color, 1.0f, 0.0f, 0.0f, 1.0f,-1.0f, 1.0f },
	{ 1, 1, color, 1.0f, 0.0f, 0.0f, 1.0f, 1.0f,-1.0f },
	{ 1, 0, color, 1.0f, 0.0f, 0.0f, 1.0f,-1.0f,-1.0f }

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
	SceCtrlData pad, lastPad;						// Controller structure				
	sceCtrlReadBufferPositive(&lastPad, 1);

	SetupCallbacks();
	pspDebugScreenInit();
	sceKernelDcacheWritebackAll();

	if( !texture.LoadTGA( "Data/Crate.tga" ) ) {
		sceKernelExitGame();
	}
	texture.Swizzle();

	InitGU();
	SetupProjection();

	while( !exiting )
	{
		sceCtrlPeekBufferPositive(&pad, 1);			
		if( pad.Buttons != lastPad.Buttons ) {		// Thanks to Insomniac for the hint!
			lastPad = pad;
			
			if( pad.Buttons & PSP_CTRL_CROSS ) {
				light = !light;	
			}
			if( pad.Buttons & PSP_CTRL_SQUARE ) {
				texFilter = !texFilter;
			}
		}
		if( pad.Buttons & PSP_CTRL_LTRIGGER ) {
			z -= (1.0f * dt);
		}
		if( pad.Buttons & PSP_CTRL_RTRIGGER ) {
			z += (1.0f * dt);
		}
		if( pad.Buttons & PSP_CTRL_UP ) {
			xspeed -= 0.01f;
		}
		if( pad.Buttons & PSP_CTRL_DOWN ) {
			xspeed += 0.01f;
		}
		if( pad.Buttons & PSP_CTRL_LEFT ) {
			yspeed -= 0.01f;
		}
		if( pad.Buttons & PSP_CTRL_RIGHT ) {
			yspeed += 0.01f;
		}

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
	sceGuEnable( GU_TEXTURE_2D );
	sceGuEnable( GU_LIGHT0 );						// Enable Light 1 (NEW)

	// setup texture
	// 32-bit image, if we swizzled the texture will return true, otherwise false (NEW)
	sceGuTexMode( GU_PSM_8888, 0, 0, texture.Swizzled() );	
	sceGuTexFunc( GU_TFX_MODULATE, GU_TCC_RGB );	// Modulate the color of the image
	sceGuTexScale( 1.0f, 1.0f );					// No scaling
	sceGuTexOffset( 0.0f, 0.0f );

	ScePspFVector3 lightPosition = { 0.0f, 0.0f, 2.0f };
    sceGuLight( 0, GU_POINTLIGHT, GU_AMBIENT_AND_DIFFUSE, &lightPosition );
	sceGuLightColor( 0, GU_AMBIENT, GU_COLOR( 0.25f, 0.25f, 0.25f, 1.0f ));
	sceGuLightColor( 0, GU_DIFFUSE, GU_COLOR( 1.0f, 1.0f, 1.0f, 1.0f ) );

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

	if( light ) {
		sceGuEnable( GU_LIGHTING );							// Enable Lighting (NEW)
	} else {
		sceGuDisable( GU_LIGHTING );						// Disable Lighting (NEW)
	}
	if( texFilter ) {
		sceGuTexFilter( GU_LINEAR, GU_LINEAR );				// Linear filtering (Good Quality)
	} else {
		sceGuTexFilter( GU_NEAREST, GU_NEAREST );			// Nearest filtering (Bad Quality) (NEW)
	}

	
	{	// Move 1.5 units left and 3 units back
		ScePspFVector3 move = { 0.0f, 0.0f, z };			// Define position structure
		ScePspFVector3 rot  = { xrot, yrot, 0.0f };			// Define rotation structure 

		sceGumTranslate( &move );							// Move the Object
		sceGumRotateXYZ( &rot );							// Rotate the Object 
	}

	sceGuTexImage( 0, texture.Width(), texture.Height(), texture.Width(), texture.Image() );

	// Draw Quad
	sceGumDrawArray( GU_TRIANGLES, GU_TEXTURE_32BITF|GU_COLOR_8888|GU_NORMAL_32BITF|GU_VERTEX_32BITF|GU_TRANSFORM_3D,
					3*12, 0, quad );						// Draw the Cube (NEW)

	xrot+= ( xspeed * dt);
	yrot+= ( yspeed * dt);

	sceGuFinish();
	sceGuSync(0,0);	

}

//END OF FILE
