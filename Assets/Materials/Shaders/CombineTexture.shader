Shader "Custom/CombineTexture"
{
    Properties
    {
        _Mask("Texture", 2D) = "white" { }
        _Source("Texture", 2D) = "white" { }
    }

    SubShader{
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _Mask;
            sampler2D _Source;

            fixed4 frag(v2f i) : SV_Target
            {
                float4 color = tex2D(_Mask, i.uv);
                float backgroundAlpha = 0;
                if (color.w == backgroundAlpha) {
                    color = tex2D(_Source, i.uv);
                }
                return color;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
