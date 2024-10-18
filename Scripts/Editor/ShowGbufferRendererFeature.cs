using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace RoyTheunissen.URPDebugDrawModes
{
    public sealed class ShowGbufferRendererFeature : ScriptableRendererFeature
    {
        [SerializeField] private ShowGbufferSettings settings;
        [SerializeField] private Shader shader;
        
        private Material material;
        private ShowGbufferRenderPass renderPass;
        
        public override void Create()
        {
            if (shader == null)
                return;
            
            material = new Material(shader);
            
            renderPass = new ShowGbufferRenderPass();
            renderPass.Initialize(settings);
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            if (renderingData.cameraData.cameraType == CameraType.SceneView)
                renderer.EnqueuePass(renderPass);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            
            if (Application.isPlaying)
                Destroy(material);
            else
                DestroyImmediate(material);
        }
    }

    [Serializable]
    public sealed class ShowGbufferSettings
    {
        public enum ChannelOptions
        {
            R,
            G,
            B,
            A,
            All,
        }
        
        [SerializeField, Range(0, 8)] private int gbufferIndex;
        public int GbufferIndex => gbufferIndex;

        [SerializeField] private ChannelOptions channels = ChannelOptions.All;
        public ChannelOptions Channels => channels;
    }
}
