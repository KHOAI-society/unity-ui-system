using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Khoai.Editors
{
    [CustomPropertyDrawer(typeof(KSpriteSelectionAttribute))]
    public class KSpriteSelectionDrawer : PropertyDrawer
    {
        private const float PreviewPadding = 4f;
        private readonly List<string> keyCache = new();

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            if (property.type != "KThemedItemProperty")
            {
                EditorGUI.HelpBox(position, $"{nameof(KThemedItemProperty)} can only be applied to KThemedItemProperty fields.", MessageType.Error);
                EditorGUI.EndProperty();
                return;
            }

            var paletteProp = property.serializedObject.FindProperty("spritePalette");
            if (paletteProp == null)
            {
                EditorGUI.HelpBox(position, "spritePalette field not found on this object.", MessageType.Error);
                EditorGUI.EndProperty();
                return;
            }

            if (paletteProp.hasMultipleDifferentValues)
            {
                EditorGUI.HelpBox(position, "Assign the same palette on all selected objects to pick a color.", MessageType.Info);
                EditorGUI.EndProperty();
                return;
            }

            var palette = paletteProp.objectReferenceValue as KSpritePalette;
            if (palette == null)
            {
                EditorGUI.HelpBox(position, "Assign a palette before selecting a sprite.", MessageType.Info);
                EditorGUI.EndProperty();
                return;
            }

            var dictionary = palette.SpriteMap;
            if (dictionary == null || dictionary.Count == 0)
            {
                EditorGUI.HelpBox(position, "Palette has no named sprites.", MessageType.Info);
                EditorGUI.EndProperty();
                return;
            }

            keyCache.Clear();
            foreach (var key in dictionary.Keys)
            {
                if (string.IsNullOrEmpty(key)) continue;
                keyCache.Add(key);
            }

            if (keyCache.Count == 0)
            {
                EditorGUI.HelpBox(position, "Palette has no valid sprite keys.", MessageType.Info);
                EditorGUI.EndProperty();
                return;
            }

            Rect controlRect = EditorGUI.PrefixLabel(position, label);
            if (controlRect.width <= 0f) return;

            float controlRectX = controlRect.x;
            float dropdownWidth =controlRect.width * 0.8f;
            float objectWidth = controlRect.width * 0.1f;
            float paddingWidth = 0f;

            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;

            
            var options = keyCache.ToArray();
            SerializedProperty useProp = property.FindPropertyRelative("use");
            SerializedProperty colorNameProp = property.FindPropertyRelative("itemName");

            bool useValue = useProp.boolValue;
            string currentKey = colorNameProp.stringValue;

            int currentIndex = Array.IndexOf(options, currentKey);  
            
            EditorGUI.BeginChangeCheck(); 
            useValue = EditorGUI.Toggle(new Rect(controlRectX, controlRect.y, objectWidth, controlRect.height), useValue);
            
            EditorGUI.BeginDisabledGroup(!useValue);
            controlRectX = controlRectX + objectWidth + paddingWidth;
            int newIndex = EditorGUI.Popup(new Rect(controlRectX, controlRect.y, dropdownWidth, controlRect.height), currentIndex, options);

            
            if(EditorGUI.EndChangeCheck())
            {
                currentIndex = newIndex;
                useProp.boolValue = useValue;
                colorNameProp.stringValue = keyCache.ElementAtOrDefault(currentIndex);
            }

            var color = dictionary.GetValueOrDefault(colorNameProp.stringValue, null);

            controlRectX = controlRectX + dropdownWidth + paddingWidth;
            var colorRect = new Rect(controlRectX, controlRect.y,
                                    objectWidth, controlRect.height);
            // EditorGUI.DrawRect(colorRect, color);
            Handles.color = Color.black;
            Handles.DrawAAPolyLine(2f,
                new Vector3(colorRect.xMin, colorRect.yMin),
                new Vector3(colorRect.xMax, colorRect.yMin),
                new Vector3(colorRect.xMax, colorRect.yMax),
                new Vector3(colorRect.xMin, colorRect.yMax),
                new Vector3(colorRect.xMin, colorRect.yMin));

            EditorGUI.EndProperty();
            EditorGUI.EndDisabledGroup();
        }
    }
}
