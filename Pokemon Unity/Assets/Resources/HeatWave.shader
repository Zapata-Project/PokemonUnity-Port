Shader "Dvornik/Distort" 
{
	// http://kostiantyn-dvornik.blogspot.com/2014/10/unity-3d-anoxemia-heat-distort-tutorial.html
	// http://wiki.unity3d.com/index.php?title=Shader_Code
	Properties 
	{
		_Refraction ("Refraction", Range (0.00, 100.0)) = 1.0
		_DistortTex ("Base (RGB)", 2D) = "white" {}
		_Radius ("Radius", Range(-1,1)) = 0.2
		_Amplitude ("Amplitude", Range(-10,10)) = 0.05
		_Wavesize ("WaveSize", Range(0, 1.0)) = 0.2
	}

	SubShader {

	Tags { "RenderType"="Transparent" "Queue"="Overlay" }
	LOD 100

	GrabPass {}

	CGPROGRAM
	#pragma surface surf NoLighting
	#pragma vertex vert

	// I need this for the surface shader, it's not directly reference though,
	// I guess that's because it's Lighting[NoLighting] ?
	fixed4 LightingNoLighting( SurfaceOutput s, fixed3 lightDir, fixed atten )
	{
		fixed4 c;
		c.rgb = s.Albedo;
		c.a = s.Alpha;
		return c;
	}

	sampler2D _GrabTexture : register(s0);   // no idea what register means? maybe put it on the register of the gpu, so its faster lookup times?
	sampler2D _DistortTex : register(s2);    // I don't think I need a texture, unless I need it to trigger the uv positions
	float _Refraction;

	float _Radius;
	float _Amplitude;
	float _Wavesize;

	// Vector4(1 / width, 1 / height, width, height)
	float4 _GrabTexture_TexelSize;

	struct Input 
	{
		// uv of the Texture. idk why it needs to be named, can't it just be uv?
		// scale ( I think ) (0,0) -> (1,1) where 0,0 is the bottom left of the Texture
		float2 uv_DistortTex;
		// Screen pos is (0,0) -> (1,1) where 0,0 is the bottom left of the SCREEN! and 1,1 is the Top Right of the SCREEN! NOT THE TEXTURE!
		float4 screenPos;
	};

	void vert ( inout appdata_full v, out Input o )
	{
		UNITY_INITIALIZE_OUTPUT( Input, o );
	}

	// We have to use surface shaders so we can grab the screen position 
	void surf ( Input IN, inout SurfaceOutput o ) 
	{
		float2 diff = float2( ( IN.uv_DistortTex.x - 0.5 ), IN.uv_DistortTex.y -0.5 );
		float dist = length( diff );
		float4 refrColor;

		// Get new uv position of this texture
		float2 uv_displaced = IN.uv_DistortTex.xy;
		// if ( dist > _Radius ) 
		// {
		// 	refrColor.r = 0;
		// 	if ( dist < _Radius + _Wavesize ) 
		// 	{
		// 		refrColor.g = 0;
		// 		float angle = ( dist - _Radius ) * 2 * 3.141592654 / _Wavesize;
		// 		float cossin = ( 1 - cos( angle ) ) * 0.5;

		// 		uv_displaced.x -= cossin * diff.x * _Amplitude / dist;
		// 		uv_displaced.y -= cossin * diff.y * _Amplitude / dist;
		// 	}
		// }

		// TEST: I just need to be able to convert my image's UV position to 
		// screen position so I can use my existing wave code.
		// If I can just get this basic, flip the image horizontally code 
		// working, then it should be easy
		// Flip it
		uv_displaced.x = 1 - uv_displaced.x;   // doesn't do anything

		// float3 distort = float3( IN.color.r, IN.color.g, IN.color.b );
		// float2 offset = distort * uv_displaced * _GrabTexture_TexelSize.xy;

		// Convert the texture's UV to the screen position so I can lookup the pixel at the screen's position
		float2 offset = uv_displaced * _GrabTexture_TexelSize.xy;
		IN.screenPos.xy = offset + IN.screenPos.xy; 

		o.Alpha = refrColor.a;
		o.Emission = refrColor.rgb;
	}
	ENDCG
	}
}