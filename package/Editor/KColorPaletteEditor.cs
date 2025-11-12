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

        private FieldInfo dictionaryField;
        private readonly List<string> keyBuffer = new();
        private bool showNamedColors = true;

        private static readonly GUIContent NamedColorsLabel = new("Named Colors");
        private static readonly GUIContent AddButtonContent = new("Add Color", "Append a new named color entry");
        private static readonly GUIContent RemoveButtonContent = new("-", "Remove this color");

        private void OnEnable()
        {
            dictionaryField = typeof(KColorPalette).GetField("colorsList", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
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
                if (dictionaryField == null)
                {
                    EditorGUILayout.HelpBox("colorsList field not found or has an incompatible type.", MessageType.Error);
                    return;
                }

                showNamedColors = EditorGUILayout.Foldout(showNamedColors, NamedColorsLabel, true);
                if (!showNamedColors)
                {
                    return;
                }

                EditorGUI.indentLevel++;

                var palette = target as KColorPalette;
                var dictionary = EnsureDictionary(palette);

                if (dictionary == null)
                {
                    EditorGUILayout.HelpBox("Failed to access colorsList dictionary.", MessageType.Error);
                    EditorGUI.indentLevel--;
                    return;
                }

                keyBuffer.Clear();
                keyBuffer.AddRange(dictionary.Keys);

                foreach (var key in keyBuffer)
                {
                    DrawDictionaryRow(key, dictionary);
                }

                EditorGUILayout.Space(4f);

                if (GUILayout.Button(AddButtonContent))
                {
                    ApplyToTargets("Add Color", dict =>
                    {
                        var uniqueKey = GenerateUniqueKey(dict);
                        dict[uniqueKey] = Color.white;
                    });
                }

                EditorGUI.indentLevel--;
            }
        }

        private void DrawDictionaryRow(string key, Dictionary<string, Color> dictionary)
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
                        if (dictionary.ContainsKey(newKey))
                        {
                            EditorUtility.DisplayDialog("Duplicate Name", $"Color \"{newKey}\" already exists.", "OK");
                        }
                        else
                        {
                            ApplyToTargets("Rename Color", dict =>
                            {
                                if (!dict.ContainsKey(key)) return;
                                var value = dict[key];
                                dict.Remove(key);
                                dict[newKey] = value;
                            });
                            return;
                        }
                    }
                }

                EditorGUI.BeginChangeCheck();
                Color newColor = EditorGUILayout.ColorField(dictionary[key]);
                if (EditorGUI.EndChangeCheck())
                {
                    ApplyToTargets("Change Color", dict =>
                    {
                        if (!dict.ContainsKey(key)) return;
                        dict[key] = newColor;
                    });
                }

                if (GUILayout.Button(RemoveButtonContent, GUILayout.Width(RemoveButtonWidth)))
                {
                    ApplyToTargets("Remove Color", dict =>
                    {
                        dict.Remove(key);
                    });
                }
            }
        }

        private void ApplyToTargets(string undoLabel, System.Action<Dictionary<string, Color>> change)
        {
            if (targets == null || targets.Length == 0) return;

            Undo.RecordObjects(targets, undoLabel);
            foreach (var obj in targets)
            {
                if (obj is not KColorPalette palette) continue;
                var dict = EnsureDictionary(palette);
                if (dict == null) continue;
                change(dict);
                EditorUtility.SetDirty(palette);
            }

            Repaint();
        }

        private Dictionary<string, Color> EnsureDictionary(KColorPalette palette)
        {
            if (!palette || dictionaryField == null) return null;

            if (dictionaryField.GetValue(palette) is Dictionary<string, Color> existing)
            {
                return existing;
            }

            var created = new Dictionary<string, Color>();
            dictionaryField.SetValue(palette, created);
            EditorUtility.SetDirty(palette);
            return created;
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
