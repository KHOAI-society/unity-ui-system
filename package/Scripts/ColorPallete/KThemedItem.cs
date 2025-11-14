using System;
using UnityEngine;

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
        public static readonly Color errorColor = new(1f, 0f, 1f);
        [KReadOnly] public KColorPalette colorPalette;
        [KReadOnly] public KTexture2DPallete texture2Dpallete;

        protected override void Start()
        {
            base.Start();
            SyncColor(KTheme.Instance.colorPalette);
        }

        public virtual void SyncColor()
        {
            SyncColor(colorPalette);
        }

        public virtual void SyncColor(KColorPalette palette)
        {
            colorPalette = palette;
        }
        public virtual void SyncTexture2D()
        {
            SyncTexture2D(texture2Dpallete);
        }

        public virtual void SyncTexture2D(KTexture2DPallete pallete)
        {
            texture2Dpallete = pallete;
        }

        protected Color GetColor(string colorName)
        {
            try
            {
                return colorPalette.ColorMap[colorName];
            }
            catch(Exception ex)
            {
                Debug.LogException(ex);
                return errorColor;
            }
        }

        protected Texture2D GetTexture2D(string texture2DName)
        {
            try
            {
                return texture2Dpallete.Texture2DMap[texture2DName];
            }
            catch(Exception ex)
            {
                Debug.LogException(ex);
                return null;
            }
        }
    }
}