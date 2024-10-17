Shader "Hidden/DebugDrawMode_Gbuffer"
{
    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline" = "UniversalPipeline"}
        LOD 100
        ZWrite Off Cull Off
        Pass
        {
            Name "RenderFeatureBlitPass"

            HLSLPROGRAM
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            // The Blit.hlsl file provides the vertex shader (Vert),
            // input structure (Attributes) and output structure (Varyings)
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

            #pragma vertex Vert
            #pragma fragment frag
            
            int _Channels;

            half4 frag (Varyings input) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
                
                float4 gbuffer = SAMPLE_TEXTURE2D(_BlitTexture, sampler_LinearClamp, input.texcoord);
                
                // Gbuffer cheat sheet:
                
                // GBUFFER0:    diffuse           diffuse         diffuse         materialFlags   (sRGB rendertarget)
                // GBUFFER1:    metallic/specular specular        specular        occlusion
                // GBUFFER2:    encoded-normal    encoded-normal  encoded-normal  smoothness
                // GBUFFER3:    GI                GI              GI              unused          (lighting buffer)
                // GBUFFER4:    exact layout unclear, but it's used for occlusion according to some of the docs
                
                if (_Channels == 0)
                    return gbuffer.r;
                if (_Channels == 1)
                    return gbuffer.g;
                if (_Channels == 2)
                    return gbuffer.b;
                if (_Channels == 3)
                    return gbuffer.a;
                return gbuffer;
            }
            ENDHLSL
        }
    }
}
