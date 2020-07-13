Shader "Custom/Blur"
{
        Properties{
            _Color("Main Color", Color) = (0.2,0.8,1.0,1)
			_Strength("Blur Strength", Float) = 3
            _MainTex("Texture", 2D) = "white" { }
        }

        SubShader{


			// Horizontal Pass
            Pass {

                CGPROGRAM

                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                fixed4 _Color;
                sampler2D _MainTex;
				float4 _MainTex_ST;
				float4 _MainTex_TexelSize;
				float _Strength;

                struct v2f {
                    float4 pos : SV_POSITION;
                    float2 uv : TEXCOORD0;
                };

                v2f vert(appdata_base v)
                {
                    v2f o;
                    o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                    o.pos = UnityObjectToClipPos(v.vertex);
                    return o;
                }

				#include "BlurUtils.cginc"

                fixed4 frag(v2f i) : SV_Target
                {
                    float horizontalOffset = _MainTex_TexelSize.x;

                    int kernelSize = 2 * int(2 * _Strength) + 3;
                    int halfSize = kernelSize / 2;
                    float kernel[KERNEL_PHYSICAL_SIZE];
                    makeGaussianKernel(_Strength, kernelSize, kernel);

                    float2 currentUV = i.uv;
                    //#if UNITY_UV_STARTS_AT_TOP
                        //currentUV.y = 1 - currentUV.y;
                    //#endif

					float4 texColor = float4(0, 0, 0, 1);
					for (float index = -halfSize; index < halfSize+1; index++) {
                        float2 current = currentUV;
						float coef = kernel[index + halfSize];
                        current.x += index * horizontalOffset;
						texColor.xyz += tex2D(_MainTex, current) * coef;
					}
					
					return texColor;
                }

                ENDCG
            }

			GrabPass { }

			// Vertical Pass
			Pass {

				CGPROGRAM

				#pragma vertex vert
				#pragma fragment frag
				#include "UnityCG.cginc"

				fixed4 _Color;
				sampler2D _GrabTexture : register(s0);
				float4 _GrabTexture_ST;
				float4 _GrabTexture_TexelSize;
                float4 _MainTex_TexelSize;
				float _Strength;

				struct v2f {
					float4 pos : SV_POSITION;
					float2 uv : TEXCOORD0;
				};

				v2f vert(appdata_base v)
				{
					v2f o;
					o.uv = TRANSFORM_TEX(v.texcoord, _GrabTexture);
					o.pos = UnityObjectToClipPos(v.vertex);
					return o;
				}

				#include "BlurUtils.cginc"

				fixed4 frag(v2f i) : SV_Target
				{
					float verticalOffset = _GrabTexture_TexelSize.y;
					int kernelSize = 2 * int(2 * _Strength) + 3;
					int halfSize = kernelSize / 2;
					float kernel[KERNEL_PHYSICAL_SIZE];
					makeGaussianKernel(_Strength, kernelSize, kernel);

                    float2 currentUV = i.uv;
                    //#if UNITY_UV_STARTS_AT_TOP
                    //    currentUV.y = 1 - currentUV.y;
                    //#endif

					float4 texColor = float4(0, 0, 0, 1);
					for (float index = -halfSize; index < halfSize + 1; index++) {
                        float2 current = currentUV;
						float coef = kernel[index + halfSize];
						current.y += index * verticalOffset;
						texColor.xyz += tex2D(_GrabTexture, current) * coef;
					}

					return texColor;
				}

				ENDCG
			}
        }
    }