using UnityEngine;

namespace RoyTheunissen.SceneViewDebugModes.Utilities
{
    /// <summary>
    /// Attribute that causes a field's visibility to depend on the value of another field.
    /// </summary>
    public abstract class ShowConditionalAttribute : PropertyAttribute
    {
        private string fieldName;
        public string FieldName => fieldName;

        private object[] targetValues;
        public object[] TargetValues => targetValues;

        protected ShowConditionalAttribute(string fieldName) : this(fieldName, true)
        {
        }

        protected ShowConditionalAttribute(string fieldName, params object[] targetValues)
        {
            this.fieldName = fieldName;
            this.targetValues = targetValues;
        }
    }
}
