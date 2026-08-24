Shader "Custom/URP_Billboard_Sprite"
{
    Properties
    {
        [MainTexture] _BaseMap("Sprite Texture", 2D) = "white" {}
        [MainColor] _BaseColor("Tint", Color) = (1,1,1,1)
        [Toggle(_CYLINDER_BILLBOARD)] _CylinderBillboard("Lock Y Axis (Cylindrical)", Float) = 0
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
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma shader_feature_local _CYLINDER_BILLBOARD

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv         : TEXCOORD0;
                float4 color      : COLOR;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv         : TEXCOORD0;
                float4 color      : COLOR;
            };

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseMap_ST;
                float4 _BaseColor;
            CBUFFER_END

            Varyings vert(Attributes input)
            {
                Varyings output;

                float3 worldOrigin = TransformObjectToWorld(float3(0, 0, 0));

                float scaleX = length(float3(UNITY_MATRIX_M[0][0], UNITY_MATRIX_M[0][1], UNITY_MATRIX_M[0][2]));
                float scaleY = length(float3(UNITY_MATRIX_M[1][0], UNITY_MATRIX_M[1][1], UNITY_MATRIX_M[1][2]));

                #if defined(_CYLINDER_BILLBOARD)
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
                    float3 viewCenter = TransformWorldToView(worldOrigin);
                    float3 viewPos = viewCenter + float3(input.positionOS.x * scaleX, input.positionOS.y * scaleY, 0.0);

                    output.positionCS = mul(UNITY_MATRIX_P, float4(viewPos, 1.0));
                #endif

                output.uv = TRANSFORM_TEX(input.uv, _BaseMap);
                output.color = input.color * _BaseColor;

                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                half4 texColor = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, input.uv);
                return texColor * input.color;
            }
            ENDHLSL
        }
    }
}