Shader "Custom/VanGoghStarfieldPlane"
{
    Properties
    {
        _MainTex ("Brush Stroke Noise", 2D) = "white" {}
        _StarNoise ("Star Noise", 2D) = "white" {}
        _TimeScale ("Time Scale", Float) = 0.2
        _DistortionStrength ("Brush Stroke Distortion", Float) = 0.1
        _StarIntensity ("Star Intensity", Float) = 2.0
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 100
        Cull Off
        ZWrite On

        Pass
        {
            Name "FORWARD"
            Tags { "LightMode" = "UniversalForward" }
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION; // 顶点位置
                float2 uv : TEXCOORD0;       // UV 坐标
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION; // 裁剪空间位置
                float2 uv : TEXCOORD0;           // 传递 UV 坐标
            };

            sampler2D _MainTex;
            sampler2D _StarNoise;
            float _TimeScale;
            float _DistortionStrength;
            float _StarIntensity;

            Varyings vert (Attributes v)
            {
                Varyings o;
                o.positionHCS = TransformObjectToHClip(v.positionOS); // 转换为裁剪空间
                o.uv = v.uv; // 直接传递 UV 坐标
                return o;
            }

            half4 frag (Varyings i) : SV_Target
            {
                // 时间动态效果
                float time = _Time.y * _TimeScale;

                // 扭曲 UV 坐标
                float2 swirlUV = i.uv + _DistortionStrength * (tex2D(_MainTex, i.uv * 3.0 + time).rg - 0.5);

                // 模拟背景天空渐变
                float skyY = saturate(i.uv.y * 0.5 + 0.5); // 基于 UV 的 Y 值
                float3 skyColor = lerp(float3(0.0, 0.0, 0.02), float3(0.01, 0.01, 0.05), skyY);

                // 添加程序化星星
                float starNoise = tex2D(_StarNoise, swirlUV * 5.0 + time * 0.1).r;
                float stars = smoothstep(0.85, 0.95, starNoise) * _StarIntensity;

                // 最终颜色
                float3 finalColor = skyColor + stars;
                return half4(finalColor, 1.0);
            }
            ENDHLSL
        }
    }
    FallBack Off
}