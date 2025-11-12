using UnityEditor;
using UnityEngine;

namespace Khoai.Editors
{
    [CustomPropertyDrawer(typeof(KColorList))]      // draw for int fields
    public class KColorListDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {

            EditorGUI.EndProperty();
        }
    }
}