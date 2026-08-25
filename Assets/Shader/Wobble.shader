Shader "Custom/UI/UI_Wobble"
{
    Properties
    {
        // Propriedade obrigatória do Canvas
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        [Header(Wobble Settings)]
        _WobbleAmplitude ("Amplitude (UV units)", Range(0, 0.1)) = 0.015
        _WobbleFrequency ("Frequência espacial", Range(0, 50)) = 15.0
        _WobbleSpeed ("Velocidade", Range(0, 20)) = 5.0
        _WobbleAxisMix ("Mistura X/Y (0=só X, 1=X e Y)", Range(0,1)) = 0.5

        // Propriedades Nativas Obrigatórias de UI (Máscaras e Stencil)
        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255
        _ColorMask ("Color Mask", Float) = 15
        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        // Bloco obrigatório para o Canvas Mask funcionar
        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode] // <-- É ISSO QUE FAZ APARECER NA ABA SCENE!
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]

        Pass
        {
            Name "Default"
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS   : POSITION;
                float4 color        : COLOR;
                float2 uv           : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS  : SV_POSITION;
                float4 color        : COLOR;
                float2 uv           : TEXCOORD0;
                float4 positionOS   : TEXCOORD1;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            CBUFFER_START(UnityPerMaterial)
                float4 _Color;
                float _WobbleAmplitude;
                float _WobbleFrequency;
                float _WobbleSpeed;
                float _WobbleAxisMix;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                
                // O Canvas já gerencia a cor baseada no componente Image, multiplicamos pelo Tint
                OUT.color = IN.color * _Color;
                
                // Salvamos a posição local para a onda não quebrar caso a UI se mova
                OUT.positionOS = IN.positionOS;
                
                return OUT;
            }

            float4 frag(Varyings IN) : SV_Target
            {
                float t = _Time.y * _WobbleSpeed;
                
                // Mesma lógica de onda que usamos antes
                float phaseX = IN.positionOS.y * _WobbleFrequency + t;
                float phaseY = IN.positionOS.x * _WobbleFrequency + t * 1.3;

                float offsetX = sin(phaseX) * _WobbleAmplitude;
                float offsetY = sin(phaseY) * _WobbleAmplitude * _WobbleAxisMix;

                float2 finalUV = IN.uv + float2(offsetX, offsetY);

                float4 texColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, finalUV);
                float4 colorFinal = texColor * IN.color;

                #ifdef UNITY_UI_ALPHACLIP
                clip (colorFinal.a - 0.001);
                #endif

                return colorFinal;
            }
            ENDHLSL
        }
    }
}