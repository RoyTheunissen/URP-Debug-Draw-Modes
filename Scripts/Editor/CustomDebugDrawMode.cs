using System;
using RoyTheunissen.URPBufferDebugging.Utilities;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace RoyTheunissen.URPBufferDebugging
{
    [Serializable]
    public sealed class CustomDebugDrawMode
    {
        public enum Types
        {
            ReplacementShader,
            RendererFeature,
            Gbuffer,
            EncodedNormals,
            Depth,
            Metallic,
        }
        
        public enum Categories
        {
            Gbuffers = 1 << 0,
            Deferred = 1 << 1,
        }

        [SerializeField] private string name;
        public string Name => name;

        [SerializeField] private Categories category = Categories.Gbuffers;
        public Categories Category => category;

        public string Section => category.ToString();
        
        [SerializeField] private Types type;
        
        [ShowIf(nameof(type), Types.ReplacementShader)]
        [SerializeField] private Shader shader;
        
        [ShowIf(nameof(type), Types.RendererFeature)]
        [SerializeField]
        private ScriptableRendererFeature rendererFeature;
        
        [ShowIf(nameof(type), Types.Gbuffer)]
        [SerializeField]
        private ShowGbufferSettings showGbufferSettings;

        [NonSerialized] private DebugDrawModeRenderPass renderPass;

        public bool Equals(SceneView.CameraMode mode)
        {
            return string.Equals(name, mode.name) && string.Equals(Section, mode.section);
        }

        public void Disable(SceneView sceneView)
        {
            switch (type)
            {
                case Types.ReplacementShader:
                    sceneView.SetSceneViewShaderReplace(null, "");
                    break;
                
                case Types.RendererFeature:
                    if (rendererFeature != null)
                        rendererFeature.SetActive(false);
                    break;
                
                case Types.Gbuffer:
                case Types.EncodedNormals:
                case Types.Depth:
                case Types.Metallic:
                    EndRenderPass();
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Enable(SceneView sceneView)
        {
            switch (type)
            {
                case Types.ReplacementShader:
                    sceneView.SetSceneViewShaderReplace(shader, "");
                    break;
                
                case Types.RendererFeature:
                    if (rendererFeature != null)
                        rendererFeature.SetActive(true);
                    break;
                
                case Types.Gbuffer:
                    BeginRenderPass<ShowGbufferRenderPass>().Initialize(showGbufferSettings);
                    break;

                case Types.EncodedNormals:
                    BeginRenderPass<ShowEncodedNormalsRenderPass>().Initialize();
                    break;
                
                case Types.Depth:
                    BeginRenderPass<ShowDepthRenderPass>().Initialize();
                    break;
                
                case Types.Metallic:
                    BeginRenderPass<ShowMetallicRenderPass>().Initialize();
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private RenderPassType BeginRenderPass<RenderPassType>()
            where RenderPassType : DebugDrawModeRenderPass, new()
        {
            renderPass = new RenderPassType();

            RenderPipelineManager.beginCameraRendering -= OnBeginCamera;
            RenderPipelineManager.beginCameraRendering += OnBeginCamera;

            return renderPass as RenderPassType;
        }

        private void EndRenderPass()
        {
            RenderPipelineManager.beginCameraRendering -= OnBeginCamera;
            renderPass?.Cleanup();
            renderPass = null;
        }

        private void OnBeginCamera(ScriptableRenderContext scriptableRenderContext, Camera camera)
        {
            if (camera.cameraType == CameraType.SceneView)
                camera.GetUniversalAdditionalCameraData().scriptableRenderer.EnqueuePass(renderPass);
        }
    }
}
