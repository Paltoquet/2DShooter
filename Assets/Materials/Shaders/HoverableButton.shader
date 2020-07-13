Shader "Custom/HoverableButton"
{
    Properties {
        _FirstColor("FirstColor", Color) = (0.975,0.580,0.004,1.000)
        _SecondColor("SecondColor", Color) = (0.957,0.980,0.878,1.000)
        _GlowRadius("GlowRadius", Float) = 0.168
        _FallOff("FallOff", Float) = 2.0
        _Speed("Speed", Float) = 42.0
        _MainTex("Texture", 2D) = "white" { }
        [PerRendererData][MaterialToggle] _Hovered("Hovered", Float) = 0
    }

    SubShader {

        Tags {"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent"}
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass {

            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;

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

            fixed4 _FirstColor;
            fixed4 _SecondColor;
            float _GlowRadius;
            float _FallOff;
            float _Speed;
            float _Hovered;

            #define tableSize 256.0
            #define numLayers 4.0   

            float rect(float2 p, float2 size) {
                float2 d = abs(p) - size;
                return min(max(d.x, d.y), 0.0) + length(max(d, 0.0));
            }

            float easingOut(float val) {
                return 1.0 - pow(1.0 - val, 5.0);
            }

            float smoothstep(float val) {
                return val * val * (3.0 - 2.0 * val);
            }

            float random(float2 val) {
                return frac(sin(dot(val, float2(12.9898, 78.233)))* 43758.513);
            }

            float noise(float2 p) {
                float xi = floor(p.x);
                float yi = floor(p.y);

                float tx = p.x - xi;
                float ty = p.y - yi;

                float rx0 = xi % tableSize;
                float rx1 = (rx0 + 1.0) % tableSize;
                float ry0 = yi % tableSize;
                float ry1 = (ry0 + 1.0) % tableSize;

                float c00 = random(float2(rx0, ry0));
                float c10 = random(float2(rx1, ry0));
                float c01 = random(float2(rx0, ry1));
                float c11 = random(float2(rx1, ry1));

                float sx = smoothstep(tx);
                float sy = smoothstep(ty);

                float nx0 = lerp(c00, c10, sx);
                float nx1 = lerp(c01, c11, sx);

                float result = lerp(nx0, nx1, sy);
                return result;
            }

            float fbm(float2 pos) {
                float dt = _Time.y * _Speed;

                pos.x += dt;
                pos.y += dt;

                float brownianNoise = 0.0;
                float rateOfChange = 2.0;
                float baseFrequency = 0.03;
                float noiseMax = 1.000;
                float relight = 1.5 + sin(_Time.x) / 2.0;
                for (float i = 0.0; i < numLayers; ++i)
                {
                    float frequency = baseFrequency * pow(rateOfChange, i);
                    float amplitude = pow(rateOfChange, i);
                    brownianNoise += noise(pos * frequency) / amplitude;
                    noiseMax += 1.0 / amplitude;
                }

                brownianNoise = brownianNoise / noiseMax;
                brownianNoise *= relight;
                brownianNoise = clamp(0.0, 1.0, brownianNoise);

                return brownianNoise;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float width = 0.8;
                float2 screenPos = i.pos.xy;
                float2 st = i.uv;
                st = st * float2(2.0, 2.0) - float2(1.0, 1.0);

                float noise = fbm(screenPos);
                float border = rect(st, float2(width, width));
                float val = 0.0;
                float4 color = float4(0.0, 0.0, 0.0, 0.0);
                if (border < _GlowRadius && _Hovered) {
                    float range = 1.0 - _GlowRadius;
                    float easing = 1.0 - border;
                    float coef = 1.0 / _GlowRadius;
                    float dist = (easing - range) * coef;
                    val = dist * noise;

                    color = lerp(_FirstColor, _SecondColor, val);
                    color = lerp(float4(color.xyz / _FallOff, 0.0), color, dist);
                }

                if (border <= 0) {
                    float deltaUV = 1 - width;
                    float leftMargin = deltaUV / 2;
                    float coef = 1 / width;
                    float2 uv = (i.uv - float2(leftMargin, leftMargin)) * coef;
                    color = tex2D(_MainTex, uv);
                    color = _Hovered ? color * 1.1 : color;
                }

                return color;
            }

            ENDCG
        }

    }
}