using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;

namespace RoyTheunissen.URPDebugDrawModes
{
    public sealed class ShowDepthRenderPass : DebugDrawModeRenderPass
    {
        protected override string ShaderName => "Hidden/DebugDrawMode_Depth";

        protected override string RenderTextureName => "_DepthRenderTexture";

        protected override TextureHandle GetSourceTexture(UniversalResourceData resourceData)
        {
            return resourceData.cameraDepth;
        }

        public void Initialize()
        {
            InitializeInternal();
        }
    }
}
