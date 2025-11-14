using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Khoai.Editors
{
    [CustomEditor(typeof(KSpritePalette))]
    public class KSpritePaletteEditor : Editor
    {
        private const float RemoveButtonWidth = 24f;
        private const float PreviewSize = 56f;

        private readonly List<string> keyBuffer = new();
        private readonly Dictionary<string, Sprite> dictBuffer = new();
        private bool showNamedColors = true;

        private static readonly GUIContent NamedColorsLabel = new("Sprites");
        private static readonly GUIContent AddButtonContent = new("Add Sprite", "Append a new sprite entry");
        private static readonly GUIContent RemoveButtonContent = new("-", "Remove this sprite");

        private SerializedProperty spriteNamesListSerializedProperty;
        private SerializedProperty spriteListSerializedProperty;

        private void OnEnable()
        {
            spriteNamesListSerializedProperty = serializedObject.FindProperty("spriteNamesList");
            spriteListSerializedProperty = serializedObject.FindProperty("spriteList");
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
                    dictBuffer.Add(GenerateUniqueKey(dictBuffer), null);
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
                        EditorUtility.DisplayDialog("Invalid Name", "Sprite name cannot be empty.", "OK");
                    }
                    else if (newKey != key)
                    {
                        if (dictBuffer.ContainsKey(newKey))
                        {
                            EditorUtility.DisplayDialog("Duplicate Name", $"Sprite \"{newKey}\" already exists.", "OK");
                        }
                        else
                        {
                            Sprite oldSprite = dictBuffer[key];
                            dictBuffer.Remove(key);
                            dictBuffer.Add(newKey, oldSprite);
                            SyncToLists();
                            return;
                        }
                    }
                }

                using (new EditorGUILayout.VerticalScope(GUILayout.Width(PreviewSize + 12f)))
                {
                    EditorGUI.BeginChangeCheck();
                    Sprite newSprite = (Sprite)EditorGUILayout.ObjectField(dictBuffer[key], typeof(Sprite), false);
                    if (EditorGUI.EndChangeCheck())
                    {
                        dictBuffer[key] = newSprite;
                        SyncToLists();
                    }
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
            List<string> names = Enumerable.Range(0, spriteNamesListSerializedProperty.arraySize)
                .Select(i => spriteNamesListSerializedProperty.GetArrayElementAtIndex(i).stringValue)
                .ToList();

            List<Sprite> sprites = Enumerable.Range(0, spriteListSerializedProperty.arraySize)
                .Select(i => spriteListSerializedProperty.GetArrayElementAtIndex(i).objectReferenceValue as Sprite)
                .ToList();

            if (names.Count != sprites.Count)
            {
                Debug.LogError("Mismatched sprite list and names list. Filled to match");
            }
            var maxLength = Math.Max(names.Count, sprites.Count);

            dictBuffer.Clear();
            keyBuffer.Clear();
            for(int i=0;i<maxLength;i++)
            {
                if(i == sprites.Count) sprites.Add(null);
                if(i== names.Count) names.Add(GenerateUniqueKey(dictBuffer));

                dictBuffer.Add(names[i], sprites[i]);
                keyBuffer.Add(names[i]);
            }
        }

        private void SyncToLists()
        {
            while (spriteNamesListSerializedProperty.arraySize > 0)
            {
                spriteNamesListSerializedProperty.DeleteArrayElementAtIndex(spriteNamesListSerializedProperty.arraySize - 1);
            }
            while (spriteListSerializedProperty.arraySize > 0)
            {
                spriteListSerializedProperty.DeleteArrayElementAtIndex(spriteListSerializedProperty.arraySize - 1);
            }

            var keys = dictBuffer.Keys.ToArray();
            for(int i =0;i<keys.Length;i++)
            {
                var key = keys[i];
                spriteNamesListSerializedProperty.InsertArrayElementAtIndex(i);
                var nameElement = spriteNamesListSerializedProperty.GetArrayElementAtIndex(i);
                nameElement.stringValue = key;

                spriteListSerializedProperty.InsertArrayElementAtIndex(i);
                var spriteElement = spriteListSerializedProperty.GetArrayElementAtIndex(i);
                spriteElement.objectReferenceValue = dictBuffer[key];
            }
        }

        private static string GenerateUniqueKey(Dictionary<string, Sprite> dictionary)
        {
            int index = dictionary.Count + 1;
            string candidate = $"Sprite {index}";
            while (dictionary.ContainsKey(candidate))
            {
                index++;
                candidate = $"Sprite {index}";
            }

            return candidate;
        }
    }
}
