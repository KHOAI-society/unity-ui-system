// Editor/ColorKeyDrawer.cs
using UnityEditor;
using UnityEngine;
using System;

[CustomPropertyDrawer(typeof(ColorKey))]
public class ColorKeyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // get the palette reference from the same serialized object
        var paletteProp = property.serializedObject.FindProperty("palette");
        var palette = paletteProp?.objectReferenceValue as ColorPalette;

        EditorGUI.BeginProperty(position, label, property);

        if (palette == null)
        {
            EditorGUI.LabelField(position, label.text, "No palette assigned");
            EditorGUI.EndProperty();
            return;
        }

        // get colors & names from enum
        var colors = palette.colors;
        string[] names = Enum.GetNames(typeof(ColorKey));

        // draw label
        var labelRect = new Rect(position.x, position.y, EditorGUIUtility.labelWidth, position.height);
        EditorGUI.LabelField(labelRect, label);

        float square = position.height - 4;
        float x = position.x + EditorGUIUtility.labelWidth;

        for (int i = 0; i < colors.Count; i++)
        {
            var rect = new Rect(x + i * (square + 4), position.y + 2, square, square);
            EditorGUI.DrawRect(rect, colors[i]);

            // outline
            Handles.color = Color.black;
            Handles.DrawAAPolyLine(2, new Vector3[]
            {
                new(rect.xMin, rect.yMin),
                new(rect.xMax, rect.yMin),
                new(rect.xMax, rect.yMax),
                new(rect.xMin, rect.yMax),
                new(rect.xMin, rect.yMin)
            });

            // click to select
            if (Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition))
            {
                property.enumValueIndex = i;
                property.serializedObject.ApplyModifiedProperties();
                GUI.changed = true;
            }

            // mark currently selected
            if (property.enumValueIndex == i)
            {
                var checkRect = new Rect(rect.x + 2, rect.y + 2, square - 4, square - 4);
                EditorGUI.DrawRect(checkRect, new Color(1, 1, 1, 0.25f));
            }
        }

        EditorGUI.EndProperty();
    }
}
