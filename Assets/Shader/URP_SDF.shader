Shader "Custom/UI/ShapeSDF"
{
    Properties
    {
        [Header(Forma e Estilo)]
        [Enum(Circulo, 0, Retangulo, 1, RetanguloArredondado, 2, Triangulo, 3, Hexagono, 4, Anel, 5)] 
        _ShapeType ("Tipo de Forma", Float) = 0
        
        _Color ("Cor Principal", Color) = (1, 1, 1, 1)
        
        [Header(Dimensoes)]
        _Size ("Tamanho (Largura, Altura)", Vector) = (0.5, 0.5, 0, 0)
        _CornerRadius ("Raio do Canto (Retangulo Arredondado)", Range(0, 1)) = 0.1
        _RingThickness ("Espessura (Anel)", Range(0, 1)) = 0.05
        _EdgeSoftness ("Suavizacao da Borda", Range(0.001, 0.2)) = 0.01

        [Header(Configuracoes Nativa de UI)]
        [HideInInspector] _StencilComp ("Stencil Comparison", Float) = 8
        [HideInInspector] _Stencil ("Stencil ID", Float) = 0
        [HideInInspector] _StencilOp ("Stencil Operation", Float) = 0
        [HideInInspector] _StencilWriteMask ("Stencil Write Mask", Float) = 255
        [HideInInspector] _StencilReadMask ("Stencil Read Mask", Float) = 255
        [HideInInspector] _ColorMask ("Color Mask", Float) = 15
    }

    SubShader
    {
        Tags 
        { 
            "RenderType" = "Transparent" 
            "Queue" = "Transparent" 
            "RenderPipeline" = "UniversalPipeline" 
            "IgnoreProjector" = "True"
            "PreviewType" = "Plane"
        }

        // Configuração de Stencil necessária para funcionar dentro de UI Masks (ScrollRects, etc)
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
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS   : POSITION;
                float2 uv           : TEXCOORD0;
                float4 color        : COLOR; // Cor vinda do componente Image no Canvas
            };

            struct Varyings
            {
                float4 positionHCS  : SV_POSITION;
                float2 uv           : TEXCOORD0;
                float4 color        : COLOR;
            };

            // O uso do CBUFFER garante compatibilidade com o SRP Batcher, economizando draw calls
            CBUFFER_START(UnityPerMaterial)
                float _ShapeType;
                float4 _Color;
                float4 _Size;
                float _CornerRadius;
                float _RingThickness;
                float _EdgeSoftness;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                OUT.color = IN.color;
                return OUT;
            }

            // --- Funções SDF ---
            float sdCircle(float2 p, float r) {
                return length(p) - r;
            }

            float sdBox(float2 p, float2 b) {
                float2 d = abs(p) - b;
                return length(max(d, 0.0)) + min(max(d.x, d.y), 0.0);
            }

            float sdRoundedBox(float2 p, float2 b, float r) {
                return sdBox(p, b) - r;
            }

            float sdEquilateralTriangle(float2 p, float r) {
                const float k = sqrt(3.0);
                p.x = abs(p.x) - r;
                p.y = p.y + r/k;
                if(p.x + k*p.y > 0.0) p = float2(p.x - k*p.y, -k*p.x - p.y) / 2.0;
                p.x -= clamp(p.x, -2.0*r, 0.0);
                return -length(p) * sign(p.y);
            }

            float sdHexagon(float2 p, float r) {
                const float3 k = float3(-0.866025404, 0.5, 0.577350269);
                p = abs(p);
                p -= 2.0 * min(dot(k.xy, p), 0.0) * k.xy;
                p -= float2(clamp(p.x, -k.z*r, k.z*r), r);
                return length(p) * sign(p.y);
            }

            float sdRing(float2 p, float r, float th) {
                return abs(length(p) - r) - th;
            }

            // --- Fragment Shader ---
            float4 frag(Varyings IN) : SV_Target
            {
                // 1. Converte o UV (0..1) diretamente para pixels reais da tela (corrige a deformacao)
                float2 pixelScale = 1.0 / fwidth(IN.uv);
                float2 p = (IN.uv - 0.5) * pixelScale;
                
                // 2. Os tamanhos agora sao definidos em pixels reais (ex: 200px de largura, 10px de canto)
                float2 boxSize = _Size.xy * pixelScale * 0.5;
                float cornerRadius = _CornerRadius * min(pixelScale.x, pixelScale.y) * 0.5;

                float d = 0;
                int shape = (int)_ShapeType;

                if (shape == 0)      d = sdCircle(p, _Size.x * pixelScale.x * 0.5);
                else if (shape == 1) d = sdBox(p, boxSize);
                else if (shape == 2) d = sdRoundedBox(p, boxSize, cornerRadius);
                else if (shape == 3) d = sdEquilateralTriangle(p, _Size.x * pixelScale.x * 0.5);
                else if (shape == 4) d = sdHexagon(p, _Size.x * pixelScale.x * 0.5);
                else if (shape == 5) d = sdRing(p, _Size.x * pixelScale.x * 0.5, _RingThickness * pixelScale.x * 0.5);

                // 3. Antialiasing ultra-nítido travado em 1 pixel de tela (sub-pixel crisp)
                float alphaMask = 1.0 - smoothstep(0.0, 1.0, d);

                // 4. Cor final combinada com a cor do componente Image do Canvas
                float4 finalColor = _Color * IN.color;
                
                // 5. Multiplica o RGB pelo Alpha no cálculo da borda para eliminar a auréola branca/cinza
                finalColor.rgb *= finalColor.a;
                finalColor.a *= alphaMask;

                clip(finalColor.a - 0.001);

                return finalColor;
            }
            ENDHLSL
        }
    }
}