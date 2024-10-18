using System.Collections.Generic;
using UnityEditor;
using SettingsProvider = UnityEditor.SettingsProvider;

namespace RoyTheunissen.URPDebugDrawModes
{
    public static class UrpDebugDrawModesSettings
    {
        private const string ActiveCategoriesEditorPref = "RoyTheunissen/URPDebugDrawModes/ActiveCategories";
        public static CustomDebugDrawMode.Categories ActiveCategories
        {
            get
            {
                // By default, just show the Deferred category.
                if (!EditorPrefs.HasKey(ActiveCategoriesEditorPref))
                    return CustomDebugDrawMode.Categories.Deferred;
                
                return (CustomDebugDrawMode.Categories)EditorPrefs.GetInt(ActiveCategoriesEditorPref);
            }
            set
            {
                if (ActiveCategories != value)
                    EditorPrefs.SetInt(ActiveCategoriesEditorPref, (int)value);
            }
        }

        [SettingsProvider]
        public static SettingsProvider UrpDebugDrawModesSettingsProvider()
        {
            SettingsProvider provider = new SettingsProvider("Project/UrpDebugDrawModesSettings", SettingsScope.User)
            {
                label = "URP Debug Draw Modes",
                
                guiHandler = (searchContext) =>
                {
                    float originalLabelWidth = EditorGUIUtility.labelWidth;
                    EditorGUIUtility.labelWidth = originalLabelWidth + 50;
                    
                    // Draw the active categories as a flags field even though the enum is not explicitly marked with
                    // [Flags]. This way, you have to pick exactly ONE category for a custom debug draw mode to be in,
                    // but we can still specify multiple debug draw modes that should currently be active.
                    ActiveCategories = (CustomDebugDrawMode.Categories)EditorGUILayout.EnumFlagsField(
                        "Active Categories", ActiveCategories);

                    originalLabelWidth = EditorGUIUtility.labelWidth;
                },
                
                keywords = new HashSet<string>(new[] { "Deferred", "GBuffer", "Buffer", "Debugging", "Graphics", "Draw Modes", "Debug", "Debug Draw Modes" })
            };

            return provider;
        }
    }
}
