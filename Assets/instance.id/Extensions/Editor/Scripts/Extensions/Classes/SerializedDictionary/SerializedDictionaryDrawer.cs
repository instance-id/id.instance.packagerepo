using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace instance.id.Extensions.Editors
{
    // -- SerializedDictionary.cs
    [CustomEditor(typeof(DictionaryAttribute))] // @formatter:off
    public class SerializedDictEditor : DefaultUIElementsEditor { } // @formatter:on

    [CustomPropertyDrawer(typeof(DictionaryAttribute))]
    public class SerializedDictionaryDrawer : PropertyDrawer
    {
        private StyleSheet styleSheet;
        private VisualElement container;
        private SerializedProperty propertyKeyField;
        private SerializedProperty propertyValueField;

        private List<string> objectTypes = new List<string>
        {
            "LayerTag",
            "PPtr<$LayerTag>",
            "SubStreamEvent",
            "ObjectID",
            "SerializedScriptableObject",
            "ScriptableObject",
            "Object",
            "PPtr<$Object>",
            "SubSceneData",
            "VolumeObject",
            "ObjectID",
            "VolumeObject",
            "GridVolume"
        };

        private List<string> guidTypes = new List<string>
        {
            "GuidComponent",
            "PPtr<$GuidComponent>"
        };

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            container = new VisualElement();
            if (styleSheet is null)
            {
                styleSheet = typeof(SerializedDictionary<object, object>).GetStyleSheet($"InstanceId{nameof(SerializedDictionary<object, object>)}");
                if (!(styleSheet is null)) container.styleSheets.Add(styleSheet);
            }

            propertyKeyField = property.FindPropertyRelative(SerializedDictionary<object, object>.KeyProperty);
            propertyValueField = property.FindPropertyRelative(SerializedDictionary<object, object>.ValueProperty);
            propertyKeyField.serializedObject.ApplyModifiedProperties();
            propertyValueField.serializedObject.ApplyModifiedProperties();

            var serializedDictionaryFoldout = new Foldout {text = property.displayName, value = false};
            serializedDictionaryFoldout.NameAsUSS(nameof(serializedDictionaryFoldout));

            var box = new Box();
            var scroller = new ScrollView {name = "serialDictScroller"};

            for (int i = 0; i < propertyKeyField.arraySize; i++)
            {
                var keyTypeString = propertyKeyField.GetArrayElementAtIndex(i).type;
                var keyType = propertyKeyField.GetArrayElementAtIndex(i).GetType();
                var valueTypeString = propertyValueField.GetArrayElementAtIndex(i).type;
                var valueType = propertyValueField.GetArrayElementAtIndex(i).GetType();

                // ----------------------------------------------------------- Dictionary Container
                // -- Dictionary Container --------------------------------------------------------
                var layerEntry = new VisualElement {focusable = true, name = $"Entry: {i}"};
                layerEntry.AddToClassList("serialDictionaryContainer");

                // ----------------------------------------------------------------- Dictionary Key
                // -- Dictionary Key --------------------------------------------------------------
                VisualElement listKey;
                listKey = new PropertyField(propertyKeyField.GetArrayElementAtIndex(i))
                {
                    name = "keyTextField"
                };

                var keyTextField = new TextField
                {
                    bindingPath = propertyKeyField.propertyPath,
                    label = null,
                    name = "keyTextField"
                };
                keyTextField.tooltip = keyTextField.text;

                switch (valueTypeString)
                {
                    case { } a when a.Contains("GuidReference"):
                    case { } b when b.Contains("GuidComponent"):
                        keyTextField.AddToClassList("serialDictionaryKeyGuid");
                        keyTextField.AddToClassList("unity-base-field--no-label");
                        break;
                    default:
                        keyTextField.AddToClassList("serialDictionaryKey");
                        keyTextField.AddToClassList("unity-base-field--no-label");
                        break;
                }

                keyTextField.SetValueWithoutNotify(propertyKeyField.GetArrayElementAtIndex(i).stringValue);
                keyTextField.SetEnabled(false);
                // keyTextField.Q(null, "unity-base-text-field__input").RemoveFromClassList("unity-base-text-field__input");

                keyTextField.Q(null, "unity-disabled")
                    .RemoveFromClassList("unity-disabled");

                keyTextField.AddToClassList("serialDictionaryKeyLocator");
                keyTextField.SetEnabled(false);

                listKey = keyTextField;
                layerEntry.Add(listKey);

                // --------------------------------------------------------------- Dictionary Value
                // -- Dictionary Value ------------------------------------------------------------
                VisualElement listValue;
                listValue = new PropertyField(propertyValueField.GetArrayElementAtIndex(i))
                {
                    name = "valueObjectField"
                };
                listValue.SetEnabled(true);

                switch (valueType)
                {
                    case { } a when objectTypes.Contains(a.Name):
                    case { } t1 when t1 == typeof(Object):
                    case { } t2 when typeof(Object).IsSubclassOf(t2):
                    case { } t3 when typeof(Object).IsAssignableFrom(t3):
                        
                    case { } t4 when Convert.GetTypeCode(t4) == TypeCode.Object:
                    case { } t5 when typeof(Object).IsSubclassOf(t5):
                    case { } t6 when typeof(Object).IsAssignableFrom(t6):
                        var objectField = new ObjectField
                        {
                            bindingPath = propertyValueField.propertyPath,
                            label = null,
                            name = "valueObjectField"
                        };
                        objectField.SetValueWithoutNotify(propertyValueField.GetArrayElementAtIndex(i).objectReferenceValue);
                        objectField.AddToClassList("serialDictionaryValue");
                        objectField.AddToClassList("unity-base-field--no-label");
                        objectField.Q(null, "unity-object-field__selector")
                            .RemoveFromClassList("unity-object-field__selector");
                        // objectField.Q(null, "unity-object-field__input").RemoveFromClassList("unity-object-field__input");
                        listValue = objectField;
                        break;
                    case { } b when guidTypes.Contains(b.Name):
                        var guidObjectField = new ObjectField
                        {
                            bindingPath = propertyValueField.propertyPath,
                            label = null,
                            name = "valueObjectField"
                        };
                        guidObjectField.SetValueWithoutNotify(propertyValueField.GetArrayElementAtIndex(i).objectReferenceValue);
                        guidObjectField.AddToClassList("serialDictionaryGuidValue");
                        guidObjectField.AddToClassList("unity-base-field--no-label");
                        guidObjectField.Q(null, "unity-object-field__selector")
                            .RemoveFromClassList("unity-object-field__selector");
                        // objectField.Q(null, "unity-object-field__input").RemoveFromClassList("unity-object-field__input");
                        listValue = guidObjectField;
                        break;
                    case { } d when d == (typeof(int)):
                        var valueTextField = new TextField
                        {
                            bindingPath = propertyValueField.propertyPath,
                            label = null
                        };
                        valueTextField.SetValueWithoutNotify(propertyValueField.GetArrayElementAtIndex(i).intValue.ToString());
                        valueTextField.SetEnabled(false);
                        listValue = valueTextField;
                        listValue.AddToClassList("serialDictionaryValue");
                        listValue.RemoveFromClassList("unity-base-text-field__input");
                        break;
                    case { } d when d == typeof(Type):

                        var valueTypeTextField = new TextField
                        {
                            bindingPath = propertyValueField.propertyPath,
                            label = null
                        };
                        valueTypeTextField.SetValueWithoutNotify(propertyValueField.GetArrayElementAtIndex(i).stringValue);
                        valueTypeTextField.SetEnabled(false);
                        listValue = valueTypeTextField;
                        listValue.AddToClassList("serialDictionaryValue");
                        listValue.RemoveFromClassList("unity-base-text-field__input");
                        break;
                    default:
                        if (property.IsReallyArray())
                        {
                            listValue = new PropertyField(propertyValueField);
                        }
                        
                        listValue.AddToClassList("serialDictionaryValue");
                        listValue.AddToClassList("unity-base-field--no-label");
                        break;
                }

                listValue.AddToClassList("serialDictionaryValueLocator");
                layerEntry.Add(listValue);

                scroller.Add(layerEntry);
            }

            box.Add(scroller);
            serializedDictionaryFoldout.Add(box);
            container.Add(serializedDictionaryFoldout);

            return container;
        }
    }
}
