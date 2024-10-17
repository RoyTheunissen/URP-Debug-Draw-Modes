using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RoyTheunissen.SceneViewDebugModes
{
    [CreateAssetMenu(fileName = "CustomDebugDrawModesConfig.asset", menuName = "Scriptable Objects/Custom Debug Draw Modes Config")]
    public sealed class CustomDebugDrawModesConfig : ScriptableObject
    {
        [SerializeField] private List<CustomDebugDrawMode> debugDrawModes;

        [SerializeField, HideInInspector] private CustomDebugDrawMode.Categories activeCategories = (CustomDebugDrawMode.Categories)~0;
        public CustomDebugDrawMode.Categories ActiveCategories => activeCategories;

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
                    cachedInstance = AssetDatabase.LoadAssetAtPath<CustomDebugDrawModesConfig>(
                            "Assets/URP-Buffer-Debugging/Configs/Editor/CustomDebugDrawModesConfig.asset");
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
            for (int i = 0; i < debugDrawModes.Count; i++)
            {
                if ((activeCategories & debugDrawModes[i].Category) == debugDrawModes[i].Category)
                    SceneView.AddCameraMode(debugDrawModes[i].Name, debugDrawModes[i].Section);
            }
        }
    }
}
