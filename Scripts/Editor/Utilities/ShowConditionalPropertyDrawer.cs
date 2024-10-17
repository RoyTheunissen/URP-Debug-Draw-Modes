using System;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace RoyTheunissen.SceneViewDebugModes.Utilities
{
    /// <summary>
    /// Makes sure a field is only shown if a certain condition is met.
    /// </summary>
    public abstract class ShowConditionalPropertyDrawer : PropertyDrawer
    {
        private const float Indentation = 16;
        
        /// <summary>
        /// From: https://gist.github.com/monry/9de7009689cbc5050c652bcaaaa11daa
        /// </summary>
        private static SerializedProperty GetParent(SerializedProperty serializedProperty)
        {
            string[] propertyPaths = serializedProperty.propertyPath.Split('.');
            if (propertyPaths.Length <= 1)
            {
                return default(SerializedProperty);
            }

            SerializedProperty parentSerializedProperty =
                serializedProperty.serializedObject.FindProperty(propertyPaths.First());
            for (int index = 1; index < propertyPaths.Length - 1; index++)
            {
                if (propertyPaths[index] == "Array")
                {
                    if (index + 1 == propertyPaths.Length - 1)
                    {
                        // reached the end
                        break;
                    }
                    if (propertyPaths.Length > index + 1 && Regex.IsMatch(propertyPaths[index + 1], "^data\\[\\d+\\]$"))
                    {
                        Match match = Regex.Match(propertyPaths[index + 1], "^data\\[(\\d+)\\]$");
                        int arrayIndex = int.Parse(match.Groups[1].Value);
                        parentSerializedProperty = parentSerializedProperty.GetArrayElementAtIndex(arrayIndex);
                        index++;
                    }
                }
                else
                {
                    parentSerializedProperty = parentSerializedProperty.FindPropertyRelative(propertyPaths[index]);
                }
            }

            return parentSerializedProperty;
        }
        
        private static SerializedProperty FindPropertyInParent(
            SerializedProperty serializedProperty, string propertyPath)
        {
            SerializedProperty parentProperty = GetParent(serializedProperty);
            
            // If this is a top level property already, just find the property in the serialized object itself.
            if (parentProperty == default)
                return serializedProperty.serializedObject.FindProperty(propertyPath);
            
            return parentProperty.FindPropertyRelative(propertyPath);
        }
        
        protected virtual bool ShouldShow(SerializedProperty property)
        {
            ShowConditionalAttribute showIf = attribute as ShowConditionalAttribute;
            
            SerializedProperty conditionalProperty = FindPropertyInParent(property, showIf.FieldName);

            // If no specific values are specified, we are just checking if it is 'true'.
            if (showIf.TargetValues.Length == 0)
                return IsPropertyEqualTo(conditionalProperty, true);

            // Check if the property has any of the specified values.
            for (int i = 0; i < showIf.TargetValues.Length; i++)
            {
                object targetValue = showIf.TargetValues[i];

                if (IsPropertyEqualTo(conditionalProperty, targetValue))
                    return true;
            }

            return false;
        }

        private static bool IsPropertyEqualTo(SerializedProperty property, object targetValue)
        {
            if (targetValue == null)
                return property.objectReferenceValue == null;
            
            if (targetValue is UnityEngine.Object unityObject)
                return unityObject == property.objectReferenceValue;

            switch (targetValue)
            {
                case int integer: return integer == property.intValue;
                case long longInteger: return longInteger == property.longValue;
                case bool boolean: return boolean == property.boolValue;
                case float singlePrecision: return Mathf.Approximately(singlePrecision, property.floatValue);
                case double doublePrecision: return Math.Abs(doublePrecision - property.doubleValue) < 1E-06f;
                case string text: return text == property.stringValue;
                case Color color: return color == property.colorValue;
                case AnimationCurve animationCurve: return animationCurve.Equals(property.animationCurveValue);
                case Enum @enum: return (int)targetValue == property.enumValueIndex;
                case Vector2 vector2: return vector2 == property.vector2Value;
                case Vector3 vector3: return vector3 == property.vector3Value;
                case Vector4 vector4: return vector4 == property.vector4Value;
                case Vector2Int vector2Int: return vector2Int == property.vector2IntValue;
                case Vector3Int vector3Int: return vector3Int == property.vector3IntValue;
                case Quaternion quaternion: return quaternion == property.quaternionValue;
                case Rect rect: return rect == property.rectValue;
                case RectInt rectInt: return rectInt.Equals(property.rectIntValue);
                case Bounds bounds: return bounds == property.boundsValue;
                case BoundsInt boundsInt: return boundsInt.Equals(property.boundsIntValue);
                default: throw new ArgumentException($"Tried to compare unknown value to target value '{targetValue}'");
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (!ShouldShow(property))
                return 0.0f;
            
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (!ShouldShow(property))
                return;

            using (new EditorGUI.IndentLevelScope())
            {
                EditorGUI.PropertyField(position, property, label, true);
            }
        }
    }
}
