using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Khoai.Editors
{
    [CustomEditor(typeof(KColorPalette))]
    public class KColorPaletteEditor : Editor
    {
        private const float RemoveButtonWidth = 24f;
        private readonly List<string> keyBuffer = new();
        private readonly Dictionary<string, Color> dictBuffer = new();
        private bool showNamedColors = true;

        private static readonly GUIContent NamedColorsLabel = new("Colors");
        private static readonly GUIContent AddButtonContent = new("Add Color", "Append a new named color entry");
        private static readonly GUIContent RemoveButtonContent = new("-", "Remove this color");

        private SerializedProperty colorNamesListSerializedProperty;
        private SerializedProperty colorListSerializedProperty;

        private void OnEnable()
        {
            colorNamesListSerializedProperty = serializedObject.FindProperty("colorNamesList");
            colorListSerializedProperty = serializedObject.FindProperty("colorList");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawNamedColorsSection();

            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(target);
            Repaint();
        }

        private void DrawNamedColorsSection()
        {
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                SyncListsToDict();

                showNamedColors = EditorGUILayout.Foldout(showNamedColors, NamedColorsLabel, true);
                if (!showNamedColors)
                {
                    return;
                }

                EditorGUI.indentLevel++;


                foreach (var key in keyBuffer)
                {
                    DrawDictionaryRow(key);
                }

                EditorGUILayout.Space(4f);

                if (GUILayout.Button(AddButtonContent))
                {
                    dictBuffer.Add(GenerateUniqueKey(dictBuffer), Color.white);
                    SyncToLists();
                }

                EditorGUI.indentLevel--;
            }
        }

        private void DrawDictionaryRow(string key)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUI.BeginChangeCheck();
                string newKey = EditorGUILayout.TextField(key);
                if (EditorGUI.EndChangeCheck())
                {
                    newKey = newKey.Trim();
                    if (string.IsNullOrEmpty(newKey))
                    {
                        EditorUtility.DisplayDialog("Invalid Name", "Color name cannot be empty.", "OK");
                    }
                    else if (newKey != key)
                    {
                        if (dictBuffer.ContainsKey(newKey))
                        {
                            EditorUtility.DisplayDialog("Duplicate Name", $"Color \"{newKey}\" already exists.", "OK");
                        }
                        else
                        {
                            Color oldColor = dictBuffer[key];
                            dictBuffer.Remove(key);
                            dictBuffer.Add(newKey, oldColor);
                            SyncToLists();
                            return;
                        }
                    }
                }

                EditorGUI.BeginChangeCheck();
                Color newColor = EditorGUILayout.ColorField(dictBuffer[key]);
                if (EditorGUI.EndChangeCheck())
                {
                    dictBuffer[key] = newColor;
                    SyncToLists();
                }

                if (GUILayout.Button(RemoveButtonContent, GUILayout.Width(RemoveButtonWidth)))
                {
                    dictBuffer.Remove(key);
                    SyncToLists();
                }
            }
        }

        private void SyncListsToDict()
        {
            List<string> names = Enumerable.Range(0, colorNamesListSerializedProperty.arraySize)
                .Select(i => colorNamesListSerializedProperty.GetArrayElementAtIndex(i).stringValue)
                .ToList();

            List<Color> colors = Enumerable.Range(0, colorListSerializedProperty.arraySize)
                .Select(i => colorListSerializedProperty.GetArrayElementAtIndex(i).colorValue)
                .ToList();

            if (names.Count != colors.Count)
            {
                Debug.LogError("Mismatched colors list and names list. Filled to match");
            }
            var maxLength = Math.Max(names.Count, colors.Count);

            dictBuffer.Clear();
            keyBuffer.Clear();
            for(int i=0;i<maxLength;i++)
            {
                if(i == colors.Count) colors.Add(Color.white);
                if(i== names.Count) names.Add(GenerateUniqueKey(dictBuffer));

                dictBuffer.Add(names[i], colors[i]);
                keyBuffer.Add(names[i]);
            }
        }

        private void SyncToLists()
        {
            while (colorNamesListSerializedProperty.arraySize > 0)
            {
                colorNamesListSerializedProperty.DeleteArrayElementAtIndex(colorNamesListSerializedProperty.arraySize - 1);
            }
            while (colorListSerializedProperty.arraySize > 0)
            {
                colorListSerializedProperty.DeleteArrayElementAtIndex(colorListSerializedProperty.arraySize - 1);
            }

            var keys = dictBuffer.Keys.ToArray();
            for(int i =0;i<keys.Length;i++)
            {
                var key = keys[i];
                colorNamesListSerializedProperty.InsertArrayElementAtIndex(i);
                var nameElement = colorNamesListSerializedProperty.GetArrayElementAtIndex(i);
                nameElement.stringValue = key;

                colorListSerializedProperty.InsertArrayElementAtIndex(i);
                var colorElement = colorListSerializedProperty.GetArrayElementAtIndex(i);
                colorElement.colorValue = dictBuffer[key];
            }
        }

        private static string GenerateUniqueKey(Dictionary<string, Color> dictionary)
        {
            int index = dictionary.Count + 1;
            string candidate = $"Color {index}";
            while (dictionary.ContainsKey(candidate))
            {
                index++;
                candidate = $"Color {index}";
            }

            return candidate;
        }
    }
}
