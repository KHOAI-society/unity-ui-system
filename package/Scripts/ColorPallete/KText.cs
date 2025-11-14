
using UnityEngine;
using UnityEngine.UI;

namespace Khoai
{
    [RequireComponent(typeof(Text))]
    public class KText : KThemedItem, KIColoredItem
    {
        [KColorSelection]
        [SerializeField]
        KThemedItemProperty mainColor;

        public override void SyncColor(KColorPalette colorPalette)
        {
            base.SyncColor(colorPalette);
            if (!palette) return;
            if(!mainColor.use) return;
            GetComponent<Text>().color = GetColor(mainColor.colorName);
        }

#if UNITY_EDITOR
        protected void OnValidate()
        {
            SyncColor();
        }
#endif
    }
}