// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Cg per-pixel lighting with vertex lights" {
   Properties {
      _Color ("Diffuse Material Color", Color) = (1,1,1,1) 
      _SpecColor ("Specular Material Color", Color) = (1,1,1,1) 
      _Shininess ("Shininess", Float) = 10
	 
	_StencilVal ("stencilVal", Int) = 1
		cutZ ("cutZ", Float) = 0.5
		color_map ("color map", 2D) = "white" {}

   }
   SubShader {
   Tags { "Queue"="Transparent+2"}

      Pass {     
	   Stencil {
                Ref [_StencilVal]
                Comp equal
            } 
	  
         Tags { "LightMode" = "ForwardBase" } // pass for 
            // 4 vertex lights, ambient light & first pixel light
 
         CGPROGRAM
         #pragma multi_compile_fwdbase 
         #pragma vertex vert
         #pragma fragment frag
 
         #include "UnityCG.cginc" 
         uniform float4 _LightColor0; 
            // color of light source (from "Lighting.cginc")
 
         // User-specified properties
         uniform float4 _Color; 
         uniform float4 _SpecColor; 
         uniform float _Shininess;
 
 uniform float4 section_depth;

uniform sampler2D color_map;
uniform float cutZ;

 float4x4 rotate(float3 r) 
{ 
	float3 c, s; 
	sincos(r.x, s.x, c.x); 
	sincos(r.y, s.y, c.y); 
	sincos(r.z, s.z, c.z);
	return float4x4( c.y*c.z,	 -s.z,     s.y, 0, 
						 s.z, c.x*c.z,    -s.x, 0, 
						-s.y,     s.x, c.x*c.y, 0, 
						   0,       0,       0, 1 );
} 


         struct vertexInput {
            float4 vertex : POSITION;
			float4 color	: COLOR;
			float2 texcoord : TEXCOORD;
            float3 normal : NORMAL;
         };
         struct vertexOutput {
            float4 pos : SV_POSITION;
            float4 posWorld : TEXCOORD0;
            float3 normalDir : TEXCOORD1;
            float3 vertexLighting : TEXCOORD2;
			float4 mask		: TEXCOORD3;
			float2 texcoord : TEXCOORD4;	
         };
 
         vertexOutput vert(vertexInput input)
         {          
            vertexOutput output; 

            float4x4 modelMatrix = unity_ObjectToWorld;
            float4x4 modelMatrixInverse = unity_WorldToObject; 
               // unity_Scale.w is unnecessary here
 
            output.posWorld = mul(modelMatrix, input.vertex);
            output.normalDir = normalize(
               mul(float4(input.normal, 0.0), modelMatrixInverse).xyz);
            output.pos = UnityObjectToClipPos(input.vertex);
 
	output.mask = input.vertex;
	output.texcoord = input.texcoord;
	
            // Diffuse reflection by four "vertex lights"            
            output.vertexLighting = float3(0.0, 0.0, 0.0);
            #ifdef VERTEXLIGHT_ON
            for (int index = 0; index < 4; index++)
            {    
               float4 lightPosition = float4(unity_4LightPosX0[index], 
                  unity_4LightPosY0[index], 
                  unity_4LightPosZ0[index], 1.0);
 
               float3 vertexToLightSource = 
                  lightPosition.xyz - output.posWorld.xyz;        
               float3 lightDirection = normalize(vertexToLightSource);
               float squaredDistance = 
                  dot(vertexToLightSource, vertexToLightSource);
               float attenuation = 1.0 / (1.0 + 
                  unity_4LightAtten0[index] * squaredDistance);
               float3 diffuseReflection = attenuation 
                  * unity_LightColor[index].rgb * _Color.rgb 
                  * max(0.0, dot(output.normalDir, lightDirection));         
 
               output.vertexLighting = 
                  output.vertexLighting + diffuseReflection;
            }
            #endif
            return output;
         }
 
         float4 frag(vertexOutput input) : COLOR
         {
            float3 normalDirection = normalize(input.normalDir); 
            float3 viewDirection = normalize(
               _WorldSpaceCameraPos - input.posWorld.xyz);
            float3 lightDirection;
            float attenuation;
 
            if (0.0 == _WorldSpaceLightPos0.w) // directional light?
            {
               attenuation = 1.0; // no attenuation
               lightDirection = 
                  normalize(_WorldSpaceLightPos0.xyz);
            } 
            else // point or spot light
            {
               float3 vertexToLightSource = 
                  _WorldSpaceLightPos0.xyz - input.posWorld.xyz;
               float distance = length(vertexToLightSource);
               attenuation = 1.0 / distance; // linear attenuation 
               lightDirection = normalize(vertexToLightSource);
            }
 
			_Color.rgb = tex2D(color_map, input.texcoord).xyz * _Color.rgb;

            float3 ambientLighting = 
                UNITY_LIGHTMODEL_AMBIENT.rgb * _Color.rgb;
 
            float3 diffuseReflection = 
               attenuation * _LightColor0.rgb * _Color.rgb 
               * max(0.0, dot(normalDirection, lightDirection));
 
            float3 specularReflection;
            if (dot(normalDirection, lightDirection) < 0.0) 
               // light source on the wrong side?
            {
               specularReflection = float3(0.0, 0.0, 0.0); 
                  // no specular reflection
            }
            else // light source on the right side
            {
               specularReflection = attenuation * _LightColor0.rgb 
                  * _SpecColor.rgb * pow(max(0.0, dot(
                  reflect(-lightDirection, normalDirection), 
                  viewDirection)), _Shininess);
            }
 
            return float4(input.vertexLighting + ambientLighting 
               + diffuseReflection + specularReflection, 1.0);
         }
         ENDCG
      }
	   Pass {     
	  
	   Stencil {
                Ref 0
                Comp equal
            } 	  
         Tags { "LightMode" = "ForwardBase" } // pass for 
            // 4 vertex lights, ambient light & first pixel light
 
         CGPROGRAM
         #pragma multi_compile_fwdbase 
         #pragma vertex vert
         #pragma fragment frag
 
         #include "UnityCG.cginc" 
         uniform float4 _LightColor0; 
            // color of light source (from "Lighting.cginc")
 
         // User-specified properties
         uniform float4 _Color; 
         uniform float4 _SpecColor; 
         uniform float _Shininess;
 
 uniform float4 section_depth;

uniform sampler2D color_map;
uniform float cutZ;

 float4x4 rotate(float3 r) 
{ 
	float3 c, s; 
	sincos(r.x, s.x, c.x); 
	sincos(r.y, s.y, c.y); 
	sincos(r.z, s.z, c.z);
	return float4x4( c.y*c.z,	 -s.z,     s.y, 0, 
						 s.z, c.x*c.z,    -s.x, 0, 
						-s.y,     s.x, c.x*c.y, 0, 
						   0,       0,       0, 1 );
} 


         struct vertexInput {
            float4 vertex : POSITION;
			float4 color	: COLOR;
			float2 texcoord : TEXCOORD;
            float3 normal : NORMAL;
         };
         struct vertexOutput {
            float4 pos : SV_POSITION;
            float4 posWorld : TEXCOORD0;
            float3 normalDir : TEXCOORD1;
            float3 vertexLighting : TEXCOORD2;
			float4 mask		: TEXCOORD3;
			float2 texcoord : TEXCOORD4;	
         };
 
         vertexOutput vert(vertexInput input)
         {          
            vertexOutput output;
 

            float4x4 modelMatrix = unity_ObjectToWorld;
            float4x4 modelMatrixInverse = unity_WorldToObject; 
               // unity_Scale.w is unnecessary here
 
            output.posWorld = mul(modelMatrix, input.vertex);
            output.normalDir = normalize(
               mul(float4(input.normal, 0.0), modelMatrixInverse).xyz);
            output.pos = UnityObjectToClipPos(input.vertex);
 
  //float4x4 r 	 = rotate(radians(section_depth.xyz));
//	float4 c 	 = input.vertex;

	output.mask = input.vertex;
	output.texcoord = input.texcoord;




            // Diffuse reflection by four "vertex lights"            
            output.vertexLighting = float3(0.0, 0.0, 0.0);
            #ifdef VERTEXLIGHT_ON
            for (int index = 0; index < 4; index++)
            {    
               float4 lightPosition = float4(unity_4LightPosX0[index], 
                  unity_4LightPosY0[index], 
                  unity_4LightPosZ0[index], 1.0);
 
               float3 vertexToLightSource = 
                  lightPosition.xyz - output.posWorld.xyz;        
               float3 lightDirection = normalize(vertexToLightSource);
               float squaredDistance = 
                  dot(vertexToLightSource, vertexToLightSource);
               float attenuation = 1.0 / (1.0 + 
                  unity_4LightAtten0[index] * squaredDistance);
               float3 diffuseReflection = attenuation 
                  * unity_LightColor[index].rgb * _Color.rgb 
                  * max(0.0, dot(output.normalDir, lightDirection));         
 
               output.vertexLighting = 
                  output.vertexLighting + diffuseReflection;
            }
            #endif
            return output;
         }
 
         float4 frag(vertexOutput input) : COLOR
         {


		 if(input.mask.y < cutZ)
		discard;


            float3 normalDirection = normalize(input.normalDir); 
            float3 viewDirection = normalize(
               _WorldSpaceCameraPos - input.posWorld.xyz);
            float3 lightDirection;
            float attenuation;
 
            if (0.0 == _WorldSpaceLightPos0.w) // directional light?
            {
               attenuation = 1.0; // no attenuation
               lightDirection = 
                  normalize(_WorldSpaceLightPos0.xyz);
            } 
            else // point or spot light
            {
               float3 vertexToLightSource = 
                  _WorldSpaceLightPos0.xyz - input.posWorld.xyz;
               float distance = length(vertexToLightSource);
               attenuation = 1.0 / distance; // linear attenuation 
               lightDirection = normalize(vertexToLightSource);
            }
 
 _Color.rgb = tex2D(color_map, input.texcoord).xyz * _Color.rgb;

            float3 ambientLighting = 
                UNITY_LIGHTMODEL_AMBIENT.rgb * _Color.rgb;
 
            float3 diffuseReflection = 
               attenuation * _LightColor0.rgb * _Color.rgb 
               * max(0.0, dot(normalDirection, lightDirection));
 
            float3 specularReflection;
            if (dot(normalDirection, lightDirection) < 0.0) 
               // light source on the wrong side?
            {
               specularReflection = float3(0.0, 0.0, 0.0); 
                  // no specular reflection
            }
            else // light source on the right side
            {
               specularReflection = attenuation * _LightColor0.rgb 
                  * _SpecColor.rgb * pow(max(0.0, dot(
                  reflect(-lightDirection, normalDirection), 
                  viewDirection)), _Shininess);
            }
 
            return float4(input.vertexLighting + ambientLighting 
               + diffuseReflection + specularReflection, 1.0);
         }
         ENDCG
      }
      Pass {    
         Tags { "LightMode" = "ForwardAdd" } 

		 
	   Stencil {
                Ref 0
                Comp equal
            } 

            // pass for additional light sources
         Blend One One // additive blending 
 
          CGPROGRAM
 
         #pragma vertex vert  
         #pragma fragment frag 
 
         #include "UnityCG.cginc" 
         uniform float4 _LightColor0; 
            // color of light source (from "Lighting.cginc")
 
         // User-specified properties
         uniform float4 _Color; 
         uniform float4 _SpecColor; 
         uniform float _Shininess;
 
  uniform float4 section_depth;

uniform sampler2D color_map;
 uniform float cutZ;

  float4x4 rotate(float3 r) 
{ 
	float3 c, s; 
	sincos(r.x, s.x, c.x); 
	sincos(r.y, s.y, c.y); 
	sincos(r.z, s.z, c.z);
	return float4x4( c.y*c.z,	 -s.z,     s.y, 0, 
						 s.z, c.x*c.z,    -s.x, 0, 
						-s.y,     s.x, c.x*c.y, 0, 
						   0,       0,       0, 1 );
} 

         struct vertexInput {
            float4 vertex : POSITION;
				float4 color	: COLOR;
			float2 texcoord : TEXCOORD;
            float3 normal : NORMAL;
         };
         struct vertexOutput {
            float4 pos : SV_POSITION;
            float4 posWorld : TEXCOORD0;
            float3 normalDir : TEXCOORD1;
			float4 mask		: TEXCOORD2;
			float2 texcoord : TEXCOORD3;	
         };
 
         vertexOutput vert(vertexInput input) 
         {
            vertexOutput output;
 
            float4x4 modelMatrix = unity_ObjectToWorld;
            float4x4 modelMatrixInverse = unity_WorldToObject; 
               // multiplication with unity_Scale.w is unnecessary 
               // because we normalize transformed vectors
 
            output.posWorld = mul(modelMatrix, input.vertex);
            output.normalDir = normalize(
               mul(float4(input.normal, 0.0), modelMatrixInverse).xyz);
            output.pos = UnityObjectToClipPos(input.vertex);

		//	  float4x4 r 	 = rotate(radians(section_depth.xyz));
	//float4 c 	 =  output.pos;

	output.mask =input.vertex;
	output.texcoord = input.texcoord;

            return output;
         }
 
         float4 frag(vertexOutput input) : COLOR
         {
		  if(input.mask.y > cutZ)
		discard;

            float3 normalDirection = normalize(input.normalDir);
 
            float3 viewDirection = normalize(
               _WorldSpaceCameraPos.xyz - input.posWorld.xyz);
            float3 lightDirection;
            float attenuation;
 
            if (0.0 == _WorldSpaceLightPos0.w) // directional light?
            {
               attenuation = 1.0; // no attenuation
               lightDirection = 
                  normalize(_WorldSpaceLightPos0.xyz);
            } 
            else // point or spot light
            {
               float3 vertexToLightSource = 
                  _WorldSpaceLightPos0.xyz - input.posWorld.xyz;
               float distance = length(vertexToLightSource);
               attenuation = 1.0 / distance; // linear attenuation 
               lightDirection = normalize(vertexToLightSource);
            }
 
 _Color.rgb = tex2D(color_map, input.texcoord).xyz * _Color.rgb;

            float3 diffuseReflection = 
               attenuation * _LightColor0.rgb * _Color.rgb
               * max(0.0, dot(normalDirection, lightDirection));
 
            float3 specularReflection;
            if (dot(normalDirection, lightDirection) < 0.0) 
               // light source on the wrong side?
            {
               specularReflection = float3(0.0, 0.0, 0.0); 
                  // no specular reflection
            }
            else // light source on the right side
            {
               specularReflection = attenuation * _LightColor0.rgb 
                  * _SpecColor.rgb * pow(max(0.0, dot(
                  reflect(-lightDirection, normalDirection), 
                  viewDirection)), _Shininess);
            }
 
            return float4(diffuseReflection 
               + specularReflection, 1.0);
               // no ambient lighting in this pass
         }
 
         ENDCG
      }
 
   } 
   // The definition of a fallback shader should be commented out 
   // during development:
   // Fallback "Specular"

}