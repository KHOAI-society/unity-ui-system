using System;

namespace Khoai
{
    [Serializable]
    public class KThemedItemProperty
    {
        public bool use;
        public string colorName;
    }    

    public class KThemedItem : KMonoBehaviour
    {
        public static readonly UnityEngine.Color errorColor = new(1f, 0f, 1f);
        [KReadOnly] public KColorPalette palette;

        protected override void Start()
        {
            base.Start();
            SyncColor(KTheme.Instance.colorPalette);
        }

        public virtual void SyncColor()
        {
            SyncColor(palette);
        }

        public virtual void SyncColor(KColorPalette colorPalette)
        {
            palette = colorPalette;
        }

        protected UnityEngine.Color GetColor(string colorName)
        {
            try
            {
                return palette.colorsList[colorName];
            }
            catch(Exception ex)
            {
                UnityEngine.Debug.LogException(ex);
                return errorColor;
            }
        }
    }
}