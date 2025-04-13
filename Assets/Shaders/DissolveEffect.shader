Shader "Custom/FullscreenDissolveWithCameraView"
{
    Properties
    {
        _Dissolve("Dissolve Amount", Range(0, 1)) = 0
        _UVScale("UV Scale", Float) = 50.0 // UV 缩放比例
        _color("Color", Color) = (1, 1, 1, 1)
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" }
        Pass
        {
            ZWrite Off
            ZTest Always
            Blend SrcAlpha OneMinusSrcAlpha

            HLSLPROGRAM
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
                float2 uv : TEXCOORD0; // 主相机视图的 UV
                float2 noiseUV : TEXCOORD1; // 噪声图的 UV
                float4 vertex : SV_POSITION;
            };

            float _Dissolve;
            float _UVScale; // UV 缩放比例
            float4 _color; // 颜色属性

            // 伪随机数生成函数
            float Hash(float2 p)
            {
                return frac(sin(dot(p, float2(127.1, 311.7))) * 43758.5453);
            }

            // 平滑噪声函数
            float Noise(float2 p)
            {
                float2 i = floor(p);
                float2 f = frac(p);

                // 双线性插值
                float a = Hash(i);
                float b = Hash(i + float2(1.0, 0.0));
                float c = Hash(i + float2(0.0, 1.0));
                float d = Hash(i + float2(1.0, 1.0));

                float2 u = f * f * (3.0 - 2.0 * f);

                return lerp(lerp(a, b, u.x), lerp(c, d, u.x), u.y);
            }

            // 分形布朗运动 (FBM) 实现
            float FBM(float2 p)
            {
                float value = 0.0;
                float amplitude = 0.5;
                float frequency = 1.0;

                for (int i = 0; i < 5; i++) // 5 层噪声叠加
                {
                    value += amplitude * Noise(p * frequency);
                    frequency *= 2.0;
                    amplitude *= 0.5;
                }

                return value;
            }

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv; // 主相机视图的 UV 坐标
                o.noiseUV = v.uv * _UVScale; // 噪声图的 UV 坐标，受 _UVScale 控制
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // 生成 FBM 噪声
                float noise = FBM(i.noiseUV);

                // 溶解阈值
                float dissolveThreshold = _Dissolve;

                // 如果噪声值小于溶解阈值，则显示主相机视图
                if (noise < dissolveThreshold)
                {
                    return float4(0, 0, 0, 0); // 保持 alpha 为 1
                }

                // 否则显示颜色
                return _color;
            }
            ENDHLSL
        }
    }
    FallBack Off
}