using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;

namespace RoyTheunissen.URPDebugDrawModes
{
    public sealed class ShowEncodedNormalsRenderPass : DebugDrawModeRenderPass
    {
        protected override string ShaderName => "Hidden/DebugDrawMode_EncodedNormals";

        protected override string RenderTextureName => "_EncodedNormalsRenderTexture";

        protected override TextureHandle GetSourceTexture(UniversalResourceData resourceData)
        {
            return resourceData.gBuffer[2];
        }

        public void Initialize()
        {
            InitializeInternal();
        }
    }
}
