Shader "Tutorial/Display Normals" {
	Properties{
		_MainTex("Texture", 2D) = "white" {}
		_MaskTexture("Texture", 2D) = "white" {}
	}
	SubShader{
		Pass {

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION; // vertex position
				float2 uv : TEXCOORD0; // texture coordinate
			};

			struct v2f {
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			v2f vert(appdata_base v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 color = text2D(_MainTex, i.uv);
				fixed4 mask = text2D(_MaskTexture, i.uv);
				return color * mask.w;
			}
			ENDCG

		}
	}
}