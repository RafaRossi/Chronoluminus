Shader "Custom/URP/Sprite_Wobble"
{
    Properties
    {
        [MainTexture] _MainTex ("Sprite Texture", 2D) = "white" {}
        [MainColor] _Color ("Tint", Color) = (1,1,1,1)

        [Header(Wobble)]
        _WobbleAmplitude ("Amplitude (UV units)", Range(0, 0.1)) = 0.015
        _WobbleFrequency ("Frequência espacial", Range(0, 50)) = 15.0
        _WobbleSpeed ("Velocidade", Range(0, 20)) = 5.0
        _WobbleAxisMix ("Mistura X/Y (0=só X, 1=X e Y)", Range(0,1)) = 0.5

        [Header(Sprite Defaults)]
        [HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
        [HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
        [PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
        [PerRendererData] _EnableExternalAlpha ("Enable External Alpha", Float) = 0
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
            "RenderPipeline" = "UniversalPipeline"
            "IgnoreProjector" = "True"
            "CanUseSpriteAtlas" = "True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            Name "SpriteWobble"
            
            Tags { "LightMode" = "Universal2D" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float3 positionOS : POSITION;
                float4 color      : COLOR;
                float2 uv         : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float4 color       : COLOR;
                float2 uv          : TEXCOORD0;
                float3 positionOS  : TEXCOORD1;
                
                UNITY_VERTEX_INPUT_INSTANCE_ID
                UNITY_VERTEX_OUTPUT_STEREO
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
                float4 _Color;
                float _WobbleAmplitude;
                float _WobbleFrequency;
                float _WobbleSpeed;
                float _WobbleAxisMix;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                UNITY_SETUP_INSTANCE_ID(IN);
                UNITY_TRANSFER_INSTANCE_ID(IN, OUT);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

                OUT.positionHCS = TransformObjectToHClip(IN.positionOS);
                OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
                OUT.color = IN.color * _Color;
                
                OUT.positionOS = IN.positionOS;

                return OUT;
            }

            float4 frag(Varyings IN) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(IN);
                
                float t = _Time.y * _WobbleSpeed;
                
                float phaseX = IN.positionOS.y * _WobbleFrequency + t;
                float phaseY = IN.positionOS.x * _WobbleFrequency + t * 1.3;

                float offsetX = sin(phaseX) * _WobbleAmplitude;
                float offsetY = sin(phaseY) * _WobbleAmplitude * _WobbleAxisMix;

                float2 finalUV = IN.uv + float2(offsetX, offsetY);

                float4 texColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, finalUV);
                return texColor * IN.color;
            }
            ENDHLSL
        }
    }

    FallBack "Sprites/Default"
}