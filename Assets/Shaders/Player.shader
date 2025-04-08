Shader "Custom/SpriteLitWithOutline"
{
    Properties
    {
        _MainColor ("Main Color", Color) = (1, 1, 1, 1)
        _BrushNoise ("Brush Noise Texture", 2D) = "white" {}
        _CanvasTex ("Canvas Texture", 2D) = "white" {}
        _BrushStrength ("Brush Strength", Range(0, 1)) = 0.5
        _UVScale ("UV Scale", Vector) = (1, 1, 0, 0)
        _OutlineColor ("Outline Color", Color) = (0, 0, 0, 1)
        _OutlineWidth ("Outline Width", Float) = 0.03
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" "RenderPipeline"="UniversalPipeline" }
        LOD 200

        // 描边 Pass
        Pass
        {
            Name "Outline"
            Tags { "LightMode"="SRPDefaultUnlit" }
            Cull Front
            ZWrite Off
            ZTest Less

            CGPROGRAM
            #pragma vertex vertOutline
            #pragma fragment fragOutline
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
            };

            float _OutlineWidth;
            float4 _OutlineColor;

            v2f vertOutline (appdata v)
            {
                v2f o;
                float3 offset = v.normal * _OutlineWidth;
                float4 objectPos = v.vertex + float4(offset, 0);
                o.pos = UnityObjectToClipPos(objectPos);
                return o;
            }

            fixed4 fragOutline (v2f i) : SV_Target
            {
                return _OutlineColor;
            }
            ENDCG
        }

        // 主渲染 Pass
        Pass
        {
            Name "SpriteLitPass"
            Tags { "Queue" = "Transparent" "LightMode"="UniversalForward" }
            ZWrite Off
            Cull Off
            Blend SrcAlpha OneMinusSrcAlpha

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
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _BrushNoise;
            sampler2D _CanvasTex;
            float4 _MainColor;
            float _BrushStrength;
            float4 _UVScale;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 brushNoise = tex2D(_BrushNoise, i.uv);
                float2 canvasUV = i.uv * _UVScale.xy;
                fixed4 canvasTex = tex2D(_CanvasTex, canvasUV);
                float canvasGray = dot(canvasTex.rgb, float3(0.299, 0.587, 0.114));
                fixed4 canvasGrayTex = fixed4(canvasGray, canvasGray, canvasGray, canvasTex.a);
                fixed4 combined = lerp(canvasGrayTex, brushNoise, _BrushStrength);
                fixed4 finalColor = combined * _MainColor;
                return finalColor;
            }
            ENDCG
        }
    }
    FallBack "Sprites/Default"
}