using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.RenderGraphModule.Util;
using UnityEngine.Rendering.Universal;

namespace RoyTheunissen.URPBufferDebugging
{
    public abstract class DebugDrawModeRenderPass : ScriptableRenderPass
    {
        private Material material;
        private RenderTextureDescriptor rtDescriptor;

        protected abstract string ShaderName { get; }
        
        protected abstract string RenderTextureName { get; }

        protected DebugDrawModeRenderPass()
        {
            rtDescriptor = new RenderTextureDescriptor(Screen.width, Screen.height, RenderTextureFormat.Default, 0);
        }

        protected void InitializeInternal()
        {
            Shader shader = Shader.Find(ShaderName);
            material = new Material(shader);
            
            renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing;
        }

        public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
        {
            UniversalResourceData resourceData = frameData.Get<UniversalResourceData>();
            
            UniversalCameraData cameraData = frameData.Get<UniversalCameraData>();
            
            // The following line ensures that the render pass doesn't blit
            // from the back buffer.
            if (resourceData.isActiveTargetBackBuffer)
                return;
            
            // Set the RT size to be the same as the camera target size.
            rtDescriptor.width = cameraData.cameraTargetDescriptor.width;
            rtDescriptor.height = cameraData.cameraTargetDescriptor.height;
            rtDescriptor.depthBufferBits = 0;
            
            TextureHandle srcCamColor = resourceData.activeColorTexture;
            TextureHandle dst = UniversalRenderer.CreateRenderGraphTexture(
                renderGraph, rtDescriptor, RenderTextureName, false);

            // Update the settings in the material
            UpdateSettings();

            // This check is to avoid an error from the material preview in the scene
            if (!srcCamColor.IsValid() || !dst.IsValid())
                return;

            TextureHandle sourceTexture = GetSourceTexture(resourceData);
            
            // The AddBlitPass method adds a render graph pass that blits from the source texture
            // (camera color in this case) to the destination texture using the specified shader pass
            RenderGraphUtils.BlitMaterialParameters parameters =
                new RenderGraphUtils.BlitMaterialParameters(sourceTexture, srcCamColor, material, 0);
            renderGraph.AddBlitPass(parameters, "RenderFeatureBlitPass");
        }

        protected abstract TextureHandle GetSourceTexture(UniversalResourceData resourceData);

        private void UpdateSettings()
        {
            if (material == null)
                return;

            OnUpdateSettings(material);
        }

        protected virtual void OnUpdateSettings(Material material)
        {
        }

        public void Cleanup()
        {
            if (material != null)
            {
                if (Application.isPlaying)
                    Object.Destroy(material);
                else
                    Object.DestroyImmediate(material);
                material = null;
            }
        }
    }
}
