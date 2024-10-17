Shader "Hidden/DebugDrawMode_Metallic"
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
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/BRDF.hlsl"
            // The Blit.hlsl file provides the vertex shader (Vert),
            // input structure (Attributes) and output structure (Varyings)
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

            #pragma vertex Vert
            #pragma fragment frag
            
            int _Channels;

            half4 frag (Varyings input) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
                
                float4 specular = SAMPLE_TEXTURE2D(_BlitTexture, sampler_LinearClamp, input.texcoord);
                
                // TODO: Support the specular setup too? I would have to access Gbuffer0.a to get the material flags
                // and then use that to determine whether it's using the specular workflow or not. See: UnityGBuffer.hlsl
                
                // UPDATE: I looked into it. There seems to be no easy way to do a blit and pass other buffers along.
                // It might be possible to do it with multiple passes or by not using Unity's blit pass at all and using
                // a custom pass. Both of those options look really time-consuming to me.
                // Not worth it to me, I never use the specular workflow to begin with...
                
                // Metallic setup
                float reflectivity = specular.r;
                half metallic = MetallicFromReflectivity(reflectivity);
                
                return metallic;
            }
            ENDHLSL
        }
    }
}
