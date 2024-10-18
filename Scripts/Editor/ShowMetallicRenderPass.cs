using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;

namespace RoyTheunissen.URPDebugDrawModes
{
    public sealed class ShowMetallicRenderPass : DebugDrawModeRenderPass
    {
        protected override string ShaderName => "Hidden/DebugDrawMode_Metallic";

        protected override string RenderTextureName => "_MetallicRenderTexture";

        protected override TextureHandle GetSourceTexture(UniversalResourceData resourceData)
        {
            return resourceData.gBuffer[1];
        }

        public void Initialize()
        {
            InitializeInternal();
        }
    }
}
