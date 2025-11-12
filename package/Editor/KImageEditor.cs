using UnityEditor;
using UnityEditor.UI;

namespace Khoai.Editor
{
    [CustomEditor(typeof(KImage))]
    [CanEditMultipleObjects]
    public class KImageEditor : ImageEditor
    {
        SerializedProperty _paletteProp;
        SerializedProperty _selectedColorProp;

        protected override void OnEnable()
        {
            base.OnEnable();
            _paletteProp = serializedObject.FindProperty("palette");
            _selectedColorProp = serializedObject.FindProperty("selectedColor");
            if (_paletteProp.objectReferenceValue != null) return;

            foreach (var obj in serializedObject.targetObjects)
            {
                var kimage = obj as KImage;
                if (kimage == null) continue;
                _paletteProp.objectReferenceValue = KTheme.Instance.colorPalette;
            serializedObject.ApplyModifiedProperties();
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(_paletteProp);
            EditorGUILayout.PropertyField(_selectedColorProp);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
