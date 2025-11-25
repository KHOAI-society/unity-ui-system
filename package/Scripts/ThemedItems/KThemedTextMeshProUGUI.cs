
using TMPro;
using UnityEngine;

namespace Khoai
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class KThemedTextMeshProUGUI : KThemedItem, KIColorAppliedItem
    {
        [KColorSelection]
        [SerializeField]
        KThemedItemProperty mainColor;

        public override void SyncColor(KColorPalette colorPalette)
        {
            base.SyncColor(colorPalette);
            if (!base.colorPalette) return;
            if(!mainColor.use) return;
            GetComponent<TextMeshProUGUI>().color = GetColor(mainColor.itemName);
        }

#if UNITY_EDITOR
        protected void OnValidate()
        {
            SyncColor();
        }
#endif
    }
}