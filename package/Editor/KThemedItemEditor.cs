using UnityEditor;

namespace Khoai.Editors
{
    [CustomEditor(typeof(KThemedItem), true)]
    [CanEditMultipleObjects]
    public class KThemedItemEditor : Editor
    {
        protected void OnEnable()
        {
            foreach (var obj in serializedObject.targetObjects)
            {
                (obj as KIColorAppliedItem)?.SyncColor(KTheme.Instance.colorPalette);
                (obj as KISpriteAppliedItem)?.SyncSprite(KTheme.Instance.spritePalette);
            }
        }
    }
}