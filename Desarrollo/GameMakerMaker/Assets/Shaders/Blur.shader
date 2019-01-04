// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

/*===============================================================================
Copyright 2017-2018 PTC Inc.

Licensed under the Apache License, Version 2.0 (the "License"); you may not
use this file except in compliance with the License. You may obtain a copy of
the License at

http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software distributed
under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
CONDITIONS OF ANY KIND, either express or implied. See the License for the
specific language governing permissions and limitations under the License.
===============================================================================*/

Shader "Custom/Blur" {
    // Used to render the Vuforia Video Background

 Properties
    {
        // we have removed support for texture tiling/offset,
        // so make them not be displayed in material inspector
        [NoScaleOffset] _MainTex ("Texture", 2D) = "white" {}
		[Range] _Blur ("isBlur",float) = 0
    }
    SubShader
    {
    Tags { "QUEUE"="geometry-11" "RenderType"="Opaque" }
        Pass
        {
            Tags { "QUEUE"="geometry-11" "RenderType"="Opaque" }
            ZWrite Off
            Cull Off
            
            CGPROGRAM
            // use "vert" function as the vertex shader
            #pragma vertex vert
            // use "frag" function as the pixel (fragment) shader
            #pragma fragment frag
			
			#include "UnityCG.cginc"

            // vertex shader inputs
            struct appdata
            {
                float4 vertex : POSITION; // vertex position
                float2 uv : TEXCOORD0; // texture coordinate
            };

            // vertex shader outputs ("vertex to fragment")
            struct v2f
            {
                float2 uv : TEXCOORD0; // texture coordinate
                float4 vertex : SV_POSITION; // clip space position
            };

            // vertex shader
            v2f vert (appdata v)
            {
                v2f o;
                // transform position to clip space
                // (multiply with model*view*projection matrix)
                o.vertex = UnityObjectToClipPos(v.vertex);
                // just pass the texture coordinate
                o.uv = v.uv;
                return o;
            }
            
            // texture we will sample
            sampler2D _MainTex;
			float _Blur;
			float bluramout;
			float4 blurFragment(float2 uv) {
				// accumulate the color for this pixel by sampling neighboring pixels.
				float4 col = tex2D(_MainTex, uv); // center pixel color.

				// top row.
				col += tex2D(_MainTex, float2(uv.x + (bluramout * 2), uv.y - (bluramout * 2)));
				col += tex2D(_MainTex, float2(uv.x + (bluramout * 1), uv.y - (bluramout * 2)));
				col += tex2D(_MainTex, float2(uv.x, uv.y - (bluramout * 2)));
				col += tex2D(_MainTex, float2(uv.x + (bluramout * 1), uv.y - (bluramout * 2)));
				col += tex2D(_MainTex, float2(uv.x + (bluramout * 2), uv.y - (bluramout * 2)));

				// 2nd row.
				col += tex2D(_MainTex, float2(uv.x + (bluramout * 2), uv.y - (bluramout * 1)));
				col += tex2D(_MainTex, float2(uv.x + (bluramout * 1), uv.y - (bluramout * 1)));
				col += tex2D(_MainTex, float2(uv.x, uv.y - (bluramout * 1)));
				col += tex2D(_MainTex, float2(uv.x + (bluramout * 1), uv.y - (bluramout * 1)));
				col += tex2D(_MainTex, float2(uv.x + (bluramout * 2), uv.y - (bluramout * 1)));

				// middle row (note that we occluded middle pixel because it's handled above.
				col += tex2D(_MainTex, float2(uv.x + (bluramout * 2), uv.y));
				col += tex2D(_MainTex, float2(uv.x + (bluramout * 1), uv.y));
				col += tex2D(_MainTex, float2(uv.x + (bluramout * 1), uv.y));
				col += tex2D(_MainTex, float2(uv.x + (bluramout * 2), uv.y));


				// 4th row.
				col += tex2D(_MainTex, float2(uv.x + (bluramout * 2), uv.y + (bluramout * 1)));
				col += tex2D(_MainTex, float2(uv.x + (bluramout * 1), uv.y + (bluramout * 1)));
				col += tex2D(_MainTex, float2(uv.x, uv.y + (bluramout * 1)));
				col += tex2D(_MainTex, float2(uv.x + (bluramout * 1), uv.y + (bluramout * 1)));
				col += tex2D(_MainTex, float2(uv.x + (bluramout * 2), uv.y + (bluramout * 1)));

				// bottom row.
				col += tex2D(_MainTex, float2(uv.x + (bluramout * 2), uv.y + (bluramout * 2)));
				col += tex2D(_MainTex, float2(uv.x + (bluramout * 1), uv.y + (bluramout * 2)));
				col += tex2D(_MainTex, float2(uv.x, uv.y + (bluramout * 2)));
				col += tex2D(_MainTex, float2(uv.x + (bluramout * 1), uv.y + (bluramout * 2)));
				col += tex2D(_MainTex, float2(uv.x + (bluramout * 2), uv.y + (bluramout * 2)));

				col /= 25; // normalize values
				return col;
			}




            // pixel shader; returns low precision ("fixed4" type)
            // color ("SV_Target" semantic)
            fixed4 frag (v2f i) : SV_Target
            {
                // sample texture and return it
				fixed4 col = fixed4(0.0,0.0,0.0,0.0);
                if(_Blur == 0)
					 col = tex2D(_MainTex, i.uv);
				else {
					bluramout = _Blur / 100;
					col = blurFragment(i.uv);


					/*float remaining = 1.0f;
					float coef = 1.0;
					float fI = 0;
					for (int j = 0; j < 3; j++) {
						fI++;
						coef *= 0.33;
						col += tex2D(_MainTex, float2(i.uv.x, i.uv.y - fI * _Blur / 100)) * coef;
						col += tex2D(_MainTex, float2(i.uv.x - fI * _Blur / 100, i.uv.y)) * coef;
						col += tex2D(_MainTex, float2(i.uv.x + fI * _Blur / 100, i.uv.y)) * coef;
						col += tex2D(_MainTex, float2(i.uv.x, i.uv.y + fI * _Blur / 100)) * coef;

						remaining -= 4 * coef;
					}
					col += tex2D(_MainTex, float2(i.uv.x, i.uv.y)) * remaining;*/

					
				}
				
					
				



            #ifdef UNITY_COLORSPACE_GAMMA
                return col;
            #else
                return fixed4(GammaToLinearSpace(col.rgb), col.a);
            #endif	
            }
            ENDCG
        }
    }
    Fallback "Legacy Shaders/Diffuse"
}
