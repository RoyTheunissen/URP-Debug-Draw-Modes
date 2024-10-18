using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace RoyTheunissen.URPDebugDrawModes
{
    [InitializeOnLoad]
    public static class CustomDebugDrawModes
    {
        private static SceneView currentSceneView;
        private static CustomDebugDrawMode lastActiveDebugDrawMode;

        static CustomDebugDrawModes()
        {
            EditorApplication.update += OnUpdateEditor;
            
            if (CustomDebugDrawModesConfig.Instance == null)
            {
                Debug.LogError($"Custom Debug Draw Modes Config could not be found. Debug draw modes will not " +
                               $"work. If this is your first time installing it, it may not have loaded correctly " +
                               $"in which case it should work after a restart. If it continues to persist, check " +
                               $"that a {nameof(CustomDebugDrawModesConfig)} exists with GUID " +
                               $"{CustomDebugDrawModesConfig.CustomDebugDrawModesConfigGuid}.");
                return;
            }

            EditorApplication.delayCall += InitializeAfterDelay;
        }

        private static void InitializeAfterDelay()
        {
            UniversalRenderPipelineAsset universalRenderPipelineAsset =
                GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset;
            if (universalRenderPipelineAsset == null)
            {
                Debug.LogError($"Checked the Default Render Pipeline in the Graphics settings but it was not a " +
                               $"UniversalRenderPipelineAsset. Did you install URP Buffer Debugging in a BRP or HDRP " +
                               $"project?");
                return;
            }
            
            UniversalRendererData data = universalRenderPipelineAsset.rendererDataList[0] as UniversalRendererData;
            if (universalRenderPipelineAsset == null)
            {
                Debug.LogError($"Checked the Default Render Pipeline's renderers but could not find a " +
                               $"UniversalRendererData. Did you install URP Buffer Debugging in a BRP or HDRP " +
                               $"project?");
                return;
            }
            
            if (data.renderingMode != RenderingMode.Deferred)
            {
                Debug.LogError($"Custom Debug Draw Modes will not be registered because it seems like your project " +
                               $"is set to {data.renderingMode}, and the Debug Draw Modes are for " +
                               $"Deferred Rendering URP projects (there seems to currently be no support for " +
                               $"enabling/disabling custom draw modes so I have to not register them instead)");
                return;
            }

            CustomDebugDrawModesConfig.Instance.RegisterDebugDrawModes();
        }

        private static void OnUpdateEditor()
        {
            if (SceneView.lastActiveSceneView == currentSceneView)
                return;
            
            if (currentSceneView != null)
            {
                currentSceneView.onCameraModeChanged -= OnDrawModeChanged;

                if (lastActiveDebugDrawMode != null)
                {
                    lastActiveDebugDrawMode.Disable(currentSceneView);
                    lastActiveDebugDrawMode = null;
                }
            }

            if (SceneView.lastActiveSceneView != null)
            {
                currentSceneView = SceneView.lastActiveSceneView;

                lastActiveDebugDrawMode?.Enable(currentSceneView);
                currentSceneView.onCameraModeChanged += OnDrawModeChanged;
            }
        }

        private static void OnDrawModeChanged(SceneView.CameraMode mode)
        {
            if (CustomDebugDrawModesConfig.Instance == null)
                return;
            
            CustomDebugDrawMode customDebugDrawMode = CustomDebugDrawModesConfig.Instance.GetDebugDrawMode(mode);
            
            if (lastActiveDebugDrawMode != null)
            {
                lastActiveDebugDrawMode.Disable(currentSceneView);
                lastActiveDebugDrawMode = null;
            }
            
            lastActiveDebugDrawMode = customDebugDrawMode;
            lastActiveDebugDrawMode?.Enable(currentSceneView);
        }
    }
}
