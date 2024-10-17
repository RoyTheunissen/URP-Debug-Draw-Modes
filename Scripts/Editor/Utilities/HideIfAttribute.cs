namespace RoyTheunissen.URPBufferDebugging.Utilities
{
    /// <summary>
    /// Hides a field in the inspector if a certain field has a certain value.
    /// </summary>
    public class HideIfAttribute : ShowConditionalAttribute
    {
        public HideIfAttribute(string fieldName) : base(fieldName)
        {
        }

        public HideIfAttribute(string fieldName, params object[] targetValues) : base(fieldName, targetValues)
        {
        }
    }
}
