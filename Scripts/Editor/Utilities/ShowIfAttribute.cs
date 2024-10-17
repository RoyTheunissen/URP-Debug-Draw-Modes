namespace RoyTheunissen.URPBufferDebugging.Utilities
{
    /// <summary>
    /// Shows a field in the inspector only when a certain field has a certain value.
    /// </summary>
    public class ShowIfAttribute : ShowConditionalAttribute
    {
        public ShowIfAttribute(string fieldName) : base(fieldName)
        {
        }

        public ShowIfAttribute(string fieldName, params object[] targetValues) : base(fieldName, targetValues)
        {
        }
    }
}
