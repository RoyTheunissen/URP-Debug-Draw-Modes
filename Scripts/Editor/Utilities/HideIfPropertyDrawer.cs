using UnityEditor;

namespace RoyTheunissen.URPDebugDrawModes.Utilities
{
    /// <summary>
    /// Hides a field in the inspector if a certain field has a certain value.
    /// </summary>
    [CustomPropertyDrawer(typeof(HideIfAttribute))]
    public class HideIfPropertyDrawer : ShowConditionalPropertyDrawer
    {
        protected override bool ShouldShow(SerializedProperty property)
        {
            return !base.ShouldShow(property);
        }
    }
}
