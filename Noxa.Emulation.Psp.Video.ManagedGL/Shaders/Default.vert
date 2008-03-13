// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2008 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

uniform mat4 worldMatrix;

uniform int boneCount;
uniform mat4 boneMatrices[ 8 ];
attribute float boneWeights[ 8 ];

uniform int morphCount;
uniform float morphWeights[ 8 ];

void main()
{
	vec4 position = gl_Vertex;
	
	// Texture setup
	gl_TexCoord[ 0 ] = gl_TextureMatrix[ 0 ] * gl_MultiTexCoord0;
	
	// Morphing
	if( morphCount != 0 )
	{
		// Vertices come through as
		// V0[ m0 ] V0[ m1 ] V0[ m2 ]...
		// V1[ m0 ] V1[ m1 ] V1[ m2 ]...
		// The morphWeights says for each mN, how strong it is from 0 to 1
		// I have no clue how to do this ;)
	}
	
	// Skinning
	vec3 normal = gl_Normal;
	if( boneCount != 0 )
	{
		//  |x|                                  |b00, b03, b06||x| |b09|
		//  |y|=sum(0..boneCount){ weight[i] * ( |b01, b04, b07||y|+|b10| ) }
		//  |z|                                  |b02, b05, b08||z| |b11|
		// |Nx|                                  |b00, b03, b06||Nx|
		// |Ny|=sum(0..boneCount){ weight[i] * ( |b01, b04, b07||Ny| ) }
		// |Nz|                                  |b02, b05, b08||Nz|
		vec3 position1 = vec3( 0, 0, 0 );
		vec3 normal1 = vec3( 0, 0, 0, );
		for( int n = 0; n < boneCount; n++ )
		{
			position1 += boneWeights[ n ] * ( boneMatrices[ n ] * position );
			normal1 += boneWeights[ n ] * ( boneMatrices[ n ] * normal );
		}
		position = position1;
		normal = normal1;
	}
	
	// Lighting / materials
	// ...
	gl_FrontColor = gl_Color;
	
	gl_Position = gl_ModelViewProjectionMatrix * worldMatrix * position;
}
