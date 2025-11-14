
using UnityEngine;
using UnityEngine.UI;

namespace Khoai
{
    [RequireComponent(typeof(Image))]
    public class KImage : KThemedItem, KIColorAppliedItem, KISpriteAppliedItem
    {
        [KColorSelection]
        [SerializeField]
        KThemedItemProperty mainColor;

        [KSpriteSelection]
        [SerializeField]
        KThemedItemProperty mainSprite;

        public override void SyncColor(KColorPalette colorPalette)
        {
            base.SyncColor(colorPalette);
            if (!colorPalette) return;
            if(!mainColor.use) return;
            GetComponent<Image>().color = GetColor(mainColor.itemName);
        }

        public override void SyncSprite(KSpritePalette palette)
        {
            base.SyncSprite(palette);
            if(!spritePalette) return;
            GetComponent<Image>().sprite = GetSprite(mainSprite.itemName);
        }

#if UNITY_EDITOR
        protected void OnValidate()
        {
            SyncColor();
        }
#endif
    }
}