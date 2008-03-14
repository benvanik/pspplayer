// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2008 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

// NOTE: texture must be set to GL_NEAR, as we need to get specific pixels

uniform sampler2D texture;
// 4 = Indexed4, 8 = Indexed8, 16 = Indexed16, 32 = Indexed32
uniform int textureFormat;

uniform sampler1D clut;
// 0 = BGR5650, 1 = ABGR5551, 2 = ABGR4444, 3 = ABGR8888
uniform int clutFormat;
uniform int clutStart;
uniform int clutShift;
uniform int clutMask;

void main()
{
	vec4 sample = texture2D( texture, gl_TexCoord[ 0 ].st );
	int index = int( sample.r ); // ?
	index = ( ( clutStart + index ) >> clutShift ) & clutMask;
	
	if( clutFormat == 3 )
		sample = texture1D( clut, index );
	else
	{
		sample = texture1D( clut, index );
		if( clutFormat == 0 )
		{
			// 16-bit BGR 5650
		}
		else if( clutFormat == 1 )
		{
			// 16-bit ABGR 5551
		}
		else
		{
			// 16-bit ABGR 4444
		}
	}
	
	gl_FragColor = sample;
}
