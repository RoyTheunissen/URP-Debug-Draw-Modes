using UnityEngine;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;

namespace RoyTheunissen.URPDebugDrawModes
{
    public sealed class ShowGbufferRenderPass : DebugDrawModeRenderPass
    {
        private static readonly int ChannelsPropertyId = Shader.PropertyToID("_Channels");

        private ShowGbufferSettings defaultSettings;

        protected override string ShaderName => "Hidden/DebugDrawMode_Gbuffer";

        protected override string RenderTextureName => "_ShowGbufferRenderTexture";

        public void Initialize(ShowGbufferSettings defaultSettings)
        {
            InitializeInternal();
            
            this.defaultSettings = defaultSettings;
        }

        protected override TextureHandle GetSourceTexture(UniversalResourceData resourceData)
        {
            return resourceData.gBuffer[defaultSettings.GbufferIndex];
        }

        protected override void OnUpdateSettings(Material material)
        {
            // NOTE: You can get the settings from a volume but we don't care about that
            
            material.SetInt(ChannelsPropertyId, (int)defaultSettings.Channels);
        }
    }
}
