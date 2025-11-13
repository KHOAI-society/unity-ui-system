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
                var kimage = obj as KImage;
                if (kimage == null) continue;
                kimage.SyncColor(KTheme.Instance.colorPalette);
            }
        }
    }
}