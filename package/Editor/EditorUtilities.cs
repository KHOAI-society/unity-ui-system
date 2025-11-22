using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Khoai.Editors
{
    public static class EditorUtilities
    {
        // Actual action
        [MenuItem("GameObject/Khoai/Copy Path", false, 0)]
        private static void LogSelectedPath()
        {
            var t = Selection.activeGameObject.transform;
            var names = new List<string>();
            while (t != null) { names.Add(t.name); t = t.parent; }
            names.Reverse();
            GUIUtility.systemCopyBuffer = string.Join("/", names);
        }

        // Validation so it shows only when a GameObject is selected
        [MenuItem("GameObject/Khoai/Copy Path", true)]
        private static bool LogSelectedPathValidate() => Selection.activeGameObject != null;


        [MenuItem("GameObject/Khoai/Add Themed Components")]
        public static void AddThemedComponents()
        {
            if (Selection.activeGameObject == null) return;

            Undo.IncrementCurrentGroup();
            var undoGroup = Undo.GetCurrentGroup();
            AddThemedComponents(Selection.activeGameObject.transform);
            Undo.CollapseUndoOperations(undoGroup);
        }

        private static void AddThemedComponents(Transform tf)
        {
            foreach (Transform child in tf)
            {
                AddThemedComponents(child);
            }

            if (tf.TryGetComponent<KThemedItem>(out var _)) return;
            if (tf.TryGetComponent<Text>(out var _))
            {
                Undo.AddComponent<KThemedText>(tf.gameObject);
                return;
            }
            else if (tf.TryGetComponent<TextMeshProUGUI>(out var _))
            {
                Undo.AddComponent<KThemedTextMeshProUGUI>(tf.gameObject);
                return;
            }
            else if (tf.TryGetComponent<Image>(out var _))
            {
                Undo.AddComponent<KThemedImage>(tf.gameObject);
                return;
            }
        }
    }
}
