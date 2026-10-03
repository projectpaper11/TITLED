Shader "Custom/FovMask"
{
    // FOV 콘 메쉬 전용. 색은 하나도 안 쓰고 스텐실 자국만 남기는 용도.
    // 스텐실 값 자체는 Renderer2D의 "Draw View Mask" RenderObjects가 덮어쓰므로
    // 여기에는 Stencil 블록을 적지 않는다.
    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
            "RenderPipeline" = "UniversalPipeline"
        }

        Pass
        {
            // LightMode 태그를 안 달면 SRPDefaultUnlit으로 잡힌다.
            // RenderObjects가 기본으로 찾는 태그 목록에 들어있는 값이다.
            ColorMask 0 // 화면에 색을 안 그린다. 이게 핵심.
            ZWrite Off
            ZTest Always
            Cull Off
            Blend Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
            };

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                return half4(0, 0, 0, 0); // ColorMask 0이라 어차피 버려진다.
            }
            ENDHLSL
        }
    }

    Fallback Off
}
