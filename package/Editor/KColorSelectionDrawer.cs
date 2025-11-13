using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Khoai.Editors
{
    [CustomPropertyDrawer(typeof(KColorSelectionAttribute))]
    public class KColorSelectionDrawer : PropertyDrawer
    {
        private const float PreviewWidth = 36f;
        private const float PreviewPadding = 4f;

        private readonly List<string> keyCache = new();

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            if (property.propertyType != SerializedPropertyType.String)
            {
                EditorGUI.HelpBox(position, $"{nameof(KColorSelectionAttribute)} can only be applied to string fields.", MessageType.Error);
                EditorGUI.EndProperty();
                return;
            }

            var paletteProp = property.serializedObject.FindProperty("palette");
            if (paletteProp == null)
            {
                EditorGUI.HelpBox(position, "Palette field not found on this object.", MessageType.Error);
                EditorGUI.EndProperty();
                return;
            }

            if (paletteProp.hasMultipleDifferentValues)
            {
                EditorGUI.HelpBox(position, "Assign the same palette on all selected objects to pick a color.", MessageType.Info);
                EditorGUI.EndProperty();
                return;
            }

            var palette = paletteProp.objectReferenceValue as KColorPalette;
            if (palette == null)
            {
                EditorGUI.HelpBox(position, "Assign a palette before selecting a color.", MessageType.Info);
                EditorGUI.EndProperty();
                return;
            }

            var dictionary = palette.colorsList;
            if (dictionary == null || dictionary.Count == 0)
            {
                EditorGUI.HelpBox(position, "Palette has no named colors.", MessageType.Info);
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
                EditorGUI.HelpBox(position, "Palette has no valid color keys.", MessageType.Info);
                EditorGUI.EndProperty();
                return;
            }

            Rect controlRect = EditorGUI.PrefixLabel(position, label);
            float previewWidth = Mathf.Min(PreviewWidth, controlRect.width * 0.35f);
            previewWidth = Mathf.Max(18f, previewWidth);

            Rect colorRect = new(controlRect.xMax - previewWidth, controlRect.y, previewWidth, controlRect.height);
            Rect popupRect = new(controlRect.x, controlRect.y, colorRect.x - controlRect.x - PreviewPadding, controlRect.height);

            if (popupRect.width <= 0f)
            {
                popupRect.width = controlRect.width;
                colorRect.width = 0f;
            }

            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;

            if (colorRect.width <= 0f) return;

            var options = keyCache.ToArray();
            int currentIndex = Array.IndexOf(options, property.stringValue);  
            EditorGUI.BeginChangeCheck(); 
            int newIndex = EditorGUI.Popup(popupRect, currentIndex, options);
            if(EditorGUI.EndChangeCheck())
            {
                currentIndex = newIndex;
                property.stringValue = keyCache[currentIndex];
            }

            var color = dictionary.GetValueOrDefault(property.stringValue, KThemedItem.errorColor);

            EditorGUI.DrawRect(colorRect, color);
            Handles.color = Color.black;
            Handles.DrawAAPolyLine(2f,
                new Vector3(colorRect.xMin, colorRect.yMin),
                new Vector3(colorRect.xMax, colorRect.yMin),
                new Vector3(colorRect.xMax, colorRect.yMax),
                new Vector3(colorRect.xMin, colorRect.yMax),
                new Vector3(colorRect.xMin, colorRect.yMin));

            EditorGUI.EndProperty();
        }
    }
}
