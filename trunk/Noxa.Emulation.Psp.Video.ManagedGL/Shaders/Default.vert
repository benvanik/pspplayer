// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2008 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

// Note: this matrix is precomputed for 480x272, -1 to 1 - may need to make this a uniform
const mat4 orthoMatrix = mat4( 0.004166667, 0.0, 0.0, 0.0, 0.0, -0.007352941, 0.0, 0.0, 0.0, 0.0, -1.0, 0.0, -1.0, 1.0, 0.0, 1.0 );
uniform mat4 worldMatrix;

uniform vec4 isTransformed; // {0,0,0,0} if raw, {1,1,1,1} if transformed
uniform vec4 isRaw;			// inverse of isTransformed

uniform vec2 textureSize;
uniform vec2 textureOffset;
uniform vec2 textureScale;

uniform bool isSkinned;
uniform mat4 boneMatrices[ 8 ];
attribute vec4 boneWeights03;
attribute vec4 boneWeights47;

uniform int morphCount;
uniform float morphWeights[ 8 ];

uniform bool lightingEnabled;
// .x = type, .y = mode, .z = convergence, .w = cutoff
// type: 0 = directional, 1 = point, 2 = spot
// mode: 0 = diffuse, 1 = diffuse + specular, 2 = powered specular ?
uniform bool lightEnabled[ 4 ];
uniform vec4 lightParameters[ 4 ];
uniform vec3 lightPositions[ 4 ];
uniform vec3 lightDirections[ 4 ];
uniform vec3 lightColors[ 4 ];

void main()
{
	vec4 position = gl_Vertex;
	
	// position = isTransformed ? position : ( worldMatrix * position );
	position = ( isTransformed * position ) + ( isRaw * ( worldMatrix * position ) );
	
	// Morphing
	if( morphCount != 0 )
	{
		// Vertices come through as
		// V0[ m0 ] V0[ m1 ] V0[ m2 ]...
		// V1[ m0 ] V1[ m1 ] V1[ m2 ]...
		// The morphWeights says for each mN, how strong it is from 0 to 1
		// I have no clue how to do this ;)
	}
	
	// -- Skinning --
	vec4 normal = vec4( gl_Normal.xyz, 0.0 );
	if( isSkinned == true )
	{
		//  |x|                                  |b00, b03, b06||x| |b09|
		//  |y|=sum(0..boneCount){ weight[i] * ( |b01, b04, b07||y|+|b10| ) }
		//  |z|                                  |b02, b05, b08||z| |b11|
		// |Nx|                                  |b00, b03, b06||Nx|
		// |Ny|=sum(0..boneCount){ weight[i] * ( |b01, b04, b07||Ny| ) }
		// |Nz|                                  |b02, b05, b08||Nz|
		
		// Spec does not allow this, so we just do everything below - note that this may be faster than a for loop anyway
		// If we don't want a bone factored in, make sure its weight is set to 0!
		//for( int n = 0; n < boneCount; n++ )
		//{
		//	position1 += boneWeights[ n ] * ( boneMatrices[ n ] * position );
		//	normal1 += boneWeights[ n ] * ( boneMatrices[ n ] * normal );
		//}
		
		position =
			boneWeights03.x * ( boneMatrices[ 0 ] * position ) +
			boneWeights03.y * ( boneMatrices[ 1 ] * position ) +
			boneWeights03.z * ( boneMatrices[ 2 ] * position ) +
			boneWeights03.w * ( boneMatrices[ 3 ] * position ) +
			boneWeights47.x * ( boneMatrices[ 4 ] * position ) +
			boneWeights47.y * ( boneMatrices[ 5 ] * position ) +
			boneWeights47.z * ( boneMatrices[ 6 ] * position ) +
			boneWeights47.w * ( boneMatrices[ 7 ] * position );
		normal =
			boneWeights03.x * ( boneMatrices[ 0 ] * normal ) +
			boneWeights03.y * ( boneMatrices[ 1 ] * normal ) +
			boneWeights03.z * ( boneMatrices[ 2 ] * normal ) +
			boneWeights03.w * ( boneMatrices[ 3 ] * normal ) +
			boneWeights47.x * ( boneMatrices[ 4 ] * normal ) +
			boneWeights47.y * ( boneMatrices[ 5 ] * normal ) +
			boneWeights47.z * ( boneMatrices[ 6 ] * normal ) +
			boneWeights47.w * ( boneMatrices[ 7 ] * normal );
		normal = vec4( normal.xyz, 0.0 );
	}
	
	// -- Lighting / materials --
	vec4 color = gl_Color;
	//if( lightingEnabled == true )
	//{
	//	color = gl_FrontMaterial.emission + gl_LightModel * gl_FrontMaterial.ambient;
	//	/*for( int n = 0; n < 4; n++ )
	//	{
	//		if( lightEnabled[ n ] == true )
	//		{
	//			// Light!
	//		}
	//	}*/
	//	color += gl_Color;
	//	color.a = gl_LightModel.ambient.a * gl_FrontMaterial.ambient.a;
	//}
	gl_FrontColor = color;
	
	/*
	struct gl_LightModelParameters {
		vec4 ambient;
	};
	uniform gl_LightModelParameters gl_LightModel;
	
	struct gl_LightSourceParameters {
		vec4 ambient;
		vec4 diffuse;
		vec4 specular;
		vec4 position;
		vec4 halfVector;
		vec3 spotDirection;
		float spotExponent;
		float spotCutoff;
		float spotCosCutoff;
		float constantAttenuation;
		float linearAttenuation;
		float quadraticAttenuation;
	};
	uniform gl_LightSourceParameters gl_LightSource[gl_MaxLights];
	
	struct gl_LightProducts {
		vec4 ambient;
		vec4 diffuse;
		vec4 specular;
	};
	uniform gl_LightProducts gl_FrontLightProduct[gl_MaxLights];
	
	struct gl_MaterialParameters {
		vec4 emission;
		vec4 ambient;
		vec4 diffuse;
		vec4 specular;
		float shininess;
	};
	uniform gl_MaterialParameters gl_FrontMaterial;
	*/
	
	// -- Texture setup --
	vec4 texCoord = gl_MultiTexCoord0;
	// TODO: do a texture matrix multiplication instead?
	//texCoord = isTransformed ? ( texCoord / textureSize ) : ( ( texCoord * textureScale ) + textureOffset );
	texCoord.st = ( isTransformed.xy * ( texCoord.st / textureSize.st ) ) + ( isRaw.xy * ( ( texCoord.st * textureScale.st ) + textureOffset.s ) );
	//texCoord.s = ( isTransformed.x * ( texCoord.s / textureSize.s ) ) + ( isRaw.x * ( ( texCoord.s * textureScale.s ) + textureOffset.s ) );
	//texCoord.t = ( isTransformed.x * ( texCoord.t / textureSize.t ) ) + ( isRaw.x * ( ( texCoord.t * textureScale.t ) + textureOffset.t ) );
	gl_TexCoord[ 0 ] = gl_TextureMatrix[ 0 ] * texCoord;
	
	// gl_Position = isTransformed ? ( orthoMatrix * position ) : ( gl_MVP * position );
	gl_Position = ( isTransformed * ( orthoMatrix * position ) ) + ( isRaw * ( gl_ModelViewProjectionMatrix * position ) );
}
