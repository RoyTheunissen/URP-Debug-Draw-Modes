using UnityEditor;
using UnityEngine;

namespace RoyTheunissen.SceneViewDebugModes
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
