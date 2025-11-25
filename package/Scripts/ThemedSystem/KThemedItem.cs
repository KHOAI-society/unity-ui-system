using System;
using UnityEngine;

namespace Khoai
{
    [Serializable]
    public class KThemedItemProperty
    {
        public bool use = false;
        public string itemName = "";
    }

    public class KThemedItem : KMonoBehaviour
    {
        public static readonly Color errorColor = new(1f, 0f, 1f);
        [KReadOnly] public KColorPalette colorPalette;
        [KReadOnly] public KSpritePalette spritePalette;

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
        public virtual void SyncSprite()
        {
            SyncSprite(spritePalette);
        }

        public virtual void SyncSprite(KSpritePalette palette)
        {
            spritePalette = palette;
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

        protected Sprite GetSprite(string spriteName)
        {
            try
            {
                return spritePalette.SpriteMap[spriteName];
            }
            catch(Exception ex)
            {
                Debug.LogException(ex);
                return null;
            }
        }
    }
}