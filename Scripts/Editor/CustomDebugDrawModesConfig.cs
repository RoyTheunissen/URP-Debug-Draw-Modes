using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RoyTheunissen.URPBufferDebugging
{
    [CreateAssetMenu(fileName = "CustomDebugDrawModesConfig.asset", menuName = "Scriptable Objects/Custom Debug Draw Modes Config")]
    public sealed class CustomDebugDrawModesConfig : ScriptableObject
    {
        public const string CustomDebugDrawModesConfigGuid = "5e0423f24c2bb4b4c94fdca22274330e";
        
        [SerializeField] private List<CustomDebugDrawMode> debugDrawModes;

        private static CustomDebugDrawModesConfig cachedInstance;

        /// <summary>
        ///     Retrieves the singleton asset for this ScriptParams type. If none or more than one asset exists for the type,
        ///     an error is thrown.
        /// </summary>
        public static CustomDebugDrawModesConfig Instance
        {
            get
            {
                if (cachedInstance == null)
                {
                    string path = AssetDatabase.GUIDToAssetPath(CustomDebugDrawModesConfigGuid);
                    cachedInstance = AssetDatabase.LoadAssetAtPath<CustomDebugDrawModesConfig>(path);
                }

                return cachedInstance;
            }
        }

        public CustomDebugDrawMode GetDebugDrawMode(SceneView.CameraMode mode)
        {
            for (int i = 0; i < debugDrawModes.Count; i++)
            {
                if (debugDrawModes[i].Equals(mode))
                    return debugDrawModes[i];
            }

            return null;
        }

        public void RegisterDebugDrawModes()
        {
            // NOTE: I guess this would break user-defined camera modes that are defined elsewhere, but there's no way
            // to check if a camera mode is already registered. The functionality seems quite limited ;(
            SceneView.ClearUserDefinedCameraModes();

            for (int i = 0; i < debugDrawModes.Count; i++)
            {
                if ((UrpBufferDebuggingSettings.ActiveCategories & debugDrawModes[i].Category) ==
                    debugDrawModes[i].Category)
                {
                    SceneView.AddCameraMode(debugDrawModes[i].Name, debugDrawModes[i].Section);
                }
            }
        }
    }
}
