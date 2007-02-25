//**************************************************************************
//		PSPGU Tutorial: 'Lesson5' - TGALoader.h
//**************************************************************************

#ifndef __CTGATexture__H
#define __CTGATexture__H

#include <psptypes.h>

class CTGATexture
{
private:
	int				type;
	bool			swizzled;
	unsigned int	imageWidth;
	unsigned int	imageHeight;
	unsigned char	*image;
	
public:
	CTGATexture( void );
	~CTGATexture( void );

	int Width( void )				{ return imageWidth; }
	int Height( void )				{ return imageHeight; }
	bool Swizzled( void )			{ return swizzled; }
	unsigned char* Image( void )	{ return image; }

	bool LoadTGA( char *filename );	// Load the TGA file
	void Swizzle( void );			// Swizzle texture
	void FreeImage( void );			// Delete image

};

#endif
