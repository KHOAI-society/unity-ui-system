using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Khoai.Editors
{
    [CustomEditor(typeof(KColorPalette))]
    public class KColorPaletteEditor : Editor
    {
        private const float RemoveButtonWidth = 24f;
        private KColorPalette kColorPalette;
        private FieldInfo dictNamesField;
        private FieldInfo dictColorsField;
        private readonly List<string> keyBuffer = new();
        private readonly Dictionary<string, Color> dictBuffer = new();
        private List<string> dictNamesBuffer = new();
        private List<Color> dictColorsBuffer = new();
        private bool showNamedColors = true;

        private static readonly GUIContent NamedColorsLabel = new("Named Colors");
        private static readonly GUIContent AddButtonContent = new("Add Color", "Append a new named color entry");
        private static readonly GUIContent RemoveButtonContent = new("-", "Remove this color");

        private void OnEnable()
        {
            dictNamesField = typeof(KColorPalette).GetField("dictsName", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            dictColorsField = typeof(KColorPalette).GetField("dictsColors", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawNamedColorsSection();

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawNamedColorsSection()
        {
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                var errorMessage = SyncListsToDict();
                if(errorMessage != null)
                {
                    EditorGUILayout.HelpBox(errorMessage, MessageType.Error);
                    return;
                }

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

        private string SyncListsToDict()
        {
            kColorPalette = target as KColorPalette;
            dictColorsBuffer.Clear();
            dictNamesBuffer.Clear();

            if (!kColorPalette) return "Color palette does not exist";
            if (dictNamesField == null) return "Name list does not exist";
            if (dictColorsField == null) return "Color list does not exist";
            
            if (dictColorsField.GetValue(target) is List<Color> tempColorBuffer)
                dictColorsBuffer.AddRange(tempColorBuffer);
            if (dictNamesField.GetValue(kColorPalette) is List<string> tempNamesBuffer)
                dictNamesBuffer.AddRange(tempNamesBuffer);

            if (dictColorsBuffer.Count != dictNamesBuffer.Count)
            {
                Debug.LogError("Mismatched colors list and names list. Filled to match");
            }
            var maxLength = Math.Max(dictColorsBuffer.Count, dictNamesBuffer.Count);

            dictBuffer.Clear();
            keyBuffer.Clear();
            for(int i=0;i<maxLength;i++)
            {
                if(i == dictColorsBuffer.Count) dictColorsBuffer.Add(Color.white);
                if(i== dictNamesBuffer.Count) dictNamesBuffer.Add(GenerateUniqueKey(dictBuffer));

                dictBuffer.Add(dictNamesBuffer[i], dictColorsBuffer[i]);
            }
            keyBuffer.AddRange(dictNamesBuffer);
            return null;
        }

        private void SyncToLists()
        {
            kColorPalette = target as KColorPalette;
            dictColorsBuffer.Clear();
            dictNamesBuffer.Clear();
            
            foreach(var key in dictBuffer.Keys)
            {
                dictNamesBuffer.Add(key);
                dictColorsBuffer.Add(dictBuffer[key]);
            }
            dictNamesField.SetValue(kColorPalette, dictNamesBuffer);
            dictColorsField.SetValue(kColorPalette, dictColorsBuffer);
            EditorUtility.SetDirty(kColorPalette);
            Repaint();
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
