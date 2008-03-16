// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2008 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

uniform bool textureEnabled;

uniform sampler2D texture;
// 4 = Indexed4, 8 = Indexed8, 16 = Indexed16, 32 = Indexed32
uniform int textureFormat;

void main()
{
	vec4 sample = gl_Color;
	if( textureEnabled == true )
	{
		vec4 textureSample = texture2D( texture, gl_TexCoord[ 0 ].st );
		sample.rgb *= textureSample.rgb;
		sample.a = textureSample.a; // WRONG - maybe depends on lighting mode or something
	}
	
	gl_FragColor = sample;
}
