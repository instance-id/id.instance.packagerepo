using System.Reflection;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace instance.id.Extensions
{
    public static class EditorHelper
    {
        public static void AddDefaultInspector(VisualElement container, SerializedObject serializedObject)
        {
            var property = serializedObject.GetIterator();
            if (property.NextVisible(true)) // Expand first child.
            {
                do
                {
                    var field = new PropertyField(property) {name = "PropertyField:" + property.propertyPath};
                    if (property.propertyPath == "m_Script" && serializedObject.targetObject != null)
                    {
                        field.SetEnabled(false);
                    }

                    container.Add(field);
                } while (property.NextVisible(false));
            }
        }

        public static void AddDefaultPropertyInspector(VisualElement container, SerializedProperty inputProperty)
        {
            var property = inputProperty.Copy();
            var rootPath = property.propertyPath;
            if (property.NextVisible(true)) // Expand first child.
            {
                do
                {
                    if (!property.propertyPath.Contains(rootPath))
                    {
                        return;
                    }

                    var field = new PropertyField(property) {name = "PropertyField:" + property.propertyPath};
                    container.Add(field);
                } while (property.NextVisible(false));
            }
        }

        public static T GetSerializedPropertyValue<T>(SerializedProperty property) where T : class
        {
            // Based on https://forum.unity.com/threads/get-a-general-object-value-from-serializedproperty.327098/
            // Doesn't handle lists correctly
            object obj = property.serializedObject.targetObject;
            foreach (var path in property.propertyPath.Split('.'))
            {
                var type = obj.GetType();
                var field = type.GetField(path, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (field == null)
                {
                    Debug.LogError($"Field not found: {path}");
                    return null;
                }

                obj = field.GetValue(obj);
            }

            return obj as T;
        }
    }
}
