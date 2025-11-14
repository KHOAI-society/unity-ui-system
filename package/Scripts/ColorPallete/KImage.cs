
using UnityEngine;
using UnityEngine.UI;

namespace Khoai
{
    [RequireComponent(typeof(Image))]
    public class KImage : KThemedItem, KIThemedItem
    {
        [KColorSelection]
        [SerializeField]
        KThemedItemProperty mainColor;

        public override void SyncColor(KColorPalette colorPalette)
        {
            base.SyncColor(colorPalette);
            if (!palette) return;
            if(!mainColor.use) return;
            GetComponent<Image>().color = GetColor(mainColor.colorName);
        }

#if UNITY_EDITOR
        protected void OnValidate()
        {
            SyncColor();
        }
#endif
    }
}