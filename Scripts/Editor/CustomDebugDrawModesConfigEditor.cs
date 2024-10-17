using UnityEditor;

namespace RoyTheunissen.SceneViewDebugModes
{
    [CustomEditor(typeof(CustomDebugDrawModesConfig))]
    public class CustomDebugDrawModesConfigEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            DrawDefaultInspector();

            CustomDebugDrawModesConfig config = target as CustomDebugDrawModesConfig;
            
            // Draw the active categories as a flags field even though the enum is not explicitly marked with [Flags].
            // This way, you have to pick exactly ONE category for a custom debug draw mode to be in, but we can still
            // specify multiple debug draw modes that should currently be active.
            SerializedProperty activeCategoriesProperty = serializedObject.FindProperty("activeCategories");
            CustomDebugDrawMode.Categories newValue = (CustomDebugDrawMode.Categories)EditorGUILayout.EnumFlagsField(
                activeCategoriesProperty.displayName, config.ActiveCategories);
            activeCategoriesProperty.enumValueFlag = (int)newValue;

            serializedObject.ApplyModifiedProperties();
        }
    }
}
