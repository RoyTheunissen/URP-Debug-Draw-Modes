using UnityEditor;

namespace RoyTheunissen.URPDebugDrawModes.Utilities
{
    /// <summary>
    /// Shows a field in the inspector only when a certain field has a certain value.
    /// </summary>
    [CustomPropertyDrawer(typeof(ShowIfAttribute))]
    public class ShowIfPropertyDrawer : ShowConditionalPropertyDrawer
    {
    }
}
