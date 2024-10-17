Shader "Hidden/DebugDrawMode_EncodedNormals"
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
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/UnityGBuffer.hlsl"
            
            #pragma multi_compile_fragment _ _GBUFFER_NORMALS_OCT
            
            #define USE_FULL_PRECISION_BLIT_TEXTURE 1
            
            #define _NORMALMAP

            #pragma vertex Vert
            #pragma fragment frag
            
            int _Channels;
            
            TEXTURE2D_X(_CameraDepthTexture);

            half4 frag (Varyings input) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
                
                // Make the far clip plane black because the gbuffer is garbage there.
                float d        = SAMPLE_TEXTURE2D_X_LOD(_CameraDepthTexture, sampler_PointClamp, input.texcoord, 0).x;
                #if UNITY_REVERSED_Z
                    if (d <= .0001)
                        return 0;
                #else
                    if (d >= .9999)
                        return 0;
                #endif 
                
                float4 gbuffer2 = SAMPLE_TEXTURE2D(_BlitTexture, sampler_LinearClamp, input.texcoord);
                
                float3 worldNormal = UnpackNormal(gbuffer2.xyz).xyz;
                worldNormal = normalize(worldNormal);
                worldNormal = RangeRemap(-1, 1, worldNormal);
                
                return float4(worldNormal, 1);
            }
            ENDHLSL
        }
    }
}
