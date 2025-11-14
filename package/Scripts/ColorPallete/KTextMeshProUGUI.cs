
using TMPro;
using UnityEngine;

namespace Khoai
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class KTextMeshProUGUI : KThemedItem, KIColoredUI
    {
        [KColorSelection]
        [SerializeField]
        KThemedItemProperty mainColor;

        public override void SyncColor(KColorPalette colorPalette)
        {
            base.SyncColor(colorPalette);
            if (!palette) return;
            if(!mainColor.use) return;
            GetComponent<TextMeshProUGUI>().color = GetColor(mainColor.colorName);
        }

#if UNITY_EDITOR
        protected void OnValidate()
        {
            SyncColor();
        }
#endif
    }
}