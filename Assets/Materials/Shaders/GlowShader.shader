Shader "Custom/HoverableShader"
{
    Properties
    {
        _GlowColor("Main Color", Color) = (0.301,0.289,0.197,1)
        _MainTex("Texture", 2D) = "white" { }
        _Border("Border Radius", Float) = 0.42
        [PerRendererData][MaterialToggle] _Hovered("Hovered", Float) = 0
    }

    SubShader {

        Tags {"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent"}
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

#define PI 3.142
#define ANIMATION_DURATION 12.0
#define ANIMATION_SPEED 2.000
#define BLUR_RADIUS 12

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

            fixed4 _GlowColor;
            float _Border;
            sampler2D _MainTex;
            float _Hovered;

            float random(float theta) {
                float period = 230.000;
                return frac(sin(dot(theta, period * period))* 2.856);
            }

            float getCurrentTime() {
                float coef = (_Time * ANIMATION_SPEED) % ANIMATION_DURATION;
                coef = coef / ANIMATION_DURATION;
                coef = max(0.0, coef);
                return coef;
            }

            float getAngle(float2 pos) {
                float coef = getCurrentTime();
                float theta = atan2(pos.y, pos.x);
                theta = theta + coef * 2.0 * PI;
                return theta;
            }

            float period(float theta, float nbCroissant) {
                float period = 2.0 * PI;
                period = period * nbCroissant;
                return sin(theta * period);
            }

            float square(float2 position, float2 dim) {
                if (abs(position.x) < dim.x && abs(position.y) < dim.y) {
                    return 1.0;
                }
                return 0.0;
            }

            float rect(float2 p, float2 size) {
                float2 d = abs(p) - size;
                return min(max(d.x, d.y), 0.0) + length(max(d, 0.0));
            }

            float sphere(float2 position, float2 center, float radius) {
                float dist = length(position - center);
                if (length(position - center) <= radius) {
                    return dist;
                }
                return 0.0;
            }

            float circle(float2 position, float radius, float width) {
                float dist = abs(length(position) - radius);
                if (dist < width) {
                    return width;
                }
                return 0.0;
            }

            float easingIn(float val) {
                return pow(val, 3.0);
            }

            float easingOut(float val) {
                return 1.0 - pow(1.0 - val, 5.0);
            }

            float generateSphere(float2 position, float theta, float time) {
                float2 centers[8];
                float radius[8];
                centers[0] = float2(0.150, -0.730);
                centers[1] = float2(0.320, 0.660);
                centers[2] = float2(-0.660, 0.440);
                centers[3] = float2(-0.590, -0.450);
                centers[4] = float2(-0.630, -0.430);
                centers[5] = float2(-0.740, 0.260);
                centers[6] = float2(0.150, -0.730);
                centers[7] = float2(0.730, -0.260);

                radius[0] = 0.028;
                radius[1] = 0.037;
                radius[2] = 0.049;
                radius[3] = 0.032;
                radius[4] = 0.024;
                radius[5] = 0.042;
                radius[6] = 0.062;
                radius[7] = 0.080;

                float result = 0.0;
                float current = (theta + PI); //[0 - 360]
                current = (current + PI / 2.0) % (2.0 * PI);  //offset to put the beguining at the top
                current = current / (2.0 * PI); //ratio for animation

                float a = time * 2.0 * PI;
                float s = sin(a);
                float c = cos(a);
                float2x2 m = float2x2(c, -s, s, c);

                time = 1.0;
                for (int i = 0; i < 8; i++) {
                    float2 center = centers[i];
                    center = mul(m, center);
                    float r = radius[i];
                    //center = mix(float2(0), center, time);
                    //radius = mix(0.001, radius, time);
                    result = max(result, sphere(position, center, r));
                }
                return result;
            }

            float generateCroissant(float2 position, float nbCroissant, float theta, float time) {
                float current = (theta + PI); //[0 - 360]
                current = (current + PI / 2.0) % (2.0 * PI);  //offset to put the beguining at the top
                current = current / (2.0 * PI); //ratio for animation

                float currentCroissant = floor(current * nbCroissant);
                float amplitude = random(currentCroissant) * 1.0;

                float attenuation = current % 0.5;
                attenuation = abs(0.25 - attenuation);
                attenuation = 1.0 - attenuation * 4.0;

                float range = period(current, nbCroissant);
                float dist = length(position);
                range = range * min(amplitude + attenuation / 9.0, 0.9);
                dist = dist < range ? range : 0.0;
                return dist;
            }

            float4 getColor(float2 pos, float2 uv, float theta, float time) {
                float nbCroissant = 140.0;

                float current = (theta + PI); //[0 - 360]
                current = (current + PI / 2.0) % (2.0 * PI);  //offset to put the beguining at the top
                current = current / (2.0 * PI); //ratio for animation

                float4 firstRedColor = float4(0.995, 0.454, 0.192, 1.000);
                float4 firstYellowColor = float4(0.179, 0.179, 0.995, 1.000);
                float4 firstColor = lerp(firstYellowColor, firstRedColor, easingIn(1.0 - abs((current - 0.5)) * 1.0));
                float4 secondColor = float4(0.995, 0.924, 0.362, 1.000);
                float4 backGroundColor = float4(0.001, 0.000, 0.005, 1.000);

                float circleRadius = lerp(0.0, 0.065, time);
                float circleWidth = lerp(0.01, 0.065, time);

                float dist = generateCroissant(pos, nbCroissant, theta, time);
                dist = max(dist, generateCroissant(pos, nbCroissant / 5.0, theta, time)); // play with thoose
                dist = max(dist, generateCroissant(pos, nbCroissant * 1.8, theta, time)); // play with thoose
                dist = max(dist, generateSphere(pos, theta, time));
                dist = max(dist, easingIn(circle(pos, circleRadius, circleWidth)));
                float4 color = dist != 0.0 ? lerp(firstColor, secondColor, easingIn(dist)) : backGroundColor;

                float width = _Border;
                float glowRadius = 0.120;
                float rectBorder = max(rect(pos, float2(width, width)), 0.0);

                color.a = dist > 0.2 ? 1.0 : 0.0;

                if (rectBorder >= 0.000 && rectBorder < glowRadius) {
                    float range = 1.0 - glowRadius;
                    float easing = 1.0 - rectBorder;
                    float coef = 1.0 / glowRadius;
                    rectBorder = (easing - range) * coef;
                    color = lerp(color, _GlowColor, easingIn(rectBorder));
                    color.a = rectBorder;
                }

                return color;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                //float4 color = tex2D(_MainTex, i.uv);
                float2 uv = i.uv;
                float2 deltaUV = (1.0 - _Border) / 2.0;
                uv = max(uv - deltaUV, 0.0);
                uv = uv / _Border;
                //uv = min(max(uv, 0.0), 1.0);

                float2 st = i.uv;
                st = st * float2(2.0, 2.0) - float2(1.0, 1.0);
                float2 pixelOffset = float2(1.0, 1.0) / _ScreenParams.xy;

                float time = getCurrentTime();
                float4 color = float4(0.0, 0.0, 0.0, 0.0);

                if (_Hovered == 1.0) {
                    for (int i = -1 * BLUR_RADIUS; i < BLUR_RADIUS; i++) {
                        for (int j = -1 * BLUR_RADIUS; j < BLUR_RADIUS; j++) {
                            float2 offset = float2(i, j) * pixelOffset;
                            float2 pos = st + offset;
                            float angle = getAngle(pos);
                            color += getColor(st + offset, uv, angle, time);
                        }
                    }

                    color /= BLUR_RADIUS * BLUR_RADIUS;
                    float c = max(max(color.x, color.y), color.z);
                    c = c < 0.03 ? 0 : c;
                    color.a = c;
                }

                float glowInnerRadius = 0.080;
                float rectBorder = max(rect(st, float2(_Border, _Border)), 0.0);
                if (rectBorder == 0.0) {
                    //dist = 0 at center
                    float dist = _Border - abs(rect(st, float2(_Border, _Border)));
                    float4 imageColor = tex2D(_MainTex, uv);
                    if (_Hovered == 1.0) {
                        imageColor = imageColor * 1.1;
                        dist = min((_Border - dist) / glowInnerRadius, 1.0);
                        color = lerp(color, imageColor, easingOut(dist));
                    }
                    else {
                        color = imageColor;
                    }
                }

                //return float4(uv, 0.0, 1.0);
                //return tex2D(_MainTex, uv);
                return color;
            }
            ENDCG
        }

        /*GrabPass { "_GrabTexture" }

        Pass {

            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            fixed4 _Color;
            sampler2D _GrabTexture;
            float4 _MainTex_ST;
            float4 _MainTex_TexelSize;
            float4 _GrabTexture_TexelSize;
            float _Strength;
            float _Hovered;

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

            fixed4 frag(v2f input) : SV_Target
            {

                float2 uv = float2(input.pos.x, input.pos.y);
                uv = uv * float2(_GrabTexture_TexelSize.x, 0.001);

                float horizontalOffset = _MainTex_TexelSize.x;
                float verticalOffset = _MainTex_TexelSize.y;

                float2 pixelOffset = float2(horizontalOffset, verticalOffset);

                int kernelSize = 2 * int(2 * _Strength) + 3;
                int halfSize = 6; // kernelSize / 2;
                float kernel[KERNEL_PHYSICAL_SIZE];
                makeGaussianKernel(_Strength, kernelSize, kernel);

                float2 currentUV = input.uv;
                float4 color = float4(0.0, 0.0, 0.0, 0.0);

                //#if UNITY_UV_STARTS_AT_TOP
                    //currentUV.y = 1 - currentUV.y;
                //#endif

                //return float4(0.0, 0.6, 0.8, 1.0);
                int cmp = 0;
                for (float i = -halfSize; i <= halfSize; i++) {
                    for (float j = -halfSize; j < halfSize; j++) {
                        float2 offset = float2(i, j) * pixelOffset;
                        float2 current = uv + offset;
                        color += tex2D(_GrabTexture, current);
                        cmp++;
                    }
                }

                color = color / cmp;

                //color /= 25;

                return color;
                return float4(uv.x, uv.y, 0.0, 1.0);
                //return tex2D(_GrabTexture, input.uv);

                float4 texColor = float4(0, 0, 0, 1);
                for (float index = -halfSize; index < halfSize + 1; index++) {
                    float2 current = currentUV;
                    float coef = kernel[index + halfSize];
                    current.x += index * horizontalOffset;
                    texColor.xyz += tex2D(_MainTex, current) * coef;
                }

                return texColor;
            }

            ENDCG
        }*/

        /*GrabPass{ }

            // Vertical Pass
        Pass{

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
        }*/
    }
    FallBack "Diffuse"
}
