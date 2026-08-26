Shader "Custom/URP/Billboard_Wobble"
{
    Properties
    {
        [Header(Textura e Cor)]
        [MainTexture] _BaseMap("Sprite Texture", 2D) = "white" {}
        [MainColor] _BaseColor("Tint", Color) = (1,1,1,1)
        
        [Header(Configuracao do Billboard)]
        [Toggle(_CYLINDER_BILLBOARD)] _CylinderBillboard("Travar Eixo Y (Cilindrico)", Float) = 0

        [Header(Configuracao do Wobble)]
        _WobbleAmplitude ("Amplitude (UV units)", Range(0, 0.1)) = 0.015
        _WobbleFrequency ("Frequência espacial", Range(0, 50)) = 15.0
        _WobbleSpeed ("Velocidade", Range(0, 20)) = 5.0
        _WobbleAxisMix ("Mistura X/Y (0=só X, 1=X e Y)", Range(0,1)) = 0.5
    }

    SubShader
    {
        Tags 
        { 
            "RenderType"="Transparent" 
            "Queue"="Transparent" 
            "RenderPipeline"="UniversalPipeline"
            "IgnoreProjector"="True"
        }

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            Name "Unlit"
            // CORREÇÃO CRÍTICA: Em projetos 3D com URP Forward, usa-se SRPDefaultUnlit
            Tags { "LightMode" = "SRPDefaultUnlit" }
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma shader_feature_local _CYLINDER_BILLBOARD
            #pragma multi_compile_instancing

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv         : TEXCOORD0;
                float4 color      : COLOR;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv         : TEXCOORD0;
                float4 color      : COLOR;
                float3 positionOS : TEXCOORD1;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseMap_ST;
                float4 _BaseColor;
                float _WobbleAmplitude;
                float _WobbleFrequency;
                float _WobbleSpeed;
                float _WobbleAxisMix;
            CBUFFER_END

            Varyings vert(Attributes input)
            {
                Varyings output;
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_TRANSFER_INSTANCE_ID(input, output);

                // Posição do centro do objeto no mundo
                float3 worldOrigin = TransformObjectToWorld(float3(0, 0, 0));

                // Escala do objeto via matriz URP (compatível com SRP Batcher e Instancing)
                float4x4 objectToWorld = GetObjectToWorldMatrix();
                float scaleX = length(float3(objectToWorld[0][0], objectToWorld[0][1], objectToWorld[0][2]));
                float scaleY = length(float3(objectToWorld[1][0], objectToWorld[1][1], objectToWorld[1][2]));

                #if defined(_CYLINDER_BILLBOARD)
                    // Trava o eixo Y (Cilíndrico)
                    float3 forward = _WorldSpaceCameraPos - worldOrigin;
                    forward.y = 0; 
                    forward = normalize(forward);

                    float3 up = float3(0, 1, 0);
                    float3 right = normalize(cross(up, forward));
                    up = cross(forward, right);

                    float3 worldPos = worldOrigin 
                        + right * (input.positionOS.x * scaleX) 
                        + up * (input.positionOS.y * scaleY);

                    output.positionCS = TransformWorldToHClip(worldPos);
                #else
                    // Esférico (Olha para a câmera no View Space)
                    float3 viewCenter = TransformWorldToView(worldOrigin);
                    float3 viewPos = viewCenter + float3(input.positionOS.x * scaleX, input.positionOS.y * scaleY, 0.0);
                    
                    // Converte de View Space para Clip Space via função nativa do URP
                    output.positionCS = mul(GetViewToHClipMatrix(), float4(viewPos, 1.0));
                #endif

                output.uv = TRANSFORM_TEX(input.uv, _BaseMap);
                output.positionOS = input.positionOS.xyz;
                output.color = input.color * _BaseColor;

                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(input);

                float t = _Time.y * _WobbleSpeed;
                
                float phaseX = input.positionOS.y * _WobbleFrequency + t;
                float phaseY = input.positionOS.x * _WobbleFrequency + t * 1.3;

                float offsetX = sin(phaseX) * _WobbleAmplitude;
                float offsetY = sin(phaseY) * _WobbleAmplitude * _WobbleAxisMix;

                float2 finalUV = input.uv + float2(offsetX, offsetY);

                half4 texColor = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, finalUV);
                return texColor * input.color;
            }
            ENDHLSL
        }
    }
}