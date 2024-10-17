using UnityEditor;

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
