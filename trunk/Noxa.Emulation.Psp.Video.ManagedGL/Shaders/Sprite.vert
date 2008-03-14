// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2008 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

uniform mat4 worldMatrix;

uniform bool isTransformed;
uniform vec2 textureSize;
uniform vec2 textureOffset;
uniform vec2 textureScale;

void main()
{
	vec4 position = gl_Vertex;
	
	// Should this be here?
	position = worldMatrix * position;
	
	// Texture setup
	vec4 texCoord = ( gl_MultiTexCoord0 * textureScale ) + textureOffset;
	if( isTransformed == true )
	{
		texCoord.s /= textureSize.s;
		texCoord.t /= textureSize.t;
	}
	gl_TexCoord[ 0 ] = gl_TextureMatrix[ 0 ] * texCoord;
	
	// Lighting / materials
	// ...
	gl_FrontColor = gl_Color;
	
	gl_Position = gl_ModelViewProjectionMatrix * position;
}
