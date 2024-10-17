using UnityEditor;

namespace RoyTheunissen.URPBufferDebugging.Utilities
{
    /// <summary>
    /// Shows a field in the inspector only when a certain field has a certain value.
    /// </summary>
    [CustomPropertyDrawer(typeof(ShowIfAttribute))]
    public class ShowIfPropertyDrawer : ShowConditionalPropertyDrawer
    {
    }
}
