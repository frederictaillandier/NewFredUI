﻿// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Window" {
    Properties {
		_StencilVal ("stencilVal", Int) = 1
		_ColorWindow ("Color Window", Color) = (1,1,1,1)

    }
	SubShader {
        Tags { "RenderType"="Opaque" "Queue"="Transparent+1"}
        Pass {
			ColorMask RGB
			
			Cull Back
			
			ZWrite Off
            Stencil {
                Ref [_StencilVal]
                Comp always
                Pass replace
				
            }
        
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
			uniform float4 _ColorWindow;
            struct appdata {
                float4 vertex : POSITION;
            };
            struct v2f {
                float4 pos : SV_POSITION;
            };
            v2f vert(appdata v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }
            half4 frag(v2f i) : SV_Target {
                return _ColorWindow;
            }
            ENDCG
        }
    } 
}